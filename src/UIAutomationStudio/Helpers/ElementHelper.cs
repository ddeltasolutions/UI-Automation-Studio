using System;
using System.Windows;
using UIDeskAutomationLib;
using UIAutomationClient;

namespace UIAutomationStudio
{
	public class ElementHelper
	{
		private static string TOPLEVEL_NOT_FOUND = "Top Level Window not found";
		private static string TOPLEVEL_WITH_NAME_NOT_FOUND = "Top Level Window \"{0}\" not found";
	
		public static IUIAutomationElement GetTopLevelElement(IUIAutomationElement element)
		{
			IUIAutomationElement parent = MainWindow.uiAutomation.ControlViewWalker.GetParentElement(element);
			if (parent == null)
			{
				return null;
			}
			
			IUIAutomationElement grandParent = MainWindow.uiAutomation.ControlViewWalker.GetParentElement(parent);
			IUIAutomationElement crtElement = element;
			
			while (grandParent != null)
			{
				crtElement = parent;
				parent = grandParent;
				grandParent = MainWindow.uiAutomation.ControlViewWalker.GetParentElement(grandParent);
			}
			
			return crtElement;
		}
	
		public static ElementBase GetTopLevelElement(Element element, bool supressMsg = false)
		{
			Engine engine = LibraryEngine.GetEngine();
			ElementBase libraryElement = null;
			string wildcardsName = element.GetWildcardsName(false);
			
			if (element.ControlType == ControlType.Pane && element.Name != null)
			{
				try
				{
					libraryElement = engine.GetDesktopPane().Pane(wildcardsName);
				}
				catch (Exception ex)
				{
					if (Task.IsAttemptingPause == false && supressMsg == false)
					{
						string message = string.IsNullOrEmpty(wildcardsName) ? TOPLEVEL_NOT_FOUND : string.Format(TOPLEVEL_WITH_NAME_NOT_FOUND, wildcardsName);
						Helper.ShowMessageBoxOnMainThread(message);
					}
					return null;
				}
				
				if (libraryElement != null)
				{
					return libraryElement;
				}
			}
			
			if (string.IsNullOrEmpty(wildcardsName))
			{
				try
				{
					libraryElement = engine.GetTopLevel("", element.ClassName);
				}
				catch (Exception ex)
				{
					if (Task.IsAttemptingPause == false && supressMsg == false)
					{
						Helper.ShowMessageBoxOnMainThread(TOPLEVEL_NOT_FOUND);
					}
					return null;
				}
			}
			else
			{
				try
				{
					if (element.FrameworkId == "WPF" || element.ClassName == null)
					{
						libraryElement = engine.GetTopLevel(wildcardsName);
					}
					else
					{
						libraryElement = engine.GetTopLevel(wildcardsName, element.ClassName);
					}
				}
				catch (Exception ex)
				{
					if (Task.IsAttemptingPause == false && supressMsg == false)
					{
						string message = string.Format(TOPLEVEL_WITH_NAME_NOT_FOUND, wildcardsName);
						Helper.ShowMessageBoxOnMainThread(message);
					}
					return null;
				}
			}
			
			return libraryElement;
		}
		
		public static ElementBase GetNextElement(ElementBase libraryElement, Element element, 
			bool searchDescendants, bool supressMsg = false)
		{
			Engine engine = LibraryEngine.GetEngine();
			ElementBase returnElement = null;
			
			int lowerTimeOut = 6000;
			int initialTimeOut = engine.Timeout;
			if (initialTimeOut > lowerTimeOut)
			{
				engine.Timeout = lowerTimeOut; // lower the timeout at 6 seconds for non-toplevel elements
			}
			else
			{
				engine.Timeout = 500;
			}
			
			try
			{
				returnElement = TryGetNextElement(libraryElement, element, searchDescendants);
			}
			catch (Exception ex)
			{
				if (Task.IsAttemptingPause == false && supressMsg == false)
				{
					string message = element.ControlType.ToString();
					string wildcardsName = element.GetWildcardsName(false);
					if (string.IsNullOrEmpty(wildcardsName))
					{
						message += " not found";
					}
					else
					{
						message += " \"" + wildcardsName + "\" not found";
					}
				
					Helper.ShowMessageBoxOnMainThread(message);
				}
				
				return null;
			}
			finally
			{
				//if (initialTimeOut > lowerTimeOut)
				//{
					engine.Timeout = initialTimeOut;
				//}
			}
			
			return returnElement;
		}
		
