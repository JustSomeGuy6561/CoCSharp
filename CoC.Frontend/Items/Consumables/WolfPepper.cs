using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations;

namespace CoC.Frontend.Items.Consumables
{
	/**
	 * Wolf transformative item.
	 *
	 * @author Foxwells
	 */
	public class WolfPepper : StandardConsumable
	{
		public override string Author()
		{
			return "Foxwells";
		}

		public override bool countsAsCum => false;

		public override bool countsAsLiquid => false;

		public WolfPepper() : base()
		{ }

		public override string AbbreviatedName() => "Wolf Pp";
		public override string ItemName() => "Wolf Pp";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			string itemText = count != 1 ? "peppers" : "pepper";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}wolf {itemText}";
		}
		public override string AboutItem() => "The pepper is shiny and black, bulbous at the base but long and narrow at the tip. It has a fuzzy feel to it and it smells spicy. Somehow, you know it's different from the usual Canine Peppers you see.";
		protected override int monetaryValue => DEFAULT_VALUE;

		// Fuck yo dog shit we full-on wolf bitches now -Foxwells

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			WolfTfs tfs = new WolfTfs();
			consumeItem = true;
			return tfs.DoTransformation(consumer, out isBadEnd);
		}

		protected override string OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out bool consumeItem, out bool isBadEnd)
		{
			WolfTfs tfs = new WolfTfs();
			consumeItem = true;
			return tfs.DoTransformationFromCombat(consumer, opponent, out isBadEnd);
		}

		public override byte sateHungerAmount => 10;

		public override bool Equals(CapacityItem other)
		{
			return other is WolfPepper;
		}

		private class WolfTfs : WolfTransformations
		{
			protected override string InitialTransformationText(Creature target, double crit)
			{
				string critText = crit > 1 ? " Maybe it was a bit too spicy... The pepper seemed far more ripe than what you'd expect." : "";
				return "The pepper has an uncomfortable texture to it, being covered in soft fuzz like it's a peach but somewhat crunchy like any other pepper. "
					+ "Its spiciness makes you nearly spit it out, and you're left sniffling after." + critText + GlobalStrings.NewParagraph()
					+ "You lick your lips after you finish. That spiciness hit you in more ways than one.";
			}

			protected override string NormalizedBreastSizeText(Creature target, BreastCollectionData oldBreastData)
			{
				StringBuilder sb = new StringBuilder();

				//first, check to see if we grew any rows larger so we could allow this new row.
				//orderby should be unnecessary, but the cost is low on an already sorted list.
				IOrderedEnumerable<ValueDifference<BreastData>> changedItems = target.genitals.allBreasts.ChangedBreastRows(oldBreastData, true).Where(x => x.oldValue.cupSize != x.newValue.cupSize).OrderBy(x => x.oldValue.currBreastRowIndex);

				if (!changedItems.IsEmpty())
				{
					string armorText = target.UpperBodyArmorTextHelper(
					"As you remove your " + target.armor.ItemName() + " and " + target.upperGarment.ItemName(),
					"As you remove your " + target.armor.ItemName(),
					"As you remove your " + target.upperGarment.ItemName(),
					"Looking down"
					);

					//anthropomorphizing breasts will very rarely result in a net growth if many rows are too small and this becomes necessary. we first check for this by
					//summing all the rows of old and new data.
					bool grewSome = changedItems.Sum(x => (byte)x.oldValue.cupSize) < changedItems.Sum(x => (byte)x.newValue.cupSize);
					bool nowAnthro = true;

					for (int x = 1; x < target.breasts.Count; x++)
					{
						if (target.breasts[x].cupSize > target.breasts[x - 1].cupSize || (target.breasts[x].cupSize == target.breasts[x - 1].cupSize
							&& target.breasts[x].cupSize != target.genitals.smallestPossibleCupSize))
						{
							nowAnthro = false;
							break;
						}
					}

					if (grewSome)
					{
						sb.Append("Your breasts feel heavier, yet the additional heft strangely aides your balance somehow. " + armorText + ", you notice your uppermost rows " +
							"of breasts have enlarged slightly, as if they desire to be slightly larger than their successors.");
					}
					else
					{
						sb.Append("A strange sensation comes over your many rows of breasts, initially puzzling you - they don't feel any lighter, but you don't feel quite as " +
							"top-heavy. " + armorText + ", you notice your breasts have redistributed some of their weight, shifting toward something that is a bit more even");

						if (nowAnthro)
						{
							sb.Append(", though they get smaller as they go down.");
						}
						else
						{
							sb.Append(".");
						}
					}

					foreach (ValueDifference<BreastData> item in changedItems)
					{
						if (item.newValue.cupSize > item.oldValue.cupSize)
						{
							sb.Append(" Your " + Utils.NumberAsPlace(item.newValue.currBreastRowIndex + 1) + " row has gotten larger, and is now "
								+ item.newValue.cupSize.AsText(true) + ".");
						}
						else
						{
							sb.Append(" Your " + Utils.NumberAsPlace(item.newValue.currBreastRowIndex + 1) + " row has gotten smaller, and is now "
								+ item.newValue.cupSize.AsText(true) + ".");
						}
					}
				}

				return sb.ToString();
			}

			protected override string UpdateAndGrowAdditionalRowText(Creature target, BreastCollectionData oldBreastData, bool doCrit, bool uberCrit)
			{
				bool wearingUpper = target.wearingArmor || target.wearingUpperGarment;

				StringBuilder sb = new StringBuilder();

				//first, check to see if we grew any rows larger so we could allow this new row.
				ValueDifference<BreastData>[] changedItems = target.genitals.allBreasts.ChangedBreastRows(oldBreastData, true).Where(x => x.oldValue.cupSize < x.newValue.cupSize).ToArray();

				if (changedItems.Length > 0)
				{
					string feelText = wearingUpper ? "feel constrained and painful against your top" : "feel heavier";

					sb.Append(GlobalStrings.NewParagraph() + "Your " + changedItems[0].oldValue.LongDescription() + feelText + " as they grow larger by the moment, " +
						"finally stopping as they reach " + changedItems[0].newValue.cupSize.AsText() + " size.");
					if (changedItems.Length == oldBreastData.breasts.Count)
					{
						sb.Append(" Your remaining rows quickly follow suit, though each stops when it is slightly smaller than it's predecessor.");
					}
					else if (changedItems.Length > 2)
					{
						sb.Append(" Several of your remaining rows quickly follow suit, though each stops when it is slightly smaller than it's predecessor.");
					}
					if (changedItems.Length > 1)
					{
						sb.Append(" It is quickly followed by another row, which grows until it is is slightly smaller than the previous row.");
					}
				}
				Breasts lastRow = target.breasts[target.breasts.Count - 1];
				//Had 1 row to start
				if (target.breasts.Count == 2)
				{
					if (lastRow.cupSize == CupSize.FLAT)
					{
						sb.Append(GlobalStrings.NewParagraph() + "A second set of breasts forms under your current pair, stopping while they are still fairly flat and masculine looking.");
					}
					else
					{
						sb.Append(GlobalStrings.NewParagraph() + "A second set of breasts bulges forth under your current pair, stopping as they reach " +
							lastRow.cupSize.AsText() + "s.");
					}

					sb.Append(" A sensitive nub grows on the summit of each new tit, becoming a new nipple.");
				}
				//Many breast Rows - requires larger primary tits
				else
				{
					if (lastRow.cupSize == CupSize.FLAT)
					{
						sb.Append(GlobalStrings.NewParagraph() + "Your abdomen tingles and twitches as a new row of breasts sprouts below the others. " +
							"Your new breasts stay flat and masculine, not growing any larger.");
					}
					else
					{
						sb.Append(GlobalStrings.NewParagraph() + "Your abdomen tingles and twitches as a new row of " + lastRow.LongDescription(preciseMeasurements: true)
							+ " sprouts below your others.");
					}

					sb.Append(" A sensitive nub grows on the summit of each new tit, becoming a new nipple.");
				}
				//Extra sensitive if crit
				if (doCrit)
				{
					if (uberCrit)
					{
						sb.Append(" You heft your new chest experimentally, exploring the new flesh with tender touches. Your eyes nearly roll back in your head " +
							"from the intense feelings.");
					}
					else
					{
						sb.Append(" You touch your new nipples with a mixture of awe and desire, the experience arousing beyond measure. You squeal in delight, " +
							"nearly orgasming, but in time finding the willpower to stop yourself.");
					}
				}

				return sb.ToString();
			}

			protected override string IncreasedToughnessText(double desiredAmount, double actualAmount)
			{
				if (actualAmount > 1)
				{
					return GlobalStrings.NewParagraph() + "You roll your shoulders and tense your arms experimentally. " +
						"You feel more durable, and your blood seems to run through you more clearly. You know you have more endurance.";
				}
				else if (actualAmount > 0)
				{
					return GlobalStrings.NewParagraph() + "Your muscles feel denser and more durable. Not so much that feel stronger, " +
						"but you feel like you can take more hits.";
				}
				else
				{
					return "";
				}
			}

			protected override string DecreasedSpeedText(double desiredAmount, double actualAmount)
			{
				if (actualAmount > 1)
				{
					return GlobalStrings.NewParagraph() + "The pepper's strong taste makes you take a couple steps back and lean against the nearest solid object. " +
						"You don't feel like you'll be moving very fast anymore.";
				}
				else if (actualAmount > 0)
				{
					return GlobalStrings.NewParagraph() + "You stumble forward, but manage to catch yourself. Still, though, you feel somewhat slower.";
				}
				else
				{
					return "";
				}
			}

			protected override string IncreasedIntelligenceText(double desiredAmount, double actualAmount)
			{
				if (actualAmount <= 0)
				{
					return "";
				}
				else
				{
					string critText = actualAmount > 1 ? "a lot " : "";
					return GlobalStrings.NewParagraph() + "The spiciness makes your head twirl, but you manage to gather yourself. " +
						"A strange sense of clarity comes over you in the aftermath, and you feel " + critText + "smarter somehow.";
				}
			}

			protected override string RestoredHornsText(Creature target, HornData oldData)
			{
				return GlobalStrings.NewParagraph() + "You feel your horns crumble, falling apart in large chunks until they flake away into nothing.";
			}

			protected override string ShrunkRowsText(Creature target, BreastCollectionData oldCollection)
			{
				//the easy way to do this is to manually compare row by row, but i'm lazy. this will do that for you, and provide a convenient pair of old and new.
				IOrderedEnumerable<ValueDifference<BreastData>> changedItems = target.genitals.allBreasts.ChangedBreastRows(oldCollection, true).Where(x => x.oldValue.cupSize > x.newValue.cupSize).OrderBy(x => x.newValue.currBreastRowIndex);

				if (changedItems.IsEmpty())
				{
					return "";
				}

				StringBuilder sb = new StringBuilder();

				bool firstRow = false;
				foreach (ValueDifference<BreastData> item in changedItems)
				{
					//Big change
					if (item.oldValue.cupSize > item.newValue.cupSize + 1)
					{

						if (firstRow)
						{
							sb.Append(GlobalStrings.NewParagraph() + "The " + item.oldValue.LongDescription() + " on your chest wobble for a second, then tighten up, " +
								"losing several cup-sizes in the process!");
						}
						else
						{
							sb.Append(" The change moves down to your " + Utils.NumberAsPlace(item.newValue.currBreastRowIndex + 1) + " row of "
								+ item.oldValue.ShortDescription() + ". They shrink greatly, losing a couple cup-sizes.");
						}
					}
					//Small change
					else
					{
						if (firstRow)
						{
							sb.Append(GlobalStrings.NewParagraph() + "All at once, your sense of gravity shifts. Your back feels a sense of relief, " +
								"and it takes you a moment to realize your " + item.oldValue.LongDescription() + " have shrunk!");
						}
						else
						{
							sb.Append(" Your " + Utils.NumberAsPlace(item.newValue.currBreastRowIndex + 1) + " row of " + item.oldValue.ShortDescription()
								+ " gives a tiny jiggle as it shrinks, losing some off its mass.");
						}
					}
				}

				return sb.ToString();
			}

			protected override string UpdateBodyText(Creature target, BodyData oldData)
			{
				return GlobalStrings.NewParagraph() + "Your " + oldData.LongEpidermisDescription() + " begins to tingle, then itch. " +
				"You reach down to scratch your arm absent-mindedly and pull your fingers away to find strands of " + target.body.ShortEpidermisDescription()
					+ ". You stare at it. Fur. Wait, you just grew fur?! What happened?! Your mind reeling, you do know one thing for sure: <b>you now have fur!</b>";
			}

			protected override string ChangedCockToWolf(Creature target, CockData oldData, int cockIndex)
			{
				return GlobalStrings.NewParagraph() + "Your " + oldData.LongDescription() + " trembles, resizing and reshaping itself into a shining, " +
					"red wolf cock with a fat knot at the base. <b>You now have a wolf cock.</b>";

			}

			protected override string BecameMaleByGrowingCock(Creature target)
			{
				string armorText = target.LowerBodyArmorTextHelper(
					"take off your " + target.armor.ItemName() + " and " + target.lowerGarment.ItemName(),
					"take off your " + target.armor.ItemName(),
					"take off your " + target.lowerGarment.ItemName(),
					"look down "
					);

				return GlobalStrings.NewParagraph() + "You double over as a pain fills your groin, and you " + armorText + " just in time to watch a " +
					"bulge push out of your body. The skin folds back and bunches up into a sheath, revealing a red, knotted wolf cock drooling pre-cum underneath it. " +
					"You take a shuddering breath as the pain dies down, leaving you with only a vague sense of lust and quiet admiration for your new endowment. " +
					SafelyFormattedString.FormattedText("You now have a wolf cock!", StringFormats.BOLD);

			}

			protected override string RestoreHairText(Creature target, HairData oldHair)
			{
				return GlobalStrings.NewParagraph() + "You reach up and feel the top of your head as it begins to tingle. You put a hand on the top of your head " +
					"and slowly pull it down. Chunks of your " + oldHair.LongDescription() + " come with, replaced by a set of normal, human hair.";
			}

			protected override string RemovedFeatheryHairText(Creature target, HairData oldHair)
			{
				return RemovedFeatheryHairTextGeneric(target, true);
			}

			protected override string RestoredBackAndWings(Creature target, WingData oldWings, BackData oldBack)
			{
				string missing;
				if (oldBack.type == BackType.SHARK_FIN && !oldWings.isDefault)
				{
					missing = "shark-like fin and wings are both";
				}
				else if (oldBack.type == BackType.SHARK_FIN)
				{
					missing = "shark-like fin is";
				}
				else
				{
					missing = "wings are";
				}

				return GlobalStrings.NewParagraph() + "A wave of tightness spreads through your back, and it feels as if someone is stabbing a dagger into your spine."
						 + " After a moment the pain passes, though your " + missing + " gone!";
			}

			protected override string RestoredTongueText(Creature target, TongueData oldData)
			{
				return GlobalStrings.NewParagraph() + "You lick the roof of your mouth, noticing that your tongue feels different. It then hits you-- " +
					"<b>You have a human tongue!</b>";
			}

			protected override string RestoredEyesText(Creature target, EyeData oldData)
			{
				if (oldData.type == EyeType.SAND_TRAP)
				{
					return GlobalStrings.NewParagraph() + "You feel a twinge in your eyes and you blink. It feels like black cataracts have just fallen away from you, " +
						"and you know without needing to see your reflection that your eyes have gone back to looking human.";
				}
				else
				{
					StringBuilder sb = new StringBuilder();

					sb.Append(GlobalStrings.NewParagraph() + "You blink and stumble, a wave of vertigo threatening to pull your " + target.feet.ShortDescription()
						+ " from under you. As you steady and open your eyes, you realize something seems different. Your vision is changed somehow.");
					if (target.eyes.count > 2)
					{
						if (target.eyes.type == EyeType.SPIDER)
						{
							sb.Append(" Your extra arachnid eyes are gone - ");
						}
						else
						{
							sb.Append("Your extra eyes are gone, leaving you with just two. Additionally, ");
						}
					}
					else if (target.eyes.count == 1)
					{
						sb.Append("Wait... eyes? Well, it looks like");
					}

					if (target.eyes.count != 2)
					{
						sb.Append(SafelyFormattedString.FormattedText("you", StringFormats.BOLD));
					}

					else
					{
						sb.Append(SafelyFormattedString.FormattedText("You", StringFormats.BOLD));
					}
					sb.Append(SafelyFormattedString.FormattedText(" have normal, human eyes!", StringFormats.BOLD));

					return sb.ToString();
				}
			}

			protected override string RemovedBasiliskHair(Creature target, HairData oldData)
			{
				StringBuilder sb = new StringBuilder();


				if (oldData.type == HairType.BASILISK_PLUME)
				{
					// TF blurb derived from losing feathery hair
					//(long):
					if (oldData.length >= 5)
					{
						sb.Append(GlobalStrings.NewParagraph() + "A lock of your feathery plume droops over your eye.  Before you can blow the offending down away, "
								+ " you realize the feather is collapsing in on itself."
								+ " It continues to curl inward until all that remains is a normal strand of hair.");
					}
					//(short)
					else
					{
						sb.Append(GlobalStrings.NewParagraph() + "You run your fingers through your feathery plume while you await the effects of the item you just ingested."
								+ " While your hand is up there, it detects a change in the texture of your feathers.They're completely disappearing,"
								+ " merging down into strands of regular hair.");
					}

					sb.Append(GlobalStrings.NewParagraph() + "<b>Your hair is no longer feathery!</b>");
				}
				else
				{
					sb.Append(GlobalStrings.NewParagraph() + "You feel a tingling on your scalp. You reach up to your basilisk spines to find out what is happening.The moment"
								  + " your hand touches a spine, it comes loose and falls in front of you. One after another the other spines fall out, "
								  + " until all the spines that once decorated your head now lay around you, leaving you with a bald head.");
					sb.Append(GlobalStrings.NewParagraph() + "<b>You realize that you'll grow normal human hair again!</b>");
				}

				return sb.ToString();
			}
		}
	}
}
