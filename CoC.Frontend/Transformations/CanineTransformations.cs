using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using System;
using System.Linq;
using System.Text;

namespace CoC.Frontend.Transformations
{
	[Flags]
	public enum CanineModifiers { STANDARD = 0, LARGE = 1, DOUBLE = 2, BLACK = 4, KNOTTY = 8, BULBY = 16 }
	internal abstract class CanineTransformations : GenericTransformationBase
	{
		protected CanineModifiers modifiers;

		public CanineTransformations(CanineModifiers canineModifiers)
		{
			modifiers = canineModifiers;
		}

		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			int changeLimit = GenerateChanceCount(target, new int[] { 2, 2 });
			int remainingChanges = changeLimit;

			//crit is our modifier for basically all stats. 1 is default, though any non-standard will proc the non-default
			//standard also has a 15% chance of proccing it too.
			int crit = 1;
			if (modifiers > CanineModifiers.STANDARD || Utils.Rand(20) < 3)
			{
				crit = Utils.Rand(20) / 10 + 2;
			}
			bool hasCrit() => crit > 1;

			StringBuilder sb = new StringBuilder();

			sb.Append(TransformationIntroText(hasCrit()));

			//helper function that c# lets us do. 
			bool DoKnotChanges(float delta)
			{
				if (target.hasCock)
				{
					bool changed = false;
					var dogCockCount = target.genitals.CountCocksOfType(CockType.DOG);
					if (dogCockCount > 0)
					{
						Cock smallestKnot = target.cocks.MinItem(x => x.type == CockType.DOG ? (float?)x.knotMultiplier : null);
						if (smallestKnot.knotMultiplier > 2)
						{
							delta /= 10;
						}
						else if (smallestKnot.knotMultiplier > 1.75)
						{
							delta /= 5;
						}
						else if (smallestKnot.knotMultiplier > 1.5)
						{
							delta /= 2;
						}

						float knotMultiplierDelta = smallestKnot.IncreaseKnotMultiplier(delta);
						if (knotMultiplierDelta != 0)
						{
							sb.Append(EnlargedSmallestKnotText(target, target.cocks.IndexOf(smallestKnot), knotMultiplierDelta, dogCockCount > 1));
							changed = true;
						}
					}
					//else
					//{
					//	changed = true;

					//	var oldCockData = target.cocks[0].AsReadOnlyData();

					//	var knotSize = 1.75f;

					//	if (target.cocks[0].hasKnot)
					//	{
					//		knotSize = Math.Max(2.1f, target.cocks[0].knotMultiplier);
					//	}

					//	target.genitals.UpdateCockWithKnot(0, CockType.DOG, knotSize);
					//	if (target.cocks[0].length < 10)
					//	{
					//		target.cocks[0].SetLength(10);
					//	}
					//	sb.Append(ConvertedOneCockToDog(target, 0, oldCockData));
					//}

					target.DeltaCreatureStats(sens: 0.5f, lus: 5 * crit);
					return changed;
				}
				return false;
			}


			//bad end related checks
			if (hasCrit() && target.body.furActive && target.face.type == FaceType.DOG && target.ears.type == EarType.DOG &&
				target.lowerBody.type == LowerBodyType.DOG && target.tail.type == TailType.DOG && target is IExtendedCreature extended)
			{
				//can get bad end.
				if (extended.extendedData.hasDoggoWarning && !extended.extendedData.resistsTFBadEnds)
				{
					//bad end.
					if (Utils.RandBool())
					{
						sb.Append(BadEndText(target));
						isBadEnd = true;
						return ApplyAndReturn(target, sb, 0);
					}
					//get lucky, but warn that they got lucky
					else
					{
						sb.Append(DoggoWarningText(target, true));
					}
				}
				//not warned
				else if (!extended.extendedData.hasDoggoWarning)
				{
					//warn
					extended.extendedData.hasDoggoWarning = true;
					sb.Append(DoggoWarningText(target, false));
				}
			}
			//stat changes
			if (modifiers.HasFlag(CanineModifiers.BLACK))
			{
				target.IncreaseCreatureStats(lus: (byte)(5 + Utils.Rand(5)), lib: (byte)(2 + Utils.Rand(4)), corr: (byte)(2 + Utils.Rand(4)));
			}
			//stat changes (cont.)
			if (target is CombatCreature combat)
			{
				float strengthIncrease = 0;
				float speedIncrease = 0;
				float intelligenceDecrease = 0;
				if (combat.relativeStrength < 50 && Utils.Rand(3) == 0)
				{
					strengthIncrease = combat.IncreaseStrength(crit);
				}
				if (combat.relativeSpeed < 30 && Utils.Rand(3) == 0)
				{
					speedIncrease = combat.IncreaseSpeed(crit);
				}
				if (combat.relativeIntelligence > 30 && Utils.Rand(3) == 0)
				{
					intelligenceDecrease = combat.DecreaseIntelligence(crit);
				}

				sb.Append(StatChangeText(target, strengthIncrease, speedIncrease, intelligenceDecrease));
			}

