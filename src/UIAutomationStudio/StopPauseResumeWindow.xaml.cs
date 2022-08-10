using System;
using System.Windows;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for StopPauseResumeWindow.xaml
    /// </summary>
    public partial class StopPauseResumeWindow : Window
    {
        public StopPauseResumeWindow(Task task)
        {
            InitializeComponent();
			
			double width = System.Windows.SystemParameters.PrimaryScreenWidth;
			this.Left = width - this.Width - 1;
			
			this.task = task;
			
			this.btnStop.Click += (sender, e) => this.task.Stop();
			this.btnPause.Click += (sender, e) =>
			{
				if (this.task.IsPaused == true)
				{
					this.btnPause.Content = "Pause";
					this.task.Resume();
				}
				else
				{
					this.btnPause.Content = "Resume";
					this.task.Pause();
				}
			};
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }
		
		private Task task = null;
	}
}