using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for AddVariableWindow.xaml
    /// </summary>
    public partial class AddVariableWindow : Window
    {
        public AddVariableWindow(Condition condition)
        {
            InitializeComponent();
			
			this.condition = condition;
			tempCondition = new Condition();
			Condition.DeepCopy(condition, tempCondition);
			
			if (condition.Variable.Element != null)
			{
				this.Title = "Edit Condition Wizard";
			}
			
			AddVariableWindow.InitialCondition = this.condition;
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {
			page1 = new UserControlPickElement(tempCondition.Variable.Element, false, true);
			myGroupBox.Content = page1;
			
			HelpMessages.Show(MessageId.AddCondition);
        }
		
		private void OnNextPage(object sender, RoutedEventArgs e)
        {
			if (crtPage == 1)
			{
				if (page1.CheckEmptyElement() == false)
				{
					MessageBox.Show(this, "You need to select an UI Element");
					return;
				}
				
				page1.VerifyControls();
				tempCondition.Variable.Element = page1.SelectedElement;
				
				if (page2 == null)
				{
					page2 = new UserControlSelectProperty(tempCondition.Variable, 
						(condition.Variable.Element != null ? condition.Variable.Element.ControlType : ControlType.None));
					page2.RefreshPropertiesTab();
				}
				else if (page1.SelectedElementChanged == true)
				{
					page2.RefreshPropertiesTab();
					page1.SelectedElementChanged = false;
				}
				
				myGroupBox.Content = page2;
				crtPage = 2;
				
				prevBtn.Visibility = Visibility.Visible;
			}
			else if (crtPage == 2)
			{
				if (page2.Validate() == false)
				{
					return;
				}
				
				if (page3 == null)
				{
					page3 = new UserControlCondition(tempCondition);
				}
				
				if (page2.PropertyIdChanged == true)
				{
					page3.Refresh();
					page2.PropertyIdChanged = false;
				}
				myGroupBox.Content = page3;
				crtPage = 3;
				
				prevBtn.Visibility = Visibility.Visible;
				nextBtn.Content = "OK";
			}
			else if (crtPage == 3)
			{
				// On OK handler
				if (page3.Validate() == false)
				{
					return;
				}
				
				Condition.DeepCopy(this.tempCondition, this.condition);
				
				this.DialogResult = true;
				this.Close();
			}
        }
		
		private void OnPrevPage(object sender, RoutedEventArgs e)
		{
			if (crtPage == 2)
			{
				myGroupBox.Content = page1;
				crtPage = 1;
				prevBtn.Visibility = Visibility.Hidden;
			}
			else if (crtPage == 3)
			{
				myGroupBox.Content = page2;
				crtPage = 2;
				nextBtn.Content = "Next >>";
			}
		}
		
		private void OnCancel(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}
		
		private void OnWindowClosing(object sender, CancelEventArgs e)
		{
			if (crtPage == 1)
			{
				page1.VerifyControls();
			}
			
			Window mainWindow = this.Owner;
			mainWindow.Focus();
			
			UserControlPickElement.Instance = null;
		}
		
		private UserControlPickElement page1 = null;
		private UserControlSelectProperty page2 = null;
		private UserControlCondition page3 = null;
		
		private int crtPage = 1;
		private Condition condition = null;
		private Condition tempCondition = null;
		
		public static Condition InitialCondition = null;
	}
}