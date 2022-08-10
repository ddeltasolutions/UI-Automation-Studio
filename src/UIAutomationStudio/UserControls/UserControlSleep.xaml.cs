using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlSleep.xaml
    /// </summary>
    public partial class UserControlSleep : UserControl, IParameters
    {
        public UserControlSleep()
        {
            InitializeComponent();
        }
		
		public bool ValidateParams(Action action)
		{
			var window = Window.GetWindow(this);
			
			int milliseconds = 0;
			if (int.TryParse(txtMilliseconds.Text, out milliseconds) == false)
			{
				MessageBox.Show(window, "Number of milliseconds must be an integer positive value");
				txtMilliseconds.Focus();
				txtMilliseconds.SelectAll();
				return false;
			}
			
			if (milliseconds < 0)
			{
				MessageBox.Show(window, "Number of milliseconds must be an integer positive value");
				txtMilliseconds.Focus();
				txtMilliseconds.SelectAll();
				return false;
			}
			
			action.Parameters = new List<object>() { milliseconds };
			return true;
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null || parameters.Count == 0)
			{
				return;
			}
			
			txtMilliseconds.Text = parameters[0].ToString();
		}
    }
}
