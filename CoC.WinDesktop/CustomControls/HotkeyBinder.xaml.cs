using CoC.WinDesktop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoC.WinDesktop.CustomControls
{
	//Largely lifted from
	//https://tyrrrz.me/Blog/Hotkey-editor-control-in-WPF
	//I will probably have to modify it, but still, credit where it's due.

	/// <summary>
	/// Interaction logic for HotkeyBinder.xaml
	/// </summary>
	public partial class HotkeyBinder : UserControl
	{
		public static readonly DependencyProperty HotKeySourceProperty = DependencyProperty.Register("HotKeySource", typeof(HotKey), typeof(HotkeyBinder),
			new FrameworkPropertyMetadata(default(HotKey), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public HotKey HotKeySource
		{
			get => (HotKey)GetValue(HotKeySourceProperty);
			set => SetCurrentValue(HotKeySourceProperty, value);
		}

		public HotkeyBinder()
		{
			InitializeComponent();
		}

		private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			// Get modifiers and key data
			var modifiers = Keyboard.Modifiers;
			var key = e.Key;

			// When Alt is pressed, SystemKey is used instead
			if (key == Key.System)
			{
				key = e.SystemKey;
			}

			// Normally, don't let the event pass further
			// because we don't want standard textbox shortcuts working,
			//but we have a special case for 'Tab' because it's used for navigation (Tab/Shift-Tab) and Alt+Tab or Ctrl-Tab
			if (key == Key.Tab)
			{
				return;
			}
			else
			{

				e.Handled = true;
			}

			// Pressing delete, backspace or escape without modifiers clears the current value
			if (modifiers == ModifierKeys.None && new List<Key>(){ Key.Delete, Key.Back, Key.Escape}.Contains(key))
			{
				HotKeySource = null;
				return;
			}

			// If no actual key was pressed - return
			if (new List<Key>() { Key.LeftCtrl, Key.RightCtrl, Key.LeftAlt, Key.RightAlt, Key.LeftShift, Key.RightShift, Key.LWin, Key.RWin,
				Key.Clear, Key.OemClear, Key.Apps, Key.Tab}.Contains(key))
			{
				return;
			}


			// Set values
			HotKeySource = new HotKey(key, modifiers);
		}
	}
}
