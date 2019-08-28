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

	//Note: Had to convert everything to a pair instead of a KeyGesture because key gesture does validation on serialization and deserialization.
	//That will cause notsupportedexceptions and break the game. 
	[Serializable]
	public class HotKeyWrapper : NotifierBase, ISerializable
	{
		private const string PRIMARY_KEY = "HotKey_PrimaryKey";
		private const string PRIMARY_MODIFIERS = "HotKey_PrimaryModifiers";
		private const string SECONDARY_KEY = "HotKey_SecondaryKey";
		private const string SECONDARY_MODIFIERS = "HotKey_SecondaryModifiers";

		public Pair<Key,ModifierKeys> primaryGesture
		{
			get => _primaryGesture;
			private set => CheckPropertyChanged(ref _primaryGesture, value);
		}
		private Pair<Key,ModifierKeys> _primaryGesture;

		public Pair<Key,ModifierKeys> secondaryGesture
		{
			get => _secondaryGesture;
			private set => CheckPropertyChanged(ref _secondaryGesture, value);
		}
		private Pair<Key, ModifierKeys> _secondaryGesture;

		public HotKeyWrapper(Key key, ModifierKeys modifier)
		{
			primaryGesture = new Pair<Key, ModifierKeys>(key, modifier);
		}

		public HotKeyWrapper(Key mainKey, ModifierKeys mainModifier, Key altKey, ModifierKeys altModifier)
		{
			primaryGesture = new Pair<Key, ModifierKeys>(mainKey, mainModifier);
			secondaryGesture = new Pair<Key, ModifierKeys>(altKey, altModifier);
		}

		public void UpdateHotKey(Key key, bool isPrimary)
		{
			if (isPrimary)
			{
				primaryGesture = new Pair<Key, ModifierKeys>(key, ModifierKeys.None);
			}
			else
			{
				secondaryGesture = new Pair<Key, ModifierKeys>(key, ModifierKeys.None);
			}
		}

		public void UpdateHotKey(ModifierKeys modifier, bool isPrimary)
		{
			if (isPrimary)
			{
				primaryGesture = new Pair<Key,ModifierKeys>(Key.None, modifier);
			}
			else
			{
				secondaryGesture = new Pair<Key,ModifierKeys>(Key.None, modifier);
			}
		}

		public void UpdateHotKey(Key key, ModifierKeys modifier, bool isPrimary)
		{
			if (isPrimary)
			{
				primaryGesture = new Pair<Key,ModifierKeys>(key, modifier);
			}
			else
			{
				secondaryGesture = new Pair<Key,ModifierKeys>(key, modifier);
			}
		}

		public void UpdateHotKeys(Key mainKey, ModifierKeys mainModifier, Key altKey, ModifierKeys altModifier)
		{
			primaryGesture = new Pair<Key,ModifierKeys>(mainKey, mainModifier);
			secondaryGesture = new Pair<Key,ModifierKeys>(altKey, altModifier);
		}

		protected HotKeyWrapper(SerializationInfo info, StreamingContext context)
		{
			Key primaryKey = (Key)info.GetValue(PRIMARY_KEY, typeof(Key));
			ModifierKeys primaryModifiers = (ModifierKeys)info.GetValue(PRIMARY_MODIFIERS, typeof(ModifierKeys));
			Key secondaryKey = (Key)info.GetValue(SECONDARY_KEY, typeof(Key));
			ModifierKeys secondaryModifiers = (ModifierKeys)info.GetValue(SECONDARY_MODIFIERS, typeof(ModifierKeys));

			_primaryGesture = new Pair<Key, ModifierKeys>(primaryKey, primaryModifiers);
			_secondaryGesture = new Pair<Key, ModifierKeys>(secondaryKey, secondaryModifiers);
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(PRIMARY_KEY, primaryGesture.first, typeof(Key));
			info.AddValue(PRIMARY_MODIFIERS, primaryGesture.second, typeof(ModifierKeys));
			info.AddValue(SECONDARY_KEY, secondaryGesture.first, typeof(Key));
			info.AddValue(SECONDARY_MODIFIERS, secondaryGesture.second, typeof(ModifierKeys));
		}
	}
}
