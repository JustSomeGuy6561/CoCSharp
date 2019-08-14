//OasisTowerStrings.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:24 AM
using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Areas.Places
{
	internal sealed partial class OasisTower
	{
		private static string OasisTowerName()
		{
			return "Oasis Tower";
		}
		private static string OasisTowerUnlockText()
		{
			return SafelyFormattedString.FormattedText("(You have visited the tower enough times to be able to remember where to go. " +
				"Unlocked Oasis Tower in Places menu!)", StringFormats.BOLD);
		}

	}
}
