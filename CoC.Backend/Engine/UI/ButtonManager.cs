using CoC.Backend.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.Engine
{
	public static class ButtonManager
	{
		public const byte MAX_BUTTONS = 15;

		private static readonly ButtonData[] buttonHelper = new ButtonData[MAX_BUTTONS];

		private static readonly Action Nothing = () => { };

		private static bool buttonsChanged = false;

		private static ReadOnlyCollection<ButtonData> buttons { get; }

		static ButtonManager()
		{
			buttons = Array.AsReadOnly(buttonHelper);
		}

		public static bool AddButton(byte index, string title, Action callback)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();

			if (buttonHelper[index] != null) return false;

			buttonsChanged = true;

			buttonHelper[index] = new ButtonData(title, true, callback);
			return true;
		}

		public static bool AddButtonWithToolTip(byte index, string title, Action callback, string tip, string replacementTitle = null)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();

			if (buttonHelper[index] != null) return false;

			buttonsChanged = true;

			buttonHelper[index] = new ButtonData(title, true, callback, tip, replacementTitle);
			return true;
		}


		public static bool AddButtonDisabled(byte index, string title)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();

			if (buttonHelper[index] != null) return false;

			buttonsChanged = true;

			buttonHelper[index] = new ButtonData(title, false, Nothing);
			return true;

		}

		public static bool AddButtonDisabledWithToolTip(byte index, string title, string tip, string replacementTitle = null)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();

			if (buttonHelper[index] != null) return false;

			buttonsChanged = true;

			buttonHelper[index] = new ButtonData(title, false, Nothing, tip, replacementTitle);
			return true;
		}

		public static bool AddButtonIf(byte index, string title, bool condition, Action enabledCallback)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();
			if (buttonHelper[index] != null) return false;

			Action callback = condition ? enabledCallback : Nothing;

			buttonsChanged = true;

			buttonHelper[index] = new ButtonData(title, condition, callback);
			return true;
		}

		public static bool QueryButtons(out ReadOnlyCollection<ButtonData> buttonCollection)
		{
			bool retVal = buttonsChanged;

			buttonsChanged = false;
			buttonCollection = buttons;
			return retVal;
		}

		public static bool AddButtonIfWithToolTip(byte index, string title, bool condition, Action enabledCallback, string enabledTip, string disabledTip, string replacementTitle = null)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();
			if (buttonHelper[index] != null) return false;

			Action callback;
			string tip;
			if (condition)
			{
				callback = enabledCallback;
				tip = enabledTip;
			}
			else
			{
				callback = Nothing;
				tip = disabledTip;
			}

			buttonsChanged = true;


			buttonHelper[index] = new ButtonData(title, condition, callback, tip, replacementTitle);
			return true;
		}

		public static bool AddButtonOrAddDisabledWithToolTip(byte index, string title, bool condition, Action enabledCallback, string disabledTip, string replacementTitle = null)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();
			if (buttonHelper[index] != null) return false;

			Action callback;
			string tip;
			if (condition)
			{
				callback = enabledCallback;
				tip = "";
				replacementTitle = "";
			}
			else
			{
				callback = Nothing;
				tip = disabledTip;
			}

			buttonsChanged = true;

			buttonHelper[index] = new ButtonData(title, condition, callback, tip, replacementTitle);
			return true;
		}

		public static void ClearButtons()
		{
			Array.Clear(buttonHelper, 0, buttonHelper.Length);
			buttonsChanged = true;
		}
	}

	public static class MenuHelpers
	{
		public static bool ButtonPreviousPage(Action callback, byte location = 10)
		{
			return ButtonManager.AddButton(location, PREV_PAGE(), callback);
		}
		public static bool ButtonNextPage(Action callback, byte location = 11)
		{
			return ButtonManager.AddButton(location, NEXT_PAGE(), callback);
		}
		public static void DoNext(Action callback)
		{
			ButtonManager.ClearButtons();
			ButtonManager.AddButton(0, GlobalStrings.NEXT(), callback);
		}

		public static void DoYesNo(Action yesCallback, Action noCallback)
		{
			ButtonManager.ClearButtons();
			ButtonManager.AddButton(0, GlobalStrings.YES(), yesCallback);
			ButtonManager.AddButton(1, GlobalStrings.NO(), noCallback);
		}


		private static string NEXT_PAGE()
		{
			return "Next Page";
		}

		private static string PREV_PAGE()
		{
			return "Prev Page";
		}
	}
}
