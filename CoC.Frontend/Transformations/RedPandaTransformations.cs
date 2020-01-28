//RedPandaTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:51:48 PM
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
	internal abstract class RedPandaTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 2, 4 });
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

#warning fix me
			int ngPlus(int value) => value;


			if (target is CombatCreature cc && cc.speed < ngPlus(100) && Utils.Rand(3) == 0)
			{
				//+3 spe if less than 50
				if (cc.speed < ngPlus(50))
				{
					cc.IncreaseSpeed(3);
				}
				//+2 spe if less than 75
				else if (cc.speed < ngPlus(75))
				{
					cc.IncreaseSpeed(2);
				}
				//+1 if above 75.
				else
				{
					cc.IncreaseSpeed();
				}
			}

			// ------------- Sexual changes -------------
			//-Nipples reduction to 1 per tit.
			if (target.genitals.hasQuadNipples && Utils.Rand(4) == 0)
			{
				target.genitals.SetQuadNipples(false);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//-Remove extra breast rows
			if (target.breasts.Count > 1 && Utils.Rand(3) == 0 && !hyperHappy)
			{
				target.RemoveExtraBreastRows();
			}

			//-Butt > 5 - decrease butt size
			if (target.butt.size > 5 && Utils.Rand(4) == 0)
			{
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				target.butt.ShrinkButt();
			}

			if (target.gender.HasFlag(Gender.FEMALE))
			{
				//Breasts > D cup - Decrease breast size by up to 3 cups
				//Breasts < B cup - Increase breast size by 1 cup
				if (target.breasts.Any(x => x.cupSize > CupSize.D || x.cupSize < CupSize.B) && Utils.Rand(3) == 0)
				{
					foreach (Breasts breast in target.breasts)
					{
						if (breast.cupSize > CupSize.D)
						{
							breast.ShrinkBreasts((byte)(1 + Utils.Rand(3)));
						}
						else if (breast.cupSize < CupSize.B)
						{
							breast.GrowBreasts();
						}
					}

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

				//Hips > 12 - decrease hip size by 1-3 sizes
				if (target.hips.size > 12 && Utils.Rand(3) == 0)
				{
					target.hips.ShrinkHips((byte)(1 + Utils.Rand(3)));

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

				//Hips < 6 - increase hip size by 1-3 sizes
				if (target.hips.size < 6 && Utils.Rand(3) == 0)
				{
					target.hips.GrowHips((byte)(1 + Utils.Rand(3)));
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

				if (target.genitals.nippleLength > 1 && Utils.Rand(3) == 0)
				{
					target.genitals.SetNippleLength(target.genitals.nippleLength / 2f);
				}

				if (target.hasVagina && target.vaginas[0].wetness < VaginalWetness.SLICK && Utils.Rand(4) == 0)
				{
					target.vaginas[0].IncreaseWetness();
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

				//Increase tone (up to 65)
				if (target.build.muscleTone < 65 && Utils.Rand(3) == 0)
				{
				}

				//Decrease thickness (down to 35)
				if (target.build.thickness > 35 && Utils.Rand(3) == 0)
				{
				}
			}

			if (target.gender == Gender.MALE)
			{
				//Breasts > B cup (or applicable male max size if it's somehow > B cup) - decrease by 1 cup size
				CupSize targetSize = EnumHelper.Max(target.genitals.smallestPossibleMaleCupSize, CupSize.B);
				if (target.genitals.BiggestCupSize() > targetSize && Utils.Rand(3) == 0)
				{
					foreach (Breasts breast in target.breasts)
					{
						if (breast.cupSize > targetSize)
						{
							breast.ShrinkBreasts();
						}
					}

					if (target is CombatCreature cc2)
					{
						cc2.IncreaseSpeed();
					}

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

				if (target.genitals.nippleLength > 1 && Utils.Rand(3) == 0)
				{
					target.genitals.SetNippleLength(target.genitals.nippleLength / 2f);
				}

				//Hips > 10 - decrease hip size by 1-3 sizes
				if (target.hips.size > 10 && Utils.Rand(3) == 0)
				{
					target.hips.ShrinkHips((byte)(1 + Utils.Rand(3)));
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

				//Hips < 2 - increase hip size by 1-3 sizes
				if (target.hips.size < 2 && Utils.Rand(3) == 0)
				{
					target.hips.GrowHips((byte)(1 + Utils.Rand(3)));
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

				//Increase tone (up to 70)
				if (target.build.muscleTone < 70 && Utils.Rand(3) == 0)
				{
				}

				//Decrease thickness (down to 35)
				if (target.build.thickness > 35 && Utils.Rand(3) == 0)
				{
				}
			}

			if (target.gender.HasFlag(Gender.MALE))
			{
				//Cock -> Red Panda Cock
				if (target.hasCock && target.cocks[0].type != CockType.RED_PANDA && Utils.Rand(3) == 0)
				{
					target.genitals.UpdateCock(0, CockType.RED_PANDA);
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

				Cock shortest = target.genitals.ShortestCock();

				//Cock < 6 inches - increase by 1-2 inches
				if (shortest.length < 6 && Utils.Rand(3) == 0)
				{
					float increment = shortest.IncreaseLength(1 + Utils.Rand(2));
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

				Cock longest = target.genitals.LongestCock();

				//Shrink oversized cocks
				if (longest.length > 16 && Utils.Rand(3) == 0)
				{
					longest.DecreaseLength((Utils.Rand(10) + 5) / 10);
					if (longest.girth > 3)
					{
						longest.DecreaseThickness((Utils.Rand(4) + 1) / 10);
					}
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

				Cock smallestArea = target.genitals.SmallestCockByArea();

				//Cock thickness <2 - Increase cock thickness
				if (smallestArea.area < 10 && Utils.Rand(3) == 0)
				{
					smallestArea.IncreaseThickness(1.5f);
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}

			//Remove additional cocks
			if (target.cocks.Count > 1 && Utils.Rand(3) == 0)
			{
				//what a dick. removes the second, and only the second. not the last one or anything. ok. it's supported now.
				target.genitals.RemoveCockAt(1, 1);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Remove additional balls/remove uniball
			if (target.balls.count > 0 && Utils.Rand(3) == 0)
			{
				if (target.balls.size > 5)
				{
					target.balls.ShrinkBalls((byte)(2 + Utils.Rand(3)));

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
				else if (target.balls.size > 2)
				{
					target.balls.ShrinkBalls(1);

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
				else if (target.balls.count != 2)
				{
					//removes uniball, sets count to 2.
					target.balls.MakeStandard();

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}

			//Ovi perk loss
			if (target.womb is PlayerWomb playerWomb && playerWomb.canClearOviposition && Utils.Rand(5) == 0)
			{
				playerWomb.ClearOviposition();
				//if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			// ------------- Physical changes -------------
			// Ears
			if (target.ears.type != EarType.RED_PANDA && Utils.Rand(3) == 0)
			{
				target.UpdateEars(EarType.RED_PANDA);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			// Remove non-cockatrice antennae
			if (target.antennae.type != AntennaeType.COCKATRICE && !target.antennae.isDefault && Utils.Rand(3) == 0)
			{
				target.RestoreAntennae();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			// Restore eyes, if more than two
			if (target.eyes.count > 2 && Utils.Rand(4) == 0)
			{
				target.RestoreEyes();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			// Hair
			// store current states first
			bool hasPandaHairColor = Species.RED_PANDA.availableHairColors.Contains(target.hair.hairColor);
			bool hasNormalHair = target.hair.type == HairType.NORMAL;
			HairType oldHairType = target.hair.type;

			if ((!hasNormalHair || target.hair.length == 0 || !hasPandaHairColor) && Utils.Rand(3) == 0)
			{
				target.UpdateHair(HairType.NORMAL);
				if (!hasPandaHairColor)
				{
					target.hair.SetHairColor(Utils.RandomChoice(Species.RED_PANDA.availableHairColors));
				}

				if (target.hair.length == 0)
				{ // target is bald
					target.hair.SetHairLength(1);
				}

				target.hair.SetHairGrowthStatus(true);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			// Face
			if (target.face.type != FaceType.RED_PANDA && target.ears.type == EarType.RED_PANDA && target.body.IsFurBodyType() && Utils.Rand(3) == 0)
			{
				target.UpdateFace(FaceType.RED_PANDA);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			// Arms
			if (target.arms.type != ArmType.RED_PANDA && target.ears.type == EarType.RED_PANDA && target.tail.type == TailType.RED_PANDA && Utils.Rand(3) == 0)
			{
				target.UpdateArms(ArmType.RED_PANDA);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			// Legs
			if (target.lowerBody.type != LowerBodyType.RED_PANDA && target.arms.type == ArmType.RED_PANDA && Utils.Rand(4) == 0)
			{
				target.UpdateLowerBody(LowerBodyType.RED_PANDA);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			// Tail
			if (target.tail.type != TailType.RED_PANDA && Utils.Rand(4) == 0)
			{
				target.UpdateTail(TailType.RED_PANDA);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			// SKin


			// Fix the underBody, if the skin is already furred
			if (target.body.type == BodyType.SIMPLE_FUR && Utils.Rand(3) == 0)
			{
				target.UpdateBody(BodyType.UNDERBODY_FUR, new FurColor(HairFurColors.RUSSET), new FurColor(HairFurColors.BLACK));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			else if (!target.body.IsFurBodyType() && target.arms.type == ArmType.RED_PANDA && target.lowerBody.type == LowerBodyType.RED_PANDA && Utils.Rand(4) == 0)
			{
				target.UpdateBody(BodyType.UNDERBODY_FUR, new FurColor(HairFurColors.RUSSET), new FurColor(HairFurColors.BLACK));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//FAILSAFE CHANGE
			if (remainingChanges == changeCount)
			{
				if (Utils.Rand(100) == 0)
				{
				}
				else
				{
					if (target is CombatCreature)
					{
						((CombatCreature)target).AddHP(250);
					}

					target.DeltaCreatureStats(lus: 3);
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