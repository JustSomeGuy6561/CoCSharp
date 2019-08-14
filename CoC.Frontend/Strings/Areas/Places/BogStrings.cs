//BazaarStrings.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:24 AM
using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Areas.Places
{
	internal sealed partial class Bazaar
	{
		private static string BazaarName()
		{
			return "Bazaar";
		}
		private static string BazaarUnlock()
		{
			return "While exploring the swamps, you find yourself into a particularly dark, humid area of this already fetid biome. " +
				"You judge that you could find your way back here pretty easily in the future, if you wanted to. " +
				"With your newfound discovery fresh in your mind, you return to camp." +
				Environment.NewLine + Environment.NewLine + SafelyFormattedString.FormattedText("You've discovered the Bazaar!", StringFormats.BOLD);
		}

	}
}
