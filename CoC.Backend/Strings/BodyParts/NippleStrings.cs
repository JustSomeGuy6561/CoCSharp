using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	internal interface INipple
	{
		float length { get; }
		bool blackNipples { get; }
		NippleStatus status { get; }
		bool quadNipples { get; }

		ReadOnlyPiercing<NipplePiercings> piercings { get; }

		LactationStatus lactationStatus { get; }
		float lactationRate { get; }

		float relativeLust { get; }

		BodyType bodyType { get; }

	}
	public sealed partial class Nipples : INipple
	{
		float INipple.length => length;

		bool INipple.blackNipples => blackNipples;

		NippleStatus INipple.status => nippleStatus;

		bool INipple.quadNipples => quadNipples;

		BodyType INipple.bodyType => bodyType;

		ReadOnlyPiercing<NipplePiercings> INipple.piercings => this.nipplePiercing.AsReadOnlyData();

		public static string Name()
		{
			return "Nipples";
		}

		//private static string NippleShortDescription()
		//{
		//	return "nipples";
		//}

		public string NounText() => NippleStrings.NippleNoun(this, true, false);
		public string NounText(bool plural, bool allowQuadNippleIfApplicable = false) => NippleStrings.NippleNoun(this, plural, allowQuadNippleIfApplicable);

		public string ShortDescription() => NippleStrings.ShortDescription(this, true, true);

		public string ShortDescription(bool plural, bool allowQuadNippleTextIfApplicable = true) => NippleStrings.ShortDescription(this, plural, allowQuadNippleTextIfApplicable);

		public string LongDescription(bool alternateFormat = false, bool usePreciseMeasurements = false)
		{
			return NippleStrings.Desc(this, alternateFormat, usePreciseMeasurements, false, false, false);
		}

		public string FullDescription(bool alternateFormat = false, bool usePreciseMeasurements = false)
		{
			return NippleStrings.Desc(this, alternateFormat, usePreciseMeasurements, false, false, true);
		}

		public string LongDescription(bool alternateFormat, bool usePreciseMeasurements, bool singleNipple, bool singleNippleIgnoresQuadNipple)
		{
			return NippleStrings.Desc(this, alternateFormat, usePreciseMeasurements, singleNipple, singleNippleIgnoresQuadNipple, false);
		}

		public string FullDescription(bool alternateFormat, bool usePreciseMeasurements, bool singleNipple, bool singleNippleIgnoresQuadNipple)
		{
			return NippleStrings.Desc(this, alternateFormat, usePreciseMeasurements, singleNipple, singleNippleIgnoresQuadNipple, true);
		}

		public string SingleNippleLongDescription(bool alternateFormat, bool usePreciseMeasurements = false, bool onlyOneNippleIfQuadNipples = false)
		{
			return NippleStrings.Desc(this, alternateFormat, usePreciseMeasurements, true, onlyOneNippleIfQuadNipples, false);
		}

		public string SingleNippleFullDescription(bool alternateFormat, bool usePreciseMeasurements = false, bool onlyOneNippleIfQuadNipples = false)
		{
			return NippleStrings.Desc(this, alternateFormat, usePreciseMeasurements, true, onlyOneNippleIfQuadNipples, false);
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

		LactationStatus INipple.lactationStatus => lactationStatus;

		float INipple.lactationRate => lactationRate;

		float INipple.relativeLust => relativeLust;

		BodyType INipple.bodyType => bodyType;


		public string NounText() => NippleStrings.NippleNoun(this, true, false);
		public string NounText(bool plural, bool allowQuadNippleIfApplicable = false) => NippleStrings.NippleNoun(this, plural, allowQuadNippleIfApplicable);

		public string ShortDescription() => NippleStrings.ShortDescription(this, true, true);

		public string ShortDescription(bool plural, bool allowQuadNippleTextIfApplicable = true) => NippleStrings.ShortDescription(this, plural, allowQuadNippleTextIfApplicable);

		public string LongDescription(bool alternateFormat, bool usePreciseMeasurements = false)
		{
			return NippleStrings.Desc(this, alternateFormat, usePreciseMeasurements, false, false, false);
		}

		public string FullDescription(bool alternateFormat, bool usePreciseMeasurements = false)
		{
			return NippleStrings.Desc(this, alternateFormat, usePreciseMeasurements, false, false, true);
		}

		public string LongDescription(bool alternateFormat, bool usePreciseMeasurements, bool singleNipple, bool singleNippleIgnoresQuadNipple)
		{
			return NippleStrings.Desc(this, alternateFormat, usePreciseMeasurements, singleNipple, singleNippleIgnoresQuadNipple, false);
		}

		public string FullDescription(bool alternateFormat, bool usePreciseMeasurements, bool singleNipple, bool singleNippleIgnoresQuadNipple)
		{
			return NippleStrings.Desc(this, alternateFormat, usePreciseMeasurements, singleNipple, singleNippleIgnoresQuadNipple, true);
		}

		public string SingleNippleLongDescription(bool alternateFormat, bool usePreciseMeasurements = false, bool onlyOneNippleIfQuadNipples = false)
		{
			return NippleStrings.Desc(this, alternateFormat, usePreciseMeasurements, true, onlyOneNippleIfQuadNipples, false);
		}

		public string SingleNippleFullDescription(bool alternateFormat, bool usePreciseMeasurements = false, bool onlyOneNippleIfQuadNipples = false)
		{
			return NippleStrings.Desc(this, alternateFormat, usePreciseMeasurements, true, onlyOneNippleIfQuadNipples, false);
		}
	}

	internal static class NippleStrings
	{
		internal static string NippleNoun(INipple nipple, bool plural, bool allowQuadNippleText)
		{
			int choice = Utils.Rand(5);

			bool doQuadText = allowQuadNippleText && nipple.quadNipples && Utils.Rand(4) < 3;

			//60% chance for non-normal nipple status.
			if (choice < 3 && nipple.status != NippleStatus.NORMAL)
			{
				if (nipple.status == NippleStatus.DICK_NIPPLE)
				{
					if (doQuadText)
					{
						return Utils.Pluralize("quad-dicked nipple", plural);
					}
					else
					{
						return Utils.Pluralize("dick-nipple", plural);
					}
				}

				else if (choice == 0)
				{
					if (nipple.status == NippleStatus.FUCKABLE)
					{
						if (doQuadText)
						{
							return Utils.Pluralize("fuckable quad-nip", plural);
						}
						else
						{
							return Utils.Pluralize("fuckable nip", plural);
						}
					}
					else //if (nipple.status.IsInverted())
					{
						if (doQuadText)
						{
							return Utils.Pluralize("inverted quad-nip", plural);
						}
						return Utils.Pluralize("inverted nip", plural);
					}
				}
				else if (choice == 1)
				{
					if (nipple.status == NippleStatus.FUCKABLE)
					{
						if (doQuadText)
						{
							return Utils.Pluralize("quad-holed nipple", plural);
						}
						return Utils.Pluralize("nipple-hole", plural);
					}
					else //if (nipple.status.IsInverted())
					{
						if (doQuadText)
						{
							return Utils.Pluralize("inward-facing quad-nipple", plural);
						}
						return Utils.Pluralize("inward-facing nipple", plural);
					}
				}
				else
				{
					if (nipple.status == NippleStatus.FUCKABLE)
					{
						if (doQuadText)
						{
							return Utils.Pluralize("quad-cunt nipple", plural);
						}
						return Utils.Pluralize("nipple-cunt", plural);
					}
					else if (nipple.status == NippleStatus.FULLY_INVERTED)
					{
						if (doQuadText)
						{
							return Utils.Pluralize("inverted quad-nipple", plural);
						}
						return Utils.Pluralize("inverted nipple", plural);
					}
					else
					{
						if (doQuadText)
						{
							return Utils.Pluralize("slightly inverted quad-nipple", plural);
						}
						return Utils.Pluralize("slightly inverted nipple", plural);
					}
				}
			}
			//if normal nipple status: 40% chance for teat if lactating.
			else if (choice < 2 && nipple.lactationRate >= 1.5)
			{
				string intro = doQuadText ? "quad-" : "";
				return intro + Utils.Pluralize("teat", plural);
			}
			//20% chance a non-inverted nipple will get this special description
			else if (choice == 3 && !nipple.status.IsInverted())
			{
				if (nipple.length < 0.5 && doQuadText)
				{
					return Utils.Pluralize("perky quad-nipple", plural);
				}
				else if (nipple.length < 0.5)
				{
					return Utils.Pluralize("perky nipple", plural);
				}
				else if (doQuadText)
				{
					return Utils.Pluralize("four-pronged, cherry-like nub", plural);
				}
				return Utils.Pluralize("cherry-like nub", plural);
			}
			//default case. Occurs naturally 20% of the time, and any time the we roll something we don't have.
			else if (doQuadText)
			{
				return Utils.Pluralize(Utils.RandomChoice("quad-nipple", "quad-nipple", "quad-nipple", "four-pronged nipple", "four-tipped nipple", "quad-tipped nipple"), plural);
			}
			return Utils.Pluralize("nipple", plural);
		}

		public static string ShortDescription(INipple nipple, bool plural, bool allowQuadNippleText)
		{

			//start with noun, so we can simply say return adjective+noun as we find the adjective of choice. no need to store the description, which
			//can get complicated if you mess up somewhere.

			string noun = NippleNoun(nipple, allowQuadNippleText, plural);


			int randVal = Utils.Rand(3);

			//adjectives:
			//33.3% we desribe special things (pierced, gooey, black) if possible
			//33.3% we desribe arousal related things (lactating, fuckable, just plain aroused) if possible.

			//80% of the remaining amount uses the size desriptors. this remaining amount ranges from 33.3% of the total, if a special condition is possible
			//and an arousal condition is possible, to 100% of the total if none of the special or arousal thing are possible.
			//the last 20% of the remaining amount simply does not have an adjective.

			//the original odds were conviluted to say the least, with mixes of  anywhere from 0-75% available in some cases.

			bool isLactating = nipple.lactationStatus > LactationStatus.NOT_LACTATING;
			bool wearingJewelry = nipple.piercings.wearingJewelry;

			if (randVal == 0 && (wearingJewelry || nipple.bodyType == BodyType.GOO || nipple.blackNipples))
			{
				if (wearingJewelry)
				{
					var leftHorJewelry = nipple.piercings[NipplePiercings.LEFT_HORIZONTAL];
					//if (piercing is nipple chain) return "chained " + nount;
					return "pierced " + noun;
				}
				else if (nipple.bodyType == BodyType.GOO)
				{
					return Utils.RandomChoice("slime-slick ", "goopy ", "slippery ") + noun;
				}
				else
				{
					return Utils.RandomChoice("black ", "ebony ", "sable ") + noun;
				}
			}
			else if (randVal == 1 && (isLactating || nipple.status == NippleStatus.DICK_NIPPLE || nipple.status == NippleStatus.FUCKABLE || nipple.relativeLust >= 50))
			{
				//Fuckable chance first!
				if (nipple.status == NippleStatus.FUCKABLE)
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
				else if (nipple.status == NippleStatus.DICK_NIPPLE)
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
					var lactationState = nipple.lactationStatus;
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
					if (nipple.relativeLust >= 75)
					{
						return Utils.RandomChoice("throbbing ", "trembling ", "needy ", "throbbing ") + noun;
					}
					else //(nipple.relativeLust >= 50)
					{
						return Utils.RandomChoice("erect ", "perky ", "erect ", "firm ", "tender ") + noun;
					}

				}
			}
			else if (Utils.Rand(5) != 0) //randVal is a 1 or some above condition failed. Procs
			{
				return NippleStrings.NippleSizeAdjective(nipple.length) + noun;
			}
			else
			{
				return noun;
			}
		}

		internal static string Desc(INipple nipple, bool withArticle, bool preciseMeasurements, bool onlyOneNipple, bool singularIfQuadNipples, bool full)
		{
			bool allowQuadNipple = !onlyOneNipple || !singularIfQuadNipples;

			string retVal = "";
			if (full)
			{
				if (nipple.piercings.wearingJewelry)
				{
					retVal += "pierced";
				}
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
			if (nipple.lactationStatus > LactationStatus.NOT_LACTATING)
			{
				if (!string.IsNullOrWhiteSpace(retVal))
				{
					retVal += ", ";
				}

				//creature is not null or islactating would be false.
				var lactationState = nipple.lactationStatus;
				//Light lactation
				if (lactationState < LactationStatus.MODERATE)
				{
					retVal += Utils.RandomChoice("milk moistened ", "slightly lactating ", "milk-dampened ");
				}
				//Moderate lactation
				else if (lactationState < LactationStatus.STRONG)
				{
					retVal += Utils.RandomChoice("lactating ", "milky ", "milk-seeping ");
				}
				//Heavy lactation
				else
				{
					retVal += Utils.RandomChoice("dripping ", "dribbling ", "milk-leaking ", "drooling ");
				}
			}


			//lust is a fallback if nothing else displays for full or long descriptions.
			if (string.IsNullOrEmpty(retVal) && nipple.relativeLust > 50)
			{
				if (nipple.relativeLust >= 75)
				{
					return Utils.RandomChoice("throbbing ", "trembling ", "needy ", "throbbing ");
				}
				else //(nipple.relativeLust >= 50)
				{
					return Utils.RandomChoice("erect ", "perky ", "erect ", "firm ", "tender ");
				}
			}

			if (nipple.status == NippleStatus.DICK_NIPPLE)
			{
				retVal += Utils.Pluralize("dick-nipple", !onlyOneNipple);
			}
			else if (nipple.status == NippleStatus.FUCKABLE)
			{
				retVal += Utils.Pluralize("nipple-cunt", !onlyOneNipple);
			}
			else
			{
				retVal += Utils.Pluralize("nipple", !onlyOneNipple);
			}

			string intro = "";
			if (withArticle && onlyOneNipple)
			{
				intro = "a ";
			}
			if (preciseMeasurements)
			{
				return intro + Measurement.ToNearestQuarterInchOrMillimeter(nipple.length, false, false) + retVal;
			}
			else
			{
				return intro + NippleSizeAdjective(nipple.length) + retVal;
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
				return Utils.RandomChoice("forward-jutting ",
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
