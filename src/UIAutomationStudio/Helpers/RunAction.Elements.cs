using System;
using UIDeskAutomationLib;

namespace UIAutomationStudio
{
	public partial class RunAction
	{
		public interface IElement
		{
			bool Run(Action action);
		}
		
		public class Button: IElement
		{
			private ElementBase libraryElement;
		
			public Button(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				if (action.ActionId == ActionIds.Press)
				{
					UIDA_Button button = libraryElement as UIDA_Button;
					if (button != null)
					{
						button.Press();
						return true;
					}
				}
				return false;
			}
		}
		
		public class Calendar: IElement
		{
			private ElementBase libraryElement;
		
			public Calendar(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_Calendar calendar = libraryElement as UIDA_Calendar;
				if (calendar != null && action.Parameters.Count > 0)
				{
					if (action.ActionId == ActionIds.SelectDate)
					{
						calendar.SelectDate((DateTime)action.Parameters[0]);
						return true;
					}
					else if (action.ActionId == ActionIds.AddDateToSelection)
					{
						calendar.AddToSelection((DateTime)action.Parameters[0]);
						return true;
					}
				}
				return false;
			}
		}
		
		public class CheckBox: IElement
		{
			private ElementBase libraryElement;
		
			public CheckBox(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_CheckBox checkBox = libraryElement as UIDA_CheckBox;
				if (checkBox != null)
				{
					if (action.ActionId == ActionIds.Toggle)
					{
						checkBox.Toggle();
						return true;
					}
					if (action.ActionId == ActionIds.IsChecked && action.Parameters.Count > 0)
					{
						checkBox.IsChecked = (bool)action.Parameters[0];
						return true;
					}
				}
				return false;
			}
		}
		
		public class ComboBox: IElement
		{
			private ElementBase libraryElement;
		
			public ComboBox(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_ComboBox comboBox = libraryElement as UIDA_ComboBox;
				if (comboBox != null)
				{
					if (action.ActionId == ActionIds.SetText && action.Parameters.Count > 0)
					{
						comboBox.SetText((string)action.Parameters[0]);
						return true;
					}
					if (action.ActionId == ActionIds.Expand)
					{
						comboBox.Expand();
						return true;
					}
					if (action.ActionId == ActionIds.Collapse)
					{
						comboBox.Collapse();
						return true;
					}
					if (action.ActionId == ActionIds.SelectByIndex && action.Parameters.Count > 0)
					{
						comboBox.Select((int)action.Parameters[0]);
						return true;
					}
					if (action.ActionId == ActionIds.SelectByText && action.Parameters.Count > 0)
					{
						comboBox.Select((string)action.Parameters[0]);
						return true;
					}
				}
				return false;
			}
		}
		
		public class DataGrid: IElement
		{
			private ElementBase libraryElement;
		
			public DataGrid(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_DataGrid dataGrid = libraryElement as UIDA_DataGrid;
				if (dataGrid != null)
				{
					if (action.ActionId == ActionIds.SelectAll)
					{
						dataGrid.SelectAll();
						return true;
					}
					if (action.ActionId == ActionIds.ClearAllSelection)
					{
						dataGrid.ClearAllSelection();
						return true;
					}
					if (action.ActionId == ActionIds.Scroll && action.Parameters.Count >= 2)
					{
						dataGrid.Scroll((double)action.Parameters[0], (double)action.Parameters[1]);
						return true;
					}
					if (action.ActionId == ActionIds.SelectByIndex && action.Parameters.Count > 0)
					{
						dataGrid.Select((int)action.Parameters[0]);
						return true;
					}
					if (action.ActionId == ActionIds.AddToSelection && action.Parameters.Count > 0)
					{
						dataGrid.AddToSelection((int)action.Parameters[0]);
						return true;
					}
					if (action.ActionId == ActionIds.RemoveFromSelection && action.Parameters.Count > 0)
					{
						dataGrid.RemoveFromSelection((int)action.Parameters[0]);
						return true;
					}
				}
				return false;
			}
		}
		
		public class Group: IElement
		{
			private ElementBase libraryElement;
		
			public Group(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_Group dataGridGroup = libraryElement as UIDA_Group;
				if (dataGridGroup != null)
				{
					if (action.ActionId == ActionIds.Expand)
					{
						dataGridGroup.Expand();
						return true;
					}
					if (action.ActionId == ActionIds.Collapse)
					{
						dataGridGroup.Collapse();
						return true;
					}
				}
				return false;
			}
		}
		
		public class DataItem: IElement
		{
			private ElementBase libraryElement;
		
