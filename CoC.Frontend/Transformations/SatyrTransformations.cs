//SatyrTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:46:52 PM
using System.Linq;
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
	internal abstract class SatyrTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//huh. no rng rolls, just starts at 3. cool
			int changeCount = GenerateChangeCount(target, null, 3);
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

			//Stats and genital changes
			if (Utils.Rand(2) == 0)
			{
				target.ChangeLust(25);
				if (target.relativeLibido < 100)
				{
					if (target.relativeLibido < 50)
					{
						target.ChangeLibido(1);
					}

					target.ChangeLibido(1);
				}
			}
			Cock smallest = target.genitals.ShortestCock();

			if (target.hasCock && smallest.length < 12 && Utils.Rand(3) == 0)
			{
				smallest.IncreaseLength();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			Cock thinnest = target.genitals.ThinnestCock();

			if (target.hasCock && thinnest.girth < 4 && Utils.Rand(3) == 0)
			{
				thinnest.IncreaseThickness(0.5f);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (target.balls.count > 0 && target.genitals.cumMultiplier < 50 && Utils.Rand(3) == 0)
			{
				target.ChangeLust(20);

				if (target.genitals.cumMultiplier < 10)
				{
					target.genitals.IncreaseCumMultiplier(1);
				}

				if (target.genitals.cumMultiplier < 50)
				{
					target.genitals.IncreaseCumMultiplier(0.5f);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(3) == 0 && target.hasVagina && target.genitals.standardBonusVaginalCapacity > 0)
			{
				target.genitals.DecreaseBonusVaginalCapacity((ushort)(Utils.Rand(5) + 5));
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(3) == 0 && target.hasVagina && !target.hasCock)
			{
				target.RemoveAllVaginas();
				target.AddCock(CockType.HUMAN, 6, 1);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(3) == 0 && target.hasCock && !target.balls.hasBalls)
			{
				target.balls.GrowBalls();

				target.HaveGenericCockOrgasm(0, true, true);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Transformations
			//Neck restore
			if (target.neck.type != NeckType.HUMANOID && Utils.Rand(4) == 0)
			{
				target.RestoreNeck();
			}
			//Rear body restore
			if (!target.back.isDefault && Utils.Rand(5) == 0)
			{
				target.RestoreBack();
			}
			//Ovi perk loss
			if (target.womb.canRemoveOviposition && Utils.Rand(5) == 0)
			{
				if (target.womb.ClearOviposition())
				{
					sb.Append(RemovedOvipositionText(target));
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}

			//remove anything with a scaly or partially scaly body. now affects cockatrice too!
			if (Utils.Rand(3) == 0 && target.body.HasAny(EpidermisType.SCALES))
			{
				BodyData oldData = target.body.AsReadOnlyData();
				target.RestoreBody();
				sb.Append(RestoredBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(3) == 0 && target.arms.type != ArmType.HUMAN)
			{
				ArmData oldData = target.arms.AsReadOnlyData();
				target.RestoreArms();
				sb.Append(RestoredArmsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(4) == 0 && target.lowerBody.type != LowerBodyType.CLOVEN_HOOVED)
			{
				LowerBodyData oldData = target.lowerBody.AsReadOnlyData();
				target.UpdateLowerBody(LowerBodyType.CLOVEN_HOOVED);
				sb.Append(UpdateLowerBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(3) == 0 && target.lowerBody.type == LowerBodyType.CLOVEN_HOOVED && target.horns.type == HornType.GOAT && target.face.type != FaceType.HUMAN)
			{
				FaceData oldData = target.face.AsReadOnlyData();
				target.UpdateFace(FaceType.HUMAN);
				sb.Append(UpdateFaceText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//MOD: anything with scales will prevent this, including partial scales (a la cockatrice). since body is reworked a lot of these checks are hard to port,
			//but this i think is the closest i can get.
			if (Utils.Rand(4) == 0 && !target.body.HasAny(EpidermisType.SCALES) && target.ears.type != EarType.ELFIN)
			{
				EarData oldData = target.ears.AsReadOnlyData();
				target.UpdateEars(EarType.ELFIN);
				sb.Append(UpdateEarsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(3) == 0 && target.horns.type == HornType.NONE)
			{
				HornData oldData = target.horns.AsReadOnlyData();
				target.UpdateHorns(HornType.GOAT);
				sb.Append(UpdateHornsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Mod Note: Horns:

			if (Utils.Rand(3) == 0 && target.horns.type != HornType.GOAT)
			{
				HornData oldData = target.horns.AsReadOnlyData();
				target.UpdateHorns(HornType.GOAT);
				sb.Append(UpdateHornsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(3) == 0 && target.horns.type == HornType.GOAT && target.horns.CanStrengthen)
			{
				target.horns.StrengthenTransform();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(4) == 0 && target.antennae.type != AntennaeType.NONE)
			{
				AntennaeData oldData = target.antennae.AsReadOnlyData();
				target.RestoreAntennae();
				sb.Append(RestoredAntennaeText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(3) == 0 && target.cocks.Count == 1 && target.cocks[0].type != CockType.HUMAN)
			{
				target.genitals.UpdateCock(0, CockType.HUMAN);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(3) == 0 && target.cocks.Count > 1 && !target.genitals.OnlyHasCocksOfType(CockType.HUMAN))
			{
				target.genitals.UpdateCock(target.cocks.First(x => x.type != CockType.HUMAN), CockType.HUMAN);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(3) == 0 && target.tail.type != TailType.SATYR)
			{
				TailData oldData = target.tail.AsReadOnlyData();
				target.UpdateTail(TailType.SATYR);
				sb.Append(UpdateTailText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		protected virtual string UpdateLowerBodyText(Creature target, LowerBodyData oldData)
{
return target.lowerBody.TransformFromText(oldData);
}

		protected virtual string UpdateFaceText(Creature target, FaceData oldData)
{
return target.face.TransformFromText(oldData);
}

		protected virtual string UpdateEarsText(Creature target, EarData oldData)
{
return target.ears.TransformFromText(oldData);
}
		protected virtual string UpdateHornsText(Creature target, HornData oldData)
{
return target.horns.TransformFromText(oldData);
}

		protected virtual string UpdateTailText(Creature target, TailData oldTail)
{
return target.tail.TransformFromText(oldTail);
}
		protected virtual string RestoredBodyText(Creature target, BodyData oldData)
{
return target.body.RestoredText(oldData);
}

		protected virtual string RestoredArmsText(Creature target, ArmData oldData)
{
return target.arms.RestoredText(oldData);
}

		protected virtual string RestoredAntennaeText(Creature target, AntennaeData oldData)
{
return target.antennae.RestoredText(oldData);
}

		protected abstract string RemovedOvipositionText(Creature target);

		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target);
	}
}