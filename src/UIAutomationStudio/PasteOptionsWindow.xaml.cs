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
		private bool isInsertFirstChecked = false;
		public bool IsInsertFirstChecked 
		{
			get
			{
				return isInsertFirstChecked;
			}
			set
			{
				isInsertFirstChecked = value;
				checkBoxInsertFirst.IsChecked = isInsertFirstChecked;
			}
		}
	
        public PasteOptionsWindow(bool hasAtLeastOneConditional = false)
        {
            InitializeComponent();
			
			if (hasAtLeastOneConditional == true)
			{
				checkBoxInsertFirst.IsChecked = true;
				checkBoxInsertLast.Visibility = Visibility.Hidden;
			}
		}
		
		public void UseItForNewAction()
		{
			Title = "New Action Options";
			txtMsg.Text = "action in task?";
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {
			
        }
		
		private void OnOK(object sender, RoutedEventArgs e)
		{
			isInsertFirstChecked = (checkBoxInsertFirst.IsChecked == null ? false : checkBoxInsertFirst.IsChecked.Value);
			this.DialogResult = true;
			this.Close();
		}
	}
}