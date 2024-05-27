using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlSelectProperty.xaml
    /// </summary>
    public partial class UserControlSelectProperty : UserControl
    {
		public bool PropertyIdChanged { get; set; }
		
		private Variable variable = null;
		private ControlType initialControlType = ControlType.None;
		
		private ObservableCollection<PropertyInfo> specificProperties = new ObservableCollection<PropertyInfo>();
		private List<PropertyInfo> generalProperties = new List<PropertyInfo>();
		
		private PropertyId currentPropertyId = PropertyId.None;
		
		private UserControlItemByIndex ucItemByIndex = null;
		private UserControlValueByRowAndColumn ucValueByRowAndColumn = null;
	
        public UserControlSelectProperty(Variable variable, ControlType controlType)
        {
            InitializeComponent();
			
			this.variable = variable;
			this.initialControlType = controlType;
			this.PropertyIdChanged = true;
        }
		
		public void RefreshPropertiesTab()
		{
			ControlType controlType = variable.Element.ControlType;
		
			//Specific properties
			specificProperties.Clear();
			
			if (controlType == ControlType.Button)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Button is pressed", PropertyId = PropertyId.IsPressed });
			}
			else if (controlType == ControlType.Calendar)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Selected Date", PropertyId = PropertyId.SelectedDate });
			}
			else if (controlType == ControlType.CheckBox)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Check box is checked", PropertyId = PropertyId.IsChecked });
			}
			else if (controlType == ControlType.ComboBox)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Selected item text", PropertyId = PropertyId.SelectedItem });
				specificProperties.Add(
					new PropertyInfo { Description = "Selected item index", PropertyId = PropertyId.SelectedItemIndex });
				specificProperties.Add(
					new PropertyInfo { Description = "Item text by index", PropertyId = PropertyId.ItemByIndex });
				specificProperties.Add(
					new PropertyInfo { Description = "Items Count", PropertyId = PropertyId.ItemsCount });
			}
			else if (controlType == ControlType.DataGrid)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Column Count", PropertyId = PropertyId.ColumnCount });
				specificProperties.Add(
					new PropertyInfo { Description = "Row Count", PropertyId = PropertyId.RowCount });
				specificProperties.Add(
					new PropertyInfo { Description = "Can Select Multiple items", PropertyId = PropertyId.CanSelectMultiple });
				specificProperties.Add(
					new PropertyInfo { Description = "Value of the cell at the specified row and column", 
					PropertyId = PropertyId.ValueByRowAndColumn });
				specificProperties.Add(
					new PropertyInfo { Description = "Selected Rows Count", PropertyId = PropertyId.SelectedRowsCount });
				specificProperties.Add(
					new PropertyInfo { Description = "Value of the cell in the selected row at the specified column", 
					PropertyId = PropertyId.SelectedValueByColumn });
			}
			else if (controlType == ControlType.Group)
			{
				
			}
			else if (controlType == ControlType.DataItem)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Is Selected", PropertyId = PropertyId.IsSelected });
				specificProperties.Add(
					new PropertyInfo { Description = "Cell text by column index", PropertyId = PropertyId.ValueByColumnIndex });
				specificProperties.Add(
					new PropertyInfo { Description = "Cell text by column name", PropertyId = PropertyId.ValueByColumnName });
			}
			else if (controlType == ControlType.DatePicker)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Selected Date", PropertyId = PropertyId.SelectedDate });
			}
			else if (controlType == ControlType.Edit || controlType == ControlType.Document)
			{
				
			}
			else if (controlType == ControlType.Hyperlink)
			{
				
			}
			else if (controlType == ControlType.List)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Selected Items Count", PropertyId = PropertyId.SelectedItemsCount });
				specificProperties.Add(
					new PropertyInfo { Description = "Selected Item by Index", PropertyId = PropertyId.SelectedItemByIndex });
				specificProperties.Add(
					new PropertyInfo { Description = "First Selected Item text", PropertyId = PropertyId.SelectedItem });
				specificProperties.Add(
					new PropertyInfo { Description = "First Selected Item Index", PropertyId = PropertyId.SelectedItemIndex });
				specificProperties.Add(
					new PropertyInfo { Description = "Item text by Index", PropertyId = PropertyId.ItemByIndex });
				specificProperties.Add(
					new PropertyInfo { Description = "Items Count", PropertyId = PropertyId.ItemsCount });
			}
			else if (controlType == ControlType.ListItem || controlType == ControlType.MenuItem)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Index", PropertyId = PropertyId.Index });
					
				if (controlType == ControlType.ListItem)
				{
					specificProperties.Add(
						new PropertyInfo { Description = "Is Selected", PropertyId = PropertyId.IsSelected });
				}
				
				specificProperties.Add(
					new PropertyInfo { Description = "Is Checked", PropertyId = PropertyId.IsChecked });
			}
			else if (controlType == ControlType.RadioButton)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Radio Button Is Selected", PropertyId = PropertyId.IsSelected });
			}
			else if (controlType == ControlType.ScrollBar || controlType == ControlType.Slider || 
				controlType == ControlType.Spinner || controlType == ControlType.ProgressBar)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Value", PropertyId = PropertyId.Value });
				specificProperties.Add(
					new PropertyInfo { Description = "Minimum", PropertyId = PropertyId.Minimum });
				specificProperties.Add(
					new PropertyInfo { Description = "Maximum", PropertyId = PropertyId.Maximum });
			}
			else if (controlType == ControlType.SplitButton)
			{
				
			}
			else if (controlType == ControlType.Tab)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Selected Item text", PropertyId = PropertyId.SelectedItem });
				specificProperties.Add(
					new PropertyInfo { Description = "Selected Item Index", PropertyId = PropertyId.SelectedItemIndex });
				specificProperties.Add(
					new PropertyInfo { Description = "Item text by Index", PropertyId = PropertyId.ItemByIndex });
				specificProperties.Add(
					new PropertyInfo { Description = "Items Count", PropertyId = PropertyId.ItemsCount });
			}
			else if (controlType == ControlType.TabItem)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Is Selected", PropertyId = PropertyId.IsSelected });
				specificProperties.Add(
					new PropertyInfo { Description = "Index", PropertyId = PropertyId.Index });
			}
			else if (controlType == ControlType.Tree)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Text of Root item", PropertyId = PropertyId.Root });
			}
			else if (controlType == ControlType.TreeItem)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "SubItems Count", PropertyId = PropertyId.SubItemsCount });
				specificProperties.Add(
					new PropertyInfo { Description = "SubItem by Index", PropertyId = PropertyId.SubItemByIndex });
				specificProperties.Add(
					new PropertyInfo { Description = "Is Expanded", PropertyId = PropertyId.IsExpanded });
				specificProperties.Add(
					new PropertyInfo { Description = "Is Collapsed", PropertyId = PropertyId.IsCollapsed });
				specificProperties.Add(
					new PropertyInfo { Description = "Is Checked", PropertyId = PropertyId.IsChecked });
			}
			else if (controlType == ControlType.Custom)
			{
				
			}
			else if (controlType == ControlType.Table)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Column Count", PropertyId = PropertyId.ColumnCount });
				specificProperties.Add(
					new PropertyInfo { Description = "Row Count", PropertyId = PropertyId.RowCount });
				specificProperties.Add(
					new PropertyInfo { Description = "Value of the cell at the specified row and column", 
					PropertyId = PropertyId.ValueByRowAndColumn });
			}
			else if (controlType == ControlType.Window)
			{
				specificProperties.Add(
					new PropertyInfo { Description = "Is Minimized", PropertyId = PropertyId.IsMinimized });
				specificProperties.Add(
					new PropertyInfo { Description = "Is Maximized", PropertyId = PropertyId.IsMaximized });
			}
			tabSpecific.DataContext = specificProperties;
			
			// General Properties
			if (generalProperties.Count == 0)
			{
				generalProperties.Add(new PropertyInfo { Description = "Text of the element", 
					PropertyId = PropertyId.Text });
				generalProperties.Add(new PropertyInfo { Description = "Left coordinate of the element in pixels", 
					PropertyId = PropertyId.Left });
				generalProperties.Add(new PropertyInfo { Description = "Top coordinate of the element in pixels", 
					PropertyId = PropertyId.Top });
				generalProperties.Add(new PropertyInfo { Description = "Width of the element in pixels", 
					PropertyId = PropertyId.Width });
				generalProperties.Add(new PropertyInfo { Description = "Height of the element in pixels", 
					PropertyId = PropertyId.Height });
				generalProperties.Add(new PropertyInfo { Description = "Element Is Enabled", 
					PropertyId = PropertyId.IsEnabled });
				generalProperties.Add(new PropertyInfo { Description = "Element Is Found", 
					PropertyId = PropertyId.IsFound });
				
				tabGeneral.DataContext = generalProperties;
			}
			else
			{
				listGeneral.SelectedItem = null;
			}
			
			PropertyInfo propertyInfo = null;
			bool tabSelected = false;
			
			if (this.variable.PropertyId != PropertyId.None && this.variable.Element.ControlType == this.initialControlType)
			{
				this.currentPropertyId = this.variable.PropertyId;
				
				propertyInfo = specificProperties.FirstOrDefault(x => x.PropertyId == this.variable.PropertyId);
				if (propertyInfo != null)
				{
					tabSpecific.IsSelected = true;
					tabSelected = true;
					
					listSpecificProperties.Focus();
					listSpecificProperties.SelectedItem = propertyInfo;
				}
			}
			else
			{
				this.currentPropertyId = PropertyId.None;
			}
			
			if (this.variable.PropertyId != PropertyId.None)
			{
				propertyInfo = generalProperties.FirstOrDefault(x => x.PropertyId == this.variable.PropertyId);
				if (propertyInfo != null)
				{
					tabGeneral.IsSelected = true;
					tabSelected = true;
					
					Keyboard.Focus(listGeneral);
					listGeneral.SelectedItem = propertyInfo;
				}
			}
			
			if (specificProperties.Count == 0)
			{
				tabSpecific.Visibility = Visibility.Collapsed;
			}
			else
			{
				tabSpecific.Visibility = Visibility.Visible;
			}
			
			if (tabSelected == false)
			{
				if (specificProperties.Count == 0)
				{
					tabGeneral.IsSelected = true;
				}
				else
				{
					if (tabGeneral.IsSelected == true)
					{
						tabSpecific.IsSelected = true;
					}
				}
				
				parametersGroupBox.Content = null;
			}
		}
		
		private void OnDoubleClick(object sender, RoutedEventArgs e)
		{
			ListBox list = sender as ListBox;
			if (list == null)
			{
				return;
			}
			
			PropertyInfo propertyInfo = list.SelectedItem as PropertyInfo;
			if (propertyInfo == null)
			{
				return;
			}

			ShowActionParameters(propertyInfo);
		}
		
		private void OnPropertySelected(object sender, SelectionChangedEventArgs e)
		{
			System.Collections.IList selectedItems = e.AddedItems;
			if (selectedItems.Count == 0)
			{
				return;
			}
			
			PropertyInfo propertyInfo = selectedItems[0] as PropertyInfo;
			if (propertyInfo == null)
			{
				return;
			}
			
			ShowActionParameters(propertyInfo);
		}
		
		private void ShowActionParameters(PropertyInfo propertyInfo)
		{
			if (this.currentPropertyId != propertyInfo.PropertyId)
			{
				this.PropertyIdChanged = true;
				this.currentPropertyId = propertyInfo.PropertyId;
			}
			
			ResetUserControls();
			
			if (propertyInfo.PropertyId == PropertyId.ItemByIndex || 
				propertyInfo.PropertyId == PropertyId.SelectedItemByIndex || 
				propertyInfo.PropertyId == PropertyId.SubItemByIndex)
			{
				ucItemByIndex = new UserControlItemByIndex(propertyInfo.PropertyId);
				parametersGroupBox.Content = ucItemByIndex;
			}
			else if (propertyInfo.PropertyId == PropertyId.ValueByRowAndColumn || 
				propertyInfo.PropertyId == PropertyId.SelectedValueByColumn)
			{
				ucValueByRowAndColumn = new UserControlValueByRowAndColumn(propertyInfo.PropertyId);
				parametersGroupBox.Content = ucValueByRowAndColumn;
			}
			else if (propertyInfo.PropertyId == PropertyId.ValueByColumnIndex || 
				propertyInfo.PropertyId == PropertyId.ValueByColumnName)
			{
				ucItemByIndex = new UserControlItemByIndex(propertyInfo.PropertyId);
				parametersGroupBox.Content = ucItemByIndex;
			}
			else
			{
				parametersGroupBox.Content = null;
			}
			
			if (this.variable.PropertyId == propertyInfo.PropertyId)
			{
				if (!tabSpecific.IsSelected || 
					(tabSpecific.IsSelected && this.variable.Element.ControlType == this.initialControlType))
				{
					IParameters iparameters = parametersGroupBox.Content as IParameters;
					if (iparameters != null)
					{
						iparameters.Init(this.variable.Parameters);
					}
				}
			}
		}
		
		private void ResetUserControls()
		{
			ucItemByIndex = null;
			ucValueByRowAndColumn = null;
		}
		
		public bool Validate()
		{
			if (currentPropertyId == PropertyId.None)
			{
				MessageBox.Show(Window.GetWindow(this), "Please select a property");
				return false;
			}
			
			bool isValid = true;
			this.variable.Parameters = null;
			
			if (ucItemByIndex != null)
			{
				isValid = ucItemByIndex.ValidateParams(this.variable);
			}
			else if (ucValueByRowAndColumn != null)
			{
				isValid = ucValueByRowAndColumn.ValidateParams(this.variable);
			}
			
			if (isValid == true)
			{
				this.variable.PropertyId = currentPropertyId;
				return true;
			}
			return false;
		}
    }
	
	public class PropertyInfo
	{
		public string Description { get; set; }
		public PropertyId PropertyId { get; set; }
	}
}
