//Lusty.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:47 PM

using System;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Lusty : EndowmentPerkBase
	{
		private static string LustyStr()
		{
			return "Lusty";
		}
		private static string LustyBtn()
		{
			return "Libido";
		}
		private static string LustyHint()
		{
			return "Do you have an unusually high sex-drive? (+5 Libido)" + Environment.NewLine + Environment.NewLine + 
				"Libido affects how quickly your lust builds over time. You may find a high libido to be more trouble than it's worth...";
		}
		private static string LustyDesc()
		{
			return "Increases minimum libido. Gains lust 25% faster.";
		}
	}
}