﻿//FerretTransformations.cs
//Description:
//Author: JustSomeGuy
//1/18/2020 9:04:46 PM
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Perks;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;

namespace CoC.Frontend.Transformations
{
	internal abstract class FerretTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;

		//Original Credit: (author text):
		//
		//Coalsack (for revisions)
		//it was originally just the author credit, which presumably just means the text, not the code. still here, just in case.
		//
		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//1/12:+3. 1/3:+2, 5/12:+1, 1/6:+0.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 2, 3 });
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

			//fun fact: ferret fruit has a 1/100th chance of doing nothing. but i can't put that here because it makes no sense for non ferret fruit. also,
			//this had an author credit. afaik, no other items do. alas, i still need to support it. fun times.

			sb.Append(InitialTransformationText(target));

			//also fun fact: this is the only code i've seen (so far, this is relatively early on - NOTE TO SELF: remove this if it proves inaccurate) that resets the bad end warning
			//when it drops below a certain threshold, so you don't get a surprise bad end if you've already reached it once and forgotten about it. seems like a good idea, no?

			//BAD END:
			if (target.face.type == FaceType.FERRET && target.ears.type == EarType.FERRET && target.tail.type == TailType.FERRET && target.lowerBody.type == LowerBodyType.FERRET &&
				target.body.IsFurBodyType() && target is IExtendedCreature extended && !extended.extendedData.resistsTFBadEnds)
			{
				//Get warned!
				if (!extended.extendedData.hasFerretWarning)
				{
					target.DecreaseIntelligence(5 + Utils.Rand(3));

					if (target.intelligence < 5)
					{
						target.SetIntelligence(5);
					}

					extended.extendedData.hasFerretWarning = true;
				}
				//BEEN WARNED! BAD END! DUN DUN DUN
				else if (Utils.Rand(3) == 0)
				{
					isBadEnd = true;

					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Reset the warning if ferret score drops.
			else if (target is IExtendedCreature resetFlagCheck)
			{
				resetFlagCheck.extendedData.hasFerretWarning = true;
			}

			//if (remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			if (target.relativeSpeed < 100 && Utils.Rand(3) == 0)
			{
				target.IncreaseSpeed(2 + Utils.Rand(2));
			}

			//this will handle the edge case where the change count starts out as 0.
			if (remainingChanges <= 0)
			{
				return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Ferret Fruit Effects
			//- + Thin:
			if (target.build.thickness > 15 && Utils.Rand(3) == 0)
			{
				target.build.GetThinner(2);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//- If speed is < 100, increase speed:

			//- If male with a hip rating >4 or a female/herm with a hip rating >6:
			if ((target.hips.size > (target.gender.HasFlag(Gender.FEMALE) ? 6 : 4)) && Utils.Rand(3) == 0)
			{
				if (target.hips.size > 15)
				{
					target.build.ShrinkHips(3);
				}
				else if (target.hips.size > 10)
				{
					target.build.ShrinkHips(2);
				}
				else
				{
					target.build.ShrinkHips(1);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//- If butt.size is greater than \"petite\":
			if (target.butt.size > Butt.NOTICEABLE && Utils.Rand(3) == 0)
			{
				if (target.butt.size > 15)
				{
					target.build.ShrinkButt(3);
				}
				else if (target.butt.size > 10)
				{
					target.build.ShrinkButt(2);
				}
				else
				{
					target.build.ShrinkButt(1);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			CupSize biggestCupSize = target.genitals.BiggestCupSize();

			//-If we can shrink the breasts at all (smallest possible cup size takes current gender and any perks into consideration), and the largest cup is greater than a b cup if female/herm.
			if (!hyperHappy && biggestCupSize > target.genitals.smallestPossibleCupSize && (!target.hasVagina || target.genitals.smallestPossibleCupSize > CupSize.B) && Utils.Rand(2) == 0)
			{
				//if we have a vag, check if our min size is below B-Cup. this will only bring us down to B-Cup.
				CupSize targetSize = target.hasVagina && target.genitals.smallestPossibleCupSize < CupSize.B ? CupSize.B : target.genitals.smallestPossibleCupSize;

				foreach (Breasts breastRow in target.breasts)
				{
					if (breastRow.cupSize > targetSize)
					{
						breastRow.ShrinkBreasts(1);
					}
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
				//(this will occur incrementally until they become flat, manly breasts for males, or until they are A or B cups for females/herms)
			}

			//Remove additional cocks
			if (target.cocks.Count > 1 && Utils.Rand(3) == 0)
			{
				target.genitals.RemoveCock();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Remove additional balls/remove uniball
			if (target.balls.count > 0 && Utils.Rand(3) == 0)
			{
				bool changedBalls = false;
				if (target.balls.size > 2)
				{
					if (target.balls.size > 5)
					{
						changedBalls |= target.balls.ShrinkBalls((byte)(1 + Utils.Rand(3))) > 0;
					}
					else
					{
						changedBalls |= target.balls.ShrinkBalls(1) > 0;
					}
				}

				if (target.balls.count != 2)
				{
					changedBalls |= target.balls.MakeStandard();
				}

				if (changedBalls)
				{
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}

			Cock longestCock = target.genitals.LongestCock();

			//-If penis size is > 6 inches:
			if (target.hasCock && longestCock.length > 6 && !hyperHappy)
			{
				bool shortenedCock = false;
				if (longestCock.length >= 10)
				{
					shortenedCock |= longestCock.DecreaseLength(Utils.Rand(4) + 2) > 0;
				}
				else
				{
					shortenedCock |= longestCock.DecreaseLength(Utils.RandBool() ? 2 : 1) > 0;
				}

				if (longestCock.girth > longestCock.length / 6.0)
				{
					shortenedCock |= longestCock.SetGirth(longestCock.length / 6.0) == longestCock.length / 6.0;
				}

				if (shortenedCock)
				{
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}

			//converts first cock to ferret, no others.
			if (target.hasCock && target.cocks[0].type != CockType.FERRET && Utils.Rand(3) == 0)
			{
				target.genitals.UpdateCock(0, CockType.FERRET);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//-If the PC has quad nipples:
			if (target.genitals.hasQuadNipples && Utils.Rand(4) == 0)
			{
				target.genitals.SetQuadNipples(false);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//If the PC has gills:
			if (!target.gills.isDefault && Utils.Rand(4) == 0)
			{
				GillData oldData = target.gills.AsReadOnlyData();
				target.RestoreGills();
				sb.Append(RestoredGillsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Hair
			bool setFerretHairColor = !Species.FERRET.availableHairColors.Contains(target.hair.hairColor);
			if ((target.hair.type != HairType.NORMAL || setFerretHairColor || target.hair.length <= 0) && Utils.Rand(4) == 0)
			{
				HairData oldHairData = target.hair.AsReadOnlyData();

				double length = target.hair.length == 0 ? 1 : target.hair.length;
				HairFurColors color = setFerretHairColor ? Utils.RandomChoice(Species.FERRET.availableHairColors) : target.hair.hairColor;

				if (target.hair.type != HairType.NORMAL)
				{
					target.UpdateHair(HairType.NORMAL, true, color, newHairLength: length);
				}
				else
				{
					target.hair.SetAll(length, true, color);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//If the PC has four eyes or one eye (if ever implemented):
			if (target.eyes.count != 2 && Utils.Rand(3) == 0)
			{

				EyeData oldData = target.eyes.AsReadOnlyData();
				target.RestoreEyes();
				sb.Append(RestoredEyesText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Go into heat
			if (target.hasVagina && Utils.Rand(3) == 0)
			{
				if (target.GoIntoHeat())
				{
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
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
				target.womb.ClearOviposition();
				sb.Append(ClearOvipositionText(target));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//face TFs. they all now use the same roll.

			//Turn ferret mask to full furface.

			if (Utils.Rand(3) == 0)
			{
				if (target.face.type == FaceType.HUMAN)
				{
					FaceData oldData = target.face.AsReadOnlyData();
					target.UpdateFace(FaceType.FERRET);
					sb.Append(UpdateFaceText(target, oldData));

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
				else if (target.face.type != FaceType.HUMAN && target.face.type != FaceType.FERRET)
				{
					FaceData oldData = target.face.AsReadOnlyData();
					target.RestoreFace();
					sb.Append(RestoredFaceText(target, oldData));

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
				else if (target.face.type == FaceType.FERRET && !target.face.isFullMorph && target.body.isFurry && target.ears.type == EarType.FERRET
					&& target.tail.type == TailType.FERRET && target.lowerBody.type == LowerBodyType.FERRET && Utils.Rand(4) < 3)
				{
					if (target.face.StrengthenFacialMorph())
					{
						if (--remainingChanges <= 0)
						{
							return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
						}
					}
				}

			}
			//ferret uses the full underbody fur.
			if (target.body.type != BodyType.UNDERBODY_FUR)
			{
				//if they have full fur, but it doesn't allow an underbody, or the weird kitsune fur/skin combo, silently update it to have an underbody.
				if (target.body.type == BodyType.SIMPLE_FUR || target.body.type == BodyType.KITSUNE)
				{
					Species.FERRET.GetFurColorsFrom(target.body.mainEpidermis.fur, out FurColor primary, out FurColor secondary);
					BodyData oldData = target.body.AsReadOnlyData();
					target.UpdateBody(BodyType.UNDERBODY_FUR, primary, secondary);
					sb.Append(UpdateBodyText(target, oldData));

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
				//otherwise, require some other parts, and rng.
				else if (target.ears.type == EarType.FERRET && target.tail.type == TailType.FERRET && target.lowerBody.type == LowerBodyType.FERRET && Utils.Rand(4) == 0)
				{
					Species.FERRET.GetRandomFurColor(out FurColor primary, out FurColor secondary);
					BodyData oldData = target.body.AsReadOnlyData();
					target.UpdateBody(BodyType.UNDERBODY_FUR, primary, secondary);
					sb.Append(UpdateBodyText(target, oldData));

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}
			//rubber skin is now a perk. try to remove a stack. if this removes the perk, that's cool too.
			if (target.HasPerk<RubberySkin>())
			{
				if (target.GetPerkData<RubberySkin>().AttemptStackDecrease())
				{
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}

			//Tail TFs!
			if (target.tail.type != TailType.FERRET && target.ears.type == EarType.FERRET && Utils.Rand(3) == 0)
			{
				TailData oldData = target.tail.AsReadOnlyData();
				target.UpdateTail(TailType.FERRET);
				sb.Append(UpdateTailText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//If legs are not ferret, has ferret ears and tail
			if (target.lowerBody.type != LowerBodyType.FERRET && target.ears.type == EarType.FERRET && target.tail.type == TailType.FERRET && Utils.Rand(4) == 0)
			{
				LowerBodyData oldData = target.lowerBody.AsReadOnlyData();
				target.UpdateLowerBody(LowerBodyType.FERRET);
				sb.Append(UpdateLowerBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Arms
			if (target.arms.type != ArmType.FERRET && target.tail.type == TailType.FERRET && target.lowerBody.type == LowerBodyType.FERRET && Utils.Rand(4) == 0)
			{
				ArmData oldData = target.arms.AsReadOnlyData();
				target.UpdateArms(ArmType.FERRET);
				sb.Append(UpdateArmsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//If ears are not ferret:
			if (target.ears.type != EarType.FERRET && Utils.Rand(4) == 0)
			{
				EarData oldData = target.ears.AsReadOnlyData();
				target.UpdateEars(EarType.FERRET);
				sb.Append(UpdateEarsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Remove antennae, if insectile
			if (target.antennae.type == AntennaeType.BEE && Utils.Rand(4) == 0)
			{
				AntennaeData oldData = target.antennae.AsReadOnlyData();
				target.RestoreAntennae();
				sb.Append(RestoredAntennaeText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//If no other effect occurred, fatigue decreases:
			if (changeCount == remainingChanges && target is CombatCreature fatigueCheck)
			{
				fatigueCheck.RecoverFatigue(20);
			}

			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		protected virtual string ClearOvipositionText(Creature target)
{
return RemovedOvipositionTextGeneric(target);
}

		protected virtual string UpdateFaceText(Creature target, FaceData oldData)
		{
			return target.face.TransformFromText(oldData);
		}

		protected virtual string UpdateBodyText(Creature target, BodyData oldData)
		{
			return target.body.TransformFromText(oldData);
		}

		protected virtual string UpdateTailText(Creature target, TailData oldTail)
		{
			return target.tail.TransformFromText(oldTail);
		}
		protected virtual string UpdateLowerBodyText(Creature target, LowerBodyData oldData)
		{
			return target.lowerBody.TransformFromText(oldData);
		}

		protected virtual string UpdateArmsText(Creature target, ArmData oldData)
		{
			return target.arms.TransformFromText(oldData);
		}

		protected virtual string UpdateEarsText(Creature target, EarData oldData)
		{
			return target.ears.TransformFromText(oldData);
		}
		protected virtual string RestoredGillsText(Creature target, GillData oldData)
		{
			return target.gills.RestoredText(oldData);
		}

		protected virtual string RestoredNeckText(Creature target, NeckData oldData)
		{
			return target.neck.RestoredText(oldData);
		}

		protected virtual string RestoredEyesText(Creature target, EyeData oldData)
		{
			return target.eyes.RestoredText(oldData);
		}

		protected virtual string RestoredBackText(Creature target, BackData oldData)
		{
			return target.back.RestoredText(oldData);
		}

		protected virtual string RestoredFaceText(Creature target, FaceData oldData)
		{
			return target.face.RestoredText(oldData);
		}

		protected virtual string RestoredAntennaeText(Creature target, AntennaeData oldData)
		{
			return target.antennae.RestoredText(oldData);
		}


		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target);
	}
}