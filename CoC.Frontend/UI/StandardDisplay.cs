//StandardDisplay.cs
//Description:
//Author: JustSomeGuy
//Note: date follows MMDDYYYY format.
//10/14/2019 3:52:35 PM

using CoC.Backend;
using CoC.Backend.Engine;
using CoC.Backend.UI;
using CoC.Frontend.UI;
using CoC.Frontend.UI.ControllerData;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;

namespace CoC.Frontend.UI
{
	public sealed class StandardDisplay : DisplayBase
	{
		private static readonly Action Nothing = () => { };

		public ReadOnlyCollection<ButtonData> activeButtons => new ReadOnlyCollection<ButtonData>(buttons);

		private readonly InputField input;

		private readonly DropDownMenu dropDownMenu;

		private string imageID = null;

		private SpriteAndCreditsOutput spriteAndCredits;

		public StandardDisplay()
		{
			input = new InputField();
			dropDownMenu = new DropDownMenu();
			spriteAndCredits = new SpriteAndCreditsOutput();
		}

		public StandardDisplay(string initialText) : this()
		{
			OutputText(initialText);
		}

		//internal StandardDisplay(DisplayBase source) : this()
		//{
		//	content.Append(source.gimmeDatContent);
		//}

		public bool AddButton(byte index, string title, Action callback)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();
			if (callback is null) throw new ArgumentNullException(nameof(callback), "Callback cannot be null or the button would never do anything");
			if (buttons[index] != null) return false;

			buttonsChanged = true;

			buttons[index] = MakeButton(title, true, callback);
			return true;
		}

		public bool AddButtonWithToolTip(byte index, string title, Action callback, string tip, string replacementTitle = null)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();

			if (buttons[index] != null) return false;

			buttonsChanged = true;

			buttons[index] = MakeButton(title, true, callback, tip, replacementTitle);
			return true;
		}


		public bool AddButtonDisabled(byte index, string title)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();

			if (buttons[index] != null) return false;

			buttonsChanged = true;

			buttons[index] = MakeButton(title, false, Nothing);
			return true;

		}

		internal void ClearInputData()
		{
			input.Clear();
		}

		public bool AddButtonDisabledWithToolTip(byte index, string title, string tip, string replacementTitle = null)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();

			if (buttons[index] != null) return false;

			buttonsChanged = true;

			buttons[index] = MakeButton(title, false, Nothing, tip, replacementTitle);
			return true;
		}

		public bool AddButtonIf(byte index, string title, bool condition, Action enabledCallback)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();
			if (buttons[index] != null) return false;

			Action callback = condition ? enabledCallback : Nothing;

			buttonsChanged = true;

			buttons[index] = MakeButton(title, condition, callback);
			return true;
		}

		

		public bool QueryButtons(out ReadOnlyCollection<ButtonData> buttonCollection)
		{
			bool retVal = buttonsChanged;

			buttonsChanged = false;
			buttonCollection = activeButtons;
			return retVal;
		}

		public bool AddButtonIfWithToolTip(byte index, string title, bool condition, Action enabledCallback, string enabledTip, string disabledTip, string replacementTitle = null)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();
			if (buttons[index] != null) return false;

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


			buttons[index] = MakeButton(title, condition, callback, tip, replacementTitle);
			return true;
		}

		public bool AddButtonOrAddDisabledWithToolTip(byte index, string title, bool condition, Action enabledCallback, string disabledTip, string replacementTitle = null)
		{
			if (index >= MAX_BUTTONS) throw new IndexOutOfRangeException();
			if (buttons[index] != null) return false;

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

			buttons[index] = MakeButton(title, condition, callback, tip, replacementTitle);
			return true;
		}

		public override void ClearContent()
		{
			base.ClearContent();
			imageID = null;
		}

		public bool OutputImage(string uniqueImageIdentifier)
		{
			contentChanged = true;
			if (imageID == null)
			{
				imageID = uniqueImageIdentifier;
				return false;
			}
			else
			{
				imageID = uniqueImageIdentifier;
				return true;
			}
		}

		internal bool QueryData(out StringBuilder _outputField, out string _outputImagePath)
		{
			bool retVal = contentChanged;
			contentChanged = false;
			_outputField = new StringBuilder(content.ToString());
			_outputImagePath = imageID;
			return retVal;
		}

		public override void ClearOutput()
		{
			base.ClearOutput();

			input.DeactivateInputField();
			dropDownMenu.DeactivateDropDownMenu();
		}

		internal bool ActivateInputField(Regex possibleInputCharacters, Regex validInputString, string defaultValue = "", string titleText = "", int? maxLength = null)
		{
			return input.ActivateInputField(possibleInputCharacters, validInputString, defaultValue, titleText, maxLength);
		}

		internal bool ActivateInputField(string defaultValue = "", string titleText = "", int? maxLength = null)
		{
			return input.ActivateInputField(defaultValue, titleText, maxLength);
		}

		internal void UpdateInputText(string newDefaultText)
		{
			input.UpdateInputText(newDefaultText);
		}

		internal bool DeactivateInputField()
		{
			return input.DeactivateInputField();
		}

		internal bool QueryInputField(out InputField inputField)
		{
			inputField = input;
			return inputField.QueryStatus();
		}

		internal bool ActivateDropDownMenu(DropDownEntry[] entries)
		{
			return dropDownMenu.ActivateDropDownMenu(entries);
		}

		internal bool DeactivateDropDownMenu()
		{
			return dropDownMenu.DeactivateDropDownMenu();
		}

		internal void SetPostDropDownMenuText(SimpleDescriptor text)
		{
			dropDownMenu.SetPostDropDownMenuText(text);
		}

		internal bool QueryDropDownStatus(out DropDownMenu menu)
		{
			menu = dropDownMenu;
			return dropDownMenu.QueryStatus();
		}

		internal bool QueryPostText(out string postControlText)
		{
			return dropDownMenu.QueryPostText(out postControlText);
		}

		internal void SetSprite(string name)
		{
			spriteAndCredits.SetSprite(name);
		}

		internal void ClearSprite()
		{
			spriteAndCredits.ClearSprite();
		}

		internal void SetCreator(string creator)
		{
			spriteAndCredits.SetCreator(creator);
		}

		internal bool QuerySpriteCreditData(out string sprite, out string creator)
		{
			return spriteAndCredits.QuerySpriteCreditData(out sprite, out creator);
		}

		internal string GetOutput()
		{
			return input.output;
		}
	}
}
