using CoC.Backend.Creatures;
using CoC.Frontend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Perks;
using System;
using System.Linq;
using System.Text;
using CoC.Frontend.StatusEffect;
using CoC.Backend.BodyParts;

namespace CoC.Frontend.Transformations
{
	internal abstract class GenericTransformationBase
	{
		protected static string newLines => Utils.NewParagraph();

		//tfs can be applied to any creature, potentially - don't assume it's the player. but you can always check if the target is a Player object, 
		//and if it is, do Player related things. 
		protected internal abstract string DoTransformation(Creature target, out bool isBadEnd);

		protected int GenerateChanceCount(Creature target, int[] extraRolls = null, int initialCount = 1, int minimumCount = 1)
		{
			initialCount += target.GetExtraData()?.deltaTransforms ?? 0;
			if (extraRolls != null)
			{
				initialCount += extraRolls.Sum(x => Utils.Rand(x) == 0 ? 1 : 0);
			}

			return Math.Max(initialCount, minimumCount);
		}

		protected string ApplyAndReturn(Creature target, StringBuilder builder, int transformCount)
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

		protected string RemovedFeatheryHairTextViaDecoction(Creature creature)
		{
			if (creature.hair.length >= 6)
			{
				return Environment.NewLine + Environment.NewLine + "A lock of your downy-soft feather-hair droops over your eye. Before you can blow the offending down away, " +
					"you realize the feather is collapsing in on itself. It continues to curl inward until all that remains is a normal strand of hair. +" +
					SafelyFormattedString.FormattedText("Your hair is no longer feathery!", StringFormats.BOLD);
			}
			else
			{
				return Environment.NewLine + Environment.NewLine + "You run your fingers through your downy-soft feather-hair while you await for the effects of the item you just ingested" +
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
			return Environment.NewLine+ Environment.NewLine + "Another change in your uterus ripples through your reproductive systems. Somehow you know you've lost " +
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

		protected string EnterHeatTextGeneric(Creature target, bool isIncrease)
		{
			Heat heatEffect = target.statusEffects.GetStatusEffect<Heat>();
			if (heatEffect is null) return null;
			else if (isIncrease)
			{
				return heatEffect.IncreasedHeatText();
			}
			else
			{
				return heatEffect.obtainText();
			}
		}

		//add any common generic transformation related functions here - generic texts, common tfs, etc. 

		//helper function for mammals that have multiple breast rows that all breast rows must be ordered - that is, the largest is the first row, and smallest is the last row. 
		//it will grow up to number of rows, or until the target has the provided max rows, or the overall max rows for a given creature, whichever is reached first, but only if 
		//the target is female and the breasts are correctly ordered. if correct size is set, they will be ordered before any new rows are added. 
		//It's recommended you store the old data for each row before calling this, in a collection of your choosing. 

		//so if add 2 rows, and current size is DD, it becomes DD, D, C. If A cup and 2 rows, and correct size, becomes B, A, FLAT.
		protected bool FemaleMammalGrowBreastRows(Creature target, int numberOfRows, int maxRows, bool correctSize = true)
		{
			maxRows = Math.Min(numberOfRows, Genitals.MAX_BREAST_ROWS);
			numberOfRows = Math.Min(maxRows - target.breasts.Count, numberOfRows);

			if (numberOfRows > 0 && target.hasVagina && target.genitals.SmallestCupSize() > CupSize.FLAT)
			{
				CupSize currSize = CupSize.FLAT.ByteEnumAdd((byte)numberOfRows);

				//check and correct cup size if allowed to. if not return false.
				for (int x = target.breasts.Count-1; x >= 0; x--)
				{
					if (currSize >= target.breasts[x].cupSize)
					{
						if (correctSize)
						{
							target.breasts[x].SetCupSize(currSize + 1);
						}
						else
						{
							return false;
						}
					}

					currSize = target.breasts[x].cupSize;
				}

				while (numberOfRows-- > 0)
				{
					target.AddBreastRow(target.breasts[target.breasts.Count - 1].cupSize.ByteEnumSubtract(1));
				}
				return true;
			}
			else
			{
				return false;
			}
		}


	}
}
