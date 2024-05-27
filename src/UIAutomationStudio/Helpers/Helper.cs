using System;
using System.Windows;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UIAutomationClient;
using UIDeskAutomationLib;

namespace UIAutomationStudio
{
	internal class Helper
	{
		private static int thickness = 5;
		private static TransparentWindow transparentWindow = null;
		
		private static void ToDIP(int pixelX, int pixelY, out int dipX, out int dipY)
		{
			using (System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
			{
				dipX = Convert.ToInt32(Convert.ToDouble(pixelX * 96) / g.DpiX);
				dipY = Convert.ToInt32(Convert.ToDouble(pixelY * 96) / g.DpiY);
			}
		}
		
		public static void Highlight(IUIAutomationElement element, Window window)
        {
            if (element == null)
            {
                return;
            }

            tagRECT rect = new tagRECT {left=0, top=0, right=0, bottom=0};
            bool rectObtained = false;

            try
            {
                rect = element.CurrentBoundingRectangle;
                rectObtained = true;
            }
            catch (Exception ex)
            {
                rectObtained = false;
            }

            if ((rectObtained == false) || (rect.right <= 0 && rect.bottom <= 0)) // infinity
            {
				UnHighlight();
                return;
            }
			
			int left = 0, top = 0, width = 0, height = 0;
			bool firstHighlight = false;
			
			if (transparentWindow == null)
			{
				transparentWindow = new TransparentWindow();
				transparentWindow.Owner = window;
				
				firstHighlight = true;
			}
			
			ToDIP(rect.left, rect.top, out left, out top);
			ToDIP( (rect.right - rect.left), (rect.bottom - rect.top), out width, out height);
			
			transparentWindow.Left = left;
			transparentWindow.Top = top;
			transparentWindow.Width = width;
			transparentWindow.Height = height;
			if (firstHighlight == true)
			{
				transparentWindow.Show();
			}
        }
		
		public static void UnHighlight()
        {
			if (transparentWindow != null)
			{
				transparentWindow.Close();
				transparentWindow = null;
			}
        }
		
		public static bool CompareElements(IUIAutomationElement element1, IUIAutomationElement element2)
		{
			if (element1 == null || element2 == null)
			{
				return false;
			}
			
			try
			{
				var runtimeId1 = element1.GetRuntimeId();
				var runtimeId2 = element2.GetRuntimeId();

				if (runtimeId1 == null || runtimeId2 == null)
				{
					return false;
				}
				
				int length1 = runtimeId1.Length;
				int length2 = runtimeId2.Length;
				
				if (length1 != length2)
				{
					return false;
				}
				for (int i = 0; i < length1; i++)
				{
					if ((int)runtimeId1.GetValue(i) != (int)runtimeId2.GetValue(i))
					{
						return false;
					}
				}
				
				return true;
			}
			catch
			{
				return false;
			}
		}
		
		private static string WildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
        }
		
		private static List<IUIAutomationElement> ArrayToList(IUIAutomationElementArray collection)
        {
            List<IUIAutomationElement> result = new List<IUIAutomationElement>();
			
            for (int i = 0; i < collection.Length; i++)
            {
                result.Add(collection.GetElement(i));
            }
            return result;
        }
		
		public static List<IUIAutomationElement> MatchStrings(IUIAutomationElementArray collection, 
			string name, bool caseSensitive)
        {
            if (name == null)
            {
                return ArrayToList(collection);
            }
            if (!caseSensitive)
            {
                name = name.ToLower();
            }
            string pattern = WildcardToRegex(name);
            Regex regex = new Regex(pattern);
            List<IUIAutomationElement> list = new List<IUIAutomationElement>();
            
            for (int i = 0; i < collection.Length; i++)
            {
                IUIAutomationElement item = collection.GetElement(i);
                string text = GetElementName(item);
                
                if (text != null)
                {
                    if (!caseSensitive)
                    {
                        text = text.ToLower();
                    }
                    Match match = null;
                    try
                    {
                        match = regex.Match(text);
                    }
                    catch
                    {
                        continue;
                    }
                    if (match.Success && match.Value == text)
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }
		
		public static bool MatchStrings(string text, string textWildcards)
		{
			string pattern = WildcardToRegex(textWildcards);
			Regex regex = new Regex(pattern);
			Match match = null;
			
			try
			{
				match = regex.Match(text);
			}
			catch
			{
				return false;
			}
			
			if (match.Success && match.Value == text)
			{
				return true;
			}
			
			return false;
		}
		
		private static string GetElementName(IUIAutomationElement element)
        {
            if (element == null)
            {
				return null;
            }
            
            int controlType = 0;
            try
            {
                controlType = element.CurrentControlType;
            }
            catch
            { }
            
            if ((controlType == UIA_ControlTypeIds.UIA_PaneControlTypeId && element.CurrentClassName == "SysDateTimePick32") ||
                (controlType == UIA_ControlTypeIds.UIA_PaneControlTypeId && element.CurrentClassName.StartsWith("WindowsForms10.SysDateTimePick32")) ||
                (controlType == UIA_ControlTypeIds.UIA_CustomControlTypeId && element.CurrentClassName == "DatePicker"))
            {
                return null;
            }
            
            string text = null;
            try
            {
                text = element.CurrentName;
            }
            catch { }
            
            return text;
        }
		
		public static string GetOrdinalAsString(int i)
		{
			if (i == 0)
			{
				return "First";
			}
			else if (i == 1)
			{
				return "Second";
			}
			else if (i == 2)
			{
				return "Third";
			}
			
			return (i + 1).ToString() + "th";
		}
		
		public static void MessageBoxShow(string message)
		{
			if (MainWindow.Instance != null)
			{
				MainWindow.Instance.Activate();
				MessageBox.Show(MainWindow.Instance, message);
			}
			else if (App.stopPauseWindow != null)
			{
				App.stopPauseWindow.Activate();
				//App.stopPauseWindow.Focus();
				MessageBox.Show(App.stopPauseWindow, message);
			}
			else
			{
				MessageBox.Show(message);
			}
		}
		
		public static void ShowMessageBoxOnMainThread(string message)
		{
			Application.Current.Dispatcher.Invoke( () =>
			{
				// Code to run on the GUI thread.
				MessageBoxShow(message);
			} );
		}
		
		public static bool HasAnEndAction(ActionBase action, ref Action lastAction)
		{
			ActionBase crt = action;
			Action crtAction = null;
			
			while (true)
			{
				if (crt is ConditionalAction)
				{
					return true;
				}
				else if (crt is EndAction)
				{
					return true;
				}
				
				crtAction = (Action)crt;
				if (crtAction.HasWeakBond == true)
				{
					return true;
				}
				
				if (crtAction.Next == null)
				{
					break;
				}
				
				crt = crtAction.Next;
			}
			
			lastAction = crtAction;
			return false;
		}
	}
	
	public static class LibraryEngine
	{
		private static Engine engine = null;
		
		public static Engine GetEngine()
		{
			if (engine == null)
			{
				engine = new Engine();
			}
			return engine;
		}
	}
}