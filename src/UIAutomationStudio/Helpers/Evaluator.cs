using System;
using UIDeskAutomationLib;

namespace UIAutomationStudio
{
	public class Evaluator
	{
		public static object Evaluate(Variable variable)
		{
			ElementBase libraryElement = variable.Element.GetLibraryElement(true);
			if (libraryElement == null)
			{
				return null;
			}
			
			// general properties
			if (variable.PropertyId == PropertyId.Text)
			{
				return libraryElement.GetText();
			}
			else if (variable.PropertyId == PropertyId.Left)
			{
				return libraryElement.Left;
			}
			else if (variable.PropertyId == PropertyId.Top)
			{
				return libraryElement.Top;
			}
			else if (variable.PropertyId == PropertyId.Width)
			{
				return libraryElement.Width;
			}
			else if (variable.PropertyId == PropertyId.Height)
			{
				return libraryElement.Height;
			}
			else if (variable.PropertyId == PropertyId.IsEnabled)
			{
				return libraryElement.IsEnabled;
			}
			else if (variable.PropertyId == PropertyId.IsAlive)
			{
				return libraryElement.IsAlive;
			}
			
			ControlType controlType = variable.Element.ControlType;
			if (variable.PropertyId == PropertyId.IsPressed)
			{
				if (controlType == ControlType.Button)
				{
					UIDA_Button button = libraryElement as UIDA_Button;
					if (button != null)
					{
						return button.IsPressed;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.SelectedDate)
			{
				if (controlType == ControlType.Calendar)
				{
					UIDA_Calendar calendar = libraryElement as UIDA_Calendar;
					if (calendar != null)
					{
						DateTime[] dates = calendar.SelectedDates;
						if (dates.Length == 0)
						{
							return null;
						}
						else
						{
							return dates[0];
						}
					}
				}
				else if (controlType == ControlType.DatePicker)
				{
					UIDA_DatePicker datePicker = libraryElement as UIDA_DatePicker;
					if (datePicker != null)
					{
						return datePicker.SelectedDate;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.IsChecked)
			{
				if (controlType == ControlType.CheckBox)
				{
					UIDA_CheckBox checkBox = libraryElement as UIDA_CheckBox;
					if (checkBox != null)
					{
						return checkBox.IsChecked;
					}
				}
				else if (controlType == ControlType.TreeItem)
				{
					UIDA_TreeItem treeItem = libraryElement as UIDA_TreeItem;
					if (treeItem != null)
					{
						return treeItem.IsChecked;
					}
				}
				else if (controlType == ControlType.ListItem)
				{
					UIDA_ListItem listItem = libraryElement as UIDA_ListItem;
					if (listItem != null)
					{
						return listItem.IsChecked;
					}
				}
				else if (controlType == ControlType.MenuItem)
				{
					UIDA_MenuItem menuItem = libraryElement as UIDA_MenuItem;
					if (menuItem != null)
					{
						return menuItem.IsChecked;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.SelectedItem)
			{
				if (controlType == ControlType.ComboBox)
				{
					UIDA_ComboBox comboBox = libraryElement as UIDA_ComboBox;
					if (comboBox != null)
					{
						UIDA_ListItem listItem = comboBox.SelectedItem;
						if (listItem != null) 
						{
							return listItem.GetText();
						}
						else
						{
							Helper.MessageBoxShow("No item selected in Combo Box");
						}
					}
				}
				else if (controlType == ControlType.List)
				{
					UIDA_List list = libraryElement as UIDA_List;
					if (list != null)
					{
						UIDA_ListItem[] items = list.SelectedItems;
						if (items.Length > 0)
						{
							return items[0].GetText();
						}
						else
						{
							Helper.MessageBoxShow("No item selected in List");
						}
					}
				}
				else if (controlType == ControlType.Tab)
				{
					UIDA_TabCtrl tabCtrl = libraryElement as UIDA_TabCtrl;
					if (tabCtrl != null)
					{
						UIDA_TabItem tabItem = tabCtrl.GetSelectedTabItem();
						if (tabItem != null) 
						{
							return tabItem.GetText();
						}
						else
						{
							Helper.MessageBoxShow("No item selected in Tab Control");
						}
					}
				}
			}
			else if (variable.PropertyId == PropertyId.SelectedItemIndex)
			{
				if (controlType == ControlType.ComboBox)
				{
					UIDA_ComboBox comboBox = libraryElement as UIDA_ComboBox;
					if (comboBox != null)
					{
						UIDA_ListItem listItem = comboBox.SelectedItem;
						if (listItem != null) 
						{
							return listItem.Index;
						}
						else
						{
							Helper.MessageBoxShow("No item selected in Combo Box");
						}
					}
				}
				else if (controlType == ControlType.List)
				{
					UIDA_List list = libraryElement as UIDA_List;
					if (list != null)
					{
						UIDA_ListItem[] items = list.SelectedItems;
						if (items.Length > 0) 
						{
							return items[0].Index;
						}
						else
						{
							Helper.MessageBoxShow("No item selected in List");
						}
					}
				}
				else if (controlType == ControlType.Tab)
				{
					UIDA_TabCtrl tabCtrl = libraryElement as UIDA_TabCtrl;
					if (tabCtrl != null)
					{
						UIDA_TabItem tabItem = tabCtrl.GetSelectedTabItem();
						if (tabItem != null) 
						{
							return tabItem.Index;
						}
						else
						{
							Helper.MessageBoxShow("No item selected in Tab Control");
						}
					}
				}
			}
			else if (variable.PropertyId == PropertyId.ItemByIndex)
			{
				if (controlType == ControlType.ComboBox)
				{
					UIDA_ComboBox comboBox = libraryElement as UIDA_ComboBox;
					if (comboBox != null)
					{
						UIDA_ListItem[] items = comboBox.Items;
						int index = variable.Parameters.Count > 0 ? (int)variable.Parameters[0] : -1;
						if (index >= 0 && items.Length > index)
						{
							UIDA_ListItem listItem = items[index];
							if (listItem != null) 
							{
								return listItem.GetText();
							}
						}
						else
						{
							Helper.MessageBoxShow("Combo Box item index is outside the bounds");
						}
					}
				}
				else if (controlType == ControlType.List)
				{
					UIDA_List list = libraryElement as UIDA_List;
					if (list != null)
					{
						UIDA_ListItem[] items = list.Items;
						int index = variable.Parameters.Count > 0 ? (int)variable.Parameters[0] : -1;
						if (index >= 0 && items.Length > index)
						{
							UIDA_ListItem listItem = items[index];
							if (listItem != null) 
							{
								return listItem.GetText();
							}
						}
						else
						{
							Helper.MessageBoxShow("List item index is outside the bounds");
						}
					}
				}
				else if (controlType == ControlType.Tab)
				{
					UIDA_TabCtrl tabCtrl = libraryElement as UIDA_TabCtrl;
					if (tabCtrl != null)
					{
						UIDA_TabItem[] items = tabCtrl.Items;
						int index = variable.Parameters.Count > 0 ? (int)variable.Parameters[0] : -1;
						if (index >= 0 && items.Length > index)
						{
							UIDA_TabItem tabItem = items[index];
							if (tabItem != null) 
							{
								return tabItem.GetText();
							}
						}
						else
						{
							Helper.MessageBoxShow("Tab Item index is outside the bounds");
						}
					}
				}
			}
			else if (variable.PropertyId == PropertyId.ItemsCount)
			{
				if (controlType == ControlType.ComboBox)
				{
					UIDA_ComboBox comboBox = libraryElement as UIDA_ComboBox;
					if (comboBox != null)
					{
						return comboBox.Items.Length;
					}
				}
				else if (controlType == ControlType.List)
				{
					UIDA_List list = libraryElement as UIDA_List;
					if (list != null)
					{
						return list.Items.Length;
					}
				}
				else if (controlType == ControlType.Tab)
				{
					UIDA_TabCtrl tabCtrl = libraryElement as UIDA_TabCtrl;
					if (tabCtrl != null)
					{
						return tabCtrl.Items.Length;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.ColumnCount)
			{
				if (controlType == ControlType.DataGrid)
				{
					UIDA_DataGrid dataGrid = libraryElement as UIDA_DataGrid;
					if (dataGrid != null)
					{
						return dataGrid.ColumnCount;
					}
				}
				else if (controlType == ControlType.Table)
				{
					UIDA_Table table = libraryElement as UIDA_Table;
					if (table != null)
					{
						return table.ColumnsCount;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.RowCount)
			{
				if (controlType == ControlType.DataGrid)
				{
					UIDA_DataGrid dataGrid = libraryElement as UIDA_DataGrid;
					if (dataGrid != null)
					{
						return dataGrid.RowCount;
					}
				}
				else if (controlType == ControlType.Table)
				{
					UIDA_Table table = libraryElement as UIDA_Table;
					if (table != null)
					{
						return table.Rows.Length;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.CanSelectMultiple)
			{
				if (controlType == ControlType.DataGrid)
				{
					UIDA_DataGrid dataGrid = libraryElement as UIDA_DataGrid;
					if (dataGrid != null)
					{
						return dataGrid.CanSelectMultiple;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.ValueByRowAndColumn)
			{
				if (controlType == ControlType.DataGrid)
				{
					UIDA_DataGrid dataGrid = libraryElement as UIDA_DataGrid;
					if (dataGrid != null)
					{
						if (variable.Parameters.Count >= 2)
						{
							int row = (int)variable.Parameters[0];
							int column = (int)variable.Parameters[1];
							return dataGrid.Rows[row][column];
						}
					}
				}
				else if (controlType == ControlType.Table)
				{
					UIDA_Table table = libraryElement as UIDA_Table;
					if (table != null)
					{
						if (variable.Parameters.Count >= 2)
						{
							int row = (int)variable.Parameters[0];
							int column = (int)variable.Parameters[1];
							return table.Rows[row].Cells[column].Value;
						}
					}
				}
			}
			else if (variable.PropertyId == PropertyId.SelectedRowsCount)
			{
				if (controlType == ControlType.DataGrid)
				{
					UIDA_DataGrid dataGrid = libraryElement as UIDA_DataGrid;
					if (dataGrid != null)
					{
						return dataGrid.SelectedRows.Length;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.SelectedValueByColumn)
			{
				if (controlType == ControlType.DataGrid)
				{
					UIDA_DataGrid dataGrid = libraryElement as UIDA_DataGrid;
					if (dataGrid != null)
					{
						UIDA_DataItem[] selectedRows = dataGrid.SelectedRows;
						if (selectedRows.Length > 0 && variable.Parameters.Count > 0)
						{
							int column = (int)variable.Parameters[0];
							if (dataGrid.ColumnCount > column)
							{
								return dataGrid.SelectedRows[0][column];
							}
							else
							{
								Helper.MessageBoxShow("Data Grid column index is too big");
							}
						}
					}
				}
			}
			else if (variable.PropertyId == PropertyId.IsSelected)
			{
				if (controlType == ControlType.DataItem)
				{
					UIDA_DataItem dataItem = libraryElement as UIDA_DataItem;
					if (dataItem != null)
					{
						return dataItem.IsSelected;
					}
				}
				else if (controlType == ControlType.TabItem)
				{
					UIDA_TabItem tabItem = libraryElement as UIDA_TabItem;
					if (tabItem != null)
					{
						return tabItem.IsSelected;
					}
				}
				else if (controlType == ControlType.ListItem)
				{
					UIDA_ListItem listItem = libraryElement as UIDA_ListItem;
					if (listItem != null)
					{
						return listItem.IsSelected;
					}
				}
				else if (controlType == ControlType.RadioButton)
				{
					UIDA_RadioButton radioButton = libraryElement as UIDA_RadioButton;
					if (radioButton != null)
					{
						return radioButton.IsSelected;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.ValueByColumnIndex)
			{
				if (controlType == ControlType.DataItem)
				{
					UIDA_DataItem dataItem = libraryElement as UIDA_DataItem;
					if (dataItem != null)
					{
						if (variable.Parameters.Count > 0)
						{
							int columnIndex = (int)variable.Parameters[0];
							return dataItem[columnIndex];
						}
					}
				}
			}
			else if (variable.PropertyId == PropertyId.ValueByColumnName)
			{
				if (controlType == ControlType.DataItem)
				{
					UIDA_DataItem dataItem = libraryElement as UIDA_DataItem;
					if (dataItem != null)
					{
						if (variable.Parameters.Count > 0)
						{
							string columnName = (string)variable.Parameters[0];
							return dataItem[columnName];
						}
					}
				}
			}
			else if (variable.PropertyId == PropertyId.SelectedItemsCount)
			{
				if (controlType == ControlType.List)
				{
					UIDA_List list = libraryElement as UIDA_List;
					if (list != null)
					{
						return list.SelectedItems.Length;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.SelectedItemByIndex)
			{
				if (controlType == ControlType.List)
				{
					UIDA_List list = libraryElement as UIDA_List;
					if (list != null && variable.Parameters.Count > 0)
					{
						int index = (int)variable.Parameters[0];
						UIDA_ListItem[] selectedItems = list.SelectedItems;
						if (selectedItems.Length > index)
						{
							return list.SelectedItems[index].GetText();
						}
						else
						{
							Helper.MessageBoxShow("List selected item index is too big");
						}
					}
				}
			}
			else if (variable.PropertyId == PropertyId.Index)
			{
				if (controlType == ControlType.ListItem)
				{
					UIDA_ListItem listItem = libraryElement as UIDA_ListItem;
					if (listItem != null)
					{
						return listItem.Index;
					}
				}
				else if (controlType == ControlType.TabItem)
				{
					UIDA_TabItem tabItem = libraryElement as UIDA_TabItem;
					if (tabItem != null)
					{
						return tabItem.Index;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.Value)
			{
				if (controlType == ControlType.ProgressBar)
				{
					UIDA_ProgressBar progressBar = libraryElement as UIDA_ProgressBar;
					if (progressBar != null)
					{
						return progressBar.Value;
					}
				}
				else if (controlType == ControlType.ScrollBar)
				{
					UIDA_ScrollBar scrollBar = libraryElement as UIDA_ScrollBar;
					if (scrollBar != null)
					{
						return scrollBar.Value;
					}
				}
				else if (controlType == ControlType.Slider)
				{
					UIDA_Slider slider = libraryElement as UIDA_Slider;
					if (slider != null)
					{
						return slider.Value;
					}
				}
				else if (controlType == ControlType.Spinner)
				{
					UIDA_Spinner spinner = libraryElement as UIDA_Spinner;
					if (spinner != null)
					{
						return spinner.Value;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.Minimum)
			{
				if (controlType == ControlType.ProgressBar)
				{
					UIDA_ProgressBar progressBar = libraryElement as UIDA_ProgressBar;
					if (progressBar != null)
					{
						return progressBar.GetMinimum();
					}
				}
				else if (controlType == ControlType.ScrollBar)
				{
					UIDA_ScrollBar scrollBar = libraryElement as UIDA_ScrollBar;
					if (scrollBar != null)
					{
						return scrollBar.GetMinimum();
					}
				}
				else if (controlType == ControlType.Slider)
				{
					UIDA_Slider slider = libraryElement as UIDA_Slider;
					if (slider != null)
					{
						return slider.GetMinimum();
					}
				}
				else if (controlType == ControlType.Spinner)
				{
					UIDA_Spinner spinner = libraryElement as UIDA_Spinner;
					if (spinner != null)
					{
						return spinner.GetMinimum();
					}
				}
			}
			else if (variable.PropertyId == PropertyId.Maximum)
			{
				if (controlType == ControlType.ProgressBar)
				{
					UIDA_ProgressBar progressBar = libraryElement as UIDA_ProgressBar;
					if (progressBar != null)
					{
						return progressBar.GetMaximum();
					}
				}
				else if (controlType == ControlType.ScrollBar)
				{
					UIDA_ScrollBar scrollBar = libraryElement as UIDA_ScrollBar;
					if (scrollBar != null)
					{
						return scrollBar.GetMaximum();
					}
				}
				else if (controlType == ControlType.Slider)
				{
					UIDA_Slider slider = libraryElement as UIDA_Slider;
					if (slider != null)
					{
						return slider.GetMaximum();
					}
				}
				else if (controlType == ControlType.Spinner)
				{
					UIDA_Spinner spinner = libraryElement as UIDA_Spinner;
					if (spinner != null)
					{
						return spinner.GetMaximum();
					}
				}
			}
			else if (variable.PropertyId == PropertyId.Root)
			{
				if (controlType == ControlType.Tree)
				{
					UIDA_Tree tree = libraryElement as UIDA_Tree;
					if (tree != null)
					{
						return tree.GetRoot().GetText();
					}
				}
			}
			else if (variable.PropertyId == PropertyId.SubItemsCount)
			{
				if (controlType == ControlType.TreeItem)
				{
					UIDA_TreeItem treeItem = libraryElement as UIDA_TreeItem;
					if (treeItem != null)
					{
						return treeItem.SubItems.Length;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.SubItemByIndex)
			{
				if (controlType == ControlType.TreeItem)
				{
					UIDA_TreeItem treeItem = libraryElement as UIDA_TreeItem;
					if (treeItem != null && variable.Parameters.Count > 0)
					{
						int index = (int)variable.Parameters[0];
						UIDA_TreeItem[] subItems = treeItem.SubItems;
						if (subItems.Length > index)
						{
							return subItems[index].GetText();
						}
						else
						{
							Helper.MessageBoxShow("Tree Item index is too big");
						}
					}
				}
			}
			else if (variable.PropertyId == PropertyId.IsExpanded)
			{
				if (controlType == ControlType.TreeItem)
				{
					UIDA_TreeItem treeItem = libraryElement as UIDA_TreeItem;
					if (treeItem != null)
					{
						return treeItem.ExpandCollapseState == UIAutomationClient.ExpandCollapseState.ExpandCollapseState_Expanded;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.IsCollapsed)
			{
				if (controlType == ControlType.TreeItem)
				{
					UIDA_TreeItem treeItem = libraryElement as UIDA_TreeItem;
					if (treeItem != null)
					{
						return treeItem.ExpandCollapseState != UIAutomationClient.ExpandCollapseState.ExpandCollapseState_Expanded;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.IsMinimized)
			{
				if (controlType == ControlType.Window)
				{
					UIDA_Window window = libraryElement as UIDA_Window;
					if (window != null)
					{
						return window.IsMinimized;
					}
				}
			}
			else if (variable.PropertyId == PropertyId.IsMaximized)
			{
				if (controlType == ControlType.Window)
				{
					UIDA_Window window = libraryElement as UIDA_Window;
					if (window != null)
					{
						return window.IsMaximized;
					}
				}
			}
			
			return null;
		}
	}
}