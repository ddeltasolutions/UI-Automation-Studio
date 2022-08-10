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
    /// Interaction logic for AddActionWindow.xaml
    /// </summary>
    public partial class PasteOptionsWindow : Window
    {
        public PasteOptionsWindow(bool hasAtLeastOneConditional = false)
        {
            InitializeComponent();
			IsInsertFirstChecked = false;
			
			if (hasAtLeastOneConditional == true)
			{
				checkBoxInsertFirst.IsChecked = true;
				checkBoxInsertLast.Visibility = Visibility.Hidden;
			}
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {
			
        }
		
		private void OnOK(object sender, RoutedEventArgs e)
		{
			IsInsertFirstChecked = (checkBoxInsertFirst.IsChecked == null ? false : checkBoxInsertFirst.IsChecked.Value);
			this.DialogResult = true;
			this.Close();
		}
		
		public bool IsInsertFirstChecked { get; set; }
	}
}