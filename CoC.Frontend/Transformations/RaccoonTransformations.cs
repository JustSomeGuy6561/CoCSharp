//RaccoonTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:49:16 PM
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;

namespace CoC.Frontend.Transformations
{
	internal abstract class RaccoonTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 3, 3 });
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

			//eat it:

			//stat gains:
			//gain speed to ceiling of 80
			if (target.relativeSpeed < 80 && Utils.Rand(3) == 0)
			{
				if (target.relativeSpeed < 40)
				{
					target.IncreaseSpeed(2);
				}
				else
				{
					target.IncreaseSpeed();
				}
			}

			//lose toughness to floor of 50
			if (Utils.Rand(4) == 0 && target.relativeToughness > 50)
			{
				if (target.relativeToughness > 75)
				{
					target.DecreaseToughness(2);
				}
				else
				{
					target.DecreaseToughness();
				}
			}
			//gain sensitivity
			if (target.relativeSensitivity < 80 && Utils.Rand(3) == 0)
			{
				if (target.relativeSensitivity < 60)
				{
					target.IncreaseSensitivity(4);
				}
				else
				{
					target.IncreaseSensitivity(2);
				}
			}

			//Sex stuff
			if (target.hasCock)
			{
				//gain ball size
				if (target.balls.count > 0 && target.balls.size < 15 && Utils.Rand(4) == 0)
				{
					target.balls.EnlargeBalls((byte)(2 + Utils.Rand(3)));
					target.ChangeLibido(1);
				}
			}
			//gain balls up to 2 (only if full-coon face and fur; no dick required)
			if (target.balls.count == 0 && target.body.IsFurBodyType() && target.face.type == FaceType.RACCOON && Utils.Rand(3) == 0)
			{
				target.balls.GrowBalls();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//gain thickness or lose tone or whatever - standard message
			if (Utils.Rand(4) == 0 && target.build.thickness < 80)
			{
				target.build.ChangeThicknessToward(80, 2);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Neck restore
			if (target.neck.type != NeckType.HUMANOID && Utils.Rand(4) == 0)
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
			//bodypart changes:
			if (target.tail.type != TailType.RACCOON && Utils.Rand(4) == 0)
			{
				TailData oldData = target.tail.AsReadOnlyData();
				target.UpdateTail(TailType.RACCOON);
				sb.Append(UpdateTailText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//gain fur
			if (target.lowerBody.type == LowerBodyType.RACCOON && target.ears.type == EarType.RACCOON && !target.body.IsFurBodyType() && Utils.Rand(4) == 0)
			{
				BodyData oldData = target.body.AsReadOnlyData();
				target.UpdateBody(BodyType.SIMPLE_FUR, new FurColor(HairFurColors.GRAY));
				sb.Append(UpdateBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//gain coon ears
			if (target.tail.type == TailType.RACCOON && target.ears.type != EarType.RACCOON && Utils.Rand(4) == 0)
			{
				//from dog, kangaroo, bunny, other long ears
				//from cat, horse, cow ears
				//from human, goblin, lizard or other short ears
				EarData oldData = target.ears.AsReadOnlyData();
				target.UpdateEars(EarType.RACCOON);
				sb.Append(UpdateEarsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//gain feet-coon
			if (target.ears.type == EarType.RACCOON && target.lowerBody.type != LowerBodyType.RACCOON && Utils.Rand(4) == 0)
			{
				//from naga non-feet (gain fatigue and lose lust)
				if (target.lowerBody.type == LowerBodyType.NAGA)
				{
					target.ChangeLust(-30);
					(target as CombatCreature)?.GainFatigue(5);
				}

				LowerBodyData oldData = target.lowerBody.AsReadOnlyData();
				target.UpdateLowerBody(LowerBodyType.RACCOON);
				sb.Append(UpdateLowerBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//gain half-coon face (prevented if already full-coon)
			if (target.face.type != FaceType.RACCOON && Utils.Rand(4) == 0)
			{
				FaceData oldData = target.face.AsReadOnlyData();
				target.UpdateFace(FaceType.RACCOON);
				sb.Append(UpdateFaceText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//gain full-coon face (requires half-coon and fur)
			//from humanoid - should be the only one possible
			else if (target.face.type == FaceType.RACCOON && !target.face.isFullMorph && target.lowerBody.type == LowerBodyType.RACCOON && target.body.IsFurBodyType() && Utils.Rand(4) == 0)
			{
				target.face.StrengthenFacialMorph();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//fatigue damage (only if face change was not triggered)
			else if (target is CombatCreature && Utils.Rand(2) == 0 && target.face.type != FaceType.RACCOON)
			{
				(target as CombatCreature).GainFatigue(10);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (target is CombatCreature && remainingChanges == changeCount)
			{
				(target as CombatCreature).GainFatigue(5);
			}


			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		protected virtual string ClearOvipositionText(Creature target)
{
return RemovedOvipositionTextGeneric(target);
}

		protected virtual string UpdateTailText(Creature target, TailData oldTail)
		{
			return target.tail.TransformFromText(oldTail);
		}
		protected virtual string UpdateBodyText(Creature target, BodyData oldData)
		{
			return target.body.TransformFromText(oldData);
		}

		protected virtual string UpdateEarsText(Creature target, EarData oldData)
		{
			return target.ears.TransformFromText(oldData);
		}
		protected virtual string UpdateLowerBodyText(Creature target, LowerBodyData oldData)
		{
			return target.lowerBody.TransformFromText(oldData);
		}

		protected virtual string UpdateFaceText(Creature target, FaceData oldData)
		{
			return target.face.TransformFromText(oldData);
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