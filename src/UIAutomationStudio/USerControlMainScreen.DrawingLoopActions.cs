using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;

namespace UIAutomationStudio
{
	public partial class UserControlMainScreen : UserControl
	{
		private const int LOOP_ACTION_WIDTH = 100;
		private const int LOOP_ACTION_HEIGHT = 70;
		
		public void DrawLoopAction(LoopAction loopAction)
		{
			int startNodeLeft = 0;
			int startNodeTop = 0;
			GetNodeLeftTop(loopAction.StartAction, out startNodeLeft, out startNodeTop);
			startNodeLeft += LEFT_RIGHT_PADDING;
			int startNodeRight = startNodeLeft + NODE_WIDTH;
			int startNodeBottom = startNodeTop + NODE_HEIGHT;
			
			int loopNodeLeft = 0, loopNodeTop = 0;
			
			if (loopAction.StartAction == loopAction.EndAction)
			{
				// special case
				loopNodeTop = startNodeTop + (NODE_HEIGHT - LOOP_ACTION_HEIGHT) / 2;
				loopNodeLeft = startNodeRight + 20;
				
				DrawLoopSpecial(loopAction, startNodeLeft, startNodeTop, startNodeRight, loopNodeLeft, loopNodeTop);
				return;
			}
			
			int endNodeLeft = 0;
			int endNodeTop = 0;
			GetNodeLeftTop(loopAction.EndAction, out endNodeLeft, out endNodeTop);
			endNodeLeft += LEFT_RIGHT_PADDING;
			int endNodeRight = endNodeLeft + NODE_WIDTH;
			
			if (endNodeLeft < startNodeLeft)
			{
				// special case
				return;
			}
			
			loopNodeTop = (startNodeBottom + endNodeTop) / 2 - LOOP_ACTION_HEIGHT / 2;
			int loopNodeBottom = loopNodeTop + LOOP_ACTION_HEIGHT;
			loopNodeLeft = endNodeRight + 20;
			
			// draw line
			Path path = new Path() { StrokeThickness = LINE_THICKNESS, Stroke = System.Windows.Media.Brushes.Black };
			path.Margin = new Thickness(LEFT_OFFSET + endNodeRight, UserControlMainScreen.TOP_OFFSET + loopNodeBottom, 0, 0);
				
			PathFigure pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(0, endNodeTop - loopNodeBottom + NODE_HEIGHT / 2);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(20 + LOOP_ACTION_WIDTH / 2, endNodeTop - loopNodeBottom + NODE_HEIGHT / 2);
			LineSegment lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point(20 + LOOP_ACTION_WIDTH / 2, 0);
			
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			
			workflowDiagram.Children.Add(path);
			
			// draw arrow
			path = new Path() { StrokeThickness = LINE_THICKNESS, Stroke = System.Windows.Media.Brushes.Black };
			path.Margin = new Thickness(LEFT_OFFSET + startNodeRight, 
				UserControlMainScreen.TOP_OFFSET + startNodeTop + NODE_HEIGHT / 2 - 5, 0, 0);
				
			pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(0, 5);
			lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(loopNodeLeft - startNodeRight + LOOP_ACTION_WIDTH / 2, 5);
			lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point(loopNodeLeft - startNodeRight + LOOP_ACTION_WIDTH / 2, 
				5 + NODE_HEIGHT / 2 + loopNodeTop - startNodeBottom);
				
			pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigure pathFigure2 = new PathFigure();
			pathFigure2.StartPoint = new Point(5, 0);
			LineSegment lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(0, 5);
			LineSegment lineSegment4 = new LineSegment();
			lineSegment4.Point = new Point(5, 10);
			
			PathSegmentCollection pathSegmentCollection2 = new PathSegmentCollection();
			pathSegmentCollection2.Add(lineSegment3);
			pathSegmentCollection2.Add(lineSegment4);
			pathFigure2.Segments = pathSegmentCollection2;
			
			pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			pathFigureCollection.Add(pathFigure2);
			
			pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			
			workflowDiagram.Children.Add(path);
			
			// DRAW THE LOOP NODE
			DrawLoopNode(loopAction, loopNodeLeft, loopNodeTop);
		}
		