		private static ElementBase TryGetNextElement(ElementBase libraryElement, Element element, 
			bool searchDescendants)
		{
			string elementName = element.GetWildcardsName(false);
		
			if (element.ControlType == ControlType.Button)
			{
				if (element.Index != 0)
				{
					return libraryElement.ButtonAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Button(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Calendar)
			{
				if (element.Index != 0)
				{
					return libraryElement.CalendarAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Calendar(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.CheckBox)
			{
				if (element.Index != 0)
				{
					return libraryElement.CheckBoxAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.CheckBox(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.ComboBox)
			{
				if (element.Index != 0)
				{
					return libraryElement.ComboBoxAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.ComboBox(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.DatePicker)
			{
				if (element.Index != 0)
				{
					return libraryElement.DatePickerAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.DatePicker(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Edit)
			{
				if (element.Index != 0)
				{
					return libraryElement.EditAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Edit(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Hyperlink)
			{
				if (element.Index != 0)
				{
					return libraryElement.HyperlinkAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Hyperlink(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Image)
			{
				if (element.Index != 0)
				{
					return libraryElement.ImageAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Image(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.ListItem)
			{
				if (element.Index != 0)
				{
					return libraryElement.ListItemAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.ListItem(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.List)
			{
				if (element.Index != 0)
				{
					return libraryElement.ListAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.List(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Menu)
			{
				if (element.Index != 0)
				{
					return libraryElement.MenuAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Menu(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.MenuBar)
			{
				if (element.Index != 0)
				{
					return libraryElement.MenuBarAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.MenuBar(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.MenuItem)
			{
				if (element.Index != 0)
				{
					return libraryElement.MenuItemAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.MenuItem(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.ProgressBar)
			{
				if (element.Index != 0)
				{
					return libraryElement.ProgressBarAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.ProgressBar(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.RadioButton)
			{
				if (element.Index != 0)
				{
					return libraryElement.RadioButtonAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.RadioButton(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.ScrollBar)
			{
				if (element.Index != 0)
				{
					return libraryElement.ScrollBarAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.ScrollBar(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Slider)
			{
				if (element.Index != 0)
				{
					return libraryElement.SliderAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Slider(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Spinner)
			{
				if (element.Index != 0)
				{
					return libraryElement.SpinnerAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Spinner(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.StatusBar)
			{
				if (element.Index != 0)
				{
					return libraryElement.StatusBarAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.StatusBar(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Tab)
			{
				if (element.Index != 0)
				{
					return libraryElement.TabCtrlAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.TabCtrl(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.TabItem)
			{
				UIDA_TabCtrl tabCtrl = libraryElement as UIDA_TabCtrl;
				if (tabCtrl != null)
				{
					if (element.Index != 0)
					{
						return tabCtrl.TabItemAt(elementName, element.Index, searchDescendants);
					}
					else
					{
						return tabCtrl.TabItem(elementName, searchDescendants);
					}
				}
			}
			else if (element.ControlType == ControlType.Text)
			{
				if (element.Index != 0)
				{
					return libraryElement.LabelAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Label(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.ToolBar)
			{
				if (element.Index != 0)
				{
					return libraryElement.ToolBarAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.ToolBar(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.ToolTip)
			{
				if (element.Index != 0)
				{
					return libraryElement.ToolTipAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.ToolTip(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Tree)
			{
				if (element.Index != 0)
				{
					return libraryElement.TreeAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Tree(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.TreeItem)
			{
				if (element.Index != 0)
				{
					return libraryElement.TreeItemAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.TreeItem(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Custom)
			{
				if (element.Index != 0)
				{
					return libraryElement.CustomAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Custom(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Group)
			{
				if (element.Index != 0)
				{
					return libraryElement.GroupAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Group(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Thumb)
			{
				if (element.Index != 0)
				{
					return libraryElement.ThumbAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Thumb(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.DataGrid)
			{
				if (element.Index != 0)
				{
					return libraryElement.DataGridAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.DataGrid(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.DataItem)
			{
				UIDA_DataGrid dataGrid = libraryElement as UIDA_DataGrid;
				if (dataGrid != null)
				{
					if (element.Index != 0)
					{
						return dataGrid.DataItemAt(elementName, element.Index, searchDescendants);
					}
					else
					{
						return dataGrid.DataItem(elementName, searchDescendants);
					}
				}
			}
			else if (element.ControlType == ControlType.Document)
			{
				if (element.Index != 0)
				{
					return libraryElement.DocumentAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Document(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.SplitButton)
			{
				if (element.Index != 0)
				{
					return libraryElement.SplitButtonAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.SplitButton(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Window)
			{
				if (element.Index != 0)
				{
					return libraryElement.WindowAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Window(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Pane)
			{
				if (element.Index != 0)
				{
					return libraryElement.PaneAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Pane(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Header)
			{
				UIDA_Table.Row tableRow = libraryElement as UIDA_Table.Row;
				if (tableRow != null)
				{
					if (element.Index != 0)
					{
						return tableRow.HeaderAt(elementName, element.Index, searchDescendants);
					}
					else
					{
						return tableRow.Header(elementName, searchDescendants);
					}
				}
			}
			else if (element.ControlType == ControlType.HeaderItem)
			{
				if (element.Index != 0)
				{
					return libraryElement.HeaderItemAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.HeaderItem(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Table)
			{
				if (element.Index != 0)
				{
					return libraryElement.TableAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Table(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.TitleBar)
			{
				if (element.Index != 0)
				{
					return libraryElement.TitleBarAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.TitleBar(elementName, searchDescendants);
				}
			}
			else if (element.ControlType == ControlType.Separator)
			{
				if (element.Index != 0)
				{
					return libraryElement.SeparatorAt(elementName, element.Index, searchDescendants);
				}
				else
				{
					return libraryElement.Separator(elementName, searchDescendants);
				}
			}
			
			return null;
		}
	}
}