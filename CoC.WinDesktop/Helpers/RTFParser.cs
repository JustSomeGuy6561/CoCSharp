using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace CoCWinDesktop.Helpers
{
	public static class RTFParser
	{
		public static string FromHTMLNoHeader(StringBuilder HTMLText, Color defaultColor, out List<Color> colors)
		{
			//"Simple" replace

			////quick note: @ means treat string as literal, so it's looking for '\', not an escape character.
			//// @"\" is identical to "\\" So it won't replace "\r" or "\n"
			HTMLText.Replace(@"\", @"\\");
			HTMLText.Replace(@"{", @"\{"); //see above
			HTMLText.Replace(@"}", @"\}"); //see above

			HTMLText.Replace(@"<b>", @"\b ");
			HTMLText.Replace(@"</b>", @"\b0 ");
			HTMLText.Replace(@"<i>", @"\i ");
			HTMLText.Replace(@"</i>", @"\i0 ");
			HTMLText.Replace(@"<em>", @"\i "); //who even uses this?
			HTMLText.Replace(@"</em>", @"\i0 ");

			HTMLText.Replace(@"<pre>", @"");
			HTMLText.Replace(@"</pre>", @"");

			//note the lack of @ on these strings - these strings we actually want to escape and look for the special character. 
			//more fun facts: IIRC Environment.NewLine returns "\n" for all things not Windows, though idk if it has funny rules for Xamarin on Mac. 
			HTMLText.Replace("\r\n", @"\par "); //You're on Windows and used the Environment.NewLine. Fun fact, the older parser stripped out the \r in \r\n
			HTMLText.Replace("\n", @"\par "); //The 'standard'. IMO the safe route of throwing both (windows) is better, though then there are parsers that treat that as \n\n and that's bad.
			HTMLText.Replace("\r", @"\par "); //It was written on a Mac. I hate you. JK, but damn - I'm surprised the old parser didn't blow up with \r
			HTMLText.Replace("\t", @"\tab ");

			//This is a simplistic way of simulating an unordered list. Hacky as all hell, and it'll break with nested lists, but we never have nested lists so this should work fine. 
			//also this will break with a random <li> or if there are ever ordered lists (<ol>). It's possible to do it correctly with about 10 different bullshit RTF tags, but fuck that.
			//I mean, if you have a hard-on for perfectly formed RTF, i think you're playing the wrong game here ¯\_(ツ)_/¯. But by all means, fix it.   
			HTMLText.Replace(@"<ul>", @"");
			HTMLText.Replace(@"</ul>", @"\line ");
			HTMLText.Replace(@"<li>", @"\line  \bullet  ");
			HTMLText.Replace(@"</li>", @"");

			HTMLText.Replace(@"<u>", @"\ul ");
			HTMLText.Replace(@"</u>", @"\ul0 ");
			HTMLText.Replace(@"<br/>", @"\line ");
			HTMLText.Replace(@"<br />", @"\line ");
			HTMLText.Replace(@"<br>", @"\line ");
			HTMLText.Replace(@"</br>", @""); //i actually hate you.
			string partiallyFormatted = HTMLText.ToString();

			//Color region

			//add the first color manually b/c it's actually a solid color brush. This is our default color for this render. 
			IEnumerable<Color> colorList = new List<Color>
			{
				defaultColor
			};

			Regex regex = new Regex("<font[^<>]*color[^<>]*>", RegexOptions.IgnoreCase); //all the somewhat valid font tags with color in them somewhere.
			var matches = regex.Matches(partiallyFormatted);
			string[] matchList = new string[matches.Count];
			int x = 0;
			string reg = @".*color\s*=\s*" + "\"" + @"?\s*(#[0-9A-Fa-f]{6}|[A-Z]+)" + @".*";
			//string reg = @"color\s?=\s?";
			Regex removeSpaces = new Regex(@"\s+");
			Regex extractColor = new Regex(reg, RegexOptions.IgnoreCase);

			string[] formattedMatches = new string[matches.Count];
			foreach (Match match in matches)
			{
				matchList[x] = match.Value;

				//formattedMatches[x] = removeSpaces.Replace(matchList[x], @" ");
				formattedMatches[x] = extractColor.Replace(matchList[x], @"$1");
				x++;
			}

			Color toColor(string text)
			{
				Color color;
				try
				{
					color = (Color)ColorConverter.ConvertFromString(text);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.StackTrace);
					color = defaultColor;
				}
				return color;
			}


			//formatted matches and matchColors are the same length. which is als the length of matchList. 
			x = 0;
			IEnumerable<Color> matchColors = formattedMatches.Select(toColor);
			Color[] matchColorArray = matchColors.ToArray();
			Dictionary<string, Color> matchesToColorLookup = new Dictionary<string, Color>();
			for (x = 0; x < matchList.Length; x++)
			{
				matchesToColorLookup[matchList[x]] = matchColorArray[x]; //if there's a collision they should be identical anyway. 
			}


			colorList = colorList.Union(matchColors).Distinct();
			colors = colorList.ToList();
			Dictionary<Color, int> colorLookup = colors.Select((y, z) => new { y, z }).ToDictionary(q => q.y, q => q.z);

			foreach (var matchPair in matchesToColorLookup)
			{
				string match = matchPair.Key;
				int index = colorLookup[matchPair.Value];
				regex = new Regex(match);
				partiallyFormatted = regex.Replace(partiallyFormatted, @"\cf" + index + " ");
			}


			return partiallyFormatted.Replace(@"</font>", @"\cf0 ");
			//end horrid color nightmare
		}

		public static string FromHTML(StringBuilder HTMLBuilder, ModelViewRunner runner)
		{
			string formatted = FromHTMLNoHeader(HTMLBuilder, runner.FontColor.Color, out List<Color> colors);
			return FromRTFText(formatted, colors, runner);
		}

		public static string FromHTML(string HTMLText, ModelViewRunner runner)
		{
			StringBuilder sb = new StringBuilder(HTMLText);
			return FromHTML(sb, runner);
		}

		public static string FromRTFBuilder(StringBuilder RTFBuilder, List<Color> colors, ModelViewRunner runner)
		{
			return FromRTFText(RTFBuilder.ToString(), colors, runner);
		}

		public static string FromRTFText(string RTFText, List<Color> colors, ModelViewRunner runner)
		{
			string font = runner.TextFontFamily.FamilyNames.FirstOrDefault().Value ?? "Times New Roman";
			int fontEmSize = runner.FontSizeEms;
			string formatted = RTFText;

			StringBuilder sb = new StringBuilder(@"{\rtf1\ansi\deff0" + Environment.NewLine +
				@"{\fonttbl{\f0\ " + font + @";}" + Environment.NewLine +
				@"{\colortbl");

			colors.ForEach(y => sb.Append(RTFColor(y)));

			sb.Append("}}" + Environment.NewLine +
				@"{\fs" + fontEmSize + @"\fn0\cf0 " + formatted + @"}");
			return sb.ToString();
		}

		public static string RTFColor(Color color)
		{
			return @"\red" + color.R + @"\green" + color.G + @"\blue" + color.B + @";";
		}

		public static string ToRTFSafeString(string unsafeString)
		{
			////quick note: @ means treat string as literal, so it's looking for '\', not an escape character.
			//// @"\" is identical to "\\" So it won't replace "\r" or "\n"
			unsafeString.Replace(@"\", @"\\");
			unsafeString.Replace(@"{", @"\{"); //see above
			unsafeString.Replace(@"}", @"\}"); //see above
			return unsafeString;
		}

		//after delving ass-deep in c# source using dotPeek (apparently reference source doesn't want to play nice, idk)
		//it turns out the hyperlink needs to be quotated in order to work. fun times. 
		public static string ToRTFUrl(Uri unsafeUrl)
		{
			string temp = ToRTFSafeString(unsafeUrl.ToString());
			return @"{\cf1\f0\lang9{\field{\*\fldinst{HYPERLINK """ + temp + @""" }}{\fldrslt{" + temp + @"\ul0\cf0}}}}";
		}

	}
}
