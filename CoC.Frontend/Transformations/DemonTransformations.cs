//DemonTransformations.cs
//Description:
//Author: JustSomeGuy
//1/23/2020 4:25:13 AM
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Settings.Gameplay;

namespace CoC.Frontend.Transformations
{
	//Demon Transformations are gender-specific (succubi = female, incubi = male, omnibi = herm), but all of the code uses the same logic, just altered slightly for
	//each gender. So, they're all getting one transformation class, but with a gender state to determine what they're going for and a bool for purified.

	internal abstract class DemonTransformations : GenericTransformationBase
	{
		protected readonly Gender desiredGender;
		protected readonly bool isPurified;

		protected DemonTransformations(Gender transformationGender, bool purified)
		{
			desiredGender = transformationGender == Gender.GENDERLESS ? Gender.MALE : transformationGender;

			isPurified = purified;
		}

		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//for some unknown reason, demon tfs roll out their own chance system completely unique to them. ok.

			StringBuilder sb = new StringBuilder();

			//For all of these, any text regarding the transformation should be instead abstracted out as an abstract string function. append the result of this abstract function
			//to the string builder declared above (aka sb.Append(FunctionCall(variables));) string builder is just a fancy way of telling the compiler that you'll be creating a
			//long string, piece by piece, so don't do any crazy optimizations first.

			//the initial text for starting the transformation. feel free to add additional variables to this if needed.
			sb.Append(InitialTransformationText(target));

			//Add any free changes here - these can occur even if the change count is 0. these include things such as change in stats (intelligence, etc)
			//change in height, hips, and/or butt, or other similar stats.

			int rando = Utils.Rand(100);
			int delta = target.GetExtraData()?.deltaTransforms ?? 0;

			if (delta != 0)
			{
				rando += 5 * delta + Utils.Rand(5 * delta);
			}

			//First, check if this tf has the male flag set (for male or herm tfs). If it does, add or grow cocks.
			if (desiredGender.HasFlag(Gender.MALE))
			{
				byte addedCocks = 0;
				//if our initial roll was a crit, roll again. if we crit again, we may add several cocks, if possible.
				if (rando >= 85 && target.cocks.Count < Genitals.MAX_COCKS && Utils.Rand(10) < target.corruptionTrue / 25)
				{
					addedCocks = GrowCockGeneric(target, (byte)(Utils.Rand(2) + 2));

					target.DeltaCreatureStats(lib: 3 * addedCocks, sens: 5 * addedCocks, lus: 10 * addedCocks);
					if (!isPurified)
					{
						target.IncreaseCorruption(8);
					}
				}
				//otherwise, only add a cock if we have none or we originally rolled a crit (but failed to crit again)
				else if (target.cocks.Count == 0 || (rando >= 85 && target.cocks.Count < Genitals.MAX_COCKS))
				{
					addedCocks = GrowCockGeneric(target, 1);

					target.DeltaCreatureStats(lib: 3, sens: 5, lus: 10);
					if (!isPurified)
					{
						target.IncreaseCorruption(5);
					}
				}
				//if that fails, it means we had a cock already, or can't grow any more of them.
				else
				{
					Cock shortest = target.genitals.ShortestCock();
					double lengthDelta;

					if (rando >= 45)
					{
						lengthDelta = shortest.IncreaseLength(Utils.Rand(3) + 3);
						shortest.IncreaseThickness(1);
					}
					else if (Utils.Rand(4) == 0)
					{
						lengthDelta = shortest.IncreaseLength(3);
					}
					else
					{
						lengthDelta = shortest.IncreaseLength(1);
					}

					target.DeltaCreatureStats(lib: 2, sens: 1, lus: 5 + lengthDelta * 3);
					if (!isPurified)
					{
						target.IncreaseCorruption();
					}
					//no idea why this occurs, but ok.
					target.IncreaseIntelligence(1);
				}
			}
			//Otherwise, we're targeting female demon tfs only. this means we need to shrink (and possibly remove) the largest cock the target has, unless hyper happy is on.
			else if (!hyperHappy && target.hasCock)
			{
				Cock largest = target.genitals.LongestCock(); //this loops through all the cocks and finds the longest. obviously, if the count is 1, this is simply the first element.

				//we'll need this if it gets removed. if not, this can still be used, this time to determine how much we shrunk.
				CockData oldData = largest.AsReadOnlyData();

				//try decreasing it by 1-3. if this causes it to be removed instead, that's fine, but we need to know.
				//Note that this remove is now IN PLACE, not LAST. so if we remove the 3rd one, the old 4th is now 3rd, and so on.
				bool removed = target.genitals.ShrinkCockAndRemoveIfTooSmall(largest, Utils.Rand(3) + 1);
			}

