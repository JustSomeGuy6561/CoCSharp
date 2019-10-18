using CoC.Backend.Engine;
using CoC.Backend.Strings;
using CoC.Frontend.UI;
using CoC.Frontend.Engine;
using System;
using System.Collections.Generic;
using CoC.Backend.UI;

namespace CoC.Frontend.Tools
{
	public sealed class ButtonListMaker
	{
		public int numberOfButtons => buttons.Count;
		private readonly List<ButtonStorage> buttons = new List<ButtonStorage>();

		private Action[] generatePageCallback;

		private readonly int rowCount, columnCount;

		private readonly StandardDisplay display;

		public ButtonListMaker(StandardDisplay currentDisplay, int rows, int columns)
		{
			display = currentDisplay ?? throw new ArgumentNullException(nameof(currentDisplay));
			rowCount = rows;
			columnCount = columns;
		}

		public void AddButtonToList(string desc, bool active, Action callback)
		{
			buttons.Add(new ButtonStorage(desc, active, callback));
		}

		public void AddButtonToList(string desc, bool active, Action callback, string tip, string replacementTitle)
		{
			buttons.Add(new ButtonStorage(desc, active, callback, tip, replacementTitle));
		}
		public void ClearList()
		{
			buttons.Clear();
		}

		public void CreateButtons(bool reserveFinalSpace = true)
		{
			ButtonStorage preserveButton = null;
			if (reserveFinalSpace)
			{
				preserveButton = new ButtonStorage(display.GetButtonData((byte)(rowCount * columnCount - 1)));
			}
			CreateButtonsPrivate(preserveButton);
		}
		public void CreateButtons(string desc, bool active, Action callback)
		{
			if (callback is null) throw new ArgumentNullException(nameof(callback));
			if (desc is null) throw new ArgumentNullException(nameof(desc));

			CreateButtonsPrivate(new ButtonStorage(desc, active, callback));
		}

		public void CreateButtons(string desc, bool active, Action callback, string tip, string replacementTitle)
		{
			if (callback is null) throw new ArgumentNullException(nameof(callback));
			if (desc is null) throw new ArgumentNullException(nameof(desc));

			CreateButtonsPrivate(new ButtonStorage(desc, active, callback, tip, replacementTitle));
		}


		private void CreateButtonsPrivate(ButtonStorage preserveButton)
		{
			if (numberOfButtons > rowCount * columnCount - (preserveButton is null ? 0 : 1))
			{
				ButtonStorage[][] buttonGroups = new ButtonStorage[(int)Math.Ceiling(numberOfButtons / ((rowCount - 1) * columnCount) * 1.0)][];
				int iter = 0;
				for (int x = 0; x < buttonGroups.Length; x++)
				{
					if (numberOfButtons - iter > columnCount)
					{
						buttonGroups[x] = new ButtonStorage[columnCount];
					}
					else
					{
						buttonGroups[x] = new ButtonStorage[numberOfButtons - iter];
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
							display.AddButton((byte)((rowCount - 1) * columnCount), GlobalStrings.PREVIOUS_PAGE(), () => GenerateButtons(buttonGroups[q - 1], preserveButton));
						}
						if (x < generatePageCallback.Length - 1)
						{
							display.AddButton((byte)((rowCount - 1) * columnCount + 1), GlobalStrings.NEXT_PAGE(), () => GenerateButtons(buttonGroups[q + 1], preserveButton));
						}
					};
				}
			}
			else
			{
				GenerateButtons(buttons, preserveButton);
			}
		}

		private void GenerateButtons(IEnumerable<ButtonStorage> items, ButtonStorage preserveButton)
		{
			byte y = 0;
			foreach (var button in items)
			{
				byte x = y;
				if (button.enabled)
				{
					display.AddButtonWithToolTip(x, button.title, button.onClick, button.tooltip, button.tooltipTitle);
				}
				else
				{
					display.AddButtonDisabledWithToolTip(x, button.title, button.tooltip, button.tooltipTitle);
				}
				y++;
			}
			if (preserveButton != null)
			{
				if (preserveButton.enabled)
				{
					display.AddButtonWithToolTip((byte)(rowCount * columnCount - 1), preserveButton.title, preserveButton.onClick, preserveButton.tooltip, preserveButton.tooltipTitle);
				}
				else
				{
					display.AddButtonDisabledWithToolTip((byte)(rowCount * columnCount - 1), preserveButton.title, preserveButton.tooltip, preserveButton.tooltipTitle);
				}
			}
		}

		private class ButtonStorage
		{
			private readonly string titleStr;
			private readonly string tooltipStr;
			private readonly string tooltipTitleStr;

			public string title => titleStr.Trim().Truncate(ButtonData.MAX_BUTTON_TEXT_LEN);
			public string tooltip => tooltipStr.Trim();
			public string tooltipTitle => string.IsNullOrWhiteSpace(tooltip) ? "" : string.IsNullOrWhiteSpace(tooltipTitleStr) ? title : tooltipTitleStr;

			public readonly bool enabled;
			public readonly Action onClick;

			public ButtonStorage(string desc, bool active, Action callback)
			{
				titleStr = desc ?? throw new ArgumentNullException();
				tooltipStr = "";
				tooltipTitleStr = "";
				enabled = active;
				onClick = callback;
			}

			public ButtonStorage(string desc, bool active, Action callback, string tip, string replacementTitle)
			{
				titleStr = desc ?? throw new ArgumentNullException();
				tooltipStr = tip ?? "";
				tooltipTitleStr = replacementTitle ?? "";
				enabled = active;
				onClick = callback;
			}

			public ButtonStorage(ButtonData source)
			{
				titleStr = source.title;
				tooltipStr = source.tooltip;
				tooltipTitleStr = source.tooltipTitle;
				enabled = source.enabled;
				onClick = source.onClick;
			}
		}


	}
}
