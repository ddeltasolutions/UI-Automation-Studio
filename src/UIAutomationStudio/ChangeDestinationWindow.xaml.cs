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
    /// Interaction logic for ChangeDestinationWindow.xaml
    /// </summary>
    public partial class ChangeDestinationWindow : Window
    {
        public ChangeDestinationWindow(UserControlMainScreen mainScreen)
        {
            InitializeComponent();
			this.OkWasPressed = false;
			
			System.Action setOKButton = () => btnOK.IsEnabled = (mainScreen.SelectedAction == null) ? false : true;
			mainScreen.SelectedActionChangedEvent += setOKButton;
			
			setOKButton();
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }
		
		private void OnOK(object sender, RoutedEventArgs e)
		{
			this.OkWasPressed = true;
			this.Close();
		}
		
		private void OnCancel(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		
		public bool OkWasPressed { get; set; }
	}
}