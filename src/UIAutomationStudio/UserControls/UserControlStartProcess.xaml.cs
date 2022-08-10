using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlStartProcess.xaml
    /// </summary>
    public partial class UserControlStartProcess : UserControl, IParameters
    {
        public UserControlStartProcess(ActionIds actionId)
        {
            InitializeComponent();
			
			if (actionId == ActionIds.StartProcessAndWaitForInputIdle)
			{
				txbTitle.Text = "Start Process and Wait for Input Idle - Specify executable file";
			}
        }
		
		private void OnBrowse(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			if (!string.IsNullOrEmpty(txtFile.Text))
			{
				dlg.FileName = txtFile.Text;
			}
			
			dlg.DefaultExt = ".exe";
			dlg.Filter = "Exe files (.exe)|*.exe";
			
			if (dlg.ShowDialog(Window.GetWindow(this)) == true)
			{
				txtFile.Text = dlg.FileName;
			}
		}
		
		private void OnCheckParameters(object sender, RoutedEventArgs e)
		{
			txtParameters.IsEnabled = (chkParameters.IsChecked == true);
		}
		
		public bool ValidateParams(Action action)
		{
			var window = Window.GetWindow(this);
			
			if (txtFile.Text == "")
			{
				MessageBox.Show(window, "File name cannot be empty");
				return false;
			}
			
			action.Parameters = new List<object>() { txtFile.Text };
			
			if (chkParameters.IsChecked == true)
			{
				if (txtParameters.Text == "")
				{
					MessageBox.Show(window, "Please specify parameters for the executable file");
					return false;
				}
			
				action.Parameters.Add(txtParameters.Text);
			}
			
			return true;
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null || parameters.Count == 0)
			{
				return;
			}
			
			txtFile.Text = parameters[0].ToString();
			
			if (parameters.Count >= 2)
			{
				chkParameters.IsChecked = true;
				txtParameters.IsEnabled = true;
				txtParameters.Text = parameters[1].ToString();
			}
		}
    }
}
