//WetPussy.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:47 PM

using System;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class WetPussy : EndowmentPerkBase
	{
		private static string WetPussyStr()
		{
			return "Wet Pussy";
		}
		private static string WetPussyBtn()
		{
			return "Wet Vagina";
		}
		private static string WetPussyHint()
		{
			return "Does your pussy get particularly wet? (+1 Vaginal Wetness. This effect is permanent.)" + Environment.NewLine + Environment.NewLine +
				"Vaginal wetness will make it easier to take larger cocks, in turn helping you bring the well-endowed to orgasm quicker.";
		}
		private static string WetPussyDesc()
		{
			return "Keeps your pussy wet, which also provides a bonus to capacity.";
		}
	}
}
