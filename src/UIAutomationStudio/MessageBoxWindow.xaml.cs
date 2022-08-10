using System;
using System.Windows;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for MessageBoxWindow.xaml
    /// </summary>
    public partial class MessageBoxWindow : Window
    {
        public MessageBoxWindow(string message = "")
        {
            InitializeComponent();
			
			this.txbMessage.Text = message;
		}
		
		public void SetText(string message)
		{
			this.txbMessage.Text = message;
		}
	}
}