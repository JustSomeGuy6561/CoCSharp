using System;
using System.Collections.Generic;
using CoC.Backend.Engine;
using CoC.Backend.Strings;
using CoC.Backend.UI;

namespace CoC.Backend.Tools
{
	public sealed class ButtonListMaker
	{
		public int numberOfButtons => buttons.Count;
		private readonly List<ButtonData> buttons = new List<ButtonData>();

		private Action[] generatePageCallback;

		private readonly int rowCount;
		private readonly int columnCount;
		private readonly DisplayBase display;

		public ButtonListMaker(DisplayBase currentDisplay)
		{
			rowCount = DisplayBase.BUTTON_ROWS;
			columnCount = DisplayBase.BUTTON_COLUMNS;

			display = currentDisplay ?? throw new ArgumentNullException(nameof(currentDisplay));
		}

		public ButtonListMaker(DisplayBase currentDisplay, byte customRowCount, byte customColumnCount)
		{
			rowCount = Math.Min(customRowCount, DisplayBase.BUTTON_ROWS);
			columnCount = Math.Min(customColumnCount, DisplayBase.BUTTON_COLUMNS);

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
			CreateButtonsPrivate(null, preserveButton);
		}

		public void CreateButtons(ButtonData returnButton, ButtonData cancelButton)
		{
			CreateButtonsPrivate(returnButton, cancelButton);
		}

		public void CreateButtons(string desc, bool active, Action callback)
		{
			if (callback is null)
			{
				throw new ArgumentNullException(nameof(callback));
			}

			if (desc is null)
			{
				throw new ArgumentNullException(nameof(desc));
			}

			CreateButtonsPrivate(null, new ButtonData(desc, active, callback));
		}

		public void CreateButtons(string desc, bool active, Action callback, string tip, string replacementTitle)
		{
			if (callback is null)
			{
				throw new ArgumentNullException(nameof(callback));
			}

			if (desc is null)
			{
				throw new ArgumentNullException(nameof(desc));
			}

			CreateButtonsPrivate(null, new ButtonData(desc, active, callback, tip, replacementTitle));
		}


		private void CreateButtonsPrivate(ButtonData returnButton, ButtonData cancelButton)
		{
			int maxButtonsPerPage = rowCount * columnCount;
			if (!(returnButton is null)) maxButtonsPerPage--;
			if (!(cancelButton is null)) maxButtonsPerPage--;

			if (numberOfButtons > maxButtonsPerPage)
			{
				ButtonData[][] buttonGroups = new ButtonData[(int)Math.Ceiling(numberOfButtons * 1.0 / maxButtonsPerPage)][];

				int remainingButtons = numberOfButtons;
				for (int x = 0; x < buttonGroups.Length; x++)
				{
					var iter = numberOfButtons - remainingButtons;
					if (remainingButtons >= maxButtonsPerPage)
					{
						buttonGroups[x] = new ButtonData[rowCount * columnCount];
						remainingButtons -= maxButtonsPerPage;
					}
					else
					{
						buttonGroups[x] = new ButtonData[remainingButtons];
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
						GenerateButtons(buttonGroups[q], returnButton, cancelButton);
						if (x > 0)
						{
							display.AddButton((byte)((rowCount - 1) * columnCount), new ButtonData(GlobalStrings.PREVIOUS_PAGE(), true,
								() => GenerateButtons(buttonGroups[q - 1], returnButton, cancelButton)), true);
						}
						if (x < generatePageCallback.Length - 1)
						{
							display.AddButton((byte)((rowCount - 1) * columnCount + 1), new ButtonData(GlobalStrings.NEXT_PAGE(), true,
								() => GenerateButtons(buttonGroups[q + 1], returnButton, cancelButton)), true);
						}
					};
				}
			}
			else
			{
				GenerateButtons(buttons, returnButton, cancelButton);
			}
		}

		private void GenerateButtons(IEnumerable<ButtonData> items, ButtonData returnButton, ButtonData cancelButton)
		{
			byte y = 0;
			foreach (ButtonData button in items)
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

			if (cancelButton is null && !(returnButton is null))
			{
				cancelButton = returnButton;
				returnButton = null;
			}

			if (returnButton != null)
			{
				display.AddButton((byte)(rowCount * columnCount - 2), returnButton);
			}
			if (cancelButton != null)
			{
				display.AddButton((byte)(rowCount * columnCount - 1), cancelButton);
			}
		}
	}
}
