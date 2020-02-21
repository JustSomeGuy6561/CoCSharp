using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.UI;

namespace CoC.Frontend.Transformations
{
	/**
	 * Original credits:
	 *
	 * written by MissBlackthorne and coded by Foxwells
	 */
	internal abstract class SheepTransformations : GenericTransformationBase
	{

		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			int changeCount = GenerateChangeCount(target, new int[] { 2, 2 });
			int remainingChanges = changeCount;
			StringBuilder sb = new StringBuilder();

			//sb.Append(InitialTransformText(target));

			// Stat changes!
			if (target.intelligence > 90 && Utils.Rand(3) == 0)
			{
				target.ChangeIntelligence(-(Utils.Rand(2) + 1));
			}
			if (Utils.Rand(3) == 0)
			{
				target.ChangeToughness(Utils.Rand(2) + 1);
			}
			if (target.speed < 75 && Utils.Rand(3) == 0)
			{
				target.ChangeSpeed(Utils.Rand(2) + 1);
			}
			if (Utils.Rand(3) == 0)
			{
				target.ChangeSensitivity(-(Utils.Rand(2) + 1));
			}
			if (Utils.Rand(3) == 0)
			{
				target.ChangeCorruption(-(Utils.Rand(3) + 2));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (target.heightInInches > 67 && Utils.Rand(2) == 0)
			{
				target.build.DecreaseHeight((byte)(1 + Utils.Rand(4)));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (target.butt.size < 6 && Utils.Rand(3) == 0)
			{
				if (target.butt.size < 5)
				{
					target.butt.GrowButt((byte)(1 + Utils.Rand(2)));
				}
				else
				{
					target.butt.GrowButt(1);
				}

			}

			if (remainingChanges <= 0)
			{
				return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Neck restore
			if (target.neck.type != NeckType.defaultValue && Utils.Rand(4) == 0)
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
			if (target.back.type != BackType.defaultValue && Utils.Rand(5) == 0)
			{
				BackData oldData = target.back.AsReadOnlyData();
				target.RestoreBack();
				sb.Append(RestoredBackText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (target.ears.type != EarType.SHEEP && Utils.Rand(3) == 0)
			{
				EarData oldData = target.ears.AsReadOnlyData();
				target.UpdateEars(EarType.SHEEP);
				sb.Append(UpdateEarsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (target.tail.type != TailType.SHEEP && Utils.Rand(3) == 0)
			{
				TailData oldData = target.tail.AsReadOnlyData();
				target.UpdateTail(TailType.SHEEP);
				sb.Append(UpdateTailText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (target.lowerBody.type != LowerBodyType.CLOVEN_HOOVED && target.tail.type == TailType.SHEEP && Utils.Rand(3) == 0)
			{
				LowerBodyData oldData = target.lowerBody.AsReadOnlyData();
				target.UpdateLowerBody(LowerBodyType.CLOVEN_HOOVED);
				sb.Append(UpdateLowerBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (target.horns.type != HornType.SHEEP && target.ears.type == EarType.SHEEP && Utils.Rand(3) == 0)
			{
				HornData oldData = target.horns.AsReadOnlyData();
				target.UpdateHorns(HornType.SHEEP);
				sb.Append(UpdateHornsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(3) == 0 && target.lowerBody.type == LowerBodyType.CLOVEN_HOOVED && target.horns.type == HornType.SHEEP && target.tail.type == TailType.SHEEP &&
				target.ears.type == EarType.SHEEP && target.body.type != BodyType.WOOL)
			{
				BodyData oldData = target.body.AsReadOnlyData();
				target.UpdateBody(BodyType.WOOL);
				sb.Append(UpdateBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//shrinks horns if too feminine and grows if masculine
			if (target.horns.type == HornType.SHEEP && target.body.type == BodyType.WOOL && target.femininity <= 45 && Utils.Rand(3) == 0)
			{
				HornData oldHornData = target.horns.AsReadOnlyData();
				target.horns.StrengthenTransform();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (target.body.type == BodyType.WOOL && target.hair.type != HairType.WOOL && target.femininity >= 65 && Utils.Rand(3) == 0)
			{
				HairData oldHair = target.hair.AsReadOnlyData();
				target.UpdateHair(HairType.WOOL);
			}
			if (target.hips.size < 10 && target.femininity >= 65 && Utils.Rand(3) == 0)
			{
				byte oldSize = target.hips.size;
				if (target.hips.size == 9)
				{
					target.hips.GrowHips(1);
				}
				else
				{
					target.hips.GrowHips((byte)(Utils.Rand(2) + 1));
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				};
			}
			if (target.breasts[0].cupSize < CupSize.DD && target.femininity >= 65 && Utils.Rand(3) == 0)
			{
				if (target.breasts[0].cupSize == CupSize.D)
				{
					target.breasts[0].GrowBreasts(1);
				}
				else
				{
					target.breasts[0].GrowBreasts((byte)(Utils.Rand(2) + 1));
				}
			}

			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		protected virtual string UpdateEarsText(Creature target, EarData oldData)
		{
			return target.ears.TransformFromText(oldData);
		}
		protected virtual string UpdateTailText(Creature target, TailData oldTail)
		{
			return target.tail.TransformFromText(oldTail);
		}
		protected virtual string UpdateLowerBodyText(Creature target, LowerBodyData oldData)
		{
			return target.lowerBody.TransformFromText(oldData);
		}

		protected virtual string UpdateHornsText(Creature target, HornData oldData)
		{
			return target.horns.TransformFromText(oldData);
		}

		protected virtual string UpdateBodyText(Creature target, BodyData oldData)
		{
			return target.body.TransformFromText(oldData);
		}

		protected virtual string RestoredBackText(Creature target, BackData oldData)
		{
			return target.back.RestoredText(oldData);
		}

		protected virtual string RestoredNeckText(Creature target, NeckData oldData)
		{
			return target.neck.RestoredText(oldData);
		}

	}
}