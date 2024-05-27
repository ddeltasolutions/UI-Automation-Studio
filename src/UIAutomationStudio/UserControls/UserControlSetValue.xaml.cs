using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using UIDeskAutomationLib;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlSetValue.xaml
    /// </summary>
    public partial class UserControlSetValue : UserControl, IParameters
    {
		private ActionIds actionId = ActionIds.Value;
	
        public UserControlSetValue(Element element, ActionIds actionId = ActionIds.Value)
        {
            InitializeComponent();
			
			this.actionId = actionId;
			if (actionId == ActionIds.WindowWidth)
			{
				txbTitle.Text = "Set the width of this window";
				txbLabel.Text = "Width: ";
			}
			else if (actionId == ActionIds.WindowHeight)
			{
				txbTitle.Text = "Set the height of this window";
				txbLabel.Text = "Height: ";
			}
			else if (actionId == ActionIds.Value && (element.ControlType == ControlType.Slider || 
				element.ControlType == ControlType.Spinner || element.ControlType == ControlType.ScrollBar))
			{
				if (element.InnerElement != null)
				{
					double min = 0;
					double max = 0;
					
					try
					{
						if (element.ControlType == ControlType.Slider)
						{
							UIDA_Slider slider = new UIDA_Slider(element.InnerElement);
							min = slider.GetMinimum();
							max = slider.GetMaximum();
						}
						else if (element.ControlType == ControlType.Spinner)
						{
							UIDA_Spinner spinner = new UIDA_Spinner(element.InnerElement);
							min = spinner.GetMinimum();
							max = spinner.GetMaximum();
						}
						else if (element.ControlType == ControlType.ScrollBar)
						{
							UIDA_ScrollBar scrollBar = new UIDA_ScrollBar(element.InnerElement);
							min = scrollBar.GetMinimum();
							max = scrollBar.GetMaximum();
						}	
						
						minTxb.Text = min.ToString();
						maxTxb.Text = max.ToString();
						
						label1.Visibility = Visibility.Visible;
						label2.Visibility = Visibility.Visible;
					}
					catch { }
				}
			}
        }
		
		public bool ValidateParams(Action action)
		{
			var window = Window.GetWindow(this);
			
			if (actionId == ActionIds.Value)
			{
				double val = 0.0;
				if (double.TryParse(txtValue.Text, out val) == false)
				{
					MessageBox.Show(window, "Value must be a number");
					txtValue.Focus();
					txtValue.SelectAll();
					return false;
				}
				
				action.Parameters = new List<object>() { val };
				return true;
			}
			else
			{
				int val = 0;
				if (int.TryParse(txtValue.Text, out val) == false)
				{
					if (actionId == ActionIds.WindowWidth)
					{
						MessageBox.Show(window, "Width must be an integer positive value");
					}
					else
					{
						MessageBox.Show(window, "Height must be an integer positive value");
					}
					txtValue.Focus();
					txtValue.SelectAll();
					return false;
				}
				
				if (val < 0)
				{
					if (actionId == ActionIds.WindowWidth)
					{
						MessageBox.Show(window, "Width must be an integer positive value");
					}
					else
					{
						MessageBox.Show(window, "Height must be an integer positive value");
					}
					txtValue.Focus();
					txtValue.SelectAll();
					return false;
				}
				
				action.Parameters = new List<object>() { val };
				return true;
			}
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null || parameters.Count != 1)
			{
				return;
			}
			
			txtValue.Text = parameters[0].ToString();
		}
    }
}
