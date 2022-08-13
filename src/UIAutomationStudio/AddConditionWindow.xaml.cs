using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for AddConditionWindow.xaml
    /// </summary>
    public partial class AddConditionWindow : Window
    {
        public AddConditionWindow(ConditionalAction conditionalAction, Task task, 
			bool edit = false, bool isInsideLoop = false)
        {
            InitializeComponent();
			
			this.Task = task;
			
			CommandBinding cb = new CommandBinding(MyCommands.EvaluatePropertyCommand, (sender, e) => 
				{
					try
					{
						TryEvaluateProperty();
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, "Evaluate property failed: " + ex.Message);
					}
				},
				(sender, e) => e.CanExecute = listViewAvailable.SelectedItem != null);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.EvaluateConditionCommand, (sender, e) =>
				{
					try
					{
						TryEvaluateCondition();
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, "Evaluate condition failed: " + ex.Message);
					}
				},
				(sender, e) => e.CanExecute = listViewAvailable.SelectedItem != null);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.DuplicateConditionCommand, (sender, e) =>
				{
					try
					{
						TryDuplicateCondition();
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, "Duplicate condition failed: " + ex.Message);
					}
				}, 
				(sender, e) => e.CanExecute = listViewAvailable.SelectedItem != null);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.ConditionPropertiesCommand, (sender, e) =>
				{
					try
					{
						TryConditionProperties();
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, "Condition properties failed: " + ex.Message);
					}
				},
				(sender, e) => e.CanExecute = listViewAvailable.SelectedItem != null);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.AddConditionCommand, (sender, e) => 
				{
					try
					{
						TryAddCondition();
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, "Add condition failed: " + ex.Message);
					}
				}, 
				(sender, e) => e.CanExecute = true);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.EditConditionCommand, (sender, e) => 
				{
					try
					{
						TryOnEditCondition();
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, "Edit condition failed: " + ex.Message);
					}
				}, 
				(sender, e) => e.CanExecute = listViewAvailable.SelectedItem != null);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.DeleteConditionCommand, (sender, e) => 
				{
					try
					{
						TryDeleteCondition();
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, "Delete condition failed: " + ex.Message);
					}
				}, 
				(sender, e) => e.CanExecute = listViewAvailable.SelectedItem != null);
			this.CommandBindings.Add(cb);
			
			this.conditionalAction = conditionalAction;
			
			listViewAvailable.ItemsSource = this.conditionalAction.ConditionWrappers;
			
			this.edit = edit;
			this.isInsideLoop = isInsideLoop;
			
			if (isInsideLoop == true)
			{
				spName.Visibility = Visibility.Hidden;
				
				if (edit == false)
				{
					this.Title = "Create Conditional Statement";
					txbTitle.Text = "Add one or more Conditions to create a Conditional Statement";
				}
				else
				{
					this.Title = "Edit Conditional Statement";
					txbTitle.Text = "Add, edit or remove Conditions";
				}
			}
			else
			{
				if (edit == false)
				{
					this.Title = "Add a new Conditional Action";
					txtName.Text = "Conditional " + this.Task.ConditionalCount;
				}
				else
				{
					this.Title = "Edit the Conditional Action";
					this.txbTitle.Text = "Edit Conditional Action";
					txtName.Text = conditionalAction.Name;
				}
				
				txtName.Focus();
				txtName.SelectAll();
			}
		}
		
		private void TryEvaluateProperty()
		{
			ConditionWrapper selectedConditionWrapper = listViewAvailable.SelectedItem as ConditionWrapper;
			if (selectedConditionWrapper == null)
			{
				MessageBox.Show(this, "Please select a condition");
				return;
			}
			
			MainWindow.EvaluateProperty(selectedConditionWrapper.Condition);
		}
		
		private void TryEvaluateCondition()
		{
			ConditionWrapper selectedConditionWrapper = listViewAvailable.SelectedItem as ConditionWrapper;
			if (selectedConditionWrapper == null)
			{
				MessageBox.Show(this, "Please select a condition");
				return;
			}
			
			bool? val = selectedConditionWrapper.Condition.Evaluate();
			if (val != null)
			{
				MessageBox.Show("Condition evaluates to: " + val.ToString());
			}
		}
		
		private void TryDuplicateCondition()
		{
			ConditionWrapper selectedConditionWrapper = listViewAvailable.SelectedItem as ConditionWrapper;
			if (selectedConditionWrapper == null)
			{
				MessageBox.Show(this, "Please select a condition");
				return;
			}
			
			ConditionWrapper newConditionWrapper = new ConditionWrapper();
			selectedConditionWrapper.DeepCopy(newConditionWrapper);
			
			if (newConditionWrapper.LogicalOp == LogicalOp.None)
			{
				LogicalOpWindow logicalOpWindow = new LogicalOpWindow();
				logicalOpWindow.Owner = this;
				if (logicalOpWindow.ShowDialog() == true)
				{
					newConditionWrapper.LogicalOp = logicalOpWindow.LogicalOp;
				}
				else
				{
					return;
				}
			}
			
			this.conditionalAction.ConditionWrappers.Add(newConditionWrapper);
			listViewAvailable.Items.Refresh();
		}
		
		private void TryConditionProperties()
		{
			ConditionWrapper selectedConditionWrapper = listViewAvailable.SelectedItem as ConditionWrapper;
			if (selectedConditionWrapper == null)
			{
				MessageBox.Show(this, "Please select a condition");
				return;
			}
			
			Element element = null;
			if (selectedConditionWrapper.Condition != null && 
				selectedConditionWrapper.Condition.Variable != null)
			{
				element = selectedConditionWrapper.Condition.Variable.Element;
			}
			
			HelpMessages.Show(MessageId.Properties);
			
			PropertiesWindow window = new PropertiesWindow(element) { Task = this.Task };
			window.Owner = this;
			window.ShowDialog();
			
			if (window.HasChanged == true)
			{
				this.listViewAvailable.Items.Refresh();
			}
		}
		
		private void TryAddCondition()
		{
			LogicalOp logicalOp = LogicalOp.None;
					
			if (this.conditionalAction.ConditionWrappers.Count > 0)
			{
				LogicalOpWindow logicalOpWindow = new LogicalOpWindow();
				logicalOpWindow.Owner = this;
				if (logicalOpWindow.ShowDialog() == true)
				{
					logicalOp = logicalOpWindow.LogicalOp;
				}
				else
				{
					return;
				}
			}
				
			Condition condition = new Condition();
			
			AddVariableWindow window = new AddVariableWindow(condition);
			window.Owner = this;
			if (window.ShowDialog() == true)
			{
				ConditionWrapper conditionWrapper = new ConditionWrapper() { LogicalOp = logicalOp, 
					Condition = condition };
				
				this.conditionalAction.ConditionWrappers.Add(conditionWrapper);
				
				this.listViewAvailable.Items.Refresh();
			}
		}
		
		private void TryDeleteCondition()
		{
			ConditionWrapper selectedConditionWrapper = listViewAvailable.SelectedItem as ConditionWrapper;
			if (selectedConditionWrapper == null)
			{
				MessageBox.Show(this, "Please select a condition");
				return;
			}
					
			MessageBoxResult mbResult = MessageBox.Show(this, 
				"Are you sure you want to delete this Condition?", "", MessageBoxButton.YesNoCancel);
				
			if (mbResult == MessageBoxResult.Yes)
			{
				this.conditionalAction.ConditionWrappers.Remove(selectedConditionWrapper);
				this.listViewAvailable.Items.Refresh();
			}
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			if (isInsideLoop == false)
			{
				HelpMessages.Show(MessageId.AddConditionalAction);
			}
		}
		
		private void TryOnEditCondition()
		{
			ConditionWrapper selectedConditionWrapper = listViewAvailable.SelectedItem as ConditionWrapper;
			if (selectedConditionWrapper == null)
			{
				MessageBox.Show(this, "Please select a condition");
				return;
			}
			
			if (selectedConditionWrapper.LogicalOp != LogicalOp.None)
			{
				// edit LogicalOp
				LogicalOpWindow logicalOpWindow = new LogicalOpWindow(selectedConditionWrapper.LogicalOp);
				logicalOpWindow.Owner = this;
				if (logicalOpWindow.ShowDialog() == true)
				{
					selectedConditionWrapper.LogicalOp = logicalOpWindow.LogicalOp;
					listViewAvailable.Items.Refresh();
				}
			}
			
			AddVariableWindow window = new AddVariableWindow(selectedConditionWrapper.Condition);
			window.Owner = this;
			
			if (window.ShowDialog() == true)
			{
				listViewAvailable.Items.Refresh();
			}
		}
		
		private void OnDoubleClick(object sender, RoutedEventArgs e)
		{
			try
			{
				TryOnEditCondition();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Edit condition failed: " + ex.Message);
			}
		}
		
		private void OnOK(object sender, RoutedEventArgs e)
		{
			if (this.conditionalAction.ConditionWrappers.Count == 0)
			{
				MessageBox.Show(this, "Please add at least one Condition");
				return;
			}
			
			this.conditionalAction.Name = txtName.Text;
			this.DialogResult = true;
			this.Close();
		}
		
		private ConditionalAction conditionalAction = null;
		private bool edit = false;
		private bool isInsideLoop = false;
		
		public Task Task { get; set; }
	}
}