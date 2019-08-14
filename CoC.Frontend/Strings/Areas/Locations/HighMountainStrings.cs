//HighMountainsStrings.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:24 AM
using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Areas.Locations
{
	internal sealed partial class HighMountain
	{
		private static string HighMountainName()
		{
			return "HighMountains";
		}
		private static string HighMountainUnlock()
		{
			return "After exploring the forest so many times, you decide to really push it, and plunge deeper and deeper into the woods. The further you go the darker it gets, " +
				"but you courageously press on. The plant-life changes too, and you spot more and more lichens and fungi, many of which are luminescent. " +
				"Finally, a wall of tree-trunks as wide as houses blocks your progress. There is a knot-hole like opening in the center, and a small sign marking it as " +
				"the entrance to the 'HighMountains'. You don't press on for now, but you could easily find your way back should you decide to later." +
				Environment.NewLine + Environment.NewLine + SafelyFormattedString.FormattedText("You've discovered the High Mountains!", StringFormats.BOLD);
		}

	}
}
