using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Diagnostics;
using System.Xml;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;

namespace UIAutomationStudio
{
	public class Task
	{
		public ActionBase StartAction { get; set; }
		public string ExeFile { get; set; }
		public Speed Speed { get; set; }
		public int SpeedValue { get; set; }
		public int ConditionalCount { get; set; }
		
		public List<LoopAction> LoopActions { get; set; }
		public event System.Action ChangedEvent;
		
		public void Changed()
		{
			if (this.ChangedEvent != null)
			{
				this.ChangedEvent();
			}
		}
		
		private bool isModified = false;
		public bool IsModified 
		{
			get
			{
				return this.isModified;
			}
			set
			{
				if (this.isModified != value)
				{
					this.HasChanged = true;
				}
				else
				{
					this.HasChanged = false;
				}
				
				this.isModified = value;
			}
		}
		
		public bool HasChanged { get; set; }
		public bool IsCurrent { get; set; }
		
		public Task()
		{
			StartAction = null;
			ExeFile = null;
			Speed = Speed.VeryFast;
			SpeedValue = 0;
			
			HasChanged = false;
			ConditionalCount = 1;
			IsCurrent = false;
			
			LoopActions = new List<LoopAction>();
		}
		
		public ActionBase GetActionById(int id)
		{
			return SearchAction(this.StartAction, id);
		}
		
		public ActionBase SearchAction(ActionBase action, int id)
		{
			ActionBase current = action;
			
			while (current != null)
			{
				if (current.Id == id)
				{
					return current;
				}
				
				if (current is Action)
				{
					current = ((Action)current).Next;
				}
				else if (current is ConditionalAction)
				{
					ConditionalAction conditionalAction = (ConditionalAction)current;
					ActionBase result = SearchAction(conditionalAction.NextOnFalse, id);
					if (result != null)
					{
						return result;
					}
					
					return SearchAction(conditionalAction.NextOnTrue, id);
				}
				else
				{
					break;
				}
			}
			
			return null;
		}
		
		public void DeepCopy(Task destination)
		{
			actionsWithWeakBonds.Clear();
			if (this.StartAction != null)
			{
				if (this.StartAction is Action)
				{
					destination.StartAction = new Action();
				}
				else if (this.StartAction is ConditionalAction)
				{
					destination.StartAction = new ConditionalAction();
				}
				else if (this.StartAction is EndAction)
				{
					destination.StartAction = new EndAction();
				}
				
				DeepCopyActions(this.StartAction, destination.StartAction);
			}
			else
			{
				destination.StartAction = null;
			}
			
			bool idsWereAssigned = false;
			if (this.actionsWithWeakBonds.Count > 0)
			{
				this.AssignIds();
				destination.AssignIds();
				idsWereAssigned = true;
				
				foreach (ActionBase action in this.actionsWithWeakBonds)
				{
					int startId = action.Id;
					ActionBase start = destination.GetActionById(startId);
					
					if (action is Action)
					{
						int endId = ((Action)action).Next.Id;
						
						//connect
						Action startNormal = (Action)start;
						ActionBase end = destination.GetActionById(endId);
						startNormal.Next = end;
					}
					else if (action is ConditionalAction)
					{
						ConditionalAction conditionalAction = (ConditionalAction)action;
						ConditionalAction startConditional = (ConditionalAction)start;
						
						if (conditionalAction.HasOnFalseWeakBond == true)
						{
							int endId = conditionalAction.NextOnFalse.Id;
							
							//connect
							startConditional.NextOnFalse = destination.GetActionById(endId);
						}
						if (conditionalAction.HasOnTrueWeakBond == true)
						{
							int endId = conditionalAction.NextOnTrue.Id;
							
							//connect
							startConditional.NextOnTrue = destination.GetActionById(endId);
						}
					}
				}
			}
			
			if (this.LoopActions.Count > 0)
			{
				if (idsWereAssigned == false)
				{
					this.AssignIds();
					destination.AssignIds();
				}
				
				foreach (LoopAction loopAction in this.LoopActions)
				{
					Action startActionDest = destination.GetActionById(loopAction.StartAction.Id) as Action;
					if (startActionDest == null)
					{
						continue;
					}
					
					Action endActionDest = destination.GetActionById(loopAction.EndAction.Id) as Action;
					if (endActionDest == null)
					{
						continue;
					}
				
					if (loopAction is LoopCount)
					{
						LoopCount loopCount = (LoopCount)loopAction;
						
						LoopCount destLoopCount = new LoopCount(loopCount.InitialCount) 
							{ StartAction = startActionDest, EndAction = endActionDest };
						destLoopCount.EndAction.LoopAction = destLoopCount;
						destination.LoopActions.Add(destLoopCount);
					}
					else if (loopAction is LoopConditional)
					{
						LoopConditional loopConditional = (LoopConditional)loopAction;
						ConditionalAction destCondAction = new ConditionalAction();
						loopConditional.ConditionalAction.DeepCopy(destCondAction);
						
						LoopConditional destLoopConditional = new LoopConditional() 
							{ ConditionalAction = destCondAction, StartAction = startActionDest, 
							EndAction = endActionDest };
						destLoopConditional.EndAction.LoopAction = destLoopConditional;
						destination.LoopActions.Add(destLoopConditional);
					}
				}
			}
			
			destination.ExeFile = this.ExeFile;
			destination.Speed = this.Speed;
			destination.SpeedValue = this.SpeedValue;
			destination.ConditionalCount = this.ConditionalCount;
			
			destination.IsModified = this.IsModified;
		}
		
