using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Text;

namespace CoC.Backend.BodyParts
{
	internal interface INipple
	{
		double length { get; }
		bool blackNipples { get; }
		NippleStatus status { get; }
		bool quadNipples { get; }

		LactationStatus lactationStatus { get; }
		double lactationRate { get; }

		double relativeLust { get; }

		BodyType bodyType { get; }
	}

	partial class NippleAggregate
	{
		public static string Name()
		{
			return "Nipples";
		}

	}

	internal static class NippleStrings
	{
		internal static string NounText(INipple nipple, bool plural = true, bool allowQuadNippleText = true) => NippleNoun(nipple, plural, false, allowQuadNippleText);

		internal static string SingleFormatNippleText(INipple nipple, bool allowQuadNippleText = true) => NippleNoun(nipple, false, true, allowQuadNippleText);

		private static string NippleNoun(INipple nipple, bool plural, bool alternateFormatIfSingular, bool allowQuadNippleText)
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
						return Utils.PluralizeIf("quad-dicked nipple", plural);
					}
					else
					{
						return Utils.PluralizeIf("dick-nipple", plural);
					}
				}

				else if (choice == 0)
				{
					if (nipple.status == NippleStatus.FUCKABLE)
					{
						if (doQuadText)
						{
							return Utils.PluralizeIf("fuckable quad-nip", plural);
						}
						else
						{
							return Utils.PluralizeIf("fuckable nip", plural);
						}
					}
					else //if (nipple.status.IsInverted())
					{
						if (doQuadText)
						{
							return Utils.PluralizeIf("inverted quad-nip", plural);
						}
						return Utils.PluralizeIf("inverted nip", plural);
					}
				}
				else if (choice == 1)
				{
					if (nipple.status == NippleStatus.FUCKABLE)
					{
						if (doQuadText)
						{
							return Utils.PluralizeIf("quad-holed nipple", plural);
						}
						return Utils.PluralizeIf("nipple-hole", plural);
					}
					else //if (nipple.status.IsInverted())
					{
						if (doQuadText)
						{
							return Utils.PluralizeIf("inward-facing quad-nipple", plural);
						}
						return Utils.PluralizeIf("inward-facing nipple", plural);
					}
				}
				else
				{
					if (nipple.status == NippleStatus.FUCKABLE)
					{
						if (doQuadText)
						{
							return Utils.PluralizeIf("quad-cunt nipple", plural);
						}
						return Utils.PluralizeIf("nipple-cunt", plural);
					}
					else if (nipple.status == NippleStatus.FULLY_INVERTED)
					{
						if (doQuadText)
						{
							return Utils.PluralizeIf("inverted quad-nipple", plural);
						}
						return Utils.PluralizeIf("inverted nipple", plural);
					}
					else
					{
						if (doQuadText)
						{
							return Utils.PluralizeIf("slightly inverted quad-nipple", plural);
						}
						return Utils.PluralizeIf("slightly inverted nipple", plural);
					}
				}
			}
			//if normal nipple status: 40% chance for teat if lactating.
			else if (choice < 2 && nipple.lactationRate >= 1.5)
			{
				string intro = doQuadText ? "quad-" : "";
				return intro + Utils.PluralizeIf("teat", plural);
			}
			//20% chance a non-inverted nipple will get this special description
			else if (choice == 3 && !nipple.status.IsInverted())
			{
				if (nipple.length < 0.5 && doQuadText)
				{
					return Utils.PluralizeIf("perky quad-nipple", plural);
				}
				else if (nipple.length < 0.5)
				{
					return Utils.PluralizeIf("perky nipple", plural);
				}
				else if (doQuadText)
				{
					return Utils.PluralizeIf("four-pronged, cherry-like nub", plural);
				}
				return Utils.PluralizeIf("cherry-like nub", plural);
			}
			//default case. Occurs naturally 20% of the time, and any time the we roll something we don't have.
			else if (doQuadText)
			{
				return Utils.PluralizeIf(Utils.RandomChoice("quad-nipple", "quad-nipple", "quad-nipple", "four-pronged nipple", "four-tipped nipple", "quad-tipped nipple"), plural);
			}
			return Utils.PluralizeIf("nipple", plural);
		}

		public static string GenericShortDescription(INipple nipple, bool plural, bool allowQuadNippleText) => GenericShortDesc(nipple, plural, false, allowQuadNippleText);
		public static string ShortDescription(INipple nipple, IBreast breast, bool plural, bool allowQuadNippleText) => ShortDesc(nipple, breast, plural, false, allowQuadNippleText);

		public static string GenericSingleItemDescription(INipple nipple, bool allowQuadNippleText) => GenericShortDesc(nipple, false, true, allowQuadNippleText);
		public static string SingleItemDescription(INipple nipple, IBreast breast, bool allowQuadNippleText) => ShortDesc(nipple, breast, false, true, allowQuadNippleText);

		private static string ShortDesc(INipple nipple, IBreast breast, bool plural, bool singleMemberFormatIfNotPlural, bool allowQuadNippleText)
		{
			bool needsArticle = !plural && singleMemberFormatIfNotPlural;

			if (Utils.Rand(3) == 0 && breast.piercings.wearingJewelry)
			{
				var leftHorJewelry = breast.piercings[NipplePiercingLocation.LEFT_HORIZONTAL];
				//if (piercing is nipple chain) return "chained " + nount;

				return (needsArticle ? "a " : "") + "pierced " + NounText(nipple, plural, allowQuadNippleText);
			}
			else
			{
				return GenericShortDesc(nipple, plural, singleMemberFormatIfNotPlural, allowQuadNippleText);
			}

		}

		private static string GenericShortDesc(INipple nipple, bool plural, bool singleMemberFormatIfNotPlural, bool allowQuadNippleText)
		{

			bool needsArticle = !plural && singleMemberFormatIfNotPlural;

			//regardless of the above boolean, the noun will not need an article in nearly every instance. therefore, by default, we can simply calculate the
			//noun ahead of time and just use it when needed. technically, i could write the function call everywhere, but imo it's cleaner to write it this way.
			//when we do fall through to a case where we need an article on the noun directly, this variable will be incorrect and thus we'll need to overwrite it,
			//but it's worth trading the slight performance/memory cost (and i mean slight) for much cleaner code.
			string noun = NounText(nipple, plural, allowQuadNippleText);


			int randVal = Utils.Rand(3);

			//adjectives:
			//33.3% we desribe special things (pierced, gooey, black) if possible
			//33.3% we desribe arousal related things (lactating, fuckable, just plain aroused) if possible.

			//80% of the remaining amount uses the size desriptors. this remaining amount ranges from 33.3% of the total, if a special condition is possible
			//and an arousal condition is possible, to 100% of the total if none of the special or arousal thing are possible.
			//the last 20% of the remaining amount simply does not have an adjective.

			//the original odds were conviluted to say the least, with mixes of  anywhere from 0-75% available in some cases.

			bool isLactating = nipple.lactationStatus > LactationStatus.NOT_LACTATING;

			if (randVal == 0 && (nipple.bodyType == BodyType.GOO || nipple.blackNipples))
			{

				if (nipple.bodyType == BodyType.GOO)
				{
					return (needsArticle ? "a " : "") + Utils.RandomChoice("slime-slick ", "goopy ", "slippery ") + noun;
				}
				else
				{
					if (needsArticle) return Utils.RandomChoice("a black ", "an ebony ", "a sable ") + noun;
					else return Utils.RandomChoice("black ", "ebony ", "sable ") + noun;
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
						return (needsArticle ? "a " : "") + Utils.RandomChoice("milk-lubricated ", "lactating ", "lactating ", "milk-slicked ", "milky ") + noun;
					}
					//Just fuckable
					else
					{
						return (needsArticle ? "a " : "") + Utils.RandomChoice("wet ", "mutated ", "slimy ", "damp ", "moist ", "slippery ", "oozing ", "sloppy ", "dewy ") + noun;
					}
				}
				else if (nipple.status == NippleStatus.DICK_NIPPLE)
				{
					if (isLactating)
					{
						return (needsArticle ? "a " : "") + Utils.RandomChoice("milk-lubricated ", "lactating ", "lactating ", "milk-slicked ", "milky ") + "dick-" + noun;
					}
					//Just dick-nipple
					else
					{
						if (needsArticle) return Utils.RandomChoice("a mutant ", "a ", "a large ", "", "an upward-curving ") + "dick-" + noun;
						else return Utils.RandomChoice("mutant ", "", "large ", "", "upward-curving ") + "dick-" + noun;
					}
				}
				//Just lactating!
				else if (isLactating)
				{
					var lactationState = nipple.lactationStatus;
					//Light lactation
					if (lactationState < LactationStatus.MODERATE)
					{
						return (needsArticle ? "a " : "") + Utils.RandomChoice("milk moistened ", "slightly lactating ", "milk-dampened ") + noun;
					}
					//Moderate lactation
					else if (lactationState < LactationStatus.STRONG)
					{
						return (needsArticle ? "a " : "") + Utils.RandomChoice("lactating ", "milky ", "milk-seeping ") + noun;
					}
					//Heavy lactation
					else
					{
						return (needsArticle ? "a " : "") + Utils.RandomChoice("dripping ", "dribbling ", "milk-leaking ", "drooling ") + noun;
					}
				}
				//horny, none of the above
				else
				{
					if (nipple.relativeLust >= 75)
					{
						return (needsArticle ? "a " : "") + Utils.RandomChoice("throbbing ", "trembling ", "needy ", "throbbing ") + noun;
					}
					else //(nipple.relativeLust >= 50)
					{
						if (needsArticle) return Utils.RandomChoice("an erect ", "a perky ", "an erect ", "a firm ", "a tender ") + noun;
						else return Utils.RandomChoice("erect ", "perky ", "erect ", "firm ", "tender ") + noun;
					}

				}
			}
			else if (Utils.Rand(5) != 0) //randVal is a 1 or some above condition failed. Procs
			{
				string size = NippleStrings.NippleSizeAdjective(nipple.length, true);
				if (!string.IsNullOrEmpty(size) || !needsArticle)
				{
					return size + noun;
				}
			}

			//fall through case.
			if (!needsArticle)
			{
				return noun;
			}
			else
			{
				return SingleFormatNippleText(nipple, allowQuadNippleText);
			}

		}

		internal static string GenericLongDescription(INipple nipple, bool alternateFormat, bool plural, bool usePreciseMeasurements)
		{
			return GenericLongFullDesc(nipple, alternateFormat, plural, usePreciseMeasurements, false);

		}
		internal static string LongDescription(INipple nipple, IBreast breast, bool alternateFormat, bool plural, bool usePreciseMeasurements)
		{
			return LongFullDesc(nipple, breast, alternateFormat, plural, usePreciseMeasurements, false);
		}


		internal static string GenericFullDescription(INipple nipple, bool alternateFormat, bool plural, bool usePreciseMeasurements)
		{
			return GenericLongFullDesc(nipple, alternateFormat, plural, usePreciseMeasurements, true);
		}
		internal static string FullDescription(INipple nipple, IBreast breast, bool alternateFormat, bool plural, bool usePreciseMeasurements)
		{
			return LongFullDesc(nipple, breast, alternateFormat, plural, usePreciseMeasurements, true);
		}



		//note: if plural is set to true, with article is ignored. the alternate format for plural is identical to the regular format.
		private static string LongFullDesc(INipple nipple, IBreast breast, bool withArticle, bool allAvailableNipples, bool preciseMeasurements, bool full)
		{
			string pierced = null;
			if (breast.piercings.wearingJewelry)
			{
				pierced = "pierced";
			}

			return GenericLongFullDesc(nipple, withArticle, allAvailableNipples, preciseMeasurements, full, pierced);
		}
		private static string GenericLongFullDesc(INipple nipple, bool withArticle, bool allAvailableNipples, bool preciseMeasurements, bool full, string pierced = null)
		{
			StringBuilder sb = new StringBuilder();

			if (full && !string.IsNullOrWhiteSpace(pierced))
			{
				sb.Append(pierced);
			}

			if (nipple.blackNipples)
			{
				if (sb.Length > 0)
				{
					sb.Append(", ");
				}

				sb.Append("black");
			}

			if (nipple.lactationStatus > LactationStatus.NOT_LACTATING)
			{
				if (sb.Length > 0)
				{
					sb.Append(", ");
				}
				//creature is not null or islactating would be false.
				var lactationState = nipple.lactationStatus;
				//Light lactation
				if (lactationState < LactationStatus.MODERATE)
				{
					sb.Append(Utils.RandomChoice("milk moistened", "slightly lactating", "milk-dampened"));
				}
				//Moderate lactation
				else if (lactationState < LactationStatus.STRONG)
				{
					sb.Append(Utils.RandomChoice("lactating", "milky", "milk-seeping"));
				}
				//Heavy lactation
				else
				{
					sb.Append(Utils.RandomChoice("dripping", "dribbling", "milk-leaking", "drooling"));
				}
			}
			if (nipple.status.IsInverted())
			{
				if (sb.Length > 0)
				{
					sb.Append(", ");
				}
				//else... shit. different articles.
				if (nipple.status == NippleStatus.SLIGHTLY_INVERTED)
				{
					sb.Append("slightly-");
				}
				sb.Append("inverted");
			}

			//check if we've added any adjectives. if we have, the length is > 0. if not, try using lust. if that doesn't work, we don't have any adjectives :(.
			if (sb.Length == 0 && nipple.relativeLust > 50)
			{
				if (nipple.relativeLust >= 75)
				{
					sb.Append(Utils.RandomChoice("throbbing", "trembling", "needy ", "throbbing "));
				}
				else
				{
					sb.Append(Utils.RandomChoice("erect ", "perky ", "erect ", "firm ", "tender "));
				}
			}


			if (nipple.status == NippleStatus.DICK_NIPPLE)
			{
				if (nipple.quadNipples)
				{
					if (sb.Length > 0)
					{
						sb.Append(", ");
					}
					sb.Append("quad-tipped");
				}

				if (sb.Length > 0)
				{
					sb.Append(" ");
				}
				sb.Append(Utils.PluralizeIf("dick-nipple", allAvailableNipples));
			}
			else if (nipple.status == NippleStatus.FUCKABLE)
			{
				if (nipple.quadNipples)
				{
					if (sb.Length > 0)
					{
						sb.Append(", ");
					}
					sb.Append(Utils.PluralizeIf("quad-fuckable nipple", allAvailableNipples));
				}
				else
				{
					if (sb.Length > 0)
					{
						sb.Append(" ");
					}
					sb.Append(Utils.PluralizeIf("nipple-cunt", allAvailableNipples));
				}
			}
			else
			{
				if (sb.Length > 0)
				{
					sb.Append(" ");
				}
				if (nipple.quadNipples)
				{
					sb.Append("quad-");
				}
				sb.Append(Utils.PluralizeIf("nipple", allAvailableNipples));
			}

			bool needsArticle = !allAvailableNipples && withArticle;

			if (preciseMeasurements)
			{
				//...shit. eight uses an. well, i guess add article if handles that now. not a huge fan but i guess it works.
				return Utils.AddArticleIf(Measurement.ToNearestQuarterInchOrMillimeter(nipple.length, false, false), needsArticle) + sb.ToString();
			}
			else
			{
				return NippleSizeAdjective(nipple.length, needsArticle) + sb.ToString();
			}
		}

		public static string NippleSizeAdjective(double length, bool withArticle = false)
		{
			//TINAHHHH
			if (length < .25)
			{
				if (withArticle) return Utils.RandomChoice("a tiny ", "an itty-bitty ", "a teeny-tiny ", "a dainty ");
				return Utils.RandomChoice("tiny ", "itty-bitty ", "teeny-tiny ", "dainty ");
			}
			else if (length < 0.4)
			{
				return "";
			}
			//
			//Prominant
			else if (length < 1)
			{
				if (withArticle) return Utils.RandomChoice("a prominent ", "a pencil eraser-sized ", "an eye-catching ", "a pronounced ", "a striking ");
				else return Utils.RandomChoice("prominent ", "pencil eraser-sized ", "eye-catching ", "pronounced ", "striking ");
			}
			//Big 'uns
			else if (length < 2)
			{
				if (withArticle) return Utils.RandomChoice("a forward-jutting ", "an over-sized ", "a fleshy ", "a large, protruding ");
				return Utils.RandomChoice("forward-jutting ", "over-sized ", "fleshy ", "large, protruding ");
			}
			//'Uge
			else if (length < 3.2)
			{
				if (withArticle) return Utils.RandomChoice("an elongated ", "a massive ", "an awkward ", "a lavish ", "a hefty ");
				else return Utils.RandomChoice("elongated ", "massive ", "awkward ", "lavish ", "hefty ");
			}
			//Massive
			else //if (length >= 3.2)
			{
				return (withArticle ? "a " : "") + Utils.RandomChoice("bulky ", "ponderous ", "thumb-sized ", "cock-sized ", "cow-like ");
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