		public void DrawLoopActionOnLeft(LoopAction loopAction)
		{
			int startNodeLeft = 0;
			int startNodeTop = 0;
			GetNodeLeftTop(loopAction.StartAction, out startNodeLeft, out startNodeTop);
			startNodeLeft += LEFT_RIGHT_PADDING;
			int startNodeRight = startNodeLeft + NODE_WIDTH;
			int startNodeBottom = startNodeTop + NODE_HEIGHT;
			
			int loopNodeLeft = 0, loopNodeTop = 0;
			
			if (loopAction.StartAction == loopAction.EndAction)
			{
				// special case
				loopNodeTop = startNodeTop + (NODE_HEIGHT - LOOP_ACTION_HEIGHT) / 2;
				loopNodeLeft = startNodeLeft - 20 - LOOP_ACTION_WIDTH;
				
				DrawLoopSpecialOnLeft(loopAction, startNodeLeft, startNodeTop, startNodeRight, 
					loopNodeLeft, loopNodeTop);
				return;
			}
			
			int endNodeLeft = 0;
			int endNodeTop = 0;
			GetNodeLeftTop(loopAction.EndAction, out endNodeLeft, out endNodeTop);
			endNodeLeft += LEFT_RIGHT_PADDING;
			int endNodeRight = endNodeLeft + NODE_WIDTH;
			
			if (endNodeLeft > startNodeLeft)
			{
				// special case
				return;
			}
			
			loopNodeTop = (startNodeBottom + endNodeTop) / 2 - LOOP_ACTION_HEIGHT / 2;
			int loopNodeBottom = loopNodeTop + LOOP_ACTION_HEIGHT;
			loopNodeLeft = endNodeLeft - 20 - LOOP_ACTION_WIDTH;
			
			// draw line
			Path path = new Path() { StrokeThickness = LINE_THICKNESS, Stroke = System.Windows.Media.Brushes.Black };
			path.Margin = new Thickness(LEFT_OFFSET + loopNodeLeft + LOOP_ACTION_WIDTH / 2, 
				UserControlMainScreen.TOP_OFFSET + loopNodeBottom, 0, 0);
				
			PathFigure pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(0, 0);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(0, endNodeTop - loopNodeBottom + NODE_HEIGHT / 2);
			LineSegment lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point(20 + LOOP_ACTION_WIDTH / 2, endNodeTop - loopNodeBottom + NODE_HEIGHT / 2);
			
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			
			workflowDiagram.Children.Add(path);
			
			// draw arrow
			path = new Path() { StrokeThickness = LINE_THICKNESS, Stroke = System.Windows.Media.Brushes.Black };
			path.Margin = new Thickness(LEFT_OFFSET + endNodeLeft - 20 - LOOP_ACTION_WIDTH / 2, 
				UserControlMainScreen.TOP_OFFSET + startNodeTop + NODE_HEIGHT / 2 - 5, 0, 0);
				
			pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(0, loopNodeTop - startNodeTop - NODE_HEIGHT / 2 + 5);
			lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(0, 5);
			lineSegment2 = new LineSegment();
			int arrowPointX = startNodeLeft - loopNodeLeft - LOOP_ACTION_WIDTH / 2;
			lineSegment2.Point = new Point(arrowPointX, 5);
				
			pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigure pathFigure2 = new PathFigure();
			pathFigure2.StartPoint = new Point(arrowPointX - 5, 0);
			LineSegment lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(arrowPointX, 5);
			LineSegment lineSegment4 = new LineSegment();
			lineSegment4.Point = new Point(arrowPointX - 5, 10);
			
			PathSegmentCollection pathSegmentCollection2 = new PathSegmentCollection();
			pathSegmentCollection2.Add(lineSegment3);
			pathSegmentCollection2.Add(lineSegment4);
			pathFigure2.Segments = pathSegmentCollection2;
			
			pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			pathFigureCollection.Add(pathFigure2);
			
			pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			
			workflowDiagram.Children.Add(path);
			
			// DRAW THE LOOP NODE
			DrawLoopNode(loopAction, loopNodeLeft, loopNodeTop);
		}
		
