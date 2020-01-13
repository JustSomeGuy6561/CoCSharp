using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Text;

namespace CoC.Frontend.Items.Consumables
{
	partial class ImpFood
	{
		private static string Abbreviated()
		{
			return "ImpFood";
		}

		private static string Name()
		{
			return "Imp Food";
		}

		private static string Desc()
		{
			return "a parcel of imp food";
		}

		private static string AppearanceStr()
		{
			return "This is a small parcel of reddish-brown bread stuffed with some kind of meat. It smells delicious.";
		}

		private static string InitialTransformText(Creature target)
		{
			if (target.cocks.Count > 0)
			{
				return "The food tastes strange and corrupt - you can't really think of a better word for it, but it's unclean.";
			}
			else
			{
				return "The food tastes... corrupt, for lack of a better word." + Environment.NewLine;
			}
		}

		private static string GainVitalityText(Creature target)
		{
			return GlobalStrings.NewParagraph() + "Inhuman vitality spreads through your body, invigorating you!" + Environment.NewLine;
		}

		private static string ChangeSkinColorText(Creature target, Tones oldSkinTone)
		{
			StringBuilder sb = new StringBuilder();

			bool skinPlural;

			if (target.body.skinActive)
			{
				sb.Append("Your " + target.body.primarySkin.LongDescription(out skinPlural));
			}
			else
			{
				skinPlural = false;
				sb.Append("The skin under your " + target.body.LongEpidermisDescription());
			}

			sb.Append((skinPlural ? "begin to lose their" : "begins to lose its") + " color, fading out until you're as white as an albino. Then, starting at the crown of your head," +
				" a reddish hue rolls down your body in a wave, turning you completely " + target.body.primarySkin.tone.AsString() + ".");

			return sb.ToString();
		}

		private static string GetShorterText(Creature target, byte heightDelta)
		{
			return GlobalStrings.NewParagraph() + "Your skin crawls, making you close your eyes and shiver. When you open them again the world seems... different. " +
				"After a bit of investigation, you realize you've become shorter!";
		}

		private static string EnlargenedImpWingsText(Creature target)
		{
			return target.wings.ChangedSizeText(false);
		}

		private static string HairChangedText(Creature target, HairData oldHairData)
		{
			return GlobalStrings.NewParagraph() + "Your hair suddenly begins to shed, rapidly falling down around you before it's all completely gone. " +
				"Just when you think things are over, more hair sprouts from your head, slightly curled and colored differently." +
				SafelyFormattedString.FormattedText("You now have " + (oldHairData.type != HairType.NORMAL ? "normal, " : "") + target.hair.LongDescription() + "!", StringFormats.BOLD);
		}

		private static string CurrentBreastRowChanged(Creature target, int index, byte cupSizesShrunk, byte rowsPreviouslyModified)
		{
			if (cupSizesShrunk == 0)
			{
				return "";
			}

			StringBuilder sb = new StringBuilder();


			if (rowsPreviouslyModified == 0)
			{
				sb.Append(GlobalStrings.NewParagraph());
			}

			else if (cupSizesShrunk == 1)
			{
				if (rowsPreviouslyModified > 0)
				{
					sb.Append(" Your " + Utils.NumberAsPlace(index + 1) + " row of " + target.breasts[index].ShortDescription() + " gives a tiny jiggle as it too shrinks, " +
						"losing some off its mass.");
				}
				else
				{
					sb.Append("All at once, your sense of gravity shifts. Your back feels a sense of relief, and it takes you a moment to realize your ");

					if (target.breasts.Count > 1)
					{
						sb.Append(Utils.NumberAsPlace(index + 1) + " row of ");
					}

					sb.Append(target.breasts[index].ShortDescription() + " have shrunk!");
				}
			}
			else
			{
				if (rowsPreviouslyModified == 0)
				{
					sb.Append("The ");

					if (target.breasts.Count > 1)
					{
						sb.Append(Utils.NumberAsPlace(index + 1) + " row of ");
					}

					sb.Append(target.breasts[index].ShortDescription() + " on your chest wobble for a second, then tighten up, losing several cup-sizes in the process!");
				}
				else
				{
					sb.Append(" The change moves down to your " + Utils.NumberAsPlace(index + 1) + " row of " + target.breasts[index].ShortDescription() +
						". They shrink greatly, losing a couple cup-sizes.");
				}

			}
			return sb.ToString();
		}

		private static string RemovedQuadNippleText(Creature target)
		{
			string armorText = target.wearingArmor ? "under your " + target.armor.ItemName() : target.wearingUpperGarment ? "under your " + target.upperGarment.ItemName() : "down";
			return GlobalStrings.NewParagraph() + "A strange burning sensation fills your breasts, and you look " + armorText + " to see your extra nipples are gone! " +
				SafelyFormattedString.FormattedText("You've lost your extra nipples!", StringFormats.BOLD);
		}

		private static string ImpifyText(Creature target, GenitalsData previousGenitals)

