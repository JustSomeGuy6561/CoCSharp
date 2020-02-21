//LizardTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:50:03 PM
using System;
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Perks;
using CoC.Frontend.Perks.SpeciesPerks;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;

namespace CoC.Frontend.Transformations
{
	internal abstract class LizardTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;

		//a chance that scales with the current change count, becoming less likely if more changes have occured. it is capped with a worst case value, as well.
		private bool ScalingChance(int bestChance, int changesPerformed, int worstChance)
		{
			return Utils.Rand(Math.Min(bestChance + changesPerformed, worstChance)) == 0;
		}

		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			int changeCount = GenerateChangeCount(target, new int[] { 2, 2, 3, 4 });
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

#warning fix me
			int ngPlus(int value) => value;

			int currentChanges() => changeCount - remainingChanges;

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

			//clear screen

			//Statistical changes:
			//-Reduces speed down to 50.
			if (target.speed > ngPlus(50) && Utils.Rand(4) == 0)
			{
				target.ChangeSpeed(-1);
			}
			//-Raises toughness to 70
			//(+3 to 40, +2 to 55, +1 to 70)
			if (target.toughness < ngPlus(70) && Utils.Rand(3) == 0)
			{
				//(+3)
				if (target.toughness < ngPlus(40))
				{
					target.ChangeToughness(3);
				}
				//(+2)
				else if (target.toughness < ngPlus(55))
				{
					target.ChangeToughness(2);
				}
				//(+1)
				else
				{
					target.ChangeToughness(1);
				}
			}
			//-Reduces sensitivity.
			if (target.relativeSensitivity > 20 && Utils.Rand(3) == 0)
			{
				target.ChangeSensitivity(-1);
			}
			//Raises libido greatly to 50, then somewhat to 75, then slowly to 100.
			if (target.relativeLibido < 100 && Utils.Rand(3) == 0)
			{
				//+3 lib if less than 50
				if (target.relativeLibido < 50)
				{
					target.ChangeLibido(1);
				}
				//+2 lib if less than 75
				if (target.relativeLibido < 75)
				{
					target.ChangeLibido(1);
				}
				//+1 if above 75.
				target.ChangeLibido(1);
			}


			//Sexual Changes:
			//-Lizard dick - first one
			if (target.cocks.Any(x => x.type != CockType.LIZARD) && target.cocks.Count > 0 && ScalingChance(2, currentChanges(), 4))
			{
				//Find the first non-lizzy dick
				Cock firstNonLizard = target.cocks.First(x => x.type != CockType.LIZARD);
				//Actually xform it nau
				target.genitals.UpdateCock(firstNonLizard, CockType.LIZARD);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				target.DeltaCreatureStats(lib: 3, lus: 10);
			}
			//(CHANGE OTHER DICK)
			int lizardCocks = target.genitals.CountCocksOfType(CockType.LIZARD);
			//Requires 1 lizard cock, multiple cocks
			if (target.hasCock && (lizardCocks != target.cocks.Count || target.cocks.Count == 1) && ScalingChance(2, currentChanges(), 4))
			{
				if (target.cocks.Count > 1)
				{
					Cock nonLizard = target.cocks.First(x => x.type != CockType.LIZARD);

					target.genitals.UpdateCock(nonLizard, CockType.LIZARD);

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}

					target.DeltaCreatureStats(lib: 3, lus: 10);
				}
				else
				{
					target.genitals.AddCock(CockType.LIZARD, target.cocks[0].length, target.cocks[0].girth);

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}

