using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CoC.Frontend.UI
{
	public sealed class InputField
	{
		//optional regular expression. The GUI's job is to parse this data and prevent invalid characters. for example, non-numbers in a number related field.
		//Note that we cannot guarentee that all GUI's will be able to do this, so anything that does need this should try parsing first, and have an output for if it fails (generally, try again!)
		//NOTE FOR GUI DEVS: can be null!
		public readonly Regex limitValidInputCharacters;

		//optional default input. allows the input field to have a default value, and potentially remember it if the GUI supports some sort of reset functionality, i dunno. regardless, it's stored. 
		public readonly string defaultInput;

		//the actual input. initially set to defaultInput value.
		public string input;

		//optional, the text above the input field desribing what it is. may not be used in all GUIs, but it doesn't hurt to be verbose.
		public readonly string title;

		//feel free to add more common regular expressions here, like one for letters and numbers or only letters, etc.
		public static Regex POSITIVE_NUMBERS => new Regex("[^0-9.]+");
		public static Regex ALL_NUMBERS => new Regex("[^0-9.-]+");




		private InputField(Regex limitingRegex, string defaultValue, string titleText)
		{
			limitValidInputCharacters = limitingRegex;
			defaultInput = defaultValue;
			input = defaultValue;
			title = titleText;
		}

		internal static bool ActivateInputField(Regex limitingCharacters = null, string defaultValue = "", string titleText = "")
		{
			if (instance != null)
			{
				return false;
			}
			instance = new InputField(limitingCharacters, defaultValue, titleText);
			return true;
		}

		internal static bool DeactivateInputField()
		{
			if (instance == null)
			{
				return false;
			}
			instance = null;
			return true;
		}

		internal static InputField instance;
	}
}
