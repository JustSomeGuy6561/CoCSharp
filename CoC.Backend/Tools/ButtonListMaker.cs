using CoC.Backend.Engine;
using CoC.Backend.Strings;
using System;
using System.Collections.Generic;
using CoC.Backend.UI;

namespace CoC.Backend.Tools
{
	public sealed class ButtonListMaker
	{
		public int numberOfButtons => buttons.Count;
		private readonly List<ButtonData> buttons = new List<ButtonData>();

		private Action[] generatePageCallback;

		private readonly int rowCount = DisplayBase.BUTTON_ROWS;
		private readonly int columnCount = DisplayBase.BUTTON_COLUMNS;
		private readonly DisplayBase display;

		public ButtonListMaker(DisplayBase currentDisplay)
		{
			display = currentDisplay ?? throw new ArgumentNullException(nameof(currentDisplay));
		}

		public void AddButtonToList(string desc, bool active, Action callback)
		{
			buttons.Add(new ButtonData(desc, active, callback));
		}

		public void AddButtonToList(string desc, bool active, Action callback, string tip, string replacementTitle)
		{
			buttons.Add(new ButtonData(desc, active, callback, tip, replacementTitle));
		}
		public void ClearList()
		{
			buttons.Clear();
		}

		public void CreateButtons(bool reserveFinalSpace = true)
		{
			ButtonData preserveButton = null;
			if (reserveFinalSpace)
			{
				preserveButton = display.GetButtonData((byte)(rowCount * columnCount - 1));
			}
			CreateButtonsPrivate(preserveButton);
		}
		public void CreateButtons(string desc, bool active, Action callback)
		{
			if (callback is null) throw new ArgumentNullException(nameof(callback));
			if (desc is null) throw new ArgumentNullException(nameof(desc));

			CreateButtonsPrivate(new ButtonData(desc, active, callback));
		}

		public void CreateButtons(string desc, bool active, Action callback, string tip, string replacementTitle)
		{
			if (callback is null) throw new ArgumentNullException(nameof(callback));
			if (desc is null) throw new ArgumentNullException(nameof(desc));

			CreateButtonsPrivate(new ButtonData(desc, active, callback, tip, replacementTitle));
		}


		private void CreateButtonsPrivate(ButtonData preserveButton)
		{
			if (numberOfButtons > rowCount * columnCount - (preserveButton is null ? 0 : 1))
			{
				ButtonData[][] buttonGroups = new ButtonData[(int)Math.Ceiling(numberOfButtons / ((rowCount - 1) * columnCount) * 1.0)][];
				int iter = 0;
				for (int x = 0; x < buttonGroups.Length; x++)
				{
					if (numberOfButtons - iter > columnCount)
					{
						buttonGroups[x] = new ButtonData[columnCount];
					}
					else
					{
						buttonGroups[x] = new ButtonData[numberOfButtons - iter];
					}

					for (int y = 0; y < buttonGroups[x].Length; y++)
					{
						buttonGroups[x][y] = buttons[iter];
						iter++;
					}
				}

				generatePageCallback = new Action[buttonGroups.Length];

				for (int x = 0; x < generatePageCallback.Length; x++)
				{
					int q = x;
					generatePageCallback[q] = () =>
					{
						GenerateButtons(buttonGroups[q], preserveButton);
						if (x > 0)
						{
							display.AddButton((byte)((rowCount - 1) * columnCount), new ButtonData(GlobalStrings.PREVIOUS_PAGE(), true, () => GenerateButtons(buttonGroups[q - 1], preserveButton)), true);
						}
						if (x < generatePageCallback.Length - 1)
						{
							display.AddButton((byte)((rowCount - 1) * columnCount + 1), new ButtonData(GlobalStrings.NEXT_PAGE(), true, () => GenerateButtons(buttonGroups[q + 1], preserveButton)), true);
						}
					};
				}
			}
			else
			{
				GenerateButtons(buttons, preserveButton);
			}
		}

		private void GenerateButtons(IEnumerable<ButtonData> items, ButtonData preserveButton)
		{
			byte y = 0;
			foreach (var button in items)
			{
				byte x = y;
				if (button.enabled)
				{
					display.AddButton(x, new ButtonData(button.title, true, button.onClick, button.tooltip, button.tooltipTitle));
				}
				else
				{
					display.AddButton(x, new ButtonData(button.title, false, null, button.tooltip, button.tooltipTitle));
				}
				y++;
			}
			if (preserveButton != null)
			{
				if (preserveButton.enabled)
				{
					display.AddButton((byte)(rowCount * columnCount - 1), new ButtonData(preserveButton.title, true, preserveButton.onClick, preserveButton.tooltip, preserveButton.tooltipTitle));
				}
				else
				{
					display.AddButton((byte)(rowCount * columnCount - 1), new ButtonData(preserveButton.title, false, null, preserveButton.tooltip, preserveButton.tooltipTitle));
				}
			}
		}
	}
}