		private List<ActionBase> actionsWithWeakBonds = new List<ActionBase>();
		
		private void DeepCopyActions(ActionBase start, ActionBase destination)
		{
			start.DeepCopy(destination);
			
			ActionBase current = start;
			while (current != null)
			{
				if (current is Action)
				{
					Action currentNormal = (Action)current;
					Action destinationNormal = (Action)destination;
					
					if (currentNormal.Next == null)
					{
						break;
					}
					
					if (currentNormal.HasWeakBond == true)
					{
						this.actionsWithWeakBonds.Add(currentNormal);
						break;
					}
					
					if (currentNormal.Next is Action)
					{
						Action currentNormalNext = (Action)currentNormal.Next;
						destinationNormal.Next = new Action();
						
						currentNormalNext.DeepCopy(destinationNormal.Next);
					}
					else if (currentNormal.Next is ConditionalAction)
					{
						ConditionalAction currentNormalNext = (ConditionalAction)currentNormal.Next;
						destinationNormal.Next = new ConditionalAction();
						
						currentNormalNext.DeepCopy((ConditionalAction)(destinationNormal.Next));
					}
					else if (currentNormal.Next is EndAction)
					{
						destinationNormal.Next = new EndAction();
					}
					
					destinationNormal.Next.Previous = destinationNormal;
					
					current = currentNormal.Next;
					destination = destinationNormal.Next;
				}
				else if (current is ConditionalAction)
				{
					ConditionalAction currentConditional = (ConditionalAction)current;
					ConditionalAction destinationConditional = (ConditionalAction)destination;
					
					if (currentConditional.HasOnFalseWeakBond == false)
					{
						if (currentConditional.NextOnFalse is Action)
						{
							Action nextNormal = (Action)currentConditional.NextOnFalse;
							destinationConditional.NextOnFalse = new Action();
						}
						else if (currentConditional.NextOnFalse is ConditionalAction)
						{
							ConditionalAction nextConditional = (ConditionalAction)currentConditional.NextOnFalse;
							destinationConditional.NextOnFalse = new ConditionalAction();
						}
						else if (currentConditional.NextOnFalse is EndAction)
						{
							destinationConditional.NextOnFalse = new EndAction();
						}
						destinationConditional.NextOnFalse.Previous = destinationConditional;
						
						DeepCopyActions(currentConditional.NextOnFalse, destinationConditional.NextOnFalse);
					}
					else
					{
						this.actionsWithWeakBonds.Add(currentConditional);
					}
					
					if (currentConditional.HasOnTrueWeakBond == false)
					{
						if (currentConditional.NextOnTrue is Action)
						{
							Action nextNormal = (Action)currentConditional.NextOnTrue;
							destinationConditional.NextOnTrue = new Action();
						}
						else if (currentConditional.NextOnTrue is ConditionalAction)
						{
							ConditionalAction nextConditional = (ConditionalAction)currentConditional.NextOnTrue;
							destinationConditional.NextOnTrue = new ConditionalAction();
						}
						else if (currentConditional.NextOnTrue is EndAction)
						{
							destinationConditional.NextOnTrue = new EndAction();
						}
						destinationConditional.NextOnTrue.Previous = destinationConditional;
						
						DeepCopyActions(currentConditional.NextOnTrue, destinationConditional.NextOnTrue);
					}
					else if (!this.actionsWithWeakBonds.Contains(currentConditional))
					{
						this.actionsWithWeakBonds.Add(currentConditional);
					}
					
					break;
				}
				else if (current is EndAction)
				{
					break;
				}
			}
		}
		
