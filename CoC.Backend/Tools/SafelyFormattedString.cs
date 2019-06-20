//SafelyFormattedString.cs
//Description: An entirely optional, yet guarenteed safe, method of writing bold, italic, or underlined strings. it automatically wraps the string in the formatting tags.
//A semi-common and difficult to catch runtime error occurs when text is given a beginning format tag, but not an end tag. Using this whenever formatted text is required will
//prevent this from happening, though it will be ever so slightly less efficient than manually tagging it. 
//Author: JustSomeGuy
//5/30/2019, 8:45 PM
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Tools
{
	[Flags]
	public enum StringFormats { NONE, BOLD = 1, ITALIC = 2, BOLD_ITALIC = 3, UNDERLINE = 4, BOLD_UNDERLINE = 5, ITALIC_UNDERLINE = 6, BOLD_ITALIC_UNDERLINE = 7}

	//During conversion, i noticed a few instances where tags would be started, but if certain conditions were made (if statements, generally) the end tag would not be applied, and the entire 
	//game would mess up its display. Originally, i figured it'd be better to force a formatted text on everything, which would eliminate this error and also make the game more efficient,
	//but after a quick test, i realized it was a pain in the ass, and it wouldn't be much fun for people who don't know how to program and just want to write the text. 
	//This is my best compromise. The end result is still tags everywhere, but if you want to be safe, use this instead of manually doing tags. 

	//The TL;DR or I can't program version: C# automatically treats this class as a string. 
	//If you write                  "this is " + new SafelyFormattedString("BOLD", StringFormats.BOLD), 
	//C# will see it as             "this is <b>BOLD</b>"

	//slightly more technical version: this class is implicitly converted to string via operator overloading. when C# sees this class but expects a string, it automatically calls AsString(),
	//which takes the format applied, and manually adds the opening tags, then the text, and then the closing tags, in the correct order. 

	//NOTE: you can apply more than one format, either by using the convenient concatenations (like BOLD_ITALIC for bold and italic) or by using the bitwise or operator (like BOLD | ITALIC for bold and italic) 
	//for more technical users, this allows some pretty neat trick with binary operators. google bit flags for more info.
	
	public sealed class SafelyFormattedString
	{
		public static string FormattedText(string text, StringFormats format)
		{
			if (string.IsNullOrWhiteSpace(text) || format == StringFormats.NONE)
			{
				return text;
			}
			else
			{
				StringBuilder sb = new StringBuilder();
				//print the format opening tags(if any), in order Bold, then Italic, then Underline
				if (format.HasFlag(StringFormats.BOLD))
				{
					sb.Append("<b>");
				}
				if (format.HasFlag(StringFormats.ITALIC))
				{
					sb.Append("<i>");
				}
				if (format.HasFlag(StringFormats.UNDERLINE))
				{
					sb.Append("<u>");
				}
				//print the text
				sb.Append(text);
				//print the format end tags (if any). this is in reverse order - underline, italic, then bold, because that's the proper way to do html tags. 
				if (format.HasFlag(StringFormats.UNDERLINE))
				{
					sb.Append("</u>");
				}
				if (format.HasFlag(StringFormats.ITALIC))
				{
					sb.Append("</i>");
				}
				if (format.HasFlag(StringFormats.BOLD))
				{
					sb.Append("</b>");
				}
				return sb.ToString();
			}
		}
	}
}
