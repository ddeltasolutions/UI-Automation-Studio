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
    /// Interaction logic for SpeedWindow.xaml
    /// </summary>
    public partial class SpeedWindow : Window
    {
        public SpeedWindow(Speed speed, int speedValue)
        {
            InitializeComponent();
			
			if (speed == Speed.VeryFast)
			{
				radioVeryFast.IsChecked = true;
			}
			else if (speed == Speed.Fast)
			{
				radioFast.IsChecked = true;
			}
			else if (speed == Speed.Slower)
			{
				radioSlower.IsChecked = true;
			}
			else if (speed == Speed.Slow)
			{
				radioSlow.IsChecked = true;
			}
			else if (speed == Speed.Custom)
			{
				radioCustom.IsChecked = true;
				txtSpeedValue.Text = speedValue.ToString();
			}
			
			this.SelectedSpeed = speed;
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }
		
		private void OnChecked(object sender, RoutedEventArgs e)
		{
			RadioButton radioBtn = sender as RadioButton;
			if (radioBtn == null)
			{
				return;
			}
			
			if (radioBtn == radioCustom)
			{
				customValue.Visibility = Visibility.Visible;
			}
			else
			{
				customValue.Visibility = Visibility.Hidden;
			}
		}
		
		private void OnOK(object sender, RoutedEventArgs e)
		{
			if (radioVeryFast.IsChecked == true)
			{
				this.SelectedSpeed = Speed.VeryFast;
			}
			else if (radioFast.IsChecked == true)
			{
				this.SelectedSpeed = Speed.Fast;
			}
			else if (radioSlower.IsChecked == true)
			{
				this.SelectedSpeed = Speed.Slower;
			}
			else if (radioSlow.IsChecked == true)
			{
				this.SelectedSpeed = Speed.Slow;
			}
			else if (radioCustom.IsChecked == true)
			{
				this.SelectedSpeed = Speed.Custom;
				try
				{
					this.SpeedValue = int.Parse(txtSpeedValue.Text);
					if (this.SpeedValue < 0)
					{
						MessageBox.Show(this, "The custom value must be a positive number");
						return;
					}
				}
				catch 
				{
					MessageBox.Show(this, "The custom value is not a valid number");
					return;
				}
			}

			this.DialogResult = true;
			this.Close();
		}
		
		public Speed SelectedSpeed { get; set; }
		public int SpeedValue { get; set; }
	}
}