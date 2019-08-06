using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoCWinDesktop.Strings
{
	public static class LanguageLookup
	{
#warning ToDo: Clean this up, and make the StatDisplay not use this.
		public static string Lookup(string englishText)
		{
			return char.ToUpper(englishText[0]) + englishText.Substring(1);
		}
	}
}