			//modifier effects (no cost)

			//double pepper
			if (modifiers.HasFlag(CanineModifiers.DOUBLE))
			{
				int dogCocks = target.genitals.CountCocksOfType(CockType.DOG);
				//already has 2+ dog cocks.
				if (dogCocks >= 2)
				{
					//just treat it like a large. so we'll just bitwise or the 
					//large flag in.
					modifiers |= CanineModifiers.LARGE;
				}
				//has no dog cocks.
				else if (dogCocks == 0)
				{
					//has no cocks. - grow 2
					if (target.cocks.Count == 0)
					{
						target.AddCock(CockType.DOG, 7 + Utils.Rand(7), 1.5f + Utils.Rand(10) / 10f, 1.7f);
						target.AddCock(CockType.DOG, 7 + Utils.Rand(7), 1.5f + Utils.Rand(10) / 10f, 1.7f);

						sb.Append(GrewTwoDogCocksHadNone(target));
					}
					//has one cock. - grow one, change one
					else if (target.cocks.Count == 1)
					{
						var oldCockData = target.cocks[0].AsReadOnlyReference();

						target.genitals.UpdateCockWithKnot(0, CockType.DOG, 1.5f);
						if (target.cocks[0].length < 10)
						{
							target.cocks[0].SetLength(10);
						}
						target.AddCock(CockType.DOG, 7 + Utils.Rand(7), 1.5f + Utils.Rand(10) / 10f, 1.7f);

						sb.Append(ConvertedFirstDogCockGrewSecond(target, oldCockData));
					}
					//has 2+ cocks. -change 2
					else
					{
						var firstOldData = target.cocks[0].AsReadOnlyReference();
						var secondOldData = target.cocks[1].AsReadOnlyReference();

						target.genitals.UpdateCockWithKnot(0, CockType.DOG, 1.5f);
						if (target.cocks[0].length < 10)
						{
							target.cocks[0].SetLength(10);
						}
						target.genitals.UpdateCockWithKnot(1, CockType.DOG, 1.5f);
						if (target.cocks[1].length < 10)
						{
							target.cocks[1].SetLength(10);
						}
						sb.Append(ConvertedTwoCocksToDog(target, firstOldData, secondOldData));
					}
				}
				//one dog cock. 
				else
				{
					if (target.cocks.Count == 1)
					{
						target.AddCock(CockType.DOG, 7 + Utils.Rand(7), 1.5f + Utils.Rand(10) / 10f, 1.7f);
						sb.Append(GrewSecondDogCockHadOne(target));
					}
					else
					{
						if (target.cocks[0].type == CockType.DOG)
						{
							var oldData = target.cocks[1].AsReadOnlyReference();
							target.genitals.UpdateCockWithKnot(1, CockType.DOG, 1.5f);
							if (target.cocks[1].length < 10)
							{
								target.cocks[1].SetLength(10);
							}
							sb.Append(ConvertedSecondCockHadOne(target, oldData, false));
						}
						else
						{
							var oldData = target.cocks[0].AsReadOnlyReference();

							target.genitals.UpdateCockWithKnot(0, CockType.DOG, 1.5f);
							if (target.cocks[0].length < 10)
							{
								target.cocks[0].SetLength(10);
							}
							sb.Append(ConvertedSecondCockHadOne(target, oldData, true));

						}
					}
				}

				target.IncreaseCreatureStats(lib: 2, lus: 50);
			}

