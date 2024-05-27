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
		public static int TOP_OFFSET = 5;
		public static int LEFT_OFFSET = 0;
	
		public void DrawEndAction(ActionBase endAction)
		{
			Grid gridNode = new Grid();
			gridNode.Width = NODE_WIDTH_WITH_PADDING;
			gridNode.Height = NODE_HEIGHT;
			
			gridNode.VerticalAlignment = VerticalAlignment.Top;
			gridNode.HorizontalAlignment = HorizontalAlignment.Left;
			
			int nodeLeft = 0;
			int nodeTop = 0;
			GetNodeLeftTop(endAction, out nodeLeft, out nodeTop);
			
			gridNode.Margin = new Thickness(LEFT_OFFSET + nodeLeft, TOP_OFFSET + nodeTop, 0, 5);
			gridNode.Tag = endAction;
			endAction.GridNode = gridNode;
			
			workflowDiagram.Children.Add(gridNode);
			
			Ellipse circle = new Ellipse { Width = NODE_HEIGHT / 2, Height = NODE_HEIGHT / 2, Stroke = Brushes.Black, 
				Fill = Brushes.Orange };
			circle.VerticalAlignment = VerticalAlignment.Top;
			gridNode.Children.Add(circle);
			
			TextBlock txb1 = new TextBlock() { Width = 50, Height = 20, FontWeight = FontWeights.Bold, 
				TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
			txb1.Text = "End";
			
			txb1.Margin = new Thickness(0, 15, 0, 0);
			gridNode.Children.Add(txb1);
		}
		
		public void DrawConditionalAction(ConditionalAction conditionalAction)
		{
			Grid gridNode = new Grid();
			
			gridNode.ContextMenu = GetConditionalContextMenu();
			
			gridNode.Width = NODE_WIDTH_WITH_PADDING;
			gridNode.Height = NODE_HEIGHT;
			
			gridNode.VerticalAlignment = VerticalAlignment.Top;
			gridNode.HorizontalAlignment = HorizontalAlignment.Left;
			
			int nodeLeft = 0;
			int nodeTop = 0;
			GetNodeLeftTop(conditionalAction, out nodeLeft, out nodeTop);
			
			gridNode.Margin = new Thickness(LEFT_OFFSET + nodeLeft, TOP_OFFSET + nodeTop, 0, 5);
			gridNode.Tag = conditionalAction;
			conditionalAction.GridNode = gridNode;
			
			gridNode.MouseLeftButtonDown += new MouseButtonEventHandler(OnGridNodeClick);
			gridNode.MouseRightButtonDown += new MouseButtonEventHandler(OnGridNodeClick);
			
			//if (conditionalAction.Previous == null)
			{
				gridNode.AllowDrop = true;
				gridNode.DragEnter += OnActionDragEnter;
				gridNode.DragLeave += OnActionDragLeave;
				gridNode.Drop += OnActionDrop;
			}
			
			workflowDiagram.Children.Add(gridNode);
			
			// create path
			Path path = new Path() { StrokeThickness = 1, Stroke = System.Windows.Media.Brushes.Black };
			
			if (conditionalAction != this.selectedAction)
			{
				//path.Fill = Brushes.Orange;
				path.Fill = Brushes.LightGreen;
			}
			
			PathFigure pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(LEFT_RIGHT_PADDING + NODE_WIDTH / 2, 0);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(LEFT_RIGHT_PADDING + NODE_WIDTH, NODE_HEIGHT / 2);
			LineSegment lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point(LEFT_RIGHT_PADDING + NODE_WIDTH / 2, NODE_HEIGHT);
			LineSegment lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(LEFT_RIGHT_PADDING, NODE_HEIGHT / 2);
			LineSegment lineSegment4 = new LineSegment();
			lineSegment4.Point = new Point(LEFT_RIGHT_PADDING + NODE_WIDTH / 2, 0);
			
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathSegmentCollection1.Add(lineSegment3);
			pathSegmentCollection1.Add(lineSegment4);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			string tooltip = conditionalAction.GetDescription(true);
			path.ToolTip = tooltip;
			gridNode.Children.Add(path);
			
			TextBlock txb1 = new TextBlock() 
			{
				TextTrimming = TextTrimming.CharacterEllipsis, 
				FontWeight = FontWeights.Bold,
				TextAlignment = TextAlignment.Center,
				VerticalAlignment = VerticalAlignment.Top
			};
			txb1.Width = 140;
			txb1.Height = 20;
			txb1.Text = conditionalAction.Name;
			
			txb1.Margin = new Thickness(0, 25, 0, 0);
			gridNode.Children.Add(txb1);
			
			TextBlock txb2 = new TextBlock()
			{
				TextTrimming = TextTrimming.CharacterEllipsis,
				TextAlignment = TextAlignment.Center,
				VerticalAlignment = VerticalAlignment.Top
			};
			txb2.Width = 170;
			txb2.Height = 20;
			txb2.Text = conditionalAction.GetDescription();
			
			txb2.Margin = new Thickness(0, 45, 0, 0);
			txb2.ToolTip = tooltip;
			gridNode.Children.Add(txb2);
			
			if (conditionalAction == this.selectedAction)
			{
				DrawSelected(conditionalAction);
			}
		}
		
		private ContextMenu GetConditionalContextMenu()
		{
			ContextMenu contextMenu = new ContextMenu();
			
			MenuItem evaluateMenuItem = new MenuItem();
			evaluateMenuItem.Command = MyCommands.EvaluateConditionalActionCommand;
			evaluateMenuItem.Header = "Evaluate";
			contextMenu.Items.Add(evaluateMenuItem);
			
			MenuItem editMenuItem = new MenuItem();
			editMenuItem.Command = MyCommands.EditActionCommand;
			contextMenu.Items.Add(editMenuItem);
			
			MenuItem deleteMenuItem = new MenuItem();
			deleteMenuItem.Command = MyCommands.DeleteActionCommand;
			contextMenu.Items.Add(deleteMenuItem);
			
			MenuItem runStartingMenuItem = new MenuItem();
			runStartingMenuItem.Command = MyCommands.RunStartingCommand;
			contextMenu.Items.Add(runStartingMenuItem);
			
			return contextMenu;
		}
		
		public void DrawAction(Action action, bool isFirstOrLast = false)
		{
			Grid gridNode = new Grid();
			
			gridNode.ContextMenu = GetContextMenu();
			
			gridNode.Width = NODE_WIDTH_WITH_PADDING;
			gridNode.Height = NODE_HEIGHT;
			gridNode.VerticalAlignment = VerticalAlignment.Top;
			gridNode.HorizontalAlignment = HorizontalAlignment.Left;
			
			int nodeLeft = 0, nodeTop = 0;
			GetNodeLeftTop(action, out nodeLeft, out nodeTop);
			
			gridNode.Margin = new Thickness(LEFT_OFFSET + nodeLeft, TOP_OFFSET + nodeTop, 0, 5);
			gridNode.Tag = action;
			action.GridNode = gridNode;
			
			gridNode.MouseLeftButtonDown += new MouseButtonEventHandler(OnGridNodeClick);
			gridNode.MouseRightButtonDown += new MouseButtonEventHandler(OnGridNodeClick);
			gridNode.MouseMove += new MouseEventHandler(OnMouseMove);
			
			// enable drop only for start and end nodes
			//if (action.Previous == null || action.Next == null)
			{
				gridNode.AllowDrop = true;
				gridNode.DragEnter += OnActionDragEnter;
				gridNode.DragLeave += OnActionDragLeave;
				gridNode.Drop += OnActionDrop;
			}
			
			workflowDiagram.Children.Add(gridNode);
			
			Rectangle rect = new Rectangle()
			{
				StrokeThickness = 1,
				Stroke = System.Windows.Media.Brushes.Gray
			};
			
			if (action != this.selectedAction)
			{
				if (isFirstOrLast)
				{
					rect.Fill = Brushes.Orange;
				}
				else
				{
					rect.Fill = Brushes.LightBlue;
				}
			}
			
			if (isFirstOrLast)
			{
				rect.RadiusX = 20;
				rect.RadiusY = 50;
			}
			rect.Width = NODE_WIDTH;

			gridNode.Children.Add(rect);
			
			// text block 1 ////////////////
			TextBlock txb1 = new TextBlock() 
			{
				FontWeight = FontWeights.Bold,
				TextAlignment = TextAlignment.Center,
				VerticalAlignment = VerticalAlignment.Top
			};
			
			txb1.Width = 200;
			txb1.Height = 20;
			txb1.Text = action.Description1;
			txb1.Margin = new Thickness(0, 5, 0, 0);
			gridNode.Children.Add(txb1);
			//////////////////////////////
			
			// text block 2 ////////////////
			TextBlock txb2 = new TextBlock()
			{
				TextTrimming = TextTrimming.CharacterEllipsis,
				FontWeight = FontWeights.Bold,
				TextAlignment = TextAlignment.Center,
				VerticalAlignment = VerticalAlignment.Top
			};
			
			txb2.Width = 210;
			txb2.Height = 20;
			txb2.Text = action.Description2;
			
			txb2.Margin = new Thickness(0, 25, 0, 0);
			gridNode.Children.Add(txb2);
			//////////////////////////////
			
			if (action.Element != null)
			{
				// text block 3 ////////////////
				TextBlock txb3 = new TextBlock() 
				{
					TextAlignment = TextAlignment.Center,
					VerticalAlignment = VerticalAlignment.Top
				};
				txb3.Width = 210;
				txb3.Height = 20;
				txb3.Inlines.Add("on element of type ");
				txb3.Inlines.Add(new Run(action.Element.ControlTypeName) { FontWeight = FontWeights.Bold });
				
				txb3.Margin = new Thickness(0, 45, 0, 0);
				gridNode.Children.Add(txb3);
				///////////////////////////////
				
				// text block 4 ////////////////
				TextBlock txb4 = new TextBlock()
				{
					TextTrimming = TextTrimming.CharacterEllipsis,
					TextAlignment = TextAlignment.Center,
					VerticalAlignment = VerticalAlignment.Top
				};
				txb4.Width = 210;
				txb4.Height = 20;
				string text = action.Element.GetShortName(30);
				if (text == "")
				{
					txb4.Inlines.Add("with no name");
				}
				else
				{
					txb4.Inlines.Add("named ");
					txb4.Inlines.Add(new Run(text) { FontWeight = FontWeights.Bold });
				}
				
				txb4.Margin = new Thickness(0, 65, 0, 0);
				gridNode.Children.Add(txb4);
				///////////////////////////////
			}
			else
			{
				if (action.ActionId == ActionIds.StartProcessAndWaitForInputIdle)
				{
					// text block 3 ////////////////
					TextBlock txb3 = new TextBlock()
					{
						TextAlignment = TextAlignment.Center,
						VerticalAlignment = VerticalAlignment.Top
					};
					txb3.Width = 210;
					txb3.Height = 20;
					txb3.Inlines.Add("and wait for input idle");
					
					txb3.Margin = new Thickness(0, 45, 0, 0);
					gridNode.Children.Add(txb3);
					///////////////////////////////
				}
			}
			
			if (action == this.selectedAction)
			{
				DrawSelected(action);
			}
		}
		
		private ContextMenu GetContextMenu()
		{
			ContextMenu contextMenu = new ContextMenu();
			
			MenuItem editMenuItem = new MenuItem();
			editMenuItem.Command = MyCommands.EditActionCommand;
			contextMenu.Items.Add(editMenuItem);
			
			MenuItem deleteMenuItem = new MenuItem();
			deleteMenuItem.Command = MyCommands.DeleteActionCommand;
			contextMenu.Items.Add(deleteMenuItem);
			
			MenuItem runMenuItem = new MenuItem();
			runMenuItem.Command = MyCommands.RunSelectedActionCommand;
			contextMenu.Items.Add(runMenuItem);
			
			MenuItem runStartingMenuItem = new MenuItem();
			runStartingMenuItem.Command = MyCommands.RunStartingCommand;
			contextMenu.Items.Add(runStartingMenuItem);
			
			MenuItem copyMenuItem = new MenuItem();
			copyMenuItem.Command = ApplicationCommands.Copy;
			contextMenu.Items.Add(copyMenuItem);
			
			MenuItem cutMenuItem = new MenuItem();
			cutMenuItem.Command = ApplicationCommands.Cut;
			contextMenu.Items.Add(cutMenuItem);
			
			MenuItem highlightMenuItem = new MenuItem();
			highlightMenuItem.Command = MyCommands.HighlightCommand;
			contextMenu.Items.Add(highlightMenuItem);
			
			MenuItem propertiesMenuItem = new MenuItem();
			propertiesMenuItem.Command = MyCommands.PropertiesCommand;
			contextMenu.Items.Add(propertiesMenuItem);
			
			return contextMenu;
		}
		
		private void GetNodeLeftTop(ActionBase action, out int nodeLeft, out int nodeTop)
		{
			nodeLeft = action.Column * NODE_WIDTH_WITH_PADDING + 
				action.ColumnSpan * NODE_WIDTH_WITH_PADDING / 2 - NODE_WIDTH_WITH_PADDING / 2;
			
			nodeTop = action.Row * NODE_AND_ARROW_HEIGHT + 5;
		}
		
		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			Grid gridNode = sender as Grid;
			if (gridNode != null && e.LeftButton == MouseButtonState.Pressed)
			{
				actionDragged = gridNode.Tag as Action;
				DragDrop.DoDragDrop(gridNode, "move action", DragDropEffects.Move);
				return;
			}
		}
		
		private Brush previousStroke = null;
		private Action actionDragged = null;
		
		private void OnActionDragEnter(object sender, DragEventArgs e)
		{
			try
			{
				TryOnActionDragEnter(sender, e);
			}
			catch { }
		}
		
		private void TryOnActionDragEnter(object sender, DragEventArgs e)
		{
			Grid grid = sender as Grid;
			if (grid == null)
			{
				return;
			}
			
			Rectangle rect = grid.Children[0] as Rectangle;
			if (rect == null)
			{
				return;
			}
			
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
			
			rect.Stroke = Brushes.Black;
			rect.StrokeThickness = 3;
		}
		
		private void OnActionDragLeave(object sender, DragEventArgs e)
		{
			Grid grid = sender as Grid;
			if (grid == null)
			{
				return;
			}
			
			try
			{
				DrawActionWithoutBorder(grid);
			}
			catch { }
		}
		
		private void DrawActionWithoutBorder(Grid grid)
		{
			Rectangle rect = grid.Children[0] as Rectangle;
			if (rect == null)
			{
				return;
			}

			rect.Stroke = Brushes.Gray;
			rect.StrokeThickness = 1;
		}
		
		private void OnActionDrop(object sender, DragEventArgs e)
		{
			try
			{
				TryOnActionDrop(sender, e);
			}
			catch { }
		}
		
		private void TryOnActionDrop(object sender, DragEventArgs e)
		{
			if (actionDragged == null)
			//if (actionDragged == null && arrowDragged == null)
			{
				return;
			}
		
			Grid grid = sender as Grid;
			if (grid == null)
			{
				return;
			}
			
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

			ActionBase targetAction = grid.Tag as ActionBase;
			if (targetAction == null)
			{
				return;
			}
			
			if (str == "move action")
			{
				if (targetAction == actionDragged)
				{
					DrawActionWithoutBorder(grid);
					return;
				}
			
				if (targetAction.Previous != null)
				{
					DrawActionWithoutBorder(grid);
					
					MessageBox.Show(MainWindow.Instance, "You cannot drop an action here. You can drop an action over an arrow to move it between the two ends of the arrow or you can drop it over the first action if you want the dragged action to become the first action of the task.");
					return;
				}
			}
			
			UndoRedo.AddSnapshot(this.task);
			
			bool droppedOverStart = (targetAction.Previous == null);
			this.task.RemoveAction(actionDragged, false);
			
			if (droppedOverStart) // drop over the start action
			{
				actionDragged.Previous = null;
				actionDragged.Next = targetAction;
				targetAction.Previous = actionDragged;
				
				this.task.StartAction = actionDragged;
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