using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControlCapture : UserControl, IParameters
    {
        public UserControlCapture(string defaultFileName)
        {
            InitializeComponent();
			
			this.defaultFileName = defaultFileName;
        }
		
		private void OnBrowse(object sender, RoutedEventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			if (!string.IsNullOrEmpty(txtFile.Text))
			{
				dlg.FileName = txtFile.Text;
			}
			else
			{
				dlg.FileName = defaultFileName;
			}
			dlg.DefaultExt = ".jpg";
			dlg.Filter = "Jpg files (.jpg)|*.jpg|Jpeg files (.jpeg)|*.jpeg|Png files (.png)|*.png|Bmp files (.bmp)|*.bmp";
			
			if (dlg.ShowDialog(Window.GetWindow(this)) == true)
			{
				txtFile.Text = dlg.FileName;
			}
		}
		
		public bool ValidateParams(Action action)
		{
			if (txtFile.Text == "")
			{
				MessageBox.Show(Window.GetWindow(this), "File name cannot be empty");
				return false;
			}
			
			action.Parameters = new List<object>() { txtFile.Text };
			return true;
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null || parameters.Count != 1)
			{
				return;
			}
			
			txtFile.Text = parameters[0].ToString();
		}
		
		private string defaultFileName = null;
    }
}
