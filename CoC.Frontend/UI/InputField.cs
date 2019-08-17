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

#warning: Figure out way to check text for if it's valid "So Far..."
#warning: Likely best solution: grant two regex options: one for "valid so far" and one for "valid" - check the first on key enter, the second on lost focus. 


		public Regex limitValidInputCharacters { get; private set; }

		//optional default input. allows the input field to have a default value, and potentially remember it if the GUI supports some sort of reset functionality, i dunno. regardless, it's stored. 
		public string defaultInput { get; private set; }

		//the actual input. initially set to defaultInput value.
		//set NEEDS TO BE PUBLIC
		public string input { get; private set; }

		//optional, the text above the input field desribing what it is. may not be used in all GUIs, but it doesn't hurt to be verbose.
		public string title { get; private set; }

		public int? maxChars { get; private set; }

		internal static void ClearData()
		{
			instance.Clear();
		}

		private void Clear()
		{
			input = "";
			defaultInput = "";
			title = null;
			maxChars = null;
			limitValidInputCharacters = null;

			DeactivateInputField();
		}



		//feel free to add more common regular expressions here, like one for letters and numbers or only letters, etc.
		public static Regex POSITIVE_NUMBERS => new Regex(@"^[+]?[0-9]*\.?[0-9]+$");
		public static Regex POSITIVE_INTEGERS => new Regex(@"^[+]?[0-9]+$");
		public static Regex ALL_NUMBERS => new Regex(@"^[-+]?[0-9]*\.?[0-9]+$"); //does allow -.9, but that's still a valid number. 

		public static Regex COLOR => new Regex(@"^#?[0-9A-Fa-f]{6}?"); //standard #000000 color. html allows named colors, too, so this isn't used in out parser. 

		internal static string output => instance.input;

		//we'll allow null regex. Null value is treated as allowing everything. 
		private void UpdateInputField(Regex limitingRegex, string defaultValue, string titleText, int? maxLength)
		{
			limitValidInputCharacters = limitingRegex;
			defaultInput = defaultValue ?? "";
			input = defaultValue ?? "";
			title = titleText;
			maxChars = maxLength;

			ContentChangedSinceLastQuery = true;
		}

		internal static bool QueryStatus(out InputField inputField)
		{
			bool retVal = instance.ContentChangedSinceLastQuery;
			inputField = instance;
			instance.ContentChangedSinceLastQuery = false;
			return retVal;
		}

		private InputField()
		{
			limitValidInputCharacters = null;
			defaultInput = "";
			input = "";
			title = null;
			maxChars = null;
		}

		internal static bool ActivateInputField(Regex limitingCharacters = null, string defaultValue = "", string titleText = "", int? maxLength = null)
		{
			bool retVal = !instance.active;
			instance.active = true;
			instance.UpdateInputField(limitingCharacters, defaultValue, titleText, maxLength);

			instance.ContentChangedSinceLastQuery = true;
			return true;
		}

		internal static void UpdateInputText(string newDefaultText)
		{
			instance.defaultInput = newDefaultText;
			instance.input = newDefaultText;

			instance.ContentChangedSinceLastQuery = true;
		}

		internal static bool DeactivateInputField()
		{
			if (!instance.active)
			{
				return false;
			}
			instance.active = false;
			instance.ContentChangedSinceLastQuery = true;
			return true;
		}

		public void UpdateInputFromOutput(string output)
		{
			input = output;
		}

		private static readonly InputField instance = new InputField();
	}
}