			public DataItem(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_DataItem dataItem = libraryElement as UIDA_DataItem;
				if (dataItem != null)
				{
					if (action.ActionId == ActionIds.Select)
					{
						dataItem.Select();
						return true;
					}
					if (action.ActionId == ActionIds.AddToSelection)
					{
						dataItem.AddToSelection();
						return true;
					}
					if (action.ActionId == ActionIds.RemoveFromSelection)
					{
						dataItem.RemoveFromSelection();
						return true;
					}
				}
				return false;
			}
		}
		
		public class DatePicker: IElement
		{
			private ElementBase libraryElement;
		
			public DatePicker(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_DatePicker datePicker = libraryElement as UIDA_DatePicker;
				if (datePicker != null)
				{
					if (action.ActionId == ActionIds.SelectDate && action.Parameters.Count > 0)
					{
						datePicker.SelectedDate = (DateTime)action.Parameters[0];
						return true;
					}
				}
				return false;
			}
		}
		
		public class Document: IElement
		{
			private ElementBase libraryElement;
		
			public Document(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_Document document = libraryElement as UIDA_Document;
				if (document != null)
				{
					if (action.ActionId == ActionIds.SetText && action.Parameters.Count > 0)
					{
						document.SetText((string)action.Parameters[0]);
						return true;
					}
					if (action.ActionId == ActionIds.ClearText)
					{
						document.ClearText();
						return true;
					}
					if (action.ActionId == ActionIds.SelectText && action.Parameters.Count > 0)
					{
						document.SelectText((string)action.Parameters[0]);
						return true;
					}
					if (action.ActionId == ActionIds.SelectAll)
					{
						document.SelectAll();
						return true;
					}
					if (action.ActionId == ActionIds.ClearSelection)
					{
						document.ClearSelection();
						return true;
					}
				}
				return false;
			}
		}
		
		public class Edit: IElement
		{
			private ElementBase libraryElement;
		
			public Edit(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_Edit edit = libraryElement as UIDA_Edit;
				if (edit != null)
				{
					if (action.ActionId == ActionIds.SetText && action.Parameters.Count > 0)
					{
						edit.SetText((string)action.Parameters[0]);
						return true;
					}
					if (action.ActionId == ActionIds.ClearText)
					{
						edit.ClearText();
						return true;
					}
					if (action.ActionId == ActionIds.SelectText && action.Parameters.Count > 0)
					{
						edit.SelectText((string)action.Parameters[0]);
						return true;
					}
					if (action.ActionId == ActionIds.SelectAll)
					{
						edit.SelectAll();
						return true;
					}
					if (action.ActionId == ActionIds.ClearSelection)
					{
						edit.ClearSelection();
						return true;
					}
				}
				return false;
			}
		}
		
		public class Hyperlink: IElement
		{
			private ElementBase libraryElement;
		
			public Hyperlink(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_HyperLink hyperlink = libraryElement as UIDA_HyperLink;
				if (hyperlink != null)
				{
					if (action.ActionId == ActionIds.AccessLink)
					{
						hyperlink.AccessLink();
						return true;
					}
				}
				return false;
			}
		}
		
		public class List: IElement
		{
			private ElementBase libraryElement;
		
			public List(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_List list = libraryElement as UIDA_List;
				if (list != null)
				{
					if (action.ActionId == ActionIds.SelectByIndex && action.Parameters.Count > 0)
					{
						list.Select((int)action.Parameters[0]);
						return true;
					}
					if (action.ActionId == ActionIds.SelectByText && action.Parameters.Count > 0)
					{
						list.Select((string)action.Parameters[0]);
						return true;
					}
					if (action.ActionId == ActionIds.AddToSelection && action.Parameters.Count > 0)
					{
						list.AddToSelection((int)action.Parameters[0]);
						return true;
					}
					if (action.ActionId == ActionIds.AddToSelectionByText && action.Parameters.Count > 0)
					{
						list.AddToSelection((string)action.Parameters[0]);
						return true;
					}
					if (action.ActionId == ActionIds.RemoveFromSelection && action.Parameters.Count > 0)
					{
						list.RemoveFromSelection((int)action.Parameters[0]);
						return true;
					}
					if (action.ActionId == ActionIds.RemoveFromSelectionByText && action.Parameters.Count > 0)
					{
						list.RemoveFromSelection((string)action.Parameters[0]);
						return true;
					}
					if (action.ActionId == ActionIds.SelectAll)
					{
						list.SelectAll();
						return true;
					}
					if (action.ActionId == ActionIds.ClearAllSelection)
					{
						list.ClearAllSelection();
						return true;
					}
				}
				return false;
			}
		}
		
