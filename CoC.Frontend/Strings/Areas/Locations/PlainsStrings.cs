//PlainsStrings.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:24 AM
using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Areas.Locations
{
	internal sealed partial class Plains
	{
		private static string PlainsName()
		{
			return "Plains";
		}
		private static string PlainsUnlock()
		{
			return "You find yourself standing in knee-high grass, surrounded by flat plains on all sides. " +
				"Though the mountain, forest, and lake are all visible from here, they seem quite distant." +
				Environment.NewLine + Environment.NewLine + SafelyFormattedString.FormattedText("You've discovered the Plains!", StringFormats.BOLD);
		}

	}
}
