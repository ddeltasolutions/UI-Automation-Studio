using System;
using UIDeskAutomationLib;

namespace UIAutomationStudio
{
	public class Evaluator
	{
		public static object Evaluate(Variable variable)
		{
			ElementBase libraryElement = null;
		
			if (variable.PropertyId == PropertyId.IsFound)
			{
				libraryElement = variable.Element.GetLibraryElement(noTimeOut: true, supressMsg: true);
				return (libraryElement != null);
			}
		
			libraryElement = variable.Element.GetLibraryElement(noTimeOut: true);
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
			/*else if (variable.PropertyId == PropertyId.IsAlive)
			{
				return libraryElement.IsAlive;
			}*/
			
			IEvaluator evaluator = EvaluatorFactory(variable.PropertyId, libraryElement);
			if (evaluator != null)
			{
				return evaluator.Evaluate(variable);
			}
			
			return null;
		}
		
		private static IEvaluator EvaluatorFactory(PropertyId propertyId, ElementBase libraryElement)
		{
			if (propertyId == PropertyId.IsPressed)
			{
				return new IsPressedEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.SelectedDate)
			{
				return new SelectedDateEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.IsChecked)
			{
				return new IsCheckedEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.SelectedItem)
			{
				return new SelectedItemEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.SelectedItemIndex)
			{
				return new SelectedItemIndexEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.ItemByIndex)
			{
				return new ItemByIndexEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.ItemsCount)
			{
				return new ItemsCountEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.ColumnCount)
			{
				return new ColumnCountEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.RowCount)
			{
				return new RowCountEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.CanSelectMultiple)
			{
				return new CanSelectMultipleEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.ValueByRowAndColumn)
			{
				return new ValueByRowAndColumnEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.SelectedRowsCount)
			{
				return new SelectedRowsCountEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.SelectedValueByColumn)
			{
				return new SelectedValueByColumnEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.IsSelected)
			{
				return new IsSelectedEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.ValueByColumnIndex)
			{
				return new ValueByColumnIndexEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.ValueByColumnName)
			{
				return new ValueByColumnNameEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.SelectedItemsCount)
			{
				return new SelectedItemsCountEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.SelectedItemByIndex)
			{
				return new SelectedItemByIndexEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.Index)
			{
				return new IndexEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.Value)
			{
				return new ValueEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.Minimum)
			{
				return new MinimumEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.Maximum)
			{
				return new MaximumEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.Root)
			{
				return new RootEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.SubItemsCount)
			{
				return new SubItemsCountEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.SubItemByIndex)
			{
				return new SubItemByIndexEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.IsExpanded)
			{
				return new IsExpandedEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.IsCollapsed)
			{
				return new IsCollapsedEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.IsMinimized)
			{
				return new IsMinimizedEvaluator(libraryElement);
			}
			else if (propertyId == PropertyId.IsMaximized)
			{
				return new IsMaximizedEvaluator(libraryElement);
			}
			
			return null;
		}
		
		private interface IEvaluator
		{
			object Evaluate(Variable variable);
		}
		
		private class IsPressedEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public IsPressedEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
				if (controlType == ControlType.Button)
				{
					UIDA_Button button = libraryElement as UIDA_Button;
					if (button != null)
					{
						return button.IsPressed;
					}
				}
				return null;
			}
		}
		
		private class SelectedDateEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public SelectedDateEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class IsCheckedEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public IsCheckedEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class SelectedItemEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public SelectedItemEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class SelectedItemIndexEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public SelectedItemIndexEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class ItemByIndexEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public ItemByIndexEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class ItemsCountEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public ItemsCountEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class ColumnCountEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public ColumnCountEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class RowCountEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public RowCountEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class CanSelectMultipleEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public CanSelectMultipleEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
				if (controlType == ControlType.DataGrid)
				{
					UIDA_DataGrid dataGrid = libraryElement as UIDA_DataGrid;
					if (dataGrid != null)
					{
						return dataGrid.CanSelectMultiple;
					}
				}
				return null;
			}
		}
		
		private class ValueByRowAndColumnEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public ValueByRowAndColumnEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class SelectedRowsCountEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public SelectedRowsCountEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
				if (controlType == ControlType.DataGrid)
				{
					UIDA_DataGrid dataGrid = libraryElement as UIDA_DataGrid;
					if (dataGrid != null)
					{
						return dataGrid.SelectedRows.Length;
					}
				}
				return null;
			}
		}
		
		private class SelectedValueByColumnEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public SelectedValueByColumnEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class IsSelectedEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public IsSelectedEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class ValueByColumnIndexEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public ValueByColumnIndexEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class ValueByColumnNameEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public ValueByColumnNameEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class SelectedItemsCountEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public SelectedItemsCountEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
				if (controlType == ControlType.List)
				{
					UIDA_List list = libraryElement as UIDA_List;
					if (list != null)
					{
						return list.SelectedItems.Length;
					}
				}
				return null;
			}
		}
		
		private class SelectedItemByIndexEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public SelectedItemByIndexEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class IndexEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public IndexEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class ValueEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public ValueEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class MinimumEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public MinimumEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class MaximumEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public MaximumEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class RootEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public RootEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
				if (controlType == ControlType.Tree)
				{
					UIDA_Tree tree = libraryElement as UIDA_Tree;
					if (tree != null)
					{
						return tree.GetRoot().GetText();
					}
				}
				return null;
			}
		}
		
		private class SubItemsCountEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public SubItemsCountEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
				if (controlType == ControlType.TreeItem)
				{
					UIDA_TreeItem treeItem = libraryElement as UIDA_TreeItem;
					if (treeItem != null)
					{
						return treeItem.SubItems.Length;
					}
				}
				return null;
			}
		}
		
		private class SubItemByIndexEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public SubItemByIndexEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
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
				return null;
			}
		}
		
		private class IsExpandedEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public IsExpandedEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
				if (controlType == ControlType.TreeItem)
				{
					UIDA_TreeItem treeItem = libraryElement as UIDA_TreeItem;
					if (treeItem != null)
					{
						return treeItem.ExpandCollapseState == UIAutomationClient.ExpandCollapseState.ExpandCollapseState_Expanded;
					}
				}
				return null;
			}
		}
		
		private class IsCollapsedEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public IsCollapsedEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
				if (controlType == ControlType.TreeItem)
				{
					UIDA_TreeItem treeItem = libraryElement as UIDA_TreeItem;
					if (treeItem != null)
					{
						return treeItem.ExpandCollapseState != UIAutomationClient.ExpandCollapseState.ExpandCollapseState_Expanded;
					}
				}
				return null;
			}
		}
		
		private class IsMinimizedEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public IsMinimizedEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
				if (controlType == ControlType.Window)
				{
					UIDA_Window window = libraryElement as UIDA_Window;
					if (window != null)
					{
						return window.IsMinimized;
					}
				}
				return null;
			}
		}
		
		private class IsMaximizedEvaluator: IEvaluator
		{
			private ElementBase libraryElement;
			
			public IsMaximizedEvaluator(ElementBase libraryElement)
			{
				this.libraryElement = libraryElement;
			}
			
			public object Evaluate(Variable variable)
			{
				ControlType controlType = variable.Element.ControlType;
				if (controlType == ControlType.Window)
				{
					UIDA_Window window = libraryElement as UIDA_Window;
					if (window != null)
					{
						return window.IsMaximized;
					}
				}
				return null;
			}
		}
	}
}