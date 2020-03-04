//FoxTransformations.cs
//Description:
//Author: JustSomeGuy
//1/19/2020 2:58:47 AM
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Perks.Endowment;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;
using CoC.Frontend.StatusEffect;

namespace CoC.Frontend.Transformations
{
	//not to be confused with Kitsune transformations. they are seperate.

	internal abstract class FoxTransformations : GenericTransformationBase
	{
		protected readonly bool enhanced;

		protected FoxTransformations(bool potent)
		{
			enhanced = potent;
		}

		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;

		//Original credits:
		//since March 26, 2018
		//@author Stadler76
		//
		//Porter's note: Nearly all of the comments here are from the vanilla. comments from modification of code will be marked with <MODIFICATION>
		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;
			//<MODIFICATION NOTE>
			//potent tf has an initial count of 3.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 2 }, enhanced ? 3 : 1);
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

			sb.Append(InitialTransformationText(target));

			if (target.face.type == FaceType.FOX && target.tail.type == TailType.FOX && target.ears.type == EarType.FOX && target.lowerBody.type == LowerBodyType.FOX
				&& target.body.IsFurBodyType() && Utils.Rand(3) == 0 && target is IExtendedCreature extended && !extended.extendedData.resistsTFBadEnds)
			{
				if (!extended.extendedData.hasFoxWarning)
				{
					extended.extendedData.hasFoxWarning = true;
				}
				else
				{
					isBadEnd = true;
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//<MODIFICATION NOTE>
			//Free Changes.

			//[decrease Strength] (to some floor) // I figured 15 was fair, but you're in a better position to judge that than I am.
			if (Utils.Rand(3) == 0 && target.relativeStrength > 40)
			{
				if (target.relativeStrength > 90)
				{
					target.DecreaseStrength(4);
				}
				else if (target.relativeStrength > 80)
				{
					target.DecreaseStrength(3);
				}
				else if (target.relativeStrength > 60)
				{
					target.DecreaseStrength(2);
				}
				else
				{
					target.DecreaseStrength();
				}
			}
			//[decrease Toughness] (to some floor) // 20 or so was my thought here
			if (Utils.Rand(3) == 0 && target.relativeToughness > 30)
			{
				if (target.relativeToughness > 90)
				{
					target.DecreaseToughness(4);
				}
				else if (target.relativeToughness > 80)
				{
					target.DecreaseToughness(3);
				}
				else if (target.relativeToughness > 60)
				{
					target.DecreaseToughness(2);
				}
				else
				{
					target.DecreaseToughness();
				}
			}
			//[increase Intelligence, Libido and Sensitivity]
			if (Utils.Rand(3) == 0 && (target.relativeLibido < 80 || target.relativeSensitivity < 80 || target.relativeIntelligence < 80))
			{
				if (target.relativeIntelligence < 80)
				{
					target.IncreaseIntelligence(4);
				}

				if (target.relativeLibido < 80)
				{
					target.IncreaseLibido(1);
				}

				if (target.relativeSensitivity < 80)
				{
					target.IncreaseSensitivity(1);
				}
				//gain small lust also
				target.IncreaseLust(10);
			}
			//Modification Note: move this free change up here, where it makes the most sense.
			if (target.build.muscleTone > 40 && Utils.Rand(2) == 0)
			{
				target.build.DecreaseMuscleTone(4);
			}


			if (!Species.FOX.AvailableHairColors.Contains(target.hair.hairColor) && !Species.KITSUNE.elderKitsuneHairColors.Contains(target.hair.hairColor)
				&& !Species.KITSUNE.kitsuneHairColors.Contains(target.hair.hairColor) && Utils.Rand(4) == 0)
			{
				HairFurColors targetColor;
				if (target.tail.type == TailType.FOX && target.tail.tailCount > 1)
				{
					if (target.tail.tailCount < 9)
					{
						targetColor = Utils.RandomChoice(Species.KITSUNE.kitsuneHairColors);
					}

					targetColor = Utils.RandomChoice(Species.KITSUNE.elderKitsuneHairColors);
				}
				else
				{
					targetColor = Utils.RandomChoice(Species.FOX.AvailableHairColors);
				}
			}

			//this will handle the edge case where the change count starts out as 0.
			if (remainingChanges <= 0)
			{
				return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//<MODIFICATION NOTE>
			//Transformation Changes.
			//if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			//[Adjust hips toward 10 – wide/curvy/flared]
			if (Utils.Rand(3) == 0 && target.hips.size != 10)
			{
				//from narrow to wide
				if (target.hips.size < 7)
				{
					target.hips.GrowHips(2);
				}
				else if (target.hips.size < 10)
				{
					target.hips.GrowHips();
				}
				//from wide to narrower
				else if (target.hips.size > 13)
				{
					target.hips.ShrinkHips(2);
				}
				else
				{
					target.hips.ShrinkHips();
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//[Remove tentacle hair]
			//required if the hair length change below is triggered
			if (target.hair.type == HairType.ANEMONE && Utils.Rand(3) == 0)
			{
				//-insert anemone hair removal into them under whatever criteria you like, though hair removal should precede abdomen growth;
				HairData oldData = target.hair.AsReadOnlyData();
				target.RestoreHair();
				sb.Append(RestoredHairText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//[Adjust hair length toward range of 16-26 – very long to ass-length]
			if (target.hair.type == HairType.ANEMONE && (target.hair.length > 26 || target.hair.length < 16) && target.hair.canGrowNaturally && Utils.Rand(4) == 0)
			{
				if (target.hair.length < 16)
				{
					target.hair.GrowHair(1 + Utils.Rand(4));

				}
				else
				{
					target.hair.ShortenHair(1 + Utils.Rand(4));
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(10) == 0)
			{

				//MOD NOTE: Don't remove ?. operator.
				if ((target as CombatCreature)?.RecoverFatigue(10) > 0)
				{
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}

			//dog cocks!

			//MOD NOTE: consider adding back in fox cocks and using them? they could literally just be dog cocks but with a slightly different text descriptions.
			//MOD NOTE: this is a linq check that sees if any available cocks aren't a dog cock. it's actually faster than count of type, because it stops as soon as it hits
			//a cock that isn't a dog, instead of checking all of them. Also, this function finds all non-dog cocks and converts one at random.
			if (Utils.Rand(3) == 0 && target.hasCock && target.cocks.Any(x => x.type != CockType.DOG))
			{
				Cock toChange = Utils.RandomChoice(target.cocks.Where(x => x.type != CockType.DOG).ToArray());

				if (toChange.type == CockType.HUMAN)
				{

					toChange.IncreaseThickness(.3);
					target.DeltaCreatureStats(sens: 10, lus: 5);
				}
				//Horse
				else if (toChange.type == CockType.HORSE)
				{

					//Tweak length/thickness.

					double deltaLength;
					if (toChange.length > 6)
					{
						deltaLength = -2;
					}
					else
					{
						deltaLength = -.5;
					}

					toChange.SetLengthAndGirth(toChange.length + deltaLength, toChange.girth + 0.5);
					target.DeltaCreatureStats(sens: 4, lus: 5);
				}
				else
				{

					target.DeltaCreatureStats(sens: 4, lus: 10);
				}

				target.genitals.UpdateCock(toChange, CockType.DOG);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Cum Multiplier Xform
			if (target.genitals.totalCum < 5000 && Utils.Rand(3) == 0 && target.hasCock)
			{
				int temp = 2 + Utils.Rand(4);
				//Lots of cum raises cum multiplier cap to 2 instead of 1.5
				if (target.HasPerk<MessyOrgasms>())
				{
					temp += Utils.Rand(10);
				}
				//MOD NOTE: not sure if cum calculations changed, (i think they have) and if so, that's a lot of multiplier gain holy shit. meh. whatever.
				target.genitals.IncreaseCumMultiplier(temp);
				//Flavor text
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (target.balls.count > 0 && target.balls.size > 4 && Utils.Rand(3) == 0)
			{
				int targetSize;
				//currently above max, but whatever, that'll probably get raised anyway.
				if (target.balls.size > 50)
				{
					targetSize = target.balls.size / 5;
				}
				else if (target.balls.size > 10)
				{
					targetSize = target.balls.size / 2;
				}
				else
				{
					targetSize = target.balls.size - 1;
				}

				//allow perks to work.
				target.balls.ShrinkBalls((byte)(target.balls.size - targetSize));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Sprouting more!
			if (enhanced && target.breasts.Count < 4 && target.breasts[target.breasts.Count - 1].cupSize > CupSize.A)
			{
				target.genitals.AddBreastRow(target.breasts[target.breasts.Count - 1].cupSize);

				target.DeltaCreatureStats(sens: 2, lus: 30);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Find out if tits are eligible for evening
			//MOD NOTE: there's an easier way - just run the normalize breasts. it'll return true if it changed anything. since we display the results of the old
			//breast data anyway, this is way simpler. Also, normalize breasts now returns a bool. woo!
			//MOD NOTE 2: Fox rules here seem to use the same size as previous. if they want to function under anthro rules (size is in decreasing order, but otherwise
			//roughly even) you can use AnthropomorphizeBreasts() instead. see canine tfs for an example.
			if (target.genitals.NormalizeBreasts())
			{
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}



			//HEAT!
			if (!target.HasTimedEffect<Heat>() || target.GetTimedEffectData<Heat>().totalAddedLibido < 30 && Utils.Rand(6) == 0)
			{
				target.GoIntoHeat();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Neck restore
			if (!target.neck.isDefault && Utils.Rand(4) == 0)
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
			if (!target.back.isDefault && Utils.Rand(5) == 0)
			{
				BackData oldData = target.back.AsReadOnlyData();
				target.RestoreBack();
				sb.Append(RestoredBackText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Ovi perk loss
			if (target.womb.canRemoveOviposition && Utils.Rand(5) == 0)
			{
				if (target.womb.ClearOviposition())
				{
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}
			//[Grow Fur]
			//FOURTH
			//MOD NOTE: Body has been reworked since this was written, we'll allow kitsune's to remain unchanged, but all others need to be full fur after this.
			if ((enhanced || target.lowerBody.type == LowerBodyType.FOX) && target.body.type != BodyType.KITSUNE && !target.body.IsFurBodyType() && Utils.Rand(4) == 0)
			{
				BodyData oldBodyData = target.body.AsReadOnlyData();

				if (Species.KITSUNE.Score(target) >= 4)
				{
					FurColor[] colorChoices;
					if (Species.KITSUNE.elderKitsuneFurColors.Any(x => x.IsIdenticalTo(target.hair.hairColor))
						|| Species.KITSUNE.allKitsuneColors.Any(y => y.IsIdenticalTo(target.hair.hairColor)))
					{
						colorChoices = new FurColor[] { new FurColor(target.hair.hairColor) };
					}
					else if (target.tail.type == TailType.FOX && target.tail.tailCount == TailType.FOX.maxTailCount)
					{
						colorChoices = Species.KITSUNE.allKitsuneColors;
					}
					else
					{
						colorChoices = Species.KITSUNE.elderKitsuneFurColors;
					}

					target.UpdateBody(BodyType.UNDERBODY_FUR, Utils.RandomChoice(colorChoices));

				}
				else
				{
					Species.FOX.GetRandomFurColors(out FurColor primary, out FurColor underbody);

					target.UpdateBody(BodyType.UNDERBODY_FUR, primary, underbody);
				}

				//should always be true, but whatever.
				if (oldBodyData.type != target.body.type)
				{
					remainingChanges--;
					if (remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}
			//[Grow Fox Legs]
			//THIRD
			if ((enhanced || target.ears.type == EarType.FOX) && target.lowerBody.type != LowerBodyType.FOX && Utils.Rand(5) == 0)
			{
				LowerBodyData oldData = target.lowerBody.AsReadOnlyData();
				target.UpdateLowerBody(LowerBodyType.FOX);
				sb.Append(UpdateLowerBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Grow Fox Ears]
			//SECOND
			if ((enhanced || target.tail.type == TailType.FOX) && target.ears.type != EarType.FOX && Utils.Rand(4) == 0)
			{
				EarData oldData = target.ears.AsReadOnlyData();
				target.UpdateEars(EarType.FOX);
				sb.Append(UpdateEarsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//[Grow Fox Tail](fairly common)
			//FIRST
			if (target.tail.type != TailType.FOX && Utils.Rand(4) == 0)
			{
				TailData oldData = target.tail.AsReadOnlyData();
				target.UpdateTail(TailType.FOX);
				sb.Append(UpdateTailText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//[Grow Fox Face]
			//LAST - muzzlygoodness
			//should work from any face, including other muzzles
			if (target.body.HasAny(EpidermisType.FUR) && target.face.type != FaceType.FOX && Utils.Rand(5) == 0)
			{
				FaceData oldData = target.face.AsReadOnlyData();
				target.UpdateFace(FaceType.FOX);
				sb.Append(UpdateFaceText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Arms
			if (target.arms.type != ArmType.FOX && target.body.HasAny(EpidermisType.FUR) && target.tail.type == TailType.FOX && target.lowerBody.type == LowerBodyType.FOX && Utils.Rand(4) == 0)
			{

				ArmData oldData = target.arms.AsReadOnlyData();
				target.UpdateArms(ArmType.FOX);
				sb.Append(UpdateArmsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}


			if (remainingChanges == changeCount && !(target is null))
			{

				(target as CombatCreature)?.RecoverFatigue(5);
			}

			//<MODIFICATION NOTE>
			//Fall through, cleanup and return.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		protected virtual string UpdateLowerBodyText(Creature target, LowerBodyData oldData)
{
return target.lowerBody.TransformFromText(oldData);
}

		protected virtual string UpdateEarsText(Creature target, EarData oldData)
{
return target.ears.TransformFromText(oldData);
}
		protected virtual string UpdateTailText(Creature target, TailData oldTail)
{
return target.tail.TransformFromText(oldTail);
}
		protected virtual string UpdateFaceText(Creature target, FaceData oldData)
{
return target.face.TransformFromText(oldData);
}

		protected virtual string UpdateArmsText(Creature target, ArmData oldData)
{
return target.arms.TransformFromText(oldData);
}

		protected virtual string RestoredHairText(Creature target, HairData oldData)
{
return target.hair.RestoredText(oldData);
}

		protected virtual string RestoredNeckText(Creature target, NeckData oldData)
{
return target.neck.RestoredText(oldData);
}

		protected virtual string RestoredBackText(Creature target, BackData oldData)
{
return target.back.RestoredText(oldData);
}


		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target);
	}
}