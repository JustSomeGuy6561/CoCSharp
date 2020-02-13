//HorseTransformations.cs
//Description:
//Author: JustSomeGuy
//1/18/2020 7:02:17 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Settings.Gameplay;
using CoC.Frontend.StatusEffect;
using System.Linq;
using System.Text;

namespace CoC.Frontend.Transformations
{
	internal abstract class HorseTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;
			//1/6 of 2, 1/2 of 1, 1/3 of 0.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 3 });
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

			//For all of these, any text regarding the transformation should be instead abstracted out as an abstract string function. append the result of this abstract function
			//to the string builder declared above (aka sb.Append(FunctionCall(variables));) string builder is just a fancy way of telling the compiler that you'll be creating a
			//long string, piece by piece, so don't do any crazy optimizations first.

			//the initial text for starting the transformation. feel free to add additional variables to this if needed.
			sb.Append(InitialTransformationText(target));

			//Add any free changes here - these can occur even if the change count is 0. these include things such as change in stats (intelligence, etc)
			//change in height, hips, and/or butt, or other similar stats.

			//CHANCE OF BAD END - 20% if face/tail/skin/cock are appropriate.
			if (target.body.IsFurBodyType() && target.face.type == FaceType.HORSE && target.tail.type == TailType.HORSE && target.lowerBody.type == LowerBodyType.HOOVED
				&& target is IExtendedCreature extended)
			{
				//WARNINGS
				//First warning
				if (!extended.extendedData.hasHorseWarning)
				{

					extended.extendedData.hasHorseWarning = true;
				}
				else if (!extended.extendedData.resistsTFBadEnds)
				{
					//33% chance to roll a repeat warning, and luck out.
					if (Utils.Rand(3) == 0)
					{

						extended.extendedData.horseWarningCount++;
					}
					//otherwise, Bad End
					{
						isBadEnd = true;


						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

			}
			//Stat changes first

			if (target is CombatCreature cc)
			{
				//STRENGTH
				if (Utils.RandBool())
				{
					//Maxxed
					if (cc.relativeStrength > 60)
					{
					}
					//NOT MAXXED
					else
					{
						cc.IncreaseStrength(1);
					}
				}
				//TOUGHNESS

				if (Utils.RandBool())
				{
					//MAXXED ALREADY
					if (cc.relativeToughness >= 75)
					{
						//outputText("\n\nYour body is as tough and solid as a ");
						//if (target.gender.HasFlag(Gender.MALE)) outputText("stallion's.");
						//else outputText("mare's.");
					}
					//NOT MAXXED
					else
					{
						cc.IncreaseToughness(1.25f);
						//outputText("\n\nYour body suddenly feels tougher and more resilient.");
					}
				}
				//INTELLECT
				if (Utils.Rand(3) == 0)
				{
					var oldRelativeInt = cc.relativeIntelligence;

					if (cc.relativeIntelligence < 10 && cc.relativeIntelligence > 5)
					{
						cc.DecreaseIntelligence(1);
					}
					else if (cc.relativeIntelligence <= 20)
					{
						cc.DecreaseIntelligence(2);
					}
					else if (cc.relativeIntelligence <= 30)
					{
						cc.DecreaseIntelligence(3);
					}
					else if (cc.relativeIntelligence <= 50)
					{
						cc.DecreaseIntelligence(4);
					}
					else if (cc.relativeIntelligence > 50)
					{
						cc.DecreaseIntelligence(5);
					}

					//dumb down text. text is responsible for figuring out how dumb you became
				}
			}



			//this will handle the edge case where the change count starts out as 0.
			if (remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			//Any transformation related changes go here. these typically cost 1 change. these can be anything from body parts to gender (which technically also changes body parts,
			//but w/e). You are required to make sure you return as soon as you've applied changeCount changes, but a single line of code can be applied at the end of a change to do
			//this for you.

			//paste this line after any tf is applied, and it will: automatically decrement the remaining changes count. if it becomes 0 or less, apply the total number of changes
			//underwent to the target's change count (if applicable) and then return the StringBuilder content.
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
			if (target.womb.canRemoveOviposition && Utils.Rand(5) == 0)
			{
				if (target.womb.ClearOviposition())
				{

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Restore arms to become human arms again
			if (Utils.Rand(4) == 0 && !target.arms.isDefault)
			{
				target.RestoreArms();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Remove feathery hair
			if (RemoveFeatheryHair(target))
			{
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//
			//SEXUAL CHARACTERISTICS
			//
			//MALENESS.
			//rand of 1.5. rand takes an int in vanilla. wtf. that would have implicitely cast it to 1, even though typechecking is on. it would have always been true.
			//regardless, i'm assuming a 1 in 1.5 or 2 in 3 chance.
			if (target.gender.HasFlag(Gender.MALE) && Utils.Rand(3) < 2)
			{
				//If cocks that aren't horsified!
				if (!target.cocks.All(x => x.type == CockType.HORSE))
				{
					target.DeltaCreatureStats(lib: 5, sens: 4, lus: 35);
					//Transform a cock and store it's index value to talk about it.
					var horseCock = target.cocks.First(x => x.type != CockType.HORSE);

					target.genitals.UpdateCockWithLength(horseCock, CockType.HORSE, horseCock.length + 4);
					if (horseCock.girth < 2)
					{
						horseCock.SetGirth(2);
					}

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
				//Players cocks are all horse-type - increase size!
				else
				{
					float sizeDelta;
					//single cock
					if (target.cocks.Count == 1)
					{
						//1-3
						sizeDelta = Utils.Rand(3) + 1;

					}
					else
					{
						sizeDelta = Utils.Rand(4) + 1;
					}

					var smallest = target.genitals.ShortestCock();

					smallest.IncreaseLength(sizeDelta);
					target.DeltaCreatureStats(sens: 1, lus: 10);

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
				//Chance of thickness + daydream
				if (Utils.Rand(2) == 0 && target.cocks.Any(x => x.type == CockType.HORSE))
				{
					var thinnest = target.genitals.ThinnestCock();
					thinnest.IncreaseThickness(0.5f);

					target.DeltaCreatureStats(lib: 0.5f, lus: 10);

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
				//Chance of ball growth if not 3" yet
				if (Utils.Rand(2) == 0 && target.balls.size <= 3 && target.cocks.Any(x => x.type == CockType.HORSE))
				{
					if (!target.hasBalls)
					{
						target.genitals.GrowBalls();
						target.DeltaCreatureStats(lib: 2, lus: 5);
					}
					else
					{
						target.genitals.EnlargeBalls(1);
						target.DeltaCreatureStats(lib: 1, lus: 3);
					}

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//FEMALE
			if (target.gender.HasFlag(Gender.FEMALE))
			{
				Vagina leastWet = target.genitals.SmallestVaginalByWetness();

				if (leastWet.wetness <= VaginalWetness.NORMAL)
				{
					leastWet.IncreaseWetness(1);
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				Vagina tightest = target.genitals.TightestVagina();

				if (tightest.looseness <= VaginalLooseness.GAPING && Utils.RandBool())
				{
					tightest.IncreaseLooseness(1);
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				if ((!target.HasTimedEffect<Heat>() || target.GetTimedEffectData<Heat>().totalAddedLibido < 30) && Utils.Rand(2) == 0)
				{
					if (target.GoIntoHeat())
					{
						if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

				if (!hyperHappy && target.genitals.BiggestCupSize() > CupSize.B && Utils.RandBool())
				{
					foreach (var breastRow in target.breasts)
					{
						if (breastRow.cupSize > CupSize.B)
						{
							if (breastRow.cupSize > CupSize.E_BIG)
							{
								breastRow.ShrinkBreasts(2);
							}
							else
							{
								breastRow.ShrinkBreasts(1);
							}
						}
					}

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//NON - GENDER SPECIFIC CHANGES
			//Tail -> Ears -> Fur -> Face
			//Remove odd eyes
			if (Utils.Rand(5) == 0 && !target.eyes.isDefault)
			{
				target.RestoreEyes();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//HorseFace - Req's Fur && Ears
			if (target.face.type != FaceType.HORSE && target.body.isFurry && Utils.Rand(5) == 0 && target.ears.type == EarType.HORSE)
			{
				target.UpdateFace(FaceType.HORSE);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Fur - if has horse tail && ears and not at changelimit
			if (!target.body.IsFurBodyType() && Utils.Rand(4) == 0 && target.tail.type == TailType.HORSE)
			{
				target.UpdateBody(BodyType.SIMPLE_FUR);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			// Hooves - Tail
			if (target.lowerBody.type != LowerBodyType.HOOVED && target.tail.type == TailType.HORSE && Utils.Rand(5) == 0)
			{
				target.UpdateLowerBody(LowerBodyType.HOOVED);

				if (target is CombatCreature speedyLegs)
				{
					speedyLegs.IncreaseSpeed(1);
				}

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Ears - requires tail
			if (target.ears.type != EarType.HORSE && target.tail.type == TailType.HORSE && Utils.Rand(3) == 0)
			{
				target.UpdateEars(EarType.HORSE);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Tail - no-prereq
			if (target.tail.type != TailType.HORSE && Utils.Rand(2) == 0)
			{
				target.UpdateTail(TailType.HORSE);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			// Remove gills
			if (Utils.Rand(4) == 0 && !target.gills.isDefault)
			{
				target.RestoreGills();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			if (Utils.Rand(3) == 0)
			{
				if (target.build.ChangeMuscleToneToward(60, 1) != 0)
				{
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//FAILSAFE CHANGE
			if (remainingChanges == changeCount && target is CombatCreature cc2)
			{
				cc2.AddHP(20);
				target.DeltaCreatureStats(lus: 3);
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