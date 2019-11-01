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

		protected string SafeReturn(Creature target, StringBuilder builder, int transformCount)
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

		//add any common generic transformation related functions here - generic texts, common tfs, etc. 
	}
}
