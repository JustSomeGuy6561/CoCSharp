using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CoC.Backend;
using CoC.Backend.Strings;
using CoC.Frontend.UI.ControllerData;

namespace CoC.Frontend.UI
{
	internal static class ButtonManager
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

		internal static bool AddButton(byte index, SimpleDescriptor title, Action callback)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();

			if (buttonHelper[index] != null) return false;

			buttonsChanged = true;

			buttonHelper[index] = new ButtonData(title, true, callback);
			return true;
		}

		internal static bool AddButtonWithToolTip(byte index, SimpleDescriptor title, Action callback, SimpleDescriptor tip, SimpleDescriptor replacementTitle = null)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();

			if (buttonHelper[index] != null) return false;

			buttonsChanged = true;

			buttonHelper[index] = new ButtonData(title, true, callback, tip, replacementTitle);
			return true;
		}


		internal static bool AddButtonDisabled(byte index, SimpleDescriptor title)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();

			if (buttonHelper[index] != null) return false;

			buttonsChanged = true;

			buttonHelper[index] = new ButtonData(title, false, Nothing);
			return true;

		}

		internal static bool AddButtonDisabledWithToolTip(byte index, SimpleDescriptor title, SimpleDescriptor tip, SimpleDescriptor replacementTitle = null)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();

			if (buttonHelper[index] != null) return false;

			buttonsChanged = true;

			buttonHelper[index] = new ButtonData(title, false, Nothing, tip, replacementTitle);
			return true;
		}

		internal static bool AddButtonIf(byte index, SimpleDescriptor title, bool condition, Action enabledCallback)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();
			if (buttonHelper[index] != null) return false;

			Action callback = condition ? enabledCallback : Nothing;

			buttonsChanged = true;

			buttonHelper[index] = new ButtonData(title, condition, callback);
			return true;
		}

		internal static bool QueryButtons(out ReadOnlyCollection<ButtonData> buttonCollection)
		{
			bool retVal = buttonsChanged;

			buttonsChanged = false;
			buttonCollection = buttons;
			return retVal;
		}

		internal static bool AddButtonIfWithToolTip(byte index, SimpleDescriptor title, bool condition, Action enabledCallback, SimpleDescriptor enabledTip, SimpleDescriptor disabledTip, SimpleDescriptor replacementTitle = null)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();
			if (buttonHelper[index] != null) return false;

			Action callback;
			SimpleDescriptor tip;
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

		internal static bool AddButtonOrAddDisabledWithToolTip(byte index, SimpleDescriptor title, bool condition, Action enabledCallback, SimpleDescriptor disabledTip, SimpleDescriptor replacementTitle = null)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();
			if (buttonHelper[index] != null) return false;

			Action callback;
			SimpleDescriptor tip;
			if (condition)
			{
				callback = enabledCallback;
				tip = GlobalStrings.None;
				replacementTitle = GlobalStrings.None;
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

		internal static void ClearButtons()
		{
			Array.Clear(buttonHelper, 0, buttonHelper.Length);
			buttonsChanged = true;
		}
	}
}
