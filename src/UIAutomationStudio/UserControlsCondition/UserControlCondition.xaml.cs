using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlCondition.xaml
    /// </summary>
    public partial class UserControlCondition : UserControl
    {
		private Condition condition = null;
		private bool guard = false;
	
        public UserControlCondition(Condition condition)
        {
			guard = true;
            InitializeComponent();
			
			this.condition = condition;
			
			guard = false;
        }
		
		public void Refresh()
		{
			propertyName.Text = condition.Variable.PropertyId.ToString();
			
			if (condition.Variable.PropertyType != PropertyType.Text)
			{
				txbTreat.Visibility = Visibility.Hidden;
				cmbTreat.Visibility = Visibility.Hidden;
			}
			else
			{
				txbTreat.Visibility = Visibility.Visible;
				cmbTreat.Visibility = Visibility.Visible;
			}
			
			if (condition.Variable.PropertyType == PropertyType.YesNo)
			{
				condGroupBox.Content = new UserControlYesNo();
			}
			else if (condition.Variable.PropertyType == PropertyType.Number)
			{
				condGroupBox.Content = new UserControlNumber();
			}
			else if (condition.Variable.PropertyType == PropertyType.Text)
			{
				if (AddVariableWindow.InitialCondition.Variable != null && 
				AddVariableWindow.InitialCondition.Variable.Element != null &&
				AddVariableWindow.InitialCondition.Variable.Element.ControlType == this.condition.Variable.Element.ControlType && 
				AddVariableWindow.InitialCondition.Variable.PropertyId == this.condition.Variable.PropertyId && 
				this.condition.Values != null && this.condition.Values.Count > 0)
				{
					Type firstParamType = this.condition.Values[0].GetType();
					guard = true;
					if (firstParamType == typeof(string))
					{
						cmbTreat.SelectedIndex = 0;
						condGroupBox.Content = new UserControlText();
					}
					else if (firstParamType == typeof(double))
					{
						cmbTreat.SelectedIndex = 1;
						condGroupBox.Content = new UserControlNumber();
					}
					else if (firstParamType == typeof(DateTime))
					{
						cmbTreat.SelectedIndex = 2;
						condGroupBox.Content = new UserControlDate();
					}
					guard = false;
				}
				else
				{
					condGroupBox.Content = new UserControlText();
				}
			}
			else if (condition.Variable.PropertyType == PropertyType.Date)
			{
				condGroupBox.Content = new UserControlDate();
			}
			
			if (AddVariableWindow.InitialCondition.Variable != null && 
				AddVariableWindow.InitialCondition.Variable.Element != null &&
				AddVariableWindow.InitialCondition.Variable.Element.ControlType == this.condition.Variable.Element.ControlType && 
				AddVariableWindow.InitialCondition.Variable.PropertyId == this.condition.Variable.PropertyId &&
				this.condition.Values != null && this.condition.Values.Count > 0)
			{
				IValidateCondition iValidCond = condGroupBox.Content as IValidateCondition;
				if (iValidCond == null)
				{
					return;
				}
				
				iValidCond.Init(this.condition);
			}
		}
		
		public bool Validate()
		{
			IValidateCondition iValidCond = condGroupBox.Content as IValidateCondition;
			
			if (iValidCond != null && iValidCond.ValidateParams(this.condition) == true)
			{
				return true;
			}
			
			return false;
		}
		
		private void OnSelectionChanged(object sender, RoutedEventArgs e)
		{
			if (guard == true)
			{
				return;
			}
		
			ComboBoxItem selectedItem = cmbTreat.SelectedItem as ComboBoxItem;
			if (selectedItem == null)
			{
				return;
			}
			
			string selectedItemText = selectedItem.ToString();
			if (selectedItemText.EndsWith("Text"))
			{
				condGroupBox.Content = new UserControlText();
			}
			else if (selectedItemText.EndsWith("Number"))
			{
				condGroupBox.Content = new UserControlNumber();
			}
			else if (selectedItemText.EndsWith("Date"))
			{
				condGroupBox.Content = new UserControlDate();
			}
		}
    }
}
