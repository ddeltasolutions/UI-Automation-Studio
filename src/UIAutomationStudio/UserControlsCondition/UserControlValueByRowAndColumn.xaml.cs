using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlValueByRowAndColumn.xaml
    /// </summary>
    public partial class UserControlValueByRowAndColumn : UserControl, IParameters
    {
        public UserControlValueByRowAndColumn(PropertyId propertyId)
        {
            InitializeComponent();
			
			this.propertyId = propertyId;
			if (propertyId == PropertyId.SelectedValueByColumn)
			{
				txbTitle.Text = "Text of the cell in the selected row at the specified column index";
				gridCtrl.RowDefinitions[1].Height = new GridLength(0);
				txtRowIndex.IsEnabled = false;
			}
        }
		
		public bool ValidateParams(Variable variable)
		{
			var window = Window.GetWindow(this);
			
			int rowIndex = 0;
			if (propertyId != PropertyId.SelectedValueByColumn)
			{
				if (int.TryParse(txtRowIndex.Text, out rowIndex) == false)
				{
					MessageBox.Show(window, "Row Index must be an integer positive value");
					txtRowIndex.Focus();
					txtRowIndex.SelectAll();
					return false;
				}
				
				if (rowIndex < 0)
				{
					MessageBox.Show(window, "Row Index must be an integer positive value");
					txtRowIndex.Focus();
					txtRowIndex.SelectAll();
					return false;
				}
			}
			
			int columnIndex = 0;
			if (int.TryParse(txtColumnIndex.Text, out columnIndex) == false)
			{
				MessageBox.Show(window, "Column Index must be an integer positive value");
				txtColumnIndex.Focus();
				txtColumnIndex.SelectAll();
				return false;
			}
			
			if (columnIndex < 0)
			{
				MessageBox.Show(window, "Column Index must be an integer positive value");
				txtColumnIndex.Focus();
				txtColumnIndex.SelectAll();
				return false;
			}
			
			if (propertyId == PropertyId.SelectedValueByColumn)
			{
				variable.Parameters = new List<object>() { columnIndex };
			}
			else
			{
				variable.Parameters = new List<object>() { rowIndex, columnIndex };
			}
			return true;
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null)
			{
				return;
			}
			
			if (propertyId == PropertyId.SelectedValueByColumn)
			{
				if (parameters.Count != 1)
				{
					return;
				}
				
				txtColumnIndex.Text = parameters[0].ToString();
			}
			else
			{
				if (parameters.Count < 2)
				{
					return;
				}
				
				txtRowIndex.Text = parameters[0].ToString();
				txtColumnIndex.Text = parameters[1].ToString();
			}
		}
		
		private PropertyId propertyId;
    }
}
