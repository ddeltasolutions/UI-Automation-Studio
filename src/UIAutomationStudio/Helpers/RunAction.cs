using System;
using System.Windows;
using System.Collections.Generic;
using UIDeskAutomationLib;

namespace UIAutomationStudio
{
	public class RunAction
	{
		private Action action = null;
	
		public RunAction(Action action)
		{
			this.action = action;
		}
		
		public bool Run(bool noTimeOut = false)
		{
			Element element = action.Element;
			
			if (element == null)
			{
				Engine engine = new Engine();
				return CallGeneralAction(engine);
			}
			
			ElementBase libraryElement = element.GetLibraryElement(noTimeOut);
			if (libraryElement == null)
			{
				return false;
			}
			
			try
			{
				return CallAction(libraryElement);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return false;
			}
		}
		
		private bool CallGeneralAction(Engine engine)
		{
			if (this.action.ActionId == ActionIds.StartProcess)
			{
				if (this.action.Parameters.Count == 1)
				{
					engine.StartProcess((string)this.action.Parameters[0]);
					return true;
				}
				else if (this.action.Parameters.Count == 2)
				{
					engine.StartProcess((string)this.action.Parameters[0], (string)this.action.Parameters[1]);
					return true;
				}
			}
			else if (this.action.ActionId == ActionIds.StartProcessAndWaitForInputIdle)
			{
				if (this.action.Parameters.Count == 1)
				{
					engine.StartProcessAndWaitForInputIdle((string)this.action.Parameters[0]);
					return true;
				}
				else if (this.action.Parameters.Count == 2)
				{
					engine.StartProcessAndWaitForInputIdle((string)this.action.Parameters[0], (string)this.action.Parameters[1]);
					return true;
				}
			}
			else if (this.action.ActionId == ActionIds.Sleep && this.action.Parameters.Count >= 1)
			{
				engine.Sleep((int)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.ShowDesktop)
			{
				engine.ShowDesktop();
				return true;
			}
			else if (this.action.ActionId == ActionIds.ClickAt && this.action.Parameters.Count >= 2)
			{
				engine.ClickScreenCoordinatesAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.RightClickAt && this.action.Parameters.Count >= 2)
			{
				engine.RightClickScreenCoordinatesAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MiddleClickAt && this.action.Parameters.Count >= 2)
			{
				engine.MiddleClickScreenCoordinatesAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.DoubleClickAt && this.action.Parameters.Count >= 2)
			{
				engine.DoubleClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MoveMouse && this.action.Parameters.Count >= 2)
			{
				engine.MoveMouse((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MouseScrollUp && this.action.Parameters.Count >= 1)
			{
				engine.MouseScrollUp((uint)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MouseScrollDown && this.action.Parameters.Count >= 1)
			{
				engine.MouseScrollDown((uint)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SendKeys && this.action.Parameters.Count >= 1)
			{
				engine.SendKeys((string)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeyDown && this.action.Parameters.Count >= 1)
			{
				engine.KeyDown((VirtualKeys)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeyPress && this.action.Parameters.Count >= 1)
			{
				engine.KeyPress((VirtualKeys)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeysPress && this.action.Parameters.Count >= 1)
			{
				List<VirtualKeys> keys = new List<VirtualKeys>();
				foreach (object item in this.action.Parameters)
				{
					keys.Add((VirtualKeys)item);
				}
				
				engine.KeysPress(keys.ToArray());
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeyUp && this.action.Parameters.Count >= 1)
			{
				engine.KeyUp((VirtualKeys)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateSendKeys && this.action.Parameters.Count >= 1)
			{
				engine.SimulateSendKeys((string)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateClickAt && this.action.Parameters.Count >= 2)
			{
				engine.SimulateClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateRightClickAt && this.action.Parameters.Count >= 2)
			{
				engine.SimulateRightClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateMiddleClickAt && this.action.Parameters.Count >= 2)
			{
				engine.SimulateMiddleClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateDoubleClickAt && this.action.Parameters.Count >= 2)
			{
				engine.SimulateDoubleClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			
			return false;
		}
		
		private bool CallAction(ElementBase libraryElement)
		{
			if (this.action.Element.ControlType == ControlType.Button)
			{
				if (this.action.ActionId == ActionIds.Press)
				{
					UIDA_Button button = libraryElement as UIDA_Button;
					if (button != null)
					{
						button.Press();
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.Calendar)
			{
				UIDA_Calendar calendar = libraryElement as UIDA_Calendar;
				if (calendar != null && this.action.Parameters.Count > 0)
				{
					if (this.action.ActionId == ActionIds.SelectDate)
					{
						calendar.SelectDate((DateTime)this.action.Parameters[0]);
						return true;
					}
					else if (this.action.ActionId == ActionIds.AddDateToSelection)
					{
						calendar.AddToSelection((DateTime)this.action.Parameters[0]);
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.CheckBox)
			{
				UIDA_CheckBox checkBox = libraryElement as UIDA_CheckBox;
				if (checkBox != null)
				{
					if (this.action.ActionId == ActionIds.Toggle)
					{
						checkBox.Toggle();
						return true;
					}
					if (this.action.ActionId == ActionIds.IsChecked && this.action.Parameters.Count > 0)
					{
						checkBox.IsChecked = (bool)this.action.Parameters[0];
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.ComboBox)
			{
				UIDA_ComboBox comboBox = libraryElement as UIDA_ComboBox;
				if (comboBox != null)
				{
					if (this.action.ActionId == ActionIds.SetText && this.action.Parameters.Count > 0)
					{
						comboBox.SetText((string)this.action.Parameters[0]);
						return true;
					}
					if (this.action.ActionId == ActionIds.Expand)
					{
						comboBox.Expand();
						return true;
					}
					if (this.action.ActionId == ActionIds.Collapse)
					{
						comboBox.Collapse();
						return true;
					}
					if (this.action.ActionId == ActionIds.SelectByIndex && this.action.Parameters.Count > 0)
					{
						comboBox.Select((int)this.action.Parameters[0]);
						return true;
					}
					if (this.action.ActionId == ActionIds.SelectByText && this.action.Parameters.Count > 0)
					{
						comboBox.Select((string)this.action.Parameters[0]);
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.DataGrid)
			{
				UIDA_DataGrid dataGrid = libraryElement as UIDA_DataGrid;
				if (dataGrid != null)
				{
					if (this.action.ActionId == ActionIds.SelectAll)
					{
						dataGrid.SelectAll();
						return true;
					}
					if (this.action.ActionId == ActionIds.ClearAllSelection)
					{
						dataGrid.ClearAllSelection();
						return true;
					}
					if (this.action.ActionId == ActionIds.Scroll && this.action.Parameters.Count >= 2)
					{
						dataGrid.Scroll((double)this.action.Parameters[0], (double)this.action.Parameters[1]);
						return true;
					}
					if (this.action.ActionId == ActionIds.SelectByIndex && this.action.Parameters.Count > 0)
					{
						dataGrid.Select((int)this.action.Parameters[0]);
						return true;
					}
					if (this.action.ActionId == ActionIds.AddToSelection && this.action.Parameters.Count > 0)
					{
						dataGrid.AddToSelection((int)this.action.Parameters[0]);
						return true;
					}
					if (this.action.ActionId == ActionIds.RemoveFromSelection && this.action.Parameters.Count > 0)
					{
						dataGrid.RemoveFromSelection((int)this.action.Parameters[0]);
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.Group)
			{
				UIDA_Group dataGridGroup = libraryElement as UIDA_Group;
				if (dataGridGroup != null)
				{
					if (this.action.ActionId == ActionIds.Expand)
					{
						dataGridGroup.Expand();
						return true;
					}
					if (this.action.ActionId == ActionIds.Collapse)
					{
						dataGridGroup.Collapse();
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.DataItem)
			{
				UIDA_DataItem dataItem = libraryElement as UIDA_DataItem;
				if (dataItem != null)
				{
					if (this.action.ActionId == ActionIds.Select)
					{
						dataItem.Select();
						return true;
					}
					if (this.action.ActionId == ActionIds.AddToSelection)
					{
						dataItem.AddToSelection();
						return true;
					}
					if (this.action.ActionId == ActionIds.RemoveFromSelection)
					{
						dataItem.RemoveFromSelection();
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.DatePicker)
			{
				UIDA_DatePicker datePicker = libraryElement as UIDA_DatePicker;
				if (datePicker != null)
				{
					if (this.action.ActionId == ActionIds.SelectDate && this.action.Parameters.Count > 0)
					{
						datePicker.SelectedDate = (DateTime)this.action.Parameters[0];
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.Document)
			{
				UIDA_Document document = libraryElement as UIDA_Document;
				if (document != null)
				{
					if (this.action.ActionId == ActionIds.SetText && this.action.Parameters.Count > 0)
					{
						document.SetText((string)this.action.Parameters[0]);
						return true;
					}
					if (this.action.ActionId == ActionIds.ClearText)
					{
						document.ClearText();
						return true;
					}
					if (this.action.ActionId == ActionIds.SelectText && this.action.Parameters.Count > 0)
					{
						document.SelectText((string)this.action.Parameters[0]);
						return true;
					}
					if (this.action.ActionId == ActionIds.SelectAll)
					{
						document.SelectAll();
						return true;
					}
					if (this.action.ActionId == ActionIds.ClearSelection)
					{
						document.ClearSelection();
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.Edit)
			{
				UIDA_Edit edit = libraryElement as UIDA_Edit;
				if (edit != null)
				{
					if (this.action.ActionId == ActionIds.SetText && this.action.Parameters.Count > 0)
					{
						edit.SetText((string)this.action.Parameters[0]);
						return true;
					}
					if (this.action.ActionId == ActionIds.ClearText)
					{
						edit.ClearText();
						return true;
					}
					if (this.action.ActionId == ActionIds.SelectText && this.action.Parameters.Count > 0)
					{
						edit.SelectText((string)this.action.Parameters[0]);
						return true;
					}
					if (this.action.ActionId == ActionIds.SelectAll)
					{
						edit.SelectAll();
						return true;
					}
					if (this.action.ActionId == ActionIds.ClearSelection)
					{
						edit.ClearSelection();
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.Hyperlink)
			{
				UIDA_HyperLink hyperlink = libraryElement as UIDA_HyperLink;
				if (hyperlink != null)
				{
					if (this.action.ActionId == ActionIds.AccessLink)
					{
						hyperlink.AccessLink();
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.List)
			{
				UIDA_List list = libraryElement as UIDA_List;
				if (list != null)
				{
					if (this.action.ActionId == ActionIds.SelectByIndex && this.action.Parameters.Count > 0)
					{
						list.Select((int)this.action.Parameters[0]);
						return true;
					}
					if (this.action.ActionId == ActionIds.SelectByText && this.action.Parameters.Count > 0)
					{
						list.Select((string)this.action.Parameters[0]);
						return true;
					}
					if (this.action.ActionId == ActionIds.AddToSelection && this.action.Parameters.Count > 0)
					{
						list.AddToSelection((int)this.action.Parameters[0]);
						return true;
					}
					if (this.action.ActionId == ActionIds.AddToSelectionByText && this.action.Parameters.Count > 0)
					{
						list.AddToSelection((string)this.action.Parameters[0]);
						return true;
					}
					if (this.action.ActionId == ActionIds.RemoveFromSelection && this.action.Parameters.Count > 0)
					{
						list.RemoveFromSelection((int)this.action.Parameters[0]);
						return true;
					}
					if (this.action.ActionId == ActionIds.RemoveFromSelectionByText && this.action.Parameters.Count > 0)
					{
						list.RemoveFromSelection((string)this.action.Parameters[0]);
						return true;
					}
					if (this.action.ActionId == ActionIds.SelectAll)
					{
						list.SelectAll();
						return true;
					}
					if (this.action.ActionId == ActionIds.ClearAllSelection)
					{
						list.ClearAllSelection();
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.ListItem)
			{
				UIDA_ListItem listItem = libraryElement as UIDA_ListItem;
				if (listItem != null)
				{
					if (this.action.ActionId == ActionIds.Select)
					{
						listItem.Select();
						return true;
					}
					if (this.action.ActionId == ActionIds.RemoveFromSelection)
					{
						listItem.RemoveFromSelection();
						return true;
					}
					if (this.action.ActionId == ActionIds.AddToSelection)
					{
						listItem.AddToSelection();
						return true;
					}
					if (this.action.ActionId == ActionIds.BringIntoView)
					{
						listItem.BringIntoView();
						return true;
					}
					if (this.action.ActionId == ActionIds.IsChecked && this.action.Parameters.Count > 0)
					{
						listItem.IsChecked = (bool)this.action.Parameters[0];
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.MenuItem)
			{
				UIDA_MenuItem menuItem = libraryElement as UIDA_MenuItem;
				if (menuItem != null)
				{
					if (this.action.ActionId == ActionIds.AccessMenu)
					{
						menuItem.AccessMenu();
						return true;
					}
					if (this.action.ActionId == ActionIds.Expand)
					{
						menuItem.Expand();
						return true;
					}
					if (this.action.ActionId == ActionIds.Collapse)
					{
						menuItem.Collapse();
						return true;
					}
					if (this.action.ActionId == ActionIds.Toggle)
					{
						menuItem.Toggle();
						return true;
					}
					if (this.action.ActionId == ActionIds.IsChecked && this.action.Parameters.Count > 0)
					{
						menuItem.IsChecked = (bool)this.action.Parameters[0];
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.RadioButton)
			{
				UIDA_RadioButton radioButton = libraryElement as UIDA_RadioButton;
				if (radioButton != null)
				{
					if (this.action.ActionId == ActionIds.Select)
					{
						radioButton.Select();
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.ScrollBar)
			{
				UIDA_ScrollBar scrollBar = libraryElement as UIDA_ScrollBar;
				if (scrollBar != null)
				{
					if (this.action.ActionId == ActionIds.SmallIncrement)
					{
						scrollBar.SmallIncrement();
						return true;
					}
					if (this.action.ActionId == ActionIds.LargeIncrement)
					{
						scrollBar.LargeIncrement();
						return true;
					}
					if (this.action.ActionId == ActionIds.SmallDecrement)
					{
						scrollBar.SmallDecrement();
						return true;
					}
					if (this.action.ActionId == ActionIds.LargeDecrement)
					{
						scrollBar.LargeDecrement();
						return true;
					}
					if (this.action.ActionId == ActionIds.Value && this.action.Parameters.Count > 0)
					{
						scrollBar.Value = (double)this.action.Parameters[0];
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.Slider)
			{
				UIDA_Slider slider = libraryElement as UIDA_Slider;
				if (slider != null)
				{
					if (this.action.ActionId == ActionIds.SmallIncrement)
					{
						slider.SmallIncrement();
						return true;
					}
					if (this.action.ActionId == ActionIds.LargeIncrement)
					{
						slider.LargeIncrement();
						return true;
					}
					if (this.action.ActionId == ActionIds.SmallDecrement)
					{
						slider.SmallDecrement();
						return true;
					}
					if (this.action.ActionId == ActionIds.LargeDecrement)
					{
						slider.LargeDecrement();
						return true;
					}
					if (this.action.ActionId == ActionIds.Value && this.action.Parameters.Count > 0)
					{
						slider.Value = (double)this.action.Parameters[0];
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.Spinner)
			{
				UIDA_Spinner spinner = libraryElement as UIDA_Spinner;
				if (spinner != null)
				{
					if (this.action.ActionId == ActionIds.Increment)
					{
						spinner.Increment();
						return true;
					}
					if (this.action.ActionId == ActionIds.Decrement)
					{
						spinner.Decrement();
						return true;
					}
					if (this.action.ActionId == ActionIds.Value && this.action.Parameters.Count > 0)
					{
						spinner.Value = (double)this.action.Parameters[0];
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.SplitButton)
			{
				UIDA_SplitButton splitButton = libraryElement as UIDA_SplitButton;
				if (splitButton != null)
				{
					if (this.action.ActionId == ActionIds.Press)
					{
						splitButton.Press();
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.Tab)
			{
				UIDA_TabCtrl tabCtrl = libraryElement as UIDA_TabCtrl;
				if (tabCtrl != null)
				{
					if (this.action.ActionId == ActionIds.SelectByIndex && this.action.Parameters.Count > 0)
					{
						tabCtrl.Select((int)this.action.Parameters[0]);
						return true;
					}
					if (this.action.ActionId == ActionIds.SelectByText && this.action.Parameters.Count > 0)
					{
						tabCtrl.Select((string)this.action.Parameters[0]);
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.TabItem)
			{
				UIDA_TabItem tabItem = libraryElement as UIDA_TabItem;
				if (tabItem != null)
				{
					if (this.action.ActionId == ActionIds.Select)
					{
						tabItem.Select();
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.TreeItem)
			{
				UIDA_TreeItem treeItem = libraryElement as UIDA_TreeItem;
				if (treeItem != null)
				{
					if (this.action.ActionId == ActionIds.Select)
					{
						treeItem.Select();
						return true;
					}
					if (this.action.ActionId == ActionIds.Expand)
					{
						treeItem.Expand();
						return true;
					}
					if (this.action.ActionId == ActionIds.Collapse)
					{
						treeItem.Collapse();
						return true;
					}
					if (this.action.ActionId == ActionIds.Toggle)
					{
						treeItem.Toggle();
						return true;
					}
					if (this.action.ActionId == ActionIds.BringIntoView)
					{
						treeItem.BringIntoView();
						return true;
					}
					if (this.action.ActionId == ActionIds.IsChecked && this.action.Parameters.Count > 0)
					{
						treeItem.IsChecked = (bool)this.action.Parameters[0];
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.Custom)
			{
				UIDA_Table.Cell cell = libraryElement as UIDA_Table.Cell;
				if (cell != null)
				{
					if (this.action.ActionId == ActionIds.Value && this.action.Parameters.Count > 0)
					{
						cell.Value = (string)this.action.Parameters[0];
						return true;
					}
				}
			}
			else if (this.action.Element.ControlType == ControlType.Window)
			{
				UIDA_Window window = libraryElement as UIDA_Window;
				if (window != null)
				{
					if (this.action.ActionId == ActionIds.Show)
					{
						window.Show();
						return true;
					}
					if (this.action.ActionId == ActionIds.Minimize)
					{
						window.Minimize();
						return true;
					}
					if (this.action.ActionId == ActionIds.Maximize)
					{
						window.Maximize();
						return true;
					}
					if (this.action.ActionId == ActionIds.Restore)
					{
						window.Restore();
						return true;
					}
					if (this.action.ActionId == ActionIds.Close)
					{
						window.Close();
						return true;
					}
					if (this.action.ActionId == ActionIds.BringToForeground)
					{
						window.BringToForeground();
						return true;
					}
					if (this.action.ActionId == ActionIds.Move && this.action.Parameters.Count >= 2)
					{
						window.Move((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
						return true;
					}
					if (this.action.ActionId == ActionIds.MoveOffset && this.action.Parameters.Count >= 2)
					{
						window.MoveOffset((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
						return true;
					}
					if (this.action.ActionId == ActionIds.Resize && this.action.Parameters.Count >= 2)
					{
						window.Resize((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
						return true;
					}
					if (this.action.ActionId == ActionIds.WindowWidth && this.action.Parameters.Count >= 1)
					{
						window.WindowWidth = (int)this.action.Parameters[0];
						return true;
					}
					if (this.action.ActionId == ActionIds.WindowHeight && this.action.Parameters.Count >= 1)
					{
						window.WindowHeight = (int)this.action.Parameters[0];
						return true;
					}
				}
			}
			
			if (this.action.ActionId == ActionIds.Click)
			{
				libraryElement.Click();
				return true;
			}
			else if (this.action.ActionId == ActionIds.RightClick)
			{
				libraryElement.RightClick();
				return true;
			}
			else if (this.action.ActionId == ActionIds.MiddleClick)
			{
				libraryElement.MiddleClick();
				return true;
			}
			else if (this.action.ActionId == ActionIds.DoubleClick)
			{
				libraryElement.DoubleClick();
				return true;
			}
			else if (this.action.ActionId == ActionIds.ClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.ClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.RightClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.RightClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MiddleClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.MiddleClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.DoubleClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.DoubleClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MoveMouse && this.action.Parameters.Count >= 2)
			{
				libraryElement.MoveMouse((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MouseScrollUp && this.action.Parameters.Count >= 1)
			{
				libraryElement.MouseScrollUp((uint)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MouseScrollDown && this.action.Parameters.Count >= 1)
			{
				libraryElement.MouseScrollDown((uint)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.Invoke)
			{
				libraryElement.Invoke();
				return true;
			}
			else if (this.action.ActionId == ActionIds.Focus)
			{
				libraryElement.Focus();
				return true;
			}
			else if (this.action.ActionId == ActionIds.BringToForeground)
			{
				libraryElement.BringToForeground();
				return true;
			}
			else if (this.action.ActionId == ActionIds.SendKeys && this.action.Parameters.Count >= 1)
			{
				libraryElement.SendKeys((string)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeyDown && this.action.Parameters.Count >= 1)
			{
				libraryElement.KeyDown((VirtualKeys)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeyPress && this.action.Parameters.Count >= 1)
			{
				libraryElement.KeyPress((VirtualKeys)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeysPress && this.action.Parameters.Count >= 1)
			{
				List<VirtualKeys> keys = new List<VirtualKeys>();
				foreach (object item in this.action.Parameters)
				{
					keys.Add((VirtualKeys)item);
				}
				
				libraryElement.KeysPress(keys.ToArray());
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeyUp && this.action.Parameters.Count >= 1)
			{
				libraryElement.KeyUp((VirtualKeys)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.PredefinedKeysCombination && this.action.Parameters.Count >= 1)
			{
				string keysCombination = this.action.Parameters[0].ToString();
				if (keysCombination.StartsWith("Ctrl"))
				{
					libraryElement.KeyDown(VirtualKeys.Control);
				}
				else if (keysCombination.StartsWith("Alt"))
				{
					libraryElement.KeyDown(VirtualKeys.Alt);
				}
				
				if (keysCombination == "Ctrl+C")
				{
					libraryElement.KeyPress(VirtualKeys.C);
				}
				else if (keysCombination == "Ctrl+V")
				{
					libraryElement.KeyPress(VirtualKeys.V);
				}
				else if (keysCombination == "Ctrl+X")
				{
					libraryElement.KeyPress(VirtualKeys.X);
				}
				else if (keysCombination == "Ctrl+Z")
				{
					libraryElement.KeyPress(VirtualKeys.Z);
				}
				else if (keysCombination == "Ctrl+Y")
				{
					libraryElement.KeyPress(VirtualKeys.Y);
				}
				else if (keysCombination == "Ctrl+A")
				{
					libraryElement.KeyPress(VirtualKeys.A);
				}
				else if (keysCombination == "Ctrl+S")
				{
					libraryElement.KeyPress(VirtualKeys.S);
				}
				else if (keysCombination == "Ctrl+F")
				{
					libraryElement.KeyPress(VirtualKeys.F);
				}
				else if (keysCombination == "Ctrl+N")
				{
					libraryElement.KeyPress(VirtualKeys.N);
				}
				else if (keysCombination == "Ctrl+O")
				{
					libraryElement.KeyPress(VirtualKeys.O);
				}
				else if (keysCombination == "Ctrl+P")
				{
					libraryElement.KeyPress(VirtualKeys.P);
				}
				else if (keysCombination == "Alt+F4")
				{
					libraryElement.KeyPress(VirtualKeys.F4);
				}
				
				if (keysCombination.StartsWith("Ctrl"))
				{
					libraryElement.KeyUp(VirtualKeys.Control);
				}
				else if (keysCombination.StartsWith("Alt"))
				{
					libraryElement.KeyUp(VirtualKeys.Alt);
				}
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateSendKeys && this.action.Parameters.Count >= 1)
			{
				libraryElement.SimulateSendKeys((string)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateClick)
			{
				libraryElement.SimulateClick();
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateRightClick)
			{
				libraryElement.SimulateRightClick();
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateMiddleClick)
			{
				libraryElement.SimulateMiddleClick();
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateDoubleClick)
			{
				libraryElement.SimulateDoubleClick();
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.SimulateClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateRightClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.SimulateRightClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateMiddleClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.SimulateMiddleClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateDoubleClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.SimulateDoubleClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.CaptureToFile && this.action.Parameters.Count >= 1)
			{
				libraryElement.CaptureToFile((string)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.WaitForInputIdle)
			{
				libraryElement.WaitForInputIdle();
				return true;
			}
			
			return false;
		}
	}
}