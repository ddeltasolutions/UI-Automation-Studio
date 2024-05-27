using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using UIAutomationClient;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControlMouseCoordinates : UserControl, IParameters
    {
		private Element element = null;
		private System.Windows.Forms.Timer timer = null;
	
        public UserControlMouseCoordinates(CoordinatesType coordType, ActionIds actionId, bool screen, Element element = null)
        {
            InitializeComponent();
			this.element = element;
			
			if (screen)
			{
				txbTitle.Text = "Screen mouse coordinates";
			}
			
			if (coordType == CoordinatesType.Move || coordType == CoordinatesType.MoveOffset || 
				coordType == CoordinatesType.Resize || 
				(element != null && element.InnerElement == null && screen == false))
			{
				btnPickCoords.Visibility = Visibility.Hidden;
				txbPickDescription.Visibility = Visibility.Hidden;
				
				mouseCoordsGrid.RowDefinitions[2].Height = new GridLength(0);
				stackPanelValues.SetValue(Grid.ColumnSpanProperty, 2);
			}
			
			if (coordType == CoordinatesType.Move)
			{
				txbTitle.Text = "Move to specified X and Y screen coordinates";
			}
			if (coordType == CoordinatesType.MoveOffset)
			{
				txbTitle.Text = "Specify horizontal and vertical offsets";
				txbX.Text = "Horizontal: ";
				txbY.Text = "Vertical: ";
			}
			if (coordType == CoordinatesType.Resize)
			{
				txbTitle.Text = "Specify width and height of this window";
				txbX.Text = "Width: ";
				txbY.Text = "Height: ";
			}
			else if (actionId == ActionIds.ClickAt)
			{
				txbTitle.Text += " for Click";
			}
			else if (actionId == ActionIds.RightClickAt)
			{
				txbTitle.Text += " for Right Click";
			}
			else if (actionId == ActionIds.MiddleClickAt)
			{
				txbTitle.Text += " for Middle Click";
			}
			else if (actionId == ActionIds.DoubleClickAt)
			{
				txbTitle.Text += " for Double Click";
			}
			else if (actionId == ActionIds.MoveMouse)
			{
				txbTitle.Text += " for Move Mouse Cursor";
			}
			else if (actionId == ActionIds.SimulateClickAt)
			{
				txbTitle.Text += " for Simulate Click";
			}
			else if (actionId == ActionIds.SimulateRightClickAt)
			{
				txbTitle.Text += " for Simulate Right Click";
			}
			else if (actionId == ActionIds.SimulateMiddleClickAt)
			{
				txbTitle.Text += " for Simulate Middle Click";
			}
			else if (actionId == ActionIds.SimulateDoubleClickAt)
			{
				txbTitle.Text += " for Simulate Double Click";
			}
			
			if (element != null && element.InnerElement != null)
			{
				txbPickDescription.Text += " The picked coordinates are relative to the currently selected element.";
			}
			else if (screen == true)
			{
				txbPickDescription.Text += " The picked coordinates are absolute screen coordinates.";
			}
			
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
				timer = null;
			}
		}
		
		~UserControlMouseCoordinates()
		{
			if (timer != null && timer.Enabled) // extra care for stopping the timer
			{
				timer.Stop();
				timer = null;
			}
		}
		
		private void OnClearValues(object sender, RoutedEventArgs e)
		{
			txtX.Text = "";
			txtY.Text = "";
		}
		
		private int left = 0;
		private int top = 0;
		private void OnPickCoordinates(object sender, RoutedEventArgs e)
		{
			if (btnPickCoords.IsChecked == true)
			{
				btnPickCoords.FontWeight = FontWeights.Bold;
				
				// start timer
				if (timer == null)
                {
                    timer = new System.Windows.Forms.Timer();

                    timer.Interval = 500;
                    timer.Tick += timer_Tick;
                }
				
				if (this.element != null && this.element.InnerElement != null)
				{
					tagRECT rect;
					rect.left = rect.top = rect.right = rect.bottom = 0;
					try
					{
						rect = this.element.InnerElement.CurrentBoundingRectangle;
					}
					catch (Exception ex)
					{
						MessageBox.Show("The picked element is not available anymore. Please enter the coordinates manually.");
						btnPickCoords.FontWeight = FontWeights.Normal;
						btnPickCoords.IsChecked = false;
						return;
					}
					
					left = rect.left;
					top = rect.top;
				}
				
				timer.Start();
			}
			else
			{
				btnPickCoords.FontWeight = FontWeights.Normal;
				
				// stop timer
				if (timer != null)
                {
                    timer.Stop();
                }
			}
		}
		
		void timer_Tick(object sender, EventArgs e)
		{
			bool leftCtrlIsPressed = ((int)Keyboard.GetKeyStates(Key.LeftShift) & (int)KeyStates.Down) != 0;
			bool rightCtrlIsPressed = ((int)Keyboard.GetKeyStates(Key.RightShift) & (int)KeyStates.Down) != 0;
			
			if (leftCtrlIsPressed || rightCtrlIsPressed)
			{
				int xMouse = Convert.ToInt32(System.Windows.Forms.Cursor.Position.X);
				int yMouse = Convert.ToInt32(System.Windows.Forms.Cursor.Position.Y);
				
				txtX.Text = (xMouse - left).ToString();
				txtY.Text = (yMouse - top).ToString();
			}
		}
		
		public bool ValidateParams(Action action)
		{
			var window = Window.GetWindow(this);
			
			int x = 0;
			if (int.TryParse(txtX.Text, out x) == false)
			{
				MessageBox.Show(window, "X must be an integer positive value");
				txtX.Focus();
				txtX.SelectAll();
				return false;
			}
			
			if (x < 0)
			{
				MessageBox.Show(window, "X must be an integer positive value");
				txtX.Focus();
				txtX.SelectAll();
				return false;
			}
			
			int y = 0;
			if (int.TryParse(txtY.Text, out y) == false)
			{
				MessageBox.Show(window, "Y must be an integer positive value");
				txtY.Focus();
				txtY.SelectAll();
				return false;
			}
			
			if (y < 0)
			{
				MessageBox.Show(window, "Y must be an integer positive value");
				txtY.Focus();
				txtY.SelectAll();
				return false;
			}
			
			action.Parameters = new List<object>() { x, y };
			return true;
		}
		
		public void Init(List<object> parameters)
		{
			if (parameters == null || parameters.Count < 2)
			{
				return;
			}
			
			txtX.Text = parameters[0].ToString();
			txtY.Text = parameters[1].ToString();
		}
    }
	
	public enum CoordinatesType
	{
		Mouse,
		Move,
		MoveOffset,
		Resize
	}
}
