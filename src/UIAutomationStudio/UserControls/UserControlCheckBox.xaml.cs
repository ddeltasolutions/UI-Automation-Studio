using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControlCheckBox : UserControl, IParameters
    {
        public UserControlCheckBox()
        {
            InitializeComponent();
        }
		
		public bool ValidateParams(Action action)
		{
			if (cmbStates.SelectedItem == null)
			{
				MessageBox.Show(Window.GetWindow(this), "Please specify a checked state");
				return false;
			}
			
			ComboBoxItem selectedItem = cmbStates.SelectedItem as ComboBoxItem;
			if (selectedItem == null)
			{
				return false;
			}
			
			action.Parameters = new List<object>();
			action.Parameters.Add(selectedItem.Content.ToString() == "Checked" ? true : false);
			
			return true;
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null || parameters.Count != 1)
			{
				return;
			}
			
			bool ischecked = (bool)parameters[0];
			cmbStates.SelectedIndex = (ischecked ? 0 : 1);
		}
    }
}
