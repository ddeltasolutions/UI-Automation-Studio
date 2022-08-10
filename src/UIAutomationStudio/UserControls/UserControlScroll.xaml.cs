using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlScroll.xaml
    /// </summary>
    public partial class UserControlScroll : UserControl, IParameters
    {
        public UserControlScroll()
        {
            InitializeComponent();
        }
		
		public bool ValidateParams(Action action)
		{
			var window = Window.GetWindow(this);
			
			double vertPercent = 0;
			if (double.TryParse(txtVertically.Text, out vertPercent) == false)
			{
				MessageBox.Show(window, "Vertically percent must be a number between 0 and 100");
				txtVertically.Focus();
				txtVertically.SelectAll();
				return false;
			}
			
			if (vertPercent < 0 || vertPercent > 100)
			{
				MessageBox.Show(window, "Vertically percent must be a number between 0 and 100");
				txtVertically.Focus();
				txtVertically.SelectAll();
				return false;
			}
			
			double horizPercent = 0;
			if (double.TryParse(txtHorizontally.Text, out horizPercent) == false)
			{
				MessageBox.Show(window, "Horizontally percent must be a number between 0 and 100");
				txtHorizontally.Focus();
				txtHorizontally.SelectAll();
				return false;
			}
			
			if (horizPercent < 0 || horizPercent > 100)
			{
				MessageBox.Show(window, "Horizontally percent must be a number between 0 and 100");
				txtHorizontally.Focus();
				txtHorizontally.SelectAll();
				return false;
			}
			
			action.Parameters = new List<object>() { vertPercent, horizPercent };
			return true;
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null || parameters.Count < 2)
			{
				return;
			}
			
			txtVertically.Text = parameters[0].ToString();
			txtHorizontally.Text = parameters[1].ToString();
		}
    }
}
