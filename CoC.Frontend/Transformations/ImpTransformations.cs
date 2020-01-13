using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.UI;
using System;
using System.Text;

namespace CoC.Frontend.Transformations
{
	/**
	 * Original Credits:
	 * Based on the Imp transformative item
	 * fucking overhauled by Foxwells who was depressed by the sorry state of imp food
	 */
	internal abstract class ImpTransformation : GenericTransformationBase
	{
		StandardDisplay currentDisplay => DisplayManager.GetCurrentDisplay();
		private bool hyperHappy => SaveData.FrontendSessionSave.data?.HyperHappyLocal ?? false;

		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			int changeCount = GenerateChanceCount(target, new int[] { 2, 2 });
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

			sb.Append(InitialTransformText(target));
			target.DeltaCreatureStats(lus: 3, corr: 1);

			uint hpDelta;
			if (target.hasCock)
			{
				if (target.cocks[0].length < 12)
				{
					float temp = target.cocks[0].LengthenCock(Utils.Rand(2) + 2);
					sb.Append(OneCockGrewLarger(target, target.cocks[0], temp));
				}
				hpDelta = 30;
			}
			else
			{
				hpDelta = 20;
			}

			if (target is CombatCreature healthCheck)
			{
				healthCheck.AddHP((uint)(hpDelta + healthCheck.toughness / 3));
				sb.Append(GainVitalityText(target));
			}
			if (remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			//Red or orange skin!
			if (Utils.Rand(30) == 0 && !Array.Exists(Species.IMP.availableTones, x => x == target.body.primarySkin.tone))
			{
				var oldSkinTone = target.body.primarySkin.tone;
				if (target.body.ChangeMainSkin(Utils.RandomChoice(Species.IMP.availableTones)))
				{
					sb.Append(ChangeSkinColorText(target, oldSkinTone));

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}


			//Shrinkage!
			if (Utils.Rand(2) == 0 && target.build.heightInInches > 42)
			{
				//currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "Your skin crawls, making you close your eyes and shiver. When you open them again the world seems... different. After a bit of investigation, you realize you've become shorter!");
				byte heightDelta = target.build.GetShorter((byte)(1 + Utils.Rand(3)));
				if (heightDelta > 0)
				{
					sb.Append(GetShorterText(target, heightDelta));

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Imp wings - I just kinda robbed this from demon changeCount ~Foxwells
			if (Utils.Rand(3) == 0 && ((target.wings.type != WingType.IMP && target.IsCorruptEnough(25)) || (target.wings.type == WingType.IMP && target.IsCorruptEnough(50))))
			{
				bool changedWings = false;
				//grow smalls to large
				if (target.wings.type == WingType.IMP)
				{
					if (target.wings.GrowLarge())
					{
						sb.Append(EnlargenedImpWingsText(target));
						changedWings = true;
					}
				}
				else
				{
					var oldData = target.wings.AsReadOnlyData();
					if (target.UpdateWings(WingType.IMP))
					{
						sb.Append(GrowOrChangeWingsText(target, oldData));
						changedWings = true;
					}
				}

				if (changedWings)
				{
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Imp tail, because that's a unique thing from what I see?
			if (Utils.Rand(3) == 0 && target.tail.type != TailType.IMP)
			{
				var oldData = target.tail.AsReadOnlyData();
				if (target.UpdateTail(TailType.IMP))
				{
					sb.Append(GrowOrChangeTailText(target, oldData));
					target.IncreaseCorruption(2);

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Feets, needs red/orange skin and tail
			if (Species.IMP.availableTones.Contains(target.body.primarySkin.tone) && target.tail.type == TailType.IMP && target.lowerBody.type != LowerBodyType.IMP && Utils.Rand(3) == 0)
			{
				var oldData = target.lowerBody.AsReadOnlyData();
				if (target.UpdateLowerBody(LowerBodyType.IMP))
				{
					sb.Append(ChangeLowerBodyText(target, oldData));

					target.IncreaseCorruption(2);

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Imp ears, needs red/orange skin and horns
			if (target.horns.type == HornType.IMP && Array.Exists(Species.IMP.availableTones, x => target.body.primarySkin.tone == x) && target.ears.type != EarType.IMP && Utils.Rand(3) == 0)
			{
				var oldData = target.ears.AsReadOnlyData();
				if (target.UpdateEar(EarType.IMP))
				{
					sb.Append(ChangeEarsText(target, oldData));

					target.IncreaseCorruption(2);
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Horns, because why not?
			if (target.horns.type != HornType.IMP && Utils.RandBool())
			{
				var oldData = target.horns.AsReadOnlyData();
				if (target.UpdateHorns(HornType.IMP))
				{
					sb.Append(ChangeOrGrowHornsText(target, oldData));

					target.IncreaseCorruption(2);

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Imp arms, needs orange/red skin. Also your hands turn human.
			if (Species.IMP.availableTones.Contains(target.body.primarySkin.tone) && target.arms.type != ArmType.IMP && Utils.Rand(3) == 0)
			{
				var oldData = target.arms.AsReadOnlyData();
				if (target.UpdateArms(ArmType.IMP))
				{
					sb.Append(ChangeArmText(target, oldData));

					target.IncreaseCorruption(2);
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

			}

			//Changes hair to red/dark red, shortens it, sets it normal, and makes it curly.
			if (!Species.IMP.availableHairColors.Contains(target.hair.hairColor) && Utils.Rand(3) == 0)
			{
				var oldHairData = target.hair.AsReadOnlyData();

				var hairColor = Utils.RandomChoice(Species.IMP.availableHairColors);
				var hairLength = 1f;

				if (target.hair.type != HairType.NORMAL)
				{
					target.UpdateHair(HairType.NORMAL, hairColor, newHairLength: hairLength, newStyle: HairStyle.CURLY);
				}
				else
				{
					target.hair.SetAll(hairLength, hairColor, style: HairStyle.CURLY);
				}
				sb.Append(HairChangedText(target, oldHairData));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			}

			//Remove spare titties
			if (target.breasts.Count > 1 && Utils.Rand(3) == 0 && !hyperHappy)
			{

				var toRemove = target.breasts[target.breasts.Count - 1].AsReadOnlyData();

				if (target.genitals.RemoveBreastRows() > 0)
				{
					sb.Append(RemovedAnExtraRowOfBreasts(target, toRemove));

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Shrink titties
			if (target.genitals.BiggestCupSize() > CupSize.FLAT && Utils.Rand(3) == 0 && !hyperHappy)
			{
				byte rowsAlreadyModified = 0;
				//temp3 stores how many rows are changed
				foreach (Breasts breast in target.breasts)
				{
					//If this row is over threshhold
					if (breast.cupSize > CupSize.FLAT)
					{
						CupSize oldSize = breast.cupSize;

						byte delta;
						//Big change
						if (breast.cupSize > CupSize.EE_BIG)
						{
							delta = breast.ShrinkBreasts((byte)(2 + Utils.Rand(3)));
						}
						else
						{
							delta = breast.ShrinkBreasts(1);
						}

						if (delta != 0)
						{
							sb.Append(CurrentBreastRowChanged(target, breast.index, delta, rowsAlreadyModified));

							rowsAlreadyModified++;
						}
					}

				}
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}


			//Free extra nipple removal service
			if (target.genitals.quadNipples && Utils.Rand(3) == 0)
			{
				target.genitals.SetQuadNipples(false);

				sb.Append(RemovedQuadNippleText(target));

				target.DecreaseSensitivity(3);
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Neck restore
			if (target.neck.type != NeckType.defaultValue && Utils.Rand(4) == 0)
			{

				var oldData = target.neck.AsReadOnlyData();
				if (target.RestoreNeck())
				{
					sb.Append(RestoredNeckText(target, oldData));

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

			}
			//Rear body restore
			if (target.back.type != BackType.defaultValue && Utils.Rand(5) == 0)
			{
				var oldData = target.back.AsReadOnlyData();
				if (target.RestoreBack())
				{
					sb.Append(RestoredBackText(target, oldData));

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Ovi perk loss
			if (target is Player && Utils.Rand(5) == 0)
			{
				if (((PlayerWomb)target.womb).ClearOviposition())
				{
					sb.Append(RemovedOvipositionText(target));

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//You lotta imp? Time to turn male!
			//Unless you're one of the hyper happy assholes I guess
			//For real tho doesn't seem like female imps exist? Guess they're goblins
			if (target.ImpScore() >= 4 && !hyperHappy)
			{
				bool changedSomething = false;
				var oldGenitals = target.genitals.AsReadOnlyData();

				changedSomething |= target.genitals.RemoveExtraBreastRows() > 0;
				changedSomething |= target.breasts[0].MakeMale(true);
				changedSomething |= target.RemoveAllVaginas() > 0;

				if (!target.hasCock)
				{

					changedSomething |= target.AddCock(CockType.HUMAN, 12, 2);
				}
				if (target.balls.count == 0)
				{
					changedSomething |= target.genitals.GrowBalls(2);
				}

				if (changedSomething)
				{
					sb.Append(ImpifiedText(target, oldGenitals));
					remainingChanges--;
					target.IncreaseCorruption(20);
				}


			}
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		protected abstract string ImpifiedText(Creature target, GenitalsData oldGenitals);
		protected virtual string RemovedOvipositionText(Creature target)
		{
			return RemovedOvipositionTextGeneric(target);
		}
		protected virtual string RestoredBackText(Creature target, BackData oldBack)
		{
			return target.back.RestoredText(oldBack);
		}
		protected virtual string RestoredNeckText(Creature target, NeckData oldNeck)
		{
			return target.neck.RestoredText(oldNeck);
		}
		protected abstract string RemovedQuadNippleText(Creature target);
		protected abstract string CurrentBreastRowChanged(Creature target, int index, byte cupSizesShrunk, byte rowsPreviouslyModified);
		protected virtual string RemovedAnExtraRowOfBreasts(Creature target, BreastData removedRow)
		{
			return RemovedExtraBreastRowGenericText(target, removedRow);
		}
		protected abstract string HairChangedText(Creature target, HairData oldHairData);
		protected virtual string ChangeArmText(Creature target, ArmData oldArms)
		{
			return target.arms.TransformFromText(oldArms);
		}
		protected virtual string ChangeOrGrowHornsText(Creature target, HornData oldHorns)
		{
			return target.horns.TransformFromText(oldHorns);
		}
		protected virtual string ChangeEarsText(Creature target, EarData oldEars)
		{
			return target.ears.TransformFromText(oldEars);
		}
		protected virtual string ChangeLowerBodyText(Creature target, LowerBodyData oldLegs)
		{
			return target.lowerBody.TransformFromText(oldLegs);
		}
		protected abstract string InitialTransformText(Creature target);
		protected abstract string OneCockGrewLarger(Creature target, Cock cock, float deltaSize);
		protected abstract string GainVitalityText(Creature target);
		protected abstract string ChangeSkinColorText(Creature target, Tones oldSkinTone);
		protected abstract string GetShorterText(Creature target, byte heightDelta);
		protected virtual string EnlargenedImpWingsText(Creature target)
		{
			return target.wings.ChangedSizeText(false);
		}
		protected virtual string GrowOrChangeWingsText(Creature target, WingData oldWings)
		{
			return target.wings.TransformFromText(oldWings);
		}
		protected virtual string GrowOrChangeTailText(Creature target, TailData oldTail)
		{
			return target.tail.TransformFromText(oldTail);
		}
	}
}