//DeerTransformations.cs
//Description:
//Author: JustSomeGuy
//1/23/2020 4:08:12 AM
using System.Text;
using System.Linq;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Settings.Gameplay;

namespace CoC.Frontend.Transformations
{
	//note: This probably could be used for reindeer tfs, as well. however, as far as i can tell the reindeer tf item
	//is relatively simplistic and doesn't do much, so for now it's not really a full transformation. i suppose it could be,
	//and we can make that happen if needed.

	internal abstract class DeerTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;

		/**
		 * Original Credits:
		 * Golden Rind/Deer TF, part of the Wild Hunt by Frogapus
		 * @author Kitteh6660
		 *
		 * As with all of these, template comments may be left in, but other than that, any comments from the port will be marked with MOD.
		 */

		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			int changeCount = GenerateChangeCount(target, new int[] { 2, 3 }, 2);
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

			//Main TFs
			if (!target.neck.isDefault && Utils.Rand(4) == 0) //neck restore
			{
				target.RestoreNeck();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			if (!target.back.isDefault && Utils.Rand(5) == 0) //rear body restore
			{
				target.RestoreBack();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			if (target.womb.canRemoveOviposition &&Utils.Rand(5) == 0) //ovi perk loss
			{
				target.womb.ClearOviposition();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			if (Utils.Rand(3) == 0 && target.ears.type != EarType.DEER)
			{
				target.UpdateEars(EarType.DEER); //gain deer ears

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			if (Utils.Rand(3) == 0 && target.ears.type == EarType.DEER && target.tail.type != TailType.DEER)
			{
				target.UpdateTail(TailType.DEER); //gain deer tail

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//MOD: Horns now get a common check.
			if (Utils.Rand(3) == 0)
			{
				if (target.horns.type == HornType.NONE)
				{

					target.UpdateHorns(HornType.DEER_ANTLERS); //gain deer horns AKA antlers

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
				else if (target.horns.type != HornType.DEER_ANTLERS)
				{
					target.UpdateHornsAndStrengthenTransform(HornType.DEER_ANTLERS, 1); //gain deer horns AKA antlers


					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
				else if (target.horns.numHorns < 30)
				{

					if (target.horns.numHorns < 20 && Utils.Rand(2) == 0)
					{
						target.horns.StrengthenTransform(2);
					}
					else
					{
						target.horns.StrengthenTransform();
					}

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			if (Utils.Rand(4) == 0 && target.horns.numHorns > 0 && !target.body.IsFurBodyType())
			{
				target.UpdateBody(BodyType.UNDERBODY_FUR, new FurColor(HairFurColors.BROWN), new FurColor(HairFurColors.WHITE));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			if (Utils.Rand(3) == 0 && target.ears.type == EarType.DEER && !target.face.isDefault && target.face.type != FaceType.DEER)
			{
				target.RestoreFace(); //change face to human

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			if (Utils.Rand(4) == 0 && target.body.IsFurBodyType() && target.ears.type == EarType.DEER && target.tail.type == TailType.DEER && target.face.type != FaceType.DEER)
			{
				target.UpdateFace(FaceType.DEER); //gain deer face

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			if (Utils.Rand(4) == 0 && target.ears.type == EarType.DEER && target.tail.type == TailType.DEER && target.body.IsFurBodyType() && target.lowerBody.type != LowerBodyType.CLOVEN_HOOVED)
			{
				target.UpdateLowerBody(LowerBodyType.CLOVEN_HOOVED); //change legs to cloven hooves

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Genital Changes
			//morph dick to horsediiiiick
			//MOD: now checks to see if you have a cock that isn't a horse dick first.
			if (Utils.Rand(3) == 0 && target.cocks.Count > 0 && target.cocks.Any(x=>x.type != CockType.HORSE))
			{
				var selectedCock = target.cocks.First(x => x.type != CockType.HORSE);

				target.genitals.UpdateCockWithLength(selectedCock, CockType.HORSE, selectedCock.length + 4);

				target.IncreaseCreatureStats(lib: 5, sens: 4, lus: 35);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Body thickness/tone changes
			if (Utils.Rand(3) == 0 && target.build.muscleTone > 20)
			{
				if (target.build.muscleTone > 50)
				{
					target.build.ChangeMuscleToneToward(20, (byte)(2 + Utils.Rand(3)));
				}
				else
				{
					target.build.ChangeMuscleToneToward(20, 2);
				}
			}
			if (Utils.Rand(3) == 0 && target.build.thickness > 20)
			{
				if (target.build.thickness > 50)
				{
					target.build.ChangeThicknessToward(20, (byte)(2 + Utils.Rand(3)));
				}
				else
				{
					target.build.ChangeThicknessToward(20, 2);
				}
			}

			//paste this line after any tf is applied, and it will: automatically decrement the remaining changes count. if it becomes 0 or less, apply the total number of changes
			//underwent to the target's change count (if applicable) and then return the StringBuilder content.
			//if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);




			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target);
	}
}