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
    /// Interaction logic for HelpMessageWindow.xaml
    /// </summary>
    public partial class HelpMessageWindow : Window
    {
		public bool ShowAgain { get; set; }
	
        public HelpMessageWindow(string message)
        {
            InitializeComponent();
			
			txbMessage.Text = message;
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {
			
        }
		
		private void OnOK(object sender, RoutedEventArgs e)
		{
			ShowAgain = chkShowAgain.IsChecked.Value;
			this.DialogResult = true;
			this.Close();
		}
	}
}