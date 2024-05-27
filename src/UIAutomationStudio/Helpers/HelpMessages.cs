using System;
using System.Windows;
using System.Collections.Generic;
using System.Xml;

namespace UIAutomationStudio
{
	public class HelpMessages
	{
		public static Dictionary<MessageId, Message> Messages = null;
		
		static HelpMessages()
		{
			string enter = Environment.NewLine + Environment.NewLine;
			Messages = new Dictionary<MessageId, Message>()
			{
				{ MessageId.NewTask, new Message { Text = "You can start by adding a New Action." } },
				{ MessageId.AddAction, new Message { Text = "Press 'Select Element' button. Go with the mouse pointer over the desired UI element (don't click on it) and hold Shift pressed for a second or two." + 
					enter + "You can optionally press the 'Highlight' button to see the selected element." + 
					enter + "You can use 'Select a Parent' button to move up in the element hierarchy. For example, if you choose a list item and you actually want to select the whole list then you can use 'Select a Parent' to pick the list." +
					enter + "If you want to make a general action like starting an application or make a pause in the task flow you can check the 'Don't choose a specific UI Element, make a general action' option. A general action doesn't depend on a UI element." } },
				{ MessageId.EditAction, new Message { Text = "Press 'Select Element' button. Go with the mouse pointer over the desired UI element (don't click on it) and hold Shift pressed for a second or two. You can optionally press the 'Highlight' button to see the selected element." + 
					enter + "You can use 'Select a Parent' button to move up in the element hierarchy. For example, if you choose a list item and you actually want to select the whole list then you can use 'Select a Parent' to pick the list." +
					enter + "If you want to make a general action like starting an application or make a pause in the task flow you can check the 'Don't choose a specific UI Element, make a general action' option. A general action doesn't depend on a UI element." +
					enter + "You can edit an action (including conditional and loop actions) by selecting the action and using 'Edit Action' button or by double-clicking the action." } },
				{ MessageId.SelectAction, new Message { Text = "The actions in the 'Specific Actions' tab are specific to the type of the selected UI element." +
					enter + "The actions in the other tabs (Mouse Actions, Keyboard Actions, etc) are the same for any type of element." } },
				{ MessageId.CopyAction, new Message { Text = "You can paste (Ctrl+V) the copied action as the first (or the last) action in the task flow." +
					enter + "If you first select an arrow and then paste, the copied action will be inserted between the two ends of the arrow." +
					enter + "The same logic applies to Copy (Ctrl+C) and Cut (Ctrl+X) operations." + 
					enter + "You can move an action by drag-and-drop it over an arrow. If you drag an action and drop it over the first action of the task then the dragged action will become the first action." } },
				{ MessageId.AddConditionalAction, new Message { Text = "Here you can create one or combine more Conditions in order to create a Conditional Action." } },
				{ MessageId.AddCondition, new Message { Text = "First you need to select an UI element and in the next steps you need to select a property for this UI element and finally create a condition for the value of the selected property." } },
				{ MessageId.SaveTask, new Message { Text = "You can go to the saved task in Windows Explorer using the Tasks/Open Task Location menu." +
					enter + "You should keep both .cmd and .xml files in the same folder." +
					enter + "You can run the task from outside the studio using the .cmd file." } },
				{ MessageId.Properties, new Message { Text = "If the text of either the top level window or the inner selected element is changing during execution, or from one execution to another, but it also has a fixed part then you can use wildcards (* and ?) so the task can find your UI element. " +
					enter + "For example you can replace \"Untitled - Notepad\" text of the top level window with \"* - Notepad\" when you automate Notepad application." } },
				{ MessageId.AddLoop, new Message { Text = "A Loop Action executes repeatedly a part of the task while a condition is true or for a specified number of times." +
					enter + "First, you need to specify the first action and the last action of the sequence you want to repeat. The last action must be a descendent of the first action, i.e. there should be a path between the first and the last action of the sequence." } },
				{ MessageId.AppOpened, new Message { Text = "You can open the sample tasks " + 
					"(located in the Samples folder inside the application folder) via menu \"Tasks/Open Task\" and select a .cmd file to open. " + 
					enter + "Run the task using the \"Run All\" button." } },
				{ MessageId.NewActionWithShift, new Message { Text = "The new Action will be, by default, appended " + 
					"at the end of the task." + enter + "If you want to insert it as first press \"New Action\" button (or menu) while holding Shift pressed." } }
			};
		}
		
		// returns true if it displayed the message, false otherwise
		public static bool Show(MessageId messageId)
		{
			if (Messages.ContainsKey(messageId))
			{
				Message message = Messages[messageId];
				if (message.Show == true)
				{
					HelpMessageWindow wnd = new HelpMessageWindow(message.Text);
					wnd.Owner = MainWindow.Instance;
					if (wnd.ShowDialog() == true)
					{
						message.Show = !wnd.ShowAgain;
					}
					return true;
				}
			}
			return false;
		}
		
		public static void SaveHiddenMessages(XmlNode nodeHideMessages, XmlDocument doc)
		{
			foreach (MessageId messageId in Messages.Keys)
			{
				Message message = Messages[messageId];
				XmlNode node = doc.CreateNode(XmlNodeType.Element, messageId.ToString(), null);
				
				node.InnerText = message.Show.ToString();
				nodeHideMessages.AppendChild(node);
			}
		}
		
		public static void LoadHiddenMessages(XmlNode nodeHideMessages)
		{
			foreach (XmlNode node in nodeHideMessages.ChildNodes)
			{
				MessageId messageId = (MessageId)Enum.Parse(typeof(MessageId), node.Name);
				if (Messages.ContainsKey(messageId))
				{
					bool show = true;
					
					bool.TryParse(node.InnerText, out show);
					Messages[messageId].Show = show;
				}
			}
		}
	}
	
	public enum MessageId
	{
		None,
		NewTask,
		AddAction,
		EditAction,
		SelectAction,
		CopyAction,
		AddConditionalAction,
		ConditionsScreen,
		AddCondition,
		SaveTask,
		Properties,
		AddLoop,
		AppOpened,
		NewActionWithShift
	}
	
	public class Message
	{
		public string Text { get; set; }
		public bool Show { get; set; }
		
		public Message()
		{
			Text = null;
			Show = true;
		}
	}
}