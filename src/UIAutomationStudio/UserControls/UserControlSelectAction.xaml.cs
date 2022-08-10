using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlSelectAction.xaml
    /// </summary>
    public partial class UserControlSelectAction : UserControl
    {
        public UserControlSelectAction(Action action, ControlType controlType)
        {
            InitializeComponent();
			
			this.action = action;
			this.initialControlType = controlType;
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
				ucMouseCoordinates = new UserControlMouseCoordinates(CoordinatesType.Mouse, actionInfo.ActionId, false, this.action.Element);
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
			else if (actionInfo.ActionId == ActionIds.PredefinedKeysCombination)
			{
				ucKeysCombination = new UserControlKeysCombination();
				parametersGroupBox.Content = ucKeysCombination;
			}
			else if (actionInfo.ActionId == ActionIds.CaptureToFile)
			{
				ucCapture = new UserControlCapture(action.Element.ControlType.ToString());
				parametersGroupBox.Content = ucCapture;
			}
			else if (actionInfo.ActionId == ActionIds.SelectDate)
			{
				ucSelectDate = new UserControlSelectDate();
				parametersGroupBox.Content = ucSelectDate;
			}
			else if (actionInfo.ActionId == ActionIds.AddDateToSelection)
			{
				ucSelectDate = new UserControlSelectDate("Specify date to add to selection");
				parametersGroupBox.Content = ucSelectDate;
			}
			else if (actionInfo.ActionId == ActionIds.IsChecked)
			{
				ucCheckBox = new UserControlCheckBox();
				parametersGroupBox.Content = ucCheckBox;
			}
			else if (actionInfo.ActionId == ActionIds.SetText)
			{
				if (this.action.Element.ControlType == ControlType.ComboBox)
				{
					ucSetText = new UserControlSetText(false);
				}
				else if (this.action.Element.ControlType == ControlType.Edit || 
					this.action.Element.ControlType == ControlType.Document)
				{
					ucSetText = new UserControlSetText(true);
				}
				parametersGroupBox.Content = ucSetText;
			}
			else if (actionInfo.ActionId == ActionIds.SelectByIndex)
			{
				ucSelectByIndex = new UserControlSelectByIndex(SelectType.Select);
				parametersGroupBox.Content = ucSelectByIndex;
			}
			else if (actionInfo.ActionId == ActionIds.SelectByText)
			{
				if (this.action.Element.ControlType == ControlType.ComboBox || 
					this.action.Element.ControlType == ControlType.Tab)
				{
					ucSelectByText = new UserControlSelectByText(false);
				}
				else if (this.action.Element.ControlType == ControlType.List)
				{
					ucSelectByText = new UserControlSelectByText(true);
				}
				parametersGroupBox.Content = ucSelectByText;
			}
			else if (actionInfo.ActionId == ActionIds.Scroll)
			{
				ucScroll = new UserControlScroll();
				parametersGroupBox.Content = ucScroll;
			}
			else if (actionInfo.ActionId == ActionIds.AddToSelection)
			{
				if (this.action.Element.ControlType == ControlType.DataGrid || 
					this.action.Element.ControlType == ControlType.List)
				{
					ucSelectByIndex = new UserControlSelectByIndex(SelectType.AddToSelection);
					parametersGroupBox.Content = ucSelectByIndex;
				}
			}
			else if (actionInfo.ActionId == ActionIds.RemoveFromSelection)
			{
				if (this.action.Element.ControlType == ControlType.DataGrid || 
					this.action.Element.ControlType == ControlType.List)
				{
					ucSelectByIndex = new UserControlSelectByIndex(SelectType.RemoveFromSelection);
					parametersGroupBox.Content = ucSelectByIndex;
				}
			}
			else if (actionInfo.ActionId == ActionIds.SelectText)
			{
				ucSetText = new UserControlSetText(true, true);
				parametersGroupBox.Content = ucSetText;
			}
			else if (actionInfo.ActionId == ActionIds.AddToSelectionByText)
			{
				ucSelectByText = new UserControlSelectByText(true, SelectType.AddToSelection);
				parametersGroupBox.Content = ucSelectByText;
			}
			else if (actionInfo.ActionId == ActionIds.RemoveFromSelectionByText)
			{
				ucSelectByText = new UserControlSelectByText(true, SelectType.RemoveFromSelection);
				parametersGroupBox.Content = ucSelectByText;
			}
			else if (actionInfo.ActionId == ActionIds.Value)
			{
				ucSetValue = new UserControlSetValue(this.action.Element);
				parametersGroupBox.Content = ucSetValue;
			}
			else if (actionInfo.ActionId == ActionIds.Move)
			{
				ucMouseCoordinates = new UserControlMouseCoordinates(CoordinatesType.Move, actionInfo.ActionId, false, this.action.Element);
				parametersGroupBox.Content = ucMouseCoordinates;
			}
			else if (actionInfo.ActionId == ActionIds.MoveOffset)
			{
				ucMouseCoordinates = new UserControlMouseCoordinates(CoordinatesType.MoveOffset, actionInfo.ActionId, false);
				parametersGroupBox.Content = ucMouseCoordinates;
			}
			else if (actionInfo.ActionId == ActionIds.Resize)
			{
				ucMouseCoordinates = new UserControlMouseCoordinates(CoordinatesType.Resize, actionInfo.ActionId, false);
				parametersGroupBox.Content = ucMouseCoordinates;
			}
			else if (actionInfo.ActionId == ActionIds.WindowWidth || actionInfo.ActionId == ActionIds.WindowHeight)
			{
				ucSetValue = new UserControlSetValue(this.action.Element, actionInfo.ActionId);
				parametersGroupBox.Content = ucSetValue;
			}
			else
			{
				parametersGroupBox.Content = null;
			}
			
			if (this.action.ActionId == actionInfo.ActionId)
			{
				if (!tabSpecific.IsSelected || 
					(tabSpecific.IsSelected && this.action.Element.ControlType == this.initialControlType))
				{
					IParameters iparameters = parametersGroupBox.Content as IParameters;
					if (iparameters != null)
					{
						iparameters.Init(this.action.Parameters);
					}
				}
			}
		}
		
		private void ResetUserControls()
		{
			ucMouseCoordinates = null;
			ucMouseTicks = null;
			ucSendKeys = null;
			ucPressKey = null;
			ucCapture = null;
			ucSelectDate = null;
			ucCheckBox = null;
			ucSetText = null;
			ucSelectByIndex = null;
			ucSelectByText = null;
			ucScroll = null;
			ucSetValue = null;
			ucKeysCombination = null;
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
			else if (ucCapture != null)
			{
				isValid = ucCapture.ValidateParams(this.action);
			}
			else if (ucSelectDate != null)
			{
				isValid = ucSelectDate.ValidateParams(this.action);
			}
			else if (ucCheckBox != null)
			{
				isValid = ucCheckBox.ValidateParams(this.action);
			}
			else if (ucSetText != null)
			{
				isValid = ucSetText.ValidateParams(this.action);
			}
			else if (ucSelectByIndex != null)
			{
				isValid = ucSelectByIndex.ValidateParams(this.action);
			}
			else if (ucSelectByText != null)
			{
				isValid = ucSelectByText.ValidateParams(this.action);
			}
			else if (ucScroll != null)
			{
				isValid = ucScroll.ValidateParams(this.action);
			}
			else if (ucSetValue != null)
			{
				isValid = ucSetValue.ValidateParams(this.action);
			}
			else if (ucKeysCombination != null)
			{
				isValid = ucKeysCombination.ValidateParams(this.action);
			}
			
			if (isValid == true)
			{
				this.action.ActionId = currentActionId;
				return true;
			}
			return false;
		}
		
		private Action action = null;
		private ControlType initialControlType = ControlType.None;
		
		private ObservableCollection<ActionInfo> specificActions = new ObservableCollection<ActionInfo>();
		private List<ActionInfo> mouseActions = new List<ActionInfo>();
		private List<ActionInfo> keyboardActions = new List<ActionInfo>();
		private List<ActionInfo> otherActions = new List<ActionInfo>();
		private List<ActionInfo> simulateActions = new List<ActionInfo>();
		
		private ActionIds currentActionId = ActionIds.None;
		
		private UserControlMouseCoordinates ucMouseCoordinates = null;
		private UserControlMouseTicks ucMouseTicks = null;
		private UserControlSendKeys ucSendKeys = null;
		private UserControlPressKey ucPressKey = null;
		private UserControlCapture ucCapture = null;
		private UserControlSelectDate ucSelectDate = null;
		private UserControlCheckBox ucCheckBox = null;
		private UserControlSetText ucSetText = null;
		private UserControlSelectByIndex ucSelectByIndex = null;
		private UserControlSelectByText ucSelectByText = null;
		private UserControlScroll ucScroll = null;
		private UserControlSetValue ucSetValue = null;
		private UserControlKeysCombination ucKeysCombination = null;
    }
	
	public class ActionInfo
	{
		public string Description { get; set; }
		public ActionIds ActionId { get; set; }
		public string GroupName { get; set; }
	}
}
