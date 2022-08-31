using System;
using UIDeskAutomationLib;
using UIAutomationClient;
using System.Collections.Generic;
using System.Xml;
using System.Windows;

namespace UIAutomationStudio
{
	public class Action: ActionBase
	{
		public ActionBase Next { get; set; }
		public Element Element { get; set; }
		public ActionIds ActionId { get; set; }
		public List<object> Parameters { get; set; }
		
		public LoopAction LoopAction { get; set; }
		
		public Action()
		{
			Next = null;
			Element = null;
			Parameters = null;
			LoopAction = null;
		}
		
		public Action(IUIAutomationElement element)
		{
			Next = null;
			Element = new Element(element);
			Parameters = null;
			LoopAction = null;
		}
		
		private string description1 = null;
		
		public string Description2 { get; set; }
		
		public string Description1
		{
			get
			{
				if (description1 != null)
				{
					return description1;
				}
				Description2 = null;
				
				if (this.ActionId == ActionIds.ClickAt)
				{
					if (Element != null)
					{
						description1 = "Click at";
					}
					else
					{
						description1 = "Click at screen coordinates";
					}
					Description2 = "(" + Parameters[0] + ", " + Parameters[1] + ")";
				}
				else if (this.ActionId == ActionIds.SimulateClickAt)
				{
					if (Element != null)
					{
						description1 = "Simulate Click at";
						Description2 = "(" + Parameters[0] + ", " + Parameters[1] + ")";
					}
					else
					{
						description1 = "Simulate Click at";
						Description2 = "screen coordinates (" + Parameters[0] + ", " + Parameters[1] + ")";
					}
				}
				else if (ActionId == ActionIds.RightClickAt)
				{
					if (Element != null)
					{
						description1 = "Right Click at";
					}
					else
					{
						description1 = "Right Click at screen coordinates";
					}
					Description2 = "(" + Parameters[0] + ", " + Parameters[1] + ")";
				}
				else if (ActionId == ActionIds.SimulateRightClickAt)
				{
					if (Element != null)
					{
						description1 = "Simulate Right Click at";
						Description2 = "(" + Parameters[0] + ", " + Parameters[1] + ")";
					}
					else
					{
						description1 = "Simulate Right Click at";
						Description2 = "screen coordinates (" + Parameters[0] + ", " + Parameters[1] + ")";
					}
				}
				else if (ActionId == ActionIds.MiddleClickAt)
				{
					if (Element != null)
					{
						description1 = "Middle Click at";
					}
					else
					{
						description1 = "Middle Click at screen coordinates";
					}
					Description2 = "(" + Parameters[0] + ", " + Parameters[1] + ")";
				}
				else if (ActionId == ActionIds.SimulateMiddleClickAt)
				{
					if (Element != null)
					{
						description1 = "Simulate Middle Click at";
						Description2 = "(" + Parameters[0] + ", " + Parameters[1] + ")";
					}
					else
					{
						description1 = "Simulate Middle Click at";
						Description2 = "screen coordinates (" + Parameters[0] + ", " + Parameters[1] + ")";
					}
				}
				else if (ActionId == ActionIds.DoubleClickAt)
				{
					if (Element != null)
					{
						description1 = "Double Click at";
					}
					else
					{
						description1 = "Double Click at screen coordinates";
					}
					Description2 = "(" + Parameters[0] + ", " + Parameters[1] + ")";
				}
				else if (ActionId == ActionIds.SimulateDoubleClickAt)
				{
					if (Element != null)
					{
						description1 = "Simulate Double Click at";
						Description2 = "(" + Parameters[0] + ", " + Parameters[1] + ")";
					}
					else
					{
						description1 = "Simulate Double Click at";
						Description2 = "screen coordinates (" + Parameters[0] + ", " + Parameters[1] + ")";
					}
				}
				else if (ActionId == ActionIds.MoveMouse)
				{
					description1 = "Move mouse cursor at";
					if (Element != null)
					{
						Description2 = "(" + Parameters[0] + ", " + Parameters[1] + ")";
					}
					else
					{
						Description2 = "screen coordinates (" + Parameters[0] + ", " + Parameters[1] + ")";
					}
				}
				else if (ActionId == ActionIds.MouseScrollUp)
				{
					description1 = "Scroll the mouse Up";
					Description2 = "with " + Parameters[0] + " ticks";
				}
				else if (ActionId == ActionIds.MouseScrollDown)
				{
					description1 = "Scroll the mouse Down";
					Description2 = "with " + Parameters[0] + " ticks";
				}
				else if (ActionId == ActionIds.SendKeys)
				{
					description1 = "Send keys";
					Description2 = "\"" + Parameters[0] + "\"";
				}
				else if (ActionId == ActionIds.SimulateSendKeys)
				{
					description1 = "Simulate Send keys";
					Description2 = "\"" + Parameters[0] + "\"";
				}
				else if (ActionId == ActionIds.KeyDown)
				{
					description1 = "Press " + Parameters[0] + " key";
				}
				else if (ActionId == ActionIds.KeyPress)
				{
					description1 = "Press and release";
					Description2 = Parameters[0] + " key";
				}
				else if (ActionId == ActionIds.KeysPress)
				{
					string keys = "";
					foreach (object key in Parameters)
					{
						VirtualKeys virtKey = (VirtualKeys)key;
						if (keys != "")
						{
							keys += ",";
						}
						keys += virtKey;
					}
					description1 = "Press and release";
					Description2 = keys + " keys";
				}
				else if (ActionId == ActionIds.KeyUp)
				{
					description1 = "Release " + Parameters[0] + " key";
				}
				else if (ActionId == ActionIds.PredefinedKeysCombination)
				{
					description1 = "Send";
					Description2 = Parameters[0].ToString();
				}
				else if (ActionId == ActionIds.StartProcess || ActionId == ActionIds.StartProcessAndWaitForInputIdle)
				{
					string fileName = Parameters[0].ToString();
					string parameters = "";
					if (Parameters.Count >= 2)
					{
						parameters = Parameters[1].ToString();
					}
					
					description1 = "Start Process";
					if (parameters != "")
					{
						Description2 = "\"" + System.IO.Path.GetFileName(fileName) + " " + parameters + "\"";
					}
					else
					{
						Description2 = "\"" + System.IO.Path.GetFileName(fileName) + "\"";
					}
				}
				else if (ActionId == ActionIds.Sleep)
				{
					description1 = "Sleep " + Parameters[0] + " milliseconds";
				}
				else if (ActionId == ActionIds.SelectDate)
				{
					DateTime date = (DateTime)Parameters[0];
					description1 = "Select " + date.ToString("MM/dd/yyyy");
				}
				else if (ActionId == ActionIds.AddDateToSelection)
				{
					DateTime date = (DateTime)Parameters[0];
					description1 = "Add " + date.ToString("MM/dd/yyyy");
					Description2 = "to selection";
				}
				else if (ActionId == ActionIds.IsChecked)
				{
					bool isChecked = (bool)Parameters[0];
					
					if (isChecked == true)
					{
						description1 = "Check";
					}
					else
					{
						description1 = "Uncheck";
					}
				}
				else if (ActionId == ActionIds.SetText)
				{
					description1 = "Set the text";
					Description2 = "\"" + Parameters[0] + "\"";
				}
				else if (ActionId == ActionIds.SelectByIndex)
				{
					description1 = "Select the " + GetIndexString((int)Parameters[0]) + " item";
				}
				else if (ActionId == ActionIds.SelectByText)
				{
					description1 = "Select item with text";
					Description2 = "\"" + Parameters[0] + "\"";
				}
				else if (ActionId == ActionIds.Scroll)
				{
					double vertical = (double)Parameters[0];
					double horizontal = (double)Parameters[1];
					
					if (vertical == 0 && horizontal != 0)
					{
						description1 = "Scroll horizontally with " + horizontal + "%";
					}
					else if (vertical != 0 && horizontal == 0)
					{
						description1 = "Scroll vertically with " + vertical + "%";
					}
					else if (vertical != 0 && horizontal != 0)
					{
						description1 = "Scroll vertically with " + vertical + "%";
						Description2 = "and horizontally with " + horizontal + "%";
					}
					else
					{
						description1 = "Scroll with 0%";
						Description2 = "both vertically and horizontally";
					}
				}
				else if (ActionId == ActionIds.AddToSelection && 
					(Element.ControlType == ControlType.DataGrid || Element.ControlType == ControlType.List))
				{
					int index = (int)Parameters[0];
					description1 = "Add the " + GetIndexString(index) + " item";
					Description2 = "to selection";
				}
				else if (ActionId == ActionIds.RemoveFromSelection && 
					(Element.ControlType == ControlType.DataGrid || Element.ControlType == ControlType.List))
				{
					int index = (int)Parameters[0];
					description1 = "Remove the " + GetIndexString(index) + " item";
					Description2 = "from selection";
				}
				else if (ActionId == ActionIds.SelectText)
				{
					description1 = "Select text";
					Description2 = "\"" + Parameters[0] + "\"";
				}
				else if (ActionId == ActionIds.AddToSelectionByText)
				{
					description1 = "Add to selection the item";
					Description2 = "with text \"" + Parameters[0] + "\"";
				}
				else if (ActionId == ActionIds.RemoveFromSelectionByText)
				{
					description1 = "Remove from selection the item";
					Description2 = "with text \"" + Parameters[0] + "\"";
				}
				else if (ActionId == ActionIds.Value)
				{
					description1 = "Set the value to " + Parameters[0];
				}
				else if (ActionId == ActionIds.Move)
				{
					description1 = "Move to (" + Parameters[0] + ", " + Parameters[1] + ")";
				}
				else if (ActionId == ActionIds.MoveOffset)
				{
					description1 = "Move with horizontal and vertical";
					Description2 = "offsets: H " + Parameters[0] + ", V " + Parameters[1];
				}
				else if (ActionId == ActionIds.Resize)
				{
					description1 = "Set the width and height:";
					Description2 = "W " + Parameters[0] + ", H " + Parameters[1];
				}
				else if (ActionId == ActionIds.WindowWidth)
				{
					description1 = "Set the width to " + Parameters[0];
				}
				else if (ActionId == ActionIds.WindowHeight)
				{
					description1 = "Set the height to " + Parameters[0];
				}
				else
				{
					description1 = this.ActionId.ToString();
				}
				
				return description1;
			}
			set
			{
				this.description1 = value;
			}
		}
		
