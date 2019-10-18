//InputField.cs
//Description:
//Author: JustSomeGuy
//6/19/2019, 7:40 PM
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CoC.Frontend.UI
{
	public sealed class InputField
	{
		private bool ContentChangedSinceLastQuery = false;

		public bool active { get; private set; } = false;

		//optional regular expression. The GUI's job is to parse this data and prevent invalid characters. for example, non-numbers in a number related field.
		//Note that we cannot guarentee that all GUI's will be able to do this, so anything that does need this should try parsing first, and have an output for if it fails (generally, try again!)
		//NOTE FOR GUI DEVS: can be null!

		//two Regular Expressions: one that does the simple filtering: limits input to only the valid characters, and one that checks if the finished string is valid.
		//for example: for a length, '.' is not valid, but it is part of the valid string - the player could type '.25' and that's perfectly fine. You can't prevent them from
		//entering an '.' because it could become valid, event if it's not currently valid. you can, of course, prevent all letters, as they aren't valid. you can also prevent 
		//a second '.', because a number cannot have two decimal places.
		public Regex limitValidInputCharacters { get; private set; }
		public Regex checkTextForValidity { get; private set; }

		//optional default input. allows the input field to have a default value, and potentially remember it if the GUI supports some sort of reset functionality, i dunno. regardless, it's stored. 
		public string defaultInput { get; private set; }

		//the actual input. initially set to defaultInput value.
		//set NEEDS TO BE PUBLIC
		public string input { get; private set; }

		//optional, the text above the input field desribing what it is. may not be used in all GUIs, but it doesn't hurt to be verbose.
		public string title { get; private set; }

		public int? maxChars { get; private set; }

		internal void Clear()
		{
			input = "";
			defaultInput = "";
			title = null;
			maxChars = null;
			limitValidInputCharacters = null;

			DeactivateInputField();
		}

		public static Regex VALID_POSITIVE_INTEGER_NONZERO => new Regex(@"^\+?[\d]*[1-9][\d]*$"); //optional plus sign. 0 or more digits. 1 non-zero digit. 0 or more digits.
		
		//optional plus. Three alternatives:
			//0 or more digits. 1-9. 0 or more digits.
			//0 or more digits. 1-9. 0 or more digits. decimal point. 1 or more digits. 
			//0 or more digits. decimal point. 0 or more digits. 1-9. 0 or more digits. 
		public static Regex VALID_POSITIVE_NUMBER_NONZERO => new Regex(@"^[+]?(\d*[1-9]\d*|\d*[1-9]\d*\.\d+|\d*\.\d*[1-9]\d*)$");

		public static Regex VALID_ALL_NUMBER_NONZERO => new Regex(@"^[-+]?(\d*[1-9]\d*|\d*[1-9]\d*\.\d+|\d*\.\d*[1-9]\d*)$");

		//feel free to add more common regular expressions here, like one for letters and numbers or only letters, etc.
		public static Regex INPUT_POSITIVE_NUMBERS => new Regex(@"^[+]?[0-9]*\.?[0-9]*$");//optional + sign. 0 or more digits. optional decimal point. zero or more digits.
		public static Regex VALID_POSITIVE_NUMBERS => new Regex(@"^[+]?[0-9]*\.?[0-9]+$");//optional + sign. 0 or more digits. optional decimal point. zero or more digits.

		public static Regex INPUT_POSITIVE_NUMBERS_NOSIGN => new Regex(@"^[0-9]*\.?[0-9]*$");//0 or more digits. optional decimal point. zero or more digits.
		public static Regex VALID_POSITIVE_NUMBERS_NOSIGN => new Regex(@"^[0-9]*\.?[0-9]+$");//0 or more digits. optional decimal point. zero or more digits.

		public static Regex INPUT_POSITIVE_INTEGERS => new Regex(@"^[+]?[0-9]*$");//optional + sign. 0 or more digits.
		public static Regex VALID_POSITIVE_INTEGERS => new Regex(@"^[+]?[0-9]+$");//optional + sign. 1 or more digits.

		public static Regex INPUT_POSITIVE_INTEGERS_NOSIGN => new Regex(@"^[0-9]*$");//0 or more digits.
		public static Regex VALID_POSITIVE_INTEGERS_NOSIGN => new Regex(@"^[0-9]+$");//1 or more digits.

		public static Regex INPUT_ALL_NUMBERS => new Regex(@"^[-+]?[0-9]*\.?[0-9]*$"); //optional +/- sign. 0 or more digits. optional decimal point. zero or more digits
		public static Regex VALID_ALL_NUMBERS => new Regex(@"^[-+]?[0-9]*\.?[0-9]+$"); //does allow -.9, but that's still a valid number. 

		public static Regex COLOR => new Regex(@"^#?[0-9A-Fa-f]{6}?"); //standard #000000 color. html allows named colors, too, so this isn't used in out parser. 

		internal string output => input;

		//we'll allow null regex. Null value is treated as allowing everything. 
		private void UpdateInputField(Regex possibleInputCharacters, Regex validInputString, string defaultValue, string titleText, int? maxLength)
		{
			limitValidInputCharacters = possibleInputCharacters;
			checkTextForValidity = validInputString;
			defaultInput = defaultValue ?? "";
			input = defaultValue ?? "";
			title = titleText;
			maxChars = maxLength;

			ContentChangedSinceLastQuery = true;
		}

		internal bool QueryStatus()
		{
			bool retVal = ContentChangedSinceLastQuery;
			ContentChangedSinceLastQuery = false;
			return retVal;
		}

		internal InputField()
		{
			limitValidInputCharacters = null;
			checkTextForValidity = null;
			defaultInput = "";
			input = "";
			title = null;
			maxChars = null;
		}

		internal bool ActivateInputField(Regex possibleInputCharacters, Regex validInputString, string defaultValue = "", string titleText = "", int? maxLength = null)
		{
			if (possibleInputCharacters is null) validInputString = null;

			bool retVal = !active;
			active = true;
			UpdateInputField(possibleInputCharacters, validInputString, defaultValue, titleText, maxLength);

			ContentChangedSinceLastQuery = true;
			return true;
		}

		internal bool ActivateInputField(string defaultValue = "", string titleText = "", int? maxLength = null)
		{
			bool retVal = !active;
			active = true;
			UpdateInputField(null, null, defaultValue, titleText, maxLength);

			ContentChangedSinceLastQuery = true;
			return true;
		}

		internal void UpdateInputText(string newDefaultText)
		{
			defaultInput = newDefaultText;
			input = newDefaultText;

			ContentChangedSinceLastQuery = true;
		}

		internal bool DeactivateInputField()
		{
			if (!active)
			{
				return false;
			}
			active = false;
			ContentChangedSinceLastQuery = true;
			return true;
		}

		public void UpdateInputFromOutput(string output)
		{
			input = output;
		}
	}
}