		public class ListItem: IElement
		{
			private ElementBase libraryElement;
		
			public ListItem(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_ListItem listItem = libraryElement as UIDA_ListItem;
				if (listItem != null)
				{
					if (action.ActionId == ActionIds.Select)
					{
						listItem.Select();
						return true;
					}
					if (action.ActionId == ActionIds.RemoveFromSelection)
					{
						listItem.RemoveFromSelection();
						return true;
					}
					if (action.ActionId == ActionIds.AddToSelection)
					{
						listItem.AddToSelection();
						return true;
					}
					if (action.ActionId == ActionIds.BringIntoView)
					{
						listItem.BringIntoView();
						return true;
					}
					if (action.ActionId == ActionIds.IsChecked && action.Parameters.Count > 0)
					{
						listItem.IsChecked = (bool)action.Parameters[0];
						return true;
					}
				}
				return false;
			}
		}
		
		public class MenuItem: IElement
		{
			private ElementBase libraryElement;
		
			public MenuItem(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_MenuItem menuItem = libraryElement as UIDA_MenuItem;
				if (menuItem != null)
				{
					if (action.ActionId == ActionIds.AccessMenu)
					{
						menuItem.AccessMenu();
						return true;
					}
					if (action.ActionId == ActionIds.Expand)
					{
						menuItem.Expand();
						return true;
					}
					if (action.ActionId == ActionIds.Collapse)
					{
						menuItem.Collapse();
						return true;
					}
					if (action.ActionId == ActionIds.Toggle)
					{
						menuItem.Toggle();
						return true;
					}
					if (action.ActionId == ActionIds.IsChecked && action.Parameters.Count > 0)
					{
						menuItem.IsChecked = (bool)action.Parameters[0];
						return true;
					}
				}
				return false;
			}
		}
		
		public class RadioButton: IElement
		{
			private ElementBase libraryElement;
		
			public RadioButton(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_RadioButton radioButton = libraryElement as UIDA_RadioButton;
				if (radioButton != null)
				{
					if (action.ActionId == ActionIds.Select)
					{
						radioButton.Select();
						return true;
					}
				}
				return false;
			}
		}
		
		public class ScrollBar: IElement
		{
			private ElementBase libraryElement;
		
			public ScrollBar(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_ScrollBar scrollBar = libraryElement as UIDA_ScrollBar;
				if (scrollBar != null)
				{
					if (action.ActionId == ActionIds.SmallIncrement)
					{
						scrollBar.SmallIncrement();
						return true;
					}
					if (action.ActionId == ActionIds.LargeIncrement)
					{
						scrollBar.LargeIncrement();
						return true;
					}
					if (action.ActionId == ActionIds.SmallDecrement)
					{
						scrollBar.SmallDecrement();
						return true;
					}
					if (action.ActionId == ActionIds.LargeDecrement)
					{
						scrollBar.LargeDecrement();
						return true;
					}
					if (action.ActionId == ActionIds.Value && action.Parameters.Count > 0)
					{
						scrollBar.Value = (double)action.Parameters[0];
						return true;
					}
				}
				return false;
			}
		}
		
		public class Slider: IElement
		{
			private ElementBase libraryElement;
		
			public Slider(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_Slider slider = libraryElement as UIDA_Slider;
				if (slider != null)
				{
					if (action.ActionId == ActionIds.SmallIncrement)
					{
						slider.SmallIncrement();
						return true;
					}
					if (action.ActionId == ActionIds.LargeIncrement)
					{
						slider.LargeIncrement();
						return true;
					}
					if (action.ActionId == ActionIds.SmallDecrement)
					{
						slider.SmallDecrement();
						return true;
					}
					if (action.ActionId == ActionIds.LargeDecrement)
					{
						slider.LargeDecrement();
						return true;
					}
					if (action.ActionId == ActionIds.Value && action.Parameters.Count > 0)
					{
						slider.Value = (double)action.Parameters[0];
						return true;
					}
				}
				return false;
			}
		}
		
		public class Spinner: IElement
		{
			private ElementBase libraryElement;
		
			public Spinner(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_Spinner spinner = libraryElement as UIDA_Spinner;
				if (spinner != null)
				{
					if (action.ActionId == ActionIds.Increment)
					{
						spinner.Increment();
						return true;
					}
					if (action.ActionId == ActionIds.Decrement)
					{
						spinner.Decrement();
						return true;
					}
					if (action.ActionId == ActionIds.Value && action.Parameters.Count > 0)
					{
						spinner.Value = (double)action.Parameters[0];
						return true;
					}
				}
				return false;
			}
		}
		
