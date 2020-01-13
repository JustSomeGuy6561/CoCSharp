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
		private CanineModifiers modifiers;

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
					else
					{
						changed = true;

						var oldCockData = target.cocks[0].AsReadOnlyData();

						var knotSize = 1.75f;

						if (target.cocks[0].hasKnot)
						{
							knotSize = Math.Max(2.1f, target.cocks[0].knotMultiplier);
						}

						target.genitals.UpdateCockWithKnot(0, CockType.DOG, knotSize);
						if (target.cocks[0].length < 10)
						{
							target.cocks[0].SetLength(10);
						}
						sb.Append(ConvertedOneCockToDog(target, 0, oldCockData));
					}

					target.DeltaCreatureStats(sens: 0.5f, lus: 5 * crit);
					return changed;
				}
				return false;
			}


			sb.Append(InitialTransformationText(modifiers, hasCrit()));

			//bad end related checks
			if (hasCrit() && target.body.hasActiveFurData && target.face.type == FaceType.DOG && target.ears.type == EarType.DOG &&
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
						return ApplyChangesAndReturn(target, sb, 0);
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
						var oldCockData = target.cocks[0].AsReadOnlyData();

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
						var firstOldData = target.cocks[0].AsReadOnlyData();
						var secondOldData = target.cocks[1].AsReadOnlyData();

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
							var oldData = target.cocks[1].AsReadOnlyData();
							target.genitals.UpdateCockWithKnot(1, CockType.DOG, 1.5f);
							if (target.cocks[1].length < 10)
							{
								target.cocks[1].SetLength(10);
							}
							sb.Append(ConvertedOneCockToDog(target, 1, oldData));
						}
						else
						{
							var oldData = target.cocks[0].AsReadOnlyData();

							target.genitals.UpdateCockWithKnot(0, CockType.DOG, 1.5f);
							if (target.cocks[0].length < 10)
							{
								target.cocks[0].SetLength(10);
							}
							sb.Append(ConvertedOneCockToDog(target, 0, oldData));

						}
					}
				}

				target.IncreaseCreatureStats(lib: 2, lus: 50);
			}

			//knotty, by default, or large, or with random chance. This has a cost if not knotty.
			if (modifiers.HasFlag(CanineModifiers.KNOTTY))
			{
				float delta = ((Utils.Rand(2) + 5) / 20f) * crit;
				DoKnotChanges(delta);
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
					var oldData = target.balls.AsReadOnlyData();
					target.genitals.ConvertToNormalBalls();
					target.DeltaCreatureStats(lib: 1, lus: 1);

					sb.Append(EnlargedBallsText(target, oldData));
				}
				else
				{
					var oldData = target.balls.AsReadOnlyData();

					var enlargeAmount = target.genitals.balls.EnlargeBalls(1);
					target.IncreaseCreatureStats(lib: 1, lus: 3);

					sb.Append(EnlargedBallsText(target, oldData));

				}
			}
			//tfs (cost 1).

			//restore neck
			if (target.neck.type != NeckType.defaultValue && Utils.Rand(4) == 0)
			{
				var oldNeck = target.neck.AsReadOnlyData();
				target.RestoreNeck();

				sb.Append(RestoredNeckText(target, oldNeck));
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
			}

			//remove oviposition
			if (target is Player && Utils.Rand(5) == 0)
			{
				if (((PlayerWomb)target.womb).ClearOviposition())
				{
					sb.Append(RemovedOvipositionText(target));
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
				}
			}

			//remove feather-hair
			if (RemoveFeatheryHair(target))
			{
				sb.Append(RemovedFeatheryHairText(target));
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
			}

			//knot multiplier (but not knotty)
			if (!modifiers.HasFlag(CanineModifiers.KNOTTY) && target.genitals.CountCocksOfType(CockType.DOG) > 0 && (modifiers.HasFlag(CanineModifiers.LARGE) || Utils.Rand(7) < 5))
			{
				var delta = ((Utils.Rand(2) + 1) / 20f) * crit;
				if (DoKnotChanges(delta))
				{
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
				}
			}

			//transform a cock.
			if (target.genitals.CountCocksOfType(CockType.DOG) < target.cocks.Count && (modifiers.HasFlag(CanineModifiers.LARGE) || Utils.Rand(8) < 5))
			{
				var nonDoggo = target.cocks.FirstOrDefault(x => x.type != CockType.DOG && x.type != CockType.DEMON); //find any cock that isn't a dog, but also isn't a demon cock.
				if (nonDoggo != null)
				{
					var oldData = nonDoggo.AsReadOnlyData();
					var index = target.cocks.IndexOf(nonDoggo);
					sb.Append(ConvertedOneCockToDog(target, index, oldData));

					target.genitals.UpdateCock(index, CockType.DOG);

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
				}
				else
				{
					var demonSpecialCase = target.cocks.FirstOrDefault(x => x.type == CockType.DEMON);
					var index = demonSpecialCase.cockIndex;
					if (demonSpecialCase != null)
					{
						float delta = demonSpecialCase.ThickenCock(2);

						if (delta != 0)
						{
							sb.Append(CouldntConvertDemonCockThickenedInstead(target, index, delta));
							if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
						}
					}
				}
			}

			//update cum
			if (target.hasCock && (target.genitals.cumMultiplier < 1.5f || target.genitals.additionalCum < 500) && Utils.RandBool())
			{
				float delta;
				bool wasMultiplier;
				if (target.genitals.cumMultiplier < 1.5f)
				{
					delta = target.genitals.IncreaseCumMultiplier(0.05f * crit);
					wasMultiplier = true;
				}
				else
				{
					delta = target.genitals.AddFlatCumAmount(50);
					wasMultiplier = false;
				}
				sb.Append(AddedCumText(target, delta, wasMultiplier));
			}

			if (target.hasCock && modifiers.HasFlag(CanineModifiers.LARGE))
			{
				var smallest = target.genitals.SmallestCock();
				var oldData = smallest.AsReadOnlyData();

				smallest.LengthenCock(Utils.Rand(4) + 3);
				if (smallest.girth < 1)
				{
					var delta = 1 - smallest.girth;
					smallest.ThickenCock(delta);
				}
				sb.Append(GrewSmallestCockText(target, smallest.cockIndex, oldData));
			}

			//do female changes.

			if (target.hasVagina && target.genitals.SmallestCupSize() > CupSize.FLAT)
			{
				var breastCount = target.breasts.Count;
				if (breastCount < 3)
				{
					foreach (var breast in target.breasts)
					{
						if (breast.cupSize < (CupSize)breastCount)
						{
							var delta = breast.GrowBreasts((CupSize)breastCount - breast.cupSize);
							if (delta != 0)
							{
								sb.Append(GrowCurrentBreastRowText(target, breast.index, delta));
							}
						}
					}

					target.genitals.AddBreastRow(target.breasts[breastCount - 1].cupSize.ByteEnumSubtract(1));
					sb.Append(GrewAdditionalBreastRowText(target));

					target.IncreaseCreatureStats(lus: 5, sens: 6);
				}
				else
				{
					var oldSizes = target.breasts.Select(x => x.cupSize).ToArray();
					target.genitals.NormalizeBreasts();

					for (int x = 0; x < target.breasts.Count; x++)
					{
						if (target.breasts[x].cupSize != oldSizes[x])
						{
							sb.Append(NormalizedBreastSizeText(target, x, ((byte)target.breasts[x].cupSize).delta((byte)oldSizes[x])));
						}
					}
				}

				if (hasCrit())
				{
					if (crit > 2) target.IncreaseCreatureStats(sens: 6, lus: 15);
					else target.IncreaseCreatureStats(sens: 3, lus: 10);
				}
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
			}

			//Go into heat
			if (EnterHeat(target, out bool increased))
			{
				sb.Append(EnterOrIncreaseHeatText(target, increased));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);

			}

			//doggo fantasies
			if (target.DogScore() > 3 && Utils.Rand(4) == 0)
			{

				sb.Append(DoggoFantasyText(target));
				target.IncreaseLust(5 + (target.libidoTrue / 20f));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
			}

			//doggo tfs.

			if (!target.eyes.isDefault && Utils.Rand(5) == 0)
			{
				var oldData = target.eyes.AsReadOnlyData();

				target.RestoreEyes();

				sb.Append(RestoredEyesText(target, oldData));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (modifiers.HasFlag(CanineModifiers.BLACK) && !target.body.mainEpidermis.fur.IsIdenticalTo(HairFurColors.BLACK))
			{
				//for now, we're ignoring underbody, apparently.
				if (target.body.type != BodyType.SIMPLE_FUR)
				//if (target.body.mainEpidermis.currentType != EpidermisType.FUR)
				{
					var oldBodyData = target.body.AsReadOnlyData();

					target.UpdateBody(BodyType.SIMPLE_FUR, new FurColor(HairFurColors.BLACK), FurTexture.THICK);

					sb.Append(ChangedBodyTypeText(target, oldBodyData));
				}
				else
				{
					var oldFur = target.body.mainEpidermis.fur.AsReadOnly();
					target.body.ChangeMainFur(new FurColor(HairFurColors.MIDNIGHT_BLACK), FurTexture.THICK);
					sb.Append(ChangedFurText(target, oldFur));
				}

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
			}
			//again, we're ignoring underbody for now, idk.
			else if (target.lowerBody.type == LowerBodyType.DOG && target.tail.type == TailType.DOG &&
				//target.body.mainEpidermis.currentType != EpidermisType.FUR
				target.body.type != BodyType.SIMPLE_FUR
				&& Utils.Rand(4) == 0)
			{
				var oldBodyData = target.body.AsReadOnlyData();

				FurColor oldFur = target.body.mainEpidermis.fur;

				target.UpdateBody(BodyType.SIMPLE_FUR, Utils.RandomChoice(Species.DOG.availableColors));

				sb.Append(ChangedBodyTypeText(target, oldBodyData));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (target.lowerBody.type != LowerBodyType.DOG && target.tail.type == TailType.DOG && target.ears.type == EarType.DOG && Utils.Rand(3) == 0)
			{
				var oldData = target.lowerBody.AsReadOnlyData();

				target.UpdateLowerBody(LowerBodyType.DOG);
				sb.Append(ChangedLowerBodyText(target, oldData));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (target.ears.type != EarType.DOG && target.tail.type == TailType.DOG && Utils.RandBool())
			{
				var oldData = target.ears.AsReadOnlyData();

				target.UpdateEar(EarType.DOG);

				sb.Append(ChangedEarsText(target, oldData));
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (target.tail.type != TailType.DOG && Utils.Rand(3) == 0)
			{
				var oldData = target.tail.AsReadOnlyData();

				target.UpdateTail(TailType.DOG);

				sb.Append(ChangedTailText(target, oldData));
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (target.arms.type != ArmType.DOG && target.body.isFurry && target.tail.type == TailType.DOG
				&& target.lowerBody.type == LowerBodyType.DOG && Utils.Rand(4) == 0)
			{
				var oldArmData = target.arms.AsReadOnlyData();

				target.UpdateArms(ArmType.DOG);

				sb.Append(ChangedArmsText(target, oldArmData));
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (!target.gills.isDefault && Utils.Rand(4) == 0)
			{
				var oldGillData = target.gills.AsReadOnlyData();

				target.RestoreGills();

				sb.Append(RemovedGillsText(target, oldGillData));
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (target is CombatCreature cc && target.body.isFurry && Utils.Rand(3) == 0)
			{

				cc.DeltaCombatCreatureStats(tou: 4, sens: -3);


				sb.Append(FallbackToughenUpText(target));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);
			}

			if (target is CombatCreature cc2 && remainingChanges == changeLimit)
			{
				cc2.AddHP(20);
				sb.Append(NothingHappenedGainHpText(target));
				target.IncreaseLust(3);
			}

			return ApplyChangesAndReturn(target, sb, changeLimit - remainingChanges);

		}

		protected abstract string InitialTransformationText(CanineModifiers modifiers, bool crit);
		protected abstract string BadEndText(Creature target);
		protected abstract string DoggoWarningText(Creature target, bool wasPreviouslyWarned);
		protected abstract string StatChangeText(Creature target, float strengthIncrease, float speedIncrease, float intelligenceDecrease);
		protected abstract string EnlargedSmallestKnotText(Creature target, int indexOfSmallestKnotCock, float knotMultiplierDelta, bool hasOtherDogCocks);
		protected abstract string GrewTwoDogCocksHadNone(Creature target);
		protected abstract string ConvertedFirstDogCockGrewSecond(Creature target, CockData oldCockData);
		protected abstract string ConvertedTwoCocksToDog(Creature target, CockData firstOldData, CockData secondOldData);
		protected abstract string GrewSecondDogCockHadOne(Creature target);
		protected abstract string GrewBallsText(Creature target);
		protected abstract string EnlargedBallsText(Creature target, BallsData oldData);

		protected virtual string RestoredNeckText(Creature target, NeckData oldNeck)
		{
			return target.neck.RestoredText(oldNeck);
		}
		protected virtual string RemovedOvipositionText(Creature target)
		{
			return RemovedOvipositionTextGeneric(target);
		}
		protected virtual string RemovedFeatheryHairText(Creature target)
		{
			return RemovedFeatheryHairTextGeneric(target);
		}
		protected abstract string ConvertedOneCockToDog(Creature target, int index, CockData oldData);
		protected abstract string CouldntConvertDemonCockThickenedInstead(Creature target, int index, float delta);
		protected abstract string AddedCumText(Creature target, float delta, bool deltaIsMultiplier);
		protected abstract string GrewSmallestCockText(Creature target, int index, CockData oldData);
		protected abstract string GrowCurrentBreastRowText(Creature target, int index, byte delta);
		protected abstract string GrewAdditionalBreastRowText(Creature target);
		protected abstract string NormalizedBreastSizeText(Creature target, int index, short cupSizeDelta);
		protected abstract string EnterOrIncreaseHeatText(Creature target, bool isIncrease);
		protected abstract string DoggoFantasyText(Creature target);
		protected virtual string RestoredEyesText(Creature target, EyeData oldEyes)
		{
			return target.eyes.RestoredText(oldEyes);
		}
		protected abstract string ChangedFurText(Creature target, ReadOnlyFurColor oldFurColor);
		protected virtual string ChangedBodyTypeText(Creature target, BodyData oldBodyData)
		{
			return target.body.TransformFromText(oldBodyData);
		}
		protected virtual string ChangedLowerBodyText(Creature target, LowerBodyData oldLegs)
		{
			return target.lowerBody.TransformFromText(oldLegs);
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
			return target.gills.RestoredText(oldGillData);
		}
		protected abstract string FallbackToughenUpText(Creature target);

		protected abstract string NothingHappenedGainHpText(Creature target);
	}
}
