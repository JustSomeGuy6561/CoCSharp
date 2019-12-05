using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	internal interface INipple
	{
		float length { get; }
		bool blackNipples { get; }
		NippleStatus status { get; }
		bool quadNipples { get; }

		ReadOnlyPiercing<NipplePiercings> piercings { get; }
	}

	public sealed partial class Nipples : INipple
	{
		float INipple.length => length;

		bool INipple.blackNipples => blackNipples;

		NippleStatus INipple.status => nippleStatus;

		bool INipple.quadNipples => quadNipples;

		ReadOnlyPiercing<NipplePiercings> INipple.piercings => this.nipplePiercing.AsReadOnlyData();

		public static string Name()
		{
			return "Nipples";
		}

		//private static string NippleShortDescription()
		//{
		//	return "nipples";
		//}

		public string ShortDescription() => NippleData.ShortDesc();

		

		//uses the creature, so it can't be a fullDescription by syntax. also not the player description b/c it'll work for all creatures and original worked for 
		//all creatures. so it gets a unique name. c'est la vie.
		public string CreatureDescription()
		{

			//start with noun, so we can simply say return adjective+noun as we find the adjective of choice. no need to store the description, which
			//can get complicated if you mess up somewhere.

			//NOTE: creature may be null, so always be safe with it. the elvis operator (?.) will work for the most part, though you may need the null coalescing operator too (??)
			//additionally, if you know for a fact it can't be null b/c you've reached a point where it could not be null for you to get here, you can simply call it normally.
			//error on the side of caution - there's no real cost to saying if (creature != null) creature.value or the equivalent creature?.value 


			string noun;
			int choice = Utils.Rand(5);

			//60% chance for non-normal nipple status.
			if (choice < 3 && nippleStatus != NippleStatus.NORMAL)
			{
				if (nippleStatus == NippleStatus.DICK_NIPPLE)
				{
					noun = "dick-nipple";
				}

				else if (choice == 0)
				{
					if (nippleStatus == NippleStatus.FUCKABLE)
					{
						noun = "fuckable nip";
					}
					else //if (nippleStatus.IsInverted())
					{
						noun = "inverted nip";
					}
				}
				else if (choice == 1)
				{
					if (nippleStatus == NippleStatus.FUCKABLE)
					{
						noun = "nipple-hole";
					}
					else //if (nippleStatus.IsInverted())
					{
						noun = "inward-facing nipple";
					}
				}
				else
				{
					if (nippleStatus == NippleStatus.FUCKABLE)
					{
						noun = "nipple-cunt";
					}
					else if (nippleStatus == NippleStatus.FULLY_INVERTED)
					{
						noun = "inverted nipple";
					}
					else
					{
						noun = "slightly inverted nipple";
					}
				}
			}
			//if normal nipple status: 40% chance for teat if lactating.
			else if (choice < 2 && creature?.genitals.lactationRate >= 1.5)
			{
				noun = "teat";
			}
			//20% chance a non-inverted nipple will get this special description
			else if (choice == 3 && !nippleStatus.IsInverted())
			{
				noun = length < .5 ? "perky nipple" : "cherry-like nub";
			}
			//default case. Occurs naturally 20% of the time, and any time the we roll something we don't have.
			else
			{
				noun = "nipple";
			}

			int randVal = Utils.Rand(3);

			//adjectives:
			//33.3% we desribe special things (pierced, gooey, black) if possible
			//33.3% we desribe arousal related things (lactating, fuckable, just plain aroused) if possible.

			//80% of the remaining amount uses the size desriptors. this remaining amount ranges from 33.3% of the total, if a special condition is possible 
			//and an arousal condition is possible, to 100% of the total if none of the special or arousal thing are possible.
			//the last 20% of the remaining amount simply does not have an adjective.

			//the original odds were conviluted to say the least, with mixes of  anywhere from 0-75% available in some cases.

			bool isLactating = creature?.genitals.isLactating ?? false;


			if (randVal == 0 && (isPierced || creature?.body.type == BodyType.GOO || blackNipples))
			{
				if (isPierced)
				{
					var leftHorJewelry = nipplePiercing[NipplePiercings.LEFT_HORIZONTAL];
					//if (piercing is nipple chain) return "chained " + nount;
					return "pierced " + noun;
				}
				else if (creature?.body.type == BodyType.GOO)
				{
					return Utils.RandomChoice("slime-slick ", "goopy ", "slippery ") + noun;
				}
				else
				{
					return Utils.RandomChoice("black ", "ebony ", "sable ") + noun;
				}
			}
			else if (randVal == 1 && (isLactating || nippleStatus == NippleStatus.DICK_NIPPLE || nippleStatus == NippleStatus.FUCKABLE || creature?.relativeLust >= 50))
			{
				//Fuckable chance first!
				if (nippleStatus == NippleStatus.FUCKABLE)
				{
					//Fuckable and lactating?
					if (isLactating)
					{
						return Utils.RandomChoice("milk-lubricated ", "lactating ", "lactating ", "milk-slicked ", "milky ") + noun;
					}
					//Just fuckable
					else
					{
						return Utils.RandomChoice("wet ", "mutated ", "slimy ", "damp ", "moist ", "slippery ", "oozing ", "sloppy ", "dewy ") + noun;
					}
				}
				else if (nippleStatus == NippleStatus.DICK_NIPPLE)
				{
					if (isLactating)
					{
						return Utils.RandomChoice("milk-lubricated ", "lactating ", "lactating ", "milk-slicked ", "milky ") + "dick-" + noun;
					}
					//Just dick-nipple
					else
					{
						return Utils.RandomChoice("mutant ", "", "large ", "", "upward-curving ") + "dick-" + noun;
					}
				}
				//Just lactating!
				else if (isLactating)
				{
					//creature is not null or islactating would be false.
					var lactationState = creature.genitals.lactationStatus;
					//Light lactation
					if (lactationState < LactationStatus.MODERATE)
					{
						return Utils.RandomChoice("milk moistened ", "slightly lactating ", "milk-dampened ") + noun;
					}
					//Moderate lactation
					else if (lactationState < LactationStatus.STRONG)
					{
						return Utils.RandomChoice("lactating ", "milky ", "milk-seeping ") + noun;
					}
					//Heavy lactation
					else
					{
						return Utils.RandomChoice("dripping ", "dribbling ", "milk-leaking ", "drooling ") + noun;
					}
				}
				//horny, none of the above
				else
				{
					//if we got here, it means every other check in our initial if statement was false. the only way we'd reach this is if 
					//creature is not null. 
					if (creature.relativeLust >= 75)
					{
						return Utils.RandomChoice("throbbing ", "trembling ", "needy ", "throbbing ") + noun;
					}
					else //(creature.relativeLust >= 50)
					{
						return Utils.RandomChoice("erect ", "perky ", "erect ", "firm ", "tender ") + noun;
					}

				}
			}
			else if (Utils.Rand(5) != 0) //randVal is a 1 or some above condition failed. Procs 
			{
				return NippleData.NippleSizeAdjective(length) + noun;
			}
			else
			{
				return noun;
			}
		}

		private string NipplesLessInvertedDueToPiercingInThem(bool hasOtherBreastRows)
		{
			string otherStr = "";
			if (hasOtherBreastRows)
			{
				otherStr = " A quick check tells you your remaining nipples seem to have followed suit. That's curious.";
			}
			return "You notice the tug from the jewelry in your " + ShortDescription() + " has lessened slightly, and after close inspection you can confirm your nipples are less inverted." + otherStr;
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
	}

	public partial class NippleData : INipple
	{
		float INipple.length => length;

		bool INipple.blackNipples => blackNipples;

		NippleStatus INipple.status => status;

		bool INipple.quadNipples => quadNipples;

		ReadOnlyPiercing<NipplePiercings> INipple.piercings => nipplePiercings;

		public string ShortDescription() => ShortDesc();

		public string LongDescription(bool preciseMeasurements, bool withArticle) => Desc(this, withArticle, preciseMeasurements, false);

		public string FullDescription(bool preciseMeasurements, bool withArticle) => Desc(this, withArticle, preciseMeasurements, true);

		internal static string ShortDesc()
		{
			return "nipples";
		}

		internal static string Desc(INipple nipple, bool withArticle, bool preciseMeasurements, bool full)
		{

			string retVal = "";
			if (full)
			{
				retVal += nipple.piercings.isPierced ? " pierced" : "";
			}
			if (nipple.blackNipples)
			{
				if (!string.IsNullOrWhiteSpace(retVal))
				{
					retVal += ", ";
				}
				retVal = " black";
			}
			if (nipple.status.IsInverted())
			{
				if (!string.IsNullOrWhiteSpace(retVal))
				{
					retVal += ", ";
				}
				retVal += (nipple.status == NippleStatus.SLIGHTLY_INVERTED ? "slightly-" : "") + "inverted";
			}
			if (!withArticle)
			{
				retVal += nipple.quadNipples ? "quad " : "";
			}
			if (nipple.status == NippleStatus.DICK_NIPPLE)
			{
				retVal += "dick-nipple";
			}
			else if (nipple.status == NippleStatus.FUCKABLE)
			{
				retVal += "nipple-cunt";
			}
			else
			{
				retVal += "nipple";
			}

			if (preciseMeasurements)
			{
				if (withArticle)
				{
					return (nipple.quadNipples ? "a " : "four ") + Measurement.ToNearestQuarterInchOrMillimeter(nipple.length, false) + retVal + (nipple.quadNipples ? "s" : "");
				}
				else
				{
					return Measurement.ToNearestQuarterInchOrMillimeter(nipple.length, false) + retVal + "s";
				}
			}
			else
			{
				if (withArticle)
				{
					return (nipple.quadNipples ? "a " : "four ") + NippleSizeAdjective(nipple.length) + retVal + (nipple.quadNipples ? "s" : "");
				}
				else
				{
					return NippleSizeAdjective(nipple.length) + retVal + "s";
				}
			}
		}

		public static string NippleSizeAdjective(float length)
		{
			//TINAHHHH
			if (length < .25)
			{
				return Utils.RandomChoice("tiny ",
					"itty-bitty ",
					"teeny-tiny ",
					"dainty ");
			}
			else if (length < 0.4)
			{
				return "";
			}
			//
			//Prominant
			else if (length < 1)
			{
				return Utils.RandomChoice("prominent ",
					"pencil eraser-sized ",
					"eye-catching ",
					"pronounced ",
					"striking ");
			}
			//Big 'uns
			else if (length < 2)
			{
				return Utils.RandomChoice("forwards-jutting ",
					"over-sized ",
					"fleshy ",
					"large protruding ");
			}
			//'Uge
			else if (length < 3.2)
			{
				return Utils.RandomChoice("elongated ",
					"massive ",
					"awkward ",
					"lavish ",
					"hefty ");
			}
			//Massive
			else //if (length >= 3.2)
			{
				return Utils.RandomChoice("bulky ",
					"ponderous ",
					"thumb-sized ",
					"cock-sized ",
					"cow-like ");
			}
		}
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
