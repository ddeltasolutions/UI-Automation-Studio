using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		public static StopPauseResumeWindow stopPauseWindow = null;
	
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			if (e.Args.Length > 0)
			{
				string xmlFile = e.Args[0];
				Task task = new Task();
				task.OnTaskCompleted += () => 
				{
					if (stopPauseWindow != null)
					{
						stopPauseWindow.Close();
					}
					Shutdown();
				};
				
				try
				{
					if (task.LoadFromXmlFile(xmlFile) == true)
					{
						task.Run();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
				
				stopPauseWindow = new StopPauseResumeWindow(task);
				stopPauseWindow.ShowDialog();
				
				Shutdown();
			}
		}
    }
}
