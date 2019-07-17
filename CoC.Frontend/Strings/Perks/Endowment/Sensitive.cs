//Sensitive.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:47 PM

using System;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Sensitive : EndowmentPerkBase
	{
		private static string SensitiveStr()
		{
			return "Sensitive";
		}
		private static string SensitiveBtn()
		{
			return "Touch";
		}
		private static string SensitiveHint()
		{
			return "Is your skin unusually sensitive? (+5 Sensitivity)" + Environment.NewLine + Environment.NewLine + 
				"Sensitivity affects how easily touches and certain magics will raise your lust. Very low sensitivity will make it difficult to orgasm.";
		}
		private static string SensitiveDesc()
		{
			return "Increases minimum sensitivity. Gains sensitivity 25% faster.";
		}
	}
}