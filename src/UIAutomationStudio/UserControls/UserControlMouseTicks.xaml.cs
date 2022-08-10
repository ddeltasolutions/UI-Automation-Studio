using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControlMouseTicks : UserControl, IParameters
    {
        public UserControlMouseTicks(ActionIds actionId)
        {
            InitializeComponent();
			
			if (actionId == ActionIds.MouseScrollUp)
			{
				txbTitle.Text += " for Scroll Up";
			}
			else if (actionId == ActionIds.MouseScrollDown)
			{
				txbTitle.Text += " for Scroll Down";
			}
        }
		
		public bool ValidateParams(Action action)
		{
			var window = Window.GetWindow(this);
			
			uint ticks = 0;
			if (uint.TryParse(txtTicks.Text, out ticks) == false)
			{
				MessageBox.Show(window, "Number of ticks must be an integer positive value");
				txtTicks.Focus();
				txtTicks.SelectAll();
				return false;
			}
			
			if (ticks < 0)
			{
				MessageBox.Show(window, "Number of ticks must be an integer positive value");
				txtTicks.Focus();
				txtTicks.SelectAll();
				return false;
			}
			
			action.Parameters = new List<object>() { ticks };
			return true;
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null || parameters.Count != 1)
			{
				return;
			}
			
			txtTicks.Text = parameters[0].ToString();
		}
    }
}
