//SandTrapTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:41:27 PM
using System.Linq; //Useful if you're going to do any special queries on collections. (if you don't know about Linq, feel free to remove this. if you do, you'll probably want it.)
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;

namespace CoC.Frontend.Transformations
{
	internal abstract class SandTrapTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 3, 3 });
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
			if (remainingChanges <= 0)
			{
				return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Any transformation related changes go here. these typically cost 1 change. these can be anything from body parts to gender (which technically also changes body parts,
			//but w/e). You are required to make sure you return as soon as you've applied changeCount changes, but a single line of code can be applied at the end of a change to do
			//this for you.

			//paste this line after any tf is applied, and it will: automatically decrement the remaining changes count. if it becomes 0 or less, apply the total number of changes
			//underwent to the target's change count (if applicable) and then return the StringBuilder content.
			//if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);


			if (target is CombatCreature cc)
			{
				//Speed Increase:
				if (cc.relativeSpeed < 100 && Utils.Rand(3) == 0)
				{

					cc.DeltaCombatCreatureStats(spe: 1);
				}
				//Strength Loss:
				else if (cc.relativeStrength > 40 && Utils.Rand(3) == 0)
				{
					cc.DeltaCombatCreatureStats(str: -1);
				}
			}
			//Sensitivity Increase:
			if (target.relativeSensitivity < 70 && target.hasCock && Utils.Rand(3) == 0)
			{
				target.DeltaCreatureStats(sens: 5);
			}
			//Libido Increase:
			if (target.relativeLibido < 70 && target.hasVagina && Utils.Rand(3) == 0)
			{
				target.DeltaCreatureStats(lib: 2);
				if (target.relativeLibido < 30)
				{
					target.DeltaCreatureStats(lib: 2);
				}
			}
			//Body Mass Loss:
			if (target.build.thickness > 40 && Utils.Rand(3) == 0)
			{
				target.build.ChangeThicknessToward(40, 3);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Thigh Loss: (towards \"girly\")
			if (target.hips.size >= 10 && Utils.Rand(4) == 0)
			{

				target.hips.ShrinkHips();
				if (target.hips.size > 15)
				{
					target.hips.ShrinkHips((byte)(2 + Utils.Rand(3)));
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Thigh Gain: (towards \"girly\")
			if (target.hips.size < 6 && Utils.Rand(4) == 0)
			{
				target.hips.GrowHips();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Breast Loss: (towards A cup)
			CupSize largestSize = target.genitals.BiggestCupSize();

			CupSize targetCup = EnumHelper.Max(target.genitals.smallestPossibleCupSize, CupSize.A);

			//Mod note: both shrink and gain combined into one check.
			if (largestSize != targetCup && Utils.Rand(4) == 0)
			{
				foreach (Breasts breast in target.breasts)
				{
					//Mod Note: Generally i prefer the enum value over magic constants, but i have no idea what 70 is in cup land and don't want to look it up.
					if (breast.cupSize > (CupSize)70)
					{
						breast.ShrinkBreasts((byte)(Utils.Rand(3) + 15));
					}
					else if (breast.cupSize > (CupSize)50)
					{
						breast.ShrinkBreasts((byte)(Utils.Rand(3) + 10));
					}
					else if (breast.cupSize > (CupSize)30)
					{
						breast.ShrinkBreasts((byte)(Utils.Rand(3) + 7));
					}
					else if (breast.cupSize > (CupSize)15)
					{
						breast.ShrinkBreasts((byte)(Utils.Rand(3) + 4));
					}
					else if (breast.cupSize > targetCup)
					{
						breast.ShrinkBreasts((byte)(2 + Utils.Rand(2)));
					}

					//Mod note: if we shrunk it below the target cup size or it already was,
					if (breast.cupSize < targetCup)
					{
						breast.SetCupSize(targetCup);
					}
				}
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Penis Reduction towards 3.5 Inches:
			if (target.genitals.LongestCockLength() >= 3.5 && target.hasCock && Utils.Rand(2) == 0)
			{

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Testicle Reduction:
			if (target.balls.count > 0 && target.hasCock && (target.balls.size > 1 || target.balls.count > 1) && Utils.Rand(4) == 0)
			{
				if (target.balls.size > 20)
				{
					target.balls.ShrinkBalls(6);
				}
				else if (target.balls.size > 15)
				{
					target.balls.ShrinkBalls(5);
				}
				else if (target.balls.size > 12)
				{
					target.balls.ShrinkBalls(4);
				}
				else if (target.balls.size > 10)
				{
					target.balls.ShrinkBalls(3);
				}
				else if (target.balls.size > 8)
				{
					target.balls.ShrinkBalls(2);
				}
				else if (target.balls.size > 1)
				{
					target.balls.ShrinkBalls(1);
				}
				else
				{
					target.balls.MakeUniBall();
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Anal Wetness Increase:
			if (target.ass.wetness < AnalWetness.SLIME_DROOLING && Utils.Rand(4) == 0)
			{
				//Anal Wetness Increase Final (always loose):
				target.ass.IncreaseWetness();
				//buttChange(30,false,false,false);
				if (target.ass.looseness < AnalLooseness.STRETCHED)
				{
					target.ass.IncreaseLooseness();
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				target.DeltaCreatureStats(sens: 2);
			}
			//Fertility Decrease:
			if (target.hasVagina && Utils.Rand(4) == 0)
			{
				target.DeltaCreatureStats(sens: -2);

				target.fertility.DecreaseFertility((byte)(1 + Utils.Rand(3)));
				if (target.fertility.currentFertility < 4)
				{
					target.fertility.SetFertility(4);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Male Effects
			if (target.gender == Gender.MALE)
			{
				//Femininity Increase Final (max femininity allowed increased by +10):
				if (Utils.Rand(4) == 0)
				{
					if (target.femininity < 70 && target.femininity >= 60)
					{
						if (target.femininity.femininityLimitedByGender)
						{
							target.femininity.ActivateAndrogyny();
						}
						target.femininity.IncreaseFemininity(10);
						if (target.femininity > 70)
						{
							target.femininity.SetFemininity(70);
						}

						if (--remainingChanges <= 0)
						{
							return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
						}
					}
					//Femininity Increase:
					else
					{
						target.femininity.IncreaseFemininity(10);
						if (--remainingChanges <= 0)
						{
							return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
						}
					}
				}
				//Muscle tone reduction:
				if (target.build.muscleTone > 20 && Utils.Rand(4) == 0)
				{
					target.build.DecreaseMuscleTone(10);
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}
			//Female Effects
			else if (target.gender == Gender.FEMALE)
			{
				//Masculinity Increase:
				if (target.femininity > 30 && Utils.Rand(4) == 0)
				{
					target.femininity.IncreaseMasculinity(10);
					if (target.femininity < 30)
					{
						target.femininity.SetFemininity(30);
						//Masculinity Increase Final (max masculinity allowed increased by +10):

						if (target.femininity.femininityLimitedByGender)
						{
							target.femininity.ActivateAndrogyny();
						}
					}

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
				//Muscle tone gain:
				if (target.build.muscleTone < 80 && Utils.Rand(4) == 0)
				{
					target.build.GainMuscle(10);
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}
			//Neck restore
			if (target.neck.type != NeckType.HUMANOID && Utils.Rand(4) == 0)
			{
				target.RestoreNeck();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Rear body restore
			if (!target.back.isDefault && Utils.Rand(5) == 0)
			{
				target.RestoreBack();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Ovi perk loss
			if (target.womb.canRemoveOviposition && Utils.Rand(5) == 0)
			{
				target.womb.ClearOviposition();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Nipples Turn Black:
			if (!target.genitals.hasBlackNipples && Utils.Rand(6) == 0)
			{
				target.genitals.SetBlackNipples(true);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Remove odd eyes
			if (target.eyes.count != 2 && target.eyes.type != EyeType.SAND_TRAP && Utils.Rand(2) == 0)
			{
				target.RestoreEyes();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//PC Trap Effects
			if (target.eyes.type != EyeType.SAND_TRAP && Utils.Rand(4) == 0)
			{
				//Eyes Turn Black:
				target.UpdateEyes(EyeType.SAND_TRAP);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Vagina Turns Black:
			//has a vagina and any vagina is not a sand trap (aka, not all of the vaginas are sand trap vags)
			if (target.hasVagina && !target.genitals.OnlyHasVaginasOfType(VaginaType.SAND_TRAP) && Utils.Rand(4) == 0)
			{
				//(Wet:
				//(Corruption <50:
				target.DeltaCreatureStats(sens: 2, lus: 10);

				target.vaginas.ForEach(x => target.genitals.UpdateVagina(x, VaginaType.SAND_TRAP));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

			}
			//Dragonfly Wings:
			if (target.wings.type != WingType.DRAGONFLY && Utils.Rand(4) == 0)
			{
				//Wings Fall Out: You feel a sharp pinching sensation in your shoulders and you cringe slightly.  Your former dragonfly wings make soft, papery sounds as they fall into the dirt behind you.
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				target.UpdateWings(WingType.DRAGONFLY);
			}
			if (remainingChanges == changeCount)
			{

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