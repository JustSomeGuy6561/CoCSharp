//FemininityFertilityEtcity.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 4:08 PM
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.BodyParts
{
#warning Just like build, fertility and femininity don't have any descriptors i could find. consider adding a few?
	public partial class Fertility
	{
		public static string Name()
		{
			return "Fertility";
		}
	}

	public partial class Femininity
	{
		public static string Name()
		{
			return "Femininity";
		}

		//diff: how did our femininity change? positive means more female, negative means more male. 0 means no change.

		private string FemininityChangedDueToGenderHormonesStr(short diff)
		{
			//NOTE: it's possible to have different text for M/F/H/G, but the base game didn't. And oddly enough, the way this is designed right now lets us
			//just use diff to determine what to display. 100% planned.
			//if we became more masculine
			if (diff > 0)
			{
				return Environment.NewLine + SafelyFormattedString.FormattedText("Your incredibly masculine, chiseled features become a little bit softer " +
					"from your body's changing hormones.", StringFormats.BOLD) + Environment.NewLine;
			}
			//if we became more feminine.
			else if (diff < 0)
			{
				return Environment.NewLine + SafelyFormattedString.FormattedText("You find your overly feminine face loses a little bit of its former female " +
					"beauty due to your body's changing hormones.", StringFormats.BOLD) + Environment.NewLine;
			}
			else return "";
		}
	}
}
