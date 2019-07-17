//MessyOrgasms.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:47 PM

using System;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class MessyOrgasms : EndowmentPerkBase
	{
		private static string MessyOrgasmsStr()
		{
			return "Messy Orgasms";
		}
		private static string MessyOrgasmsBtn()
		{
			return "Lots of Jizz";
		}
		private static string MessyOrgasmsHint()
		{
			return "Are your orgasms particularly messy? (+50% Cum Multiplier)" + Environment.NewLine + Environment.NewLine + 
				"A higher cum multiplier will result in messier orgasms, and makes you more virile.";
		}
		private static string MessyOrgasmsDesc()
		{
			return "Produces 50% more cum volume.";
		}
	}
}