using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlSelectByText.xaml
    /// </summary>
    public partial class UserControlSelectByText : UserControl, IParameters
    {
        public UserControlSelectByText(bool multiple, SelectType selectType = SelectType.Select)
        {
            InitializeComponent();
			
			if (selectType == SelectType.AddToSelection)
			{
				if (multiple == true)
				{
					txbTitle.Text = "Add all items matching the given text to selection";
					txbDesc.Text = "Wildcards can be used (*.). All matching items will be added to selection.";
				}
			}
			else if (selectType == SelectType.RemoveFromSelection)
			{
				if (multiple == true)
				{
					txbTitle.Text = "Remove all items matching the given text from selection";
					txbDesc.Text = "Wildcards can be used (*.). All matching items will be removed from selection.";
				}
			}
			else if (multiple == false)
			{
				txbTitle.Text = "Select the first item matching the given text";
				txbDesc.Text = "Wildcards can be used (*.).";
			}
        }
		
		public bool ValidateParams(Action action)
		{
			if (txtText.Text == "")
			{
				MessageBox.Show(Window.GetWindow(this), "Text cannot be empty");
				txtText.Focus();
				return false;
			}
			
			action.Parameters = new List<object>() { txtText.Text };
			return true;
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null || parameters.Count != 1)
			{
				return;
			}
			
			txtText.Text = parameters[0].ToString();
		}
    }
}