		private static string GetIndexString(int index)
		{
			if (index == 0 || index == 1)
			{
				return "1st";
			}
			else if (index == 2)
			{
				return "2nd";
			}
			else if (index == 3)
			{
				return "3rd";
			}
			else
			{
				return index.ToString() + "th";
			}
		}
		
		public void DeepCopy(ActionBase destination)
		{
			if (!(destination is Action))
			{
				return;
			}
			
			Action.DeepCopy(this, (Action)destination);
		}
		
		public static void DeepCopy(Action source, Action destination)
		{
			if (source.Element == null)
			{
				destination.Element = null;
			}
			else
			{
				if (destination.Element == null)
				{
					destination.Element = new Element();
				}
				Element.DeepCopy(source.Element, destination.Element);
			}
			
			destination.ActionId = source.ActionId;
			if (source.Parameters == null)
			{
				destination.Parameters = null;
			}
			else
			{
				if (destination.Parameters == null)
				{
					destination.Parameters = new List<object>();
				}
				else
				{
					destination.Parameters.Clear();
				}
				foreach (object parameter in source.Parameters)
				{
					destination.Parameters.Add(parameter);
				}
			}
			
			destination.Description1 = null;
			
			/*if (source.LoopAction == null)
			{
				destination.LoopAction = null;
				return;
			}
			
			if (source.LoopAction is LoopConditional)
			{
				LoopConditional loopConditional = new LoopConditional();
				LoopConditional.DeepCopy((LoopConditional)source.LoopAction, loopConditional);
				destination.LoopAction = loopConditional;
			}
			else if (source.LoopAction is LoopCount)
			{
				LoopCount sourceLoopCount = (LoopCount)source.LoopAction;
				LoopCount loopCount = new LoopCount(sourceLoopCount.InitialCount);
				//LoopCount.DeepCopy(sourceLoopCount, loopCount);
				destination.LoopAction = loopCount;
			}*/
		}
		
