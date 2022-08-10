using System;
using System.Windows;
using System.Collections.Generic;
using UIDeskAutomationLib;
using System.Xml;

namespace UIAutomationStudio
{
	public class Variable
	{
		private string displayElement = null;
		public string DisplayElement
		{
			get
			{
				if (Element != null)
				{
					displayElement = Element.ControlTypeName + " (" + Element.GetShortName(50) + ")";
				}
				return displayElement;
			}
			set
			{
				displayElement = value;
			}
		}
		
		public Element Element { get; set; }
		
		private PropertyId propertyId = PropertyId.None;
		public PropertyId PropertyId
		{
			get
			{
				return propertyId;
			}
			set
			{
				propertyId = value;
				
				if (propertyId == PropertyId.Text || propertyId == PropertyId.SelectedItem || 
					propertyId == PropertyId.ItemByIndex || propertyId == PropertyId.ValueByRowAndColumn || 
					propertyId == PropertyId.SelectedValueByColumn || propertyId == PropertyId.ValueByColumnIndex || 
					propertyId == PropertyId.ValueByColumnName || propertyId == PropertyId.SelectedItemByIndex || 
					propertyId == PropertyId.Root || propertyId == PropertyId.SubItemByIndex)
				{
					this.PropertyType = PropertyType.Text;
				}
				else if (propertyId == PropertyId.Left || propertyId == PropertyId.Top || 
					propertyId == PropertyId.Width || propertyId == PropertyId.Height || 
					propertyId == PropertyId.SelectedItemIndex || propertyId == PropertyId.ItemsCount || 
					propertyId == PropertyId.ColumnCount || propertyId == PropertyId.RowCount || 
					propertyId == PropertyId.SelectedRowsCount || propertyId == PropertyId.SelectedItemsCount || 
					propertyId == PropertyId.Index || propertyId == PropertyId.Value || 
					propertyId == PropertyId.Minimum || propertyId == PropertyId.Maximum || 
					propertyId == PropertyId.SubItemsCount)
				{
					this.PropertyType = PropertyType.Number;
				}
				else if (propertyId == PropertyId.IsEnabled || propertyId == PropertyId.IsAlive || 
					propertyId == PropertyId.IsPressed || propertyId == PropertyId.IsChecked || 
					propertyId == PropertyId.CanSelectMultiple || propertyId == PropertyId.IsSelected || 
					propertyId == PropertyId.IsExpanded || propertyId == PropertyId.IsCollapsed || 
					propertyId == PropertyId.IsMinimized || propertyId == PropertyId.IsMaximized)
				{
					this.PropertyType = PropertyType.YesNo;
				}
				else if (propertyId == PropertyId.SelectedDate)
				{
					this.PropertyType = PropertyType.Date;
				}
				else if (propertyId == PropertyId.None)
				{
					this.PropertyType = PropertyType.None;
				}
			}
		}
		public PropertyType PropertyType { get; set; }
		
		public List<object> Parameters { get; set; }
		
		public Variable()
		{
			this.Element = null;
			this.Parameters = null;
		}
		
		public static void DeepCopy(Variable source, Variable destination)
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
			
			destination.DisplayElement = source.DisplayElement;
			destination.PropertyId = source.PropertyId;
			destination.PropertyType = source.PropertyType;
			
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
		}
		
		public object Evaluate()
		{
			try
			{
				return Evaluator.Evaluate(this);
			}
			catch (Exception ex)
			{
				Helper.MessageBoxShow(ex.Message);
				return null;
			}
		}
		
		public string PropertyDescription
		{
			get
			{
				if (PropertyId == PropertyId.ItemByIndex || PropertyId == PropertyId.SelectedValueByColumn ||
					PropertyId == PropertyId.ValueByColumnIndex || PropertyId == PropertyId.SelectedItemByIndex || 
					PropertyId == PropertyId.SubItemByIndex)
				{
					if (this.Parameters.Count > 0)
					{
						return PropertyId.ToString() + "[" + this.Parameters[0] + "]";
					}
				}
				else if (PropertyId == PropertyId.ValueByRowAndColumn)
				{
					if (this.Parameters.Count >= 2)
					{
						return PropertyId.ToString() + "[" + this.Parameters[0] + "," + this.Parameters[1] + "]";
					}
				}
				else if (PropertyId == PropertyId.ValueByColumnName)
				{
					if (this.Parameters.Count > 0)
					{
						return PropertyId.ToString() + "[\"" + this.Parameters[0] + "\"]";
					}
				}
				
				return PropertyId.ToString();
			}
		}
	}
	
	public enum PropertyType
	{
		None,
		YesNo,
		Number,
		Text,
		Date
	}
	
	public enum PropertyId
	{
		None,
	
		// general
		Text,
		Left,
		Top,
		Right,
		Bottom,
		Width,
		Height,
		IsEnabled,
		IsAlive,
		
		// button
		IsPressed,
		
		// calendar, date picker
		SelectedDate,
		
		// check box
		IsChecked, // tree item, list item, menu item
		
		// combo box, list, tab control
		SelectedItem,
		SelectedItemIndex,
		ItemByIndex,
		ItemsCount,
		
		// data grid
		ColumnCount, // table
		RowCount, // table
		CanSelectMultiple,
		ValueByRowAndColumn, // table[row, column]
		SelectedRowsCount,
		SelectedValueByColumn,
		
		// data item
		IsSelected, // tab item, list item, radio button
		ValueByColumnIndex,
		ValueByColumnName,
		
		// list
		SelectedItemsCount,
		SelectedItemByIndex,
		
		// list item, menu item, tab item
		Index,
		
		// progress bar, scroll bar, slider, spinner
		Value, 
		Minimum,
		Maximum,
		
		// tree
		Root,
		
		// tree item
		SubItemsCount,
		SubItemByIndex,
		IsExpanded,
		IsCollapsed,
		
		// window
		IsMinimized,
		IsMaximized
	}
}