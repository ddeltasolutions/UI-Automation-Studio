using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Controls;
using UIAutomationClient;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for SelectActionWindow.xaml
    /// </summary>
    public partial class SelectActionWindow : Window
    {
		public bool OkWasPressed { get; set; }
		private UserControlMainScreen mainScreen = null;
	
        public SelectActionWindow(UserControlMainScreen mainScreen, bool selectEndAction = false, 
			Action actionToSelect = null)
        {
            InitializeComponent();
			
			if (selectEndAction == true)
			{
				this.Title = "Select the END ACTION of the Loop";
				txbDesc.Text = "Keeping this window open, select the END ACTION of the Loop and press OK.";
			}
			this.OkWasPressed = false;
			
			setOKButton = () => 
			{
				ActionBase selectedAction = mainScreen.SelectedAction;
				if (selectedAction != null)
				{
					if (selectedAction is ConditionalAction)
					{
						MessageBox.Show(this, "Conditional type not supported");
						return;
					}
					else if (selectedAction is EndAction)
					{
						MessageBox.Show(this, "End type not supported");
						return;
					}
					
					this.SelectedAction = (Action)selectedAction;
				}
				else
				{
					this.SelectedAction = null;
				}
				
				btnOK.IsEnabled = (selectedAction == null) ? false : true;
			};
			
			mainScreen.SelectedActionChangedEvent += setOKButton;
			this.mainScreen = mainScreen;
			
			if (actionToSelect != null)
			{
				if (this.mainScreen.SelectedAction == actionToSelect)
				{
					this.mainScreen.SelectedAction = null;
				}
				this.mainScreen.SelectedAction = actionToSelect;
			}
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }
		
		private System.Action setOKButton = null;
		
		private void OnOK(object sender, RoutedEventArgs e)
		{
			this.OkWasPressed = true;
			this.Close();
		}
		
		private void OnCancel(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		
		private void OnClosing(object sender, CancelEventArgs e)
		{
			this.mainScreen.SelectedActionChangedEvent -= setOKButton;
		}
		
		private Action selectedAction = null;
		public Action SelectedAction 
		{ 
			get
			{
				return this.selectedAction;
			}
			set
			{
				this.selectedAction = value;
				
				if (this.selectedAction == null)
				{
					line1.Text = "";
					actionDescription.Visibility = Visibility.Hidden;
					return;
				}
				
				line1.Text = this.selectedAction.Description1 + " " + this.selectedAction.Description2;
				Element element = this.selectedAction.Element;
				if (element != null)
				{
					actionDescription.Visibility = Visibility.Visible;
					
					txbType.Text = element.ControlType.ToString();
					txbName.Text = "\"" + element.Name + "\"";
				}
				else
				{
					actionDescription.Visibility = Visibility.Hidden;
				}
			}
		}
	}
}