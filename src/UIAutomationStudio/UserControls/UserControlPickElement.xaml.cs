using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using UIAutomationClient;
using System.ComponentModel;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControlPickElement.xaml
    /// </summary>
    public partial class UserControlPickElement : UserControl
    {
		private ForegroundColors foregroundColors;
		public static UserControlPickElement Instance = null;
		
        public UserControlPickElement(Element element, bool isGeneral, bool isVariable = false)
        {
            InitializeComponent();
			Instance = this;
			HighlightHelper.guard = false;
			
			if (element != null)
			{
				SelectElement(element);
			}
			else
			{
				this.SelectedElement = null;
			}
			this.SelectedElementChanged = false;
			
			if (isGeneral == true)
			{
				chkChooseElement.IsChecked = true;
				OnCheckChooseElement(null, null);
			}
			
			if (isVariable == true)
			{
				chkChooseElement.Visibility = Visibility.Hidden;
			}
			
			cmbLastSelected.ItemsSource = lastSelectedElements;
			
			foregroundColors = new ForegroundColors();
			this.DataContext = foregroundColors;
			
			btnHighlight.IsChecked = true;
			OnHighlight(null, null);
			
			this.Loaded += UserControl_Loaded;
        }
		
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			Window window = Window.GetWindow(this);
			window.Closing += window_Closing;
		}
		
		private void window_Closing(object sender, CancelEventArgs e)
		{
			if (timer != null && timer.Enabled)
			{
				timer.Stop();
			}
		}
		
		public void VerifyControls()
		{
			HighlightHelper.guard = true;
			if (btnChooseElement.IsChecked == true)
			{
				btnChooseElement.IsChecked = false;
				OnSelectElement(null, null);
			}
			
			if (btnHighlight.IsChecked == true)
			{
				btnHighlight.IsChecked = false;
				OnHighlight(null, null);
				
				HighlightHelper.CloseHighlightFrames();
			}
			HighlightHelper.guard = false;
		}
		
		public bool CheckEmptyElement()
		{
			if (chkChooseElement.IsChecked.Value == false && SelectedElement == null)
			{
				return false;
			}
			return true;
		}
		
		private void OnCheckChooseElement(object sender, RoutedEventArgs e)
		{
			if (chkChooseElement.IsChecked.Value == true)
			{
				VerifyControls();
			}
			
			if (chkChooseElement.IsChecked.Value == true)
			{
				btnSelectParent.IsEnabled = false;
			}
			else if (this.SelectedElement != null)
			{
				btnSelectParent.IsEnabled = true;
			}
			
			if (chkChooseElement.IsChecked.Value == true)
			{
				txbLastSelected.Foreground = Brushes.Gray;
				txtElementType.Foreground = Brushes.Gray;
				txtElementText.Foreground = Brushes.Gray;
				txtElementTypeLabel.Foreground = Brushes.Gray;
				txtElementTextLabel.Foreground = Brushes.Gray;
				txtDescription.Foreground = Brushes.Gray;
				cmbLastSelected.IsEnabled = false;
				btnChooseElement.IsEnabled = false;
				btnHighlight.IsEnabled = false;
			}
			else
			{
				txbLastSelected.Foreground = Brushes.Black;
				txtElementType.Foreground = Brushes.Blue;
				txtElementText.Foreground = Brushes.Blue;
				txtElementTypeLabel.Foreground = Brushes.Black;
				txtElementTextLabel.Foreground = Brushes.Black;
				txtDescription.Foreground = Brushes.Black;
				cmbLastSelected.IsEnabled = true;
				btnChooseElement.IsEnabled = true;
				btnHighlight.IsEnabled = true;
			}
			
			this.IsGeneralAction = chkChooseElement.IsChecked.Value;
		}
		
		public bool IsGeneralAction { get; set; }
		
		private void OnSelectElement(object sender, RoutedEventArgs e)
		{
			if (btnChooseElement.IsChecked == true)
			{
				btnChooseElement.Content = "_Selecting Element...";
				btnChooseElement.FontWeight = FontWeights.Bold;
				
				// start timer
				if (timer == null)
                {
                    timer = new System.Windows.Forms.Timer();

                    timer.Interval = 1000;
                    timer.Tick += timer_Tick;
                }
				
				timer.Start();
			}
			else
			{
				btnChooseElement.Content = "_Select Element";
				btnChooseElement.FontWeight = FontWeights.Normal;
				
				// stop timer
				if (timer != null)
                {
                    timer.Stop();
                }
			}
		}
		
		void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                //bool leftCtrlIsPressed = ((int)Keyboard.GetKeyStates(Key.LeftCtrl) & (int)KeyStates.Down) != 0;
				bool leftCtrlIsPressed = ((int)Keyboard.GetKeyStates(Key.LeftShift) & (int)KeyStates.Down) != 0;
                //bool rightCtrlIsPressed = ((int)Keyboard.GetKeyStates(Key.RightCtrl) & (int)KeyStates.Down) != 0;
				bool rightCtrlIsPressed = ((int)Keyboard.GetKeyStates(Key.RightShift) & (int)KeyStates.Down) != 0;

                if (leftCtrlIsPressed || rightCtrlIsPressed)
                {
                    int xMouse = Convert.ToInt32(System.Windows.Forms.Cursor.Position.X);
                    int yMouse = Convert.ToInt32(System.Windows.Forms.Cursor.Position.Y);

					tagPOINT mousePoint = new tagPOINT {x = xMouse, y = yMouse};
					IUIAutomationElement automationElement = MainWindow.uiAutomation.ElementFromPoint(mousePoint);
					
					Element element = new Element(automationElement);
					SelectElement(element);
					InsertToLastSelected(element);
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(Window.GetWindow(this), "Exception: " + ex.Message);
            }
        }
		
		private static int LAST_SELECTED_MAX = 7;
		private void InsertToLastSelected(Element element)
		{
			int index = -1;

			for (int i = 0; i < lastSelectedElements.Count; i++)
			{
				Element crtElem = lastSelectedElements[i];
				if (Helper.CompareElements(crtElem.InnerElement, element.InnerElement) == true)
				{
					index = i;
					break;
				}
			}
			
			if (index == -1)
			{
				lastSelectedElements.Insert(0, element);
				if (lastSelectedElements.Count > LAST_SELECTED_MAX)
				{
					lastSelectedElements.RemoveAt(LAST_SELECTED_MAX);
				}
				
				if (cmbLastSelected.SelectedItem != null)
				{
					cmbLastSelected.SelectedIndex = 0;
				}
			}
			else
			{
				if (index == 0)
				{
					return;
				}
				Element elementRemoved = lastSelectedElements[index];
				
				lastSelectedElements.RemoveAt(index);
				lastSelectedElements.Insert(0, elementRemoved);
				
				if (cmbLastSelected.SelectedItem != null)
				{
					cmbLastSelected.SelectedIndex = 0;
				}
			}
		}
		
		private void SelectElement(Element element)
		{
			this.SelectedElement = element;
			this.SelectedElementChanged = true;
			
			btnSelectParent.IsEnabled = true;
			
			if (btnHighlight.IsChecked == true)
			{
				HighlightHelper.HighlightElement(element.InnerElement);
			}
			
			txtElementType.Text = this.SelectedElement.ControlType.ToString();
			txtElementText.Text = this.SelectedElement.GetShortName(70);
		}
		
		private void OnHighlight(object sender, RoutedEventArgs e)
		{
			if (btnHighlight.IsChecked == true)
			{
				btnHighlight.Content = "_Highlighting...";
				btnHighlight.FontWeight = FontWeights.Bold;
				
				if (this.SelectedElement != null)
				{
					HighlightHelper.HighlightElement(this.SelectedElement.InnerElement);
				}
			}
			else
			{
				btnHighlight.Content = "_Highlight";
				btnHighlight.FontWeight = FontWeights.Normal;
				
				HighlightHelper.UnHighlight();
			}
		}
		
		private void OnSelectParent(object sender, RoutedEventArgs e)
		{
			if (this.SelectedElement == null)
			{
				return;
			}
			
			SelectParentWindow selectParentWindow = new SelectParentWindow(this.SelectedElement);
			selectParentWindow.Owner = Window.GetWindow(this);
			if (selectParentWindow.ShowDialog() == true)
			{
				SelectElement(selectParentWindow.SelectedElement);
				InsertToLastSelected(selectParentWindow.SelectedElement);
			}
		}
		
		private void OnSelectionChanged(object sender, RoutedEventArgs e)
		{
			Element selectedElement = cmbLastSelected.SelectedItem as Element;
			if (selectedElement == null)
			{
				return;
			}
			
			SelectElement(selectedElement);
		}
		
		public Element SelectedElement { get; set; }
		public bool SelectedElementChanged { get; set; }
		
		private System.Windows.Forms.Timer timer = null;
		private static ObservableCollection<Element> lastSelectedElements = new ObservableCollection<Element>();
    }
	
	public class ForegroundColors: INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		void Notify(string propName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}
		
		SolidColorBrush labelForegroundColor;
		public SolidColorBrush LabelForegroundColor
		{
			get
			{
				return labelForegroundColor;
			}
			set
			{
				labelForegroundColor = value;
				Notify("LabelForegroundColor");
			}
		}
		
		SolidColorBrush textForegroundColor;
		public SolidColorBrush TextForegroundColor
		{
			get
			{
				return textForegroundColor;
			}
			set
			{
				textForegroundColor = value;
				Notify("TextForegroundColor");
			}
		}
		
		bool myIsEnabled;
		public bool MyIsEnabled
		{
			get
			{
				return myIsEnabled;
			}
			set
			{
				myIsEnabled = value;
				Notify("MyIsEnabled");
			}
		}
		
		public ForegroundColors()
		{
			labelForegroundColor = Brushes.Black;
			textForegroundColor = Brushes.Blue;
			myIsEnabled = true;
		}
	}
}
