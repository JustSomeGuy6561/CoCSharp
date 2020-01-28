//GoblinTransformations.cs
//Description:
//Author: JustSomeGuy
//1/21/2020 2:47:56 AM
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;
using System.Text;
using System.Linq;

namespace CoC.Frontend.Transformations
{
	internal abstract class GoblinTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;

		//trying to think of a good way to do nipple piercings for goblins when you go full goblin without being too intrusive.
		//can't. oh well. but hey, at least it removes inverted nipples without requiring piercings, i guess

		//fun fact: this is (as of writing) the only way to restore nipples back to normal after you've made them fuckable. it's now possible to remove fuckable nipples just
		//by shrinking them down (they become inverted), but they're still not 'normal.' this will convert inverted nipples back to normal. so, if you want to remove fuckable nipples,
		//shrink them down, then proc this a few times. It's also possible that nipple piercings remove inverted nipples over time, but as of writing, that's not (currently) implemented
		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//rolls of 1/2, 1/3, 1/4, and 1/5 for extra changes. i don't feel like figuring out the odds, so here's the simple split:
			//the most changes (+4) has an odds of 1/120, and the least changes (+0) have an odds of 1/5. the remaining change counts vary, totaling 19/24.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 3, 4, 5 });
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

			//For all of these, any text regarding the transformation should be instead abstracted out as an abstract string function. append the result of this abstract function
			//to the string builder declared above (aka sb.Append(FunctionCall(variables));) string builder is just a fancy way of telling the compiler that you'll be creating a
			//long string, piece by piece, so don't do any crazy optimizations first.

			//the initial text for starting the transformation. feel free to add additional variables to this if needed.
			sb.Append(InitialTransformationText(target));

			//Add any free changes here - these can occur even if the change count is 0. these include things such as change in stats (intelligence, etc)
			//change in height, hips, and/or butt, or other similar stats.