			//knotty, by default, or large, or with random chance. This has a cost if not knotty.
			if (modifiers.HasFlag(CanineModifiers.KNOTTY))
			{
				float delta = ((Utils.Rand(2) + 5) / 20f) * crit;
				//change the smallest knot. if no knots to grow, but we have a male cock.
				if (!DoKnotChanges(delta) && target.hasCock)
				{
					CockData oldData = target.cocks[0].AsReadOnlyReference();
					target.genitals.UpdateCockWithKnot(0, CockType.DOG, Math.Max(target.cocks[0].knotMultiplier, 2.1f));

					sb.Append(ConvertingOneCockToDog(target, 0, oldData));
				}
				else if (!target.hasCock)
				{
					sb.Append(WastedKnottyText(target));
				}
			}

			//bulby
			if (modifiers.HasFlag(CanineModifiers.BULBY))
			{
				if (!target.hasBalls)
				{
					target.genitals.GrowBalls(2, 1);
					target.DeltaCreatureStats(lib: 2, lus: -10);
					sb.Append(GrewBallsText(target));
				}
				else if (target.balls.uniBall)
				{
					var oldData = target.balls.AsReadOnlyReference();
					target.genitals.ConvertToNormalBalls();
					target.DeltaCreatureStats(lib: 1, lus: 1);

					sb.Append(EnlargedBallsText(target, oldData));
				}
				else
				{
					var oldData = target.balls.AsReadOnlyReference();

					var enlargeAmount = target.genitals.balls.EnlargeBalls(1);
					target.IncreaseCreatureStats(lib: 1, lus: 3);

					sb.Append(EnlargedBallsText(target, oldData));

				}
			}
			//tfs (cost 1).

			//restore neck
			if (target.neck.type != NeckType.defaultValue && Utils.Rand(4) == 0)
			{
				var oldData = target.neck.AsReadOnlyReference();
				target.RestoreNeck();

				sb.Append(RestoredNeckText(target, oldData));
				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
			}

			//remove oviposition
			if (target is Player && Utils.Rand(5) == 0)
			{
				if (((PlayerWomb)target.womb).ClearOviposition())
				{
					sb.Append(RemovedOvipositionText(target));
					if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
				}
			}

			//remove feather-hair
			if (RemoveFeatheryHair(target))
			{
				var oldData = target.hair.AsReadOnlyReference();

				sb.Append(RemovedFeatheryHairText(target, oldData));

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
			}

			//knot multiplier (but not knotty)
			if (!modifiers.HasFlag(CanineModifiers.KNOTTY) && target.genitals.CountCocksOfType(CockType.DOG) > 0 && (modifiers.HasFlag(CanineModifiers.LARGE) || Utils.Rand(7) < 5))
			{
				var delta = ((Utils.Rand(2) + 1) / 20f) * crit;
				if (DoKnotChanges(delta))
				{
					if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
				}
			}

			//transform a cock.
			if (target.genitals.CountCocksOfType(CockType.DOG) < target.cocks.Count && (modifiers.HasFlag(CanineModifiers.LARGE) || Utils.Rand(8) < 5))
			{
				var nonDoggo = target.cocks.FirstOrDefault(x => x.type != CockType.DOG && x.type != CockType.DEMON); //find any cock that isn't a dog, but also isn't a demon cock.
				if (nonDoggo != null)
				{
					var oldData = nonDoggo.AsReadOnlyReference();
					var index = target.cocks.IndexOf(nonDoggo);
					sb.Append(ConvertedOneCockToDog(target, index, oldData));


					if (oldData.type != CockType.HORSE)
					{
						target.genitals.UpdateCockWithAll(index, CockType.DOG, oldData.length, oldData.girth + 2, Math.Max(oldData.knotMultiplier, 2.1f));
					}
					else
					{
						float shrinkBy = oldData.length > 6 ? 2.5f : 0.5f;

						target.genitals.UpdateCockWithAll(index, CockType.DOG, oldData.length - shrinkBy, oldData.girth + 2.5f, 1.1f);
					}
					byte sensInc = 4;
					if (oldData.type == CockType.HUMAN)
					{
						sensInc = 10;
					}

					target.DeltaCreatureStats(sens: sensInc, lus: 5 * crit);

					if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
				}
				else
				{
					var demonSpecialCase = target.cocks.FirstOrDefault(x => x.type == CockType.DEMON);
					var index = demonSpecialCase.index;
					if (demonSpecialCase != null)
					{
						var delta = demonSpecialCase.ThickenCock(2);
						target.DeltaCreatureStats(sens: 1, lus: 2 * crit);

						sb.Append(CouldntConvertDemonCockThickenedInstead(target, index, delta));
						if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
					}
				}
			}