		public void DrawLoopNode(LoopAction loopAction, int loopNodeLeft, int loopNodeTop)
		{
			Grid gridNode = new Grid() { VerticalAlignment = VerticalAlignment.Top, 
				HorizontalAlignment = HorizontalAlignment.Left };
			
			gridNode.Width = LOOP_ACTION_WIDTH;
			gridNode.Height = LOOP_ACTION_HEIGHT;
			
			gridNode.Margin = new Thickness(LEFT_OFFSET + loopNodeLeft, TOP_OFFSET + loopNodeTop, 0, 0);
			gridNode.Tag = loopAction;
			loopAction.GridNode = gridNode;
			
			gridNode.MouseLeftButtonDown += new MouseButtonEventHandler(OnGridNodeClick);
			workflowDiagram.Children.Add(gridNode);
			
			Rectangle rect = new Rectangle() { StrokeThickness = 1, Stroke = System.Windows.Media.Brushes.Gray };
			if (loopAction != this.selectedAction)
			{
				rect.Fill = Brushes.Orange;
			}
			
			gridNode.Children.Add(rect);
			
			TextBlock txb1 = new TextBlock() { TextAlignment = TextAlignment.Center, 
					VerticalAlignment = VerticalAlignment.Top, FontWeight = FontWeights.Bold };
			txb1.Height = 20;

			txb1.Text = "Loop";
			
			TextBlock txb2 = new TextBlock() { TextAlignment = TextAlignment.Center, 
					VerticalAlignment = VerticalAlignment.Top, FontWeight = FontWeights.Bold };
			txb2.Height = 20;
			
			if (loopAction is LoopConditional)
			{
				txb1.Margin = new Thickness(0, 5, 0, 0);
				txb2.Margin = new Thickness(0, 25, 0, 0);
				txb2.Text = "Conditional";
			}
			else
			{
				txb1.Margin = new Thickness(0, 10, 0, 0);
				txb2.Margin = new Thickness(0, 35, 0, 0);
				LoopCount loopCount = (LoopCount)loopAction;
				txb2.Text = loopCount.InitialCount.ToString() + " times";
			}

			gridNode.Children.Add(txb1);
			gridNode.Children.Add(txb2);
			
			if (loopAction is LoopConditional)
			{
				TextBlock txb3 = new TextBlock() { TextAlignment = TextAlignment.Center, 
					VerticalAlignment = VerticalAlignment.Top };
				txb3.Height = 20;
				txb3.Margin = new Thickness(0, 45, 0, 0);
				txb3.Text = "See tooltip...";
				
				LoopConditional loopConditional = (LoopConditional)loopAction;
				string tooltip = "Loop while " + loopConditional.ConditionalAction.GetDescription(true);
				
				gridNode.ToolTip = tooltip;
				ToolTipService.SetShowDuration(gridNode, 12000);
				
				gridNode.Children.Add(txb3);
			}
			
			if (loopAction == this.selectedAction)
			{
				DrawSelected(loopAction);
			}
		}
		
		public const int DISTANCE_GO_UP = 15;
		