		public bool HasWeakBond
		{
			get
			{
				if (this.Next != null)
				{
					return this.Next.Previous != this;
				}
				return false;
			}
		}
	}
	
	public enum ActionIds
	{
		None,
		Click,
		RightClick,
		MiddleClick,
		DoubleClick,
		ClickAt,
		RightClickAt,
		MiddleClickAt,
		DoubleClickAt,
		MoveMouse,
		MouseScrollUp,
		MouseScrollDown,
		Invoke,
		Focus,
		BringToForeground,
		SendKeys,
		KeyDown,
		KeyPress,
		KeysPress,
		KeyUp,
		PredefinedKeysCombination,
		SimulateSendKeys,
		SimulateClick,
		SimulateRightClick,
		SimulateMiddleClick,
		SimulateDoubleClick,
		SimulateClickAt,
		SimulateRightClickAt,
		SimulateMiddleClickAt,
		SimulateDoubleClickAt,
		CaptureToFile,
		WaitForInputIdle,
		
		StartProcess,
		StartProcessAndWaitForInputIdle,
		Sleep,
		ShowDesktop,
		
		//Button
		Press,
		
		//Calendar
		SelectDate,
		SelectRange,
		AddDateToSelection,
		AddRangeToSelection,
		
		//Checkbox
		Toggle,
		IsChecked,
		
		//ComboBox
		SetText,
		Expand,
		Collapse,
		SelectByIndex,
		SelectByText,
		
		//DataGrid
		SelectAll,
		ClearAllSelection,
		Scroll,
		RemoveFromSelection,
		
		//DataItem
		Select,
		
		//Edit
		ClearText,
		SelectText,
		ClearSelection,
		
		//Hyperlink
		AccessLink,
		
		//List
		AddToSelection,
		AddToSelectionByText,
		RemoveFromSelectionByText,
		
		//ListItem
		BringIntoView,
		
		//MenuItem
		AccessMenu,
		
		//ScrollBar
		SmallIncrement,
		LargeIncrement,
		SmallDecrement,
		LargeDecrement,
		Value,
		
		//Slider
		Increment,
		Decrement,
		
		//Window
		Show,
		Minimize,
		Maximize,
		Restore,
		Close,
		Move,
		MoveOffset,
		Resize,
		WindowWidth,
		WindowHeight
	}
}