			//Then, check if the tf has the female flag set (for female or herm tfs). if it does, add a vagina (if needed), and increase breast size.
			if (desiredGender.HasFlag(Gender.FEMALE))
			{
				//don't currently have a vagina and herm tf or we're genderless, or it's a crit, or we rerolled a crit.
				if (!target.hasVagina && (desiredGender == Gender.HERM || target.gender == Gender.GENDERLESS || rando > 65 || Utils.Rand(3) == 0))
				{
					target.genitals.AddVagina(VaginaType.HUMAN);
				}
				//do have one, and rolled a high crit.
				else if (target.hasVagina && rando >= 85)
				{
					foreach (Vagina vag in target.vaginas)
					{
						//grow each clit anywhere from 0.25in to 1in, if they are below the largest normal size.
						if (vag.clit.length < vag.clit.largestNormalSize)
						{
							vag.GrowClit((Utils.Rand(4) + 1) * 0.25f);
							//cap it at the largest normal size.
							if (vag.clit.length > vag.clit.largestNormalSize)
							{
								vag.SetClitSize(vag.clit.largestNormalSize);
							}
						}
					}
				}
				//do have one, rolled a crit, but not a high crit.
				else if (target.hasVagina && rando > 65)
				{
					target.HaveGenericVaginalOrgasm(0, true, true);
					target.vaginas.ForEach(x => x.IncreaseWetness());
				}


				//now, breasts. these are the fallback, of sorts, so they grow larger when we don't crit. they also grow larger when we crit if we're targeting herms.

				if (rando < 85 || desiredGender == Gender.HERM)
				{
					//only occurs via herm.
					if (rando >= 85)
					{
						target.breasts.ForEach(x => x.GrowBreasts(3));
					}
					else
					{

						byte temp = (byte)(1 + Utils.Rand(3));
						CupSize largestSize = target.genitals.BiggestCupSize();
						if (largestSize < CupSize.B && Utils.Rand(3) == 0)
						{
							temp++;
						}

						if (largestSize < CupSize.DD && Utils.Rand(4) == 0)
						{
							temp++;
						}

						if (largestSize < CupSize.DD_BIG && Utils.Rand(5) == 0)
						{
							temp++;
						}

						target.breasts.ForEach(x => x.GrowBreasts(temp));
					}
				}
			}
			//if not, we're targeting male demon tfs only. this means we may need to shrink any overlarge breasts. Additionally, higher rng rolls may now remove vaginas.
			else if (!hyperHappy)
			{
				//if high crit: decrease bonus capacity, and remove a vagina, flat-out.
				if (rando >= 85 && target.hasVagina)
				{
					target.genitals.DecreaseBonusVaginalCapacity(5);
					target.genitals.RemoveVagina();
				}
				//otherwise, if somewhat high, decrease bonus vaginal capacity (all vaginas), and wetness (last vagina). if this causes the bonus capacity to drop to 0
				//and would cause it to go negative if we allowed that, remove the last vagina.
				else if (rando >= 65)
				{
					Vagina lastVagina = target.vaginas[target.vaginas.Count - 1];
					lastVagina.DecreaseWetness(1);

					//this is being super pedantic, but i'd prefer it lower the stat, then remove the vagina. hence this bool here.
					bool remove = target.genitals.standardBonusVaginalCapacity < 5;
					//decrease first.
					target.genitals.DecreaseBonusVaginalCapacity(5);

					//then remove it.
					if (remove)
					{
						target.genitals.RemoveVagina();
					}
				}

				//
				if (target.genitals.BiggestCupSize() > target.genitals.smallestPossibleCupSize && (rando >= 85 || (rando > 65 && Utils.RandBool()) || (rando <= 65 && Utils.Rand(4) == 0)))
				{
					foreach (Breasts breast in target.breasts)
					{
						if (breast.cupSize > target.genitals.smallestPossibleCupSize)
						{
							byte amount = 1;
							if (rando >= 85)
							{
								amount++;
							}

							breast.ShrinkBreasts(amount);
						}
					}
				}
			}

