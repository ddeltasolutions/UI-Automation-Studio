using System;
using UIDeskAutomationLib;
using UIAutomationClient;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Windows;

namespace UIAutomationStudio
{
	public class Element
	{
		public IUIAutomationElement InnerElement { get; set; }
		public string Name { get; set; }
		public ControlType ControlType { get; set; }
		public Element Parent { get; set; }
		public int Index { get; set; }
		public string ClassName { get; set; }
		public string FrameworkId { get; set; }
		public bool WasNameTruncated { get; set; }
		
		public Element()
		{
			this.InnerElement = null;
			this.Name = null;
			this.ControlType = ControlType.None;
			this.Parent = null;
			this.Index = 0;
			this.ClassName = null;
			this.FrameworkId = null;
			this.WasNameTruncated = false;
		}
		
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
		
		public Element(IUIAutomationElement element, bool isParent = false)
		{
			this.WasNameTruncated = false;
			
			this.InnerElement = element;
			if (element == null)
			{
				// This should never happen!!
				System.Windows.MessageBox.Show(MainWindow.Instance, "null element");
				return;
			}
			
			// Handle Name ****************
			string name = element.CurrentName;
			if (name != null)
			{
				if (name.Length > 100)
				{
					this.Name = name.Substring(0, 100);
					this.WasNameTruncated = true;
				}
				else
				{
					this.Name = name;
				}
				
				int idx = this.Name.IndexOf("\r"); // get the first line
				if (idx < 0)
				{
					idx = this.Name.IndexOf("\n");
				}
				if (idx >= 0)
				{
					this.Name = this.Name.Substring(0, idx);
					this.WasNameTruncated = true;
				}
			}
			else
			{
				this.Name = null;
			}
			// ******************************
			
			// Handle ControlType ****************
			string className = element.CurrentClassName;
			string frameworkId = element.CurrentFrameworkId;
			int type = element.CurrentControlType;
			ControlType controlType = (ControlType)type;
		
			bool isWin32Calendar = false;
			
			if ((controlType == ControlType.Custom && className.StartsWith("DatePicker") && frameworkId == "WPF") ||
			(controlType == ControlType.Pane && className.StartsWith("SysDateTimePick32") && frameworkId == "Win32") ||
			(controlType == ControlType.Pane && className.StartsWith("WindowsForms10.SysDateTimePick32") && frameworkId == "WinForm"))
			{
				this.ControlType = ControlType.DatePicker;
				this.Name = null; // don't take name into account because it can change
			}
			else if (controlType == ControlType.Pane && className == "SysMonthCal32")
			{
				this.ControlType = ControlType.Calendar;
				isWin32Calendar = true;
			}
			else
			{
				this.ControlType = controlType;
			}
			// ***********************************
			
			// get Class Name ***********
			IUIAutomationElement parent = MainWindow.uiAutomation.ControlViewWalker.GetParentElement(element);
			bool isTopLevel = false;
			
			IUIAutomationElement grandParent = null;
			if (parent != null)
			{
				grandParent = MainWindow.uiAutomation.ControlViewWalker.GetParentElement(parent);
			}
			
			if (grandParent == null && parent != null)
			{
				isTopLevel = true;
				
				StringBuilder sbClassName = new StringBuilder(256);
				GetClassName(element.CurrentNativeWindowHandle, sbClassName, 256);
				this.ClassName = sbClassName.ToString();
				
				this.FrameworkId = frameworkId;
			}
			// **************************
			
			// Handle Parent ****************
			if (parent != null)
			{
				this.Parent = new Element(parent, true);
			}
			else
			{
				this.Parent = null;
				return;
			}
			// ******************************
			
			// Handle Index ****************
			this.Index = 0;
			
			string text = this.Name;
			if (this.WasNameTruncated)
			{
				text += "*";
			}
			
			IUIAutomationCondition condition = MainWindow.uiAutomation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, type);
			IUIAutomationElementArray automationElementCollection = null;
			if (isTopLevel == true)
			{
				automationElementCollection = parent.FindAll(TreeScope.TreeScope_Children, condition);
			}
			else
			{
				if (isParent == true)
				{
					automationElementCollection = parent.FindAll(TreeScope.TreeScope_Children, condition);
				}
				else
				{
					if (text == null || text == "")
					{
						automationElementCollection = parent.FindAll(TreeScope.TreeScope_Children, condition);
					}
					else
					{
						IUIAutomationElement topLevelEl = null;
						try
						{
							topLevelEl = ElementHelper.GetTopLevelElement(element);
						}
						catch { }
						
						if (topLevelEl != null)
						{
							automationElementCollection = topLevelEl.FindAll(TreeScope.TreeScope_Descendants, condition);
						}
						else
						{
							automationElementCollection = parent.FindAll(TreeScope.TreeScope_Descendants, condition);
						}
					}
				}
			}
			
            if (automationElementCollection == null)
            {
                return;
            }
			
			List<IUIAutomationElement> list = Helper.MatchStrings(automationElementCollection, text, true);
			if (list.Count <= 1)
			{
				// it's only our element
				return;
			}
			
			int index = 0;
            foreach (IUIAutomationElement item in list)
            {
                if (isWin32Calendar)
                {
                    if (item.CurrentClassName != "SysMonthCal32")
                    {
                        continue;
                    }
                }
                else if (this.ControlType == ControlType.DatePicker)
                {
                    if (frameworkId == "WPF" && item.CurrentClassName != "DatePicker")
                    {
                        continue;
                    }
                    else if (frameworkId == "WinForm" && !item.CurrentClassName.StartsWith("WindowsForms10.SysDateTimePick32"))
                    {
                        continue;
                    }
                    else if (frameworkId == "Win32" && item.CurrentClassName != "SysDateTimePick32")
                    {
                        continue;
                    }
                }
                
                index++;
                
                if (Helper.CompareElements(item, element))
                {
                    break;
                }
            }
			