			//update cum
			if (target.hasCockOrClitCock && target.genitals.cumMultiplier < 1.5f && Utils.RandBool())
			{
				float delta = target.genitals.IncreaseCumMultiplier(0.05f * crit);
				sb.Append(AddedCumText(target, delta, true));
			}
			else if (target.genitals.additionalCum < 500)
			{
				float delta = target.genitals.AddFlatCumAmount(50);
				sb.Append(AddedCumText(target, delta, false));
			}

			if (target.hasCock && modifiers.HasFlag(CanineModifiers.LARGE))
			{
				var smallest = target.genitals.SmallestCock(false);
				var oldData = smallest.AsReadOnlyReference();

				smallest.LengthenCock(Utils.Rand(4) + 3);
				if (smallest.girth < 1)
				{
					var delta = 1 - smallest.girth;
					smallest.ThickenCock(delta);
				}
				sb.Append(GrewSmallestCockText(target, smallest.index, oldData));
			}

			//do female changes.

			if (target.hasVagina && target.genitals.SmallestCupSize() > CupSize.FLAT)
			{
				var breastCount = target.breasts.Count;
				if (breastCount < 3)
				{
					BreastData[] oldBreasts = target.breasts.Select(x => x.AsReadOnlyReference()).ToArray();
					if (FemaleMammalGrowBreastRows(target, 1, 3, true))
					{
						int changeCount = 0;

						for (int x = 0; x < oldBreasts.Length; x++)
						{

							if (oldBreasts[x].cupSize != target.breasts[x].cupSize)
							{
								changeCount++;
							}
						}

						bool othersChanged = false;
						for (int x = 0; x < oldBreasts.Length; x++)
						{

							if (oldBreasts[x].cupSize != target.breasts[x].cupSize)
							{
								var delta = target.breasts[x].cupSize - oldBreasts[x].cupSize;
								sb.Append(GrowCurrentBreastRowText(target, x, delta, othersChanged, changeCount-- > 0));
							}

						}

						target.genitals.AddBreastRow(target.breasts[breastCount - 1].cupSize.ByteEnumSubtract(1));
						sb.Append(GrewAdditionalBreastRowText(target, crit));

						target.IncreaseCreatureStats(lus: 5, sens: 6);
					}
				}
				else
				{
					var oldSizes = target.breasts.Select(x => x.cupSize).ToArray();
					target.genitals.NormalizeBreasts();

					for (int x = 0; x < target.breasts.Count; x++)
					{
						bool isFirstAlteredRow = true;
						if (target.breasts[x].cupSize != oldSizes[x])
						{
							sb.Append(NormalizedBreastSizeText(target, x, ((byte)target.breasts[x].cupSize).delta((byte)oldSizes[x]), isFirstAlteredRow));
							isFirstAlteredRow = false;
						}
					}
				}

				if (hasCrit())
				{
					if (crit > 2) target.IncreaseCreatureStats(sens: 6, lus: 15);
					else target.IncreaseCreatureStats(sens: 3, lus: 10);
				}
				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
			}

			//Go into heat
			if (EnterHeat(target, out bool increased))
			{
				sb.Append(EnterOrIncreaseHeatText(target, increased));

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);

			}

			//doggo fantasies
			if (target.DogScore() > 3 && Utils.Rand(4) == 0)
			{

				sb.Append(DoggoFantasyText(target));

				//stacks up to 4 times, based on corruption.
				int multiplier = 1;
				if (target.corruption >= 33)
				{
					multiplier++;
				}
				if (target.corruption > 66)
				{
					multiplier++;
				}
				if (target.corruption >= 80)

					//apply the stacks;
					target.IncreaseLust(multiplier * (5 + (target.libidoTrue / 20f)));

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
			}

