using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Documents;

namespace UIAutomationStudio
{
	public partial class UserControlMainScreen : UserControl
	{
		public void DrawRightArrow(ConditionalAction conditionalAction)
		{
			int nodeLeft = 0;
			int nodeTop = 0;
			GetNodeLeftTop(conditionalAction, out nodeLeft, out nodeTop);
			
			int nextNodeLeft = 0, nextNodeTop = 0;
			GetNodeLeftTop(conditionalAction.NextOnTrue, out nextNodeLeft, out nextNodeTop);
			
			int startLeft = nodeLeft + LEFT_RIGHT_PADDING + NODE_WIDTH;
			int startTop = nodeTop + NODE_HEIGHT / 2;
			
			Path path = null;
			bool arrowGoesUp = false;
			
			if (conditionalAction.NextOnTrue.Previous != conditionalAction)
			{
				nodeLeft += LEFT_RIGHT_PADDING;
				nextNodeLeft += LEFT_RIGHT_PADDING;
				
				if (nextNodeTop <= nodeTop)
				{
					path = DrawArrowHelper.CreateArrowPathRightTopLeft(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
					arrowGoesUp = true;
				}
				else if (nextNodeLeft <= nodeLeft)
				{
					path = DrawArrowHelper.CreateRightArrowPathDownLeft(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
				}
				else
				{
					path = DrawArrowHelper.CreateArrowPathDownRight(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
				}
			}
			else
			{
				path = DrawArrowHelper.CreateRegularRightArrow(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
			}
			
			path.StrokeThickness = LINE_THICKNESS;
			path.Stroke = System.Windows.Media.Brushes.Black;
			
			workflowDiagram.Children.Add(path);
			
			path.ContextMenu = GetArrowContextMenu();
			path.ContextMenu.PlacementTarget = path;
			
			// The conditional True goes on right branch
			Arrow arrow = new Arrow() { Path = path, PrevAction = conditionalAction, 
				NextAction = conditionalAction.NextOnTrue, ArrowType = ArrowType.Right };
			path.Tag = arrow;
			path.MouseLeftButtonDown += new MouseButtonEventHandler(OnArrowClick);
			
			path.AllowDrop = true;
			path.DragEnter += OnDragEnter;
			path.DragLeave += OnDragLeave;
			path.Drop += OnDrop;
			
			if (this.SelectedArrow != null && this.SelectedArrow.PrevAction == arrow.PrevAction && 
				this.SelectedArrow.NextAction == arrow.NextAction)
			{
				this.SelectedArrow = null;
				this.SelectedArrow = arrow;
			}
			
			TextBlock txb = new TextBlock { Text = "True", 
				Foreground = Brushes.Blue, FontWeight = FontWeights.Bold, 
				HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, 
				Width = 30, Height = 18 };
			
			int textTop = TOP_OFFSET + startTop - 20;
			if (arrowGoesUp == true)
			{
				textTop += 20;
			}
			txb.Margin = new Thickness(LEFT_OFFSET + startLeft - 5, textTop, 0, 0);
			
			workflowDiagram.Children.Add(txb);
		}
		
		public void DrawLeftArrow(ConditionalAction conditionalAction)
		{
			int nodeLeft = 0;
			int nodeTop = 0;
			GetNodeLeftTop(conditionalAction, out nodeLeft, out nodeTop);
			
			int nextNodeLeft = 0, nextNodeTop = 0;
			GetNodeLeftTop(conditionalAction.NextOnFalse, out nextNodeLeft, out nextNodeTop);
			
			int startLeft = nodeLeft + LEFT_RIGHT_PADDING;
			int startTop = nodeTop + NODE_HEIGHT / 2;
			
			Path path = null;
			bool arrowGoesUp = false;
			if (conditionalAction.NextOnFalse.Previous != conditionalAction)
			{
				nodeLeft += LEFT_RIGHT_PADDING;
				nextNodeLeft += LEFT_RIGHT_PADDING;
					
				if (nextNodeTop <= nodeTop)
				{
					path = DrawArrowHelper.CreateArrowPathLeftTopRight(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
					arrowGoesUp = true;
				}
				else if (nextNodeLeft >= nodeLeft)
				{
					path = DrawArrowHelper.CreateLeftArrowPathDownRight(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
				}
				else
				{
					path = DrawArrowHelper.CreateArrowPathDownLeft(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
				}
			}
			else
			{
				path = DrawArrowHelper.CreateRegularLeftArrow(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
			}
			
			path.StrokeThickness = LINE_THICKNESS;
			path.Stroke = System.Windows.Media.Brushes.Black;
			
			workflowDiagram.Children.Add(path);
			
			path.ContextMenu = GetArrowContextMenu();
			path.ContextMenu.PlacementTarget = path;
			
			// The conditional False goes on left branch
			Arrow arrow = new Arrow() { Path = path, PrevAction = conditionalAction, 
				NextAction = conditionalAction.NextOnFalse, ArrowType = ArrowType.Left };
			path.Tag = arrow;
			path.MouseLeftButtonDown += new MouseButtonEventHandler(OnArrowClick);
			
			path.AllowDrop = true;
			path.DragEnter += OnDragEnter;
			path.DragLeave += OnDragLeave;
			path.Drop += OnDrop;
			
			if (this.SelectedArrow != null && this.SelectedArrow.PrevAction == arrow.PrevAction && 
				this.SelectedArrow.NextAction == arrow.NextAction)
			{
				this.SelectedArrow = null;
				this.SelectedArrow = arrow;
			}
			
			TextBlock txb = new TextBlock { Text = "False", 
				Foreground = Brushes.Blue, FontWeight = FontWeights.Bold, 
				HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, 
				Width = 30, Height = 18 };
			
			int textTop = TOP_OFFSET + startTop - 20;
			if (arrowGoesUp == true)
			{
				textTop += 20;
			}
			txb.Margin = new Thickness(LEFT_OFFSET + startLeft - 20, textTop, 0, 0);
			
			workflowDiagram.Children.Add(txb);
		}
		
		public void DrawArrow(Action action)
		{
			int nodeLeft = 0;
			int nodeTop = 0;
			GetNodeLeftTop(action, out nodeLeft, out nodeTop);
			nodeLeft += LEFT_RIGHT_PADDING;
			int nodeRight = nodeLeft + NODE_WIDTH;
			
			int nextNodeLeft = 0;
			int nextNodeTop = 0;
			GetNodeLeftTop(action.Next, out nextNodeLeft, out nextNodeTop);
			nextNodeLeft += LEFT_RIGHT_PADDING;
			int nextNodeRight = nextNodeLeft + NODE_WIDTH;
			
			Path path = null;
			if (nextNodeTop < nodeTop)
			{
				if (nextNodeLeft <= nodeLeft)
				{
					if (action.Next.IsInHierarchyOf(action))
					{
						path = DrawArrowHelper.CreateArrowPathRightTopLeft(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
					}
					else
					{
						path = DrawArrowHelper.CreateArrowPathLeft(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
					}
				}
				else
				{
					if (action.Next.IsInHierarchyOf(action))
					{
						path = DrawArrowHelper.CreateArrowPathLeftTopRight(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
					}
					else
					{
						path = DrawArrowHelper.CreateArrowPathRight(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
					}
				}
			}
			else if (nextNodeTop == nodeTop)
			{
				if (nextNodeLeft < nodeLeft)
				{
					path = DrawArrowHelper.CreateArrowPathLeft(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
				}
				else
				{
					path = DrawArrowHelper.CreateArrowPathRight(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
				}
			}
			else
			{
				if (nextNodeLeft < nodeLeft)
				{
					// draw down left
					path = DrawArrowHelper.CreateArrowPathDownLeft(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
				}
				else if (nextNodeLeft > nodeLeft)
				{
					// draw down right
					path = DrawArrowHelper.CreateArrowPathDownRight(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
				}
				else
				{
					path = DrawArrowHelper.CreateArrowPathDown(nodeLeft, nodeTop, nextNodeLeft, nextNodeTop);
				}
			}
			
			path.StrokeThickness = LINE_THICKNESS;
			path.Stroke = System.Windows.Media.Brushes.Black;
			
			workflowDiagram.Children.Add(path);
			
			path.ContextMenu = GetArrowContextMenu();
			path.ContextMenu.PlacementTarget = path;
			
			Arrow arrow = new Arrow() { Path = path, PrevAction = action, NextAction = action.Next };
			path.Tag = arrow;
			path.MouseLeftButtonDown += new MouseButtonEventHandler(OnArrowClick);
			
			path.AllowDrop = true;
			path.DragEnter += OnDragEnter;
			path.DragLeave += OnDragLeave;
			path.Drop += OnDrop;
			
			if (this.SelectedArrow != null && this.SelectedArrow.PrevAction == arrow.PrevAction && 
				this.SelectedArrow.NextAction == arrow.NextAction)
			{
				this.SelectedArrow = null;
				this.SelectedArrow = arrow;
			}
		}
		
		private ContextMenu GetArrowContextMenu()
		{
			ContextMenu contextMenu = new ContextMenu();
			
			MenuItem pasteMenuItem = new MenuItem();
			pasteMenuItem.Command = ApplicationCommands.Paste;
			contextMenu.Items.Add(pasteMenuItem);
			
			MenuItem addActionMenuItem = new MenuItem();
			addActionMenuItem.Command = MyCommands.AddActionCommand;
			contextMenu.Items.Add(addActionMenuItem);
			
			MenuItem changeDestinationMenuItem = new MenuItem();
			changeDestinationMenuItem.Header = "Change Destination";
			changeDestinationMenuItem.Click += (sender1, e1) => 
			{
				Path path = contextMenu.PlacementTarget as Path;
				if (path == null)
				{
					return;
				}
				
				Arrow arrow = path.Tag as Arrow;
				if (arrow == null)
				{
					return;
				}
			
				try
				{
					ChangeArrowDestination(arrow);
				}
				catch (Exception ex)
				{
					MessageBox.Show(MainWindow.Instance, "Change destination failed: " + ex.Message);
				}
			};
			contextMenu.Items.Add(changeDestinationMenuItem);
			
			MenuItem deleteArrowMenuItem = new MenuItem();
			deleteArrowMenuItem.Header = "Delete Arrow";
			deleteArrowMenuItem.Click += (sender1, e1) => 
			{
				Path path = contextMenu.PlacementTarget as Path;
				if (path == null)
				{
					return;
				}
				
				Arrow arrow = path.Tag as Arrow;
				if (arrow == null)
				{
					return;
				}
			
				try
				{
					DeleteArrow(arrow);
				}
				catch (Exception ex)
				{
					MessageBox.Show(MainWindow.Instance, "Delete arrow failed: " + ex.Message);
				}
			};
			contextMenu.Items.Add(deleteArrowMenuItem);
			
			return contextMenu;
		}
		
		private void OnDragEnter(object sender, DragEventArgs e)
		{
			Path path = sender as Path;
			if (path == null)
			{
				return;
			}
			
			previousStroke = path.Stroke;

			// If the DataObject contains string data, extract it.
			if (!e.Data.GetDataPresent(DataFormats.StringFormat))
			{
				return;
			}
			
			string str = e.Data.GetData(DataFormats.StringFormat) as string;
			if (str == null || str != "move action")
			//if (str == null || (str != "move action" && str != "move arrow"))
			{
				return;
			}
			
			path.Stroke = Brushes.Red;
		}
		
		private void OnDragLeave(object sender, DragEventArgs e)
		{
			Path path = sender as Path;
			if (path == null)
			{
				return;
			}
			
			path.Stroke = previousStroke;
		}
		
		private void OnDrop(object sender, DragEventArgs e)
		{
			if (actionDragged == null)
			//if (actionDragged == null && arrowDragged == null)
			{
				return;
			}
			
			if (!e.Data.GetDataPresent(DataFormats.StringFormat))
			{
				return;
			}
			
			string str = e.Data.GetData(DataFormats.StringFormat) as string;
			//if (str == null || str != "move action")
			if (str == null)
			{
				return;
			}
			
			Path path = sender as Path;
			if (path == null)
			{
				return;
			}

			Arrow arrow = path.Tag as Arrow;
			if (arrow == null)
			{
				return;
			}
			
			if (str != "move action")
			{
				return;
			}
			
			if (actionDragged == arrow.PrevAction || actionDragged == arrow.NextAction)
			{
				path.Stroke = previousStroke;
				return;
			}
			
			UndoRedo.AddSnapshot(this.task);
			
			this.task.RemoveAction(actionDragged, false);
			
			ActionBase prevAction = arrow.PrevAction;
			ActionBase nextAction = arrow.NextAction;
			
			if (prevAction is Action)
			{
				Action prevActionNormal = (Action)prevAction;
				prevActionNormal.Next = actionDragged;
			}
			else if (prevAction is ConditionalAction)
			{
				ConditionalAction prevActionConditional = (ConditionalAction)prevAction;
				if (prevActionConditional.NextOnFalse == nextAction)
				{
					// insert in the left branch
					prevActionConditional.NextOnFalse = actionDragged;
				}
				else if (prevActionConditional.NextOnTrue == nextAction)
				{
					// insert in the right branch
					prevActionConditional.NextOnTrue = actionDragged;
				}
			}
			actionDragged.Previous = prevAction;
			actionDragged.Next = nextAction;
			if (arrow.HasDestinationChanged == false)
			{
				nextAction.Previous = actionDragged;
			}
			
			this.task.IsModified = true;
			this.task.Changed();
			
			if (this.SelectedAction != actionDragged)
			{
				this.SelectedAction = actionDragged;
			}
		}
	}
}