using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.StatusEffect;
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace CoC.Frontend.Transformations
{
	internal abstract class GenericTransformationBase
	{
		//tfs can be applied to any creature, potentially - don't assume it's the player. but you can always check if the target is a Player object,
		//and if it is, do Player related things.
		protected internal abstract string DoTransformation(Creature target, out bool isBadEnd);

		protected int GenerateChangeCount(Creature target, int[] extraRolls = null, int initialCount = 1, int minimumCount = 1)
		{
			initialCount += target.GetExtraData()?.deltaTransforms ?? 0;
			if (extraRolls != null)
			{
				initialCount += extraRolls.Sum(x => Utils.Rand(x) == 0 ? 1 : 0);
			}

			return Math.Max(initialCount, minimumCount);
		}

		protected string ApplyChangesAndReturn(Creature target, StringBuilder builder, int transformCount)
		{
			if (target is IExtendedCreature extendedCreature)
			{
				extendedCreature.extendedData.TotalTransformCount += transformCount;
			}
			return builder.ToString();
		}

		protected bool RemoveFeatheryHair(Creature creature)
		{
			if (creature.hair.type == HairType.FEATHER && Utils.Rand(4) == 0)
			{
				return creature.RestoreHair();
			}
			return false;
		}

		protected byte GrowCockGeneric(Creature creature, byte count)
		{
			if (count == 0)
			{
				return 0;
			}

			byte added = 0;
			while (count-- > 0 && creature.AddCock(CockType.HUMAN, Utils.Rand(3) + 5, 0.75f))
			{
				added++;
			}
			return added;
		}

		protected string RemovedFeatheryHairTextGeneric(Creature creature, bool ingestedSomething = true)
		{
			if (creature.hair.length >= 6)
			{
				return Environment.NewLine + Environment.NewLine + "A lock of your downy-soft feather-hair droops over your eye. Before you can blow the offending down away, " +
					"you realize the feather is collapsing in on itself. It continues to curl inward until all that remains is a normal strand of hair. +" +
					SafelyFormattedString.FormattedText("Your hair is no longer feathery!", StringFormats.BOLD);
			}
			else
			{
				string waitingForText = ingestedSomething ? "of the item you just ingested" : "for something to happen";
				return Environment.NewLine + Environment.NewLine + "You run your fingers through your downy-soft feather-hair while you await the effects " + waitingForText +
					". While your hand is up there, it detects a change in the texture of your feathers. They're completely disappearing, merging down into strands of regular hair." +
					SafelyFormattedString.FormattedText("Your hair is no longer feathery!", StringFormats.BOLD);
			}
		}

		protected string GainedOvipositionTextGeneric(Creature target)
		{
			return Environment.NewLine + Environment.NewLine + "Deep inside yourself there is a change. It makes you feel a little woozy, but passes quickly. Beyond that, " +
				"you aren't sure exactly what just happened, but you are sure it originated from your womb." + Environment.NewLine +
				"(" + SafelyFormattedString.FormattedText("Perk Gained: Oviposition", StringFormats.BOLD) + ")";
		}

		protected string RemovedOvipositionTextGeneric(Creature target)
		{
			return Environment.NewLine + Environment.NewLine + "Another change in your uterus ripples through your reproductive systems. Somehow you know you've lost " +
				"a little bit of reptilian reproductive ability." + Environment.NewLine + SafelyFormattedString.FormattedText("Perk Lost: Oviposition", StringFormats.BOLD);
		}

		protected bool EnterHeat(Creature target, out bool deepenedHeat, byte roll = 2)
		{
			deepenedHeat = false;
			if (Utils.Rand(roll) == 0 && target.hasVagina && !target.womb.isPregnant)
			{
				deepenedHeat = target.statusEffects.HasStatusEffect<Heat>();
				target.GoIntoHeat();
			}
			return false;
		}

		protected bool EnterRut(Creature target, out bool deepenedRut, byte roll = 2)
		{
			deepenedRut = false;
			if (Utils.Rand(roll) == 0 && target.hasVagina && !target.womb.isPregnant)
			{
				deepenedRut = target.statusEffects.HasStatusEffect<Rut>();
				target.GoIntoRut();
			}
			return false;
		}


		//displays text for one more more cocks, with each growing or shortening by the deltaAmount value (positive = grow. negative = shorten)
		//the number of cocks is determined by the
		protected string GenericChangeOneCockLengthText(Creature target, int index, float deltaAmount)
		{
			return GenericChangeCockLengthText(target, new int[] { index }, deltaAmount);
		}
		protected string GenericChangeCockLengthText(Creature target, int[] grownCocks, float deltaAmount)
		{
			//handle null checks so we don't have to worry about them.
			if (target == null) throw new ArgumentNullException(nameof(target));
			if (grownCocks == null) throw new ArgumentNullException(nameof(grownCocks));


			//clean up the array so that we don't have any duplicates, and they are ordered. i've converted them to an array of booleans, which is true if the cock was in our list.
			BitArray cocksChanged = new BitArray(10);
			foreach (var item in grownCocks)
			{
				if (item >= Genitals.MAX_COCKS || item >= target.cocks.Count) continue;
				cocksChanged[item] = true;
			}
			int cockChangeCount = cocksChanged.GetCardinality();
			//handle no cocks changed (either because growCocks is null or delta is 0) with a quick exit.
			if (cockChangeCount == 0 || deltaAmount == 0)
			{
				return "";
			}

			Cock largestUnchangedCock = target.cocks.Where((x, y) => !cocksChanged[y]).OrderByDescending(x => x.length).FirstOrDefault();
			Cock largestUpdatedCock = target.cocks.Where((x, y) => cocksChanged[y]).OrderByDescending(x => x.length).First();

			StringBuilder sb = new StringBuilder();
			//DIsplay the degree of length change.
			if (deltaAmount <= 1 && deltaAmount > 0)
			{
				if (target.cocks.Count == 1) sb.Append("Your " + target.cocks[0].LongDescription() + " has grown slightly longer.");
				else //if (target.cocks.Count > 1)
				{
					if (cockChangeCount == 1)
					{
						sb.Append("One of your " + target.genitals.AllCocksShortDescription() + " grows slightly longer.");
					}
					else if (cockChangeCount < target.cocks.Count)
					{
						sb.Append("Some of your " + target.genitals.AllCocksShortDescription() + " grow slightly longer.");
					}
					else //if (cockChangeCount == target.cocks.Count)
					{
						sb.Append("Your " + target.genitals.AllCocksShortDescription() + " seem to fill up... growing a little bit larger.");
					}
				}
			}
			else if (deltaAmount > 1 && deltaAmount < 3)
			{
				if (target.cocks.Count == 1)
				{
					sb.Append("A very pleasurable feeling spreads from your groin as your " + target.cocks[0].LongDescription() + " grows permanently longer - at least "
						+ (Measurement.UsesMetric ? "a few centimeters" : "an inch") + " - and leaks pre-cum from the pleasure of the change.");
				}

				else //if (target.cocks.Count > 1)
				{
					if (cockChangeCount == 1)
					{
						sb.Append("A very pleasurable feeling spreads from your groin as one of your " + target.genitals.AllCocksShortDescription() + " grows permanently longer, " +
							"by at least " + (Measurement.UsesMetric ? "a few centimeters" : "an inch") + ", and leaks plenty of pre-cum from the pleasure of the change.");
					}
					else if (cockChangeCount < target.cocks.Count)
					{
						sb.Append("A very pleasurable feeling spreads from your groin as " + Utils.NumberAsText(cockChangeCount) + " of your "
							+ target.genitals.AllCocksShortDescription() + " grow permanently longer, by at least " + (Measurement.UsesMetric ? "a few centimeters" : "an inch") +
							", and leak plenty of pre-cum from the pleasure of the change.");
					}
					else //if (cockChangeCount == target.cocks.Count)
					{
						sb.Append("A very pleasurable feeling spreads from your groin as your " + target.genitals.AllCocksShortDescription() + " grow permanently longer - at least "
							+ (Measurement.UsesMetric ? "a few centimeters" : "an inch") + " - and leak plenty of pre-cum from the pleasure of the change.");
					}
				}
			}
			else if (deltaAmount >= 3)
			{
				if (target.cocks.Count == 1)
				{
					sb.Append("Your " + target.cocks[0].LongDescription() + " feels incredibly tight as a few more " + (Measurement.UsesMetric ? "centimeters" : "inches") + " of length seem to pour out from your crotch.");
				}

				else //if (target.cocks.Count > 1)
				{
					if (cockChangeCount == 1)
					{
						sb.Append("Your " + target.genitals.AllCocksShortDescription() + " feel incredibly tight as one of their number begins to grow, "
							+ (Measurement.UsesMetric ? "centimeter after centimeter" : "inch after inch") + ".");
					}
					else if (cockChangeCount < target.cocks.Count)
					{
						sb.Append("Your " + target.genitals.AllCocksShortDescription() + " feel incredibly numb as " + Utils.NumberAsText(cockChangeCount) + " of them begin to grow, "
							+ (Measurement.UsesMetric ? "centimeter after centimeter" : "inch after inch") + " ");
					}
					else //if (cockChangeCount == target.cocks.Count)
					{
						sb.Append("Your " + target.genitals.AllCocksShortDescription() + " feel incredibly tight as they grow,"
							+ (Measurement.UsesMetric ? "centimeter after centimeter" : "inch after inch") + " of added length pouring from your groin.");
					}
				}
			}
			//Display LengthChange
			if (deltaAmount > 0)
			{
				//this now handles multiple cocks better - if we already had a cock at the current threshold, we don't mention what having a cock of that length entails.
				//we may, however, mention we have another cock at that threshold.

				//start with the largest and work our way down.
				if (largestUpdatedCock.length >= 20 && largestUpdatedCock.length - deltaAmount < 20)
				{
					bool describeAll = target.cocks.All((x, y) => x.length >= 20 && cocksChanged[y] && x.length - deltaAmount < 20);
					bool describeSeveral = target.cocks.Any((x, y) => x.length >= 20 && cocksChanged[y] && x != largestUpdatedCock && x.length - deltaAmount < 20);

					//if we didn't have any cocks above 20 before this, mention how it/they now obscure your lower vision.
					if (largestUnchangedCock is null || largestUnchangedCock.length < 20)
					{
						sb.Append("<b>As if the pulsing heat ");


						if (cockChangeCount == 1)
						{
							sb.Append("of your " + largestUpdatedCock.LongDescription());
						}
						else
						{
							sb.Append("of your " + target.genitals.AllCocksShortDescription());
						}
						sb.Append(" wasn't bad enough, ");

						if (describeAll || describeSeveral)
						{
							sb.Append("every time you get hard, the tips of ");

							if (describeSeveral)
							{
								sb.Append("several of ");
							}
							sb.Append("your " + target.genitals.AllCocksShortDescription() + "wave before you, obscuring the lower portions of your vision");
						}
						else
						{
							sb.Append("your " + largestUpdatedCock.ShortDescription() + "'s " + largestUpdatedCock.HeadDescription() + " keeps poking its way into your view " +
								"every time you get hard.");
						}

						sb.Append("</b>");

						//then, describe what this entails.
						if (target.corruption > 80)
						{
							sb.Append(" You find yourself fantasizing about impaling nubile young champions on your " + target.genitals.AllCocksLongDescription() + " in a year's time.");
						}
						else if (target.corruption > 60)
						{
							sb.Append(" You daydream about being attacked by a massive tentacle beast, its tentacles engulfing your " + target.genitals.AllCocksLongDescription() + " to the hilt, milking it of all your cum.\n\nYou smile at the pleasant thought.");
						}
						else if (target.corruption > 40)
						{
							if (target.cocks.Count == 1)
							{
								sb.Append(" You wonder if there is a demon or beast out there that could handle your full length.");
							}
							else
							{
								sb.Append(" You wonder - is a demon or beast out there that could take the full length of the largest of your " + target.genitals.AllCocksShortDescription() + "?");
							}
						}
					}
					//otherwise, just mention that you now have more that can obscure your vision
					else
					{
						if (describeAll || describeSeveral)
						{
							sb.Append((describeAll ? "They've each" : "Several have") + " grown long enough that you now have even more cocks starting to obscure your " +
								"vision whenever you have an erection.");
						}
						else
						{
							sb.Append("It's grown so long that you now have yet another cock that partially obscures your vision when erect.");
						}
					}
				}
				else if (largestUpdatedCock.length >= 16 && largestUpdatedCock.length - deltaAmount < 16)
				{

					bool describeAll = target.cocks.All((x, y) => x.length >= 16 && cocksChanged[y] && x.length - deltaAmount < 16);

					sb.Append(" <b>");
					if (describeAll)
					{
						sb.Append("Each one of your " + target.genitals.AllCocksShortDescription() + "now looks like it'd be more at home " +
							"on a large horse, let alone together on one body");
					}
					else if (cockChangeCount == 1)
					{
						sb.Append("Your " + largestUpdatedCock.LongDescription() + " would look more at home on a large horse than you");
					}
					else
					{
						sb.Append("The largest of them now looks like it'd be more at home on a large horse than you");
					}

					//if multiple cocks, and all the cocks that grew longer are still shorter than the longest, make note of that.
					if (!describeAll && !(largestUnchangedCock is null) && largestUpdatedCock.length < largestUnchangedCock.length)
					{
						sb.Append(", though it's still not as long as your " + largestUnchangedCock.LongDescription());
					}
					sb.Append(".</b>");

					//if it is the longest, that means we've just reached this threshold. mention that we can now tit-fuck ourselves.
					if (largestUnchangedCock is null || largestUnchangedCock.length < 16)
					{
						if (target.genitals.BiggestCupSize() >= CupSize.C)
						{
							//you only have one cock that grew larger or all the other ones are still below the threshold.
							if (cockChangeCount == 1 || target.cocks.Where((x, y) => cocksChanged[y] && x != largestUpdatedCock).All(x => x.length < 16))
							{
								sb.Append(" You could easily stuff your " + largestUpdatedCock.LongDescription() + " between your breasts and give yourself the titty-fuck of a lifetime.");
							}
							//some (but not all) are now this large.
							else if (!describeAll)
							{
								sb.Append(" Several of your " + target.genitals.AllCocksShortDescription() + " now reach so far up your chest it would be easy to stuff a few of them " +
									"between your breasts and give yourself the titty-fuck of a lifetime.");
							}
							else //if (target.cocks.Count > 1)
							{
								sb.Append(" They reach so far up your chest it would be easy to stuff a few cocks between your breasts and give yourself the titty-fuck of a lifetime.");
							}
						}
						else
						{
							if (cockChangeCount == 1 || target.cocks.Where((x, y) => cocksChanged[y] && x != largestUpdatedCock).All(x => x.length < 16))
							{
								sb.Append("  Your " + largestUpdatedCock.LongDescription() + " is so long it easily reaches your chest. " +
									"The possibility of autofellatio is now a foregone conclusion.");
							}
							else if (!describeAll)
							{
								sb.Append(" Several of your " + target.genitals.AllCocksShortDescription() + "are now long enough to easily reach your chest. " +
									"Autofellatio would be about as hard as looking down.");
							}
							else
							{
								sb.Append(" They are so long that they easily reach your chest; you'd be able to perform autofellatio on any of them with little effort.");
							}
						}
					}
				}
				else if (largestUpdatedCock.length >= 12 && largestUpdatedCock.length - deltaAmount < 12)
				{
					bool describeAll = target.cocks.All((x, y) => x.length >= 12 && cocksChanged[y] && x.length - deltaAmount < 12);
					sb.Append(" <b>");
					if (describeAll)
					{
						sb.Append("They are all so long now that they nearly reach your knees when at full length");
					}
					else if (cockChangeCount > 1)
					{
						sb.Append("The largest of them is now so long, it nearly reaches your knees");
					}
					else //if (cockChangeCount == 1)
					{
						sb.Append("Your " + target.cocks[0].LongDescription() + " is so long it nearly swings to your knee at its full length");
					}

					//if multiple cocks, and all the cocks that grew longer are still shorter than the longest, make note of that.
					if (!describeAll && !(largestUnchangedCock is null) && largestUpdatedCock.length < largestUnchangedCock.length)
					{
						sb.Append(", though it's still not as long as your " + largestUnchangedCock.LongDescription());
					}

					sb.Append(".</b>");
				}
				else if (largestUpdatedCock.length >= 8 && largestUpdatedCock.length - deltaAmount < 8)
				{
					bool describeAll = target.cocks.All((x, y) => x.length >= 8 && cocksChanged[y] && x.length - deltaAmount < 8);
					bool describeSeveral = target.cocks.Any((x, y) => x.length >= 20 && cocksChanged[y] && x != largestUpdatedCock && x.length - deltaAmount < 20);
					sb.Append("<b>");

					if (describeAll)
					{
						sb.Append("Most men would be overly proud to have a cock as long as your " + target.genitals.ShortestCock() + ", and that's the shortest one you have!");
					}
					else if (describeSeveral)
					{
						sb.Append("Several have now reached lengths most men would be proud to match");
					}
					else if (target.cocks.Count != 1)
					{
						sb.Append("The largest is now long enough most men would be proud to match its length");
					}
					else //if (target.cocks.Count == 1)
					{
						sb.Append(" Most men would be overly proud to have a tool as long as yours");
					}



					//if multiple cocks, and all the cocks that grew longer are still shorter than the longest, make note of that.
					if (!describeAll && !(largestUnchangedCock is null) && largestUpdatedCock.length < largestUnchangedCock.length)
					{
						sb.Append(", and it's still not as long as your " + largestUnchangedCock.LongDescription() + "!");
					}
					else
					{
						sb.Append(".");
					}

					sb.Append("</b>");
				}
			}

			//Display the degree of length loss.
			else if (deltaAmount >= -1)
			{
				if (target.cocks.Count == 1)
				{
					sb.Append("Your " + target.genitals.AllCocksShortDescription() + " has shrunk to a slightly shorter length.");
				}
				else //if (target.cocks.Count > 1)
				{
					if (cockChangeCount == target.cocks.Count)
					{
						sb.Append("Your " + target.genitals.AllCocksShortDescription() + " have shrunk to a slightly shorter length.");
					}
					else if (cockChangeCount > 1)
					{
						sb.Append("You feel " + Utils.NumberAsText(cockChangeCount) + " of your " + target.genitals.AllCocksShortDescription() + " have shrunk to a slightly shorter length.");
					}
					else //if (cockChangeCount == 1)
					{
						sb.Append("You feel " + Utils.NumberAsText(cockChangeCount) + " of your " + target.genitals.AllCocksShortDescription() + " has shrunk to a slightly shorter length.");
					}
				}
			}
			else if (deltaAmount > -3)
			{
				if (target.cocks.Count == 1)
				{
					sb.Append("Your " + target.genitals.AllCocksShortDescription() + " shrinks smaller, flesh vanishing into your groin.");
				}
				else //if (target.cocks.Count > 1)
				{
					if (cockChangeCount == target.cocks.Count)
					{
						sb.Append("Your " + target.genitals.AllCocksShortDescription() + " shrink smaller, the flesh vanishing into your groin.");
					}
					else if (cockChangeCount > 1)
					{
						sb.Append("You feel " + Utils.NumberAsText(cockChangeCount) + " of your " + target.genitals.AllCocksShortDescription() + " shrink smaller, the flesh vanishing into your groin.");
					}
					else //if (cockChangeCount == 1)
					{
						sb.Append("You feel " + Utils.NumberAsText(cockChangeCount) + " of your " + target.genitals.AllCocksShortDescription() + " shrink smaller, the flesh vanishing into your groin.");
					}
				}
			}
			else //if (deltaAmount <= -3)
			{
				if (target.cocks.Count == 1)
				{
					sb.Append("A large portion of your " + target.genitals.AllCocksShortDescription() + "'s length shrinks and vanishes.");
				}
				else //if (target.cocks.Count > 1)
				{
					if (cockChangeCount == target.cocks.Count)
					{
						sb.Append("A large portion of your " + target.genitals.AllCocksShortDescription() + " recedes towards your groin, receding rapidly in length.");
					}
					else if (cockChangeCount > 1)
					{
						sb.Append("Your " + target.genitals.AllCocksShortDescription() + " tingles as " + Utils.NumberAsText(cockChangeCount) + " of your members vanish into your groin, receding rapidly in length.");
					}
					else //if (cockChangeCount == 1)
					{
						sb.Append("A single member of your " + target.genitals.AllCocksShortDescription() + " vanishes into your groin, receding rapidly in length.");
					}
				}
			}

			return sb.ToString();
		}

		//add any common generic transformation related functions here - generic texts, common tfs, etc.

		protected string RemovedExtraBreastRowGenericText(Creature target, BreastData removedBreastRow)
		{
			return $"You stumble back when your center of balance shifts, and though you adjust before you can fall over, you're left to watch in awe as your bottom-most " +
				$"{removedBreastRow.ShortDescription()} shrink down, disappearing completely into your {(target.breasts.Count > 2 ? "abdomen" : "chest")}. The " +
				$"{removedBreastRow.ShortNippleDescription()} even fade until nothing but {target.body.mainEpidermis.ShortDescription()} remains. " +
				SafelyFormattedString.FormattedText("You've lost a row of breasts!", StringFormats.BOLD);
		}

		protected string GainedOrEnteredHeatTextGeneric(Creature target, bool isIncrease)
		{
			if (!target.statusEffects.HasStatusEffect<Heat>())
			{
				return "";
			}

			var heat = target.statusEffects.GetStatusEffect<Heat>();

			if (isIncrease)
			{
				return heat.IncreasedHeatText();
			}
			else
			{
				return heat.ObtainText();
			}
		}
	}
}