			target.IncreaseLust(15);
			//Stronger
			if (target is CombatCreature cc)
			{
				if (cc.relativeStrength > 50)
				{
					if (cc.relativeStrength > 90)
					{
						cc.DecreaseStrengthByPercent(3);
					}
					else if (cc.relativeStrength > 70)
					{
						cc.DecreaseStrengthByPercent(2);
					}
					else
					{
						cc.DecreaseStrengthByPercent(1);
					}
				}
				///Less tough
				if (cc.relativeToughness > 50)
				{
					if (cc.relativeToughness > 90)
					{
						cc.DecreaseToughnessByPercent(4);
					}
					else if (cc.relativeToughness > 70)
					{
						cc.DecreaseToughnessByPercent(2);
					}
					else
					{
						cc.DecreaseToughnessByPercent(1);
					}
				}
				//Speed boost
				if (Utils.Rand(3) == 0 && cc.relativeSpeed < 50)
				{
					cc.IncreaseSpeed(1 + Utils.Rand(2));
				}
			}
			//antianemone corollary:
			if (target.hair.type == HairType.ANEMONE && Utils.Rand(2) == 0)
			{
				//-insert anemone hair removal into them under whatever criteria you like, though hair removal should precede abdomen growth; here's some sample text:
				target.RestoreHair();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Shrink
			if (Utils.Rand(2) == 0 && target.heightInInches > 48)
			{
				target.build.DecreaseHeight((byte)(1 + Utils.Rand(5)));
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//this will handle the edge case where the change count starts out as 0.
			if (remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			//Normal transforms (Cost 1)
			//if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			//Neck restore
			if (!target.neck.isDefault && Utils.Rand(4) == 0)
			{
				target.RestoreNeck();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Rear body restore
			if (!target.back.isDefault && Utils.Rand(5) == 0)
			{
				target.RestoreBack();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Ovi perk loss
			if (target.womb is PlayerWomb playerWomb && playerWomb.canClearOviposition && Utils.Rand(5) == 0)
			{
				playerWomb.ClearOviposition();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Restore arms to become human arms again
			if (Utils.Rand(4) == 0 && !target.arms.isDefault)
			{
				target.RestoreArms();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//SEXYTIEMS
			//Multidick killa!
			if (target.cocks.Count > 1 && Utils.Rand(3) == 0)
			{
				target.genitals.RemoveCock();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Boost vaginal capacity without gaping
			if (Utils.Rand(3) == 0 && target.hasVagina && target.genitals.standardBonusVaginalCapacity < 40)
			{
				target.genitals.IncreaseBonusVaginalCapacity(5);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Boost fertility
			if (Utils.Rand(4) == 0 && target.fertility.totalFertility < 40 && target.hasVagina)
			{
				target.fertility.IncreaseFertility((byte)(2 + Utils.Rand(5)));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Shrink primary dick to no longer than 12 inches
			else if (target.cocks.Count == 1 && Utils.Rand(2) == 0 && !hyperHappy)
			{
				if (target.cocks[0].length > 12)
				{
					float delta = target.cocks[0].DecreaseLength(1 + Utils.Rand(3));

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//GENERAL APPEARANCE STUFF BELOW
			//REMOVAL STUFF
			//Removes wings!
			if ((target.wings.type != WingType.NONE) && Utils.Rand(4) == 0)
			{
				target.RestoreWings();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Removes antennae!
			if (target.antennae.type != AntennaeType.NONE && Utils.Rand(3) == 0)
			{
				target.RestoreAntennae();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Remove odd eyes
			if (Utils.Rand(5) == 0 && target.eyes.count != 2)
			{
				target.RestoreEyes();
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//-Remove extra breast rows
			if (target.breasts.Count > 1 && Utils.Rand(3) == 0)
			{
				target.genitals.RemoveExtraBreastRows();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Skin/fur
			if (target.body.type != BodyType.HUMANOID && Utils.Rand(4) == 0 && target.face.type == FaceType.HUMAN)
			{
				target.RestoreBody();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//skinTone
			if (!Species.GOBLIN.availableTones.Contains(target.body.primarySkin.tone) && Utils.Rand(2) == 0)
			{
				if (Utils.Rand(10) != 0)
				{
					target.body.ChangeMainSkin(Tones.DARK_GREEN);
				}
				else if (Utils.RandBool())
				{
					target.body.ChangeMainSkin(Tones.PALE_YELLOW);
				}
				else
				{
					target.body.ChangeMainSkin(Tones.GRAYISH_BLUE);
				}

				//MOD note: rathazal mixology called here - how do we want to standardize that?

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Face!
			if (target.face.type != FaceType.HUMAN && Utils.Rand(4) == 0 && target.ears.type == EarType.ELFIN)
			{
				target.UpdateFace(FaceType.HUMAN);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Ears!
			if (target.ears.type != EarType.ELFIN && Utils.Rand(3) == 0)
			{
				target.UpdateEars(EarType.ELFIN);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			// Remove gills
			if (Utils.Rand(4) == 0 && !target.gills.isDefault)
			{
				target.RestoreGills();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Nipples Turn Back:
			if (target.genitals.hasBlackNipples && Utils.Rand(3) == 0)
			{
				target.genitals.SetBlackNipples(false);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//MOD: Added: remove inverted nipples. this by entension is able to remove fuckable nipples, assuming you shrunk them down until they became inverted.
			if (target.genitals.nippleType.IsInverted() && Utils.Rand(3) == 0)
			{
				target.genitals.SetNippleStatus(NippleStatus.NORMAL);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Debugcunt
			if (Utils.Rand(3) == 0 && target.hasVagina && target.vaginas.Any(x => !x.isDefault))
			{
				foreach (var vag in target.vaginas)
				{
					if (!vag.isDefault)
					{
						target.genitals.RestoreVagina(vag);
					}
				}

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			if (Utils.Rand(4) == 0 && target.ass.wetness > target.ass.minWetness)
			{
				if (target.ass.wetness - target.ass.minWetness > 1)
				{
					target.ass.DecreaseWetness(2);
				}
				else
				{
					target.ass.DecreaseWetness();
				}

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			if (Utils.Rand(3) == 0)
			{
				if (Utils.Rand(2) == 0) target.femininity.ChangeFemininityToward(85, 3);
				if (Utils.Rand(2) == 0) target.build.ChangeThicknessToward(20, 3);
				if (Utils.Rand(2) == 0) target.build.ChangeMuscleToneToward(15, 5);
			}

			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract bool InitialTransformationText(Creature target);
	}
}