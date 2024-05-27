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
    /// Interaction logic for InsertConditionalWindow.xaml
    /// </summary>
    public partial class InsertConditionalWindow : Window
    {
        public InsertConditionalWindow()
        {
            InitializeComponent();
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {
			
        }
		
		private void OnOK(object sender, RoutedEventArgs e)
		{
			if (radioDelete.IsChecked == true)
			{
				this.InsertConditional = InsertConditionalEnum.Delete;
			}
			else if (radioTrue.IsChecked == true)
			{
				this.InsertConditional = InsertConditionalEnum.True;
			}
			else if (radioFalse.IsChecked == true)
			{
				this.InsertConditional = InsertConditionalEnum.False;
			}
			else
			{
				MessageBox.Show(this, "Please select an option");
				return;
			}
		
			this.DialogResult = true;
			this.Close();
		}
		
		public InsertConditionalEnum InsertConditional { get; set; }
	}
	
	public enum InsertConditionalEnum
	{
		Delete, True, False
	}
}