using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.UI;
using System.Text;

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
			if (target is CombatCreature cc)
			{
				if (cc.intelligence > 90 && Utils.Rand(3) == 0)
				{
					cc.DeltaCombatCreatureStats(inte: -(Utils.Rand(2) + 1));
				}
				if (Utils.Rand(3) == 0)
				{
					cc.DeltaCombatCreatureStats(tou: Utils.Rand(2) + 1);
				}
				if (cc.speed < 75 && Utils.Rand(3) == 0)
				{
					cc.DeltaCombatCreatureStats(spe: Utils.Rand(2) + 1);
				}
			}
			if (Utils.Rand(3) == 0)
			{
				target.DeltaCreatureStats(sens: -(Utils.Rand(2) + 1));
			}
			if (Utils.Rand(3) == 0)
			{
				target.DeltaCreatureStats(corr: -(Utils.Rand(3) + 2));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			if (target.heightInInches > 67 && Utils.Rand(2) == 0)
			{
				target.build.GetShorter((byte)(1 + Utils.Rand(4)));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
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

			if (remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			//Neck restore
			if (target.neck.type != NeckType.defaultValue && Utils.Rand(4) == 0)
			{
				var oldNeck = target.neck.AsReadOnlyData();
				target.RestoreNeck();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Rear body restore
			if (target.back.type != BackType.defaultValue && Utils.Rand(5) == 0)
			{
				var oldBack = target.back.AsReadOnlyData();
				target.RestoreBack();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			}
			if (target.ears.type != EarType.SHEEP && Utils.Rand(3) == 0)
			{
				var oldEars = target.ears.AsReadOnlyData();
				target.UpdateEars(EarType.SHEEP);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			if (target.tail.type != TailType.SHEEP && Utils.Rand(3) == 0)
			{
				var oldTail = target.tail.AsReadOnlyData();
				target.UpdateTail(TailType.SHEEP);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			if (target.lowerBody.type != LowerBodyType.CLOVEN_HOOVED && target.tail.type == TailType.SHEEP && Utils.Rand(3) == 0)
			{
				var oldLegs = target.lowerBody.AsReadOnlyData();
				target.UpdateLowerBody(LowerBodyType.CLOVEN_HOOVED);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			}
			if (target.horns.type != HornType.SHEEP && target.ears.type == EarType.SHEEP && Utils.Rand(3) == 0)
			{
				var oldHorns = target.horns.AsReadOnlyData();
				target.UpdateHorns(HornType.SHEEP);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			if (Utils.Rand(3) == 0 && target.lowerBody.type == LowerBodyType.CLOVEN_HOOVED && target.horns.type == HornType.SHEEP && target.tail.type == TailType.SHEEP &&
				target.ears.type == EarType.SHEEP && target.body.type != BodyType.WOOL)
			{
				var oldBody = target.body.AsReadOnlyData();
				target.UpdateBody(BodyType.WOOL);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//shrinks horns if too feminine and grows if masculine
			if (target.horns.type == HornType.SHEEP && target.body.type == BodyType.WOOL && target.femininity <= 45 && Utils.Rand(3) == 0)
			{
				var oldHornData = target.horns.AsReadOnlyData();
				target.horns.StrengthenTransform();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			if (target.body.type == BodyType.WOOL && target.hair.type != HairType.WOOL && target.femininity >= 65 && Utils.Rand(3) == 0)
			{
				var oldHair = target.hair.AsReadOnlyData();
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

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges); ;
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
	}
}