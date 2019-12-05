//Fertile.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:47 PM

using System;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Fertile : EndowmentPerkBase
	{
		private static string FertileStr()
		{
			return "Fertile";
		}
		private static string FertileBtn()
		{
			return "Fertile";
		}
		private static string FertileHint()
		{
			return "Is your family particularly fertile? (+15% Fertility)" + Environment.NewLine + Environment.NewLine +
				"A high fertility will cause you to become pregnant much more easily. " +
				"Pregnancy may result in: Strange children, larger bust, larger hips, a bigger ass, and other weirdness.";
		}
		private static string FertileDesc()
		{
			return "Makes you 15% more likely to become pregnant.";
		}
	}
}
