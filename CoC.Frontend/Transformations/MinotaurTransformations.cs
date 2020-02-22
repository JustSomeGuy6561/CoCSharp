//MinotaurTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:40:51 PM
using System;
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;
using CoC.Frontend.StatusEffect;

namespace CoC.Frontend.Transformations
{
	internal abstract class MinotaurTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 3, 3 }, 1, 2);
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


			//STATS
			//Strength h
			if (Utils.Rand(3) == 0)
			{
				//weaker characters gain more
				if (target.relativeStrength <= 50)
				{
					//very weak targets gain more
					if (target.relativeStrength <= 20)
					{
						target.ChangeStrength(3);
					}
					else
					{
						target.ChangeStrength(2);
					}
				}
				//stronger characters gain less
				else
				{
					//small growth if over 75
					if (target.relativeStrength >= 75)
					{
						target.ChangeStrength(.5f);
					}
					//faster from 50-75
					else
					{
						target.ChangeStrength(1);
					}
				}
				//Chance of speed drop
				if (Utils.Rand(2) == 0 && target.relativeStrength > 50)
				{
					target.ChangeSpeed(-1);
				}
			}
			//Toughness (chance of - sensitivity)
			if (Utils.Rand(3) == 0)
			{
				//weaker characters gain more
				if (target.relativeToughness <= 50)
				{
					//very weak targets gain more
					if (target.relativeToughness <= 20)
					{
						target.ChangeToughness(3);
					}
					else
					{
						target.ChangeToughness(2);
					}
				}
				//stronger characters gain less
				else
				{
					//small growth if over 75
					if (target.relativeToughness >= 75)
					{
						target.ChangeToughness(.5f);
					}
					//faster from 50-75
					else
					{
						target.ChangeToughness(1);
					}
				}
				//chance of less sensitivity
				if (Utils.Rand(2) == 0 && target.relativeSensitivity > 10)
				{
					if (target.relativeToughness > 75)
					{
						target.ChangeSensitivity(-3);
					}
					if (target.relativeToughness <= 75 && target.relativeToughness > 50)
					{
						target.ChangeSensitivity(-2);
					}
					if (target.relativeToughness <= 50)
					{
						target.ChangeSensitivity(-3);
					}
				}
			}
			//SEXUAL
			//Boosts ball size MORE than equinum :D:D:D:D:D:D:
			if (Utils.Rand(2) == 0 && target.balls.size <= 5 && target.genitals.HasAnyCocksOfType(CockType.HORSE))
			{
				//Chance of ball growth if not 3" yet
				if (target.balls.count == 0)
				{
					target.balls.GrowBalls();

					target.DeltaCreatureStats(lib: 2, lus: 5);
				}
				else
				{
					target.balls.EnlargeBalls(1);
					target.DeltaCreatureStats(lib: 1, lus: 3);
				}
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
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
				target.womb.ClearOviposition();
				//if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Restore arms to become human arms again
			if (Utils.Rand(4) == 0)
			{
				target.RestoreArms();
			}
			//+hooves
			if (target.lowerBody.type != LowerBodyType.HOOVED)
			{
				if (Utils.Rand(3) == 0)
				{
					//Catch-all
					target.UpdateLowerBody(LowerBodyType.HOOVED);
					target.ChangeSpeed(1);

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}
			if (!hyperHappy)
			{
				//Kills vagina size (and eventually the whole vagina)
				if (target.vaginas.Count > 0)
				{
					//behavior not defined for multi vag. i'
					VaginalLooseness minLooseness = target.genitals.minVaginalLooseness;
					//
					if (target.genitals.LargestVaginalLooseness() > minLooseness)
					{
						//tighten that bitch up!
						foreach (Vagina vag in target.vaginas)
						{
							if (vag.looseness > minLooseness)
							{
								vag.DecreaseLooseness();
							}
						}
					}
					else if (target.vaginas.Count > 1)
					{
						target.RemoveExtraVaginas();
					}
					else
					{
						if (target.cocks.Count == 0)
						{
							target.genitals.AddCock(CockType.HORSE, target.vaginas[0].clit.length + 2, 1);
						}

						//Goodbye womanhood!
						target.genitals.RemoveAllVaginas();

					}
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
				//-Remove extra breast rows
				if (target.breasts.Count > 1 && Utils.Rand(3) == 0)
				{
					target.RemoveExtraBreastRows();
				}

				//Shrink boobages till they are normal
				else if (Utils.Rand(2) == 0 && target.breasts.Count > 0)
				{
					//Single row
					CupSize smallestCup = EnumHelper.Max(target.genitals.smallestPossibleCupSize, CupSize.B);

					bool changedAny = false;

					foreach (Breasts row in target.breasts.Where(x => x.cupSize > smallestCup))
					{
						changedAny |= row.ShrinkBreasts() > 0;
						if (row.cupSize > CupSize.E_BIG)
						{
							changedAny |= row.ShrinkBreasts() > 0;
						}
					}

					if (changedAny && --remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}


			//Boosts cock size up to 36"x5".
			//MOD: Used linq: find the first cock that matches the requirement, or if no cocks exist, return null.
			Cock firstCockToGrow = target.cocks.FirstOrDefault(x => x.type == CockType.HORSE && x.length < 36 || x.girth < 5);
			//MOD: thus, we can simply place the null check here to see if we actually grow it.
			if (!(firstCockToGrow is null) && Utils.Rand(2) == 0)
			{
				if (firstCockToGrow.girth < 5 && firstCockToGrow.length < 36)
				{
					firstCockToGrow.IncreaseThickness(1);
					firstCockToGrow.IncreaseLength(2 + Utils.Rand(8));
				}
				else if (firstCockToGrow.length < 36)
				{
					firstCockToGrow.IncreaseLength(2 + Utils.Rand(8));
				}
				else
				{
					firstCockToGrow.IncreaseThickness(1);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Morph dick to horsediiiiick
			Cock firstNonHorse = target.cocks.FirstOrDefault(x => x.type != CockType.HORSE);
			if (!(firstNonHorse is null) && Utils.Rand(2) == 0)
			{

				//Text for humandicks or others
				//Text for dogdicks
				target.genitals.UpdateCockWithLength(firstNonHorse, CockType.HORSE, firstNonHorse.length + 4);

				target.DeltaCreatureStats(lib: 5, sens: 4, lus: 35);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Males go into rut
			if (Utils.Rand(4) == 0)
			{
				if (target.GoIntoRut())
				{
					remainingChanges--;
					if (remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}

			//Anti-masturbation status
			if (Utils.Rand(4) == 0 && !target.HasTimedEffect<Dysfunction>() && target.gender != Gender.GENDERLESS)
			{
				target.AddTimedEffect<Dysfunction>();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Appearance shit:
			//Tail, Ears, Hooves, Horns, Height (no prereq), Face
			//+height up to 9 foot
			//Mod note: rand(1.7) seriously? rand only works with ints, guys. that's straight from vanilla. though to be fair, it's not caught there b/c actionscript is shit.
			if (Utils.Rand(17) < 10 && target.build.heightInInches < 108)
			{
				byte temp = (byte)(Utils.Rand(5) + 3);
				//Slow rate of growth near ceiling
				if (target.build.heightInInches > 90)
				{
					temp /= 2;
				}
				//Never 0
				if (temp == 0)
				{
					temp = 1;
				}
				//Flavor texts. Flavored like 1950's cigarettes. Yum.
				target.build.IncreaseHeight(temp);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Face change, requires Ears + Height + Hooves
			if (target.ears.type == EarType.COW && target.lowerBody.type == LowerBodyType.HOOVED && target.build.heightInInches >= 90
					&& target.face.type != FaceType.COW_MINOTAUR && Utils.Rand(3) == 0)
			{
				FaceData oldData = target.face.AsReadOnlyData();
				target.UpdateFace(FaceType.COW_MINOTAUR);
				sb.Append(UpdateFaceText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//+mino horns require ears/tail
			if (Utils.Rand(3) == 0 && target.ears.type == EarType.COW && target.tail.type == TailType.COW)
			{
				//New horns or expanding mino horns
				if (target.horns.type == HornType.BOVINE || target.horns.type == HornType.NONE)
				{
					//Get bigger if target has horns
					if (target.horns.type == HornType.BOVINE)
					{
						//Fems horns don't get bigger.
						if (target.vaginas.Count > 0)
						{
							if (target.horns.significantHornSize > 4)
							{
								target.genitals.AddPentUpTime(200);
								target.ChangeLust(20);
							}
							else
							{
								target.horns.StrengthenTransform(2);
							}
							if (--remainingChanges <= 0)
							{
								return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
							}
						}
						//Males horns get 'uge.
						else
						{

							target.horns.StrengthenTransform(2);

							//boys get a cum refill sometimes
							if (Utils.Rand(2) == 0)
							{
								target.genitals.AddPentUpTime(200);
								target.ChangeLust(20);
							}
							if (--remainingChanges <= 0)
							{
								return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
							}
						}
					}
					//If no horns yet..
					else
					{
						HornData oldData = target.horns.AsReadOnlyData();
						target.UpdateHorns(HornType.BOVINE);
						sb.Append(UpdateHornsText(target, oldData));

						if (--remainingChanges <= 0)
						{
							return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
						}
					}
				}
				//Not mino horns, change to cow-horns
				else
				{
					HornData oldData = target.horns.AsReadOnlyData();
					target.UpdateHorns(HornType.BOVINE);
					sb.Append(UpdateHornsText(target, oldData));

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}
			//+cow ears	- requires tail
			if (target.ears.type != EarType.COW && target.tail.type == TailType.COW && Utils.Rand(2) == 0)
			{
				EarData oldData = target.ears.AsReadOnlyData();
				target.UpdateEars(EarType.COW);
				sb.Append(UpdateEarsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//+cow tail
			if (Utils.Rand(2) == 0 && target.tail.type != TailType.COW)
			{
				TailData oldData = target.tail.AsReadOnlyData();
				target.UpdateTail(TailType.COW);
				sb.Append(UpdateTailText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			// Remove gills
			if (Utils.Rand(4) == 0 && !target.gills.isDefault)
			{
				GillData oldData = target.gills.AsReadOnlyData();
				target.RestoreGills();
				sb.Append(RestoredGillsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			if (Utils.Rand(4) == 0 && target.ass.wetness > target.ass.minWetness)
			{
				target.ass.DecreaseWetness();
				AnalLooseness targetLooseness = EnumHelper.Max(AnalLooseness.LOOSE, target.ass.minLooseness);
				if (target.ass.looseness > targetLooseness)
				{
					target.ass.DecreaseLooseness();
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Give you that mino build!
			//Default
			if (remainingChanges == changeCount)
			{
				if (target.balls.count > 0)
				{
					target.genitals.AddPentUpTime(200);
				}
				(target as CombatCreature)?.AddHP(50);
				target.ChangeLust(50);
			}

			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		protected virtual string UpdateFaceText(Creature target, FaceData oldData)
{
return target.face.TransformFromText(oldData);
}

		protected virtual string UpdateHornsText(Creature target, HornData oldData)
{
return target.horns.TransformFromText(oldData);
}

		protected virtual string UpdateEarsText(Creature target, EarData oldData)
{
return target.ears.TransformFromText(oldData);
}
		protected virtual string UpdateTailText(Creature target, TailData oldTail)
{
return target.tail.TransformFromText(oldTail);
}
		protected virtual string RestoredGillsText(Creature target, GillData oldData)
{
return target.gills.RestoredText(oldData);
}


		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target);
	}
}