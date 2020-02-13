//BeeTransformations.cs
//Description:
//Author: JustSomeGuy
//1/27/2020 9:44:24 PM
using System;
using System.Linq;
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
	internal enum BeeModifiers { PURE, SPECIAL, NORMAL }

	internal abstract class BeeTransformations : GenericTransformationBase
	{
		protected readonly BeeModifiers modifier;

		protected bool isPure => modifier == BeeModifiers.PURE;
		protected bool isSpecial => modifier == BeeModifiers.SPECIAL;

		protected BeeTransformations(BeeModifiers beeModifier)
		{
			modifier = beeModifier;
		}

		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 2 });
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

			//Corruption reduction
			if (isPure)
			{ //Special honey will also reduce corruption, but uses different text and is handled separately

				target.DeltaCreatureStats(corr: -(1 + (target.corruptionTrue / 20)));
				//Libido Reduction
				if (target.corruption > 0 && Utils.Rand(3) < 2 && target.relativeLibido > 40)
				{
					target.DeltaCreatureStats(lib: -3, lus: -20);
				}

			}
			if (target is CombatCreature cc)
			{
				//Intelligence Boost
				if (Utils.Rand(2) == 0 && cc.relativeIntelligence < 80)
				{
					cc.IncreaseIntelligence(0.1f * (80 - cc.relativeIntelligence));
				}
			}
			//bee item corollary:
			if (target.hair.type == HairType.ANEMONE && Utils.Rand(2) == 0)
			{
				target.RestoreHair();
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Sexual Stuff
			//No idears
			//Appearance Stuff
			//Hair Color
			if (target.hair.hairColor != HairFurColors.BLACK && target.hair.length > 10 && Utils.Rand(5) == 0)
			{
				if (Utils.Rand(9) == 0)
				{
					target.hair.SetBothHairColors(HairFurColors.BLACK, HairFurColors.YELLOW);
				}
				else
				{
					target.hair.SetHairColor(HairFurColors.BLACK);
				}

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Hair Length
			if (target.hair.length < 25 && target.hair.type.canLengthen && Utils.Rand(3) == 0)
			{
				target.hair.GrowHair(Utils.Rand(4) + 1);
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//-Remove extra breast rows
			if (target.breasts.Count > 2 && Utils.Rand(3) == 0 && !hyperHappy)
			{
				target.genitals.RemoveBreastRows();
			}
			//Antennae
			if (target.antennae.type == AntennaeType.NONE && target.horns.numHorns == 0 && Utils.Rand(3) == 0)
			{
				target.UpdateAntennae(AntennaeType.BEE);
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Horns
			if (!target.horns.isDefault && Utils.Rand(3) == 0)
			{
				target.RestoreHorns();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Bee Legs
			if (target.lowerBody.type != LowerBodyType.BEE && Utils.Rand(4) == 0)
			{
				target.UpdateLowerBody(LowerBodyType.BEE);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//(Arms to carapace-covered arms)
			if (target.arms.type != ArmType.BEE && Utils.Rand(4) == 0)
			{
				target.UpdateArms(ArmType.BEE);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//-Nipples reduction to 1 per tit.
			if (target.genitals.hasQuadNipples && Utils.Rand(4) == 0)
			{
				target.genitals.SetQuadNipples(false);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Neck restore
			if (!target.neck.isDefault && Utils.Rand(4) == 0)
			{
				target.RestoreNeck();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Rear body restore
			if (!target.back.isDefault && Utils.Rand(5) == 0)
			{
				target.RestoreBack();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Lose reptile oviposition!
			if (target.womb.canRemoveOviposition && Utils.Rand(5) == 0)
			{
				target.womb.ClearOviposition();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Gain bee ovipositor!
			if (target.tail.type == TailType.BEE_STINGER && !target.tail.hasOvipositor && Utils.Rand(2) == 0)
			{
				target.tail.GrantOvipositor();
			}

			//Bee butt - 66% lower chance if already has a tail
			if (target.tail.type != TailType.BEE_STINGER && (target.tail.type == TailType.NONE || Utils.Rand(3) < 2) && Utils.Rand(4) == 0)
			{
				target.UpdateTail(TailType.BEE_STINGER);
				target.tail.UpdateResources((short)(10 - target.tail.resources), (short)(2 - target.tail.regenRate));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Venom Increase
			if (target.tail.type == TailType.BEE_STINGER && target.tail.regenRate < 15 && Utils.Rand(2) == 0)
			{
				short additionalRegen = 1;
				if (target.tail.regenRate < 5)
				{
					additionalRegen = 3;
				}
				else if (target.tail.regenRate < 10)
				{
					additionalRegen = 2;
				}

				target.tail.UpdateResources(50, additionalRegen);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Wings
			//Grow bigger bee wings!
			if (target.wings.type == WingType.BEE_LIKE && !target.wings.isLarge && Utils.Rand(4) == 0)
			{
				target.wings.GrowLarge();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Grow new bee wings if target has none.
			if (target.wings.type == WingType.NONE && Utils.Rand(4) == 0)
			{
				if (target.back.type == BackType.SHARK_FIN)
				{
					target.RestoreBack();
				}
				target.UpdateWings(WingType.BEE_LIKE);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Melt demon wings!
			if (target.wings.type == WingType.BAT_LIKE)
			{
				target.RestoreWings();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Remove gills!
			if (Utils.Rand(4) == 0 && !target.gills.isDefault)
			{
				target.RestoreGills();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//All the speical honey effects occur after any normal bee transformations (if the target wasn't a full bee morph)
			if (isSpecial)
			{

				//if no cock: grow one.
				if (!target.hasCock)
				{
					target.genitals.AddCock(CockType.defaultValue, Utils.Rand(3) + 8, 2);
					target.HaveGenericCockOrgasm(0, false, true);
					target.DeltaCreatureStats(sens: 10);
				}
				//if multiple cocks, remove the largest cock by area. combine its length/girth into the first cock.
				else if (target.cocks.Count > 1)
				{
					//find the biggest cock that isn't the first one.
					Cock biggest = target.cocks.Skip(1).MaxItem(x => x.area);
					float delta = (float)(5 * Math.Sqrt(0.2 * biggest.area));

					target.cocks[0].DeltaLengthAndGirth(delta, delta);
					target.HaveGenericCockOrgasm(0, false, true);
				}
				//one cock. grow it to 100 area total or larger
				else if (target.cocks[0].area < 100)
				{
					target.cocks[0].DeltaLengthAndGirth(Utils.Rand(3) + 4, 0.1f * Utils.Rand(5) + 0.5f);
				}
				//
				else
				{
					float baseLengthChange;
					float baseGirthChange;
					if (target.cocks[0].type != CockType.BEE && Species.CurrentSpecies(target) == Species.BEE)
					{
						target.genitals.UpdateCock(0, CockType.BEE);
						target.DeltaCreatureStats(sens: 15);

						baseLengthChange = 5;
						baseGirthChange = 1;
					}
					else
					{
						baseLengthChange = 0.1f * Utils.Rand(10) + 1;
						baseGirthChange = 0.1f * Utils.Rand(2) + 1;
					}

					double mult;
					var cock = target.cocks[0];
					if (cock.area >= 400)
					{
						mult = 0; //Cock stops growing at that point.
					}
					else if (cock.area >= 300)
					{
						mult = 0.1;
					}
					if (cock.area > 100)
					{
						int offset = (((int)cock.area) - 100) / 40;
						mult = 1 - 0.2f * offset;
					}
					else
					{
						mult = 1;
					}

					float deltaLength = (float)(mult * baseLengthChange);
					float deltaGirth = (float)(mult * baseGirthChange);

					target.cocks[0].DeltaLengthAndGirth(deltaLength, deltaGirth);
				}


				if (target.corruption >= 5)
				{
					float corrLoss = Math.Min(0.1f * target.corruptionTrue + 5, target.corruptionTrue);
					target.DeltaCreatureStats(corr: -corrLoss, lib: corrLoss); //Lose corruption and gains that much libido
				}
				else
				{
					target.DeltaCreatureStats(lib: 5);
				}

				if (target.femininity >= 60 || target.femininity <= 40)
				{
					if (target.femininity >= 60)
					{
						target.femininity.IncreaseMasculinity(3);
					}
					else
					{
						target.femininity.IncreaseFemininity(3);
					}
				}
				target.DeltaCreatureStats(lus: 0.2f * target.libidoTrue + 5);
			}


			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target);
	}
}