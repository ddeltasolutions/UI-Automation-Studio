using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Controls;
using UIDeskAutomationLib;

namespace UIAutomationStudio
{
    /// <summary>
    /// Interaction logic for PickKeyWindow.xaml
    /// </summary>
    public partial class PickKeyWindow : Window
    {
		public VirtualKeys SelectedKey { get; set; }
	
        public PickKeyWindow()
        {
            InitializeComponent();
			
			btnEsc.Tag = VirtualKeys.Escape;
			btnF1.Tag = VirtualKeys.F1;
			btnF2.Tag = VirtualKeys.F2;
			btnF3.Tag = VirtualKeys.F3;
			btnF4.Tag = VirtualKeys.F4;
			btnF5.Tag = VirtualKeys.F5;
			btnF6.Tag = VirtualKeys.F6;
			btnF7.Tag = VirtualKeys.F7;
			btnF8.Tag = VirtualKeys.F8;
			btnF9.Tag = VirtualKeys.F9;
			btnF10.Tag = VirtualKeys.F10;
			btnF11.Tag = VirtualKeys.F11;
			btnF12.Tag = VirtualKeys.F12;
			
			btnTilde.Tag = VirtualKeys.Tilde;
			btn1.Tag = VirtualKeys.N1;
			btn2.Tag = VirtualKeys.N2;
			btn3.Tag = VirtualKeys.N3;
			btn4.Tag = VirtualKeys.N4;
			btn5.Tag = VirtualKeys.N5;
			btn6.Tag = VirtualKeys.N6;
			btn7.Tag = VirtualKeys.N7;
			btn8.Tag = VirtualKeys.N8;
			btn9.Tag = VirtualKeys.N9;
			btn0.Tag = VirtualKeys.N0;
			btnMinus.Tag = VirtualKeys.Minus;
			btnEquals.Tag = VirtualKeys.Plus;
			btnBack.Tag = VirtualKeys.Back;
			
			btnTab.Tag = VirtualKeys.Tab;
			btnQ.Tag = VirtualKeys.Q;
			btnW.Tag = VirtualKeys.W;
			btnE.Tag = VirtualKeys.E;
			btnR.Tag = VirtualKeys.R;
			btnT.Tag = VirtualKeys.T;
			btnY.Tag = VirtualKeys.Y;
			btnU.Tag = VirtualKeys.U;
			btnI.Tag = VirtualKeys.I;
			btnO.Tag = VirtualKeys.O;
			btnP.Tag = VirtualKeys.P;
			btnSquareP1.Tag = VirtualKeys.OpenSquareBracket;
			btnSquareP2.Tag = VirtualKeys.CloseSquareBracket;
			
			btnCaps.Tag = VirtualKeys.CapsLock;
			btnA.Tag = VirtualKeys.A;
			btnS.Tag = VirtualKeys.S;
			btnD.Tag = VirtualKeys.D;
			btnF.Tag = VirtualKeys.F;
			btnG.Tag = VirtualKeys.G;
			btnH.Tag = VirtualKeys.H;
			btnJ.Tag = VirtualKeys.J;
			btnK.Tag = VirtualKeys.K;
			btnL.Tag = VirtualKeys.L;
			btnSemicolon.Tag = VirtualKeys.Semicolon;
			btnApostrophe.Tag = VirtualKeys.SingleQuote;
			btnBackslash.Tag = VirtualKeys.BackSlash;
			
			btnLeftShift.Tag = VirtualKeys.LeftShift;
			btnBackslash2.Tag = VirtualKeys.BackSlash;
			btnZ.Tag = VirtualKeys.Z;
			btnX.Tag = VirtualKeys.X;
			btnC.Tag = VirtualKeys.C;
			btnV.Tag = VirtualKeys.V;
			btnB.Tag = VirtualKeys.B;
			btnN.Tag = VirtualKeys.N;
			btnM.Tag = VirtualKeys.M;
			btnComa.Tag = VirtualKeys.Comma;
			btnDot.Tag = VirtualKeys.Period;
			btnSlash.Tag = VirtualKeys.ForwardSlash;
			btnRightShift.Tag = VirtualKeys.RightShift;
			
			btnLeftCtrl.Tag = VirtualKeys.LeftControl;
			btnWin.Tag = VirtualKeys.LeftWindows;
			btnLeftAlt.Tag = VirtualKeys.LeftAlt;
			btnSpace.Tag = VirtualKeys.Space;
			btnRightAlt.Tag = VirtualKeys.RightAlt;
			btnFn.Tag = null; //0xFF;
			btnRClick.Tag = VirtualKeys.RightWindows;
			btnRightCtrl.Tag = VirtualKeys.RightControl;
			
			btnPrintScreen.Tag = VirtualKeys.PrintScreen;
			btnScrollLock.Tag = VirtualKeys.ScrollLock;
			btnPause.Tag = VirtualKeys.Pause;
			
			btnInsert.Tag = VirtualKeys.Insert;
			btnHome.Tag = VirtualKeys.Home;
			btnPageUp.Tag = VirtualKeys.PageUp;
			btnDelete.Tag = VirtualKeys.Delete;
			btnEnd.Tag = VirtualKeys.End;
			btnPageDown.Tag = VirtualKeys.PageDown;
			btnUp.Tag = VirtualKeys.Up;
			btnLeft.Tag = VirtualKeys.Left;
			btnDown.Tag = VirtualKeys.Down;
			btnRight.Tag = VirtualKeys.Right;
			
			btnNumLock.Tag = VirtualKeys.NumLock;
			btnDivide.Tag = VirtualKeys.Divide;
			btnMultiply.Tag = VirtualKeys.Multiply;
			btnNumMinus.Tag = VirtualKeys.Subtract;
			btnNum7.Tag = VirtualKeys.Numpad7;
			btnNum8.Tag = VirtualKeys.Numpad8;
			btnNum9.Tag = VirtualKeys.Numpad9;
			btnPlus.Tag = VirtualKeys.Add;
			btnNum4.Tag = VirtualKeys.Numpad4;
			btnNum5.Tag = VirtualKeys.Numpad5;
			btnNum6.Tag = VirtualKeys.Numpad6;
			btnNum1.Tag = VirtualKeys.Numpad1;
			btnNum2.Tag = VirtualKeys.Numpad2;
			btnNum3.Tag = VirtualKeys.Numpad3;
			btnNumEnter.Tag = VirtualKeys.Enter;
			btnNum0.Tag = VirtualKeys.Numpad0;
			btnNumDot.Tag = VirtualKeys.Decimal;
			
			btnEnter.Tag = VirtualKeys.Enter;
		}
		
		private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }
		
		private void OnClick(object sender, RoutedEventArgs e)
		{
			Button btn = sender as Button;
			if (btn == null || btn.Tag == null)
			{
				return;
			}
			
			VirtualKeys vk = (VirtualKeys)btn.Tag;
			this.SelectedKey = vk;
			
			this.DialogResult = true;
			this.Close();
		}
	}
}