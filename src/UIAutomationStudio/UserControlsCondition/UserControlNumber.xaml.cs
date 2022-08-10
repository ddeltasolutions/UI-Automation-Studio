using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlNumber.xaml
    /// </summary>
    public partial class UserControlNumber : UserControl, IValidateCondition
    {
        public UserControlNumber()
        {
            InitializeComponent();
        }
		
		private void OnRadioSelected(object sender, RoutedEventArgs e)
		{
			RadioButton radioBtn = sender as RadioButton;
			if (radioBtn == null)
			{
				return;
			}
			
			if (radioBtn == radioEquals || radioBtn == radioLess || radioBtn == radioLessOrEqual || 
				radioBtn == radioGreater || radioBtn == radioGreaterOrEqual)
			{
				txbAnd.Visibility = Visibility.Hidden;
				txtSecond.Visibility = Visibility.Hidden;
				txtSecondLabel.Visibility = Visibility.Hidden;
			}
			else
			{
				txbAnd.Visibility = Visibility.Visible;
				txtSecond.Visibility = Visibility.Visible;
				txtSecondLabel.Visibility = Visibility.Visible;
			}
		}
		
		private void OnDeny(object sender, RoutedEventArgs e)
		{
			if (chkDeny.IsChecked == true)
			{
				radioEquals.Content = "Not equal to";
				radioLess.Content = "Not Less than";
				radioLessOrEqual.Content = "Not Less than nor equals to";
				radioGreater.Content = "Not Greater than";
				radioGreaterOrEqual.Content = "Not Greater than nor equals to";
				radioBetween.Content = "Not Between";
				radioOutside.Content = "Not Outside Interval";
			}
			else
			{
				radioEquals.Content = "Equals to";
				radioLess.Content = "Less than";
				radioLessOrEqual.Content = "Less than or equals to";
				radioGreater.Content = "Greater than";
				radioGreaterOrEqual.Content = "Greater than or equals to";
				radioBetween.Content = "Between";
				radioOutside.Content = "Outside Interval";
			}
		}
		
		public void Init(Condition condition)
		{
			if (condition.Operator == Operator.Equals)
			{
				radioEquals.IsChecked = true;
				OnRadioSelected(radioEquals, null);
			}
			else if (condition.Operator == Operator.LessThan)
			{
				radioLess.IsChecked = true;
				OnRadioSelected(radioLess, null);
			}
			else if (condition.Operator == Operator.LessThanOrEquals)
			{
				radioLessOrEqual.IsChecked = true;
				OnRadioSelected(radioLessOrEqual, null);
			}
			else if (condition.Operator == Operator.GreaterThan)
			{
				radioGreater.IsChecked = true;
				OnRadioSelected(radioGreater, null);
			}
			else if (condition.Operator == Operator.GreaterThanOrEquals)
			{
				radioGreaterOrEqual.IsChecked = true;
				OnRadioSelected(radioGreaterOrEqual, null);
			}
			else if (condition.Operator == Operator.Between)
			{
				radioBetween.IsChecked = true;
				OnRadioSelected(radioBetween, null);
			}
			else if (condition.Operator == Operator.Outside)
			{
				radioOutside.IsChecked = true;
				OnRadioSelected(radioOutside, null);
			}
			
			if (condition.Operator == Operator.Between || condition.Operator == Operator.Outside)
			{
				if (condition.Values.Count >= 2)
				{
					double val1 = (double)condition.Values[0];
					double val2 = (double)condition.Values[1];
					
					txtValue1.Text = val1.ToString();
					txtSecond.Text = val2.ToString();
				}
			}
			else
			{
				if (condition.Values.Count >= 1)
				{
					double val = (double)condition.Values[0];
					txtValue1.Text = val.ToString();
				}
			}
			
			if (condition.Deny == true)
			{
				chkDeny.IsChecked = true;
				OnDeny(null, null);
			}
		}
		
		public bool ValidateParams(Condition condition)
		{
			var window = Window.GetWindow(this);
			
			if (radioEquals.IsChecked == true)
			{
				condition.Operator = Operator.Equals;
			}
			else if (radioLess.IsChecked == true)
			{
				condition.Operator = Operator.LessThan;
			}
			else if (radioLessOrEqual.IsChecked == true)
			{
				condition.Operator = Operator.LessThanOrEquals;
			}
			else if (radioGreater.IsChecked == true)
			{
				condition.Operator = Operator.GreaterThan;
			}
			else if (radioGreaterOrEqual.IsChecked == true)
			{
				condition.Operator = Operator.GreaterThanOrEquals;
			}
			else if (radioBetween.IsChecked == true)
			{
				condition.Operator = Operator.Between;
			}
			else if (radioOutside.IsChecked == true)
			{
				condition.Operator = Operator.Outside;
			}
			else
			{
				MessageBox.Show(window, "Please select one option");
				return false;
			}
			
			double val1 = 0.0;
			if (double.TryParse(txtValue1.Text, out val1) == false)
			{
				MessageBox.Show(window, "The value entered must be numeric");
				txtValue1.Focus();
				txtValue1.SelectAll();
				return false;
			}
			
			if (radioBetween.IsChecked == false && radioOutside.IsChecked == false)
			{
				condition.Values = new List<object>() { val1 };
			}
			else
			{
				double val2 = 0.0;
				if (double.TryParse(txtSecond.Text, out val2) == false)
				{
					MessageBox.Show(window, "The value entered in the second field must be numeric");
					txtSecond.Focus();
					txtSecond.SelectAll();
					return false;
				}
				
				condition.Values = new List<object>() { val1, val2 };
			}
			
			condition.Deny = chkDeny.IsChecked != null ? chkDeny.IsChecked.Value : false;
			return true;
		}
    }
}
