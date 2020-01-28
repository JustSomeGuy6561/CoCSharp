//CentaurTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:43:06 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;
using System.Text;

namespace CoC.Frontend.Transformations
{
	internal abstract class CentaurTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;

		//this tf is pretty lame. feel free to make it less lame buy incorporating satyr or horse tfs.

		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 3 });
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
			if (remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			//Any transformation related changes go here. these typically cost 1 change. these can be anything from body parts to gender (which technically also changes body parts,
			//but w/e). You are required to make sure you return as soon as you've applied changeCount changes, but a single line of code can be applied at the end of a change to do
			//this for you.

			//paste this line after any tf is applied, and it will: automatically decrement the remaining changes count. if it becomes 0 or less, apply the total number of changes
			//underwent to the target's change count (if applicable) and then return the StringBuilder content.
			//if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			CombatCreature combatCreature = target as CombatCreature;

			//will be false if combat creature is null.
			if (Utils.Rand(2) == 0 && combatCreature?.relativeSpeed < 80)
			{
				combatCreature.DeltaCombatCreatureStats( spe: 1);
			}
			if (target.lowerBody.type != LowerBodyType.CENTAUR && !target.lowerBody.isQuadruped)
			{
				//MOD NOTE: Has hooves, but not 4 legs.
				//classic horse-taur version
				if (Utils.RandBool() && target.feet.type == FootType.HOOVES && !target.lowerBody.isQuadruped)
				{
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

					combatCreature?.DeltaCombatCreatureStats(spe: 3);

					target.UpdateLowerBody(LowerBodyType.CENTAUR);
				}
				else if (Utils.Rand(3) == 0)
				{
					//generic version
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					if (target.lowerBody.type == LowerBodyType.NAGA)
					{
						target.UpdateLowerBody(LowerBodyType.HOOVED);
					}
					//Catch-all
					else
					{
						if (target.lowerBody.type == LowerBodyType.HUMAN)
							target.UpdateLowerBody(LowerBodyType.HOOVED);
					}

					combatCreature?.DeltaCombatCreatureStats(spe: 3);
				}

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
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