			if (index >= 2)
			{
				this.Index = index;
			}
			// *****************************
		}
		
		public string GetWildcardsName(bool doubleBackslash = true)
		{
			string wildcardsName = this.Name;
			if (this.WasNameTruncated)
			{
				wildcardsName += "*";
			}
			
			if (doubleBackslash == true && wildcardsName != null)
			{
				wildcardsName = wildcardsName.Replace("\\", "\\\\");
			}
			return wildcardsName;
		}
		
		public string ControlTypeName
		{
			get
			{
				if (this.ControlType == ControlType.Tab)
				{
					return "TabCtrl";
				}
				else
				{
					return this.ControlType.ToString();
				}
			}
		}
		
		public string GetShortName(int maxCharsCount)
		{
			string text = this.Name;
			if (text == null)
			{
				return "";
			}
			
			if (text.Length > maxCharsCount)
			{
				text = text.Substring(0, maxCharsCount);
				text += "...";
			}
			return "\"" + text + "\"";
		}
		
		public string GetName()
		{
			if (this.Name == null)
			{
				return null;
			}
			
			if (this.WasNameTruncated == true)
			{
				return this.Name + "...";
			}
			else
			{
				return this.Name;
			}
		}
		
		public static void DeepCopy(Element source, Element destination)
		{
			destination.InnerElement = source.InnerElement;
			destination.Name = source.Name;
			destination.ControlType = source.ControlType;
			destination.Index = source.Index;
			destination.ClassName = source.ClassName;
			destination.FrameworkId = source.FrameworkId;
			destination.WasNameTruncated = source.WasNameTruncated;
			
			if (source.Parent == null)
			{
				destination.Parent = null;
			}
			else
			{
				if (destination.Parent == null)
				{
					destination.Parent = new Element();
				}
				DeepCopy(source.Parent, destination.Parent);
			}
		}
		
		public override string ToString()
		{
			return this.ControlTypeName + " (" + GetShortName(20) + ")";
		}
		
		public ElementBase GetLibraryElement(bool noTimeOut = false)
		{
			Engine engine = new Engine();
			
			int timeOut = 0;
			if (noTimeOut == true)
			{
				timeOut = engine.Timeout;
				engine.Timeout = 100;
			}
			
			try
			{
				return GetLibraryElement(engine);
			}
			catch
			{
				return null;
			}
			finally
			{
				if (noTimeOut == true)
				{
					engine.Timeout = timeOut;
				}
			}
		}

		private ElementBase GetLibraryElement(Engine engine)
		{
			engine.ThrowExceptionsForSearchFunctions = true;
			List<Element> ancestors = new List<Element>();
			
			Element parent = this.Parent;
			while (parent != null)
			{
				ancestors.Insert(0, parent);
				parent = parent.Parent;
			}
		
			ElementBase libraryElement = null;
			if (ancestors.Count > 0)
			{
				ancestors.RemoveAt(0); // remove desktop element
				
				if (ancestors.Count == 0)
				{
					// this element is a top level window
					libraryElement = ElementHelper.GetTopLevelElement(engine, this);
					if (libraryElement == null)
					{
						return null;
					}
				}
				else
				{
					// first get the top level window
					libraryElement = ElementHelper.GetTopLevelElement(engine, ancestors[0]);
					if (libraryElement == null)
					{
						return null;
					}
					
					bool isNameEmpty = (this.Name == null || this.Name == "");
					if (isNameEmpty == true)
					{
						for (int i = 1; i < ancestors.Count; i++)
						{
							Element ancestor = ancestors[i];
							try
							{
								libraryElement = ElementHelper.GetNextElement(libraryElement, ancestor, false);
							}
							catch (Exception ex)
							{
								MessageBox.Show(ex.Message);
								return null;
							}
						}
					}
					
					try
					{
						libraryElement = ElementHelper.GetNextElement(libraryElement, this, !isNameEmpty);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
						return null;
					}
				}
			}
			else
			{
				// this is desktop element
				try
				{
					libraryElement = engine.GetDesktopPane();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					return null;
				}
			}
			
			return libraryElement;
		}
	}
	
	public enum ControlType
	{
		None,
		Button = 50000,
		Calendar = 50001,
		CheckBox = 50002,
		ComboBox = 50003,
		DatePicker = 1,
		Edit = 50004,
		Hyperlink = 50005,
		Image = 50006,
		ListItem = 50007,
		List = 50008,
		Menu = 50009,
		MenuBar = 50010,
		MenuItem = 50011,
		ProgressBar = 50012,
		RadioButton = 50013,
		ScrollBar = 50014,
		Slider = 50015,
		Spinner = 50016,
		StatusBar = 50017,
		Tab = 50018,
		TabItem = 50019,
		Text = 50020,
		ToolBar = 50021,
		ToolTip = 50022,
		Tree = 50023,
		TreeItem = 50024,
		Custom = 50025,
		Group = 50026,
		Thumb = 50027,
		DataGrid = 50028,
		DataItem = 50029,
		Document = 50030,
		SplitButton = 50031,
		Window = 50032,
		Pane = 50033,
		Header = 50034,
		HeaderItem = 50035,
		Table = 50036,
		TitleBar = 50037,
		Separator = 50038,
		SemanticZoom = 50039,
		AppBar = 50040
	}
}