		public void DrawLoopSpecial(LoopAction loopAction, int startNodeLeft, int startNodeTop, int startNodeRight,
			int loopNodeLeft, int loopNodeTop)
		{
			Path path = new Path() { StrokeThickness = LINE_THICKNESS, Stroke = System.Windows.Media.Brushes.Black };
			path.Margin = new Thickness(LEFT_OFFSET + startNodeRight, 
				UserControlMainScreen.TOP_OFFSET + startNodeTop + NODE_HEIGHT / 2, 0, 0);
				
			var pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(0, 0);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(20, 0);
			
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			workflowDiagram.Children.Add(path);
			
			// draw arrow
			path = new Path() { StrokeThickness = LINE_THICKNESS, Stroke = System.Windows.Media.Brushes.Black };
			path.Margin = new Thickness(LEFT_OFFSET + startNodeLeft + NODE_WIDTH / 2 + 15, 
				UserControlMainScreen.TOP_OFFSET + startNodeTop - DISTANCE_GO_UP, 0, 0);
				
			pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(5, DISTANCE_GO_UP);
			lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(5, 0);
			var lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point(NODE_WIDTH / 2 - 15 + 20 + LOOP_ACTION_WIDTH / 2, 0);
			var lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(NODE_WIDTH / 2 - 15 + 20 + LOOP_ACTION_WIDTH / 2, 
				DISTANCE_GO_UP + loopNodeTop - startNodeTop);
			
			pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathSegmentCollection1.Add(lineSegment3);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigure pathFigure2 = new PathFigure();
			pathFigure2.StartPoint = new Point(0, DISTANCE_GO_UP - 5);
			var lineSegment4 = new LineSegment();
			lineSegment4.Point = new Point(5, DISTANCE_GO_UP);
			var lineSegment5 = new LineSegment();
			lineSegment5.Point = new Point(10, DISTANCE_GO_UP - 5);
			
			PathSegmentCollection pathSegmentCollection2 = new PathSegmentCollection();
			pathSegmentCollection2.Add(lineSegment4);
			pathSegmentCollection2.Add(lineSegment5);
			pathFigure2.Segments = pathSegmentCollection2;
			
			pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			pathFigureCollection.Add(pathFigure2);
			
			pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			workflowDiagram.Children.Add(path);
			
			DrawLoopNode(loopAction, loopNodeLeft, loopNodeTop);
		}
		
		public void DrawLoopSpecialOnLeft(LoopAction loopAction, int startNodeLeft, int startNodeTop, int startNodeRight,
			int loopNodeLeft, int loopNodeTop)
		{
			Path path = new Path() { StrokeThickness = LINE_THICKNESS, Stroke = System.Windows.Media.Brushes.Black };
			path.Margin = new Thickness(LEFT_OFFSET + startNodeLeft - 20, 
				UserControlMainScreen.TOP_OFFSET + startNodeTop + NODE_HEIGHT / 2, 0, 0);
				
			var pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(0, 0);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(20, 0);
			
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			workflowDiagram.Children.Add(path);
			
			// draw arrow
			path = new Path() { StrokeThickness = LINE_THICKNESS, Stroke = System.Windows.Media.Brushes.Black };
			path.Margin = new Thickness(LEFT_OFFSET + startNodeLeft - 20 - LOOP_ACTION_WIDTH / 2, 
				TOP_OFFSET + startNodeTop - DISTANCE_GO_UP, 0, 0);
				
			pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(0, DISTANCE_GO_UP + loopNodeTop - startNodeTop);
			lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(0, 0);
			var lineSegment2 = new LineSegment();
			int arrowPointX = LOOP_ACTION_WIDTH / 2 + NODE_WIDTH / 2;
			lineSegment2.Point = new Point(arrowPointX, 0);
			var lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(arrowPointX, DISTANCE_GO_UP);
			
			pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathSegmentCollection1.Add(lineSegment3);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigure pathFigure2 = new PathFigure();
			pathFigure2.StartPoint = new Point(arrowPointX - 5, DISTANCE_GO_UP - 5);
			var lineSegment4 = new LineSegment();
			lineSegment4.Point = new Point(arrowPointX, DISTANCE_GO_UP);
			var lineSegment5 = new LineSegment();
			lineSegment5.Point = new Point(arrowPointX + 5, DISTANCE_GO_UP - 5);
			
			PathSegmentCollection pathSegmentCollection2 = new PathSegmentCollection();
			pathSegmentCollection2.Add(lineSegment4);
			pathSegmentCollection2.Add(lineSegment5);
			pathFigure2.Segments = pathSegmentCollection2;
			
			pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			pathFigureCollection.Add(pathFigure2);
			
			pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			workflowDiagram.Children.Add(path);
			
			DrawLoopNode(loopAction, loopNodeLeft, loopNodeTop);
		}
	}
}