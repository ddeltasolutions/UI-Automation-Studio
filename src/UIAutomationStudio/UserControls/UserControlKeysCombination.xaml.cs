using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlKeysCombination.xaml
    /// </summary>
    public partial class UserControlKeysCombination : UserControl, IParameters
    {
		public UserControlKeysCombination()
        {
            InitializeComponent();
			
			string[] keysCombinations = new string[] { "Ctrl+C", "Ctrl+V", "Ctrl+X", "Ctrl+Z", "Ctrl+Y", "Ctrl+A", 
				"Ctrl+S", "Ctrl+F", "Ctrl+N", "Ctrl+O", "Ctrl+P", "Alt+F4" };
			cmbKeys.ItemsSource = keysCombinations; //virtualKeys;
        }
		
		public bool ValidateParams(Action action)
		{
			var window = Window.GetWindow(this);
			
			if (cmbKeys.SelectedItem == null)
			{
				MessageBox.Show(window, "Please select a combination");
				cmbKeys.Focus();
				return false;
			}
			
			action.Parameters = new List<object>() { cmbKeys.SelectedItem };
			return true;
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null || parameters.Count == 0)
			{
				return;
			}
			
			cmbKeys.SelectedItem = parameters[0];
		}
    }
}
