//Trap.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:47 PM

using System;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Trap : EndowmentPerkBase
	{
		private static string TrapStr()
		{
			return "Trap";
		}
		private static string TrapBtn()
		{
			return "Trap";
		}
		private static string TrapHint()
		{
			return "Does your body appear androgynous? (Shrinks all endowments by 1\")" + Environment.NewLine + Environment.NewLine +
				"Your body will resist effects that grow your endowments, and is more susceptible to effects that shrink them.";
		}
		private static string TrapDesc()
		{
			return "Your body naturally resists effects that grow your endowments, and enhances effects that shrink them";
		}
	}
}