using System;
using System.Windows;

namespace UIAutomationStudio
{
	public partial class MainWindow : Window
	{
		private Action actionInClipboard = null;
		public Action ActionInClipboard
		{
			get
			{
				return this.actionInClipboard;
			}
			set
			{
				if (value == null)
				{
					mainScreen.EnablePaste(false);
				}
				else
				{
					mainScreen.EnablePaste(true);
				}
				
				this.actionInClipboard = value;
			}
		}
		
		private bool cutAction = false;
		private void OnCopyAction(object sender, RoutedEventArgs e)
		{
			try
			{
				TryOnCopyAction();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Copy action failed: " + ex.Message);
			}
		}
		
		private void TryOnCopyAction()
		{
			if (mainScreen.SelectedAction == null)
			{
				return;
			}
			
			// support copy-paste only for normal actions
			if (!(mainScreen.SelectedAction is Action))
			{
				return;
			}
			
			if (this.ActionInClipboard != null && this.ActionInClipboard.GridNode != null)
			{
				this.ActionInClipboard.GridNode.Opacity = 1;
			}
			
			this.ActionInClipboard = new Action();
			
			Action selectedActionNormal = (Action)mainScreen.SelectedAction;
			Action.DeepCopy(selectedActionNormal, this.ActionInClipboard);
			
			cutAction = false;
			
			HelpMessages.Show(MessageId.CopyAction);
		}
		
		private void OnCutAction(object sender, RoutedEventArgs e)
		{
			try
			{
				TryOnCutAction();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Cut action failed: " + ex.Message);
			}
		}
		
		private void TryOnCutAction()
		{
			if (mainScreen.SelectedAction == null)
			{
				return;
			}
			
			// support cut-paste only for normal actions
			if (!(mainScreen.SelectedAction is Action))
			{
				return;
			}
			
			Action selectedActionNormal = (Action)mainScreen.SelectedAction;
			this.ActionInClipboard = selectedActionNormal;
			this.ActionInClipboard.GridNode.Opacity = 0.5;
			cutAction = true;
			
			HelpMessages.Show(MessageId.CopyAction);
		}
		
		private void OnPasteFirst(object sender, RoutedEventArgs e)
		{
			if (this.ActionInClipboard == null)
			{
				MessageBox.Show(this, "You don't have an action in the clipboard");
				return;
			}
			
			try
			{
				PasteFirst();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message);
			}
		}
		
		private	void PasteFirst()
		{
			UndoRedo.AddSnapshot(this.Task);
		
			if (cutAction == true)
			{
				this.Task.RemoveAction(this.ActionInClipboard, false);
			}
			
			this.ActionInClipboard.Previous = null;
			this.ActionInClipboard.Next = this.Task.StartAction;
			
			if (this.Task.StartAction != null)
			{
				this.Task.StartAction.Previous = this.ActionInClipboard;
			}
			
			this.Task.StartAction = this.ActionInClipboard;
			
			this.Task.IsModified = true;
			this.Task.Changed();
			
			if (cutAction == false)
			{
				mainScreen.SelectedAction = this.ActionInClipboard;
			}
			
			this.ActionInClipboard = null;
			
			mainScreen.ScrollToTop();
		}
		
		private void OnPasteLast(object sender, RoutedEventArgs e)
		{
			if (this.ActionInClipboard == null)
			{
				MessageBox.Show(this, "You don't have an action in the clipboard");
				return;
			}
			
			try
			{
				PasteLast();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message);
			}
		}
		
		private void PasteLast()
		{
			UndoRedo.AddSnapshot(this.Task);
		
			if (cutAction == true)
			{
				this.Task.RemoveAction(this.ActionInClipboard, false);
			}
			
			Action lastAction = this.Task.GetLastAction();
			if (lastAction != null)
			{
				lastAction.Next = this.ActionInClipboard;
				this.ActionInClipboard.Previous = lastAction;
				this.ActionInClipboard.Next = null;
			}
			else
			{
				this.Task.StartAction = this.ActionInClipboard;
			}
			
			this.Task.IsModified = true;
			this.Task.Changed();
			
			if (cutAction == false)
			{
				mainScreen.SelectedAction = this.ActionInClipboard;
			}
			
			this.ActionInClipboard = null;
			
			mainScreen.ScrollToBottom();
		}
		
		private void OnPasteAction(object sender, RoutedEventArgs e)
		{
			if (this.ActionInClipboard == null)
			{
				MessageBox.Show(this, "You don't have an action in the clipboard");
				return;
			}
			
			try
			{
				PasteAction();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message);
			}
		}
		
		private void PasteAction()
		{
			if (mainScreen.SelectedArrow == null)
			{
				PasteOptionsWindow pasteOptionsWindow = new PasteOptionsWindow(this.Task.HasAtLeastOneConditional);
				pasteOptionsWindow.Owner = this;
				
				if (pasteOptionsWindow.ShowDialog() == true)
				{
					if (pasteOptionsWindow.IsInsertFirstChecked)
					{
						PasteFirst();
					}
					else
					{
						PasteLast();
					}
				}
				return;
			}
			
			if (cutAction == true && (this.ActionInClipboard == mainScreen.SelectedArrow.PrevAction || 
				this.ActionInClipboard == mainScreen.SelectedArrow.NextAction))
			{
				return;
			}
			
			UndoRedo.AddSnapshot(this.Task);
			
			if (cutAction == true)
			{
				this.Task.RemoveAction(this.ActionInClipboard, false);
			}
			
			ActionBase prevAction = mainScreen.SelectedArrow.PrevAction;
			ActionBase nextAction = mainScreen.SelectedArrow.NextAction;
			
			if (prevAction is Action)
			{
				Action prevActionNormal = (Action)prevAction;
				prevActionNormal.Next = this.ActionInClipboard;
			}
			else if (prevAction is ConditionalAction)
			{
				ConditionalAction prevActionConditional = (ConditionalAction)prevAction;
				if (prevActionConditional.NextOnFalse == nextAction)
				{
					// insert in the left branch
					prevActionConditional.NextOnFalse = this.ActionInClipboard;
				}
				else if (prevActionConditional.NextOnTrue == nextAction)
				{
					// insert in the right branch
					prevActionConditional.NextOnTrue = this.ActionInClipboard;
				}
			}
			this.ActionInClipboard.Previous = prevAction;
			this.ActionInClipboard.Next = nextAction;
			if (mainScreen.SelectedArrow.HasDestinationChanged == false)
			{
				nextAction.Previous = this.ActionInClipboard;
			}
			
			this.Task.IsModified = true;
			
			this.mainScreen.SelectedArrow = null;
			
			this.Task.Changed();
			mainScreen.SelectedAction = this.ActionInClipboard;
			
			this.ActionInClipboard = null;
		}
	}
}