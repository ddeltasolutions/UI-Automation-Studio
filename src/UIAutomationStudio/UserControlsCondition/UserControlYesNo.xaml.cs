using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlYesNo.xaml
    /// </summary>
    public partial class UserControlYesNo : UserControl, IValidateCondition
    {
        public UserControlYesNo()
        {
            InitializeComponent();
        }
		
		public void Init(Condition condition)
		{
			if (condition.Values.Count > 0)
			{
				bool val = (bool)condition.Values[0];
				if (val == true)
				{
					chkYes.IsChecked = true;
				}
				else
				{
					chkNo.IsChecked = true;
				}
			}
		}
		
		public bool ValidateParams(Condition condition)
		{
			var window = Window.GetWindow(this);
			
			bool val = false;
			if (chkYes.IsChecked == true)
			{
				val = true;
			}
			else if (chkNo.IsChecked == true)
			{
				val = false;
			}
			else
			{
				MessageBox.Show(window, "Please select Yes or No");
				return false;
			}
			
			condition.Operator = Operator.Equals;
			condition.Values = new List<object>() { val };
			
			return true;
		}
    }
}
