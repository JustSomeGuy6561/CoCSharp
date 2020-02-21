//WolfTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:31:45 PM
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
	internal abstract class WolfTransformations : GenericTransformationBase
	{
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

			//Add any free changes here - these can occur even if the change count is 0. these include things such as change in stats (intelligence, etc)
			//change in height, hips, and/or butt, or other similar stats.



			float crit = 0;

			if (Utils.Rand(100) < 15)
			{
				crit = Utils.Rand(20) / 10f + 2;
			}

			bool hasCrit() => crit > 1;

			sb.Append(InitialTransformationText(target, crit));


			//STAT CHANGES - TOU SPE INT RANDOM CHANCE, LIB LUST COR ALWAYS UPPED
			target.DeltaCreatureStats(lib: 1 + Utils.Rand(2), lus: 5 + Utils.Rand(10), corr: 1 + Utils.Rand(5));
			if (target.relativeToughness < 70 && Utils.Rand(3) == 0)
			{
				float delta = target.ChangeToughness(crit);
				sb.Append(IncreasedToughnessText(crit, delta));
			}
			if (target.relativeSpeed > 30 && Utils.Rand(7) == 0)
			{
				float loss = target.DecreaseSpeed(crit);
				sb.Append(DecreasedSpeedText(crit, loss));
			}
			if (target.relativeIntelligence < 60 && Utils.Rand(7) == 0)
			{
				float delta = target.ChangeIntelligence(crit);
				sb.Append(IncreasedIntelligenceText(crit, delta));
			}

			//handle edge case where we start at 0.
			if (remainingChanges <= 0)
			{
				return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Non-free changes.

			//if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);


			//MUTATIONZZZZZ
			//PRE-CHANGES: become biped, remove horns, remove wings, give human tongue, remove claws, remove antennea
			//no claws
			if (Utils.Rand(4) == 0 && target.arms.hands.isClaws)
			{
				ArmData oldData = target.arms.AsReadOnlyData();

				if (target.RestoreArms())
				{
					sb.Append(RestoredArmsText(target, oldData));
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}
			//remove antennae
			if (target.antennae.type != AntennaeType.NONE && Utils.Rand(3) == 0)
			{
				AntennaeData oldData = target.antennae.AsReadOnlyData();
				target.RestoreAntennae();

				sb.Append(RestoredAntennaeText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//remove horns
			if (target.horns.type != HornType.NONE && Utils.Rand(3) == 0)
			{
				HornData oldData = target.horns.AsReadOnlyData();
				target.RestoreHorns();
				sb.Append(RestoredHornsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//remove wings
			if ((target.wings.type != WingType.NONE || target.back.type == BackType.SHARK_FIN) && Utils.Rand(3) == 0)
			{
				BackData oldBack = target.back.AsReadOnlyData();
				if (target.back.type == BackType.SHARK_FIN)
				{
					target.RestoreBack();
				}

				WingData oldWings = target.wings.AsReadOnlyData();
				target.RestoreWings();
				sb.Append(RestoredBackAndWings(target, oldWings, oldBack));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//give human tongue
			if (target.tongue.type != TongueType.HUMAN && Utils.Rand(3) == 0)
			{
				//MOD NOTE: this was incorrect - this actually was === (equal with strict typechecking) instead of = (assign). so this was an implicit bool.
				//this is part of the reason why the rework forces all value updates as function calls. the more you know.
				TongueData oldData = target.tongue.AsReadOnlyData();
				target.RestoreTongue();
				sb.Append(RestoredTongueText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//remove non-wolf eyes
			if (Utils.Rand(3) == 0 && target.eyes.type != EyeType.HUMAN && target.eyes.type != EyeType.WOLF)
			{
				EyeData oldData = target.eyes.AsReadOnlyData();
				target.RestoreEyes();
				sb.Append(RestoredEyesText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//normal legs
			if (target.lowerBody.type != LowerBodyType.WOLF && Utils.Rand(4) == 0)
			{
				LowerBodyData oldData = target.lowerBody.AsReadOnlyData();
				target.RestoreLowerBody();
				sb.Append(RestoredLowerBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//normal arms
			if (Utils.Rand(4) == 0)
			{
				ArmData oldData = target.arms.AsReadOnlyData();
				target.RestoreArms();
				sb.Append(RestoredArmsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			HairData oldHair = target.hair.AsReadOnlyData();
			//remove feather hair
			if (Utils.Rand(4) == 0 && RemoveFeatheryHair(target))
			{
				sb.Append(RemovedFeatheryHairText(target, oldHair));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//remove basilisk hair
			if (Utils.Rand(4) == 0 && target.hair.IsBasiliskHair())
			{
				HairData oldData = target.hair.AsReadOnlyData();
				target.RestoreHair();
				sb.Append(RemovedBasiliskHair(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//MUTATIONZ AT ANY TIME: wolf dick, add/decrease breasts, decrease breast size if above D
			//get a wolf dick
			//if ya genderless we give ya a dick cuz we nice like that
			if (target.gender == Gender.GENDERLESS && !hyperHappy)
			{
				target.genitals.AddCock(CockType.WOLF, Utils.Rand(4) + 4, Utils.Rand(8) / 4.0f + 0.25f, 1.5f);

				sb.Append(BecameMaleByGrowingCock(target));

				target.DeltaCreatureStats(lib: 3, sens: 2, lus: 25);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//if ya got a dick that's ok too we'll change it to wolf
			//MOD NOTE: but only if you don't only have wolf cocks, of course (because if they are all wolf cocks, we can't make anything wolf cocks, duh).
			if (target.hasCock && !target.genitals.OnlyHasCocksOfType(CockType.WOLF) && Utils.RandBool())
			{
				//MOD NOTE: no longer shamelessly copy/pasted from dog cock, because we have better ways of looping in C#.

				Cock firstNonWolf = target.cocks.First(x => x.type != CockType.WOLF);
				CockData oldData = firstNonWolf.AsReadOnlyData();

				//Select first non-wolf cock
				//MOD: now using type description because we can, so the fact this used generic is irrelevant now :)

				if (firstNonWolf.type == CockType.HORSE)
				{ //horses get changes
					if (firstNonWolf.length > 6)
					{
						firstNonWolf.DecreaseLength(2);
					}
					else
					{
						firstNonWolf.DecreaseLength(.5f);
					}

					firstNonWolf.IncreaseThickness(.5f);
				}

				target.DeltaCreatureStats(sens: 3, lus: 5 * crit);


				target.genitals.UpdateCockWithKnot(firstNonWolf, CockType.WOLF, 1.5f);
				firstNonWolf.IncreaseThickness(2);

				sb.Append(ChangedCockDog(target, oldData, oldData.cockIndex));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//titties for those who got titties
			//wolfs have 8 nips so, 4 rows max. fen has no power here I'm making a wolf not a dog.
			//MOD: still stolen shamelessly from dog and updated, but now with the modern format. woo!

			//MOD: if breasts can be updated (not flat) and we flip a coin.
			if (target.genitals.BiggestCupSize() > CupSize.FLAT && Utils.RandBool())
			{
				if (target.breasts.Count < 4)
				{
					byte breastCount = (byte)target.breasts.Count;
					BreastCollectionData oldBreastData = target.genitals.allBreasts.AsReadOnlyData();

					//in vanilla code, we had some strange checks here that required the first row (and only the first row) be a certain size.
					//now, we check all the rows, but no longer require any of them to be a certain size. instead, if it's not the right size, we make it that size,
					//then add the row. so, for two rows, your first row must be at least a B-Cup. for 3: first must be a C cup, second an A cup.
					//if we supported 4 rows, it'd be D, B, A.

					for (int x = 0; x < target.breasts.Count; x++)
					{
						CupSize requiredSize = (CupSize)(byte)(target.breasts.Count - x);
						if (x == 0)
						{
							requiredSize++;
						}
						if (target.breasts[x].cupSize < requiredSize)
						{
							target.breasts[x].SetCupSize(requiredSize);
						}
					}

					target.genitals.AddBreastRow(target.breasts[breastCount - 1].cupSize.ByteEnumSubtract(1));


					bool doCrit = false, uberCrit = false;
					if (target.breasts.Count == 2)
					{
						target.IncreaseCreatureStats(lus: 5, sens: 6);
					}
					else if (hasCrit())
					{
						doCrit = true;
						if (crit > 2)
						{
							target.IncreaseCreatureStats(sens: 6, lus: 15);
							uberCrit = true;
						}
						else
						{
							target.IncreaseCreatureStats(sens: 3, lus: 10);
						}

					}

					sb.Append(UpdateAndGrowAdditionalRowText(target, oldBreastData, doCrit, uberCrit));


					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
				else
				{
					BreastCollectionData oldBreastData = target.genitals.allBreasts.AsReadOnlyData();

					//only call the normalize text if we actually did anything. related, anthro breasts now returns a bool. thanks, fox tfs.
					if (target.genitals.AnthropomorphizeBreasts())
					{
						sb.Append(NormalizedBreastSizeText(target, oldBreastData));
					}
				}
			}


			//Remove breast rows if over 4
			if (target.breasts.Count > 4 && Utils.Rand(3) == 0)
			{
				BreastCollectionData oldCollection = target.genitals.allBreasts.AsReadOnlyData();
				target.genitals.RemoveBreastRows(target.breasts.Count - 4);

				sb.Append(RemovedExcessRows(target, oldCollection));

			}
			//Grow breasts if has vagina and has no breasts/nips
			//Shrink breasts if over D-cup
			CupSize targetSize = EnumHelper.Max(target.genitals.smallestPossibleCupSize, CupSize.D);
			if (!hyperHappy && target.genitals.BiggestCupSize() > targetSize && Utils.Rand(3) == 0)
			{
				BreastCollectionData oldCollection = target.genitals.allBreasts.AsReadOnlyData();

				bool changedAnything = false;
				foreach (Breasts row in target.breasts)
				{
					//If this row is over threshhold
					if (row.cupSize > targetSize)
					{
						//Big change
						if (row.cupSize > CupSize.EE_BIG)
						{
							changedAnything |= row.ShrinkBreasts((byte)(2 + Utils.Rand(3))) > 0;
						}
						//Small change
						else
						{
							changedAnything |= row.ShrinkBreasts() > 0;
						}
						//Increment changed rows
					}
				}
				//Count shrinking
				if (changedAnything)
				{
					sb.Append(ShrunkRowsText(target, oldCollection));

					remainingChanges--;
					if (remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}
			//MUTATIONZ LEVEL 1: fur, stop hair growth, ears, tail
			//Gain fur
			if (Utils.Rand(5) == 0 && !target.body.IsFurBodyType())
			{
				BodyData oldData = target.body.AsReadOnlyData();
				Species.WOLF.GetRandomFurColors(out FurColor primary, out FurColor underbody);
				if (FurColor.IsNullOrEmpty(underbody))
				{
					target.UpdateBody(BodyType.UNDERBODY_FUR, primary);
				}
				else
				{
					target.UpdateBody(BodyType.UNDERBODY_FUR, primary, underbody);
				}

				sb.Append(UpdateBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Ears time
			if (Utils.Rand(3) == 0 && target.ears.type != EarType.WOLF)
			{
				EarData oldData = target.ears.AsReadOnlyData();
				target.UpdateEars(EarType.WOLF);
				sb.Append(UpdateEarsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Wolf tail
			if (Utils.Rand(3) == 0 && target.tail.type != TailType.WOLF)
			{
				TailData oldData = target.tail.AsReadOnlyData();
				target.UpdateTail(TailType.WOLF);
				sb.Append(UpdateTailText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Sets hair normal
			if (target.hair.type != HairType.NORMAL && Utils.Rand(3) == 0)
			{
				HairData oldData = target.hair.AsReadOnlyData();
				target.RestoreHair();
				sb.Append(RestoreHairText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//MUTATIONZ LEVEL 2: fur->arms fur+tail+ears->face stophair->nohair fur+tail->legs
			//gain wolf face
			if (target.face.type != FaceType.WOLF && target.ears.type == EarType.WOLF && target.tail.type == TailType.WOLF && target.body.IsFurBodyType() && Utils.Rand(5) == 0)
			{
				FaceData oldData = target.face.AsReadOnlyData();
				target.UpdateFace(FaceType.WOLF);
				sb.Append(UpdateFaceText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//legz
			if (target.lowerBody.legCount == 2 && target.lowerBody.type != LowerBodyType.WOLF && target.tail.type == TailType.WOLF && target.body.IsFurBodyType() && Utils.Rand(4) == 0)
			{
				//Hooman feets
				//Hooves -> Paws
				LowerBodyData oldData = target.lowerBody.AsReadOnlyData();
				target.UpdateLowerBody(LowerBodyType.WOLF);
				sb.Append(UpdateLowerBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//MUTATIONZ LEVEL 3: face->eyes
			if (target.eyes.type != EyeType.WOLF && target.face.type == FaceType.WOLF && Utils.Rand(4) == 0)
			{
				EyeData oldData = target.eyes.AsReadOnlyData();
				target.UpdateEyes(EyeType.WOLF);
				sb.Append(UpdateEyesText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//MISC CRAP
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


			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		protected virtual string ClearOvipositionText(Creature target)
{
return RemovedOvipositionTextGeneric(target);
}




		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target, float crit);

		protected abstract string NormalizedBreastSizeText(Creature target, BreastCollectionData oldBreastData);
		protected abstract string UpdateAndGrowAdditionalRowText(Creature target, BreastCollectionData oldBreastData, bool doCrit, bool uberCrit);

		protected abstract string IncreasedToughnessText(float desiredAmount, float actualAmount);
		protected abstract string DecreasedSpeedText(float desiredAmount, float actualAmount);
		protected abstract string IncreasedIntelligenceText(float desiredAmount, float actualAmount);

		protected virtual string RestoredArmsText(Creature target, ArmData oldData)
		{
			return target.arms.RestoredText(oldData);
		}

		protected virtual string UpdateEarsText(Creature target, EarData oldData)
		{
			return target.ears.TransformFromText(oldData);
		}
		protected virtual string UpdateTailText(Creature target, TailData oldTail)
		{
			return target.tail.TransformFromText(oldTail);
		}
		protected virtual string RestoreHairText(Creature target, HairData oldHair)
		{
			return target.hair.RestoredText(oldHair);
		}
		protected virtual string UpdateFaceText(Creature target, FaceData oldFace)
		{
			return target.face.RestoredText(oldFace);
		}
		protected virtual string UpdateLowerBodyText(Creature target, LowerBodyData oldLowerBody)
		{
			return target.lowerBody.TransformFromText(oldLowerBody);
		}
		protected virtual string UpdateEyesText(Creature target, EyeData oldData)
		{
			return target.eyes.TransformFromText(oldData);
		}
		protected virtual string UpdateBodyText(Creature target, BodyData oldData)
		{
			return target.body.TransformFromText(oldData);
		}
		protected abstract string ShrunkRowsText(Creature target, BreastCollectionData oldCollection);
		protected abstract string RemovedExcessRows(Creature target, BreastCollectionData oldCollection);
		protected abstract string ChangedCockDog(Creature target, CockData oldData, int cockIndex);
		protected abstract string BecameMaleByGrowingCock(Creature target);
		protected abstract string RemovedFeatheryHairText(Creature target, HairData oldHair);
		protected abstract string RestoredBackAndWings(Creature target, WingData oldWings, BackData oldBack);
		protected virtual string RestoredTongueText(Creature target, TongueData oldData)
		{
			return target.tongue.RestoredText(oldData);
		}
		protected virtual string RestoredEyesText(Creature target, EyeData oldData)
		{
			return target.eyes.RestoredText(oldData);
		}
		protected virtual string RestoredLowerBodyText(Creature target, LowerBodyData oldData)
		{
			return target.lowerBody.RestoredText(oldData);
		}
		protected abstract string RemovedBasiliskHair(Creature target, HairData oldData);
		protected virtual string RestoredNeckText(Creature target, NeckData oldData)
		{
			return target.neck.RestoredText(oldData);
		}
		protected virtual string RestoredBackText(Creature target, BackData oldData)
		{
			return target.back.RestoredText(oldData);
		}
		protected virtual string RestoredHornsText(Creature target, HornData oldData)
		{
			return target.horns.RestoredText(oldData);
		}
		protected virtual string RestoredAntennaeText(Creature target, AntennaeData oldData)
		{
			return target.antennae.RestoredText(oldData);
		}
	}
}