using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlSelectDate.xaml
    /// </summary>
    public partial class UserControlSelectDate : UserControl, IParameters
    {
        public UserControlSelectDate(string title = null)
        {
            InitializeComponent();
			
			if (title != null)
			{
				txbTitle.Text = title;
			}
        }
		
		public bool ValidateParams(Action action)
		{
			if (datePicker.SelectedDate == null)
			{
				MessageBox.Show(Window.GetWindow(this), "Selected date cannot be empty");
				return false;
			}
			
			action.Parameters = new List<object>() { datePicker.SelectedDate };
			return true;
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null || parameters.Count != 1)
			{
				return;
			}
			
			datePicker.SelectedDate = (DateTime?)parameters[0];
		}
    }
}
