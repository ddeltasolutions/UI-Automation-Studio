using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Controls;
using UIAutomationClient;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow : Window
    {
        public PropertiesWindow(Element element)
        {
            InitializeComponent();
			
			this.element = element;
			this.ancestors = new List<Element>();
			this.HasChanged = false;
			
			if (element != null)
			{
				Element parent = element.Parent;
				while (parent != null)
				{
					this.ancestors.Insert(0, parent);
					parent = parent.Parent;
				}
			}
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {
			if (this.ancestors.Count >= 2)
			{
				txtWindow.Text = this.ancestors[1].Name;
				txtElement.Text = this.element.Name;
			}
			else if (this.ancestors.Count == 1)
			{
				txtWindow.Text = this.element.Name;
				spElement.Visibility = Visibility.Hidden;
			}
        }
		
		private void OnChange(object sender, RoutedEventArgs e)
		{
			Button btn = sender as Button;
			if (btn == null)
			{
				return;
			}
			
			EditFieldWindow editFieldWindow = new EditFieldWindow() { Owner = this };
			if (btn == btnChangeTopLevel)
			{
				editFieldWindow.FieldValue = txtWindow.Text;
			}
			else if (btn == btnChangeElement)
			{
				editFieldWindow.FieldValue = txtElement.Text;
			}
			
			if (editFieldWindow.ShowDialog() == true)
			{
				if (btn == btnChangeTopLevel && this.ancestors.Count >= 2)
				{
					if (this.ancestors[1].Name != editFieldWindow.FieldValue)
					{
						this.ancestors[1].Name = editFieldWindow.FieldValue;
						txtWindow.Text = editFieldWindow.FieldValue;
						this.Task.IsModified = true;
					}
				}
				else if (this.element.Name != editFieldWindow.FieldValue)
				{
					this.element.Name = editFieldWindow.FieldValue;
					txtElement.Text = editFieldWindow.FieldValue;
					this.HasChanged = true;
					this.Task.IsModified = true;
					this.Task.Changed();
				}
			}
		}
		
		public bool HasChanged { get; set; }
		public Task Task { get; set; }
		
		private Element element = null;
		private List<Element> ancestors = null;
	}
}