		public void Run()
		{
			RunStartingWith(this.StartAction);
		}
		
		private BackgroundWorker backgroundWorker = null;
		public void RunStartingWith(ActionBase startAction)
		{
			/*BackgroundWorker*/ backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += backgroundWorker_DoWork;
			backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
			
			if (MainWindow.Instance != null)
			{
				MainWindow.Instance.IsTaskRunning = true;
			}
			cancelled = false;
			backgroundWorker.RunWorkerAsync(startAction);
		}
		
		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			// Reset all counter loops
			if (isResumed == false)
			{
				ResetAllCountLoops();
			}
			else
			{
				isResumed = false;
			}
		
			ActionBase startAction = (ActionBase)e.Argument;
			RunStartingWith_InBackground(startAction);
		}
		
		public void ResetAllCountLoops()
		{
			foreach (LoopAction loopAction in this.LoopActions)
			{
				if (loopAction is LoopCount)
				{
					((LoopCount)loopAction).Reset();
				}
			}
		}
		
		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (MainWindow.Instance != null && paused == false)
			{
				MainWindow.Instance.IsTaskRunning = false;
			}
			
			if (this.OnTaskCompleted != null && paused == false)
			{
				this.OnTaskCompleted();
			}
			
			if (mbw != null)
			{
				Application.Current.Dispatcher.Invoke( () =>
					{
						// Code to run on the GUI thread.
						mbw.Close();
					} );
				mbw = null;
			}
			