		{
			StringBuilder sb = new StringBuilder("A strange sensation begins to sweep through your body, ");

			if (target.wearingAnything)
			{
				sb.Append("and you quickly disrobe, curious to see what other effects the imp food could have on your body.");
			}
			else
			{
				sb.Append("and you look down, curious as to what else the imp food could change.");
			}

			sb.Append(" As you give yourself a quick look-over, you're unsure what else the food could change, considering how impish you already are. ");

			if (previousGenitals.gender != Gender.MALE)
			{
				sb.Append("Come to think of it, though, you've only seen male imps... Oh.");
			}
			else if (previousGenitals.breasts.Count > 1)
			{
				sb.Append("Then again, you've never seen an imp with anything but a single pair of flat, manly breasts... Oh.");
			}
			else if (!previousGenitals.breasts[0].isMaleBreasts)
			{
				sb.Append("Then again, you've never seen an imp with breasts... Oh.");
			}
			else if (!previousGenitals.balls.hasBalls)
			{
				sb.Append("Then again, you can't say you've seen an imp without a pair of balls... Oh");
			}

			if (previousGenitals.breasts.Count > 1)
			{
				sb.Append(GlobalStrings.NewParagraph() + "You stumble back when your center of balance shifts, and though you adjust before you can fall over, " +
					"you're left to watch in awe as your extra breasts shrink down, disappearing completely into your body. The nipples even fade away until they're gone completely. " +
					SafelyFormattedString.FormattedText("Your impification has removed all of your extra breast rows!", StringFormats.BOLD));
			}

			if (!previousGenitals.breasts[0].isMaleBreasts)
			{
				if (previousGenitals.breasts.Count > 1)
				{
					sb.Append(SafelyFormattedString.FormattedText(" Additionally, your remaining breast row is now more flat and manly!", StringFormats.BOLD));
					//link the two together with flavor text, maybe.
				}
				else
				{
					sb.Append("You stumble back when your center of balance shifts, and though you adjust before you can fall over, you're left to watch in awe as your "
						+ previousGenitals.breasts[0].ShortDescription() + " shrink down, " + SafelyFormattedString.FormattedText("becoming flatter and manlier!", StringFormats.BOLD));
				}
			}

			bool grewBalls = !previousGenitals.balls.hasBalls;

			//previously female
			if (previousGenitals.gender == Gender.FEMALE)
			{
				sb.Append("The sensation reaches your groin, and your " + (previousGenitals.numVaginas > 1 ? "vaginas seem" : "vagina seems") + " to be particularly affected. " +
					"You can't help but watch as ");

				if (previousGenitals.numVaginas > 1)
				{
					sb.Append(SafelyFormattedString.FormattedText("your " + previousGenitals.AllVaginasLongDescription() + " receed inward, their fleshy entrances " +
						"combining together before shrinking down to almost nothing.", StringFormats.BOLD));
				}
				else
				{
					sb.Append(SafelyFormattedString.FormattedText("your innermost depths collapse on themselves, dragging the rest of your "
						+ previousGenitals.vaginas[0].ShortDescription() + " with it. ", StringFormats.BOLD));
				}

				sb.Append(" Instead of disappearing, though, your remaining folds begin to bunch together, then push outward, flooding you with pleasure. " +
					"You soon reach your limit, not able to stop yourself from cumming " + SafelyFormattedString.FormattedText("out of your new cock!", StringFormats.BOLD));

				if (grewBalls)
				{
					sb.Append(SafelyFormattedString.FormattedText(" A pair of balls drop below it, completing the look!", StringFormats.BOLD));
				}

				sb.Append("You soon realize that this means you're no longer female, but you suppose that's the price you pay to become fully impish.");
			}

			//previously herm
			else if (previousGenitals.gender == Gender.HERM)
			{
				sb.Append("The sensation reaches your groin, and your " + (previousGenitals.numVaginas > 1 ? "vaginas seem" : "vagina seems") + " to be particularly affected. " +
					"You can't help but watch as ");

				if (previousGenitals.numVaginas > 1)
				{
					sb.Append(SafelyFormattedString.FormattedText("your " + previousGenitals.AllVaginasLongDescription() + " receed inward, collapsing upon themselves " +
						"until they're completely gone!", StringFormats.BOLD));
				}
				else
				{
					sb.Append(SafelyFormattedString.FormattedText("it receeds inwards, collapsing upon itself until it's completely gone!", StringFormats.BOLD));
				}

				sb.Append("It appears you're no longer a herm, but you suppose that's the price you pay to become fully impish.");

				if (grewBalls)
				{
					sb.Append(Environment.NewLine + SafelyFormattedString.FormattedText("A pair of balls drops in place below your" + target.genitals.AllCocksLongDescription() +
						"completing the transformation.", StringFormats.BOLD));
				}
			}
			//previously genderless
			else if (previousGenitals.gender == Gender.GENDERLESS)
			{
				sb.Append("The sensation reaches your groin, stirring up a sexual need with no real way to satisfy it. As if on cue, a bulge begins to form " +
					"on your smooth groin, gradually building up pressure until it bursts forth. The feeling is so intense you can't help but cum from " +
					SafelyFormattedString.FormattedText("your new cock!", StringFormats.BOLD));

				if (grewBalls)
				{
					sb.Append(Environment.NewLine + SafelyFormattedString.FormattedText("A pair of balls drops in place below it, completing the look.", StringFormats.BOLD));
				}

				sb.Append("It appears you're male now, which is apparently part of the price of becoming fully impish.");
			}
			//previously 'male' but without balls.
			else if (!previousGenitals.balls.hasBalls)
			{
				sb.Append(GlobalStrings.NewParagraph() + "Sure enough, the sensation drops below your thighs, where a strange, unpleasant pressure forms, and " +
					SafelyFormattedString.FormattedText("A pair of balls drop below your " + target.genitals.AllCocksShortDescription() + "!", StringFormats.BOLD) +
					"Welp, guess that's one more thing you have in common with imps now.");
			}


			return sb.ToString();
		}
	}
}
