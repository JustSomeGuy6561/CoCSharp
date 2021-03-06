﻿//FemininityFertilityEtcity.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 4:08 PM
using CoC.Backend.Strings;
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

		public string FemininityChangedText(FemininityData oldFemininity)
		{
			var strength = value - oldFemininity.value;
			 if (strength == 0)
			{
				return "";
			}
			//See if a change happened!
			if (ThresholdChanged(oldFemininity))
            {
				//Gain fem?
				if (strength > 0)
				{
					return GlobalStrings.NewParagraph() + "<b>Your facial features soften as your body becomes more feminine. (+" + strength + ")</b>";
				}
				else
				{
					strength *= -1;
					return GlobalStrings.NewParagraph() + "<b>Your facial features harden as your body becomes more masculine. (+" + strength + ")</b>";
				}
			}
            //Barely noticable change!
            else
            {
                if (strength > 0)
				{
					return GlobalStrings.NewParagraph() + "Your face tingles slightly as it changes imperceptibly towards being more feminine. (+" + strength + ")";
				}
				else
				{
					strength *= -1;
					return GlobalStrings.NewParagraph() + "Your face tingles slightly as it changes imperceptibly towards being more masculine. (+" + strength + ")";
				}
			}
		}
	}
}
