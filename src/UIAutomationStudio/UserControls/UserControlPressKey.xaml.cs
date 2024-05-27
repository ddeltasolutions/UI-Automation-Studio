using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UIDeskAutomationLib;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlPressKey.xaml
    /// </summary>
    public partial class UserControlPressKey : UserControl, IParameters
    {
		private ActionIds actionId;
	
		public UserControlPressKey(ActionIds actionId)
        {
            InitializeComponent();
			
			this.actionId = actionId;
			if (actionId == ActionIds.KeyDown)
			{
				txbTitle.Text = "Key to press";
			}
			else if (actionId == ActionIds.KeyPress)
			{
				txbTitle.Text = "Key to press and release";
			}
			else if (actionId == ActionIds.KeysPress)
			{
				txbTitle.Text = "Keys to press and release";
				
				btnAdd.Visibility = Visibility.Visible;
				lstSelectedKeys.Visibility = Visibility.Visible;
				stackDelButtons.Visibility = Visibility.Visible;
			}
			else if (actionId == ActionIds.KeyUp)
			{
				txbTitle.Text = "Key to release";
			}
			
			VirtualKeys[] virtualKeys = Enum.GetValues(typeof(VirtualKeys)) as VirtualKeys[];
			cmbKeys.ItemsSource = virtualKeys;
        }
		
		private void OnPickKey(object sender, RoutedEventArgs e)
		{
			PickKeyWindow window = new PickKeyWindow();
			window.Owner = Window.GetWindow(this);
			if (window.ShowDialog() == true)
			{
				cmbKeys.SelectedItem = window.SelectedKey;
			}
		}
		
		private void AddKeyToList(object sender, RoutedEventArgs e)
		{
			if (cmbKeys.SelectedItem == null)
			{
				return;
			}
			
			VirtualKeys key = (VirtualKeys)cmbKeys.SelectedItem;
			
			lstSelectedKeys.Items.Add(key);
		}
		
		private void OnDeleteAll(object sender, RoutedEventArgs e)
		{
			lstSelectedKeys.Items.Clear();
		}
		
		private void OnDelete(object sender, RoutedEventArgs e)
		{
			DeleteSelectedItem();
		}
		
		private void DeleteSelectedItem()
		{
			if (lstSelectedKeys.SelectedItem == null)
			{
				return;
			}
			
			lstSelectedKeys.Items.Remove(lstSelectedKeys.SelectedItem);
		}
		
		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				DeleteSelectedItem();
			}
		}
		
		public bool ValidateParams(Action action)
		{
			var window = Window.GetWindow(this);
			
			if (actionId != ActionIds.KeysPress)
			{
				if (cmbKeys.SelectedItem == null)
				{
					MessageBox.Show(window, "You need to select a key");
					cmbKeys.Focus();
					return false;
				}
			}
			else
			{
				if (lstSelectedKeys.Items.Count == 0)
				{
					MessageBox.Show(window, "You need to select at least one key");
					return false;
				}
			}
			
			if (actionId != ActionIds.KeysPress)
			{
				action.Parameters = new List<object>() { cmbKeys.SelectedItem };
			}
			else
			{
				action.Parameters = new List<object>();
				foreach (object item in lstSelectedKeys.Items)
				{
					action.Parameters.Add(item);
				}
			}
			return true;
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null || parameters.Count == 0)
			{
				return;
			}
			
			if (actionId != ActionIds.KeysPress)
			{
				cmbKeys.SelectedItem = (VirtualKeys)parameters[0];
			}
			else
			{
				foreach (object parameter in parameters)
				{
					lstSelectedKeys.Items.Add((VirtualKeys)parameter);
				}
			}
		}
    }
}
