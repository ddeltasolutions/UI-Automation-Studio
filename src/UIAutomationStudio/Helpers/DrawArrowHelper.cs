using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace UIAutomationStudio
{
	public class DrawArrowHelper
	{
		private static int INSERT_OFFSET = 12;
		
		public static Path CreateArrowPathRightTopLeft(int nodeLeft, int nodeTop, int nextNodeLeft, int nextNodeTop)
		{
			Path path = new Path();
			
			path.Margin = new Thickness(UserControlMainScreen.LEFT_OFFSET + nextNodeLeft + UserControlMainScreen.NODE_WIDTH / 2 - 5, 
				UserControlMainScreen.TOP_OFFSET + nextNodeTop - INSERT_OFFSET, 0, 0);
				
			PathFigure pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(5, INSERT_OFFSET);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(5, 0);
			LineSegment lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point(5 + UserControlMainScreen.NODE_WIDTH / 2 + nodeLeft - nextNodeLeft + 10, 0);
			LineSegment lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(5 + UserControlMainScreen.NODE_WIDTH / 2 + nodeLeft - nextNodeLeft + 10, 
				INSERT_OFFSET + nodeTop - nextNodeTop + UserControlMainScreen.NODE_HEIGHT / 2);
			LineSegment lineSegment4 = new LineSegment();
			lineSegment4.Point = new Point(5 + UserControlMainScreen.NODE_WIDTH / 2 + nodeLeft - nextNodeLeft, 
				INSERT_OFFSET + nodeTop - nextNodeTop + UserControlMainScreen.NODE_HEIGHT / 2);
				
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathSegmentCollection1.Add(lineSegment3);
			pathSegmentCollection1.Add(lineSegment4);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigure pathFigure2 = new PathFigure();
			pathFigure2.StartPoint = new Point(0, INSERT_OFFSET - 5);
			LineSegment lineSegment5 = new LineSegment();
			lineSegment5.Point = new Point(5, INSERT_OFFSET);
			LineSegment lineSegment6 = new LineSegment();
			lineSegment6.Point = new Point(10, INSERT_OFFSET - 5);
			
			PathSegmentCollection pathSegmentCollection2 = new PathSegmentCollection();
			pathSegmentCollection2.Add(lineSegment5);
			pathSegmentCollection2.Add(lineSegment6);
			pathFigure2.Segments = pathSegmentCollection2;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			pathFigureCollection.Add(pathFigure2);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			return path;
		}
		
		public static Path CreateArrowPathLeftTopRight(int nodeLeft, int nodeTop, int nextNodeLeft, int nextNodeTop)
		{
			Path path = new Path();
			
			int arrowHeight = INSERT_OFFSET + nodeTop - nextNodeTop + UserControlMainScreen.NODE_HEIGHT / 2;
			int arrowWidth = 10 + nextNodeLeft - nodeLeft + UserControlMainScreen.NODE_WIDTH / 2;
			
			path.Margin = new Thickness(UserControlMainScreen.LEFT_OFFSET + nodeLeft - 10, 
				UserControlMainScreen.TOP_OFFSET + nextNodeTop - INSERT_OFFSET, 0, 0);
			
			PathFigure pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(10, arrowHeight);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(0, arrowHeight);
			LineSegment lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point(0, 0);
			LineSegment lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(arrowWidth, 0);
			LineSegment lineSegment4 = new LineSegment();
			lineSegment4.Point = new Point(arrowWidth, INSERT_OFFSET);
			
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathSegmentCollection1.Add(lineSegment3);
			pathSegmentCollection1.Add(lineSegment4);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigure pathFigure2 = new PathFigure();
			pathFigure2.StartPoint = new Point(arrowWidth - 5, INSERT_OFFSET - 5);
			LineSegment lineSegment5 = new LineSegment();
			lineSegment5.Point = new Point(arrowWidth, INSERT_OFFSET);
			LineSegment lineSegment6 = new LineSegment();
			lineSegment6.Point = new Point(arrowWidth + 5, INSERT_OFFSET - 5);
			
			PathSegmentCollection pathSegmentCollection2 = new PathSegmentCollection();
			pathSegmentCollection2.Add(lineSegment5);
			pathSegmentCollection2.Add(lineSegment6);
			pathFigure2.Segments = pathSegmentCollection2;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			pathFigureCollection.Add(pathFigure2);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			return path;
		}
		
		public static Path CreateArrowPathLeft(int nodeLeft, int nodeTop, int nextNodeLeft, int nextNodeTop)
		{
			Path path = new Path();
			int nextNodeRight = nextNodeLeft + UserControlMainScreen.NODE_WIDTH;
			
			path.Margin = new Thickness(UserControlMainScreen.LEFT_OFFSET + nextNodeLeft + UserControlMainScreen.NODE_WIDTH / 2 - 5, 
				UserControlMainScreen.TOP_OFFSET + nextNodeTop - 10, 0, 0);
				
			PathFigure pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(5, 10);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(5, 0);
			LineSegment lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point(5 + UserControlMainScreen.NODE_WIDTH / 2 + (nodeLeft - nextNodeRight) / 2, 0);
			LineSegment lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(5 + UserControlMainScreen.NODE_WIDTH / 2 + (nodeLeft - nextNodeRight) / 2,
				10 + nodeTop - nextNodeTop + UserControlMainScreen.NODE_HEIGHT / 2);
			LineSegment lineSegment4 = new LineSegment();
			lineSegment4.Point = new Point(5 + UserControlMainScreen.NODE_WIDTH / 2 + nodeLeft - nextNodeRight,
				10 + nodeTop - nextNodeTop + UserControlMainScreen.NODE_HEIGHT / 2);
			
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathSegmentCollection1.Add(lineSegment3);
			pathSegmentCollection1.Add(lineSegment4);
			pathFigure1.Segments = pathSegmentCollection1;
		
			PathFigure pathFigure2 = new PathFigure();
			pathFigure2.StartPoint = new Point(0, 5);
			LineSegment lineSegment5 = new LineSegment();
			lineSegment5.Point = new Point(5, 10);
			LineSegment lineSegment6 = new LineSegment();
			lineSegment6.Point = new Point(10, 5);
			
			PathSegmentCollection pathSegmentCollection2 = new PathSegmentCollection();
			pathSegmentCollection2.Add(lineSegment5);
			pathSegmentCollection2.Add(lineSegment6);
			pathFigure2.Segments = pathSegmentCollection2;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			pathFigureCollection.Add(pathFigure2);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			return path;
		}
		
		public static Path CreateArrowPathRight(int nodeLeft, int nodeTop, int nextNodeLeft, int nextNodeTop)
		{
			Path path = new Path();
			int nodeRight = nodeLeft + UserControlMainScreen.NODE_WIDTH;
			int arrowWidth = nextNodeLeft - nodeRight + UserControlMainScreen.NODE_WIDTH / 2;
			
			path.Margin = new Thickness(UserControlMainScreen.LEFT_OFFSET + nodeRight, UserControlMainScreen.TOP_OFFSET + nextNodeTop - 10, 0, 0);
			
			PathFigure pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(0, 10 + nodeTop - nextNodeTop + UserControlMainScreen.NODE_HEIGHT / 2);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point((nextNodeLeft - nodeRight) / 2, 
				10 + nodeTop - nextNodeTop + UserControlMainScreen.NODE_HEIGHT / 2);
			LineSegment lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point((nextNodeLeft - nodeRight) / 2, 0);
			LineSegment lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(arrowWidth, 0);
			LineSegment lineSegment4 = new LineSegment();
			lineSegment4.Point = new Point(arrowWidth, 10);
			
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathSegmentCollection1.Add(lineSegment3);
			pathSegmentCollection1.Add(lineSegment4);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigure pathFigure2 = new PathFigure();
			pathFigure2.StartPoint = new Point(arrowWidth - 5, 5);
			LineSegment lineSegment5 = new LineSegment();
			lineSegment5.Point = new Point(arrowWidth, 10);
			LineSegment lineSegment6 = new LineSegment();
			lineSegment6.Point = new Point(arrowWidth + 5, 5);
			
			PathSegmentCollection pathSegmentCollection2 = new PathSegmentCollection();
			pathSegmentCollection2.Add(lineSegment5);
			pathSegmentCollection2.Add(lineSegment6);
			pathFigure2.Segments = pathSegmentCollection2;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			pathFigureCollection.Add(pathFigure2);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			return path;
		}
		
		public static Path CreateArrowPathDownLeft(int nodeLeft, int nodeTop, int nextNodeLeft, int nextNodeTop)
		{
			Path path = new Path();
			int arrowHeight = nextNodeTop - nodeTop - UserControlMainScreen.NODE_HEIGHT / 2;
			int arrowWidth = nodeLeft - nextNodeLeft - UserControlMainScreen.NODE_WIDTH / 2;
			
			path.Margin = new Thickness(UserControlMainScreen.LEFT_OFFSET + nextNodeLeft + UserControlMainScreen.NODE_WIDTH / 2 - 5, 
				UserControlMainScreen.TOP_OFFSET + nodeTop + UserControlMainScreen.NODE_HEIGHT / 2, 0, 0);
			
			PathFigure pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(5, arrowHeight);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(5, arrowHeight - 10);
			LineSegment lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point(5 + arrowWidth - 10, arrowHeight - 10);
			LineSegment lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(5 + arrowWidth - 10, 0);
			LineSegment lineSegment4 = new LineSegment();
			lineSegment4.Point = new Point(5 + arrowWidth, 0);
			
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathSegmentCollection1.Add(lineSegment3);
			pathSegmentCollection1.Add(lineSegment4);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigure pathFigure2 = new PathFigure();
			pathFigure2.StartPoint = new Point(0, arrowHeight - 5);
			LineSegment lineSegment5 = new LineSegment();
			lineSegment5.Point = new Point(5, arrowHeight);
			LineSegment lineSegment6 = new LineSegment();
			lineSegment6.Point = new Point(10, arrowHeight - 5);
			
			PathSegmentCollection pathSegmentCollection2 = new PathSegmentCollection();
			pathSegmentCollection2.Add(lineSegment5);
			pathSegmentCollection2.Add(lineSegment6);
			pathFigure2.Segments = pathSegmentCollection2;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			pathFigureCollection.Add(pathFigure2);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			return path;
		}
		
		public static Path CreateArrowPathDownRight(int nodeLeft, int nodeTop, int nextNodeLeft, int nextNodeTop)
		{
			Path path = new Path();
			int nodeRight = nodeLeft + UserControlMainScreen.NODE_WIDTH;
			int arrowWidth = UserControlMainScreen.NODE_WIDTH / 2 + nextNodeLeft - nodeRight;
			int arrowHeight = nextNodeTop - nodeTop - UserControlMainScreen.NODE_HEIGHT / 2;
			
			path.Margin = new Thickness(UserControlMainScreen.LEFT_OFFSET + nodeRight, 
				UserControlMainScreen.TOP_OFFSET + nodeTop + UserControlMainScreen.NODE_HEIGHT / 2, 0, 0);
				
			PathFigure pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(0, 0);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(10, 0);
			LineSegment lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point(10, arrowHeight - 10);
			LineSegment lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(arrowWidth, arrowHeight - 10);
			LineSegment lineSegment4 = new LineSegment();
			lineSegment4.Point = new Point(arrowWidth, arrowHeight);
				
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathSegmentCollection1.Add(lineSegment3);
			pathSegmentCollection1.Add(lineSegment4);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigure pathFigure2 = new PathFigure();
			pathFigure2.StartPoint = new Point(arrowWidth - 5, arrowHeight - 5);
			LineSegment lineSegment5 = new LineSegment();
			lineSegment5.Point = new Point(arrowWidth, arrowHeight);
			LineSegment lineSegment6 = new LineSegment();
			lineSegment6.Point = new Point(arrowWidth + 5, arrowHeight - 5);
		
			PathSegmentCollection pathSegmentCollection2 = new PathSegmentCollection();
			pathSegmentCollection2.Add(lineSegment5);
			pathSegmentCollection2.Add(lineSegment6);
			pathFigure2.Segments = pathSegmentCollection2;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			pathFigureCollection.Add(pathFigure2);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			return path;
		}
		
		public static Path CreateArrowPathDown(int nodeLeft, int nodeTop, int nextNodeLeft, int nextNodeTop)
		{
			int height = nextNodeTop - nodeTop - UserControlMainScreen.NODE_HEIGHT;
		
			Path path = new Path();
			path.Margin = new Thickness(UserControlMainScreen.LEFT_OFFSET + nodeLeft + UserControlMainScreen.NODE_WIDTH / 2 - 5, 
				UserControlMainScreen.TOP_OFFSET + nodeTop + UserControlMainScreen.NODE_HEIGHT, 0, 0);
			
			PathFigure pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(5, 0);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(5, height);
			LineSegment lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point(0, height - 5);
			
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigure pathFigure2 = new PathFigure();
			pathFigure2.StartPoint = new Point(5, height);
			LineSegment lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(10, height - 5);
			
			PathSegmentCollection pathSegmentCollection2 = new PathSegmentCollection();
			pathSegmentCollection2.Add(lineSegment3);
			pathFigure2.Segments = pathSegmentCollection2;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			pathFigureCollection.Add(pathFigure2);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			return path;
		}
		
		public static Path CreateRegularLeftArrow(int nodeLeft, int nodeTop, int nextNodeLeft, int nextNodeTop)
		{
			int startLeft = nodeLeft + UserControlMainScreen.LEFT_RIGHT_PADDING;
			int startTop = nodeTop + UserControlMainScreen.NODE_HEIGHT / 2;
			
			int endLeft = nextNodeLeft + UserControlMainScreen.NODE_WIDTH / 2 + 
				UserControlMainScreen.LEFT_RIGHT_PADDING;
			int endTop = nextNodeTop;
			
			int boundingRectWidth = 5 + startLeft - endLeft;
			int boundingRectHeight = endTop - startTop;
			
			Path path = new Path();
			path.Margin = new Thickness(UserControlMainScreen.LEFT_OFFSET + endLeft - 5, UserControlMainScreen.TOP_OFFSET + startTop, 0, 0);
			
			PathFigure pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(boundingRectWidth, 0);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(5, 0);
			LineSegment lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point(5, boundingRectHeight);
			LineSegment lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(0, boundingRectHeight - 5);
			
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathSegmentCollection1.Add(lineSegment3);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigure pathFigure2 = new PathFigure();
			pathFigure2.StartPoint = new Point(5, boundingRectHeight);
			LineSegment lineSegment4 = new LineSegment();
			lineSegment4.Point = new Point(10, boundingRectHeight - 5);
			
			PathSegmentCollection pathSegmentCollection2 = new PathSegmentCollection();
			pathSegmentCollection2.Add(lineSegment4);
			pathFigure2.Segments = pathSegmentCollection2;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			pathFigureCollection.Add(pathFigure2);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			return path;
		}
		
		public static Path CreateRegularRightArrow(int nodeLeft, int nodeTop, int nextNodeLeft, int nextNodeTop)
		{
			int startLeft = nodeLeft + UserControlMainScreen.LEFT_RIGHT_PADDING + UserControlMainScreen.NODE_WIDTH;
			int startTop = nodeTop + UserControlMainScreen.NODE_HEIGHT / 2;
		
			int endLeft = nextNodeLeft + UserControlMainScreen.LEFT_RIGHT_PADDING + UserControlMainScreen.NODE_WIDTH / 2;
			int endTop = nextNodeTop;
			
			int boundingRectWidth = 5 + endLeft - startLeft;
			int boundingRectHeight = endTop - startTop;
			
			Path path = new Path();
			path.Margin = new Thickness(UserControlMainScreen.LEFT_OFFSET + startLeft, UserControlMainScreen.TOP_OFFSET + startTop, 0, 0);
			
			PathFigure pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(0, 0);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(boundingRectWidth - 5, 0);
			LineSegment lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point(boundingRectWidth - 5, boundingRectHeight);
			LineSegment lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(boundingRectWidth - 10, boundingRectHeight - 5);
			
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathSegmentCollection1.Add(lineSegment3);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigure pathFigure2 = new PathFigure();
			pathFigure2.StartPoint = new Point(boundingRectWidth - 5, boundingRectHeight);
			LineSegment lineSegment4 = new LineSegment();
			lineSegment4.Point = new Point(boundingRectWidth, boundingRectHeight - 5);
			
			PathSegmentCollection pathSegmentCollection2 = new PathSegmentCollection();
			pathSegmentCollection2.Add(lineSegment4);
			pathFigure2.Segments = pathSegmentCollection2;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			pathFigureCollection.Add(pathFigure2);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			return path;
		}
		
		public static Path CreateRightArrowPathDownLeft(int nodeLeft, int nodeTop, int nextNodeLeft, int nextNodeTop)
		{
			Path path = new Path();
			int arrowHeight = nextNodeTop - nodeTop - UserControlMainScreen.NODE_HEIGHT / 2;
			int arrowWidth = nodeLeft - nextNodeLeft + UserControlMainScreen.NODE_WIDTH / 2 + 10;
			
			path.Margin = new Thickness(UserControlMainScreen.LEFT_OFFSET + nextNodeLeft + UserControlMainScreen.NODE_WIDTH / 2 - 5, 
				UserControlMainScreen.TOP_OFFSET + nodeTop + UserControlMainScreen.NODE_HEIGHT / 2, 0, 0);
			
			PathFigure pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(5, arrowHeight);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(5, arrowHeight - 10);
			LineSegment lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point(5 + arrowWidth, arrowHeight - 10);
			LineSegment lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(5 + arrowWidth, 0);
			LineSegment lineSegment4 = new LineSegment();
			lineSegment4.Point = new Point(5 + arrowWidth - 10, 0);
			
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathSegmentCollection1.Add(lineSegment3);
			pathSegmentCollection1.Add(lineSegment4);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigure pathFigure2 = new PathFigure();
			pathFigure2.StartPoint = new Point(0, arrowHeight - 5);
			LineSegment lineSegment5 = new LineSegment();
			lineSegment5.Point = new Point(5, arrowHeight);
			LineSegment lineSegment6 = new LineSegment();
			lineSegment6.Point = new Point(10, arrowHeight - 5);
			
			PathSegmentCollection pathSegmentCollection2 = new PathSegmentCollection();
			pathSegmentCollection2.Add(lineSegment5);
			pathSegmentCollection2.Add(lineSegment6);
			pathFigure2.Segments = pathSegmentCollection2;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			pathFigureCollection.Add(pathFigure2);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			return path;
		}
		
		public static Path CreateLeftArrowPathDownRight(int nodeLeft, int nodeTop, int nextNodeLeft, int nextNodeTop)
		{
			Path path = new Path();
			int arrowHeight = nextNodeTop - nodeTop - UserControlMainScreen.NODE_HEIGHT / 2;
			int arrowWidth = nextNodeLeft - nodeLeft + UserControlMainScreen.NODE_WIDTH / 2 + 10;
			
			path.Margin = new Thickness(UserControlMainScreen.LEFT_OFFSET + nodeLeft - 10, 
				UserControlMainScreen.TOP_OFFSET + nodeTop + UserControlMainScreen.NODE_HEIGHT / 2, 0, 0);
			
			PathFigure pathFigure1 = new PathFigure();
			pathFigure1.StartPoint = new Point(10, 0);
			LineSegment lineSegment1 = new LineSegment();
			lineSegment1.Point = new Point(0, 0);
			LineSegment lineSegment2 = new LineSegment();
			lineSegment2.Point = new Point(0, arrowHeight - 10);
			LineSegment lineSegment3 = new LineSegment();
			lineSegment3.Point = new Point(arrowWidth, arrowHeight - 10);
			LineSegment lineSegment4 = new LineSegment();
			lineSegment4.Point = new Point(arrowWidth, arrowHeight);
			
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection();
			pathSegmentCollection1.Add(lineSegment1);
			pathSegmentCollection1.Add(lineSegment2);
			pathSegmentCollection1.Add(lineSegment3);
			pathSegmentCollection1.Add(lineSegment4);
			pathFigure1.Segments = pathSegmentCollection1;
			
			PathFigure pathFigure2 = new PathFigure();
			pathFigure2.StartPoint = new Point(arrowWidth - 5, arrowHeight - 5);
			LineSegment lineSegment5 = new LineSegment();
			lineSegment5.Point = new Point(arrowWidth, arrowHeight);
			LineSegment lineSegment6 = new LineSegment();
			lineSegment6.Point = new Point(arrowWidth + 5, arrowHeight - 5);
			
			PathSegmentCollection pathSegmentCollection2 = new PathSegmentCollection();
			pathSegmentCollection2.Add(lineSegment5);
			pathSegmentCollection2.Add(lineSegment6);
			pathFigure2.Segments = pathSegmentCollection2;
			
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure1);
			pathFigureCollection.Add(pathFigure2);
			
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			
			path.Data = pathGeometry;
			return path;
		}
	}
}