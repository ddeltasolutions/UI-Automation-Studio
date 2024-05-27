using System.Windows.Controls;
using System.Linq;
using System.Windows.Input;

namespace UIAutomationStudio
{
	public partial class UserControlSelectAction : UserControl
	{
		public void RefreshActionsTab()
		{
			ControlType controlType = action.Element.ControlType;
			
			//Specific actions
			specificActions.Clear();
			if (controlType == ControlType.Button)
			{
				specificActions.Add(
					new ActionInfo { Description = "Press the button", ActionId = ActionIds.Press });
			}
			else if (controlType == ControlType.Calendar)
			{
				specificActions.Add(
					new ActionInfo { Description = "Select the specified date and deselect other selected dates", 
					ActionId = ActionIds.SelectDate });
				specificActions.Add(
					new ActionInfo { Description = "Add the specified date to selection", 
					ActionId = ActionIds.AddDateToSelection });
			}
			else if (controlType == ControlType.CheckBox)
			{
				specificActions.Add(
					new ActionInfo { Description = "Toggle between the states of the checkbox", 
					ActionId = ActionIds.Toggle });
				specificActions.Add(
					new ActionInfo { Description = "Set the checked state of the checkbox", 
					ActionId = ActionIds.IsChecked });
			}
			else if (controlType == ControlType.ComboBox)
			{
				specificActions.Add(
					new ActionInfo { Description = "Set the text of the combobox", 
					ActionId = ActionIds.SetText });
				specificActions.Add(
					new ActionInfo { Description = "Expand the combobox", 
					ActionId = ActionIds.Expand });
				specificActions.Add(
					new ActionInfo { Description = "Collapse the combobox", 
					ActionId = ActionIds.Collapse });
				specificActions.Add(
					new ActionInfo { Description = "Select an item by index", 
					ActionId = ActionIds.SelectByIndex });
				specificActions.Add(
					new ActionInfo { Description = "Select an item by text", 
					ActionId = ActionIds.SelectByText });
			}
			else if (controlType == ControlType.DataGrid)
			{
				specificActions.Add(
					new ActionInfo { Description = "Select all items in the DataGrid control", 
					ActionId = ActionIds.SelectAll });
				specificActions.Add(
					new ActionInfo { Description = "Clear all selected items in the DataGrid control", 
					ActionId = ActionIds.ClearAllSelection });
				specificActions.Add(
					new ActionInfo { Description = "Scroll vertically and/or horizontally using the specified percents", 
					ActionId = ActionIds.Scroll });
				specificActions.Add(
					new ActionInfo { Description = "Select an item by index", 
					ActionId = ActionIds.SelectByIndex });
				specificActions.Add(
					new ActionInfo { Description = "Add an item to selection in the DataGrid by the item index", 
					ActionId = ActionIds.AddToSelection });
				specificActions.Add(
					new ActionInfo { Description = "Remove an item from selection in the DataGrid by the item index", 
					ActionId = ActionIds.RemoveFromSelection });
			}
			else if (controlType == ControlType.Group)
			{
				Element parent = action.Element.Parent;
				if (parent != null && 
					(parent.ControlType == ControlType.DataGrid || 
					parent.ControlType == ControlType.DataItem || 
					parent.ControlType == ControlType.Header || 
					parent.ControlType == ControlType.HeaderItem))
					{
						specificActions.Add(
							new ActionInfo { Description = "Expand a group inside a data grid - if supported", 
							ActionId = ActionIds.Expand });
						specificActions.Add(
							new ActionInfo { Description = "Collapse a group inside a data grid - if supported", 
							ActionId = ActionIds.Collapse });
					}
			}
			else if (controlType == ControlType.DataItem)
			{
				specificActions.Add(
					new ActionInfo { Description = "Select the item and deselect all other selected items", 
					ActionId = ActionIds.Select });
				specificActions.Add(
					new ActionInfo { Description = "Add the item to selection", 
					ActionId = ActionIds.AddToSelection });
				specificActions.Add(
					new ActionInfo { Description = "Remove the item from selection", 
					ActionId = ActionIds.RemoveFromSelection });
			}
			else if (controlType == ControlType.DatePicker)
			{
				specificActions.Add(
					new ActionInfo { Description = "Set specified date in the DatePicker control", 
					ActionId = ActionIds.SelectDate });
			}
			else if (controlType == ControlType.Edit || controlType == ControlType.Document)
			{
				specificActions.Add(
					new ActionInfo { Description = "Set the text of the edit control", 
					ActionId = ActionIds.SetText });
				specificActions.Add(
					new ActionInfo { Description = "Clear the text of the edit control", 
					ActionId = ActionIds.ClearText });
				specificActions.Add(
					new ActionInfo { Description = "Select a specified text in the edit control", 
					ActionId = ActionIds.SelectText });
				specificActions.Add(
					new ActionInfo { Description = "Select all text in the edit control", 
					ActionId = ActionIds.SelectAll });
				specificActions.Add(
					new ActionInfo { Description = "Clear any selected text in the edit control", 
					ActionId = ActionIds.ClearSelection });
			}
			else if (controlType == ControlType.Hyperlink)
			{
				specificActions.Add(
					new ActionInfo { Description = "Access the link", 
					ActionId = ActionIds.AccessLink });
			}
			else if (controlType == ControlType.List)
			{
				specificActions.Add(
					new ActionInfo { Description = "Select an item by the item index and deselect other selected items", 
					ActionId = ActionIds.SelectByIndex });
				specificActions.Add(
					new ActionInfo { Description = "Select an item (or more items) by the item text and deselect other selected items", 
					ActionId = ActionIds.SelectByText });
				specificActions.Add(
					new ActionInfo { Description = "Add an item to selection in the list by the item index", 
					ActionId = ActionIds.AddToSelection });
				specificActions.Add(
					new ActionInfo { Description = "Add an item (or more items) to selection by the item text", 
					ActionId = ActionIds.AddToSelectionByText });
				specificActions.Add(
					new ActionInfo { Description = "Remove an item from selection in the list by the item index", 
					ActionId = ActionIds.RemoveFromSelection });
				specificActions.Add(
					new ActionInfo { Description = "Remove an item (or more items) from selection by the item text", 
					ActionId = ActionIds.RemoveFromSelectionByText });
				specificActions.Add(
					new ActionInfo { Description = "Select all items in the list", 
					ActionId = ActionIds.SelectAll });
				specificActions.Add(
					new ActionInfo { Description = "Clear all selections in the list", 
					ActionId = ActionIds.ClearAllSelection });
			}
			else if (controlType == ControlType.ListItem)
			{
				specificActions.Add(
					new ActionInfo { Description = "Select the list item and deselect any other selected items", 
					ActionId = ActionIds.Select });
				specificActions.Add(
					new ActionInfo { Description = "Remove the list item from selection", 
					ActionId = ActionIds.RemoveFromSelection });
				specificActions.Add(
					new ActionInfo { Description = "Add the list item to selection", 
					ActionId = ActionIds.AddToSelection });
				specificActions.Add(
					new ActionInfo { Description = "Bring the list item into viewable area of the parent List control", 
					ActionId = ActionIds.BringIntoView });
				specificActions.Add(
					new ActionInfo { Description = "Set the checked state of the list item", 
					ActionId = ActionIds.IsChecked });
			}
			else if (controlType == ControlType.MenuItem)
			{
				specificActions.Add(
					new ActionInfo { Description = "Access the menu item, like clicking on it", 
					ActionId = ActionIds.AccessMenu });
				specificActions.Add(
					new ActionInfo { Description = "Expand the menu item", 
					ActionId = ActionIds.Expand });
				specificActions.Add(
					new ActionInfo { Description = "Collapse the menu item", 
					ActionId = ActionIds.Collapse });
				specificActions.Add(
					new ActionInfo { Description = "Toggle the menu item through its checked states (checked/unchecked)", 
					ActionId = ActionIds.Toggle });
				specificActions.Add(
					new ActionInfo { Description = "Set the checked state of the menu item", 
					ActionId = ActionIds.IsChecked });
			}
			else if (controlType == ControlType.RadioButton)
			{
				specificActions.Add(
					new ActionInfo { Description = "Select the radio button", 
					ActionId = ActionIds.Select });
			}
			else if (controlType == ControlType.ScrollBar)
			{
				specificActions.Add(
					new ActionInfo { Description = "Small Increment the scrollbar", 
					ActionId = ActionIds.SmallIncrement });
				specificActions.Add(
					new ActionInfo { Description = "Large Increment the scrollbar", 
					ActionId = ActionIds.LargeIncrement });
				specificActions.Add(
					new ActionInfo { Description = "Small Decrement the scrollbar", 
					ActionId = ActionIds.SmallDecrement });
				specificActions.Add(
					new ActionInfo { Description = "Large Decrement the scrollbar", 
					ActionId = ActionIds.LargeDecrement });
				specificActions.Add(
					new ActionInfo { Description = "Set the value of the scrollbar", 
					ActionId = ActionIds.Value });
			}
			else if (controlType == ControlType.Slider)
			{
				specificActions.Add(
					new ActionInfo { Description = "Small Increment the slider. Is like pressing the arrow \"Right\" key.", 
					ActionId = ActionIds.SmallIncrement });
				specificActions.Add(
					new ActionInfo { Description = "Large Increment the slider. Is like pressing in the right side of the thumb or \"Page Up\" key.", 
					ActionId = ActionIds.LargeIncrement });
				specificActions.Add(
					new ActionInfo { Description = "Small Decrement the slider. Is like pressing the arrow \"Left\" key.", 
					ActionId = ActionIds.SmallDecrement });
				specificActions.Add(
					new ActionInfo { Description = "Large Decrement the slider. Is like pressing in the left side of the thumb or \"Page Down\" key.", 
					ActionId = ActionIds.LargeDecrement });
				specificActions.Add(
					new ActionInfo { Description = "Set the value of the slider", 
					ActionId = ActionIds.Value });
			}
			else if (controlType == ControlType.Spinner)
			{
				specificActions.Add(
					new ActionInfo { Description = "Increment the value of spinner. Is like pressing the up arrow.", 
					ActionId = ActionIds.Increment });
				specificActions.Add(
					new ActionInfo { Description = "Decrement the value of spinner. Is like pressing the down arrow.", 
					ActionId = ActionIds.Decrement });
				specificActions.Add(
					new ActionInfo { Description = "Set the value of the spinner", 
					ActionId = ActionIds.Value });
			}
			else if (controlType == ControlType.SplitButton)
			{
				specificActions.Add(
					new ActionInfo { Description = "Press the split button", 
					ActionId = ActionIds.Press });
			}
			else if (controlType == ControlType.Tab)
			{
				specificActions.Add(
					new ActionInfo { Description = "Select a Tab Item in a Tab Control by the tab item index", 
					ActionId = ActionIds.SelectByIndex });
				specificActions.Add(
					new ActionInfo { Description = "Select a Tab Item in a Tab Control by the tab item text", 
					ActionId = ActionIds.SelectByText });
			}
			else if (controlType == ControlType.TabItem)
			{
				specificActions.Add(
					new ActionInfo { Description = "Select the tab item", 
					ActionId = ActionIds.Select });
			}
			else if (controlType == ControlType.TreeItem)
			{
				specificActions.Add(
					new ActionInfo { Description = "Select the tree item and deselect all other selected tree items", 
					ActionId = ActionIds.Select });
				specificActions.Add(
					new ActionInfo { Description = "Expand the tree item", 
					ActionId = ActionIds.Expand });
				specificActions.Add(
					new ActionInfo { Description = "Collapse the tree item", 
					ActionId = ActionIds.Collapse });
				specificActions.Add(
					new ActionInfo { Description = "Cycle through the check states (checked, unchecked, indeterminate)", 
					ActionId = ActionIds.Toggle });
				specificActions.Add(
					new ActionInfo { Description = "Bring the tree item into viewable area of the parent Tree control", 
					ActionId = ActionIds.BringIntoView });
				specificActions.Add(
					new ActionInfo { Description = "Set the checked state of the tree item if supported", 
					ActionId = ActionIds.IsChecked });
			}
			else if (controlType == ControlType.Custom)
			{
				if (action.Element.Parent != null && action.Element.Parent.ControlType == ControlType.Custom)
				{
					if (action.Element.Parent.Parent != null && action.Element.Parent.Parent.ControlType == ControlType.Table)
					{
						// this is a cell in a Table
						specificActions.Add(
							new ActionInfo { Description = "Set the value of this cell in a table", 
							ActionId = ActionIds.Value });
					}
				}
			}
			else if (controlType == ControlType.Window)
			{
				specificActions.Add(
					new ActionInfo { Description = "Show the window", 
					ActionId = ActionIds.Show });
				specificActions.Add(
					new ActionInfo { Description = "Minimize the window", 
					ActionId = ActionIds.Minimize });
				specificActions.Add(
					new ActionInfo { Description = "Maximize the window", 
					ActionId = ActionIds.Maximize });
				specificActions.Add(
					new ActionInfo { Description = "Restore the window position", 
					ActionId = ActionIds.Restore });
				specificActions.Add(
					new ActionInfo { Description = "Close the window", 
					ActionId = ActionIds.Close });
				specificActions.Add(
					new ActionInfo { Description = "Bring the window to foreground", 
					ActionId = ActionIds.BringToForeground });
				specificActions.Add(
					new ActionInfo { Description = "Move the window to specified x and y screen coordinates", 
					ActionId = ActionIds.Move });
				specificActions.Add(
					new ActionInfo { Description = "Move the window relatively with horizontal and vertical offsets", 
					ActionId = ActionIds.MoveOffset });
				specificActions.Add(
					new ActionInfo { Description = "Set both width and height of the window", 
					ActionId = ActionIds.Resize });
				specificActions.Add(
					new ActionInfo { Description = "Set the width of the window", 
					ActionId = ActionIds.WindowWidth });
				specificActions.Add(
					new ActionInfo { Description = "Set the height of the window", 
					ActionId = ActionIds.WindowHeight });
			}
			tabSpecific.DataContext = specificActions;
			
			// Mouse Events
			if (mouseActions.Count == 0)
			{
				mouseActions.Add(
					new ActionInfo { Description = "Click element in the center", 
					ActionId = ActionIds.Click, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Right Click element in the center", 
					ActionId = ActionIds.RightClick, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Middle Click element in the center using the middle mouse button", 
					ActionId = ActionIds.MiddleClick, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Double Click element in the center", 
					ActionId = ActionIds.DoubleClick, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Click element at specified relative coordinates", 
					ActionId = ActionIds.ClickAt, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Right Click element at specified relative coordinates", 
					ActionId = ActionIds.RightClickAt, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Middle Click element at specified relative coordinates using the middle mouse button", 
					ActionId = ActionIds.MiddleClickAt, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Double Click element at specified relative coordinates", 
					ActionId = ActionIds.DoubleClickAt, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Move the mouse cursor at specified relative coordinates", 
					ActionId = ActionIds.MoveMouse, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Scroll the Mouse Wheel Up over the element with specified number of ticks", 
					ActionId = ActionIds.MouseScrollUp, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Scroll the Mouse Wheel Down over the element with specified number of ticks", 
					ActionId = ActionIds.MouseScrollDown , GroupName = "Mouse Events"});
				tabMouse.DataContext = mouseActions;
			}
			else
			{
				listMouse.SelectedItem = null;
			}
				
			// Keyboard Events
			if (keyboardActions.Count == 0)
			{
				keyboardActions.Add(
					new ActionInfo { Description = "Send keys to element", 
					ActionId = ActionIds.SendKeys, GroupName = "Keyboard Events" });
				keyboardActions.Add(
					new ActionInfo { Description = "Press a Key (without releasing it)", 
					ActionId = ActionIds.KeyDown, GroupName = "Keyboard Events" });
				keyboardActions.Add(
					new ActionInfo { Description = "Press and Release a Key", 
					ActionId = ActionIds.KeyPress, GroupName = "Keyboard Events" });
				keyboardActions.Add(
					new ActionInfo { Description = "Press and Release Multiple Keys", 
					ActionId = ActionIds.KeysPress, GroupName = "Keyboard Events" });
				keyboardActions.Add(
					new ActionInfo { Description = "Release a Key", 
					ActionId = ActionIds.KeyUp, GroupName = "Keyboard Events" });
				keyboardActions.Add(
					new ActionInfo { Description = "Send a predefined keys combination", 
					ActionId = ActionIds.PredefinedKeysCombination, GroupName = "Keyboard Events" });
				tabKeyboard.DataContext = keyboardActions;
			}
			else
			{
				listKeyboard.SelectedItem = null;
			}
			
			// Other Events
			if (otherActions.Count == 0)
			{
				otherActions.Add(
					new ActionInfo { Description = "Invoke the default action of the element", 
					ActionId = ActionIds.Invoke, GroupName = "Other Events" });
				otherActions.Add(
					new ActionInfo { Description = "Bring keyboard Focus to element", 
					ActionId = ActionIds.Focus, GroupName = "Other Events" });
				otherActions.Add(
					new ActionInfo { Description = "Bring element into Foreground", 
					ActionId = ActionIds.BringToForeground, GroupName = "Other Events" });
				otherActions.Add(
					new ActionInfo { Description = "Capture element and save the image into a file", 
					ActionId = ActionIds.CaptureToFile, GroupName = "Other Events" });
				otherActions.Add(
					new ActionInfo { Description = "Wait for the process that created the element to enter an idle state", 
					ActionId = ActionIds.WaitForInputIdle, GroupName = "Other Events" });
				tabOther.DataContext = otherActions;
			}
			else
			{
				listOther.SelectedItem = null;
			}
				
			// Simulate Events
			if (simulateActions.Count == 0)
			{
				simulateActions.Add(
					new ActionInfo { Description = "Simulate sending keys to element - doesn't bring the element into focus", 
					ActionId = ActionIds.SimulateSendKeys, GroupName = "Simulate Events" });
				simulateActions.Add(
					new ActionInfo { Description = "Simulate a left mouse button click in the center - doesn't bring the element into focus", 
					ActionId = ActionIds.SimulateClick, GroupName = "Simulate Events" });
				simulateActions.Add(
					new ActionInfo { Description = "Simulate a right mouse button click in the center - doesn't bring the element into focus", 
					ActionId = ActionIds.SimulateRightClick, GroupName = "Simulate Events" });
				simulateActions.Add(
					new ActionInfo { Description = "Simulate a middle mouse button click in the center - doesn't bring the element into focus", 
					ActionId = ActionIds.SimulateMiddleClick, GroupName = "Simulate Events" });
				simulateActions.Add(
					new ActionInfo { Description = "Simulate a left mouse button double click in the center - doesn't bring the element into focus", 
					ActionId = ActionIds.SimulateDoubleClick, GroupName = "Simulate Events" });
				simulateActions.Add(
					new ActionInfo { Description = "Simulate a left mouse button click at relative location - doesn't bring the element into focus", 
					ActionId = ActionIds.SimulateClickAt, GroupName = "Simulate Events" });
				simulateActions.Add(
					new ActionInfo { Description = "Simulate a right mouse button click at relative location - doesn't bring the element into focus", 
					ActionId = ActionIds.SimulateRightClickAt, GroupName = "Simulate Events" });
				simulateActions.Add(
					new ActionInfo { Description = "Simulate a middle mouse button click at relative location - doesn't bring the element into focus", 
					ActionId = ActionIds.SimulateMiddleClickAt, GroupName = "Simulate Events" });
				simulateActions.Add(
					new ActionInfo { Description = "Simulate a double click at relative location - doesn't bring the element into focus", 
					ActionId = ActionIds.SimulateDoubleClickAt, GroupName = "Simulate Events" });
				tabSimulate.DataContext = simulateActions;
			}
			else
			{
				listSimulate.SelectedItem = null;
			}
			
			ActionInfo actionInfo = null;
			bool tabSelected = false;
			
			if (this.action.ActionId != ActionIds.None && this.action.Element.ControlType == this.initialControlType)
			{
				this.currentActionId = this.action.ActionId;
				
				actionInfo = specificActions.FirstOrDefault(x => x.ActionId == this.action.ActionId);
				if (actionInfo != null)
				{
					tabSpecific.IsSelected = true;
					tabSelected = true;
					
					listSpecificActions.Focus();
					listSpecificActions.SelectedItem = actionInfo;
				}
			}
			else
			{
				this.currentActionId = ActionIds.None;
			}
			
			if (this.action.ActionId != ActionIds.None)
			{
				actionInfo = mouseActions.FirstOrDefault(x => x.ActionId == this.action.ActionId);
				if (actionInfo != null)
				{
					tabMouse.IsSelected = true;
					tabSelected = true;
					
					Keyboard.Focus(listMouse);
					listMouse.SelectedItem = actionInfo;
				}
				
				actionInfo = keyboardActions.FirstOrDefault(x => x.ActionId == this.action.ActionId);
				if (actionInfo != null)
				{
					tabKeyboard.IsSelected = true;
					tabSelected = true;
					
					listKeyboard.Focus();
					listKeyboard.SelectedItem = actionInfo;
				}
				
				actionInfo = otherActions.FirstOrDefault(x => x.ActionId == this.action.ActionId);
				if (actionInfo != null)
				{
					tabOther.IsSelected = true;
					tabSelected = true;
					
					listOther.Focus();
					listOther.SelectedItem = actionInfo;
				}
				
				actionInfo = simulateActions.FirstOrDefault(x => x.ActionId == this.action.ActionId);
				if (actionInfo != null)
				{
					tabSimulate.IsSelected = true;
					tabSelected = true;
					
					listSimulate.Focus();
					listSimulate.SelectedItem = actionInfo;
				}
			}
			
			if (tabSelected == false)
			{
				if (specificActions.Count == 0)
				{
					if (tabSpecific.IsSelected == true)
					{
						tabMouse.IsSelected = true;
					}
				}
				else
				{
					if (tabMouse.IsSelected == true)
					{
						tabSpecific.IsSelected = true;
					}
				}
				
				parametersGroupBox.Content = null;
			}
		}
	}
}