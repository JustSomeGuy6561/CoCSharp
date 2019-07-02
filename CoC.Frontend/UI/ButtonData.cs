//ButtonData.cs
//Description:
//Author: JustSomeGuy
//6/10/2019, 7:52 PM
using System;
using System.Collections.ObjectModel;

namespace CoC.Frontend.UI
{
	public sealed class ButtonData
	{
		public const byte MAX_BUTTON_TEXT_LEN = 12;
		public const byte MAX_BUTTONS = 15;
		public readonly string title;
		public readonly string tooltipTitle;
		public readonly string tooltip;
		public readonly bool enabled;

		public readonly Action onClick;

		private ButtonData(string desc, bool active, Action callback)
		{
			title = desc.Trim().Substring(0, MAX_BUTTON_TEXT_LEN);
			tooltip = "";
			tooltipTitle = "";
			enabled = active;
			onClick = callback;
		}

		private ButtonData(string desc, bool active, Action callback, string tip, string replacementTitle)
		{
			title = desc.Trim().Substring(0, MAX_BUTTON_TEXT_LEN);
			tooltip = tip.Trim();
			tooltipTitle = string.IsNullOrWhiteSpace(tip) ? "" : string.IsNullOrWhiteSpace(replacementTitle) ? title : replacementTitle;
			enabled = active;
			onClick = callback;
		}

		internal static bool AddButton(byte index, string title, Action callback)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();

			if (buttonHelper[index] != null) return false;

			buttonHelper[index] = new ButtonData(title, true, callback);
			return true;
		}

		internal static bool AddButtonWithToolTip(byte index, string title, Action callback, string tip, string replacementTitle = "")
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();

			if (buttonHelper[index] != null) return false;

			buttonHelper[index] = new ButtonData(title, true, callback, tip, replacementTitle);
			return true;
		}


		internal static bool AddButtonDisabled(byte index, string title)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();

			if (buttonHelper[index] != null) return false;

			buttonHelper[index] = new ButtonData(title, false, Nothing);
			return true;

		}

		internal static bool AddButtonDisabledWithToolTip(byte index, string title, string tip, string replacementTitle = "")
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();

			if (buttonHelper[index] != null) return false;

			buttonHelper[index] = new ButtonData(title, false, Nothing, tip, replacementTitle);
			return true;
		}

		internal static bool AddButtonIf(byte index, string title, bool condition, Action enabledCallback)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();
			if (buttonHelper[index] != null) return false;

			Action callback = condition ? enabledCallback : Nothing;

			buttonHelper[index] = new ButtonData(title, condition, callback);
			return true;
		}


		internal static bool AddButtonIfWithToolTip(byte index, string title, bool condition, Action enabledCallback, string enabledTip, string disabledTip, string replacementTitle = "")
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
			buttonHelper[index] = new ButtonData(title, condition, callback, tip, replacementTitle);
			return true;
		}

		internal static bool AddButtonOrAddDisabledWithToolTip(byte index, string title, bool condition, Action enabledCallback, string disabledTip, string replacementTitle = "")
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
			buttonHelper[index] = new ButtonData(title, condition, callback, tip, replacementTitle);
			return true;
		}

		internal static void ClearButtons()
		{
			Array.Clear(buttonHelper, 0, buttonHelper.Length);
		}

		private static readonly ButtonData[] buttonHelper = new ButtonData[MAX_BUTTONS];

		internal static ReadOnlyCollection<ButtonData> buttons => Array.AsReadOnly(buttonHelper);

		private static readonly Action Nothing = () => { };
	}
}
