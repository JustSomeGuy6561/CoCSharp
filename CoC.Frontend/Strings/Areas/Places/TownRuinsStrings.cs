//TownRuinsStrings.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:24 AM
using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Areas.Places
{
	internal sealed partial class TownRuins
	{
		private static string TownRuinsName()
		{
			return "TownRuins";
		}
		private static string TownRuinsUnlockText()
		{
			return "You follow the overgrown path inland, away from the shore of the lake. You pass through thick trees, struggling not to lose the path, " +
				"before finally reaching what is clearly the end. In front of you lie crumbling walls, broken and scattered by the wind and rain... " +
				"and by other forces entirely. Beyond them are houses that have been torn apart, burned or collapsed. This was clearly once a village, " +
				"but it was devastated at some point in the past. Demon attack is the first possibility that leaps into your mind. You examine the ruins for a " +
				"time, and then decide to head back to camp. You don't think it would be wise to investigate here without preparing first." + Environment.NewLine +
				Environment.NewLine + "(" + SafelyFormattedString.FormattedText("\"TownRuins\" added to Places menu.", StringFormats.BOLD) + ")";
		}
	}
}
