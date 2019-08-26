using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Engine.Language
{
	public sealed class AmericanEnglish : LanguageBase
	{
		public override string GenericFlavorTextExample()
		{
			return @"You were out walking  one fine and dandy day and decided to speak aloud in your native tongue, because apparently that's something you do" +
			@" in order to make sure nothing weird has happened like someone magically changing the language you speak. ""<i>Hello World!</i>"" Sure enough, it was English. " +
			@"You feel silly for questioning yourself, but stranger things have happened." + Environment.NewLine + Environment.NewLine + 
			"This is <u>underlined</u>. This is <b>bold</b>. This is <font color=DarkRed>Dark Red</font>. This is <font color=Green>Green</font>";
		}

		//fallback to default, because we're English already. 
		public override string GetText(string functionName, Type functionClass)
		{
			return null;
		}

		public override string Language()
		{
			return "Language";
		}

		public override string LanguageInstructionText()
		{
			return "You can change the language this game uses by selecting a new one from the drop-down menu here. Note that due to the open-source nature, " +
			"it's entirely possible new content has been added that hasn't been translated into your language. In such cases, it will print the fallback text in it's native language, " +
			"which is most likely English. In an ideal world this would never happen, but the truth is we're glad we even have the language option at all!";
		}

		public override string LanguageName()
		{
			return "English";
		}
	}
}
