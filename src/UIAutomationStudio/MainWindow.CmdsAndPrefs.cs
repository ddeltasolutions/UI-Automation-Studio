using System;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.IO;

namespace UIAutomationStudio
{
	public partial class MainWindow : Window
	{
		// Commands
		private void AddCommands()
		{
			CommandBinding cb = new CommandBinding(ApplicationCommands.New);
			cb.Executed += new ExecutedRoutedEventHandler(OnNewTask);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(ApplicationCommands.Open);
			cb.Executed += new ExecutedRoutedEventHandler(OnOpenTask);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(ApplicationCommands.Save, OnSaveTask, CanExecuteSaveAs);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(ApplicationCommands.SaveAs, OnSaveAsTask, CanExecuteSaveAs);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(ApplicationCommands.Close, OnCloseTask, CanExecuteSaveAs);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.AddActionCommand, OnAddAction, CanExecuteAddAction);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.DeleteActionCommand, OnDeleteAction, CanExecuteDelete);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.EditActionCommand, OnEditAction, CanExecuteDelete);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(ApplicationCommands.Copy, OnCopyAction, CanExecuteCopy);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(ApplicationCommands.Cut, OnCutAction, CanExecuteCopy);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(ApplicationCommands.Paste, OnPasteAction, CanExecutePaste);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.RunSelectedActionCommand, OnRunSelectedAction, CanExecuteRunSelected);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.RunAllActionsCommand, OnRunAllActions, CanExecuteSaveAs);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.OpenExeLocationCommand, OnOpenExeLocation, CanExecuteOpenExeLocation);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.PasteFirstCommand, OnPasteFirst, CanExecutePaste);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.PasteLastCommand, OnPasteLast, CanExecutePaste);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.RunStartingCommand, OnRunStarting, CanExecuteMoveNext);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.HighlightCommand, OnHighlightItem, CanExecuteDelete);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.ActionInfoCommand, OnShowActionInfo, CanExecuteActionInfo);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.ActionPropertiesCommand, OnShowActionProperties, CanExecuteDelete);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.SpeedCommand, OnSetSpeed, CanExecuteSaveAs);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.WorkflowCommand, OnViewWorkflow, CanExecuteSaveAs);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.ConditionsCommand, OnViewConditions, CanExecuteSaveAs);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.AddConditionalCommand, OnAddConditional, CanExecuteAddAction);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding( MyCommands.ChangeDestinationCommand, 
				(sender, e) => 
				{
					try
					{
						this.mainScreen.ChangeArrowDestination(this.mainScreen.SelectedArrow);
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, "Change arrow destination failed: " + ex.Message);
					}
				}, 
				(sender, e) => 
				{
					if (this.IsTaskRunning == true)
					{
						e.CanExecute = false;
						return;
					}
					
					e.CanExecute = (this.currentScreen == AppScreen.Workflow && this.mainScreen != null && this.mainScreen.SelectedArrow != null);
				});
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding( MyCommands.DeleteArrowCommand, 
				(sender, e) => 
				{
					try
					{
						this.mainScreen.DeleteArrow(this.mainScreen.SelectedArrow);
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, "Delete arrow failed: " + ex.Message);
					}
				},
				(sender, e) => 
				{
					if (this.IsTaskRunning == true)
					{
						e.CanExecute = false;
						return;
					}
					
					e.CanExecute = (this.currentScreen == AppScreen.Workflow && this.mainScreen != null && this.mainScreen.SelectedArrow != null);
				});
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.RunSelectedAndMoveNextCommand, 
				(sender, e) => 
				{
					try
					{
						TryRunSelectedAndMoveNext();
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, "Run and move next failed: " + ex.Message);
					}
				}, CanExecuteMoveNext);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.StopTaskCommand, (sender, e) => 
				{
					if (this.Task == null)
					{
						return;
					}
					
					try
					{
						this.Task.Stop();
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, "Stop task failed: " + ex.Message);
					}
				},
				(sender, e) => e.CanExecute = this.IsTaskRunning);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.PauseResumeTaskCommand, (sender, e) => 
				{
					if (this.Task == null)
					{
						return;
					}
					
					if (this.mainScreen != null)
					{
						try
						{
							this.mainScreen.PauseResume(this.Task);
						}
						catch (Exception ex)
						{
							MessageBox.Show(this, "Pause/resume task failed: " + ex.Message);
						}
					}
				},
				(sender, e) => e.CanExecute = this.IsTaskRunning);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(ApplicationCommands.Undo, (sender, e) => 
				{
					try
					{
						Task previousTask = UndoRedo.Undo();
						if (previousTask != null)
						{
							this.Task = previousTask;
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, "Undo failed: " + ex.Message);
					}
				},
				(sender, e) => e.CanExecute = this.IsTaskRunning == false && 
					this.Task != null && UndoRedo.CanUndo == true);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(ApplicationCommands.Redo, (sender, e) => 
				{
					try
					{
						Task nextTask = UndoRedo.Redo();
						if (nextTask != null)
						{
							this.Task = nextTask;
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, "Redo failed: " + ex.Message);
					}
				},
				(sender, e) => e.CanExecute = this.IsTaskRunning == false && 
					this.Task != null && UndoRedo.CanRedo == true);
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.PropertiesCommand, (sender, e) => 
				{
					try
					{
						TryProperties();
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, "Show properties failed: " + ex.Message);
					}
				},
				(sender, e) => e.CanExecute = this.IsTaskRunning == false && 
					((this.currentScreen == AppScreen.Workflow && this.mainScreen.SelectedAction != null && 
					this.mainScreen.SelectedAction is Action && 
					((Action)(this.mainScreen.SelectedAction)).Element != null) || 
					(this.currentScreen == AppScreen.Conditions && this.variablesScreen.SelectedCondition != null)));
			this.CommandBindings.Add(cb);
			
			cb = new CommandBinding(MyCommands.AddLoopCommand, (sender, e) => 
			{
				try
				{
					TryAddLoop();
				}
				catch (Exception ex)
				{
					MessageBox.Show(this, "Add loop failed: " + ex.Message);
				}
			},
			(sender, e) => e.CanExecute = this.IsTaskRunning == false && this.Task != null && 
					this.currentScreen == AppScreen.Workflow && this.Task.StartAction != null);
			this.CommandBindings.Add(cb);
		}
		
		private void TryRunSelectedAndMoveNext()
		{
			ActionBase selectedAction = this.mainScreen.SelectedAction;
			if (selectedAction is Action)
			{
				Action selectedActionNormal = (Action)selectedAction;
				RunAction runAction = new RunAction(selectedActionNormal);
				runAction.Run();
				
				ActionBase nextAction = null;
				if (selectedActionNormal.LoopAction != null)
				{
					nextAction = selectedActionNormal.LoopAction.GetNextAction();
				}
				
				if (nextAction != null)
				{
					this.mainScreen.SelectedAction = nextAction;
				}
				else
				{
					if (selectedActionNormal.Next == null)
					{
						this.mainScreen.SelectedAction = null;
						this.Task.ResetAllCountLoops();
					}
					else
					{
						this.mainScreen.SelectedAction = selectedActionNormal.Next;
					}
				}
			}
			else if (selectedAction is ConditionalAction)
			{
				ConditionalAction selectedActionConditional = (ConditionalAction)selectedAction;
				bool? result = selectedActionConditional.Evaluate(true, true);
				
				if (result == null)
				{
					MessageBox.Show(this, "This Conditional Action could not be evaluated");
					return;
				}
				else if (result == true)
				{
					if (selectedActionConditional.NextOnTrue is EndAction)
					{
						this.mainScreen.SelectedAction = null;
					}
					else
					{
						this.mainScreen.SelectedAction = selectedActionConditional.NextOnTrue;
					}
				}
				else // false
				{
					if (selectedActionConditional.NextOnFalse is EndAction)
					{
						this.mainScreen.SelectedAction = null;
					}
					else
					{
						this.mainScreen.SelectedAction = selectedActionConditional.NextOnFalse;
					}
				}
			}
		}
		
		private void TryProperties()
		{
			Element element = null;
			if (this.currentScreen == AppScreen.Workflow)
			{
				Action selectedAction = (Action)this.mainScreen.SelectedAction;
				element = selectedAction.Element;
			}
			else // Conditions screen
			{
				Condition selectedCondition = this.variablesScreen.SelectedCondition;
				if (selectedCondition.Variable != null)
				{
					element = selectedCondition.Variable.Element;
				}
			}
			
			if (element == null)
			{
				return;
			}
			
			HelpMessages.Show(MessageId.Properties);
			
			PropertiesWindow window = new PropertiesWindow(element) { Task = this.Task };
			window.Owner = this;
			window.ShowDialog();
			
			if (window.HasChanged == true)
			{
				if (this.currentScreen == AppScreen.Workflow)
				{
					//this.Task.Changed();
				}
				else // Conditions screen
				{
					this.variablesScreen.Refresh();
				}
			}
		}
		
		private void TryAddLoop()
		{
			HelpMessages.Show(MessageId.AddLoop);
			
			this.mainScreen.SelectedAction = null;
			
			var selectActionWindow = new SelectActionWindow(this.mainScreen);
			selectActionWindow.Owner = this;
			selectActionWindow.Closed += (sender2, e2) => 
			{
				if (selectActionWindow.OkWasPressed == false)
				{
					return;
				}
				
				Action startAction = selectActionWindow.SelectedAction;
				
				this.mainScreen.SelectedAction = null;
				var selectEndActionWindow = new SelectActionWindow(this.mainScreen, true);
				selectEndActionWindow.Owner = this;
				selectEndActionWindow.Closed += (sender3, e3) => 
				{
					if (selectEndActionWindow.OkWasPressed == false)
					{
						return;
					}
					
					Action endAction = selectEndActionWindow.SelectedAction;
					
					if (!endAction.IsDescendentOf(startAction))
					{
						if (startAction.IsDescendentOf(endAction))
						{
							// swap between start and end
							Action temp = startAction;
							startAction = endAction;
							endAction = temp;
						}
						else
						{
							MessageBox.Show(this, "The End Action must be a descendent of the Start Action. " + 
								"This means there should be a path from Start Action to reach End Action.");
							return;
						}
					}
					
					LoopTypeWindow loopTypeWindow = new LoopTypeWindow();
					loopTypeWindow.Owner = this;
					if (loopTypeWindow.ShowDialog() != true)
					{
						return;
					}
					
					if (loopTypeWindow.IsConditional == false)
					{
						UndoRedo.AddSnapshot(this.Task);
					
						// is count loop
						LoopCount loopCount = new LoopCount(loopTypeWindow.Count) 
							{ StartAction = startAction, EndAction = endAction };
						endAction.LoopAction = loopCount;
						Task.LoopActions.Add(loopCount);
						
						this.Task.IsModified = true;
						this.Task.Changed();
						
						this.mainScreen.SelectedAction = loopCount;
					}
					else
					{
						// is conditional loop
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
						
							LoopConditional loopConditional = new LoopConditional() { StartAction = startAction, 
								EndAction = endAction, ConditionalAction = conditionalAction };
							endAction.LoopAction = loopConditional;
							Task.LoopActions.Add(loopConditional);
							
							Task.IsModified = true;
							Task.Changed();
							
							mainScreen.SelectedAction = loopConditional;
						};
						
						addConditionWindow.Show();
					}
				};
				selectEndActionWindow.Show();
			};
			selectActionWindow.Show();
		}
		
		private void CanExecuteSaveAs(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.IsTaskRunning == true)
			{
				e.CanExecute = false;
				return;
			}
		
			if (this.Task == null)
			{
				e.CanExecute = false;
			}
			else
			{
				e.CanExecute = true;
			}
		}
		
		private void CanExecuteAddAction(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.IsTaskRunning == true)
			{
				e.CanExecute = false;
				return;
			}
		
			if (this.Task == null || this.currentScreen != AppScreen.Workflow)
			{
				e.CanExecute = false;
			}
			else
			{
				e.CanExecute = true;
			}
		}
		
		private void CanExecutePaste(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.IsTaskRunning == true)
			{
				e.CanExecute = false;
				return;
			}
		
			if (this.ActionInClipboard == null || this.currentScreen != AppScreen.Workflow)
			{
				e.CanExecute = false;
			}
			else
			{
				e.CanExecute = true;
			}
		}
		
		private void CanExecuteMoveNext(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !(this.IsTaskRunning == true || this.mainScreen == null || 
					currentScreen != AppScreen.Workflow || this.mainScreen.SelectedAction == null || 
					this.mainScreen.SelectedAction is LoopAction);
		}
		
		private void CanExecuteRunSelected(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !(this.IsTaskRunning == true || this.mainScreen == null || 
					currentScreen != AppScreen.Workflow || this.mainScreen.SelectedAction == null || 
					this.mainScreen.SelectedAction is LoopCount);
		}
		
		private void CanExecuteDelete(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.IsTaskRunning == true)
			{
				e.CanExecute = false;
				return;
			}
			
			if (this.mainScreen == null)
			{
				e.CanExecute = false;
				return;
			}
			
			if (this.currentScreen != AppScreen.Workflow || this.mainScreen.SelectedAction == null)
			{
				e.CanExecute = false;
			}
			else
			{
				e.CanExecute = true;
			}
		}
		
		private void CanExecuteCopy(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.IsTaskRunning == true)
			{
				e.CanExecute = false;
				return;
			}
			
			if (this.mainScreen == null)
			{
				e.CanExecute = false;
				return;
			}
			
			if (this.currentScreen != AppScreen.Workflow || this.mainScreen.SelectedAction == null)
			{
				e.CanExecute = false;
			}
			else if (this.mainScreen.SelectedAction is Action)
			{
				e.CanExecute = true;
			}
		}
		
		private void CanExecuteOpenExeLocation(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.IsTaskRunning == true)
			{
				e.CanExecute = false;
				return;
			}
			
			if (this.Task == null || this.Task.ExeFile == null)
			{
				e.CanExecute = false;
			}
			else
			{
				e.CanExecute = true;
			}
		}
		
		private void CanExecuteActionInfo(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.mainScreen == null)
			{
				e.CanExecute = false;
			}
			else
			{
				e.CanExecute = true;
			}
		}
		
		// Preferences
		private static string prefFileName = null;
		
		static MainWindow()
		{
			try
			{
				string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + 
					"\\UIAutomationStudio";
				if (!Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}
				prefFileName = folder + "\\UIAutomationStudioPref.xml";
			}
			catch
			{
				prefFileName = Path.GetTempPath() + "\\UIAutomationStudioPref.xml";
			}
		}
		
		private void SavePreferences()
        {
            XmlDocument doc = new XmlDocument();

            XmlNode root = doc.CreateNode(XmlNodeType.Element, "Preferences", null);
            doc.AppendChild(root);
			
			XmlNode node = doc.CreateNode(XmlNodeType.Element, "WindowState", null);
			if (this.WindowState == WindowState.Minimized)
			{
				node.InnerText = WindowState.Normal.ToString();
			}
			else
			{
				node.InnerText = this.WindowState.ToString();
			}
			root.AppendChild(node);
			
			if (this.WindowState != WindowState.Maximized)
			{
				node = doc.CreateNode(XmlNodeType.Element, "Left", null);
				node.InnerText = this.Left.ToString();
				root.AppendChild(node);
				
				node = doc.CreateNode(XmlNodeType.Element, "Top", null);
				node.InnerText = this.Top.ToString();
				root.AppendChild(node);
				
				node = doc.CreateNode(XmlNodeType.Element, "Width", null);
				node.InnerText = this.Width.ToString();
				root.AppendChild(node);
				
				node = doc.CreateNode(XmlNodeType.Element, "Height", null);
				node.InnerText = this.Height.ToString();
				root.AppendChild(node);
			}

			foreach (string mru in MruTasks)
			{
				node = doc.CreateNode(XmlNodeType.Element, "MruTask", null);
				node.InnerText = mru;
				root.AppendChild(node);
			}
			
			node = doc.CreateNode(XmlNodeType.Element, "HelpMessages", null);
			HelpMessages.SaveHiddenMessages(node, doc);
			root.AppendChild(node);
			
			doc.Save(prefFileName);
		}
		
		private void LoadPreferences()
		{
			if (!File.Exists(prefFileName))
            {
				return;
			}
			
			XmlDocument doc = new XmlDocument();
			doc.Load(prefFileName);
			XmlNodeList nodeList = null;
			
			//load position preferences
			nodeList = doc.GetElementsByTagName("WindowState");
			if (nodeList.Count > 0)
			{
				XmlNode node = nodeList[0];
				try
				{
					this.WindowState = (WindowState)Enum.Parse(typeof(WindowState), node.InnerText, true);
				}
				catch { }
			}
			
			if (this.WindowState != WindowState.Maximized)
			{
				nodeList = doc.GetElementsByTagName("Left");
				if (nodeList.Count > 0)
				{
					try
					{
						this.WindowStartupLocation = WindowStartupLocation.Manual;
						this.Left = Double.Parse(nodeList[0].InnerText);
					}
					catch { }
				}
				
				nodeList = doc.GetElementsByTagName("Top");
				if (nodeList.Count > 0)
				{
					try
					{
						this.Top = Double.Parse(nodeList[0].InnerText);
					}
					catch { }
				}
				
				nodeList = doc.GetElementsByTagName("Width");
				if (nodeList.Count > 0)
				{
					try
					{
						this.Width = Double.Parse(nodeList[0].InnerText);
					}
					catch { }
				}
				
				nodeList = doc.GetElementsByTagName("Height");
				if (nodeList.Count > 0)
				{
					try
					{
						this.Height = Double.Parse(nodeList[0].InnerText);
					}
					catch { }
				}
			}

			// load most recently used tasks
			nodeList = doc.GetElementsByTagName("MruTask");
			foreach (XmlNode node in nodeList)
			{
				string exeFile = node.InnerText;
				if (File.Exists(exeFile))
				{
					MruTasks.Add(exeFile);
				}
			}
			
			LoadMruTasks();
			
			// load hidden help messages
			nodeList = doc.GetElementsByTagName("HelpMessages");
			if (nodeList.Count > 0)
			{
				XmlNode node = nodeList[0];
				HelpMessages.LoadHiddenMessages(node);
			}
		}
	}
}