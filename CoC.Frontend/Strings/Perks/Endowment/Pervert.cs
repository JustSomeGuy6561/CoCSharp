//Pervert.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:47 PM

using System;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Pervert : EndowmentPerkBase
	{
		private static string PervertStr()
		{
			return "Pervert";
		}
		private static string PervertBtn()
		{
			return "Perversion";
		}
		private static string PervertHint()
		{
			return "Are you unusually perverted? (+5 Corruption)" + Environment.NewLine + Environment.NewLine + 
				"Corruption affects certain scenes and having a higher corruption makes you more prone to Bad Ends.";
		}
		private static string PervertDesc()
		{
			return "Gains corruption 25% faster. Reduces corruption requirement for high-corruption variant of scenes.";
		}
	}
}