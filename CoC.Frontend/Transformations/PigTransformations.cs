//PigTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:52:28 PM
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
	internal abstract class PigTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;

		protected readonly bool isEnhanced;
		protected PigTransformations(bool enhanced)
		{
			this.isEnhanced = enhanced;
		}


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 2 }, isEnhanced ? 2 : 1);
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

			//-----------------------
			// SIZE MODIFICATIONS
			//-----------------------
			//Increase thickness
			if (Utils.Rand(3) == 0 && target.build.thickness < 75)
			{
				target.build.ChangeThicknessToward(75, 3);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Decrease muscle tone
			if (Utils.Rand(3) == 0 && target.gender.HasFlag(Gender.FEMALE) && target.build.muscleTone > 20)
			{
				target.build.ChangeMuscleToneToward(20, 4);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Increase hip rating
			if (Utils.Rand(3) == 0 && target.gender.HasFlag(Gender.FEMALE) && target.hips.size < 15)
			{
				target.hips.GrowHips();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Increase ass rating
			if (Utils.Rand(3) == 0 && target.butt.size < 12)
			{
				target.butt.GrowButt();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Increase ball size if you have balls.
			if (Utils.Rand(3) == 0 && target.balls.count > 0 && target.balls.size < 4)
			{
				target.balls.EnlargeBalls(1);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Neck restore
			if (target.neck.type != NeckType.HUMANOID && Utils.Rand(4) == 0)
			{
				target.RestoreNeck();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Rear body restore
			if (!target.back.isDefault && Utils.Rand(5) == 0)
			{
				target.RestoreBack();
			}
			//Ovi perk loss
			if (target.womb.canRemoveOviposition && Utils.Rand(5) == 0)
			{
				target.womb.ClearOviposition();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-----------------------
			// TRANSFORMATIONS
			//-----------------------
			//Gain pig cock, independent of other pig TFs.
			if (Utils.Rand(4) == 0 && target.hasCock && !target.genitals.OnlyHasCocksOfType(CockType.PIG))
			{
				if (target.cocks.Count == 1)
				{ //Single cock
					target.genitals.UpdateCock(0, CockType.PIG);
				}
				else
				{ //Multiple cocks
					target.genitals.UpdateCock(target.cocks.First(x => x.type != CockType.PIG), CockType.PIG);
				}
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Gain pig ears!
			if (Utils.Rand(isEnhanced ? 3 : 4) == 0 && target.ears.type != EarType.PIG)
			{
				target.UpdateEars(EarType.PIG);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Gain pig tail if you already have pig ears!
			if (Utils.Rand(isEnhanced ? 2 : 3) == 0 && target.ears.type == EarType.PIG && target.tail.type != TailType.PIG)
			{
				if (!target.tail.isDefault) //If you have non-pig tail.
				{}
				else //If you don't have a tail.
				{
					target.UpdateTail(TailType.PIG);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Gain pig tail even when centaur, needs pig ears.
			if (Utils.Rand(isEnhanced ? 2 : 3) == 0 && target.ears.type == EarType.PIG && target.tail.type != TailType.PIG && target.lowerBody.isQuadruped && (target.lowerBody.type == LowerBodyType.HOOVED || target.lowerBody.type == LowerBodyType.PONY))
			{
				target.UpdateTail(TailType.PIG);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Turn your lower body into pig legs if you have pig ears and tail.
			if (Utils.Rand(isEnhanced ? 3 : 4) == 0 && target.ears.type == EarType.PIG && target.tail.type == TailType.PIG && target.lowerBody.type != LowerBodyType.CLOVEN_HOOVED)
			{
				if (target.lowerBody.isQuadruped) //Centaur
				{}
				else if (target.lowerBody.type == LowerBodyType.NAGA) //Naga
				{}
				else //Bipedal
				{
					target.UpdateLowerBody(LowerBodyType.CLOVEN_HOOVED);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Gain pig face when you have the first three pig TFs.
			if (Utils.Rand(isEnhanced ? 2 : 3) == 0 && target.ears.type == EarType.PIG && target.tail.type == TailType.PIG && target.lowerBody.type == LowerBodyType.CLOVEN_HOOVED && target.face.type != FaceType.PIG)
			{
				target.UpdateFace(FaceType.PIG);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Gain enhanced face if you have pig face.
			if (Utils.Rand(3) == 0 && target.ears.type == EarType.PIG && target.tail.type == TailType.PIG && target.lowerBody.type == LowerBodyType.CLOVEN_HOOVED && target.face.type == FaceType.PIG
				&& !target.face.isFullMorph)
			{
				target.face.StrengthenFacialMorph();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Change skin colour
			if (Utils.Rand(isEnhanced ? 3 : 4) == 0)
			{
				//String skinToBeChosen = Utils.RandomChoice(enhanced?["dark brown", "brown", "brown"] : ["pink", "tan", "sable"]);
				//target.body.primarySkin.tone = skinToBeChosen;
				//target.arms.updateClaws(target.arms.claws.type);
				//getGame().rathazul.addMixologyXP(20);
				//if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			if (remainingChanges == changeCount)
			{
			}


			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract bool InitialTransformationText(Creature target);
	}
}