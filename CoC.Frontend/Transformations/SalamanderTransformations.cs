//SalamanderTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:47:38 PM
using CoC.Backend.BodyParts;
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
	internal abstract class SalamanderTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 3, 4 });
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

			//For all of these, any text regarding the transformation should be instead abstracted out as an abstract string function. append the result of this abstract function
			//to the string builder declared above (aka sb.Append(FunctionCall(variables));) string builder is just a fancy way of telling the compiler that you'll be creating a
			//long string, piece by piece, so don't do any crazy optimizations first.

			//the initial text for starting the transformation. feel free to add additional variables to this if needed.
			sb.Append(InitialTransformationText(target));

			//Add any free changes here - these can occur even if the change count is 0. these include things such as change in stats (intelligence, etc)
			//change in height, hips, and/or butt, or other similar stats.

			//this will handle the edge case where the change count starts out as 0.
			if (remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			//Any transformation related changes go here. these typically cost 1 change. these can be anything from body parts to gender (which technically also changes body parts,
			//but w/e). You are required to make sure you return as soon as you've applied changeCount changes, but a single line of code can be applied at the end of a change to do
			//this for you.

			//paste this line after any tf is applied, and it will: automatically decrement the remaining changes count. if it becomes 0 or less, apply the total number of changes
			//underwent to the target's change count (if applicable) and then return the StringBuilder content.
			//if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);


			//clear screen
			if (target is CombatCreature cc)
			{
			//Statistical changes:
			//-Reduces speed down to 70.
			if (cc.relativeSpeed > 70 && Utils.Rand(4) == 0)
			{
				cc.DeltaCombatCreatureStats(spe: -1);
			}
			//-Reduces intelligence down to 60.
			if (cc.relativeIntelligence > 60 && Utils.Rand(4) == 0)
			{
				cc.DeltaCombatCreatureStats(inte: -1);
			}

			//-Raises toughness up to 90.
			//(+3 to 50, +2 to 70, +1 to 90)
			if (cc.relativeToughness < 90 && Utils.Rand(3) == 0)
			{
				//(+3)
				if (cc.relativeToughness < 50)
				{
					cc.DeltaCombatCreatureStats(tou: 3);
				}
				//(+2)
				else if (cc.relativeToughness < 70)
				{
					cc.DeltaCombatCreatureStats(tou: 2);
				}
				//(+1)
				else
				{
					cc.DeltaCombatCreatureStats(tou: 1);
				}
			}
			//-Raises strength to 80.
			if (cc.relativeStrength < 80 && Utils.Rand(3) == 0)
			{
				cc.DeltaCombatCreatureStats(str: 1);
			}
		}
			//-Raises libido up to 90.
			if (target.relativeLibido < 90 && Utils.Rand(3) == 0)
			{
				target.DeltaCreatureStats(lib: 2);
			}
			//Sexual Changes:
			//MOD: lizard dicks get a common rng roll now. The only difference is the text.
			if (target.hasCock && !target.genitals.OnlyHasCocksOfType(CockType.LIZARD) && Utils.Rand(4) == 0)
			{
				Cock nonLizard = target.cocks.First(x => x.type != CockType.LIZARD);
				//Actually xform it nau
				target.genitals.UpdateCock(nonLizard, CockType.LIZARD);
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				target.DeltaCreatureStats(lib: 3, lus: 10);
			}

			//-Breasts vanish to 0 rating if male
			if (target.genitals.BiggestCupSize() >= target.genitals.smallestPossibleMaleCupSize && target.gender == Gender.MALE && Utils.Rand(3) == 0)
			{
				//(HUEG)
				//Loop through behind the scenes and adjust all tits.
				foreach (Breasts breastRow in target.breasts)
				{
					if (breastRow.cupSize > CupSize.E_BIG)
					{
						breastRow.ShrinkBreasts(((byte)breastRow.cupSize).div(2));
					}
					else
					{
						breastRow.SetCupSize(target.genitals.smallestPossibleMaleCupSize);
					}
				}
				//(+2 speed)
				//(target as CombatCreature)?.IncreaseSpeed(2);
				target.DeltaCreatureStats(lib: 2);
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//-Nipples reduction to 1 per tit.
			if (target.genitals.hasQuadNipples && Utils.Rand(4) == 0)
			{
				target.genitals.SetQuadNipples(false);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			var smallestTits = target.genitals.SmallestBreast();
			//Increase target's breast size, if they are big DD or smaller
			if (smallestTits.cupSize <= CupSize.DD_BIG && target.gender == Gender.FEMALE && Utils.Rand(4) == 0)
			{
				smallestTits.GrowBreasts();
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//-Remove extra breast rows
			if (target.breasts.Count > 1 && Utils.Rand(3) == 0 && !hyperHappy)
			{
				target.RemoveExtraBreastRows();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Neck restore
			if (target.neck.type != NeckType.HUMANOID && Utils.Rand(4) == 0)
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
			if (target.womb.canRemoveOviposition && Utils.Rand(5) == 0)
{
target.womb.ClearOviposition();
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Physical changes:
			//Tail - 1st gain reptilian tail, 2nd unlocks enhanced with fire tail whip attack
			if (target.tail.type != TailType.LIZARD && target.tail.type != TailType.SALAMANDER && Utils.Rand(3) == 0)
			{
				target.UpdateTail(TailType.LIZARD);
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			if (target.tail.type == TailType.LIZARD && Utils.Rand(3) == 0)
			{
				target.UpdateTail(TailType.SALAMANDER);
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Legs
			if (target.lowerBody.type != LowerBodyType.SALAMANDER && target.tail.type == TailType.SALAMANDER && Utils.Rand(3) == 0)
			{
				target.UpdateLowerBody(LowerBodyType.SALAMANDER);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Arms
			if (target.arms.type != ArmType.SALAMANDER && target.lowerBody.type == LowerBodyType.SALAMANDER && Utils.Rand(3) == 0)
			{
				target.UpdateArms(ArmType.SALAMANDER);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Remove odd eyes
			if (Utils.Rand(4) == 0 && !target.eyes.isDefault)
			{
				target.RestoreEyes();

								if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Human face
			if (target.face.type != FaceType.HUMAN && Utils.Rand(4) == 0)
			{
				target.UpdateFace(FaceType.HUMAN);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Human ears
			if (target.face.type == FaceType.HUMAN && target.ears.type != EarType.HUMAN && Utils.Rand(4) == 0)
			{
				target.UpdateEars(EarType.HUMAN);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//-Skin color change
			if (!Species.SALAMANDER.availableTones.Contains(target.body.primarySkin.tone) && Utils.Rand(4) == 0)
			{
				target.body.ChangeMainSkin(Utils.RandomChoice(Species.SALAMANDER.availableTones));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Change skin to normal
			if (target.body.type != BodyType.HUMANOID && target.ears.type == EarType.HUMAN && Utils.Rand(3) == 0)
			{
				target.RestoreBody();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Removing gills
			if (Utils.Rand(4) == 0 && !target.gills.isDefault)
			{
				target.RestoreGills();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//FAILSAFE CHANGE
			if (remainingChanges == changeCount)
			{
				(target as CombatCreature)?.AddHP(100);
				target.DeltaCreatureStats(lus: 5);
			}

			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target);
	}
}