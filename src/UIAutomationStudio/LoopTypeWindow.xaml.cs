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
    /// Interaction logic for LoopTypeWindow.xaml
    /// </summary>
    public partial class LoopTypeWindow : Window
    {
        public LoopTypeWindow()
        {
            InitializeComponent();
			
			this.IsConditional = null;
			this.Count = 0;
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {
			if (this.IsConditional == true)
			{
				radioConditional.IsChecked = true;
			}
			else if (this.IsConditional == false)
			{
				radioCount.IsChecked = true;
			}
			
			OnClickRadio(null, null);
			
			if (this.IsConditional == false)
			{
				// is count type
				txtCount.Text = this.Count.ToString();
			}
        }
		
		private void OnClickRadio(object sender, RoutedEventArgs e)
		{
			this.btnOK.IsEnabled = true;
		
			if (radioCount.IsChecked == true)
			{
				spCount.Visibility = Visibility.Visible;
				txtCount.Focus();
			}
			else
			{
				spCount.Visibility = Visibility.Hidden;
			}
		}
		
		private void OnOK(object sender, RoutedEventArgs e)
		{
			if (radioCount.IsChecked == true)
			{
				int count = 0;
				if (int.TryParse(txtCount.Text, out count) == false || count < 0)
				{
					MessageBox.Show(this, "Please enter a positive integer number");
					txtCount.Focus();
					return;
				}
				
				this.IsConditional = false;
				this.Count = count;
			}
			else
			{
				this.IsConditional = true;
			}
		
			this.DialogResult = true;
			this.Close();
		}
		
		private void OnCancel(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		
		public bool? IsConditional { get; set; }
		public int Count { get; set; }
	}
}