			attemptPause = false;
		}
		
		public event System.Action OnTaskCompleted;
		
		private bool cancelled = false;
		public void Stop()
		{
			if (paused == true)
			{
				attemptPause = false;
				paused = false;
				cancelled = false;
				
				if (MainWindow.Instance != null)
				{
					MainWindow.Instance.IsTaskRunning = false;
				}
				
				if (this.OnTaskCompleted != null)
				{
					this.OnTaskCompleted();
				}
				
				return;
			}
			
			cancelled = true;
			
			DisplayMessage("Stoping...");
		}
		
		private MessageBoxWindow mbw = null;
		private void DisplayMessage(string message)
		{
			if (mbw == null)
			{
				mbw = new MessageBoxWindow(message);
				if (MainWindow.Instance != null)
				{
					mbw.Owner = MainWindow.Instance;
				}
				else
				{
					mbw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				}
			}
			else
			{
				mbw.SetText(message);
			}
			mbw.Show();
		}
		
		private bool attemptPause = false;
		private bool paused = false;
		private ActionBase actionResume = null;
		public void Pause()
		{
			attemptPause = true;
			
			DisplayMessage("Pausing...");
		}
		
		private bool isResumed = false;
		
		public void Resume()
		{
			if (paused == true)
			{
				paused = false;
				
				isResumed = true;
				backgroundWorker.RunWorkerAsync(actionResume);
			}
		}
		
		public bool IsPaused
		{
			get	{ return paused; }
		}
		
		public bool IsAttemptingPause
		{
			get	{ return attemptPause; }
		}
		
		public void RunStartingWith_InBackground(ActionBase startAction)
		{
			ActionBase currentAction = startAction;
			while (currentAction != null)
			{
				if (currentAction is Action)
				{
					Action currentActionNormal = (Action)currentAction;
					RunAction runAction = new RunAction(currentActionNormal);
					
					if (runAction.Run() == false)
					{
						break;
					}
					
					if (this.Speed == Speed.Custom)
					{
						System.Threading.Thread.Sleep(this.SpeedValue);
					}
					else if (this.Speed != Speed.VeryFast)
					{
						System.Threading.Thread.Sleep((int)this.Speed);
					}
					
					ActionBase nextAction = null;
					if (currentActionNormal.LoopAction != null)
					{
						nextAction = currentActionNormal.LoopAction.GetNextAction();
					}
					
					if (nextAction != null)
					{
						currentAction = nextAction;
					}
					else
					{
						currentAction = currentActionNormal.Next;
					}
				}
				else if (currentAction is ConditionalAction)
				{
					ConditionalAction currentActionConditional = (ConditionalAction)currentAction;
					bool? result = currentActionConditional.Evaluate(true, false);
					if (result == null)
					{
						MessageBox.Show("The Conditional \"" + currentActionConditional.Name + 
							"\" could not be evaluated. The UI Element may not be available anymore.");
						break;
					}
					else if (result == true)
					{
						currentAction = currentActionConditional.NextOnTrue;
					}
					else
					{
						currentAction = currentActionConditional.NextOnFalse;
					}
				}
				else if (currentAction is EndAction)
				{
					break;
				}
				
				if (cancelled == true)
				{
					break;
				}
				else if (attemptPause == true)
				{
					attemptPause = false;
					paused = true;
					actionResume = currentAction;
					
					if (this.OnPauseSucceeded != null)
					{
						this.OnPauseSucceeded();
					}
					
					break;
				}
			}
		}
		
		public event System.Action OnPauseSucceeded;
		
		private void AssignIds()
		{
			int id = 0;
			AssignIds(this.StartAction, ref id);
		}
		
		private void AssignIds(ActionBase startAction, ref int id)
		{
			ActionBase currentAction = startAction;
			while (currentAction != null)
			{
				currentAction.Id = id;
				id++;
			
				if (currentAction is Action)
				{
					Action currentActionNormal = (Action)currentAction;
					if (currentActionNormal.HasWeakBond == true)
					{
						break;
					}
					currentAction = currentActionNormal.Next;
				}
				else if (currentAction is ConditionalAction)
				{
					ConditionalAction currentActionConditional = (ConditionalAction)currentAction;
					if (currentActionConditional.HasOnFalseWeakBond == false)
					{
						AssignIds(currentActionConditional.NextOnFalse, ref id);
					}
					if (currentActionConditional.HasOnTrueWeakBond == false)
					{
						AssignIds(currentActionConditional.NextOnTrue, ref id);
					}
					break;
				}
				else if (currentAction is EndAction)
				{
					break;
				}
			}
		}
		
		public string XmlFilePath
		{
			get
			{
				if (ExeFile == null)
				{
					return null;
				}
				
				try
				{
					return Path.GetDirectoryName(ExeFile) + "\\" + Path.GetFileNameWithoutExtension(ExeFile) + ".xml";
				}
				catch
				{
					return null;
				}
			}
		}
		
		public bool Save()
		{
			if (ExeFile == null)
			{
				return false;
			}
			
			// Save xml first
			string xmlFileName = null;
			try
			{
				AssignIds();
				XmlDocument doc = this.WriteToXml();
				xmlFileName = Path.GetFileNameWithoutExtension(ExeFile) + ".xml";
				string xmlFilePath = Path.GetDirectoryName(ExeFile) + "\\" + xmlFileName;
				
				doc.Save(xmlFilePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show(MainWindow.Instance, "Xml file could not be saved: " + ex.Message);
				return false;
			}
			
			string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
			string[] lines = new[] { "\"" + strExeFilePath + "\" \"%cd%\\" + xmlFileName + "\"" };
			
			try
			{
				File.WriteAllLines(ExeFile, lines);
			}
			catch (Exception ex)
			{
				MessageBox.Show(MainWindow.Instance, ex.Message);
				return false;
			}
			
			this.IsModified = false;
			return true;
		}
		
		public void RemoveAction(ActionBase action, bool raiseEvent = true)
		{
			if (action is Action)
			{
				Action actionNormal = (Action)action;
				if (action.Previous == null) // the start action is selected
				{
					this.StartAction = actionNormal.Next;
					if (this.StartAction != null)
					{
						this.StartAction.Previous = null;
					}
				}
				else
				{
					ActionBase previousAction = action.Previous;
					if (previousAction is Action)
					{
						Action previousActionNormal = (Action)previousAction;
						if (actionNormal.Next != null && actionNormal.Next.Previous != actionNormal)
						{
							EndAction endAction = new EndAction();
							previousActionNormal.Next = endAction;
							endAction.Previous = previousActionNormal;
						}
						else
						{
							previousActionNormal.Next = actionNormal.Next;
							if (actionNormal.Next != null)
							{
								actionNormal.Next.Previous = previousAction;
							}
						}
					}
					else if (previousAction is ConditionalAction)
					{
						// TO DO...
						ConditionalAction prevActionConditional = (ConditionalAction)previousAction;
						if (prevActionConditional.NextOnFalse == action)
						{
							if (actionNormal.Next != null && actionNormal.Next.Previous != actionNormal)
							{
								EndAction endAction = new EndAction();
								prevActionConditional.NextOnFalse = endAction;
								endAction.Previous = prevActionConditional;
							}
							else
							{
								prevActionConditional.NextOnFalse = actionNormal.Next;
								prevActionConditional.NextOnFalse.Previous = prevActionConditional;
							}
						}
						else if (prevActionConditional.NextOnTrue == action)
						{
							if (actionNormal.Next != null && actionNormal.Next.Previous != actionNormal)
							{
								EndAction endAction = new EndAction();
								prevActionConditional.NextOnTrue = endAction;
								endAction.Previous = prevActionConditional;
							}
							else
							{
								prevActionConditional.NextOnTrue = actionNormal.Next;
								prevActionConditional.NextOnTrue.Previous = prevActionConditional;
							}
						}
					}
				}
			}
			else if (action is ConditionalAction)
			{
				if (action.Previous == null) // the start node is selected
				{
					this.StartAction = null; // remove all actions
				}
				else
				{
					ActionBase previousAction = action.Previous;
					if (previousAction is Action)
					{
						bool hasAtLeastOneConditional = this.HasAtLeastOneConditional;
						Action previousActionNormal = (Action)previousAction;
						previousActionNormal.Next = null;
						
						if (hasAtLeastOneConditional == true)
						{
							// Add end action
							EndAction endAction = new EndAction();
							previousActionNormal.Next = endAction;
							endAction.Previous = previousActionNormal;
						}
					}
					else if (previousAction is ConditionalAction)
					{
						ConditionalAction prevActionConditional = (ConditionalAction)previousAction;
						if (prevActionConditional.NextOnFalse == action)
						{
							prevActionConditional.NextOnFalse = new EndAction();
							prevActionConditional.NextOnFalse.Previous = prevActionConditional;
						}
						else if (prevActionConditional.NextOnTrue == action)
						{
							prevActionConditional.NextOnTrue = new EndAction();
							prevActionConditional.NextOnTrue.Previous = prevActionConditional;
						}
					}
				}
			}
			else if (action is LoopAction)
			{
				LoopAction loopAction = (LoopAction)action;
				loopAction.EndAction.LoopAction = null;
				this.LoopActions.Remove(loopAction);
			}
			
			if (this.LoopActions.Count > 0 && (action is Action))
			{
				Action actionNormal = (Action)action;
				
				List<LoopAction> toBeDeleted = new List<LoopAction>();
				foreach (LoopAction loopAction in this.LoopActions)
				{
					if (loopAction.StartAction == actionNormal || loopAction.EndAction == actionNormal)
					{
						toBeDeleted.Add(loopAction);
					}
				}
				foreach (LoopAction loopAction in toBeDeleted)
				{
					loopAction.EndAction.LoopAction = null;
					this.LoopActions.Remove(loopAction);
				}
			}
			
			if (raiseEvent == true)
			{
				action.Deleted();
			}
			if (this.StartAction is EndAction)
			{
				this.StartAction = null;
			}
		}
		
		// returns null if there is at least one conditional action
		public Action GetLastAction()
		{
			ActionBase currentAction = this.StartAction;
			while (currentAction != null)
			{
				if (currentAction is ConditionalAction)
				{
					return null;
				}
				else if (currentAction is Action)
				{
					Action currentActionNormal = (Action)currentAction;
					if (currentActionNormal.Next == null || currentActionNormal.Next is EndAction)
					{
						return currentActionNormal;
					}
					currentAction = currentActionNormal.Next;
				}
			}
			
			return null;
		}
		
		public bool HasAtLeastOneConditional
		{
			get
			{
				ActionBase currentAction = this.StartAction;
				while (currentAction != null)
				{
					if (currentAction is ConditionalAction)
					{
						return true;
					}
					else if (currentAction is Action)
					{
						Action currentActionNormal = (Action)currentAction;
						if (currentActionNormal.HasWeakBond == false)
						{
							currentAction = currentActionNormal.Next;
						}
						else
						{
							break;
						}
					}
					else if (currentAction is EndAction)
					{
						break;
					}
				}
				
				return false;
			}
		}
		
		public bool GetHasArrowToStartAction(ActionBase action)
		{
			ActionBase current = action;
			
			while (current != null)
			{
				if (current is Action)
				{
					Action currentNormal = (Action)current;
					if (currentNormal.Next == this.StartAction)
					{
						return true;
					}
					if (currentNormal.HasWeakBond == true)
					{
						break;
					}
					
					current = currentNormal.Next;
				}
				else if (current is ConditionalAction)
				{
					ConditionalAction currentConditional = (ConditionalAction)current;
					if (currentConditional.NextOnFalse == this.StartAction)
					{
						return true;
					}
					
					if (currentConditional.NextOnTrue == this.StartAction)
					{
						return true;
					}
					
					if (currentConditional.HasOnFalseWeakBond == false && 
						GetHasArrowToStartAction(currentConditional.NextOnFalse) == true)
					{
						return true;
					}
					
					if (currentConditional.HasOnTrueWeakBond == false && 
						GetHasArrowToStartAction(currentConditional.NextOnTrue) == true)
					{
						return true;
					}
					
					break;
				}
				else if (current is EndAction)
				{
					break;
				}
			}
			
			return false;
		}
		
		public static void GetRowAndColumnCount(ActionBase action, ref int rowCount, ref int columnCount, 
			int currentRow, int currentColumn)
		{
			columnCount = 1;
		
			ActionBase current = action;
			while (current != null)
			{
				current.GridNode = null;
				current.Row = currentRow;
				current.Column = currentColumn;
				current.ColumnSpan = 1;
				
				rowCount++;
				if (current is Action)
				{
					Action currentNormal = (Action)current;
					if (currentNormal.Next != null && currentNormal.Next.Previous != current)
					{
						break;
					}
					current = currentNormal.Next;
				}
				else if (current is ConditionalAction)
				{
					int rowCountLeft = 0;
					int columnCountLeft = 1;
					int rowCountRight = 0;
					int columnCountRight = 1;
					
					ConditionalAction currentConditional = (ConditionalAction)current;
					if (currentConditional.NextOnFalse.Previous == current)
					{
						GetRowAndColumnCount(currentConditional.NextOnFalse, ref rowCountLeft, ref columnCountLeft, 
							currentRow + 1, currentColumn);
					}
					
					if (currentConditional.NextOnTrue.Previous == current)
					{
						GetRowAndColumnCount(currentConditional.NextOnTrue, ref rowCountRight, ref columnCountRight, 
							currentRow + 1, currentColumn + columnCountLeft);
					}
					
					rowCount += (rowCountLeft >= rowCountRight ? rowCountLeft : rowCountRight);
					columnCount = columnCountLeft + columnCountRight;
					current.ColumnSpan = columnCount;

					UpdateColumnSpan(currentConditional);
					return;
				}
				else if (current is EndAction)
				{
					break;
				}
				
				currentRow++;
			}
		}
		
		private static void UpdateColumnSpan(ConditionalAction conditionalAction)
		{
			ActionBase current = conditionalAction.Previous;
			while (current != null)
			{
				if (current is ConditionalAction)
				{
					break;
				}
				else if (current is Action)
				{
					current.ColumnSpan = conditionalAction.ColumnSpan;
					current = current.Previous;
				}
			}
		}
		
		public bool UsesCondition(Condition condition)
		{
			return UsesCondition(condition, this.StartAction);
		}
		
		private bool UsesCondition(Condition condition, ActionBase startAction)
		{
			ActionBase currentAction = startAction;
			while (currentAction != null)
			{
				if (currentAction is Action)
				{
					Action currentActionNormal = (Action)currentAction;
					if (currentActionNormal.HasWeakBond == true)
					{
						break;
					}
					currentAction = currentActionNormal.Next;
				}
				else if (currentAction is ConditionalAction)
				{
					ConditionalAction currentActionConditional = (ConditionalAction)currentAction;
					
					if (currentActionConditional.ConditionWrappers.Exists(x => x.Condition == condition) == true)
					{
						return true;
					}
					if (currentActionConditional.HasOnFalseWeakBond == false && 
						UsesCondition(condition, currentActionConditional.NextOnFalse) == true)
					{
						return true;
					}
					if (currentActionConditional.HasOnTrueWeakBond == false && 
						UsesCondition(condition, currentActionConditional.NextOnTrue) == true)
					{
						return true;
					}
					break;
				}
				else if (currentAction is EndAction)
				{
					break;
				}
			}
			
			return false;
		}
	}
	
	public enum Speed
	{
		VeryFast = 0,
		Fast = 100,
		Slower = 500,
		Slow = 1000,
		Custom
	}
}