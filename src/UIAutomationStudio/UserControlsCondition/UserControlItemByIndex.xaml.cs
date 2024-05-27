using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlItemByIndex.xaml
    /// </summary>
    public partial class UserControlItemByIndex : UserControl, IParameters
    {
		private PropertyId propertyId = PropertyId.None;
	
        public UserControlItemByIndex(PropertyId propertyId)
        {
            InitializeComponent();
			
			this.propertyId = propertyId;
			if (propertyId == PropertyId.SelectedItemByIndex)
			{
				txbTitle.Text = "Text of the Nth selected item (specify 0 to get the first selected item)";
				txtIndex.Text = "0";
			}
			else if (propertyId == PropertyId.ValueByColumnIndex)
			{
				txbTitle.Text = "Text of the cell at the specified column index";
				txbLabel.Text = "Column index (starts with 0): ";
			}
			else if (propertyId == PropertyId.ValueByColumnName)
			{
				txbTitle.Text = "Text of the cell at the specified column name";
				txbLabel.Text = "Column name: ";
				txtIndex.Width = 200;
			}
			else if (propertyId == PropertyId.SubItemByIndex)
			{
				txbTitle.Text = "Text of the Nth SubItem";
			}
        }
		
		public bool ValidateParams(Variable variable)
		{
			var window = Window.GetWindow(this);
			
			if (propertyId == PropertyId.ValueByColumnName)
			{
				string columnName = txtIndex.Text;
				if (columnName == "")
				{
					MessageBox.Show(window, "Please specify a column name");
					txtIndex.Focus();
					txtIndex.SelectAll();
					return false;
				}
				
				variable.Parameters = new List<object>() { columnName };
				return true;
			}
			
			int index = 0;
			if (int.TryParse(txtIndex.Text, out index) == false)
			{
				MessageBox.Show(window, "N must be an integer positive value");
				txtIndex.Focus();
				txtIndex.SelectAll();
				return false;
			}
			
			if (index < 0)
			{
				MessageBox.Show(window, "N must be an integer positive value");
				txtIndex.Focus();
				txtIndex.SelectAll();
				return false;
			}
			
			variable.Parameters = new List<object>() { index };
			return true;
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null || parameters.Count != 1)
			{
				return;
			}
			
			txtIndex.Text = parameters[0].ToString();
		}
    }
}
