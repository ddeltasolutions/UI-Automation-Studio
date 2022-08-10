using System;
using System.Windows;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for LogicalOpWindow.xaml
    /// </summary>
    public partial class LogicalOpWindow : Window
    {
        public LogicalOpWindow(LogicalOp logicalOp = LogicalOp.None)
        {
            InitializeComponent();
			
			this.LogicalOp = logicalOp;
			if (this.LogicalOp == LogicalOp.AND)
			{
				radioAND.IsChecked = true;
			}
			else if (this.LogicalOp == LogicalOp.OR)
			{
				radioOR.IsChecked = true;
			}
		}
		
		private void OnOK(object sender, RoutedEventArgs e)
		{
			if (radioAND.IsChecked == true)
			{
				this.LogicalOp = LogicalOp.AND;
			}
			else if (radioOR.IsChecked == true)
			{
				this.LogicalOp = LogicalOp.OR;
			}
			else
			{
				MessageBox.Show("Please select an option");
				return;
			}
			
			this.DialogResult = true;
			this.Close();
		}
		
		public LogicalOp LogicalOp { get; set; }
	}
}