using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlSetText.xaml
    /// </summary>
    public partial class UserControlSetText : UserControl, IParameters
    {
        public UserControlSetText(bool multiline, bool selectText = false)
        {
            InitializeComponent();
			
			if (multiline == false)
			{
				txtTextToSet.Height = 24;
			}
			else
			{
				if (selectText == true)
				{
					txbTitle.Text = "Text to select";
				}
				txtTextToSet.TextWrapping = TextWrapping.Wrap;
				txtTextToSet.AcceptsReturn = true;
				txtTextToSet.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
				txtTextToSet.Height = 50;
			}
        }
		
		public bool ValidateParams(Action action)
		{
			action.Parameters = new List<object>() { txtTextToSet.Text };
			return true;
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null || parameters.Count != 1)
			{
				return;
			}
			
			txtTextToSet.Text = parameters[0].ToString();
		}
    }
}
