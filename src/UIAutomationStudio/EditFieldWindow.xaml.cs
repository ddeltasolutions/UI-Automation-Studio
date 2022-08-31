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
    /// Interaction logic for EditFieldWindow.xaml
    /// </summary>
    public partial class EditFieldWindow : Window
    {
		public string FieldValue { get; set; }
	
        public EditFieldWindow()
        {
            InitializeComponent();
			
			this.txtField.Focus();
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {
			txtField.Text = this.FieldValue;
        }
		
		private void OnOK(object sender, RoutedEventArgs e)
		{
			this.FieldValue = txtField.Text;
			
			this.DialogResult = true;
			this.Close();
		}
	}
}