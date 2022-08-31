using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;

namespace UIAutomationStudio
{
	/// <summary>
	/// Interaction logic for AddActionWindow.xaml
	/// </summary>
	public partial class AddActionWindow : Window
	{
		private UserControlPickElement page1 = null;
		private UserControlSelectAction page2 = null;
		private UserControlSelectGeneralAction page3 = null;
		private int crtPage = 1;
		private Action action = null;
		private bool edit = false;
		
		public bool IsOkPressed { get; set; }
	
        public AddActionWindow(Action action, bool edit = false)
        {
            InitializeComponent();
			
			this.action = action;
			this.edit = edit;
			IsOkPressed = false;
			
			if (action.Element != null || action.ActionId != ActionIds.None)
			{
				this.Title = "Edit Action Wizard";
			}
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {
			bool isGeneral = (this.action.Element == null && this.action.ActionId != ActionIds.None);
			page1 = new UserControlPickElement(this.action.Element, isGeneral);
			myGroupBox.Content = page1;
			
			if (this.edit == false)
			{
				HelpMessages.Show(MessageId.AddAction);
			}
			else
			{
				HelpMessages.Show(MessageId.EditAction);
			}
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
				this.action.Element = page1.SelectedElement;
				
				if (page1.IsGeneralAction == false)
				{
					bool firstTime = false;
					if (page2 == null)
					{
						page2 = new UserControlSelectAction(this.action, 
							(this.action.Element != null ? action.Element.ControlType : ControlType.None));
						page2.RefreshActionsTab();
						
						firstTime = true;
					}
					else if (page1.SelectedElementChanged == true)
					{
						page2.RefreshActionsTab();
						page1.SelectedElementChanged = false;
					}
					
					myGroupBox.Content = page2;
					if (firstTime == true)
					{
						HelpMessages.Show(MessageId.SelectAction);
					}
					crtPage = 2;
				}
				else
				{
					if (page3 == null)
					{
						page3 = new UserControlSelectGeneralAction(this.action);
						page3.RefreshActionsTab();
					}
					
					myGroupBox.Content = page3;
					crtPage = 3;
				}
				
				prevBtn.Visibility = Visibility.Visible;
				nextBtn.Content = "OK";
			}
			else if (crtPage == 2)
			{
				// On OK handler
				if (page1.IsGeneralAction)
				{
					this.action.Element = null;
				}
				
				if (page2.Validate() == true)
				{
					this.IsOkPressed = true;
					this.Close();
				}
			}
			else if (crtPage == 3)
			{
				// On OK handler
				if (page1.IsGeneralAction)
				{
					this.action.Element = null;
				}
				
				if (page3.Validate() == true)
				{
					this.IsOkPressed = true;
					this.Close();
				}
			}
        }
		
		private void OnPrevPage(object sender, RoutedEventArgs e)
		{
			if (crtPage == 2 || crtPage == 3)
			{
				myGroupBox.Content = page1;
				crtPage = 1;
				prevBtn.Visibility = Visibility.Hidden;
				nextBtn.Content = "Next >>";
			}
		}
		
		private void OnCancel(object sender, RoutedEventArgs e)
		{
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
	}
}