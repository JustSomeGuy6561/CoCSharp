using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Engine.Language
{
	public abstract class LanguageBase
	{
		public abstract string GetText(string functionName, Type functionClass);

		/// <summary>
		/// Retrieves the name of the language in the native tongue. So English=English. French=Francois. German=Deutsch, etc.
		/// </summary>
		/// <returns>the name of the language</returns>
		public abstract string LanguageName();

		/// <summary>
		/// Retrieves the equivalent of the word "Language" from the language.
		/// </summary>
		/// <returns>'language' in this language</returns>
		public abstract string Language();

		/// <summary>
		/// Retrieves text explaining how to change the language. This can be formatted with HTML formatting.
		/// </summary>
		/// <returns>a string describing how to change the language.</returns>
		public abstract string LanguageInstructionText();

		/// <summary>
		/// Retrieves an example of how text may appear in the given language. This can be formatted with HTML formatting.
		/// </summary>
		/// <returns>a string showing how the language will appear in game.</returns>
		public abstract string GenericFlavorTextExample();
	}
}
