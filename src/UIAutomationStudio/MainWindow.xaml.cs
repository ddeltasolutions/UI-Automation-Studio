using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using UIAutomationClient;
using Microsoft.Win32;
using System.Diagnostics;
using System.ComponentModel;
using System.Xml;
using System.Windows.Documents;
using System.IO;
using System.Windows.Controls;
using System.Threading;
using System.Linq;
using System.Windows.Media.Imaging;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
			this.Task = null;
			this.Title = MainWindow.TITLE;
			
			AddCommands();
			
			Instance = this;
			
			try
			{
				LoadPreferences();
			}
			catch { }
        }
		
		public static MainWindow Instance = null;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
			
        }
		
		private void OnNewTask(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				TryOnNewTask();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Add new task failed: " + ex.Message);
			}
		}
		
		public void TryOnNewTask()
		{
			if (this.Task != null && this.Task.IsModified)
			{
				MessageBoxResult mbResult = MessageBox.Show(this, "Do you want to save the task before closing?", "", 
					MessageBoxButton.YesNoCancel);
					
				if (mbResult == MessageBoxResult.Yes)
				{
					if (OnSaveTask(false) == false)
					{
						return;
					}
				}
				else if (mbResult == MessageBoxResult.Cancel)
				{
					return;
				}
			}
			
			AddTaskToMruList();
			
			this.Task = new Task();
			UndoRedo.Reset(this.Task);
			if (mainScreen == null)
			{
				ShowMainScreen();
			}
			else
			{
				if (currentScreen != AppScreen.Workflow)
				{
					SwitchToScreen(AppScreen.Workflow);
				}
				variablesScreen = null;
			}
			this.Title = MainWindow.TITLE;
			
			menuItemNewAction.IsEnabled = true;
			menuItemNewConditionalAction.IsEnabled = true;
			btnToolbarAddAction.IsEnabled = true;
			
			HelpMessages.Show(MessageId.NewTask);
		}
		
		private void OnOpenTask(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				TryOnOpenTask();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Open task failed: " + ex.Message);
			}
		}
		
		private void TryOnOpenTask()
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Cmd files (*.cmd)|*.cmd";
			if (ofd.ShowDialog() == true)
			{
				string exeFile = ofd.FileName;
				string xmlFile = Path.GetDirectoryName(exeFile) + "\\" + Path.GetFileNameWithoutExtension(exeFile) + ".xml";
				OpenTask(xmlFile);
			}
		}
		
		public void OpenTask(string xmlFile)
		{
			Task newtask = new Task();
			if (newtask.LoadFromXmlFile(xmlFile) == false)
			{
				return;
			}
			
			if (this.Task != null && CloseTask() == false)
			{
				return;
			}
			
			string exeFile = Path.GetDirectoryName(xmlFile) + "\\" + Path.GetFileNameWithoutExtension(xmlFile) + ".cmd";
			newtask.ExeFile = exeFile;
			
			if (this.Task == null)
			{
				ShowMainScreen();
			}
			else if (currentScreen != AppScreen.Workflow)
			{
				SwitchToScreen(AppScreen.Workflow);
			}
			variablesScreen = null;
			
			this.Task = newtask;
			UndoRedo.Reset(this.Task);
			
			this.Title = MainWindow.TITLE + " - " + exeFile;
			menuItemNewAction.IsEnabled = true;
			menuItemNewConditionalAction.IsEnabled = true;
			btnToolbarAddAction.IsEnabled = true;
		}
		
		private void OnSaveTask(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				OnSaveTask();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Save task failed: " + ex.Message);
			}
		}
		
		private bool OnSaveTask(bool prompt = true)
		{
			if (this.Task == null)
			{
				return false;
			}
			
			bool allFilesPresent = true;
			try
			{
				if (this.Task.ExeFile != null)
				{
					allFilesPresent = allFilesPresent && File.Exists(this.Task.ExeFile);
					if (allFilesPresent == true)
					{
						string xmlFilePath = this.Task.XmlFilePath;
						if (xmlFilePath != null)
						{
							allFilesPresent = allFilesPresent && File.Exists(xmlFilePath);
						}
					}
				}
			}
			catch { }
			
			if (this.Task.IsModified == false && allFilesPresent == true)
			{
				return false;
			}
			
			if (this.Task.ExeFile == null)
			{
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.FileName = "task.cmd";

				dlg.DefaultExt = ".cmd";
				dlg.Filter = "Cmd files (.cmd)|*.cmd";
				
				if (dlg.ShowDialog(this) == true)
				{
					this.Task.ExeFile = dlg.FileName;
				}
				else
				{
					return false;
				}
			}
			
			SaveTask(prompt);
			return true;
		}
		
		private void SaveTask(bool prompt)
		{
			bool bSaved = this.Task.Save();
			
			if (bSaved == true && prompt == true)
			{
				UndoRedo.TaskSaved(this.Task);
			
				this.Title = MainWindow.TITLE + " - " + this.Task.ExeFile;
				MessageBox.Show(this, "Task Saved");
				
				HelpMessages.Show(MessageId.SaveTask);
			}
		}
		
		private void OnSaveAsTask(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				TryOnSaveAsTask();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Save as task failed: " + ex.Message);
			}
		}
		
		private void TryOnSaveAsTask()
		{
			if (this.Task == null)
			{
				return;
			}
			
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Title = "Save As";
			dlg.DefaultExt = ".cmd";
			dlg.Filter = "Cmd files (.cmd)|*.cmd";
			
			if (this.Task.ExeFile == null)
			{
				dlg.FileName = "task.cmd";
			}
			else
			{
				dlg.InitialDirectory = Path.GetDirectoryName(this.Task.ExeFile);
				dlg.FileName = Path.GetFileName(this.Task.ExeFile);
			}
			
			if (dlg.ShowDialog(this) == true)
			{
				this.Task.ExeFile = dlg.FileName;
			}
			else
			{
				return;
			}
			
			SaveTask(true);
		}
		
		private void OnCloseTask(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				TryOnCloseTask();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Close task failed: " + ex.Message);
			}
		}
		
		private void TryOnCloseTask()
		{
			if (CloseTask() == false)
			{
				return;
			}
			
			this.Task = null;
			UndoRedo.Reset(null);
			this.ActionInClipboard = null;
			
			ShowOpenScreen();
			this.Title = MainWindow.TITLE;
		}
		
		// returns false if user cancelled the message box
		private bool CloseTask()
		{
			if (this.Task.IsModified)
			{
				MessageBoxResult mbResult = MessageBox.Show(this, "Do you want to save the task before closing?", "",
					MessageBoxButton.YesNoCancel);
					
				if (mbResult == MessageBoxResult.Yes)
				{
					if (OnSaveTask(false) == false)
					{
						return false;
					}
				}
				else if (mbResult == MessageBoxResult.Cancel)
				{
					return false;
				}
			}
			
			AddTaskToMruList();
			return true;
		}
		
		private void AddTaskToMruList()
		{
			if (this.Task == null || this.Task.ExeFile == null)
			{
				return;
			}
			
			if (MruTasks.Contains(this.Task.ExeFile))
			{
				MruTasks.Remove(this.Task.ExeFile);
			}
			
			MruTasks.Insert(0, this.Task.ExeFile);
			while (MruTasks.Count > 10)
			{
				MruTasks.RemoveAt(MruTasks.Count - 1);
			}
		}
		
		private static List<string> MruTasks = new List<string>();
		
		private void OnExit(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		
		private void OnAddAction(object sender, RoutedEventArgs e)
		{
			try
			{
				TryOnAddAction();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Add action failed: " + ex.Message);
			}
		}
		
		private void TryOnAddAction()
		{
			if (this.Task == null)
			{
				return;
			}
			
			Arrow selectedArrow = this.mainScreen.SelectedArrow;
			if (selectedArrow == null && this.Task.HasAtLeastOneConditional == true)
			{
				MessageBox.Show(this, "This task contains at least one conditional action. " + 
					"Please select an arrow to specify where to insert the new action.");
				return;
			}
			if (selectedArrow != null && selectedArrow.HasDestinationChanged)
			{
				MessageBox.Show(this, "Cannot add a new action on this arrow");
				return;
			}
			
			Action action = new Action();
			
			AddActionWindow window = new AddActionWindow(action);
			window.Owner = this;
			if (window.ShowDialog() == true)
			{
				UndoRedo.AddSnapshot(this.Task);
				
				if (selectedArrow != null)
				{
					ActionBase prevAction = selectedArrow.PrevAction;
					if (prevAction is Action)
					{
						Action prevActionNormal = (Action)prevAction;
						prevActionNormal.Next = action;
						action.Previous = prevAction;
						action.Next = selectedArrow.NextAction;
						selectedArrow.NextAction.Previous = action;
					}
					else if (prevAction is ConditionalAction)
					{
						ConditionalAction prevActionConditional = (ConditionalAction)prevAction;
						if (selectedArrow.ArrowType == ArrowType.Left)
						{
							prevActionConditional.NextOnFalse = action;
						}
						else if (selectedArrow.ArrowType == ArrowType.Right)
						{
							prevActionConditional.NextOnTrue = action;
						}
						
						action.Previous = prevActionConditional;
						action.Next = selectedArrow.NextAction;
						selectedArrow.NextAction.Previous = action;
					}
				}
				else // no arrow is selected
				{
					ActionBase crtAction = this.Task.StartAction;
					if (crtAction == null)
					{
						this.Task.StartAction = action;
					}
					else
					{
						Action crtActionNormal = null;
						
						while (true)
						{
							if (crtAction is Action)
							{
								crtActionNormal = (Action)crtAction;
								if (crtActionNormal.Next == null)
								{
									break;
								}
								crtAction = crtActionNormal.Next;
							}
							else if (crtAction is EndAction)
							{
								break;
							}
						}
						
						if (crtAction is EndAction)
						{
							if (crtActionNormal != null)
							{
								crtActionNormal.Next = action;
								action.Previous = crtActionNormal;
								action.Next = crtAction;
								crtAction.Previous = action;
							}
						}
						else
						{
							// crtAction is the last action
							crtActionNormal.Next = action;
							action.Previous = crtAction;
						}
					}
				}
				
				this.Task.IsModified = true;
				this.Task.Changed();
				
				if (this.mainScreen.SelectedArrow == null)
				{
					mainScreen.ScrollToBottom();
				}
				
				this.mainScreen.SelectedAction = action;
				this.ActionInClipboard = null;
			}
		}
		
		private void OnEditAction(object sender, RoutedEventArgs e)
		{
			try
			{
				ActionBase selectedAction = this.mainScreen.SelectedAction;
				if (selectedAction == null)
				{
					MessageBox.Show(this, "Please select an action to edit");
					return;
				}
				
				OnEditAction(selectedAction);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Edit action failed: " + ex.Message);
			}
		}
		
		private void OnEditAction(ActionBase action)
		{
			if (this.IsTaskRunning == true)
			{
				return;
			}
			
			if (action is Action)
			{
				Action selectedActionNormal = (Action)action;
				EditAction(selectedActionNormal);
			}
			else if (action is ConditionalAction)
			{
				ConditionalAction selectedActionConditional = (ConditionalAction)action;
				EditConditionalAction(selectedActionConditional);
			}
			else if (action is LoopAction)
			{
				LoopAction selectedLoopAction = (LoopAction)action;
				EditLoopAction(selectedLoopAction);
			}
		}
		
		private void EditAction(Action action)
		{
			Action tempAction = new Action();
			action.DeepCopy(tempAction);
		
			AddActionWindow window = new AddActionWindow(tempAction, true);
			window.Owner = this;
			if (window.ShowDialog() == true)
			{
				UndoRedo.AddSnapshot(this.Task);
			
				tempAction.DeepCopy(action);
				this.Task.IsModified = true;
				
				this.Task.Changed();
				
				this.mainScreen.SelectedActionChanged();
			}
		}
		
		private void EditLoopAction(LoopAction loopAction)
		{
			var selectActionWindow = new SelectActionWindow(this.mainScreen, false, loopAction.StartAction);
			selectActionWindow.Owner = this;
			
			selectActionWindow.Closed += (sender2, e2) => 
			{
				if (selectActionWindow.OkWasPressed == false)
				{
					this.mainScreen.SelectedAction = loopAction;
					return;
				}
				
				if (!loopAction.EndAction.IsDescendentOf(selectActionWindow.SelectedAction))
				{
					MessageBox.Show(this, "The End Action must be a descendent of the Start Action. " + 
						"This means there should be a path from Start Action to reach End Action.");
					return;
				}
				
				if (loopAction.StartAction != selectActionWindow.SelectedAction)
				{
					UndoRedo.AddSnapshot(this.Task);
				
					loopAction.StartAction = selectActionWindow.SelectedAction;
				
					this.Task.IsModified = true;
					this.Task.Changed();
				}
				
				var selectEndActionWindow = new SelectActionWindow(this.mainScreen, true, loopAction.EndAction);
				selectEndActionWindow.Owner = this;
				selectEndActionWindow.Closed += (sender3, e3) => 
				{
					if (selectEndActionWindow.OkWasPressed == false)
					{
						this.mainScreen.SelectedAction = loopAction;
						return;
					}
					
					if (!selectEndActionWindow.SelectedAction.IsDescendentOf(loopAction.StartAction))
					{
						MessageBox.Show(this, "The End Action must be a descendent of the Start Action. " + 
							"This means there should be a path from Start Action to reach End Action.");
						return;
					}
					
					if (loopAction.EndAction != selectEndActionWindow.SelectedAction)
					{
						UndoRedo.AddSnapshot(this.Task);
					
						loopAction.EndAction.LoopAction = null;
						loopAction.EndAction = selectEndActionWindow.SelectedAction;
						loopAction.EndAction.LoopAction = loopAction;
						
						this.Task.IsModified = true;
						this.Task.Changed();
					}
					
					this.mainScreen.SelectedAction = loopAction;
					
					bool isConditional = loopAction is LoopConditional;
					LoopTypeWindow loopTypeWindow = new LoopTypeWindow() { IsConditional = isConditional };
					if (loopTypeWindow.IsConditional == false)
					{
						// is count type
						loopTypeWindow.Count = ((LoopCount)loopAction).InitialCount;
					}
					loopTypeWindow.Owner = this;
					if (loopTypeWindow.ShowDialog() != true)
					{
						return;
					}
					
					if (isConditional == true && loopTypeWindow.IsConditional == true)
					{
						LoopConditional loopConditional = (LoopConditional)loopAction;
						ConditionalAction tempConditionalAction = new ConditionalAction();
						loopConditional.ConditionalAction.DeepCopy(tempConditionalAction);
						
						AddConditionWindow addConditionWindow = 
							new AddConditionWindow(tempConditionalAction, this.Task, true, true);
						addConditionWindow.Owner = this;
						
						if (addConditionWindow.ShowDialog() == true)
						{
							UndoRedo.AddSnapshot(this.Task);
						
							tempConditionalAction.DeepCopy(loopConditional.ConditionalAction); // copy back with changes
							
							this.Task.IsModified = true;
							this.Task.Changed();
						}
					}
					if (isConditional == false && loopTypeWindow.IsConditional == false)
					{
						LoopCount loopCount = (LoopCount)loopAction;
						if (loopCount.InitialCount != loopTypeWindow.Count)
						{
							UndoRedo.AddSnapshot(this.Task);
						
							loopCount.InitialCount = loopTypeWindow.Count;
							
							this.Task.IsModified = true;
							this.Task.Changed();
						}
						
						loopCount.Reset();
					}
					if (isConditional == true && loopTypeWindow.IsConditional == false)
					{
						UndoRedo.AddSnapshot(this.Task);
					
						LoopCount newLoopCount = new LoopCount(loopTypeWindow.Count)
							{ StartAction = loopAction.StartAction, EndAction = loopAction.EndAction };
						
						loopAction.EndAction.LoopAction = newLoopCount;
						Task.LoopActions.Remove(loopAction);
						Task.LoopActions.Add(newLoopCount);
						
						this.Task.IsModified = true;
						this.Task.Changed();
						
						this.mainScreen.SelectedAction = newLoopCount;
					}
					if (isConditional == false && loopTypeWindow.IsConditional == true)
					{
						ConditionalAction conditionalAction = new ConditionalAction();
						
						AddConditionWindow addConditionWindow = 
							new AddConditionWindow(conditionalAction, this.Task, false, true);
						addConditionWindow.Owner = this;
						
						if (addConditionWindow.ShowDialog() == true)
						{
							UndoRedo.AddSnapshot(this.Task);
						
							LoopConditional newLoopConditional = new LoopConditional() 
								{ StartAction = loopAction.StartAction, EndAction = loopAction.EndAction, 
								ConditionalAction = conditionalAction };
								
							loopAction.EndAction.LoopAction = newLoopConditional;
							Task.LoopActions.Remove(loopAction);
							Task.LoopActions.Add(newLoopConditional);
							
							this.Task.IsModified = true;
							this.Task.Changed();
							
							this.mainScreen.SelectedAction = newLoopConditional;
						}
					}
				};
				selectEndActionWindow.Show();
			};
			selectActionWindow.Show();
		}
		
		private void OnDeleteAction(object sender, RoutedEventArgs e)
		{
			try
			{
				TryOnDeleteAction();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Delete action failed: " + ex.Message);
			}
		}
		
		private void TryOnDeleteAction()
		{
			ActionBase selectedAction = mainScreen.SelectedAction;
			
			if (selectedAction == null)
			{
				MessageBox.Show(this, "Please select an action to delete");
				return;
			}
			
			string message = null;
			if (selectedAction is Action)
			{
				message = "Do you want to delete the selected action?";
			}
			else if (selectedAction is ConditionalAction)
			{
				message = "The selected action and all the subtree actions will also be deleted. Are you sure you want to do this?";
			}
			else if (selectedAction is LoopAction)
			{
				message = "Do you want to delete the selected loop action?";
			}
			
			if (MessageBox.Show(this, message, "", MessageBoxButton.YesNoCancel) == 
				MessageBoxResult.Yes)
			{
				UndoRedo.AddSnapshot(this.Task);
				
				this.Task.RemoveAction(selectedAction);
				
				this.Task.IsModified = true;
				mainScreen.SelectedAction = null;

				this.Task.Changed();
			}
		}
		
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
		
		private void ShowOpenScreen()
		{
			gridMainScreen.Content = openScreen;
			currentScreen = AppScreen.Intro;
			mainScreen = null;
			variablesScreen = null;
			LoadMruTasks();
		}
		
		private void LoadMruTasks()
		{
			listMruTasks.Items.Clear();
			foreach (string mru in MruTasks)
			{
				Hyperlink link = new Hyperlink(new Run(mru));
				link.Click += OnClickMruTask;
				listMruTasks.Items.Add(link);
			}
		}
		
		private void OnClickMruTask(object sender, RoutedEventArgs e)
		{
			Hyperlink link = sender as Hyperlink;
			if (link == null)
			{
				return;
			}
			
			try
			{
				TryOnClickMruTask(link);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Click MRU task failed: " + ex.Message);
			}
		}
		
		private void TryOnClickMruTask(Hyperlink link)
		{
			if (link.Inlines.Count == 0)
			{
				return;
			}
			
			Run run = link.Inlines.FirstInline as Run;
			string exeFile = run.Text;
			string xmlFile = Path.GetDirectoryName(exeFile) + "\\" + Path.GetFileNameWithoutExtension(exeFile) + ".xml";
			
			OpenTask(xmlFile);
		}
		
		private void OnMRUOpened(object sender, RoutedEventArgs e)
		{
			try
			{
				TryOnMRUOpened();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "MRU menu failed: " + ex.Message);
			}
		}
		
		private void TryOnMRUOpened()
		{
			if (MruTasks.Count == 0)
			{
				return;
			}
		
			menuItemMRU.Items.Clear();
			foreach (string mru in MruTasks)
			{
				MenuItem menuItem = new MenuItem();
				menuItem.Header = mru;
				menuItem.Click += OnClickMruMenuItem;
				menuItemMRU.Items.Add(menuItem);
			}
		}
		
		private void OnClickMruMenuItem(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			
			try
			{
				TryOnClickMruMenuItem(menuItem);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Load MRU task failed: " + ex.Message);
			}
		}
		
		private void TryOnClickMruMenuItem(MenuItem menuItem)
		{
			string exeFile = menuItem.Header.ToString();
			string xmlFile = Path.GetDirectoryName(exeFile) + "\\" + Path.GetFileNameWithoutExtension(exeFile) + ".xml";
			OpenTask(xmlFile);
		}
		
		private object openScreen = null;
		private void ShowMainScreen()
		{
			openScreen = gridMainScreen.Content;
			
			mainScreen = new UserControlMainScreen();
			mainScreen.HideActionsInfoEvent += () => menuItemActionInfo.IsChecked = false;
			
			mainScreen.ActionDoubleClickedEvent += OnEditAction;
			mainScreen.Task = this.Task;
			
			SwitchToScreen(AppScreen.Workflow);
			
			menuItemDeleteAction.IsEnabled = true;
			menuItemEditAction.IsEnabled = true;
		}
		
		private void OnOpenExeLocation(object sender, RoutedEventArgs e)
        {
			if (this.Task == null)
			{
				return;
			}
			
			if (this.Task.ExeFile == null)
			{
				MessageBox.Show(this, "Task has not been saved yet");
				return;
			}
			
			try
			{
				Process.Start("explorer.exe", "/select, \"" + this.Task.ExeFile + "\"");
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message);
			}
        }
		
		private void OnWindowClosing(object sender, CancelEventArgs e)
		{
			try
			{
				TryOnWindowClosing(ref e);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Closing failed: " + ex.Message);
			}
		}
		
		private void TryOnWindowClosing(ref CancelEventArgs e)
		{
			if (this.Task != null && this.Task.IsModified)
			{
				MessageBoxResult mbResult = MessageBox.Show(this, "Do you want to save the task before closing?", "",
					MessageBoxButton.YesNoCancel);
					
				if (mbResult == MessageBoxResult.Yes)
				{
					if (OnSaveTask(false) == false)
					{
						e.Cancel = true;
					}
				}
				else if (mbResult == MessageBoxResult.Cancel)
				{
					e.Cancel = true;
				}
			}
			
			if (e.Cancel == false)
			{
				AddTaskToMruList();
				
				try
				{
					SavePreferences();
				}
				catch { }
			}
		}
		
		private void OnRunSelectedAction(object sender, RoutedEventArgs e)
		{
			try
			{
				TryOnRunSelectedAction();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Run selected action failed: " + ex.Message);
			}
		}
		
		private void TryOnRunSelectedAction()
		{
			if (this.mainScreen == null || this.mainScreen.SelectedAction == null)
			{
				return;
			}
			
			if (this.mainScreen.SelectedAction is Action)
			{
				Action selectedActionNormal = (Action)this.mainScreen.SelectedAction;
				RunAction runAction = new RunAction(selectedActionNormal);
				runAction.Run(true);
			}
			else if (this.mainScreen.SelectedAction is ConditionalAction)
			{
				ConditionalAction selectedActionConditional = (ConditionalAction)this.mainScreen.SelectedAction;
				bool? result = selectedActionConditional.Evaluate(true, true);
				if (result != null)
				{
					MessageBox.Show(this, "This Conditional evaluates to: " + result.Value);
				}
			}
			else if (this.mainScreen.SelectedAction is LoopConditional)
			{
				LoopConditional loopConditional = (LoopConditional)this.mainScreen.SelectedAction;
				bool? result = loopConditional.ConditionalAction.Evaluate(true, true);
				if (result != null)
				{
					MessageBox.Show(this, "This Conditional evaluates to: " + result.Value);
				}
			}
		}
		
		private void OnRunAllActions(object sender, RoutedEventArgs e)
		{
			if (this.Task == null)
			{
				return;
			}
			
			try
			{
				this.Task.Run();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Run task failed: " + ex.Message);
			}
		}
		
		private void OnRunStarting(object sender, RoutedEventArgs e)
		{
			if (this.Task == null)
			{
				return;
			}
			
			if (this.mainScreen == null || this.mainScreen.SelectedAction == null)
			{
				return;
			}
			
			try
			{
				this.Task.RunStartingWith(this.mainScreen.SelectedAction);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Run task failed: " + ex.Message);
			}
		}
		
		private void OnHighlightItem(object sender, RoutedEventArgs e)
		{
			try
			{
				TryOnHighlightItem();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Highlight failed: " + ex.Message);
			}
		}
		
		private void TryOnHighlightItem()
		{
			if (this.mainScreen.SelectedAction == null)
			{
				return;
			}
			
			if (!(this.mainScreen.SelectedAction is Action))
			{
				return;
			}
			
			Action selectedAction = (Action)this.mainScreen.SelectedAction;
			if (selectedAction.Element == null)
			{
				return;
			}
			
			IUIAutomationElement uiElement = selectedAction.Element.InnerElement;
			if (uiElement == null)
			{
				UIDeskAutomationLib.ElementBase libraryElement = selectedAction.Element.GetLibraryElement(true);
				if (libraryElement != null)
				{
					uiElement = libraryElement.InnerElement;
				}
				else
				{
					return;
				}
			}
			
			Helper.Highlight(uiElement, this);
			System.Threading.Thread.Sleep(200);
			Helper.UnHighlight();
			System.Threading.Thread.Sleep(200);
			Helper.Highlight(uiElement, this);
			System.Threading.Thread.Sleep(200);
			Helper.UnHighlight();
			this.Focus();
		}
		
		private void OnShowActionInfo(object sender, RoutedEventArgs e)
		{
			try
			{
				TryOnShowActionInfo();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Show info failed: " + ex.Message);
			}
		}
		
		private void TryOnShowActionInfo()
		{
			menuItemActionInfo.IsChecked = !menuItemActionInfo.IsChecked;
			this.mainScreen.ShowActionProperties(menuItemActionInfo.IsChecked);
			
			if (menuItemActionInfo.IsChecked == true)
			{
				this.mainScreen.SelectedActionChanged();
			}
		}
		
		private void OnShowActionProperties(object sender, RoutedEventArgs e)
		{
			try
			{
				TryOnShowActionProperties();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Show properties failed: " + ex.Message);
			}
		}
		
		private void TryOnShowActionProperties()
		{
			if (menuItemActionInfo.IsChecked == true)
			{
				return;
			}
			
			menuItemActionInfo.IsChecked = true;
			this.mainScreen.ShowActionProperties(true);
			this.mainScreen.SelectedActionChanged();
		}
		
		private void OnAbout(object sender, RoutedEventArgs e)
		{
			try
			{
				TryOnAbout();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "About failed: " + ex.Message);
			}
		}
		
		private void TryOnAbout()
		{
			AboutWindow aboutWindow = new AboutWindow();
			aboutWindow.Owner = this;
			aboutWindow.ShowDialog();
		}
		
		private void OnHelp(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start("help.html");
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Help failed: " + ex.Message);
			}
		}
		
		private void OnSetSpeed(object sender, RoutedEventArgs e)
		{
			try
			{
				TryOnSetSpeed();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Set speed failed: " + ex.Message);
			}
		}
		
		private void TryOnSetSpeed()
		{
			if (this.Task == null)
			{
				return;
			}
			
			SpeedWindow speedWindow = new SpeedWindow(this.Task.Speed, this.Task.SpeedValue);
			speedWindow.Owner = this;
			if (speedWindow.ShowDialog() == true)
			{
				if (this.Task.Speed != speedWindow.SelectedSpeed)
				{
					UndoRedo.AddSnapshot(this.Task);
				
					this.Task.Speed = speedWindow.SelectedSpeed;
					if (this.Task.Speed == Speed.Custom)
					{
						this.Task.SpeedValue = speedWindow.SpeedValue;
					}
					
					if (this.Task.StartAction != null || this.Task.ExeFile != null)
					{
						this.Task.IsModified = true;
						UpdateTitle();
					}
				}
				else if (this.Task.Speed == Speed.Custom && this.Task.SpeedValue != speedWindow.SpeedValue)
				{
					UndoRedo.AddSnapshot(this.Task);
				
					this.Task.SpeedValue = speedWindow.SpeedValue;
					
					if (this.Task.StartAction != null || this.Task.ExeFile != null)
					{
						this.Task.IsModified = true;
						UpdateTitle();
					}
				}
			}
		}
		
		public void Paused(bool paused)
		{
			Image img = this.btnToolbarPauseTask.Content as Image;
			if (img == null)
			{
				return;
			}
		
			if (paused == true)
			{
				try
				{
					Uri sourceUri = new Uri("../Pictures/resume.png", UriKind.Relative);
					BitmapImage thisIcon = new BitmapImage(sourceUri);
					
					img.Source = thisIcon;
				}
				catch (Exception ex)
				{
					return;
				}
				
				this.btnToolbarPauseTask.ToolTip = "Resume Task";
			}
			else
			{
				try
				{
					Uri sourceUri = new Uri("../Pictures/pause.png", UriKind.Relative);
					BitmapImage thisIcon = new BitmapImage(sourceUri);
					img.Source = thisIcon;
				}
				catch (Exception ex)
				{
					return;
				}
				
				this.btnToolbarPauseTask.ToolTip = "Pause Task";
			}
		}
		
		internal static CUIAutomation uiAutomation = new CUIAutomation();
		private UserControlMainScreen mainScreen = null;
		private UserControlVariables variablesScreen = null;
		private AppScreen currentScreen = AppScreen.Intro;
		public static string TITLE = "UI Automation Studio";
		public static string VERSION = "v3.3";
		
		private bool isTaskRunning = false;
		public bool IsTaskRunning
		{
			get
			{
				return this.isTaskRunning;
			}
			set
			{
				this.isTaskRunning = value;
				if (this.mainScreen != null)
				{
					this.mainScreen.SetTaskRunningButtons(value);
				}
			}
		}
		
		private Task task = null;
		public Task Task
		{
			get
			{
				return this.task;
			}
			set
			{
				try
				{
					TrySetTask(value);
				}
				catch (Exception ex)
				{
					MessageBox.Show(this, "Set task failed: " + ex.Message);
				}
			}
		}
		
		private void TrySetTask(Task value)
		{
			this.task = value;
				
			if (this.task == null)
			{
				return;
			}
			
			this.task.IsCurrent = true;
			
			if (this.mainScreen != null)
			{
				this.mainScreen.SelectedAction = null;
				this.mainScreen.SelectedArrow = null;
				
				this.mainScreen.Task = value;
				this.ActionInClipboard = null;
			}
			
			if (this.variablesScreen != null)
			{
				this.variablesScreen.ChangeTask(this.task);
			}
			
			UpdateTitle();
		}
		
		private void UpdateTitle()
		{
			string title = this.Title;
			if (this.task.IsModified == false)
			{
				if (title.EndsWith("*"))
				{
					this.Title = title.Remove(title.Length - 1, 1);
				}
			}
			else if (this.task.IsModified == true)
			{
				if (this.task.ExeFile != null && !title.EndsWith("*"))
				{
					this.Title = title + "*";
				}
			}
		}
    }
	
	public enum AppScreen
	{
		Intro,
		Workflow,
		Conditions
	}
}
