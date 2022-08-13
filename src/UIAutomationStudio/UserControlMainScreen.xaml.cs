using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControlMainScreen : UserControl
    {
        public UserControlMainScreen()
        {
            InitializeComponent();
        }
		
		public const int NODE_WIDTH = 220;
		public const int NODE_HEIGHT = 95;
		private const int NODE_AND_ARROW_HEIGHT = 145; // arrow is 50
		private const int GRID_MIN_WIDTH = 400;
		public const int LEFT_RIGHT_PADDING = 10;
		private const int NODE_WIDTH_WITH_PADDING = NODE_WIDTH + 2 * LEFT_RIGHT_PADDING;
		public const int LINE_THICKNESS = 3;
		
		private Task task = null;
		public Task Task
		{
			get
			{
				return this.task;
			}
			set
			{
				this.task = value;
				if (value != null)
				{
					this.task.ChangedEvent += () => 
					{
						RedrawAllActions(this.task.StartAction);
						UpdateTitle();
					};
					RedrawAllActions(this.task.StartAction);
				}
			}
		}
		
		private void UpdateTitle()
		{
			if (this.Task.HasChanged == false)
			{
				return;
			}
			
			if (this.Task.IsCurrent == false)
			{
				return;
			}
			
			string title = MainWindow.Instance.Title;
			
			if (this.Task.IsModified == true)
			{
				if (this.Task.ExeFile != null && !title.EndsWith("*"))
				{
					MainWindow.Instance.Title = title + "*";
				}
			}
			else if (this.Task.IsModified == false)
			{
				if (title.EndsWith("*"))
				{
					MainWindow.Instance.Title = title.Remove(title.Length - 1, 1);
				}
			}
		}
		
		private void RedrawAllActions(ActionBase startAction)
		{
			if (this.Task.GetHasArrowToStartAction(startAction) == true || 
				(startAction is Action && ((Action)startAction).LoopAction != null))
			{
				TOP_OFFSET = 15;
			}
			else
			{
				TOP_OFFSET = 5;
			}
			
			int rowCount = 0;
			int columnCount = 0;
			Task.GetRowAndColumnCount(startAction, ref rowCount, ref columnCount, 0, 0);
			
			int width = NODE_WIDTH_WITH_PADDING * columnCount;
			int height = NODE_AND_ARROW_HEIGHT * rowCount;
			
			width = (width < GRID_MIN_WIDTH ? GRID_MIN_WIDTH : width);
			
			LEFT_OFFSET = 0;
			if (this.Task.LoopActions.Count > 0)
			{
				bool hasOnRight = false, hasOnLeft = false;
				
				foreach (LoopAction loopAction in this.Task.LoopActions)
				{
					if (loopAction.IsDrawnOnLeft == true)
					{
						hasOnLeft = true;
						LEFT_OFFSET = 20 + LOOP_ACTION_WIDTH;
					}
					else
					{
						hasOnRight = true;
					}
				}
				
				if (hasOnRight == true)
				{
					width += (20 + LOOP_ACTION_WIDTH);
				}
				if (hasOnLeft == true)
				{
					width += (20 + LOOP_ACTION_WIDTH);
				}
			}
			
			workflowDiagram.Width = width;
			workflowDiagram.Height = height;
		
			workflowDiagram.Children.Clear();
			
			DrawActionSubtree(startAction);
		}
		
		private void DrawActionSubtree(ActionBase startAction)
		{
			ActionBase currentAction = startAction;
			
			while (currentAction != null)
			{
				if (currentAction.GridNode != null)
				{
					break;
				}
			
				if (currentAction is Action)
				{
					Action currentActionNormal = (Action)currentAction;
					if (currentActionNormal.Previous == null || currentActionNormal.Next == null) // first or last
					{
						DrawAction(currentActionNormal, true);
					}
					else
					{
						DrawAction(currentActionNormal);
					}
					
					if (currentActionNormal.Next != null)
					{
						DrawArrow(currentActionNormal);
					}
					
					if (currentActionNormal.LoopAction != null)
					{
						if (currentActionNormal.LoopAction.IsDrawnOnLeft == true)
						{
							DrawLoopActionOnLeft(currentActionNormal.LoopAction);
						}
						else
						{
							DrawLoopAction(currentActionNormal.LoopAction);
						}
					}
					
					currentAction = currentActionNormal.Next;
				}
				else if (currentAction is ConditionalAction)
				{
					ConditionalAction currentActionConditional = (ConditionalAction)currentAction;
					DrawConditionalAction(currentActionConditional);
					
					DrawLeftArrow(currentActionConditional);
					DrawRightArrow(currentActionConditional);
					
					DrawActionSubtree(currentActionConditional.NextOnFalse);
					DrawActionSubtree(currentActionConditional.NextOnTrue);
					break;
				}
				else if (currentAction is EndAction)
				{
					DrawEndAction(currentAction);
					break;
				}
			}
		}
		
		private Arrow selectedArrow = null;
		public Arrow SelectedArrow
		{
			get
			{
				return this.selectedArrow;
			}
			set
			{
				if (value != null && this.SelectedAction != null)
				{
					// deselect any selected action
					this.SelectedAction = null;
				}
				
				if (value == null)
				{
					if (this.selectedArrow != null)
					{
						DrawUnselectedArrow(this.selectedArrow);
					}
					this.selectedArrow = null;
					return;
				}
				
				if (this.selectedArrow == value)
				{
					DrawUnselectedArrow(value);
					this.selectedArrow = null;
				}
				else
				{
					if (this.selectedArrow != null)
					{
						DrawUnselectedArrow(this.selectedArrow);
					}
					
					DrawSelectedArrow(value);
					this.selectedArrow = value;
				}
			}
		}
		
		private void DrawSelectedArrow(Arrow arrow)
		{
			arrow.Path.StrokeThickness = LINE_THICKNESS + 1;
			arrow.Path.Stroke = Brushes.Red;
		}
		
		private void DrawUnselectedArrow(Arrow arrow)
		{
			arrow.Path.StrokeThickness = LINE_THICKNESS;
			arrow.Path.Stroke = Brushes.Black;
		}
		
		private ActionBase selectedAction = null;
		public ActionBase SelectedAction
		{
			get
			{
				return this.selectedAction;
			}
			set
			{
				if (value != null && this.SelectedArrow != null)
				{
					// deselect any selected arrow
					DrawUnselectedArrow(this.SelectedArrow);
					this.SelectedArrow = null;
				}
				
				if (value == null)
				{
					if (this.selectedAction != null)
					{
						DrawUnselected(this.selectedAction);
					}
					this.selectedAction = null;
					
					btnEditAction.IsEnabled = false;
					btnDeleteAction.IsEnabled = false;
					
					btnCopy.IsEnabled = false;
					btnCut.IsEnabled = false;
					
					SelectedActionChanged();
					return;
				}
				
				if (this.selectedAction == value)
				{
					DrawUnselected(value);
					this.selectedAction = null;
					
					btnEditAction.IsEnabled = false;
					btnDeleteAction.IsEnabled = false;
					
					btnCopy.IsEnabled = false;
					btnCut.IsEnabled = false;
				}
				else
				{
					if (this.selectedAction != null)
					{
						DrawUnselected(this.selectedAction);
					}
					
					DrawSelected(value);
					this.selectedAction = value;
					
					btnEditAction.IsEnabled = true;
					btnDeleteAction.IsEnabled = true;
					
					btnCopy.IsEnabled = true;
					btnCut.IsEnabled = true;
				}
				
				SelectedActionChanged();
			}
		}
		
		public void SelectedActionChanged()
		{
			if (SelectedActionChangedEvent != null)
			{
				SelectedActionChangedEvent();
			}
		
			if (this.showActionProperties == false)
			{
				return; // do nothing if Action Properties is closed
			}
			
			if (this.selectedAction == null)
			{
				txbTopLevel.Text = "";
				txbElement.Text = "";
				txbParameters.Text = "";
			}
			else if (this.selectedAction is Action)
			{
				Action selectedActionNormal = (Action)this.selectedAction;
				if (selectedActionNormal.Element != null)
				{
					Element element = selectedActionNormal.Element;
					List<Element> allAncestors = new List<Element>();
					while (element != null)
					{
						allAncestors.Insert(0, element);
						element = element.Parent;
					}
					
					if (allAncestors.Count >= 2)
					{
						Element topLevelWindow = allAncestors[1];
						
						txbTopLevel.Inlines.Clear();
						if (topLevelWindow.Name != null)
						{
							txbTopLevel.Inlines.Add("Top level window text: ");
							txbTopLevel.Inlines.Add(new Run("\"" + topLevelWindow.GetName() + "\"") { FontWeight = FontWeights.Bold });
							txbTopLevel.Inlines.Add(", Class Name: ");
							txbTopLevel.Inlines.Add(new Run("\"" + topLevelWindow.ClassName + "\"") { FontWeight = FontWeights.Bold });
						}
						else
						{
							txbTopLevel.Inlines.Add("Top level window Class Name: ");
							txbTopLevel.Inlines.Add(new Run("\"" + topLevelWindow.ClassName + "\"") { FontWeight = FontWeights.Bold });
						}
					}
					
					element = selectedActionNormal.Element;
					
					txbElement.Inlines.Clear();
					if (element.Name != null)
					{
						txbElement.Inlines.Add("Element type: ");
						txbElement.Inlines.Add(new Run(element.ControlTypeName) { FontWeight = FontWeights.Bold });
						txbElement.Inlines.Add(", Element name: ");
						txbElement.Inlines.Add(new Run("\"" + element.GetName() + "\"") { FontWeight = FontWeights.Bold });
					}
					else
					{
						txbElement.Inlines.Add("Element type: ");
						txbElement.Inlines.Add(new Run(element.ControlTypeName) { FontWeight = FontWeights.Bold });
					}
				}
				else
				{
					txbTopLevel.Text = "";
					txbElement.Text = "";
				}
				
				txbParameters.Inlines.Clear();
				txbParameters.Inlines.Add("Action and parameters: ");
				txbParameters.Inlines.Add(new Run(selectedActionNormal.Description1 + " " + selectedActionNormal.Description2) { FontWeight = FontWeights.Bold });
			}
		}
		
		private void DrawSelected(ActionBase action)
		{
			foreach (FrameworkElement fwElement in action.GridNode.Children)
			{
				Rectangle rect = fwElement as Rectangle;
				Path path = fwElement as Path;
				TextBlock txb = fwElement as TextBlock;
				
				if (rect != null)
				{
					rect.Fill = Brushes.Blue;
				}
				else if (txb != null)
				{
					txb.Foreground = Brushes.Yellow;
				}
				else if (path != null)
				{
					path.Fill = Brushes.Blue;
				}
			}
		}
		
		private void DrawUnselected(ActionBase action)
		{
			foreach (FrameworkElement fwElement in action.GridNode.Children)
			{
				Rectangle rect = fwElement as Rectangle;
				Path path = fwElement as Path;
				TextBlock txb = fwElement as TextBlock;
				
				if (rect != null)
				{
					rect.Fill = Brushes.Orange;
				}
				else if (txb != null)
				{
					txb.Foreground = Brushes.Black;
				}
				else if (path != null)
				{
					path.Fill = Brushes.Orange;
				}
			}
		}
		
		private void OnGridNodeClick(object sender, MouseButtonEventArgs e)
		{
			try
			{
				TryOnGridNodeClick(sender, e);
			}
			catch { }
		}
		
		private void TryOnGridNodeClick(object sender, MouseButtonEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}
			
			ActionBase action = frameworkElement.Tag as ActionBase;
			if (action == null)
			{
				return;
			}
			
			if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
			{
				ActionDoubleClickedEvent(action);
				return;
			}
			
			if (this.SelectedAction != action)
			{
				this.SelectedAction = action;
			}
		}
		
		private void OnArrowClick(object sender, MouseButtonEventArgs e)
		{
			try
			{
				TryOnArrowClick(sender);
			}
			catch { }
		}
		
		private void TryOnArrowClick(object sender)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}
			
			Arrow arrow = frameworkElement.Tag as Arrow;
			if (arrow == null)
			{
				return;
			}
			
			if (this.SelectedArrow != arrow)
			{
				this.SelectedArrow = arrow;
			}
		}
		
		public void ChangeArrowDestination(Arrow arrow)
		{
			var changeDestinationWindow = new ChangeDestinationWindow(this);
			changeDestinationWindow.Owner = MainWindow.Instance;
			changeDestinationWindow.Closed += (sender2, e2) => 
			{
				if (changeDestinationWindow.OkWasPressed == false)
				{
					return;
				}
				
				if (this.SelectedAction == null)
				{
					return;
				}
				
				if (this.SelectedAction is EndAction)
				{
					MessageBox.Show(MainWindow.Instance, "End type not supported");
					return;
				}
				
				ChangeArrowDestination(arrow, this.SelectedAction);
				
				//HelpMessages.Show(MessageId.ChangeDestination);
			};
			
			changeDestinationWindow.Show();
		}
		
		private bool ChangeArrowDestination(Arrow arrow, ActionBase actionDestination)
		{
			if (actionDestination == arrow.PrevAction)
			{
				MessageBox.Show(MainWindow.Instance, "Start and end actions of the arrow cannot be the same");
				return false;
			}
		
			if (actionDestination == arrow.NextAction)
			{
				return false;
			}
			
			if (actionDestination.IsDescendentOf(arrow.NextAction))
			{
				bool dontAllow = false;
				ActionBase prevAction = arrow.PrevAction;
				if (prevAction is Action)
				{
					Action prevActionNormal = (Action)prevAction;
					if (prevActionNormal.HasWeakBond == false)
					{
						dontAllow = true;
					}
				}
				else if (prevAction is ConditionalAction)
				{
					ConditionalAction prevActionConditional = (ConditionalAction)prevAction;
					if (arrow.ArrowType == ArrowType.Left && prevActionConditional.HasOnFalseWeakBond == false)
					{
						dontAllow = true;
					}
					else if (arrow.ArrowType == ArrowType.Right && prevActionConditional.HasOnTrueWeakBond == false)
					{
						dontAllow = true;
					}
				}
				
				if (dontAllow == true)
				{
					MessageBox.Show(MainWindow.Instance, "Cannot change the arrow to point to this destination");
					return false;
				}
			}
		
			ActionBase start = arrow.PrevAction;

			if (start is ConditionalAction)
			{
				ConditionalAction startConditional = (ConditionalAction)start;
				if (arrow.ArrowType == ArrowType.Left)
				{
					if (actionDestination == startConditional.NextOnTrue)
					{
						MessageBox.Show(MainWindow.Instance, "Both True and False branches cannot point to the same action");
						return false;
					}
				}
				else
				{
					if (actionDestination == startConditional.NextOnFalse)
					{
						MessageBox.Show(MainWindow.Instance, "Both True and False branches cannot point to the same action");
						return false;
					}
				}
			}
			
			if (this.Task != null)
			{
				UndoRedo.AddSnapshot(this.Task);
			}
			
			if (start is Action)
			{
				Action startNormal = (Action)start;
				startNormal.Next = actionDestination;
				
				actionDestination.DeletedEvent += () => 
				{
					bool toAddEndAction = false;
					if (actionDestination is Action)
					{
						ActionBase nextAction = ((Action)actionDestination).Next;
						if (nextAction != null && !(nextAction is EndAction))
						{
							ChangeArrowDestination(arrow, nextAction);
						}
						else
						{
							toAddEndAction = true;
						}
					}
					if (actionDestination is ConditionalAction || toAddEndAction == true)
					{
						startNormal.Next = new EndAction();
						startNormal.Next.Previous = startNormal;
					}
				};
			}
			else if (start is ConditionalAction)
			{
				ConditionalAction startConditional = (ConditionalAction)start;
				if (arrow.ArrowType == ArrowType.Left)
				{
					startConditional.NextOnFalse = actionDestination;
					
					actionDestination.DeletedEvent += () => 
					{
						bool toAddEndAction = false;
						if (actionDestination is Action)
						{
							ActionBase nextAction = ((Action)actionDestination).Next;
							if (!(nextAction is EndAction))
							{
								ChangeArrowDestination(arrow, nextAction);
							}
							else
							{
								toAddEndAction = true;
							}
						}
						if (actionDestination is ConditionalAction || toAddEndAction == true)
						{
							startConditional.NextOnFalse = new EndAction();
							startConditional.NextOnFalse.Previous = startConditional;
						}
					};
				}
				else // arrow type is right
				{
					startConditional.NextOnTrue = actionDestination;
					
					actionDestination.DeletedEvent += () => 
					{
						bool toAddEndAction = false;
						if (actionDestination is Action)
						{
							ActionBase nextAction = ((Action)actionDestination).Next;
							if (!(nextAction is EndAction))
							{
								ChangeArrowDestination(arrow, nextAction);
							}
							else
							{
								toAddEndAction = true;
							}
						}
						if (actionDestination is ConditionalAction || toAddEndAction == true)
						{
							startConditional.NextOnTrue = new EndAction();
							startConditional.NextOnTrue.Previous = startConditional;
						}
					};
				}
			}
			
			if (this.Task != null)
			{
				this.Task.IsModified = true;
			}
			
			if (this.Task.StartAction != null)
			{
				this.Task.Changed();
			}
			
			return true;
		}
		
		public void DeleteArrow(Arrow arrow)
		{
			if (arrow.NextAction.Previous == arrow.PrevAction)
			{
				if (MessageBox.Show(MainWindow.Instance,
					"If you delete the selected arrow all actions that follows it will be deleted. Are you sure?",
					"", MessageBoxButton.YesNoCancel) != MessageBoxResult.Yes)
				{
					return;
				}
			}
			else
			{
				if (MessageBox.Show(MainWindow.Instance,
					"Are you sure you want to delete the selected arrow?",
					"", MessageBoxButton.YesNoCancel) != MessageBoxResult.Yes)
				{
					return;
				}
			}
			
			if (this.Task != null)
			{
				UndoRedo.AddSnapshot(this.Task);
			}
			
			if (arrow.ArrowType == ArrowType.Normal)
			{
				Action start = (Action)arrow.PrevAction;
				start.Next = new EndAction();
				start.Next.Previous = start;
			}
			else if (arrow.ArrowType == ArrowType.Left)
			{
				ConditionalAction start = (ConditionalAction)arrow.PrevAction;
				start.NextOnFalse = new EndAction();
				start.NextOnFalse.Previous = start;
			}
			else // Right arrow
			{
				ConditionalAction start = (ConditionalAction)arrow.PrevAction;
				start.NextOnTrue = new EndAction();
				start.NextOnTrue.Previous = start;
			}
			
			if (arrow == this.SelectedArrow)
			{
				this.SelectedArrow = null;
			}
			
			if (this.task != null)
			{
				this.task.IsModified = true;
				if (this.task.StartAction != null)
				{
					this.task.Changed();
				}
			}
		}
		
		public void EnablePaste(bool enabled)
		{
			btnPaste.IsEnabled = enabled;
		}
		
		public void ScrollToBottom()
		{
			scrollViewer.ScrollToBottom();
		}
		
		public void ScrollToTop()
		{
			scrollViewer.ScrollToTop();
		}
		
		private void OnHideActionsInfo(object sender, RoutedEventArgs e)
		{
			ShowActionProperties(false);
			HideActionsInfoEvent();
		}
		
		private bool showActionProperties = false;
		public void ShowActionProperties(bool show)
		{
			this.showActionProperties = show;
			
			if (show)
			{
				mainGrid.RowDefinitions[1].Height = new GridLength(57);
			}
			else
			{
				mainGrid.RowDefinitions[1].Height = new GridLength(0);
			}
		}
		
		private void OnViewConditions(object sender, RoutedEventArgs e)
		{
			MainWindow.Instance.SwitchToScreen(AppScreen.Conditions);
		}
		
		public void SetTaskRunningButtons(bool isRunning)
		{
			btnStop.IsEnabled = isRunning;
			btnPauseResume.IsEnabled = isRunning;
			
			if (isRunning == false)
			{
				if (btnPauseResume.Content == "Resume Task")
				{
					btnPauseResume.Content = "Pause Task";
					MainWindow.Instance.Paused(false);
				}
			}
		}
		
		public void PauseResume(Task task)
		{
			if (task.IsAttemptingPause == true)
			{
				return;
			}
		
			if (task.IsPaused == true)
			{
				//resume
				btnPauseResume.Content = "Pause Task";
				MainWindow.Instance.Paused(false);
				task.Resume();
			}
			else
			{
				//pause
				task.OnPauseSucceeded += () => 
				{
					Application.Current.Dispatcher.Invoke( () =>
					{
						// Code to run on the GUI thread.
						btnPauseResume.Content = "Resume Task";
						MainWindow.Instance.Paused(true);
					} );
				};
				task.Pause();
			}
		}
		
		private void OnWorkflowClick(object sender, RoutedEventArgs e)
		{
			try
			{
				TryOnWorkflowClick();
			}
			catch { }
		}
		
		private void TryOnWorkflowClick()
		{
			if (scrollViewer.IsMouseDirectlyOver == true)
			{
				if (this.SelectedAction != null)
				{
					this.SelectedAction = null;
				}
				
				if (this.SelectedArrow != null)
				{
					this.SelectedArrow = null;
				}
			}
		}
		
		public delegate void HideActionsInfo();
		public event HideActionsInfo HideActionsInfoEvent;
		public delegate void ActionDoubleClicked(ActionBase action);
		public event ActionDoubleClicked ActionDoubleClickedEvent;
		public event System.Action SelectedActionChangedEvent;
    }
	
	public class Arrow
	{
		public Path Path { get; set; }
		public ActionBase PrevAction { get; set; }
		public ActionBase NextAction { get; set; }
		public ArrowType ArrowType { get; set; }
		
		public Arrow()
		{
			Path = null;
			PrevAction = null;
			NextAction = null;
			ArrowType = ArrowType.Normal;
		}
		
		public bool HasDestinationChanged
		{
			get
			{
				return (this.NextAction.Previous != this.PrevAction);
			}
		}
	}
	
	public enum ArrowType
	{
		Normal,
		Left,
		Right
	}
}
