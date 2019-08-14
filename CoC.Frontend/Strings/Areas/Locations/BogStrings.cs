//BogStrings.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:24 AM
using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Areas.Locations
{
	internal sealed partial class Bog
	{
		private static string BogName()
		{
			return "Bog";
		}
		private static string BogUnlock()
		{
			return "While exploring the swamps, you find yourself into a particularly dark, humid area of this already fetid biome. " +
				"You judge that you could find your way back here pretty easily in the future, if you wanted to. " +
				"With your newfound discovery fresh in your mind, you return to camp." +
				Environment.NewLine + Environment.NewLine + SafelyFormattedString.FormattedText("You've discovered the Bog!", StringFormats.BOLD);
		}

	}
}
