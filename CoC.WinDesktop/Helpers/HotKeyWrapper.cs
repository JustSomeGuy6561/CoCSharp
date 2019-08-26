using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace CoCWinDesktop.Helpers
{
	//public enum Hotkey
	//{
	//	MAIN_MENU, DATA, STATS, PERKS, LEVELING, APPEARANCE,
	//	BUTTON_1, BUTTON_2, BUTTON_3, BUTTON_4, BUTTON_5,
	//	BUTTON_6, BUTTON_7, BUTTON_8, BUTTON_9, BUTTON_10,
	//	BUTTON_11, BUTTON_12, BUTTON_13, BUTTON_14, BUTTON_15,
	//	QUICKSAVE, QUICKLOAD,
	//	TOGGLE_BACKGROUND,
	//}
	public class HotKeyWrapper : NotifierBase
	{
		//public readonly Hotkey hotkey;
		private static readonly KeyBinding gestureMaker = new KeyBinding();

		public KeyGesture primaryGesture
		{
			get => _primaryGesture;
			private set => CheckPropertyChanged(ref _primaryGesture, value);
		}
		private KeyGesture _primaryGesture;

		public KeyGesture secondaryGesture
		{
			get => _secondaryGesture;
			private set => CheckPropertyChanged(ref _secondaryGesture, value);
		}
		private KeyGesture _secondaryGesture;

		public HotKeyWrapper(Key key, ModifierKeys modifier)
		{
			primaryGesture = GetGesture(key, modifier);
		}

		public HotKeyWrapper(Key mainKey, ModifierKeys mainModifier, Key altKey, ModifierKeys altModifier)
		{
			primaryGesture = GetGesture(mainKey, mainModifier);
			secondaryGesture = GetGesture(altKey, altModifier);
		}

		public void UpdateHotKey(Key key, bool isPrimary)
		{
			if (isPrimary)
			{
				primaryGesture = GetGesture(key, ModifierKeys.None);
			}
			else
			{
				secondaryGesture = GetGesture(key, ModifierKeys.None);
			}
		}

		public void UpdateHotKey(ModifierKeys modifier, bool isPrimary)
		{
			if (isPrimary)
			{
				primaryGesture = GetGesture(Key.None, modifier);
			}
			else
			{
				secondaryGesture = GetGesture(Key.None, modifier);
			}
		}

		public void UpdateHotKey(Key key, ModifierKeys modifier, bool isPrimary)
		{
			if (isPrimary)
			{
				primaryGesture = GetGesture(key, modifier);
			}
			else
			{
				secondaryGesture = GetGesture(key, modifier);
			}
		}

		public void UpdateHotKeys(Key mainKey, ModifierKeys mainModifier, Key altKey, ModifierKeys altModifier)
		{
			primaryGesture = GetGesture(mainKey, mainModifier);
			secondaryGesture = GetGesture(altKey, altModifier);
		}

		private static KeyGesture GetGesture(Key key, ModifierKeys modifier)
		{
			KeyGesture retVal = null;
			lock (gestureMaker)
			{
				gestureMaker.Key = key;
				gestureMaker.Modifiers = modifier;
				retVal = gestureMaker.Gesture as KeyGesture;
			}
			return retVal;
		}
	}
}
