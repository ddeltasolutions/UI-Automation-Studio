using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using UIDeskAutomationLib;

namespace UIAutomationStudio
{
	public partial class RunAction
	{
		private Action action = null;
	
		public RunAction(Action action)
		{
			this.action = action;
		}
		
		public bool Run(bool noTimeOut = false)
		{
			Element element = action.Element;
			
			if (element == null)
			{
				try
				{
					return CallGeneralAction();
				}
				catch (Exception ex)
				{
					Helper.ShowMessageBoxOnMainThread(ex.Message);
					return false;
				}
			}
			
			ElementBase libraryElement = element.GetLibraryElement(noTimeOut: noTimeOut);
			if (libraryElement == null)
			{
				return false;
			}
			
			try
			{
				return CallAction(libraryElement);
			}
			catch (Exception ex)
			{
				Helper.ShowMessageBoxOnMainThread(ex.Message);
				return false;
			}
		}
		
		private bool CallGeneralAction()
		{
			Engine engine = LibraryEngine.GetEngine();
		
			if (this.action.ActionId == ActionIds.StartProcess)
			{
				if (this.action.Parameters.Count == 1)
				{
					engine.StartProcess(GetExecutableFile(this.action.Parameters));
					return true;
				}
				else if (this.action.Parameters.Count == 2)
				{
					engine.StartProcess(GetExecutableFile(this.action.Parameters), 
						(string)this.action.Parameters[1]);
					return true;
				}
			}
			else if (this.action.ActionId == ActionIds.StartProcessAndWaitForInputIdle)
			{
				if (this.action.Parameters.Count == 1)
				{
					engine.StartProcessAndWaitForInputIdle(GetExecutableFile(this.action.Parameters));
					return true;
				}
				else if (this.action.Parameters.Count == 2)
				{
					engine.StartProcessAndWaitForInputIdle(GetExecutableFile(this.action.Parameters), 
						(string)this.action.Parameters[1]);
					return true;
				}
			}
			else if (this.action.ActionId == ActionIds.Sleep && this.action.Parameters.Count >= 1)
			{
				engine.Sleep((int)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.ShowDesktop)
			{
				engine.ShowDesktop();
				return true;
			}
			else if (this.action.ActionId == ActionIds.ClickAt && this.action.Parameters.Count >= 2)
			{
				engine.ClickScreenCoordinatesAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.RightClickAt && this.action.Parameters.Count >= 2)
			{
				engine.RightClickScreenCoordinatesAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MiddleClickAt && this.action.Parameters.Count >= 2)
			{
				engine.MiddleClickScreenCoordinatesAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.DoubleClickAt && this.action.Parameters.Count >= 2)
			{
				engine.DoubleClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MoveMouse && this.action.Parameters.Count >= 2)
			{
				engine.MoveMouse((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MouseScrollUp && this.action.Parameters.Count >= 1)
			{
				engine.MouseScrollUp((uint)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MouseScrollDown && this.action.Parameters.Count >= 1)
			{
				engine.MouseScrollDown((uint)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SendKeys && this.action.Parameters.Count >= 1)
			{
				engine.SendKeys((string)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeyDown && this.action.Parameters.Count >= 1)
			{
				engine.KeyDown((VirtualKeys)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeyPress && this.action.Parameters.Count >= 1)
			{
				engine.KeyPress((VirtualKeys)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeysPress && this.action.Parameters.Count >= 1)
			{
				List<VirtualKeys> keys = new List<VirtualKeys>();
				foreach (object item in this.action.Parameters)
				{
					keys.Add((VirtualKeys)item);
				}
				
				engine.KeysPress(keys.ToArray());
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeyUp && this.action.Parameters.Count >= 1)
			{
				engine.KeyUp((VirtualKeys)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateSendKeys && this.action.Parameters.Count >= 1)
			{
				engine.SimulateSendKeys((string)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateClickAt && this.action.Parameters.Count >= 2)
			{
				engine.SimulateClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateRightClickAt && this.action.Parameters.Count >= 2)
			{
				engine.SimulateRightClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateMiddleClickAt && this.action.Parameters.Count >= 2)
			{
				engine.SimulateMiddleClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateDoubleClickAt && this.action.Parameters.Count >= 2)
			{
				engine.SimulateDoubleClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			
			return false;
		}
		
		private static string GetExecutableFile(List<object> parameters)
		{
			string exePath = (string)parameters[0];
				
			if (exePath == "MyCalculator.exe" && !File.Exists(exePath))
			{
				exePath = "Samples\\MyCalculator.exe";
			}
			
			return exePath;
		}
		
		private bool CallAction(ElementBase libraryElement)
		{
			IElement element = ElementFactoryMethod(libraryElement);
			if (element != null && element.Run(this.action) == true)
			{
				return true;
			}
			
			if (this.action.ActionId == ActionIds.Click)
			{
				libraryElement.Click();
				return true;
			}
			else if (this.action.ActionId == ActionIds.RightClick)
			{
				libraryElement.RightClick();
				return true;
			}
			else if (this.action.ActionId == ActionIds.MiddleClick)
			{
				libraryElement.MiddleClick();
				return true;
			}
			else if (this.action.ActionId == ActionIds.DoubleClick)
			{
				libraryElement.DoubleClick();
				return true;
			}
			else if (this.action.ActionId == ActionIds.ClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.ClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.RightClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.RightClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MiddleClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.MiddleClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.DoubleClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.DoubleClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MoveMouse && this.action.Parameters.Count >= 2)
			{
				libraryElement.MoveMouse((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MouseScrollUp && this.action.Parameters.Count >= 1)
			{
				libraryElement.MouseScrollUp((uint)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.MouseScrollDown && this.action.Parameters.Count >= 1)
			{
				libraryElement.MouseScrollDown((uint)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.Invoke)
			{
				libraryElement.Invoke();
				return true;
			}
			else if (this.action.ActionId == ActionIds.Focus)
			{
				libraryElement.Focus();
				return true;
			}
			else if (this.action.ActionId == ActionIds.BringToForeground)
			{
				libraryElement.BringToForeground();
				return true;
			}
			else if (this.action.ActionId == ActionIds.SendKeys && this.action.Parameters.Count >= 1)
			{
				libraryElement.SendKeys((string)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeyDown && this.action.Parameters.Count >= 1)
			{
				libraryElement.KeyDown((VirtualKeys)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeyPress && this.action.Parameters.Count >= 1)
			{
				libraryElement.KeyPress((VirtualKeys)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeysPress && this.action.Parameters.Count >= 1)
			{
				List<VirtualKeys> keys = new List<VirtualKeys>();
				foreach (object item in this.action.Parameters)
				{
					keys.Add((VirtualKeys)item);
				}
				
				libraryElement.KeysPress(keys.ToArray());
				return true;
			}
			else if (this.action.ActionId == ActionIds.KeyUp && this.action.Parameters.Count >= 1)
			{
				libraryElement.KeyUp((VirtualKeys)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.PredefinedKeysCombination)
			{
				return SendPredefinedKeysCombination(libraryElement);
			}
			else if (this.action.ActionId == ActionIds.SimulateSendKeys && this.action.Parameters.Count >= 1)
			{
				libraryElement.SimulateSendKeys((string)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateClick)
			{
				libraryElement.SimulateClick();
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateRightClick)
			{
				libraryElement.SimulateRightClick();
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateMiddleClick)
			{
				libraryElement.SimulateMiddleClick();
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateDoubleClick)
			{
				libraryElement.SimulateDoubleClick();
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.SimulateClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateRightClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.SimulateRightClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateMiddleClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.SimulateMiddleClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.SimulateDoubleClickAt && this.action.Parameters.Count >= 2)
			{
				libraryElement.SimulateDoubleClickAt((int)this.action.Parameters[0], (int)this.action.Parameters[1]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.CaptureToFile && this.action.Parameters.Count >= 1)
			{
				libraryElement.CaptureToFile((string)this.action.Parameters[0]);
				return true;
			}
			else if (this.action.ActionId == ActionIds.WaitForInputIdle)
			{
				libraryElement.WaitForInputIdle();
				return true;
			}
			
			return false;
		}
		
		private IElement ElementFactoryMethod(ElementBase libraryElement)
		{
			if (this.action.Element.ControlType == ControlType.Button)
			{
				return new Button(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.Calendar)
			{
				return new Calendar(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.CheckBox)
			{
				return new CheckBox(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.ComboBox)
			{
				return new ComboBox(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.DataGrid)
			{
				return new DataGrid(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.Group)
			{
				return new Group(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.DataItem)
			{
				return new DataItem(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.DatePicker)
			{
				return new DatePicker(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.Document)
			{
				return new Document(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.Edit)
			{
				return new Edit(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.Hyperlink)
			{
				return new Hyperlink(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.List)
			{
				return new List(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.ListItem)
			{
				return new ListItem(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.MenuItem)
			{
				return new MenuItem(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.RadioButton)
			{
				return new RadioButton(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.ScrollBar)
			{
				return new ScrollBar(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.Slider)
			{
				return new Slider(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.Spinner)
			{
				return new Spinner(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.SplitButton)
			{
				return new SplitButton(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.Tab)
			{
				return new Tab(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.TabItem)
			{
				return new TabItem(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.TreeItem)
			{
				return new TreeItem(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.Custom)
			{
				return new Custom(libraryElement);
			}
			else if (this.action.Element.ControlType == ControlType.Window)
			{
				return new Window(libraryElement);
			}
			
			return null;
		}
		
		private bool SendPredefinedKeysCombination(ElementBase libraryElement)
		{
			if (this.action.Parameters.Count <= 0)
			{
				return false;
			}
			
			string keysCombination = this.action.Parameters[0].ToString();
			if (keysCombination.StartsWith("Ctrl"))
			{
				libraryElement.KeyDown(VirtualKeys.Control);
			}
			else if (keysCombination.StartsWith("Alt"))
			{
				libraryElement.KeyDown(VirtualKeys.Alt);
			}
				
			if (keysCombination == "Ctrl+C")
			{
				libraryElement.KeyPress(VirtualKeys.C);
			}
			else if (keysCombination == "Ctrl+V")
			{
				libraryElement.KeyPress(VirtualKeys.V);
			}
			else if (keysCombination == "Ctrl+X")
			{
				libraryElement.KeyPress(VirtualKeys.X);
			}
			else if (keysCombination == "Ctrl+Z")
			{
				libraryElement.KeyPress(VirtualKeys.Z);
			}
			else if (keysCombination == "Ctrl+Y")
			{
				libraryElement.KeyPress(VirtualKeys.Y);
			}
			else if (keysCombination == "Ctrl+A")
			{
				libraryElement.KeyPress(VirtualKeys.A);
			}
			else if (keysCombination == "Ctrl+S")
			{
				libraryElement.KeyPress(VirtualKeys.S);
			}
			else if (keysCombination == "Ctrl+F")
			{
				libraryElement.KeyPress(VirtualKeys.F);
			}
			else if (keysCombination == "Ctrl+N")
			{
				libraryElement.KeyPress(VirtualKeys.N);
			}
			else if (keysCombination == "Ctrl+O")
			{
				libraryElement.KeyPress(VirtualKeys.O);
			}
			else if (keysCombination == "Ctrl+P")
			{
				libraryElement.KeyPress(VirtualKeys.P);
			}
			else if (keysCombination == "Alt+F4")
			{
				libraryElement.KeyPress(VirtualKeys.F4);
			}
				
			if (keysCombination.StartsWith("Ctrl"))
			{
				libraryElement.KeyUp(VirtualKeys.Control);
			}
			else if (keysCombination.StartsWith("Alt"))
			{
				libraryElement.KeyUp(VirtualKeys.Alt);
			}
			return true;
		}
	}
}