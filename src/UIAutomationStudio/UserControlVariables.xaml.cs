using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UIAutomationStudio
{
	/// <summary>
	/// Interaction logic for UserControlVariables.xaml
	/// </summary>
	public partial class UserControlVariables : UserControl
	{
		private Task task = null;
	
        public UserControlVariables(Task task)
        {
            InitializeComponent();
			
			this.task = task;
        }
		
		public void ChangeTask(Task task)
		{
			this.task = task;
			
			listViewVariables.Items.Refresh();
		}
		
		private void GoBackToWorkflow(object sender, RoutedEventArgs e)
		{
			MainWindow.Instance.SwitchToScreen(AppScreen.Workflow);
		}
		
		public Condition SelectedCondition
		{
			get
			{
				Condition condition = listViewVariables.SelectedItem as Condition;
				return condition;
			}
			set
			{
				listViewVariables.SelectedItem = value;
				listViewVariables.Focus();
			}
		}
		
		public void Refresh()
		{
			listViewVariables.Items.Refresh();
			listViewVariables.Focus();
		}
		
		private void OnSelectionChanged(object sender, RoutedEventArgs e)
		{
			if (listViewVariables.SelectedItem != null)
			{
				btnEditVariable.IsEnabled = true;
				btnDeleteVariable.IsEnabled = true;
				btnEvaluate.IsEnabled = true;
				btnEvaluateCondition.IsEnabled = true;
				btnDuplicate.IsEnabled = true;
			}
			else
			{
				btnEditVariable.IsEnabled = false;
				btnDeleteVariable.IsEnabled = false;
				btnEvaluate.IsEnabled = false;
				btnEvaluateCondition.IsEnabled = false;
				btnDuplicate.IsEnabled = false;
			}
		}
		
		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete && btnDeleteVariable.IsEnabled == true)
			{
				
			}
		}
		
		private void OnDoubleClick(object sender, RoutedEventArgs e)
		{
			Condition selectedCondition = this.SelectedCondition;
			if (selectedCondition == null)
			{
				return;
			}
		}
    }
}