			//doggo tfs.

			if (!target.eyes.isDefault && Utils.Rand(5) == 0)
			{
				var oldData = target.eyes.AsReadOnlyReference();

				target.RestoreEyes();

				sb.Append(RestoredEyesText(target, oldData));

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (Utils.Rand(5) == 0 && target.face.type != FaceType.DOG && target.lowerBody.type == LowerBodyType.DOG && target.hasPrimaryFur
				&& target.body.mainEpidermis.fur.IsIdenticalTo(HairFurColors.MIDNIGHT_BLACK) || target.body.mainEpidermis.fur.IsIdenticalTo(HairFurColors.BLACK))
			{
				var oldData = target.face.AsReadOnlyReference();
				sb.Append(ChangeFaceText(target, oldData));
				target.UpdateFace(FaceType.DOG);

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (modifiers.HasFlag(CanineModifiers.BLACK) && !target.body.mainEpidermis.fur.IsIdenticalTo(HairFurColors.BLACK))
			{
				//for now, we're ignoring underbody, apparently. 
				if (target.body.type != BodyType.SIMPLE_FUR)
				//if (target.body.mainEpidermis.currentType != EpidermisType.FUR)
				{
					var oldBodyData = target.body.AsReadOnlyReference();

					target.UpdateBody(BodyType.SIMPLE_FUR, new FurColor(HairFurColors.BLACK), FurTexture.THICK);

					sb.Append(ChangedBodyTypeText(target, oldBodyData));
				}
				else
				{
					target.body.ChangeMainFur(new FurColor(HairFurColors.MIDNIGHT_BLACK), FurTexture.THICK);
					sb.Append(ChangedFurText(target));
				}

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
			}
			//again, we're ignoring underbody for now, idk.
			else if (target.lowerBody.type == LowerBodyType.DOG && target.tail.type == TailType.DOG &&
				//target.body.mainEpidermis.currentType != EpidermisType.FUR 
				target.body.type != BodyType.SIMPLE_FUR
				&& Utils.Rand(4) == 0)
			{
				var oldBodyData = target.body.AsReadOnlyReference();

				FurColor oldFur = target.body.mainEpidermis.fur;

				target.UpdateBody(BodyType.SIMPLE_FUR, Utils.RandomChoice(Species.DOG.availableColors));

				sb.Append(ChangedBodyTypeText(target, oldBodyData));

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (target.lowerBody.type != LowerBodyType.DOG && target.tail.type == TailType.DOG && target.ears.type == EarType.DOG && Utils.Rand(3) == 0)
			{
				var oldData = target.lowerBody.AsReadOnlyReference();

				target.UpdateLowerBody(LowerBodyType.DOG);
				sb.Append(ChangedLowerBodyText(target, oldData));

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (target.ears.type != EarType.DOG && target.tail.type == TailType.DOG && Utils.RandBool())
			{
				var oldData = target.ears.AsReadOnlyReference();

				target.UpdateEar(EarType.DOG);

				sb.Append(ChangedEarsText(target, oldData));
				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (target.tail.type != TailType.DOG && Utils.Rand(3) == 0)
			{
				var oldData = target.tail.AsReadOnlyReference();

				target.UpdateTail(TailType.DOG);

				sb.Append(ChangedTailText(target, oldData));
				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (target.arms.type != ArmType.DOG && target.body.IsFurry() && target.tail.type == TailType.DOG
				&& target.lowerBody.type == LowerBodyType.DOG && Utils.Rand(4) == 0)
			{
				var oldData = target.arms.AsReadOnlyReference();

				target.UpdateArms(ArmType.DOG);

				sb.Append(ChangedArmsText(target, oldData));
				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (!target.gills.isDefault && Utils.Rand(4) == 0)
			{
				var oldGillData = target.gills.AsReadOnlyReference();

				target.RestoreGills();

				sb.Append(RemovedGillsText(target, oldGillData));
				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (target is CombatCreature cc && target.body.furActive && Utils.Rand(3) == 0)
			{

				cc.DeltaCombatCreatureStats(tou: 4, sens: -3);


				sb.Append(FallbackToughenUpText(target));

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (target is CombatCreature cc2 && remainingChanges == changeLimit)
			{
				cc2.AddHP(20);
				sb.Append(NothingHappenedGainHpText(target));
				target.IncreaseLust(3);
			}

			return ApplyAndReturn(target, sb, changeLimit - remainingChanges);

		}

		internal abstract string TransformationIntroText(bool hasCrit);
		protected abstract string BadEndText(Creature target);
		protected abstract string DoggoWarningText(Creature target, bool wasPreviouslyWarned);
		protected abstract string StatChangeText(Creature target, float strengthIncrease, float speedIncrease, float intelligenceDecrease);
		protected abstract string EnlargedSmallestKnotText(Creature target, int cockIndex, float knotMultiplierDelta, bool hasMoreThanOneDogCock);
		protected abstract string GrewTwoDogCocksHadNone(Creature target);
		protected abstract string ConvertedFirstDogCockGrewSecond(Creature target, CockData oldCockData);
		protected abstract string ConvertedTwoCocksToDog(Creature target, CockData firstOldData, CockData secondOldData);
		protected abstract string GrewSecondDogCockHadOne(Creature target);
		protected abstract string ConvertedSecondCockHadOne(Creature target, CockData convertedCockOldData, bool isFirstCock);
		protected abstract string WastedKnottyText(Creature target);
		protected abstract string GrewBallsText(Creature target);
		protected abstract string EnlargedBallsText(Creature target, BallsData oldData);
		protected virtual string RestoringNeckText(Creature target)
		{
			return target.eyes.RestoreText();
		}
		protected virtual string RemovedOvipositionText(Creature target)
		{
			return RemovedOvipositionTextGeneric(target);
		}
		protected virtual string RemovingFeatheryHairText(Creature target)
		{
			return target.hair.RestoreText();
		}
		protected virtual string ConvertingOneCockToDog(Creature target, int index)
		{
			return target.cocks[index].TransformIntoText(CockType.DOG);
		}
		protected abstract string CouldntConvertDemonCockThickenedInstead(Creature target, int index, float delta);
		protected abstract string AddedCumText(Creature target, float delta, bool isMultiplier);
		protected abstract string GrewSmallestCockText(Creature target, int index, CockData oldData);
		protected abstract string GrowCurrentBreastRowText(Creature target, int index, byte increaseAmount, bool previousRowsGrew, bool hasMoreRowsThatGrew);
		protected abstract string GrewAdditionalBreastRowText(Creature target, int critModifier);
		protected abstract string NormalizedBreastSizeText(Creature target, int breastIndex, short changeInCupSize, bool isFirstRowToChange);
		protected abstract string EnterOrIncreaseHeatText(Creature target, bool isIncrease);
		protected abstract string DoggoFantasyText(Creature target);
		protected virtual string RestoredEyesText(Creature target, EyeData oldData)
		{
			return target.eyes.TransformFromText(oldData);
		}

		protected virtual string ChangeFaceText(Creature target, FaceData oldData)
		{
			return target.face.TransformFromText(oldData);
		}

		protected abstract string ChangedFurText(Creature target);
		protected virtual string ChangedBodyTypeText(Creature target, BodyData oldBodyData)
		{
			return target.body.TransformFromText(oldBodyData);
		}
		protected virtual string ChangedLowerBodyText(Creature target, LowerBodyData oldData)
		{
			return target.lowerBody.TransformFromText(oldData);
		}
		protected virtual string ChangedEarsText(Creature target, EarData oldData)
		{
			return target.ears.TransformFromText(oldData);
		}
		protected virtual string ChangedTailText(Creature target, TailData oldData)
		{
			return target.tail.TransformFromText(oldData);
		}
		protected virtual string ChangedArmsText(Creature target, ArmData oldArmData)
		{
			return target.arms.TransformFromText(oldArmData);
		}
		protected virtual string RemovedGillsText(Creature target, GillData oldGillData)
		{
			return target.gills.TransformFromText(oldGillData);
		}

		protected abstract string FallbackToughenUpText(Creature target);

		protected abstract string NothingHappenedGainHpText(Creature target);
	}
}
