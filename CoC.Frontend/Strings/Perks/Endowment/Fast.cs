//Fast.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:47 PM

using System;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Fast : EndowmentPerkBase
	{
		public static string FastStr()
		{
			return "Fast";
		}
		private static string FastBtn()
		{
			return "Fast";
		}
		private static string FastHint()
		{
			return "Are you very quick? (+5 Speed)" + Environment.NewLine + Environment.NewLine + "Speed makes it easier to escape combat and grapples. " +
				"It also boosts your chances of evading an enemy attack and successfully catching up to enemies who try to run.";
		}
		private static string FastDesc()
		{
			return "Increases minimum speed. Gains speed 25% faster.";
		}
	}
}
