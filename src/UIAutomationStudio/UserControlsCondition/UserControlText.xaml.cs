using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlText.xaml
    /// </summary>
    public partial class UserControlText : UserControl, IValidateCondition
    {
        public UserControlText()
        {
            InitializeComponent();
        }
		
		private void OnLikeChecked(object sender, RoutedEventArgs e)
		{
			txbWildcards.Visibility = Visibility.Visible;
		}
		
		private void OnLikeUnchecked(object sender, RoutedEventArgs e)
		{
			txbWildcards.Visibility = Visibility.Hidden;
		}
		
		private void OnDeny(object sender, RoutedEventArgs e)
		{
			if (chkDeny.IsChecked == true)
			{
				radioEquals.Content = "Not equal to";
				radioStartsWith.Content = "Doesn't Start With";
				radioEndsWith.Content = "Doesn't End With";
				radioContains.Content = "Doesn't Contain";
				radioLike.Content = "Not Like";
			}
			else
			{
				radioEquals.Content = "Equals to";
				radioStartsWith.Content = "Starts With";
				radioEndsWith.Content = "Ends With";
				radioContains.Content = "Contains";
				radioLike.Content = "Like";
			}
		}
		
		public void Init(Condition condition)
		{
			if (condition.Operator == Operator.Equals)
			{
				radioEquals.IsChecked = true;
			}
			else if (condition.Operator == Operator.StartsWith)
			{
				radioStartsWith.IsChecked = true;
			}
			else if (condition.Operator == Operator.EndsWith)
			{
				radioEndsWith.IsChecked = true;
			}
			else if (condition.Operator == Operator.Contains)
			{
				radioContains.IsChecked = true;
			}
			else if (condition.Operator == Operator.Like)
			{
				radioLike.IsChecked = true;
			}
			
			if (condition.Values.Count >= 2)
			{
				string text = condition.Values[0] as string;
				bool caseSensitive = (bool)(condition.Values[1]);
				
				txtValue.Text = text;
				chkCaseSensitive.IsChecked = caseSensitive;
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
			else if (radioStartsWith.IsChecked == true)
			{
				condition.Operator = Operator.StartsWith;
			}
			else if (radioEndsWith.IsChecked == true)
			{
				condition.Operator = Operator.EndsWith;
			}
			else if (radioContains.IsChecked == true)
			{
				condition.Operator = Operator.Contains;
			}
			else if (radioLike.IsChecked == true)
			{
				condition.Operator = Operator.Like;
			}
			else
			{
				MessageBox.Show(window, "Please select one option");
				return false;
			}
			
			if (radioEquals.IsChecked == false && txtValue.Text == "")
			{
				MessageBox.Show(window, "Please enter a text");
				txtValue.Focus();
				return false;
			}
			
			condition.Values = new List<object>() { txtValue.Text, chkCaseSensitive.IsChecked };
			condition.Deny = chkDeny.IsChecked != null ? chkDeny.IsChecked.Value : false;
			
			return true;
		}
    }
}
