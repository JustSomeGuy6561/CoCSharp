//BreastNippleStrings.cs
//Description:
//Author: JustSomeGuy
//1/10/2019, 9:07 PM
using CoC.Backend.Creatures;
using CoC.Backend.Tools;

namespace   CoC.Backend.BodyParts
{

	public sealed partial class Nipples
	{
		//private static string NippleShortDescription()
		//{
		//	return "nipples";
		//}
		private string shortDesc()
		{
			string retVal = "";
			retVal += Measurement.ToNearestQuarterInchOrMillimeter(length, false);
			retVal += blackNipples ? "black" : "";
			if (nippleStatus.IsInverted())
			{
				if (string.IsNullOrWhiteSpace(retVal))
				{
					retVal += ", ";
				}
				retVal += (nippleStatus == NippleStatus.SLIGHTLY_INVERTED ? "slightly-" : "") + "inverted";
			}
			retVal += quadNipples ? "quad " : "";
			if (nippleStatus == NippleStatus.DICK_NIPPLE)
			{
				retVal += "dick-nipples";
			}
			else if (nippleStatus == NippleStatus.FUCKABLE)
			{
				retVal += "nipple-cunts";
			}
			else
			{
				retVal += "nipples";
			}
			return retVal;
		}

		private string fullDesc(Creature creature)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string NipplesLessInvertedDueToPiercingInThem(bool hasOtherBreastRows)
		{
			string otherStr = "";
			if (hasOtherBreastRows)
			{
				otherStr = " A quick check tells you your remaining nipples seem to have followed suit. That's curious.";
			}
			return "You notice the tug from the jewelry in your " + shortDesc() + " has lessened slightly, and after close inspection you can confirm your nipples are less inverted." + otherStr;
		}

		private string NipplesNoLongerInvertedDueToPiercingInThem(bool hasOtherBreastRows)
		{
			string otherStr = "";
			if (hasOtherBreastRows)
			{
				otherStr = " Not to be outdone, your remaining nipples seem to have followed suit.";
			}
			return "You pause for a moment as your nipples seem a bit more sensitive than normal. You quickly locate the cause - it seems your piercings have done their work, " +
				"as your nipples are no longer inverted!" + otherStr;
			
		}
		//	private string fullDesc(Creature creature)
		//	{
		//		var description:String = "";
		//		var options:Array;
		//		int randVal = Utils.Rand(3);

		//		//adjectives:
		//		//33.3% we desribe special things (pierced, gooey, black) if possible
		//		//33.3% we desribe arousal related things (lactating, fuckable, just plain aroused) if possible.

		//		//80% of the remaining amount uses the size desriptors. this remaining amount ranges from 33.3% of the total, if a special condition is possible and an arousal condition is possible, 
		//		//to 100% of the total if none of the special or arousal thing are possible.
		//		//the last 20% of the remaining amount simply does not have an adjective.

		//		//the original odds were conviluted to say the least, with mixes of  anywhere from 0-75% available in some cases.

		//		if (randVal == 0 && (nipplePiercing.isPierced || creature.body.type == BodyType.GOO || blackNipples))
		//		{

		//		}
		//		else if (randVal == 1 && (isLactating || nippleStatus == NippleStatus.DICK_NIPPLE || nippleStatus == NippleStatus.FUCKABLE || creature.lust100 >= 50))
		//		{
		//			//Fuckable chance first!
		//			if (nippleStatus == NippleStatus.FUCKABLE)
		//			{
		//				//Fuckable and lactating?
		//				if (isLactating)
		//				{
		//					options = ["milk-lubricated ",
		//						"lactating ",
		//						"lactating ",
		//						"milk-slicked ",
		//						"milky "];
		//					description += Utils.RandomChoice(options);
		//				}
		//				//Just fuckable
		//				else
		//				{
		//					options = ["wet ",
		//						"mutated ",
		//						"slimy ",
		//						"damp ",
		//						"moist ",
		//						"slippery ",
		//						"oozing ",
		//						"sloppy ",
		//						"dewy "];
		//					description += Utils.RandomChoice(options);
		//				}
		//				haveDescription = true;
		//			}
		//			else if (nippleStatus == NippleStatus.DICK_NIPPLE)
		//			{