		public class SplitButton: IElement
		{
			private ElementBase libraryElement;
		
			public SplitButton(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_SplitButton splitButton = libraryElement as UIDA_SplitButton;
				if (splitButton != null)
				{
					if (action.ActionId == ActionIds.Press)
					{
						splitButton.Press();
						return true;
					}
				}
				return false;
			}
		}
		
		public class Tab: IElement
		{
			private ElementBase libraryElement;
		
			public Tab(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_TabCtrl tabCtrl = libraryElement as UIDA_TabCtrl;
				if (tabCtrl != null)
				{
					if (action.ActionId == ActionIds.SelectByIndex && action.Parameters.Count > 0)
					{
						tabCtrl.Select((int)action.Parameters[0]);
						return true;
					}
					if (action.ActionId == ActionIds.SelectByText && action.Parameters.Count > 0)
					{
						tabCtrl.Select((string)action.Parameters[0]);
						return true;
					}
				}
				return false;
			}
		}
		
		public class TabItem: IElement
		{
			private ElementBase libraryElement;
		
			public TabItem(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_TabItem tabItem = libraryElement as UIDA_TabItem;
				if (tabItem != null)
				{
					if (action.ActionId == ActionIds.Select)
					{
						tabItem.Select();
						return true;
					}
				}
				return false;
			}
		}
		
		public class TreeItem: IElement
		{
			private ElementBase libraryElement;
		
			public TreeItem(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_TreeItem treeItem = libraryElement as UIDA_TreeItem;
				if (treeItem != null)
				{
					if (action.ActionId == ActionIds.Select)
					{
						treeItem.Select();
						return true;
					}
					if (action.ActionId == ActionIds.Expand)
					{
						treeItem.Expand();
						return true;
					}
					if (action.ActionId == ActionIds.Collapse)
					{
						treeItem.Collapse();
						return true;
					}
					if (action.ActionId == ActionIds.Toggle)
					{
						treeItem.Toggle();
						return true;
					}
					if (action.ActionId == ActionIds.BringIntoView)
					{
						treeItem.BringIntoView();
						return true;
					}
					if (action.ActionId == ActionIds.IsChecked && action.Parameters.Count > 0)
					{
						treeItem.IsChecked = (bool)action.Parameters[0];
						return true;
					}
				}
				return false;
			}
		}
		
		public class Custom: IElement
		{
			private ElementBase libraryElement;
		
			public Custom(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_Table.Cell cell = libraryElement as UIDA_Table.Cell;
				if (cell != null)
				{
					if (action.ActionId == ActionIds.Value && action.Parameters.Count > 0)
					{
						cell.Value = (string)action.Parameters[0];
						return true;
					}
				}
				return false;
			}
		}
		
		public class Window: IElement
		{
			private ElementBase libraryElement;
		
			public Window(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public bool Run(Action action)
			{
				UIDA_Window window = libraryElement as UIDA_Window;
				if (window != null)
				{
					if (action.ActionId == ActionIds.Show)
					{
						window.Show();
						return true;
					}
					if (action.ActionId == ActionIds.Minimize)
					{
						window.Minimize();
						return true;
					}
					if (action.ActionId == ActionIds.Maximize)
					{
						window.Maximize();
						return true;
					}
					if (action.ActionId == ActionIds.Restore)
					{
						window.Restore();
						return true;
					}
					if (action.ActionId == ActionIds.Close)
					{
						window.Close();
						return true;
					}
					if (action.ActionId == ActionIds.BringToForeground)
					{
						window.BringToForeground();
						return true;
					}
					if (action.ActionId == ActionIds.Move && action.Parameters.Count >= 2)
					{
						window.Move((int)action.Parameters[0], (int)action.Parameters[1]);
						return true;
					}
					if (action.ActionId == ActionIds.MoveOffset && action.Parameters.Count >= 2)
					{
						window.MoveOffset((int)action.Parameters[0], (int)action.Parameters[1]);
						return true;
					}
					if (action.ActionId == ActionIds.Resize && action.Parameters.Count >= 2)
					{
						window.Resize((int)action.Parameters[0], (int)action.Parameters[1]);
						return true;
					}
					if (action.ActionId == ActionIds.WindowWidth && action.Parameters.Count >= 1)
					{
						window.WindowWidth = (int)action.Parameters[0];
						return true;
					}
					if (action.ActionId == ActionIds.WindowHeight && action.Parameters.Count >= 1)
					{
						window.WindowHeight = (int)action.Parameters[0];
						return true;
					}
				}
				return false;
			}
		}
	}
}