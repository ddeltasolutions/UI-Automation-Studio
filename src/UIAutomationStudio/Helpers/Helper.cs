using System;
using System.Windows;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UIAutomationClient;

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
				MessageBox.Show(MainWindow.Instance, message);
			}
			else
			{
				MessageBox.Show(message);
			}
		}
	}
	
	internal class HighlightHelper
	{
		private static System.Windows.Forms.Form frmHighlightTop = null;
		private static System.Windows.Forms.Form frmHighlightLeft = null;
		private static System.Windows.Forms.Form frmHighlightBottom = null;
		private static System.Windows.Forms.Form frmHighlightRight = null;
		private static int width = 0;
        private static int height = 0;
		private static int thickness = 4;
		
		public static bool guard = false;
		
		public static void HighlightElement(IUIAutomationElement element)
		{
			if (element == null)
			{
				return;
			}
			
			tagRECT rect = new tagRECT {left=0, top=0, right=0, bottom=0};

            try
            {
                rect = element.CurrentBoundingRectangle;
            }
            catch
            {
				UnHighlight();
                return;
            }
			
			if (rect.right <= 0 && rect.bottom <= 0) // infinity
            {
				UnHighlight();
                return;
            }
			
			int left = 0;
            int top = 0;
			bool firstHighlight = false;
			
			left = rect.left - thickness;
			top = rect.top - thickness;
			width = rect.right - rect.left;
			height = rect.bottom - rect.top;
			
			if (frmHighlightTop == null)
			{
				frmHighlightTop = new System.Windows.Forms.Form();
                frmHighlightTop.Text = "AutomationSpy_rect_top";
				
				InitBorderWindow(ref frmHighlightTop);
				
				frmHighlightTop.Location = new System.Drawing.Point(left, top);
				
				frmHighlightTop.Load += highlightTop_Loaded;
				frmHighlightTop.Closed += highlightTop_Closed;
				frmHighlightTop.Show();
				
				firstHighlight = true;
			}
			else
			{
				frmHighlightTop.Location = new System.Drawing.Point(left, top);
				frmHighlightTop.Size = new System.Drawing.Size(width + 2 * thickness, thickness);
			}
            
			if (frmHighlightLeft == null)
			{
				frmHighlightLeft = new System.Windows.Forms.Form();
                frmHighlightLeft.Text = "AutomationSpy_rect_left";
				
				InitBorderWindow(ref frmHighlightLeft);
				
				frmHighlightLeft.Location = new System.Drawing.Point(left, top);
				
				frmHighlightLeft.Load += highlightLeft_Loaded;
				frmHighlightLeft.Closed += highlightLeft_Closed;
				frmHighlightLeft.Show();
				
				firstHighlight = true;
			}
			else
			{
				frmHighlightLeft.Location = new System.Drawing.Point(left, top);
				frmHighlightLeft.Size = new System.Drawing.Size(thickness, height + 2 * thickness);
			}
			
			if (frmHighlightBottom == null)
			{
				frmHighlightBottom = new System.Windows.Forms.Form();
                frmHighlightBottom.Text = "AutomationSpy_rect_bottom";
				
				InitBorderWindow(ref frmHighlightBottom);
				
				frmHighlightBottom.Location = new System.Drawing.Point(left, top + height + thickness);
				
				frmHighlightBottom.Load += highlightBottom_Loaded;
				frmHighlightBottom.Closed += highlightBottom_Closed;
				frmHighlightBottom.Show();
				
				firstHighlight = true;
			}
			else
			{
				frmHighlightBottom.Location = new System.Drawing.Point(left, top + height + thickness);
				frmHighlightBottom.Size = new System.Drawing.Size(width + 2 * thickness, thickness);
			}
			
			if (frmHighlightRight == null)
			{
				frmHighlightRight = new System.Windows.Forms.Form();
                frmHighlightRight.Text = "AutomationSpy_rect_right";
				
				InitBorderWindow(ref frmHighlightRight);
				
				frmHighlightRight.Location = new System.Drawing.Point(left + thickness + width, top);
				
				frmHighlightRight.Load += highlightRight_Loaded;
				frmHighlightRight.Closed += highlightRight_Closed;
				frmHighlightRight.Show();
				
				firstHighlight = true;
			}
			else
			{
				frmHighlightRight.Location = new System.Drawing.Point(left + thickness + width, top);
				frmHighlightRight.Size = new System.Drawing.Size(thickness, height + 2 * thickness);
			}

			if (firstHighlight == true)
			{
				if (UserControlPickElement.Instance != null)
				{
					UserControlPickElement.Instance.Focus();
					Window wnd = Window.GetWindow(UserControlPickElement.Instance);

					if (wnd != null)
					{
						wnd.Focus();
					}
				}
			}
		}
		
		private static void InitBorderWindow(ref System.Windows.Forms.Form form)
		{
			form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			form.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			form.MinimumSize = new System.Drawing.Size(0, 0);
			form.Size = new System.Drawing.Size(0, 0);
			form.ShowInTaskbar = false;
			form.TopMost = true;
			form.ForeColor = System.Drawing.Color.Red;
			form.BackColor = System.Drawing.Color.Red;
		}
		
		private static void highlightTop_Loaded(object sender, System.EventArgs e)
		{
			frmHighlightTop.Size = new System.Drawing.Size(width + 2 * thickness, thickness);
		}
		private static void highlightTop_Closed(object sender, System.EventArgs e)
		{
			if (guard == true)
			{
				return;
			}
			frmHighlightTop = null;
			if (UserControlPickElement.Instance != null)
			{
				UserControlPickElement.Instance.VerifyControls();
				Window.GetWindow(UserControlPickElement.Instance).Close();
			}
		}
		
		private static void highlightLeft_Loaded(object sender, System.EventArgs e)
		{
			frmHighlightLeft.Size = new System.Drawing.Size(thickness, height + 2 * thickness);
		}
		private static void highlightLeft_Closed(object sender, System.EventArgs e)
		{
			if (guard == true)
			{
				return;
			}
			frmHighlightLeft = null;
			if (UserControlPickElement.Instance != null)
			{
				UserControlPickElement.Instance.VerifyControls();
				Window.GetWindow(UserControlPickElement.Instance).Close();
			}
		}
		
		private static void highlightBottom_Loaded(object sender, System.EventArgs e)
		{
			frmHighlightBottom.Size = new System.Drawing.Size(width + 2 * thickness, thickness);
		}
		private static void highlightBottom_Closed(object sender, System.EventArgs e)
		{
			if (guard == true)
			{
				return;
			}
			frmHighlightBottom = null;
			if (UserControlPickElement.Instance != null)
			{
				UserControlPickElement.Instance.VerifyControls();
				Window.GetWindow(UserControlPickElement.Instance).Close();
			}
		}
		
		private static void highlightRight_Loaded(object sender, System.EventArgs e)
		{
			frmHighlightRight.Size = new System.Drawing.Size(thickness, height + 2 * thickness);
		}
		private static void highlightRight_Closed(object sender, System.EventArgs e)
		{
			if (guard == true)
			{
				return;
			}
			frmHighlightRight = null;
			if (UserControlPickElement.Instance != null)
			{
				UserControlPickElement.Instance.VerifyControls();
				Window.GetWindow(UserControlPickElement.Instance).Close();
			}
		}
		
		public static void UnHighlight()
		{
			guard = true;
			bool firstHighlight = false;
			
			if (frmHighlightTop != null)
			{
				frmHighlightTop.Close();
				frmHighlightTop = null;
			}
			
			if (frmHighlightLeft != null)
			{
				frmHighlightLeft.Close();
				frmHighlightLeft = null;
			}
			
			if (frmHighlightBottom != null)
			{
				frmHighlightBottom.Close();
				frmHighlightBottom = null;
			}
			
			if (frmHighlightRight != null)
			{
				frmHighlightRight.Close();
				frmHighlightRight = null;
			}
			
			if (firstHighlight == true)
			{
				if (UserControlPickElement.Instance != null)
				{
					UserControlPickElement.Instance.Focus();
				}
			}
			guard = false;
		}
		
		public static void CloseHighlightFrames()
		{
			if (frmHighlightTop != null)
			{
				frmHighlightTop.Close();
				frmHighlightTop = null;
			}
			if (frmHighlightLeft != null)
			{
				frmHighlightLeft.Close();
				frmHighlightLeft = null;
			}
			if (frmHighlightBottom != null)
			{
				frmHighlightBottom.Close();
				frmHighlightBottom = null;
			}
			if (frmHighlightRight != null)
			{
				frmHighlightRight.Close();
				frmHighlightRight = null;
			}
		}
	}
}