			//never called in vanilla. if i read it correctly, it just initializes to 0 with a max of 1.
			int changeCount = base.GenerateChangeCount(target, new int[] { 3 }, 0, 0);

			if (changeCount == 0)
			{
				return ApplyChangesAndReturn(target, sb, 0);
			}

			int remainingChanges = changeCount;


			//Neck restore
			if (!target.neck.isDefault && Utils.Rand(4) == 0)
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
			//Demonic changes - higher chance with higher corruption.
			if (Utils.Rand(40) + target.corruption / 3 > 35 && !isPurified)
			{

				//Change tail if already horned.
				if (target.tail.type != TailType.DEMONIC && !target.horns.isDefault)
				{
					target.IncreaseCorruption(4);
					TailData oldData = target.tail.AsReadOnlyData();
					target.UpdateTail(TailType.DEMONIC);
					sb.Append(UpdateTailText(target, oldData));

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
				//grow horns!
				if (target.horns.numHorns == 0 || (Utils.Rand(target.horns.numHorns + 3) == 0))
				{
					if (target.horns.numHorns < 12 && (target.horns.type == HornType.NONE || target.horns.type == HornType.DEMON))
					{

						if (target.horns.type == HornType.NONE)
						{
							target.UpdateHorns(HornType.DEMON);
						}

						target.IncreaseCorruption(3);
					}
					//Text for shifting horns
					else if (target.horns.type != HornType.DEMON)
					{
						target.UpdateHorns(HornType.DEMON);
						target.IncreaseCorruption(3);
					}

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
				//Nipples Turn Back:
				if (target.genitals.hasBlackNipples && Utils.Rand(3) == 0)
				{
					target.genitals.SetBlackNipples(false);

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

				//remove fur
				if (target.face.type != FaceType.HUMAN || (target.body.type != BodyType.HUMANOID && Utils.Rand(3) == 0))
				{
					//Remove face before fur!
					if (target.face.type != FaceType.HUMAN)
					{
						target.RestoreFace();
					}
					//De-fur
					else if (target.body.type != BodyType.HUMANOID)
					{
						target.RestoreBody();
					}

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
				//Demon tongue
				if (target.tongue.type == TongueType.SNAKE && Utils.Rand(3) == 0)
				{
					TongueData oldData = target.tongue.AsReadOnlyData();
					target.UpdateTongue(TongueType.DEMONIC);
					sb.Append(UpdateTongueText(target, oldData));

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
				//foot changes - requires furless
				if (target.body.type == BodyType.HUMANOID && Utils.Rand(4) == 0)
				{
					bool changed;
					//Males/genderless get clawed feet
					if (!target.gender.HasFlag(Gender.FEMALE) || (target.gender == Gender.HERM && target.genitals.AppearsMoreMaleThanFemale()))
					{
						changed = target.UpdateLowerBody(LowerBodyType.DEMONIC_CLAWS);
					}
					//Females/futa get high heels
					else
					{
						changed = target.UpdateLowerBody(LowerBodyType.DEMONIC_HIGH_HEELS);
					}

					if (changed && --remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
				//Grow demon wings
				if ((target.wings.type != WingType.BAT_LIKE || !target.wings.isLarge || target.back.type == BackType.SHARK_FIN) && Utils.Rand(8) == 0 && target.IsCorruptEnough(50))
				{
					//grow smalls to large

					if (target.wings.type == WingType.BAT_LIKE && target.IsCorruptEnough(75))
					{
						target.wings.GrowLarge();
					}
					else
					{
						target.UpdateWings(WingType.BAT_LIKE);
					}

					if (target.back.type == BackType.SHARK_FIN)
					{
						target.RestoreBack();
					}

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
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
		protected virtual string UpdateTongueText(Creature target, TongueData oldData)
		{
			return target.tongue.TransformFromText(oldData);
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