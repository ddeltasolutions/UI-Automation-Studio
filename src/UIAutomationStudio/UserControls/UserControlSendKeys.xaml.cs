using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlSendKeys.xaml
    /// </summary>
    public partial class UserControlSendKeys : UserControl, IParameters
    {
        public UserControlSendKeys()
        {
            InitializeComponent();
        }
		
		public bool ValidateParams(Action action)
		{
			if (txtTextToSend.Text == "")
			{
				MessageBox.Show(Window.GetWindow(this), "Text cannot be empty");
				txtTextToSend.Focus();
				return false;
			}
			
			action.Parameters = new List<object>() { txtTextToSend.Text };
			return true;
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null || parameters.Count != 1)
			{
				return;
			}
			
			txtTextToSend.Text = parameters[0].ToString();
		}
    }
}
