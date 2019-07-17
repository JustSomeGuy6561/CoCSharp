//Strong.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:47 PM

using System;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Strong : EndowmentPerkBase
	{
		private static string StrongStr()
		{
			return "Strong";
		}
		private static string StrongBtn()
		{
			return "Strong";
		}
		private static string StrongHint()
		{
			return "Are you stronger than normal? (+5 Strength)" + Environment.NewLine + Environment.NewLine +
				"Strength increases your combat damage, and your ability to hold on to an enemy or pull yourself away.";
		}
		private static string StrongDesc()
		{
			return "Increases minimum strength. Gains strength 25% faster.";
		}
	}
}
