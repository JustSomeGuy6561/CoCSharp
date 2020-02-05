//MouseTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:53:51 PM
using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;
using CoC.Frontend.StatusEffect;

namespace CoC.Frontend.Transformations
{
	internal abstract class MouseTransformations : GenericTransformationBase
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

			//use:

			//stat changes:
			//lose height + gain speed (42" height floor, no speed ceiling but no speed changes without height change)
			if (target.build.heightInInches >= 45 && Utils.Rand(3) == 0)
			{
				(target as CombatCreature)?.IncreaseSpeed();

				target.build.DecreaseHeight();
				if (target.build.heightInInches > 60)
				{
					target.build.DecreaseHeight();
				}

				if (target.build.heightInInches > 70)
				{
					target.build.DecreaseHeight();
				}

				if (target.build.heightInInches > 80)
				{
					target.build.DecreaseHeight();
				}

				if (target.build.heightInInches > 90)
				{
					target.build.DecreaseHeight(2);
				}

				if (target.build.heightInInches > 100)
				{
					target.build.DecreaseHeight(2);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (target is CombatCreature cc)
			{
				//lose tough
				if (cc.relativeToughness > 50 && Utils.Rand(3) == 0)
				{
					cc.DeltaCombatCreatureStats(tou: -1);
					if (cc.relativeToughness >= 75)
					{
						cc.DeltaCombatCreatureStats(tou: -1);
					}

					if (cc.relativeToughness >= 90)
					{
						cc.DeltaCombatCreatureStats(tou: -1);
					}
				}
			}

			//SEXYYYYYYYYYYY
			//vag-anal capacity up for non-goo (available after PC < 5 ft; capacity ceiling reasonable but not horse-like or gooey)
			if (target.build.heightInInches < 60 && (target.genitals.standardBonusAnalCapacity < 100 || (target.genitals.standardBonusVaginalCapacity < 100 && target.hasVagina)) && Utils.Rand(3) == 0)
			{

				//adds some lust
				target.DeltaCreatureStats(lus: 10 + target.sensitivity / 5);
				if (target.genitals.standardBonusVaginalCapacity < 100 && target.hasVagina)
				{
					target.genitals.IncreaseBonusVaginalCapacity(5);
				}
				else
				{
					target.genitals.IncreaseBonusAnalCapacity(5);
				}
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//fem fertility up and heat (suppress if pregnant)
			//not already in heat (add heat and lust)
			if (!target.HasTimedEffect<Heat>() || target.GetTimedEffectData<Heat>().totalAddedLibido < 30 && Utils.Rand(2) == 0)
			{

				bool intensified = target.HasTimedEffect<Heat>();
				if (target.GoIntoHeat())
				{
					if (intensified)
					{
						////[(no mino cum in inventory)]
						//if (!target.hasItem(consumables.MINOCUM))
						//{
						//}
						////(mino cum in inventory and non-horse, 100 lust)
						//else
						//{
						//	//(consumes item, increment addiction/output addict message, small chance of mino preg, reduce lust)]");
						//	target.minoCumAddiction(5);
						//	target.knockUp(PregnancyStore.PREGNANCY_MINOTAUR, PregnancyStore.INCUBATION_MINOTAUR, 175);
						//	ConsumableLib consumables = game.consumables;
						//	target.consumeItem(consumables.MINOCUM);
						//}
#warning Not Implemented: consume minotaur cum, or if you don't have it, go fuck a minotaur to get it, and immediately consume it. do whatever sex is required for getting it.
					}
					else
					{

						// Also make a permanent nudge.
						target.fertility.IncreaseFertility();
					}
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
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

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
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
			//bodypart changes:
			//gain ears
			if (target.ears.type != EarType.MOUSE && Utils.Rand(4) == 0)
			{
				target.UpdateEars(EarType.MOUSE);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//gain tail
			//from no tail
			if (target.ears.type == EarType.MOUSE && target.tail.type != TailType.MOUSE && Utils.Rand(4) == 0)
			{
				//from other tail
				target.UpdateTail(TailType.MOUSE);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//get teeth - from human, bunny, coonmask, or other humanoid teeth faces
			if (target.ears.type == EarType.MOUSE && target.face.isHumanoid && target.face.type != FaceType.MOUSE && Utils.Rand(4) == 0)
			{
				target.UpdateFace(FaceType.MOUSE);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//get mouse muzzle from mouse teeth or other muzzle
			if (target.body.IsFurBodyType() && target.face.hasMuzzle && Utils.Rand(4) == 0)
			{
				target.UpdateFace(FaceType.MOUSE);
				target.face.StrengthenFacialMorph();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//get fur
			if (!target.body.IsFurBodyType() || (!target.body.mainEpidermis.fur.IsIdenticalTo(HairFurColors.BROWN)
				&& !target.body.mainEpidermis.fur.IsIdenticalTo(HairFurColors.WHITE)) && Utils.Rand(4) == 0)
			{
				FurColor color;
				int temp = Utils.Rand(10);
				if (temp < 8)
				{
					color = new FurColor(HairFurColors.BROWN);
				}
				else
				{
					color = new FurColor(HairFurColors.WHITE);
				}
				target.UpdateBody(BodyType.SIMPLE_FUR, color);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
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