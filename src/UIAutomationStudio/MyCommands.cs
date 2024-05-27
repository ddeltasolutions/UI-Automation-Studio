using System.Windows.Input;

namespace UIAutomationStudio
{
	public class MyCommands
	{
		public static RoutedUICommand AddActionCommand;
		public static RoutedUICommand DeleteActionCommand;
		public static RoutedUICommand EditActionCommand;
		public static RoutedUICommand RunSelectedActionCommand;
		public static RoutedUICommand RunAllActionsCommand;
		public static RoutedUICommand OpenExeLocationCommand;
		public static RoutedUICommand PasteFirstCommand;
		public static RoutedUICommand PasteLastCommand;
		public static RoutedUICommand RunStartingCommand;
		public static RoutedUICommand HighlightCommand;
		public static RoutedUICommand ActionInfoCommand;
		public static RoutedUICommand ActionPropertiesCommand;
		public static RoutedUICommand SpeedCommand;
		public static RoutedUICommand WorkflowCommand;
		public static RoutedUICommand ConditionsCommand;
		public static RoutedUICommand AddConditionCommand;
		public static RoutedUICommand EditConditionCommand;
		public static RoutedUICommand DeleteConditionCommand;
		public static RoutedUICommand EvaluatePropertyCommand;
		public static RoutedUICommand EvaluateConditionCommand;
		public static RoutedUICommand EvaluateConditionalActionCommand;
		public static RoutedUICommand DuplicateConditionCommand;
		public static RoutedUICommand AddConditionalCommand;
		public static RoutedUICommand ChangeDestinationCommand;
		public static RoutedUICommand DeleteArrowCommand;
		public static RoutedUICommand RunSelectedAndMoveNextCommand;
		public static RoutedUICommand StopTaskCommand;
		public static RoutedUICommand PauseResumeTaskCommand;
		public static RoutedUICommand PropertiesCommand;
		public static RoutedUICommand ConditionPropertiesCommand;
		public static RoutedUICommand AddLoopCommand;
		
		static MyCommands()
		{
			InputGestureCollection igc = new InputGestureCollection();
			igc.Add(new KeyGesture(Key.A, ModifierKeys.Control));
			AddActionCommand = new RoutedUICommand("Add New Action", "AddAction", typeof(MyCommands), igc);
			
			igc = new InputGestureCollection();
			igc.Add(new KeyGesture(Key.Delete));
			DeleteActionCommand = new RoutedUICommand("Delete Action", "DeleteAction", typeof(MyCommands), igc);
			
			igc = new InputGestureCollection();
			igc.Add(new KeyGesture(Key.E, ModifierKeys.Control));
			EditActionCommand = new RoutedUICommand("Edit Action", "EditAction", typeof(MyCommands), igc);
			
			igc = new InputGestureCollection();
			igc.Add(new KeyGesture(Key.F4));
			RunSelectedActionCommand = new RoutedUICommand("Run Selected", "RunSelected", typeof(MyCommands), igc);
			
			igc = new InputGestureCollection();
			igc.Add(new KeyGesture(Key.F5));
			RunAllActionsCommand = new RoutedUICommand("Run All", "RunAll", typeof(MyCommands), igc);
			
			OpenExeLocationCommand = new RoutedUICommand("Open Exe Location", "OpenExeLocation", typeof(MyCommands));
			
			PasteFirstCommand = new RoutedUICommand("Paste First", "PasteFirst", typeof(MyCommands));
			PasteLastCommand = new RoutedUICommand("Paste Last", "PasteLast", typeof(MyCommands));
			
			igc = new InputGestureCollection();
			igc.Add(new KeyGesture(Key.F6));
			RunStartingCommand = new RoutedUICommand("Run Starting", "RunStarting", typeof(MyCommands), igc);
			
			HighlightCommand = new RoutedUICommand("Highlight", "Highlight", typeof(MyCommands));
			
			ActionInfoCommand = new RoutedUICommand("Action Information", "ActionInfo", typeof(MyCommands));
			ActionPropertiesCommand = new RoutedUICommand("Action Properties", "ActionProperties", typeof(MyCommands));
			SpeedCommand = new RoutedUICommand("Speed", "Speed", typeof(MyCommands));
			
			WorkflowCommand = new RoutedUICommand("Task Workflow", "Workflow", typeof(MyCommands));
			ConditionsCommand = new RoutedUICommand("Properties with Condition", "PropertiesWithCondition", typeof(MyCommands));
			AddConditionCommand = new RoutedUICommand("Add Property with Condition", "AddPropertyWithCondition", typeof(MyCommands));
			EditConditionCommand = new RoutedUICommand("Edit Property with Condition", "EditPropertyWithCondition", typeof(MyCommands));
			
			igc = new InputGestureCollection();
			igc.Add(new KeyGesture(Key.Delete));
			DeleteConditionCommand = new RoutedUICommand("Delete Property with Condition", "DeletePropertyWithCondition", typeof(MyCommands), igc);
			
			EvaluatePropertyCommand = new RoutedUICommand("Evaluate Property", "EvaluateProperty", typeof(MyCommands));
			EvaluateConditionCommand = new RoutedUICommand("Evaluate Condition", "EvaluateCondition", typeof(MyCommands));
			EvaluateConditionalActionCommand = new RoutedUICommand("Evaluate Conditional Action", "EvaluateConditionalAction", typeof(MyCommands));
			DuplicateConditionCommand = new RoutedUICommand("Duplicate Property with Condition", "DuplicateCondition", typeof(MyCommands));
			
			AddConditionalCommand = new RoutedUICommand("Add Conditional Statement", "AddConditional", typeof(MyCommands));
			ChangeDestinationCommand = new RoutedUICommand("Change Arrow Destination", "ChangeDestination", typeof(MyCommands));
			DeleteArrowCommand = new RoutedUICommand("Delete Arrow", "DeleteArrow", typeof(MyCommands));
			
			igc = new InputGestureCollection();
			igc.Add(new KeyGesture(Key.F10));
			RunSelectedAndMoveNextCommand = new RoutedUICommand("Run Selected And Move Next", 
				"RunSelectedAndMoveNext", typeof(MyCommands), igc);
				
			StopTaskCommand = new RoutedUICommand("Stop Task", "StopTask", typeof(MyCommands));
			PauseResumeTaskCommand = new RoutedUICommand("Pause Resume Task", "PauseResumeTask", typeof(MyCommands));
			
			igc = new InputGestureCollection();
			igc.Add(new KeyGesture(Key.Enter, ModifierKeys.Alt));
			PropertiesCommand = new RoutedUICommand("Properties", "Properties", typeof(MyCommands), igc);
			
			ConditionPropertiesCommand = new RoutedUICommand("Condition Properties", "ConditionProperties", typeof(MyCommands));
			
			igc = new InputGestureCollection();
			igc.Add(new KeyGesture(Key.L, ModifierKeys.Control));
			AddLoopCommand = new RoutedUICommand("Add Loop", "AddLoop", typeof(MyCommands), igc);
		}
	}
}