using CoCWinDesktop.ContentWrappers;
using CoCWinDesktop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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

		//private readonly Button[] bottomButtons;
		private readonly Dictionary<Button, HotKeyWrapper> buttonHotKeyWrapper;

		public StandardView()
		{
			InitializeComponent();
			runner = Application.Current.Resources["Runner"] as ModelViewRunner;

			Dictionary<Button, HotKeyWrapper> bottomButtons = new Dictionary<Button, HotKeyWrapper>()
			{
				[BottomButton0] = runner.Button1Hotkey,
				[BottomButton1] = runner.Button2Hotkey,
				[BottomButton2] = runner.Button3Hotkey,
				[BottomButton3] = runner.Button4Hotkey,
				[BottomButton4] = runner.Button5Hotkey,
				[BottomButton5] = runner.Button6Hotkey,
				[BottomButton6] = runner.Button7Hotkey,
				[BottomButton7] = runner.Button8Hotkey,
				[BottomButton8] = runner.Button9Hotkey,
				[BottomButton9] = runner.Button10Hotkey,
				[BottomButton10] = runner.Button11Hotkey,
				[BottomButton11] = runner.Button12Hotkey,
				[BottomButton12] = runner.Button13Hotkey,
				[BottomButton13] = runner.Button14Hotkey,
				[BottomButton14] = runner.Button15Hotkey,

			};


			hotKeys = new List<KeyBinding>()
			{
				
			};

			foreach (var pair in bottomButtons)
			{
				Binding keyBind = new Binding()
				{
					Source = pair.Value,
					Path = new PropertyPath(nameof(pair.Value.primaryGesture) + "." + nameof(pair.Value.primaryGesture.key)),
					Mode = BindingMode.OneWay,
					FallbackValue = Key.None,
				};

				Binding modifierBind = new Binding()
				{
					Source = pair.Value,
					Path = new PropertyPath(nameof(pair.Value.primaryGesture) + "." + nameof(pair.Value.primaryGesture.modifier)),
					Mode = BindingMode.OneWay,
					FallbackValue = ModifierKeys.None,
				};

				KeyBinding keyBinding = new KeyBinding()
				{
					Command = new RelayCommand(() => pair.Key.Command?.Execute(this), () => pair.Key.Command?.CanExecute(this) == true)
				};
				BindingOperations.SetBinding(keyBinding, KeyBinding.KeyProperty, keyBind);
				BindingOperations.SetBinding(keyBinding, KeyBinding.ModifiersProperty, modifierBind);

				hotKeys.Add(keyBinding);

				keyBind = new Binding()
				{
					Source = pair.Value,
					Path = new PropertyPath(nameof(pair.Value.secondaryGesture) + "." + nameof(pair.Value.secondaryGesture.key)),
					Mode = BindingMode.OneWay,
					FallbackValue = Key.None,
				};

				modifierBind = new Binding()
				{
					Source = pair.Value,
					Path = new PropertyPath(nameof(pair.Value.secondaryGesture) + "." + nameof(pair.Value.secondaryGesture.modifier)),
					Mode = BindingMode.OneWay,
					FallbackValue= ModifierKeys.None,
				};

				keyBinding = new KeyBinding()
				{
					Command = new RelayCommand(() => pair.Key.Command?.Execute(this), () => pair.Key.Command?.CanExecute(this) == true)
				};
				//keyBinding = new KeyBinding()
				//{
				//	Command = new RelayCommand(TestCommand, () => true)
				//};
				BindingOperations.SetBinding(keyBinding, KeyBinding.KeyProperty, keyBind);
				BindingOperations.SetBinding(keyBinding, KeyBinding.ModifiersProperty, modifierBind);

				hotKeys.Add(keyBinding);
			}



				//RelayCommand commandMaker(ICommand cmd) => new RelayCommand(() => cmd?.Execute(this), () => cmd?.CanExecute(this) == true);
				//Pair<KeyBinding> bindingMaker(HotKeyWrapper hotKey)
				//{
				//	KeyBinding first, second;
				//	first = new KeyBinding() { Key = hotKey.primaryGesture.Key, Modifiers = ModifierKeys.}
				//	new Pair<KeyBinding>(new hotKey.primaryGesture)
				//}



				MainContent.InputField.IsKeyboardFocusedChanged += InputField_IsKeyboardFocusedChanged;

			InputBindings.AddRange(hotKeys);
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
	}
}
