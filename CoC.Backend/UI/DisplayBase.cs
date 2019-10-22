using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Backend.UI
{
	//building block for any visual output. base class has just content and buttons. Needs to be implemented in frontend, and a constructor reference passed back to backend in initialization
	//

	//public abstract class DisplayBase2
	public abstract class DisplayBase
	{
		public const byte BUTTON_ROWS = 3;
		public const byte BUTTON_COLUMNS = 5;
		public const byte MAX_BUTTONS = BUTTON_ROWS * BUTTON_COLUMNS;

		protected readonly StringBuilder content = new StringBuilder();

		protected readonly ButtonData[] buttons = new ButtonData[MAX_BUTTONS];

		protected bool contentChanged = false;
		protected bool buttonsChanged = false;

		public void OutputText(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				content.Append(text);
				contentChanged = true;
			}
		}

		protected ButtonData MakeButton(string desc, bool active, Action callback)
		{
			return new ButtonData(desc, active, callback);
		}

		protected ButtonData MakeButton(string desc, bool active, Action callback, string tip, string replacementTitle)
		{
			return new ButtonData(desc, active, callback, tip, replacementTitle);
		}

		protected internal bool AddButton(byte index, ButtonData data, bool force = false)
		{
			if (data is null) throw new ArgumentNullException(nameof(data));

			if (index > buttons.Length) throw new IndexOutOfRangeException("Tried to add a button outside of the valid range. Only supports " + MAX_BUTTONS + " buttons.");

			if (buttons[index] is null || force)
			{
				buttons[index] = data;

				buttonsChanged = true;

				return true;
			}
			else
			{
				return false;
			}
		}

		protected DisplayBase()
		//protected DisplayBase2()
		{

		}

		public virtual bool IsEmpty()
		{
			return hasNoText && hasNoButtons;
		}

		public bool hasNoText => content.Length == 0;
		public bool hasNoButtons => buttons.All(x => x is null);

		/// <summary>
		/// Takes any content from another page, and merges it in, either before or after this object's content. The buttons from the other page are ignored.
		/// </summary>
		/// <param name="otherPage"></param>
		/// <param name="after"></param>
		//public virtual void CombineWith(DisplayBase2 otherPage, bool after)
		public virtual void CombineWith(DisplayBase otherPage, bool after)
		{
			if (otherPage is null || otherPage.content.Length == 0)
			{
				return;
			}
			if (after)
			{
				this.content.Append(otherPage.content.ToString());
			}
			else
			{
				string currContent = content.ToString();
				content.Clear();
				content.Append(otherPage.content.ToString());
				content.Append(currContent);
			}
			contentChanged = true;
		}

		public virtual void CombineWith(string other, bool after)
		{
			if (string.IsNullOrWhiteSpace(other))
			{
				return;
			}
			if (after)
			{
				this.content.Append(other);
			}
			else
			{
				string currContent = content.ToString();
				content.Clear();
				content.Append(other);
				content.Append(currContent);
			}
			contentChanged = true;
		}

		public void ClearButtons()
		{
			if (buttons.Any(x => x != null))
			{
				buttonsChanged = true;
			}
			Array.Clear(buttons, 0, buttons.Length);
		}

		public virtual void ClearContent()
		{
			if (content.Length > 0)
			{
				contentChanged = true;
			}
			content.Clear();
		}

		public virtual void ClearOutput()
		{
			ClearButtons();
			ClearContent();
		}

		public bool QueryContentChanged()
		{
			bool retVal = contentChanged;
			contentChanged = false;
			return retVal;
		}

		public ButtonData GetButtonData(byte index)
		{
			if (index >= buttons.Length) throw new IndexOutOfRangeException();
			return buttons[index];
		}

		//public static DisplayBase GenerateMultiPage(string content, ButtonData cancelButton, ButtonData[] buttons)
		//{
		//	if ((buttons is null || buttons.Length == 0) && cancelButton is null)
		//	{
		//		throw new ArgumentNullException(nameof(buttons));
		//		throw new ArgumentNullException(nameof(cancelButton));
		//	}
		//	else
		//	{
		//		GameEngine
		//	}
		//}
	}
}
