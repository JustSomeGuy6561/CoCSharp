using CoC.Backend;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
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

	//Note: Had to convert everything to a HotKey instead of a KeyGesture because key gesture does validation on serialization and deserialization.
	//That will cause notsupportedexceptions and break the game. 
	public class HotKeyWrapper : NotifierBase
	{
		private readonly SimpleDescriptor hotkeyDefinition;

		public string HotKeyText => hotkeyDefinition();

		public HotKey primaryGesture
		{
			get => _primaryGesture;
			set => CheckPropertyChanged(ref _primaryGesture, value);
		}
		private HotKey _primaryGesture;

		public HotKey secondaryGesture
		{
			get => _secondaryGesture;
			set => CheckPropertyChanged(ref _secondaryGesture, value);
		}
		private HotKey _secondaryGesture;

		public HotKeyWrapper(SimpleDescriptor associatedAction)
		{
			hotkeyDefinition = associatedAction ?? throw new ArgumentNullException(nameof(associatedAction));
		}

		public HotKeyWrapper(SimpleDescriptor associatedAction, Key key, ModifierKeys modifier)
		{
			primaryGesture = new HotKey(key, modifier);

			hotkeyDefinition = associatedAction ?? throw new ArgumentNullException(nameof(associatedAction));
		}

		public HotKeyWrapper(SimpleDescriptor associatedAction, Key mainKey, ModifierKeys mainModifier, Key altKey, ModifierKeys altModifier)
		{
			primaryGesture = new HotKey(mainKey, mainModifier);
			secondaryGesture = new HotKey(altKey, altModifier);

			hotkeyDefinition = associatedAction ?? throw new ArgumentNullException(nameof(associatedAction));
		}

		public void UpdateHotKey(Key key, bool isPrimary)
		{
			if (isPrimary)
			{
				primaryGesture = new HotKey(key, ModifierKeys.None);
			}
			else
			{
				secondaryGesture = new HotKey(key, ModifierKeys.None);
			}
		}

		public void UpdateHotKey(ModifierKeys modifier, bool isPrimary)
		{
			if (isPrimary)
			{
				primaryGesture = new HotKey(Key.None, modifier);
			}
			else
			{
				secondaryGesture = new HotKey(Key.None, modifier);
			}
		}

		public void UpdateHotKey(Key key, ModifierKeys modifier, bool isPrimary)
		{
			if (isPrimary)
			{
				primaryGesture = new HotKey(key, modifier);
			}
			else
			{
				secondaryGesture = new HotKey(key, modifier);
			}
		}

		public void UpdateHotKeys(Key mainKey, ModifierKeys mainModifier, Key altKey, ModifierKeys altModifier)
		{
			primaryGesture = new HotKey(mainKey, mainModifier);
			secondaryGesture = new HotKey(altKey, altModifier);
		}
	}
}
