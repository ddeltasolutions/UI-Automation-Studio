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
		internal static CUIAutomation uiAutomation = new CUIAutomation();
		internal UserControlMainScreen mainScreen = null;
		private UserControlVariables variablesScreen = null;
		private AppScreen currentScreen = AppScreen.Intro;
		public static string TITLE = "UI Automation Studio";
		public static string VERSION = "v4.5";
	
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
			HelpMessages.Show(MessageId.AppOpened);
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
			if (this.Task != null && this.IsTaskRunning)
			{
				this.Task.Stop();
			}
		
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
			
			if (this.Task != null && this.IsTaskRunning)
			{
				this.Task.Stop();
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
			if (this.Task != null && this.IsTaskRunning)
			{
				this.Task.Stop();
			}
		
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
		
		private static List<string> MruTasks = new List<string>();
		
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
			bool shiftIsPressed = (System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Shift) == System.Windows.Forms.Keys.Shift;
			
			if (selectedArrow == null && this.Task.HasAtLeastOneConditional == true && !shiftIsPressed)
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
			
			bool insertAsFirst = false;
			if (selectedArrow == null && Task.StartAction != null)
			{
				if (shiftIsPressed)
				{
					PasteOptionsWindow wnd = new PasteOptionsWindow() { Owner = this, IsInsertFirstChecked = true };
					wnd.UseItForNewAction();
					if (wnd.ShowDialog() == true)
					{
						insertAsFirst = wnd.IsInsertFirstChecked;
					}
					else
					{
						return;
					}
				}
				else
				{
					// helper message
					HelpMessages.Show(MessageId.NewActionWithShift);
				}
			}
			
			Action action = new Action();
			
			AddActionWindow window = new AddActionWindow(action);
			window.Owner = this;
			
			window.Closed += (sender, e) => 
			{
				if (window.IsOkPressed == false)
				{
					return;
				}
			
				UndoRedo.AddSnapshot(Task);
				
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
					ActionBase crtAction = Task.StartAction;
					if (crtAction == null)
					{
						Task.StartAction = action;
					}
					else if (insertAsFirst == true)
					{
						action.Next = crtAction;
						crtAction.Previous = action;
						Task.StartAction = action;
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
				
				Task.IsModified = true;
				Task.Changed();
				
				if (mainScreen.SelectedArrow == null && insertAsFirst == false)
				{
					mainScreen.ScrollToBottom();
				}
				
				mainScreen.SelectedAction = action;
				ActionInClipboard = null;
			};
			
			window.Show();
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
			window.Closed += (sender, e) =>
			{
				if (window.IsOkPressed == false)
				{
					return;
				}
			
				UndoRedo.AddSnapshot(Task);
			
				tempAction.DeepCopy(action);
				Task.IsModified = true;
				
				Task.Changed();
				
				mainScreen.SelectedActionChanged();
			};
			
			window.Show();
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
						addConditionWindow.Closed += (sender, e) =>
						{
							BringMainWindowToForeground();
						
							if (addConditionWindow.IsOkPressed == false)
							{
								return;
							}
						
							UndoRedo.AddSnapshot(Task);
						
							tempConditionalAction.DeepCopy(loopConditional.ConditionalAction); // copy back with changes
							
							Task.IsModified = true;
							Task.Changed();
						};
						
						addConditionWindow.Show();
						addConditionWindow.Focus();
					}
					else if (isConditional == false && loopTypeWindow.IsConditional == false)
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
					else if (isConditional == true && loopTypeWindow.IsConditional == false)
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
					else if (isConditional == false && loopTypeWindow.IsConditional == true)
					{
						ConditionalAction conditionalAction = new ConditionalAction();
						
						AddConditionWindow addConditionWindow = 
							new AddConditionWindow(conditionalAction, this.Task, false, true);
						addConditionWindow.Owner = this;
						addConditionWindow.Closed += (sender, e) =>
						{
							BringMainWindowToForeground();
						
							if (addConditionWindow.IsOkPressed == false)
							{
								return;
							}
						
							UndoRedo.AddSnapshot(Task);
						
							LoopConditional newLoopConditional = new LoopConditional() 
								{ StartAction = loopAction.StartAction, EndAction = loopAction.EndAction, 
								ConditionalAction = conditionalAction };
								
							loopAction.EndAction.LoopAction = newLoopConditional;
							Task.LoopActions.Remove(loopAction);
							Task.LoopActions.Add(newLoopConditional);
							
							Task.IsModified = true;
							Task.Changed();
							
							mainScreen.SelectedAction = newLoopConditional;
						};
						
						addConditionWindow.Show();
						addConditionWindow.Focus();
					}
				};
				selectEndActionWindow.Show();
			};
			selectActionWindow.Show();
		}
		
		private void BringMainWindowToForeground()
		{
			MainWindow.Instance.Activate();
			MainWindow.Instance.Topmost = true;
			MainWindow.Instance.Topmost = false;
			MainWindow.Instance.Focus();
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
				UIDeskAutomationLib.ElementBase libraryElement = selectedAction.Element.GetLibraryElement(noTimeOut: true);
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
				if (this.mainScreen != null)
				{
					this.mainScreen.txbPaused.Text = "Paused";
				}
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
				if (this.mainScreen != null)
				{
					this.mainScreen.txbPaused.Text = "";
				}
			}
		}
		
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
				
				if (value == false)
				{
					UIDeskAutomationLib.Engine.IsCancelled = false;
				}
				
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
		
		private void OnDropFiles(object sender, DragEventArgs e)
		{
			try
			{
				TryOnDropFiles(sender, e);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message);
			}
		}
		
		private void TryOnDropFiles(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				
				if (files.Length == 0)
				{
					return;
				}
				
				string fileNameCmd = files[0];
				string fileNameXml = null;
				string fileExtension = Path.GetExtension(fileNameCmd).ToLower();
				
				if (fileExtension == ".cmd")
				{
					fileNameXml = Path.GetDirectoryName(fileNameCmd) + "\\" + 
						Path.GetFileNameWithoutExtension(fileNameCmd) + ".xml";
						
					if (!File.Exists(fileNameXml))
					{
						MessageBox.Show(this, "File " + fileNameXml + " does not exist");
						return;
					}
				}
				else if (fileExtension == ".xml")
				{
					fileNameXml = fileNameCmd;
				}
				else
				{
					MessageBox.Show(this, "Format not supported. Supported file formats: cmd and xml.");
					return;
				}
				
				OpenTask(fileNameXml);
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
