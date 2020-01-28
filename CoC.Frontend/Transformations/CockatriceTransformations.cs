﻿//CockatriceTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:42:14 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;
using System.Text;
using System.Linq;
using CoC.Frontend.Perks;
using CoC.Backend.CoC_Colors;

namespace CoC.Frontend.Transformations
{
	internal abstract class CockatriceTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 2, 3, 4 });
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
#warning fix me
			int ngPlus(int value) => value;

			if (target is CombatCreature cc)
			{
				if (cc.speed < ngPlus(100) && Utils.Rand(3) == 0)
				{
					//+3 spe if less than 50
					if (cc.speed < ngPlus(50)) cc.DeltaCombatCreatureStats(spe: 1);
					//+2 spe if less than 75
					if (cc.speed < ngPlus(75)) cc.DeltaCombatCreatureStats(spe: 1);
					//+1 if above 75.
					cc.DeltaCombatCreatureStats(spe: 1);
				}

				if (cc.toughness > ngPlus(80) && Utils.Rand(4) == 0)
				{
					cc.DeltaCombatCreatureStats(tou: -1);

				}
			}
			//-Reduces sensitivity.
			if (target.sensitivity > 20 && Utils.Rand(3) == 0)
			{
				target.DeltaCreatureStats(sens: -1);
			}

			//Raises libido greatly to 50, then somewhat to 75, then slowly to 100.
			if (target.libido < 100 && Utils.Rand(3) == 0)
			{
				//+3 lib if less than 50
				if (target.libido < 50) target.DeltaCreatureStats(lib: 1);
				//+2 lib if less than 75
				if (target.libido < 75) target.DeltaCreatureStats(lib: 1);
				//+1 if above 75.
				target.DeltaCreatureStats(lib: 1);
			}

			//Sexual changes

			//-Lactation stoppage.
			if (target.genitals.isLactating && Utils.Rand(4) == 0)
			{
				if (target.HasPerk<Feeder>())
				{
					target.RemovePerk<Feeder>();
				}

				target.genitals.SetLactationTo(LactationStatus.NOT_LACTATING);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//-Nipples reduction to 1 per tit.
			if (target.genitals.hasQuadNipples && Utils.Rand(4) == 0)
			{
				target.genitals.SetQuadNipples(false);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//-Remove extra breast rows
			if (target.breasts.Count > 1 && Utils.Rand(3) == 0 && !hyperHappy)
			{
				target.RemoveExtraBreastRows();
			}

			//-Butt > 5 - decrease butt size
			if (target.butt.size > 5 && Utils.Rand(4) == 0)
			{
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				target.butt.ShrinkButt();
			}

			if (target.gender.HasFlag(Gender.FEMALE))
			{
				var minCup = EnumHelper.Max(CupSize.D, target.genitals.smallestPossibleFemaleCupSize);
				//Breasts > D cup - Decrease breast size by up to 3 cups
				//MOD NOTE: Now respects minimum cup size from perks.
				if (target.gender.HasFlag(Gender.FEMALE) && target.genitals.BiggestCupSize() > minCup && Utils.Rand(3) == 0)
				{

					foreach (Breasts breast in target.breasts)
					{
						if (breast.cupSize > CupSize.D)
						{
							breast.ShrinkBreasts((byte)(1 + Utils.Rand(3)));
						}
					}

					if (target is CombatCreature smallerBustMakesYouFaster)
					{
						smallerBustMakesYouFaster.IncreaseSpeed();
					}

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				//Breasts < B cup - Increase breast size by 1 cup
				if (target.gender.HasFlag(Gender.FEMALE) && target.genitals.SmallestCupSize() < CupSize.B && Utils.Rand(3) == 0)
				{
					for (int i =0; i < target.breasts.Count; i++)
					{
						if (target.breasts[i].cupSize < CupSize.B)
						{
							target.breasts[i].GrowBreasts();
						}
					}
					target.DeltaCreatureStats(lib: 1);
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				//Hips > 12 - decrease hip size by 1-3 sizes
				if (target.hips.size > 12 && Utils.Rand(3) == 0)
				{
					target.hips.ShrinkHips((byte)(1 + Utils.Rand(3)));
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				//Hips < 6 - increase hip size by 1-3 sizes
				if (target.hips.size < 6 && Utils.Rand(3) == 0)
				{
					target.hips.GrowHips((byte)(1 + Utils.Rand(3)));
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				if (target.genitals.nippleLength > 1 && Utils.Rand(3) == 0)
				{
					target.genitals.SetNippleLength(target.genitals.nippleLength / 2f);
				}

				var desiredWetness = EnumHelper.Min(target.genitals.maxVaginalWetness, VaginalWetness.SLICK);
				//MOD NOTE: now respects all vaginas, and the maximum wetness allowed by perks (if applicable)
				if (target.hasVagina && target.genitals.SmallestVaginalWetness() < desiredWetness && Utils.Rand(4) == 0)
				{
					foreach (var vag in target.vaginas)
					{
						if (vag.wetness < desiredWetness)
						{
							vag.IncreaseWetness();
						}
					}
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				//Increase tone (up to 65)
				if (target.build.muscleTone < 65 && Utils.Rand(3) == 0)
				{
					target.build.ChangeMuscleToneToward(65, 2);
				}

				//Decrease thickness (down to 35)
				if (target.build.thickness > 35 && Utils.Rand(3) == 0)
				{
					target.build.ChangeThicknessToward(35, 5);
				}
				//Grant oviposition.
				if (target.womb is PlayerWomb playerWomb && !playerWomb.hasOviposition && Species.COCKATRICE.Score(target) > 3 && Utils.Rand(5) == 0)
				{
					playerWomb.GrantOviposition();

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			if (target.gender == Gender.MALE)
			{
				//Breasts > B cup - decrease by 1 cup size
				if (target.genitals.BiggestCupSize() > CupSize.B && Utils.Rand(3) == 0)
				{
					for (int i =0; i < target.breasts.Count; i++)
					{
						if (target.breasts[i].cupSize > CupSize.B)
							target.breasts[i].ShrinkBreasts();
					}

					if (target is CombatCreature smallerBustMakesYouFaster)
					{
						smallerBustMakesYouFaster.IncreaseSpeed();
					}

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				if (target.genitals.nippleLength > 1 && Utils.Rand(3) == 0)
				{
					target.genitals.SetNippleLength(target.genitals.nippleLength / 2f);
				}

				//Hips > 10 - decrease hip size by 1-3 sizes
				if (target.hips.size > 10 && Utils.Rand(3) == 0)
				{
					target.hips.ShrinkHips((byte)(1 + Utils.Rand(3)));
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				//Hips < 2 - increase hip size by 1-3 sizes
				if (target.hips.size < 2 && Utils.Rand(3) == 0)
				{
					target.hips.GrowHips((byte)(1 + Utils.Rand(3)));
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				//Increase tone (up to 70)
				if (target.build.muscleTone < 70 && Utils.Rand(3) == 0)
				{
					target.build.ChangeMuscleToneToward(70, 2);
				}

				//Decrease thickness (down to 35)
				if (target.build.thickness > 35 && Utils.Rand(3) == 0)
				{
					target.build.ChangeThicknessToward(35, 5);
				}
			}

			if (target.gender.HasFlag(Gender.MALE))
			{
				//Cock < 6 inches - increase by 1-2 inches
				var shortest = target.genitals.ShortestCock();
				if (shortest.length < 6 && Utils.Rand(3) == 0)
				{
					float increment = shortest.IncreaseLength(1 + Utils.Rand(2));

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				var longest = target.genitals.LongestCock();
				//Shrink oversized cocks
				if (longest.length > 16 && Utils.Rand(3) == 0)
				{
					longest.DecreaseLength((Utils.Rand(10) + 5) / 10f);
					if (longest.girth > 3)
					{
						longest.DecreaseThickness((Utils.Rand(4) + 1) / 10f);
					}
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

				var thinnest = target.genitals.ThinnestCock();
				//Cock thickness <2 - Increase cock thickness
				if (thinnest.girth < 2 && Utils.Rand(3) == 0)
				{
					thinnest.IncreaseThickness(1.5f);

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			var lizardCocks = target.genitals.CountCocksOfType(CockType.LIZARD);

			if (target.hasCock && target.cocks.Count > lizardCocks && Utils.Rand(4) == 0)
			{
				//-Lizard dick - first one
				if (lizardCocks == 0)
				{
					//Actually xform it nau
					if (target.genitals.hasSheath)
					{
						target.genitals.UpdateCock(0, CockType.LIZARD);
					}
					else target.genitals.UpdateCock(0, CockType.LIZARD);
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					target.DeltaCreatureStats(lib: 3, lus: 10);
				}
				//(CHANGE OTHER DICK)
				//Requires 1 lizard cock, multiple cocks
				else //if (target.cocks.Count > 1 && target.cocks.Count > lizardCocks)
				{
					var firstNonLizard = target.cocks.First(x => x.type != CockType.LIZARD);

					target.genitals.UpdateCock(firstNonLizard, CockType.LIZARD);

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					target.DeltaCreatureStats(lib: 3, lus: 10);
				}
			}

			//MOD NOTE: Worms are being removed??? from the game due to that absolutely shitty license that prevents me from porting any content 'created' by
			//its creator (forget the name at this time). it's a bad license because it takes credit from other content creators that use its content. that's like saying
			//the creator of the rubber tire gets credit for all modern car designs. that's bullshit. of course cars require tires, and the original copyright required users
			//to pay a licensing fee for their use. that didn't grant the copyright holder for rubber tires the credit for the cars that used them. If anyone wants to get ahold
			//of the og worms creator and get him/her to relax that requirement to make it reasonable (and future content-change/port friendly), we'll see. until then, this is
			//removed.

			//For the record, that would grant him creator's credit on this document. which he/she had no input on. that's not right. and, by extension, the inventory system, since
			//this item is in use in the inventory system. and the time engine, because worms regen over time. I am the content creator for both of those, and i refuse to allow him/her
			//any credit for either of those. those took time and effort and were difficult - i'm not freely handing credit to him/her because that license would require me to. Either
			//his/her license changes, or the offending content is removed, or the entire game engine goes. Right now i think it's better go with the game engine because it make all of
			//this shit work. - JSG.

			////--Worms leave if 100% lizard dicks?
			////Require mammals?
			//if (target.genitals.CountCocksOfType(CockType.LIZARD) == target.cocks.Count && target.hasStatusEffect(StatusEffects.Infested))
			//{
			//	if (target.balls.count > 1)
			//	target.removeStatusEffect(StatusEffects.Infested);
			//	if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			//}

			//Increase height up to 5ft 7in.
			if (target.build.heightInInches < 67 && Utils.Rand(5) == 0)
			{
				target.build.IncreaseHeight((byte)(Utils.Rand(3) + 1));
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Decrease height down to a maximum of 6ft 8in.
			if (target.build.heightInInches > 80 && Utils.Rand(5) == 0)
			{
				target.build.DecreaseHeight((byte)(Utils.Rand(3) + 1));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Physical changes:
			//Removes other antennae
			if (target.antennae.type != AntennaeType.COCKATRICE && !target.antennae.isDefault && Utils.Rand(3) == 0)
			{
				target.RestoreAntennae();
			}
			//Gain antennae like feathers
			if (target.antennae.type == AntennaeType.NONE && target.face.type == FaceType.COCKATRICE && target.ears.type == EarType.COCKATRICE && Utils.Rand(3) == 0)
			{
				// Other antennae types are handled above! (Stadler76)
				target.UpdateAntennae(AntennaeType.COCKATRICE);
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Removes horns
			if (target.horns.type != HornType.NONE && Utils.Rand(5) == 0)
			{
				target.RestoreHorns();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Face TF
			if (target.face.type != FaceType.COCKATRICE && target.arms.type == ArmType.COCKATRICE && target.lowerBody.type == LowerBodyType.COCKATRICE && Utils.Rand(3) == 0)
			{
				target.UpdateFace(FaceType.COCKATRICE);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Hair TF
			if (target.hair.type != HairType.FEATHER && Utils.Rand(4) == 0)
			{
				target.UpdateHair(HairType.FEATHER);
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Eye TF
			if (target.eyes.type != EyeType.COCKATRICE && target.face.type == FaceType.COCKATRICE && target.body.type == BodyType.COCKATRICE && target.ears.type == EarType.COCKATRICE
				&& Utils.Rand(3) == 0)
			{
				target.UpdateEyes(EyeType.COCKATRICE);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Lizard tongue TF
			if (target.tongue.type != TongueType.LIZARD && target.face.type == FaceType.COCKATRICE && Utils.Rand(3) == 0)
			{
				target.UpdateTongue(TongueType.LIZARD);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Ears TF
			if (target.ears.type != EarType.COCKATRICE && target.face.type == FaceType.COCKATRICE && Utils.Rand(3) == 0)
			{
				target.UpdateEars(EarType.COCKATRICE);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Arm TF
			if (target.arms.type != ArmType.COCKATRICE && Utils.Rand(4) == 0)
			{
				target.UpdateArms(ArmType.COCKATRICE);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Neck loss, if not cockatrice neck
			if (target.neck.type != NeckType.COCKATRICE && Utils.Rand(4) == 0)
			{
				target.RestoreNeck();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Rear body restore
			if (target.back.type != BackType.NORMAL && Utils.Rand(5) == 0)
			{
				target.RestoreBack();
			}
			//Body TF
			if (target.body.type != BodyType.COCKATRICE && target.face.type == FaceType.COCKATRICE && Utils.Rand(3) == 0)
			{
				Species.COCKATRICE.GetRandomCockatriceColors(out FurColor feathers, out Tones scales);

				target.UpdateBody(BodyType.COCKATRICE, feathers, scales);
				target.UpdateNeck(NeckType.COCKATRICE, feathers.primaryColor);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Neck TF, if not already TFed from Body TF above
			if (target.neck.type != NeckType.COCKATRICE && target.body.type == BodyType.COCKATRICE && target.face.type == FaceType.COCKATRICE && Utils.Rand(3) == 0)
			{
				target.UpdateNeck(NeckType.COCKATRICE, Utils.RandomChoice(Species.COCKATRICE.availablePrimaryFeatherColors));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Leg TF
			if (target.lowerBody.type != LowerBodyType.COCKATRICE && Utils.Rand(4) == 0)
			{
				target.UpdateLowerBody(LowerBodyType.COCKATRICE);
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Tail TF
			if (target.tail.type != TailType.COCKATRICE && Utils.Rand(4) == 0)
			{

				target.UpdateTail(TailType.COCKATRICE);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Wings TF
			//feathered wings and not large and a player? that shouldn't happen. silently make them large
			if (target.wings.type == WingType.FEATHERED && !target.wings.isLarge && target is Player)
			{
				target.wings.GrowLarge();
			}
			else if (target.wings.type != WingType.FEATHERED && target.arms.type == ArmType.COCKATRICE && Utils.Rand(4) == 0)
			{
				HairFurColors wingColor = !target.body.activeFur.isEmpty ? target.body.activeFur.fur.primaryColor : target.hair.hairColor;

				target.UpdateWings(WingType.FEATHERED);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//FAILSAFE CHANGE
			if (remainingChanges == changeCount)
			{
				if (target is CombatCreature failSafe)
				{
					failSafe.AddHP(50);
				}
				target.DeltaCreatureStats(lus: 3);
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