using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlSelectByIndex.xaml
    /// </summary>
    public partial class UserControlSelectByIndex : UserControl, IParameters
    {
        public UserControlSelectByIndex(SelectType selectType)
        {
            InitializeComponent();
			
			if (selectType == SelectType.AddToSelection)
			{
				txbTitle.Text = "Add the Nth item to selection";
			}
			else if (selectType == SelectType.RemoveFromSelection)
			{
				txbTitle.Text = "Remove the Nth item from selection";
			}
        }
		
		public bool ValidateParams(Action action)
		{
			var window = Window.GetWindow(this);
			
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
			
			action.Parameters = new List<object>() { index };
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
	
	public enum SelectType
	{
		Select,
		AddToSelection,
		RemoveFromSelection
	}
}