		//			}
		//			//Just lactating!
		//			else //if (isLactating)
		//			{
		//				//Light lactation
		//				if (i_creature.biggestLactation() <= 1)
		//				{
		//					options = ["milk moistened ",
		//						"slightly lactating ",
		//						"milk-dampened "];
		//					description += Utils.RandomChoice(options);
		//				}
		//				//Moderate lactation
		//				if (i_creature.biggestLactation() > 1 && i_creature.biggestLactation() <= 2)
		//				{
		//					options = ["lactating ",
		//						"milky ",
		//						"milk-seeping "];
		//					description += Utils.RandomChoice(options);
		//				}
		//				//Heavy lactation
		//				if (i_creature.biggestLactation() > 2)
		//				{
		//					options = ["dripping ",
		//						"dribbling ",
		//						"milk-leaking ",
		//						"drooling "];
		//					description += Utils.RandomChoice(options);
		//				}
		//			}
		//			else if (Utils.Rand(5) != 0) //randVal is a 2 or some above condition failed. Procs 
		//			{
		//				//TINAHHHH
		//				if (length < .25)
		//				{
		//					options = ["tiny ",
		//						"itty-bitty ",
		//						"teeny-tiny ",
		//						"dainty "];
		//					description += Utils.RandomChoice(options);
		//				}
		//				//
		//				//Prominant
		//				if (i_creature.nippleLength >= .4 && i_creature.nippleLength < 1)
		//				{
		//					options = ["prominent ",
		//						"pencil eraser-sized ",
		//						"eye-catching ",
		//						"pronounced ",
		//						"striking "];
		//					description += Utils.RandomChoice(options);
		//				}
		//				//Big 'uns
		//				if (i_creature.nippleLength >= 1 && i_creature.nippleLength < 2)
		//				{
		//					options = ["forwards-jutting ",
		//						"over-sized ",
		//						"fleshy ",
		//						"large protruding "];
		//					description += Utils.RandomChoice(options);
		//				}
		//				//'Uge
		//				if (i_creature.nippleLength >= 2 && i_creature.nippleLength < 3.2)
		//				{
		//					options = ["elongated ",
		//						"massive ",
		//						"awkward ",
		//						"lavish ",
		//						"hefty "];
		//					description += Utils.RandomChoice(options);
		//				}
		//				//Massive
		//				if (i_creature.nippleLength >= 3.2)
		//				{
		//					options = ["bulky ",
		//						"ponderous ",
		//						"thumb-sized ",
		//						"cock-sized ",
		//						"cow-like "];
		//					description += Utils.RandomChoice(options);
		//				}
		//			}
		//			//Milkiness/Arousal/Wetness Descriptors 33% of the time
		//			//25% 3/12
		//			else if (randVal < 4)
		//			{

		//				haveDescription = true;


		//			}
		//			//~16.67% 2/12
		//			else if (creatur.lust100 >= 50)
		//			{
		//				if (i_creature.lust100 > 50 && i_creature.lust100 < 75)
		//				{
		//					options = ["erect ",
		//						"perky ",
		//						"erect ",
		//						"firm ",
		//						"tender "];
		//					description += Utils.RandomChoice(options);
		//					haveDescription = true;
		//				}
		//				if (i_creature.lust100 >= 75)
		//				{
		//					options = ["throbbing ",
		//						"trembling ",
		//						"needy ",
		//						"throbbing "];
		//					description += Utils.RandomChoice(options);
		//					haveDescription = true;
		//				}
		//			}
		//			//IF PIERCED: 2/12 or 3/12 if lust below 50%
		//			else if (nipplePiercing.isPierced && randVal < 5)
		//			{
		//				if (i_creature.nipplesPierced == 5) description += "chained ";
		//				else description += "pierced ";
		//				haveDescription = true;
		//			}
		//			//if GOOEY: Guarenteed. overall: 2/12 if pierced, lust >=50%. 3/12 if pierced, lust <50%. 4/12 if not pierced, lust >=50%. 6/12 if not pierced, lust < 50%.
		//			//guarenteed if has goo skin. and fallen to this point. best case: 50%, worst case: 16.67%
		//			else if (!haveDescription && i_creature.hasGooSkin())
		//			{
		//				options = ["slime-slick ",
		//					"goopy ",
		//					"slippery "];
		//				description += Utils.RandomChoice(options);
		//			}
		//			//IF NOT GOOEY AND BLACK_NIPPLES: guarenteed. Overall: 2/12 if pierced, lust >=50%. 3/12 if pierced, lust <50%. 4/12 if not pierced, lust >=50%. 6/12 if not pierced, lust < 50%.
		//			else if (!haveDescription && i_creature.hasStatusEffect(StatusEffects.BlackNipples))
		//			{
		//				options = ["black ",
		//					"ebony ",
		//					"sable "];
		//				description += Utils.RandomChoice(options);
		//			}
		//		//nouns
		//		var choice:int = 0;
		//		choice = rand(5);
		//		if (choice === 0) description += "nipple";
		//		if (choice === 1)
		//		{
		//			if (i_creature.nippleLength < .5) description += "perky nipple";
		//			else description += "cherry-like nub";
		//		}
		//		if (choice === 2)
		//		{
		//			if (i_creature.hasFuckableNipples()) description += "fuckable nip";
		//			else
		//			{
		//				if (i_creature.biggestLactation() >= 1 && i_creature.nippleLength >= 1) description += "teat";
		//				else description += "nipple";
		//			}
		//		}
		//		if (choice === 3)
		//		{
		//			if (i_creature.hasFuckableNipples()) description += "nipple-hole";
		//			else
		//			{
		//				if (i_creature.biggestLactation() >= 1 && i_creature.nippleLength >= 1) description += "teat";
		//				else description += "nipple";
		//			}
		//		}
		//		if (choice === 4)
		//		{
		//			if (i_creature.hasFuckableNipples()) description += "nipple-cunt";
		//			else description += "nipple";
		//		}
		//		return description;
		//	}

		//}
	}
	public static class NippleHelpers
	{
		public static bool IsInverted(this NippleStatus nippleStatus)
		{
			return nippleStatus == NippleStatus.FULLY_INVERTED || nippleStatus == NippleStatus.SLIGHTLY_INVERTED;
		}

		public static string AsText(this NippleStatus nippleStatus)
		{
			switch (nippleStatus)
			{
				case NippleStatus.FULLY_INVERTED: return "fully inverted";
				case NippleStatus.SLIGHTLY_INVERTED: return "slightly inverted";
				case NippleStatus.FUCKABLE: return "fuckable";
				case NippleStatus.DICK_NIPPLE: return "dick-nipple";

				case NippleStatus.NORMAL:
				default:
					return "normal";

			}
		}
	}
}