					target.DeltaCreatureStats(lib: 3, lus: 10);
				}
			}

			//MOD NOTE: REMOVED. SEE RANT IN SALAMANDER TRANSFORMATIONS

			//--Worms leave if 100% lizard dicks?
			//Require mammals?
			//if (target.genitals.CountCocksOfType(CockType.LIZARD) == target.cocks.Count && target.hasStatusEffect(StatusEffects.Infested))
			//{
			//	target.removeStatusEffect(StatusEffects.Infested);
			//	if (--remainingChanges <= 0)
			//	{
			//		return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			//	}
			//}

			//-Breasts vanish to 0 rating if male
			if (target.genitals.BiggestCupSize() > target.genitals.smallestPossibleMaleCupSize && target.gender == Gender.MALE && ScalingChance(2, currentChanges(), 3))
			{
				//(HUEG)
				foreach (Breasts breast in target.breasts)
				{
					if (breast.cupSize > CupSize.E_BIG)
					{
						breast.ShrinkBreasts(((byte)breast.cupSize).div(2));
					}
					else
					{
						breast.SetCupSize(target.genitals.smallestPossibleMaleCupSize);
					}
				}
				//(+2 speed)
				target.IncreaseSpeed(2);
				target.ChangeLibido(2);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Lactation stoppage.
			if (target.genitals.isLactating && ScalingChance(2, currentChanges(), 4))
			{
				if (target.HasPerk<Feeder>())
				{
					target.RemovePerk<Feeder>();
				}
				target.genitals.SetLactationTo(LactationStatus.NOT_LACTATING);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

			}
			//-Nipples reduction to 1 per tit.
			if (target.genitals.hasQuadNipples && ScalingChance(2, currentChanges(), 4))
			{
				target.genitals.SetQuadNipples(false);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Remove extra breast rows
			if (target.breasts.Count > 1 && ScalingChance(2, currentChanges(), 3) && !hyperHappy)
			{
				target.RemoveExtraBreastRows();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-VAGs
			if (target.hasVagina && target.womb.canObtainOviposition && Species.LIZARD.Score(target) > 3 && ScalingChance(3, currentChanges(), 5))
			{
				target.womb.GrantOviposition();
				sb.Append(GrantOvipositionText(target));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Physical changes:
			//-Existing horns become draconic, max of 4, max length of 1'
			if (target.horns.type != HornType.DRACONIC && ScalingChance(3, currentChanges(), 5))
			{
				HornData oldData = target.horns.AsReadOnlyData();
				target.UpdateHorns(HornType.DRACONIC);
				sb.Append(UpdateHornsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Neck restore
			if (target.neck.type != NeckType.HUMANOID && ScalingChance(2, currentChanges(), 4))
			{
				NeckData oldData = target.neck.AsReadOnlyData();
				target.RestoreNeck();
				sb.Append(RestoredNeckText(target, oldData));
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Rear body restore
			if (!target.back.isDefault && ScalingChance(3, currentChanges(), 5))
			{
				BackData oldData = target.back.AsReadOnlyData();
				target.RestoreBack();
				sb.Append(RestoredBackText(target, oldData));
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Hair changes
			if (ScalingChance(2, currentChanges(), 4))
			{
				if (target.eyes.type == EyeType.BASILISK)
				{
					if (target.corruption > 65 && target.face.IsReptilian() && target.body.type == BodyType.REPTILE && target.hair.type != HairType.BASILISK_SPINES)
					{
						HairData oldData = target.hair.AsReadOnlyData();
						target.UpdateHair(HairType.BASILISK_SPINES);
						sb.Append(UpdateHairText(target, oldData));

						if (--remainingChanges <= 0)
						{
							return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
						}
					}
					else if (target.corruption < 15 && target.gender.HasFlag(Gender.FEMALE) && target.hair.type != HairType.BASILISK_PLUME)
					{
						target.hair.ResumeNaturalGrowth();
						HairData oldData = target.hair.AsReadOnlyData();
						target.UpdateHair(HairType.BASILISK_PLUME);
						sb.Append(UpdateHairText(target, oldData));

						if (--remainingChanges <= 0)
						{
							return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
						}
					}
				}
				else
				{
					target.hair.StopNaturalGrowth();

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}
			//Remove beard!
#warning BEARDS NOT YET IMPLEMENTED
			//Mod note: Nords are so serious about beards. ... Mai'q thinks they wish they had GLORIOUS pubes like Khajiit.

			//if (target.hasBeard() && ScalingChance(2, currentChanges(), 3))
			//{
			//	target.beard.length = 0;
			//	target.beard.style = 0;
			//}
			//Big physical changes:
			//-Legs – Draconic, clawed feet
			if (target.lowerBody.type != LowerBodyType.LIZARD && ScalingChance(3, currentChanges(), 5))
			{
				//Hooves -
				//TAURS -
				//feet types -
				//Else –
				LowerBodyData oldData = target.lowerBody.AsReadOnlyData();
				target.UpdateLowerBody(LowerBodyType.LIZARD);
				sb.Append(UpdateLowerBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			// <mod name="Predator arms" author="Stadler76">
			//Gain predator arms
			//Claw transition
			//MOD OF MOD (LOL): predator arms are a collection of arm types now, not a single type. thus, we can combine the claw and type checks.
			//now, you get the arms (and thus, the claws) if they aren't a predator type and have lizard legs, or switch to lizard arms from any other predator type.
			if ((target.arms.IsPredatorArms() || target.lowerBody.type == LowerBodyType.LIZARD) && target.body.type == BodyType.REPTILE && ScalingChance(2, currentChanges(), 3))
			{
				ArmData oldData = target.arms.AsReadOnlyData();
				target.UpdateArms(ArmType.LIZARD);
				sb.Append(UpdateArmsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			// </mod>

			//-Tail – sinuous lizard tail
			if (target.tail.type != TailType.LIZARD && target.lowerBody.type == LowerBodyType.LIZARD && ScalingChance(3, currentChanges(), 5))
			{
				//No tail
				//Yes tail
				TailData oldData = target.tail.AsReadOnlyData();
				target.UpdateTail(TailType.LIZARD);
				sb.Append(UpdateTailText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Remove odd eyes
			if (ScalingChance(3, currentChanges(), 5) && target.eyes.type != EyeType.HUMAN && !target.eyes.isReptilian)
			{
				EyeData oldData = target.eyes.AsReadOnlyData();
				target.RestoreEyes();
				sb.Append(RestoredEyesText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Ears become smaller nub-like openings?
			if (target.ears.type != EarType.LIZARD && target.tail.type == TailType.LIZARD && target.lowerBody.type == LowerBodyType.LIZARD && ScalingChance(3, currentChanges(), 5))
			{
				EarData oldData = target.ears.AsReadOnlyData();
				target.UpdateEars(EarType.LIZARD);
				sb.Append(UpdateEarsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Scales – color changes to red, green, white, blue, or black. Rarely: purple or silver.
			if (target.body.type != BodyType.REPTILE && target.ears.type == EarType.LIZARD && target.tail.type == TailType.LIZARD && target.lowerBody.type == LowerBodyType.LIZARD
				&& ScalingChance(3, currentChanges(), 5))
			{
				Species.LIZARD.GetRandomSkinTone(out Tones primary, out Tones underbody);

				target.UpdateBody(BodyType.REPTILE, primary, underbody);

				//kGAMECLASS.rathazul.addMixologyXP(20);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Lizard-like face.
			if (target.face.type != FaceType.LIZARD && target.body.type == BodyType.REPTILE && target.ears.type == EarType.LIZARD && target.tail.type == TailType.LIZARD
				&& target.lowerBody.type == LowerBodyType.LIZARD && ScalingChance(3, currentChanges(), 5))
			{
				FaceData oldData = target.face.AsReadOnlyData();
				target.UpdateFace(FaceType.LIZARD);
				sb.Append(UpdateFaceText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Lizard tongue
			if (target.tongue.type == TongueType.SNAKE && Utils.Rand(10) < 6)
			{
				// Higher (60%) chance to be 'fixed' if old variant
				TongueData oldData = target.tongue.AsReadOnlyData();
				target.UpdateTongue(TongueType.LIZARD);
				sb.Append(UpdateTongueText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			if (target.tongue.type != TongueType.LIZARD && target.tongue.type != TongueType.SNAKE && target.face.IsReptilian() && ScalingChance(2, currentChanges(), 3))
			{
				TongueData oldData = target.tongue.AsReadOnlyData();
				target.UpdateTongue(TongueType.LIZARD);
				sb.Append(UpdateTongueText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Remove Gills
			if (ScalingChance(2, currentChanges(), 4) && !target.gills.isDefault)
			{
				GillData oldData = target.gills.AsReadOnlyData();
				target.RestoreGills();
				sb.Append(RestoredGillsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//<mod name="Reptile eyes" author="Stadler76">
			//-Lizard eyes
			if (target.eyes.type != EyeType.LIZARD && target.face.type == FaceType.LIZARD && target.body.type == BodyType.REPTILE && target.ears.type == EarType.LIZARD
				&& ScalingChance(2, currentChanges(), 4))
			{
				EyeData oldData = target.eyes.AsReadOnlyData();
				target.UpdateEyes(EyeType.LIZARD);
				sb.Append(UpdateEyesText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//</mod>
			//FAILSAFE CHANGE
			if (remainingChanges == changeCount)
			{
				(target as CombatCreature)?.AddHP(50);
				target.ChangeLust(3);
			}


			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		protected virtual string GrantOvipositionText(Creature target)
{
return GainedOvipositionTextGeneric(target);
}

		protected virtual string UpdateHornsText(Creature target, HornData oldData)
{
return target.horns.TransformFromText(oldData);
}

		protected virtual string UpdateHairText(Creature target, HairData oldData)
{
return target.hair.TransformFromText(oldData);
}

		protected virtual string UpdateLowerBodyText(Creature target, LowerBodyData oldData)
{
return target.lowerBody.TransformFromText(oldData);
}

		protected virtual string UpdateArmsText(Creature target, ArmData oldData)
{
return target.arms.TransformFromText(oldData);
}

		protected virtual string UpdateTailText(Creature target, TailData oldTail)
{
return target.tail.TransformFromText(oldTail);
}
		protected virtual string UpdateEarsText(Creature target, EarData oldData)
{
return target.ears.TransformFromText(oldData);
}
		protected virtual string UpdateFaceText(Creature target, FaceData oldData)
{
return target.face.TransformFromText(oldData);
}

		protected virtual string UpdateTongueText(Creature target, TongueData oldData)
{
return target.tongue.TransformFromText(oldData);
}

		protected virtual string UpdateEyesText(Creature target, EyeData oldData)
{
return target.eyes.TransformFromText(oldData);
}

		protected virtual string RestoredNeckText(Creature target, NeckData oldData)
{
return target.neck.RestoredText(oldData);
}

		protected virtual string RestoredBackText(Creature target, BackData oldData)
{
return target.back.RestoredText(oldData);
}

		protected virtual string RestoredEyesText(Creature target, EyeData oldData)
{
return target.eyes.RestoredText(oldData);
}

		protected virtual string RestoredGillsText(Creature target, GillData oldData)
{
return target.gills.RestoredText(oldData);
}


		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target);
	}
}