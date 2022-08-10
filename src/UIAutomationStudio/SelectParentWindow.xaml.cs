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
    /// Interaction logic for AddActionWindow.xaml
    /// </summary>
    public partial class SelectParentWindow : Window
    {
        public SelectParentWindow(Element element)
        {
            InitializeComponent();
			
			List<Element> allParents = new List<Element>();
			Element crtElement = element;
			
			while (crtElement != null)
			{
				allParents.Insert(0, crtElement);
				crtElement = crtElement.Parent;
			}
			
			TreeViewItem prevItem = null;
			
			foreach (Element el in allParents)
			{
				TreeViewItem item = new TreeViewItem() { Tag = el, IsExpanded = true };
				item.Header = el.GetShortName(50) + " (" + el.ControlType + ")";
				if (prevItem != null)
				{
					prevItem.Items.Add(item);
				}
				else
				{
					treeViewParents.Items.Add(item);
				}
				prevItem = item;
			}
			
			prevItem.IsSelected = true;
			treeViewParents.Focus();
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }
		
		private void OnOK(object sender, RoutedEventArgs e)
		{
			TreeViewItem selectedItem = treeViewParents.SelectedItem as TreeViewItem;
			SelectedElement = selectedItem.Tag as Element;
			
			this.DialogResult = true;
			this.Close();
		}
		
		public Element SelectedElement { get; set; }
	}
}