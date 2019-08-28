using CoC.Backend.Tools;
using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoCWinDesktop.Views
{
	/// <summary>
	/// Interaction logic for StandardView.xaml
	/// </summary>
	/// 
	public partial class StandardView : UserControl
	{
		//public static RoutedCommand TestCommand = new RoutedCommand();

		//private void ExecuteWrappedCommand(object sender, ExecutedRoutedEventArgs e)
		//{
		//	this.TestButton.Command.Execute(sender);
		//}

		//private void CanExecuteCustomCommand(object sender, CanExecuteRoutedEventArgs e)
		//{
		//	if (e.OriginalSource is TextBoxBase)
		//	{
		//		e.CanExecute = false;
		//	}
		//	else
		//	{
		//		e.CanExecute = this.TestButton.Command.CanExecute(sender);
		//	}
		//}
		private readonly ModelViewRunner runner;

		//workaround for input bindings and textboxes. Note that it's not very flexible - honestly, it's a hack. but it's the cleanest method i could find
		//basically, we store the keybindings, and remove them when the textbox in out content section has focus. we add them when it doesn't.
		//This allows the WPF engine to do all its shit naturally - no need to manually handle events and preview events and all that shit. Probably could bind them
		//as well, so we don't need to deal with updating them when the users update their hotkeys. Will need a way to capture the modifiers and shit they hit.
		private readonly List<KeyBinding> hotKeys;

		private readonly Button[] bottomButtons;

		public StandardView()
		{
			InitializeComponent();
			runner = Application.Current.Resources["Runner"] as ModelViewRunner;

			bottomButtons = new Button[15]
			{
				BottomButton0, BottomButton1, BottomButton2, BottomButton3, BottomButton4,
				BottomButton5, BottomButton6, BottomButton7, BottomButton8, BottomButton9,
				BottomButton10, BottomButton11, BottomButton12, BottomButton13, BottomButton14
			};

			Binding key = new Binding()
			{
				Source = runner.Button1Hotkey,
				Path = new PropertyPath(nameof(runner.Button1Hotkey.primaryGesture) + "." + nameof(runner.Button1Hotkey.primaryGesture.first)),
				Mode = BindingMode.OneWay,
			};

			Binding modifier = new Binding()
			{
				Source = runner.Button1Hotkey,
				Path = new PropertyPath(nameof(runner.Button1Hotkey.primaryGesture) + "." + nameof(runner.Button1Hotkey.primaryGesture.second)),
				Mode = BindingMode.OneWay,
			};

			KeyBinding keyBinding = new KeyBinding()
			{
				Command = new RelayCommand(() => bottomButtons[0].Command?.Execute(this), () => bottomButtons[0].Command?.CanExecute(this) == true)
			};
			BindingOperations.SetBinding(keyBinding, KeyBinding.KeyProperty, key);
			BindingOperations.SetBinding(keyBinding, KeyBinding.ModifiersProperty, modifier);

			//RelayCommand commandMaker(ICommand cmd) => new RelayCommand(() => cmd?.Execute(this), () => cmd?.CanExecute(this) == true);
			//Pair<KeyBinding> bindingMaker(HotKeyWrapper hotKey)
			//{
			//	KeyBinding first, second;
			//	first = new KeyBinding() { Key = hotKey.primaryGesture.Key, Modifiers = ModifierKeys.}
			//	new Pair<KeyBinding>(new hotKey.primaryGesture)
			//}

			hotKeys = new List<KeyBinding>()
			{
				keyBinding,
			};

			MainContent.InputField.IsKeyboardFocusedChanged += InputField_IsKeyboardFocusedChanged;

			//InputBindings.AddRange(hotKeys);
		}

		private void InputField_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
			{
				if ((bool)e.NewValue)
				{
					InputBindings.Clear();
				}
				else 
				{
					if (InputBindings.Count > 0)
					{
						InputBindings.Clear();
					}
					InputBindings.AddRange(hotKeys);
				}
			}
		}

		//private void StandardView_KeyDown(object sender, KeyEventArgs e)
		//{
		//	//Only do the following if we're not in a TextBox/RichTextBox (or, possibly, Combobox).

		//	//Store a lookup table that maps KeyGesture(Key+Modifier) to Hotkey Enum in ModelViewRunner. update it from options.
		//	//loop through all KeyGestures found in keys. if one matches current modifiers and key, return the Enum stored in value. else, return NONE. 
		//	//Switch the result, calling the corresponding button's command, or the quicksave/quickload command, which is not really a button but whatever.
		//	//This is the highest level we can get to in this control, so fuck all, we're fine handling it here. if none found, simply let it fall out to whatever.

		//	if (!(e.OriginalSource is TextBoxBase))
		//	{
		//		if (e.Key == Key.D1 && Keyboard.Modifiers == ModifierKeys.None)
		//		{
		//			if (TestButton.Command.CanExecute(sender)) TestButton.Command.Execute(sender);
		//			e.Handled = true;
		//		}
		//		else if ()
		//	}
		//}

		//protected override void OnKeyDown(KeyEventArgs e)
		//{
		//	if (e.InputSource!= null)
		//	base.OnKeyDown(e);
		//}

	}
}
