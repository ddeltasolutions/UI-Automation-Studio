using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;

namespace UIAutomationStudio
{
	public partial class MainWindow: Window
	{
		private void OnViewWorkflow(object sender, RoutedEventArgs e)
		{
			if (currentScreen == AppScreen.Workflow)
			{
				return;
			}
			
			SwitchToScreen(AppScreen.Workflow);
		}
		
		public void SwitchToScreen(AppScreen screen)
		{
			if (screen == AppScreen.Workflow)
			{
				gridMainScreen.Content = mainScreen;
				currentScreen = AppScreen.Workflow;
			}
			else if (screen == AppScreen.Conditions)
			{
				bool firstTime = false;
				if (variablesScreen == null)
				{
					variablesScreen = new UserControlVariables(this.Task);
					firstTime = true;
				}
				
				gridMainScreen.Content = variablesScreen;
				currentScreen = AppScreen.Conditions;
				
				if (firstTime == true)
				{
					HelpMessages.Show(MessageId.ConditionsScreen);
				}
			}
		}
	
		private void OnViewConditions(object sender, RoutedEventArgs e)
		{
			if (currentScreen == AppScreen.Conditions)
			{
				return;
			}
			
			SwitchToScreen(AppScreen.Conditions);
		}
		
		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
			{
				/*if (currentScreen == AppScreen.Workflow)
				{
					SwitchToScreen(AppScreen.Conditions);
				}
				else if (currentScreen == AppScreen.Conditions)
				{
					SwitchToScreen(AppScreen.Workflow);
				}*/
			}
		}
		
		private void OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete && this.currentScreen == AppScreen.Workflow && this.mainScreen != null && 
				this.mainScreen.SelectedArrow != null)
			{
				MyCommands.DeleteArrowCommand.Execute(null, this);
			}
		}
			
		public static void EvaluateProperty(Condition condition)
		{
			object val = condition.Variable.Evaluate();
			if (val != null)
			{
				string valString = val.ToString();
				if (val.GetType() == typeof(string))
				{
					valString = "\"" + valString + "\"";
				}
				else if (val.GetType() == typeof(bool))
				{
					valString = ((bool)val == true ? "Yes" : "No");
				}
				MessageBox.Show("Property value is now: " + valString);
			}
			else
			{
				MessageBox.Show("Property could not be evaluated. The UI element may not be available anymore.");
			}
		}
		
		public void OnAddConditional(object sender, RoutedEventArgs e)
		{
			if (this.Task == null)
			{
				return;
			}
			
			Arrow selectedArrow = this.mainScreen.SelectedArrow;
			if (selectedArrow == null && this.Task.HasAtLeastOneConditional == true)
			{
				MessageBox.Show(this, "This task contains at least one conditional action. " + 
					"Please select an arrow to specify where to insert the new conditional action.");
				return;
			}
			
			if (selectedArrow != null && !(selectedArrow.NextAction is EndAction))
			{
				MessageBoxResult mbResult = MessageBox.Show(this, 
					"All actions that follow the selected arrow will be deleted. Do you want to continue?", "", 
					MessageBoxButton.YesNoCancel);
					
				if (mbResult != MessageBoxResult.Yes)
				{
					return;
				}
			}
			
			ConditionalAction conditionalAction = new ConditionalAction();
			
			AddConditionWindow window = new AddConditionWindow(conditionalAction) { Task = this.Task };
			window.Owner = this;
			if (window.ShowDialog() == true)
			{
				UndoRedo.AddSnapshot(this.Task);
				
				if (selectedArrow == null) // there is no conditional action in this task
				{
					Action lastAction = this.Task.GetLastAction();
					if (lastAction != null)
					{
						lastAction.Next = conditionalAction;
						conditionalAction.Previous = lastAction;
					}
					else
					{
						this.Task.StartAction = conditionalAction;
					}
				}
				else
				{
					ActionBase actionToDelete = null;
					if (selectedArrow.ArrowType == ArrowType.Normal)
					{
						Action prevAction = (Action)selectedArrow.PrevAction;
						actionToDelete = prevAction.Next;
						prevAction.Next = conditionalAction;
					}
					else if (selectedArrow.ArrowType == ArrowType.Left)
					{
						ConditionalAction prevAction = (ConditionalAction)selectedArrow.PrevAction;
						actionToDelete = prevAction.NextOnFalse;
						prevAction.NextOnFalse = conditionalAction;
					}
					else if (selectedArrow.ArrowType == ArrowType.Right)
					{
						ConditionalAction prevAction = (ConditionalAction)selectedArrow.PrevAction;
						actionToDelete = prevAction.NextOnTrue;
						prevAction.NextOnTrue = conditionalAction;
					}
					
					conditionalAction.Previous = selectedArrow.PrevAction;
					actionToDelete.Deleted();
				}
				this.Task.ConditionalCount++;
				
				this.Task.IsModified = true;
				this.Task.Changed();
				
				if (mainScreen.SelectedArrow == null)
				{
					mainScreen.ScrollToBottom();
				}
				
				mainScreen.SelectedAction = conditionalAction;
				mainScreen.SelectedArrow = null;
			}
		}
		
		private void EditConditionalAction(ConditionalAction conditionalAction)
		{
			ConditionalAction tempConditionalAction = new ConditionalAction();
			conditionalAction.DeepCopy(tempConditionalAction);
			
			AddConditionWindow window = new AddConditionWindow(tempConditionalAction, true) { Task = this.Task };
			window.Owner = this;
			if (window.ShowDialog() == true)
			{
				UndoRedo.AddSnapshot(this.Task);
				tempConditionalAction.DeepCopy(conditionalAction); // copy back with changes
			
				this.Task.IsModified = true;
				this.Task.Changed();
				
				if (mainScreen.SelectedAction != conditionalAction)
				{
					mainScreen.SelectedAction = conditionalAction;
				}
			}
		}
	}
}