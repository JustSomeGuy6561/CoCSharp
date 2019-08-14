//Tough.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:47 PM

using System;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Tough : EndowmentPerkBase
	{
		public static string ToughStr()
		{
			return "Tough";
		}
		private static string ToughBtn()
		{
			return "Tough";
		}
		private static string ToughHint()
		{
			return "Are you unusually tough? (+5 Toughness)" + Environment.NewLine + Environment.NewLine +
				"Toughness gives you more HP and increases the chances an attack against you will fail to wound you.";
		}
		private static string ToughDesc()
		{
			return "Increases minimum toughness. Gains toughness 25% faster.";
		}
	}
}