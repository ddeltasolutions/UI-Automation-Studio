using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlSelectGeneralAction.xaml
    /// </summary>
    public partial class UserControlSelectGeneralAction : UserControl
    {
		private Action action = null;
		private ObservableCollection<ActionInfo> generalActions = new ObservableCollection<ActionInfo>();
		private List<ActionInfo> mouseActions = new List<ActionInfo>();
		private List<ActionInfo> keyboardActions = new List<ActionInfo>();
		private List<ActionInfo> simulateActions = new List<ActionInfo>();
		
		private ActionIds currentActionId = ActionIds.None;
		
		private UserControlMouseCoordinates ucMouseCoordinates = null;
		private UserControlMouseTicks ucMouseTicks = null;
		private UserControlSendKeys ucSendKeys = null;
		private UserControlPressKey ucPressKey = null;
		private UserControlStartProcess ucStartProcess = null;
		private UserControlSleep ucSleep = null;
	
        public UserControlSelectGeneralAction(Action action)
        {
            InitializeComponent();
			
			this.action = action;
        }
		
		public void RefreshActionsTab()
		{
			//General actions
			if (generalActions.Count == 0)
			{
				generalActions.Add(
					new ActionInfo { Description = "Start a process and wait for input idle (Recommended)", ActionId = ActionIds.StartProcessAndWaitForInputIdle });
				generalActions.Add(
					new ActionInfo { Description = "Start a process", ActionId = ActionIds.StartProcess });
				generalActions.Add(
					new ActionInfo { Description = "Make a pause in the task", ActionId = ActionIds.Sleep });
				generalActions.Add(
					new ActionInfo { Description = "Show the desktop - minimize all windows", ActionId = ActionIds.ShowDesktop });
				
				tabGeneral.DataContext = generalActions;
			}
			else
			{
				listGeneral.SelectedItem = null;
			}
			
			// Mouse Events
			if (mouseActions.Count == 0)
			{
				mouseActions.Add(
					new ActionInfo { Description = "Left mouse Click at screen coordinates", 
					ActionId = ActionIds.ClickAt, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Right mouse button Click at specified screen coordinates", 
					ActionId = ActionIds.RightClickAt, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Middle mouse button Click at specified screen coordinates", 
					ActionId = ActionIds.MiddleClickAt, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Left mouse button Double Click at specified screen coordinates", 
					ActionId = ActionIds.DoubleClickAt, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Move mouse pointer at specified screen coordinates", 
					ActionId = ActionIds.MoveMouse, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Scroll the Mouse Wheel Up with specified number of ticks", 
					ActionId = ActionIds.MouseScrollUp, GroupName = "Mouse Events" });
				mouseActions.Add(
					new ActionInfo { Description = "Scroll the Mouse Wheel Down with specified number of ticks", 
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
					new ActionInfo { Description = "Send keystrokes to the window which currently has the focus", 
					ActionId = ActionIds.SendKeys, GroupName = "Keyboard Events" });
				keyboardActions.Add(
					new ActionInfo { Description = "Press a key (without releasing it)", 
					ActionId = ActionIds.KeyDown, GroupName = "Keyboard Events" });
				keyboardActions.Add(
					new ActionInfo { Description = "Press and release a key", 
					ActionId = ActionIds.KeyPress, GroupName = "Keyboard Events" });
				keyboardActions.Add(
					new ActionInfo { Description = "Press and release multiple keys", 
					ActionId = ActionIds.KeysPress, GroupName = "Keyboard Events" });
				keyboardActions.Add(
					new ActionInfo { Description = "Release a key", 
					ActionId = ActionIds.KeyUp, GroupName = "Keyboard Events" });
				tabKeyboard.DataContext = keyboardActions;
			}
			else
			{
				listKeyboard.SelectedItem = null;
			}
				
			// Simulate Events
			if (simulateActions.Count == 0)
			{
				simulateActions.Add(
					new ActionInfo { Description = "Simulate keystrokes", 
					ActionId = ActionIds.SimulateSendKeys, GroupName = "Simulate Events" });
				
				simulateActions.Add(
					new ActionInfo { Description = "Simulate left mouse button click at screen coordinates", 
					ActionId = ActionIds.SimulateClickAt, GroupName = "Simulate Events" });
				simulateActions.Add(
					new ActionInfo { Description = "Simulate right mouse button click at screen coordinates", 
					ActionId = ActionIds.SimulateRightClickAt, GroupName = "Simulate Events" });
				simulateActions.Add(
					new ActionInfo { Description = "Simulate middle mouse button click at screen coordinates", 
					ActionId = ActionIds.SimulateMiddleClickAt, GroupName = "Simulate Events" });
				simulateActions.Add(
					new ActionInfo { Description = "Simulate left mouse button double click at screen coordinates", 
					ActionId = ActionIds.SimulateDoubleClickAt, GroupName = "Simulate Events" });
				tabSimulate.DataContext = simulateActions;
			}
			else
			{
				listSimulate.SelectedItem = null;
			}
			
			ActionInfo actionInfo = null;
			bool tabSelected = false;
			
			if (this.action.ActionId != ActionIds.None)
			{
				actionInfo = generalActions.FirstOrDefault(x => x.ActionId == this.action.ActionId);
				if (actionInfo != null)
				{
					tabGeneral.IsSelected = true;
					tabSelected = true;
					
					listGeneral.Focus();
					listGeneral.SelectedItem = actionInfo;
				}
				
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
				parametersGroupBox.Content = null;
			}
			
			this.currentActionId = this.action.ActionId;
		}
		
		private void OnDoubleClick(object sender, RoutedEventArgs e)
		{
			ListBox list = sender as ListBox;
			if (list == null)
			{
				return;
			}
			
			ActionInfo actionInfo = list.SelectedItem as ActionInfo;
			if (actionInfo == null)
			{
				return;
			}
			
			ShowActionParameters(actionInfo);
		}
		
		private void OnActionSelected(object sender, SelectionChangedEventArgs e)
		{
			System.Collections.IList selectedItems = e.AddedItems;
			if (selectedItems.Count == 0)
			{
				return;
			}
			
			ActionInfo actionInfo = selectedItems[0] as ActionInfo;
			if (actionInfo == null)
			{
				return;
			}
			
			ShowActionParameters(actionInfo);
		}	
		
		private void ShowActionParameters(ActionInfo actionInfo)
		{
			this.currentActionId = actionInfo.ActionId;
			
			ResetUserControls();
			
			if (actionInfo.ActionId == ActionIds.ClickAt || actionInfo.ActionId == ActionIds.RightClickAt || 
				actionInfo.ActionId == ActionIds.MiddleClickAt || actionInfo.ActionId == ActionIds.DoubleClickAt ||
				actionInfo.ActionId == ActionIds.MoveMouse || 
				actionInfo.ActionId == ActionIds.SimulateClickAt || actionInfo.ActionId == ActionIds.SimulateRightClickAt ||
				actionInfo.ActionId == ActionIds.SimulateMiddleClickAt || actionInfo.ActionId == ActionIds.SimulateDoubleClickAt)
			{
				ucMouseCoordinates = new UserControlMouseCoordinates(CoordinatesType.Mouse, actionInfo.ActionId, true);
				parametersGroupBox.Content = ucMouseCoordinates;
			}
			else if (actionInfo.ActionId == ActionIds.MouseScrollUp || actionInfo.ActionId == ActionIds.MouseScrollDown)
			{
				ucMouseTicks = new UserControlMouseTicks(actionInfo.ActionId);
				parametersGroupBox.Content = ucMouseTicks;
			}
			else if (actionInfo.ActionId == ActionIds.SendKeys || actionInfo.ActionId == ActionIds.SimulateSendKeys)
			{
				ucSendKeys = new UserControlSendKeys();
				parametersGroupBox.Content = ucSendKeys;
			}
			else if (actionInfo.ActionId == ActionIds.KeyDown || actionInfo.ActionId == ActionIds.KeyPress ||
				actionInfo.ActionId == ActionIds.KeysPress || actionInfo.ActionId == ActionIds.KeyUp)
			{
				ucPressKey = new UserControlPressKey(actionInfo.ActionId);
				parametersGroupBox.Content = ucPressKey;
			}
			else if (actionInfo.ActionId == ActionIds.StartProcess || 
				actionInfo.ActionId == ActionIds.StartProcessAndWaitForInputIdle)
			{
				ucStartProcess = new UserControlStartProcess(actionInfo.ActionId);
				parametersGroupBox.Content = ucStartProcess;
			}
			else if (actionInfo.ActionId == ActionIds.Sleep)
			{
				ucSleep = new UserControlSleep();
				parametersGroupBox.Content = ucSleep;
			}
			else
			{
				parametersGroupBox.Content = null;
			}
			
			if (this.action.ActionId == actionInfo.ActionId)
			{
				IParameters iparameters = parametersGroupBox.Content as IParameters;
				if (iparameters != null)
				{
					iparameters.Init(this.action.Parameters);
				}
			}
		}
		
		private void ResetUserControls()
		{
			ucMouseCoordinates = null;
			ucMouseTicks = null;
			ucSendKeys = null;
			ucPressKey = null;
			ucStartProcess = null;
			ucSleep = null;
		}
		
		public bool Validate()
		{
			if (currentActionId == ActionIds.None)
			{
				MessageBox.Show(Window.GetWindow(this), "Please select an action");
				return false;
			}
			
			bool isValid = true;
			this.action.Parameters = null;
			
			if (ucMouseCoordinates != null)
			{
				isValid = ucMouseCoordinates.ValidateParams(this.action);
			}
			else if (ucMouseTicks != null)
			{
				isValid = ucMouseTicks.ValidateParams(this.action);
			}
			else if (ucSendKeys != null)
			{
				isValid = ucSendKeys.ValidateParams(this.action);
			}
			else if (ucPressKey != null)
			{
				isValid = ucPressKey.ValidateParams(this.action);
			}
			else if (ucStartProcess != null)
			{
				isValid = ucStartProcess.ValidateParams(this.action);
			}
			else if (ucSleep != null)
			{
				isValid = ucSleep.ValidateParams(this.action);
			}
			
			if (isValid == true)
			{
				this.action.ActionId = currentActionId;
				return true;
			}
			return false;
		}
    }
}
