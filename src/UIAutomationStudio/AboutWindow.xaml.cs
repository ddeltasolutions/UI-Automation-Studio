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
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
			
			this.Title = "About " + MainWindow.TITLE;
			txbName.Text = MainWindow.TITLE + " 2021 " + MainWindow.VERSION;
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }
	}
}