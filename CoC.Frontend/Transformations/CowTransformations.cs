//CowTransformations.cs
//Description:
//Author: JustSomeGuy
//2/22/2020 2:37:31 AM
using System;
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Items;
using CoC.Frontend.Perks;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;

namespace CoC.Frontend.Transformations
{
	internal abstract class CowTransformations : GenericTransformationBase
	{
		protected readonly TransformationType transformation;

		protected bool isPurified => transformation == TransformationType.PURIFIED;
		protected bool isEnhanced => transformation == TransformationType.ENHANCED;

		public CowTransformations(TransformationType transformationType)
		{
			transformation = transformationType;
		}


		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 2, 3 }, isEnhanced ? 3 : 1);
			int totalChanges = 0;

			StringBuilder sb = new StringBuilder();

			//For all of these, any text regarding the transformation should be instead abstracted out as an abstract string function. append the result of this abstract function
			//to the string builder declared above (aka sb.Append(FunctionCall(variables));) string builder is just a fancy way of telling the compiler that you'll be creating a
			//long string, piece by piece, so don't do any crazy optimizations first.

			//the initial text for starting the transformation. feel free to add additional variables to this if needed.
			sb.Append(InitialTransformationText(target));

			//Add any free changes here - these can occur even if the change count is 0. these include things such as change in stats (intelligence, etc)
			//change in height, hips, and/or butt, or other similar stats.

			//this will handle the edge case where the change count starts out as 0.

			//paste this line after any tf is applied, and it will: automatically decrement the remaining changes count. if it becomes 0 or less, apply the total number of changes
			//underwent to the target's change count (if applicable) and then return the StringBuilder content.
			//if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			//STATS
			//Increase player str:
			if (target.strength < 60 && Utils.Rand(3) == 0)
			{
				double delta = target.IncreaseStrength((60 - target.strength) / 10.0);
				sb.Append(StrengthUpText(delta));
			}
			//Increase player tou:
			if (target.toughness < 60 && Utils.Rand(3) == 0)
			{
				double delta = target.IncreaseToughness((60 - target.toughness) / 10.0);
				sb.Append(ToughnessUpText(delta));
			}
			//Decrease player spd if it is over 30:
			if (target.relativeSpeed > 30 && target.speed > 30 && Utils.Rand(3) == 0)
			{
				double decrease = target.DecreaseSpeed((target.speed - 30) / 10.0);
				sb.Append(SpeedDownText(decrease));
			}
			//Increase Corr, up to a max of 50.
			//this is silent, apparently.
			if (!isPurified && target.corruption < 50)
			{
				target.IncreaseCorruptionBy((50 - target.corruption) / 10.0);
			}

			if (totalChanges >= changeCount)
			{
				return FinalizeTransformation(target, sb, totalChanges);
			}

			//Sex bits - Duderiffic
			if (target.cocks.Count > 0 && Utils.Rand(2) == 0 && !hyperHappy)
			{
				//If the player has at least one dick, decrease the size of each slightly,

				Cock biggest = target.genitals.LongestCock();
				CockData preChange = biggest.AsReadOnlyData();

				if (biggest.DecreaseLengthAndCheckIfNeedsRemoval(Utils.Rand(3) + 1))
				{
					target.genitals.RemoveCock(biggest);
					if (target.cocks.Count == 0 && !target.hasVagina)
					{
						BallsData oldBalls = target.balls.AsReadOnlyData();
						target.AddVagina(0.25);
						target.genitals.RemoveAllBalls();

						sb.Append(MadeFemale(target, preChange, oldBalls));

					}
					else
					{
						sb.Append(ShrunkOneCock(target, preChange, true));
					}
				}
				else
				{
					sb.Append(ShrunkOneCock(target, preChange, false));
				}

				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}

			}
			//Sex bits - girly
			bool boobsGrew = false;
			//Increase player's breast size, if they are FF or bigger
			//do not increase size, but do the other actions:
			CupSize biggestCup = target.genitals.BiggestCupSize();

			if ((biggestCup < CupSize.DD || (!isPurified && biggestCup < CupSize.FF)) && (isEnhanced || Utils.Rand(3) == 0))
			{

				BreastData oldBreasts = target.breasts[0].AsReadOnlyData();
				if (target.breasts[0].GrowBreasts((byte)(1 + Utils.Rand(3))) > 0)
				{
					boobsGrew = true;
					sb.Append(GrewFirstBreastRow(target, oldBreasts));
					if (++totalChanges >= changeCount)
					{
						return FinalizeTransformation(target, sb, totalChanges);
					}
				}
				target.DeltaCreatureStats(sens: .5);
			}

			//Remove feathery hair
			HairData oldHair = target.hair.AsReadOnlyData();
			if (base.RemoveFeatheryHair(target))
			{
				sb.Append(RemovedFeatheryHairText(target, oldHair));
				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}
			}

			//refresh the biggest cup size because we may have grown some breasts earlier.
			biggestCup = target.genitals.BiggestCupSize();

			//If breasts are D or bigger and are not lactating, they also start lactating:
			if (biggestCup >= CupSize.D && !target.genitals.isLactating && (isEnhanced || boobsGrew || Utils.Rand(3) == 0))
			{
				BreastData preLactation = target.breasts[0].AsReadOnlyData();
				target.genitals.StartLactating();

				sb.Append(StartedLactatingText(target, preLactation));

				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}

				target.DeltaCreatureStats(sens: .5);
			}
			//Quad nipples and other 'special isEnhanced things.
			if (isEnhanced)
			{
				//QUAD DAMAGE!
				if (!target.genitals.hasQuadNipples)
				{
					BreastCollectionData oldBreasts = target.genitals.allBreasts.AsReadOnlyData();
					target.genitals.SetQuadNipples(true);

					sb.Append(GrantQuadNippleText(target, oldBreasts));

					if (++totalChanges >= changeCount)
					{
						return FinalizeTransformation(target, sb, totalChanges);
					}
				}
				else if (target.genitals.isLactating)
				{
					BreastCollectionData oldBreasts = target.genitals.allBreasts.AsReadOnlyData();

					target.genitals.BoostLactation(2.5);
					double delta = 0;

					if (target.genitals.nippleLength < 1 || (target.genitals.nippleLength < 1.5 && !isPurified))
					{

						delta = target.genitals.GrowNipples(0.25);
						target.DeltaCreatureStats(sens: .5);
					}

					sb.Append(BoostedLactationText(target, oldBreasts, delta));

					if (++totalChanges >= changeCount)
					{
						return FinalizeTransformation(target, sb, totalChanges);
					}
				}
			}
			//If breasts are already lactating and the player is not lactating beyond a reasonable level, they start lactating more:
			else
			{
				if (target.genitals.isLactating && (target.genitals.lactationStatus < LactationStatus.MODERATE ||
					(!isPurified && target.genitals.lactationStatus < LactationStatus.STRONG)) && (isEnhanced || Utils.Rand(3) == 0))
				{
					BreastCollectionData oldBreasts = target.genitals.allBreasts.AsReadOnlyData();
					double delta = 0;

					target.genitals.BoostLactation(0.75);

					if (target.genitals.nippleLength < 1 || (target.genitals.nippleLength < 1.5 && !isPurified))
					{

						delta = target.genitals.GrowNipples(0.25);
						target.DeltaCreatureStats(sens: .5);
					}

					sb.Append(BoostedLactationText(target, oldBreasts, delta));

					if (++totalChanges >= changeCount)
					{
						return FinalizeTransformation(target, sb, totalChanges);
					}
				}
				else if (isPurified && target.genitals.lactationStatus >= LactationStatus.STRONG)
				{
					BreastCollectionData oldData = target.genitals.allBreasts.AsReadOnlyData();

					target.DeltaCreatureStats(sens: .5);
					target.genitals.BoostLactation(-1);

					sb.Append(BoostedLactationText(target, oldData, 0));


					if (++totalChanges >= changeCount)
					{
						return FinalizeTransformation(target, sb, totalChanges);
					}
				}
			}
			//If breasts are lactating at a fair level
			//and the player has not received this status,
			//apply an effect where the player really wants
			//to give their milk to other creatures
			//(capable of getting them addicted):
			biggestCup = target.genitals.BiggestCupSize();

			if (!target.HasPerk<Feeder>() && target.genitals.lactationStatus >= LactationStatus.MODERATE && Utils.Rand(2) == 0 && biggestCup >= CupSize.DD &&
				target.IsCorruptEnough(35))
			{
				target.perks.AddPerk<Feeder>();
				sb.Append(GainedFeederPerk(target));
				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}
			}
			//UNFINISHED
			//If player has addictive quality and drinks pure version, removes addictive quality.
			//if the player has a vagina and it is tight, it loosens.
			if (target.hasVagina)
			{
				VaginalLooseness targetLooseness = EnumHelper.Min(VaginalLooseness.LOOSE, target.genitals.maxVaginalLooseness);
				Vagina tightest = target.genitals.TightestVagina();

				if (tightest.looseness < targetLooseness && Utils.Rand(2) == 0)
				{
					VaginaData preLoosened = tightest.AsReadOnlyData();


					tightest.IncreaseLooseness(2);
					sb.Append(LoosenedTwatText(target, preLoosened));

					if (++totalChanges >= changeCount)
					{
						return FinalizeTransformation(target, sb, totalChanges);
					}

					target.DeltaCreatureStats(lus: 10);
				}
			}
			//Neck restore
			if (target.neck.type != NeckType.HUMANOID && Utils.Rand(4) == 0)
			{
				NeckData oldData = target.neck.AsReadOnlyData();
				target.RestoreNeck();
				sb.Append(RestoredNeckText(target, oldData));

				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}
			}
			//Rear body restore
			if (target.back.type != BackType.SHARK_FIN && Utils.Rand(5) == 0)
			{
				BackData oldData = target.back.AsReadOnlyData();
				target.RestoreBack();
				sb.Append(RestoredBackText(target, oldData));
				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}
			}
			//Ovi perk loss
			if (!isPurified && target.womb.canRemoveOviposition && Utils.Rand(5) == 0)
			{
				target.womb.ClearOviposition();
				sb.Append(ClearOvipositionText(target));

				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}
			}
			//General Appearance (Tail -> Ears -> Paws(fur stripper) -> Face -> Horns
			//Give the player a bovine tail, same as the minotaur
			if (!isPurified && target.tail.type != TailType.COW && Utils.Rand(3) == 0)
			{
				TailData oldData = target.tail.AsReadOnlyData();

				target.UpdateTail(TailType.COW);
				sb.Append(ChangedTailText(target, oldData));

				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}
			}
			//Give the player bovine ears, same as the minotaur
			if (!isPurified && target.ears.type != EarType.COW && Utils.Rand(4) == 0 && target.tail.type == TailType.COW)
			{

				EarData oldEars = target.ears.AsReadOnlyData();
				target.UpdateEars(EarType.COW);

				sb.Append(ChangedEarsText(target, oldEars));

				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}
			}
			//If the player is under 7 feet in height, increase their height, similar to the minotaur
			if (((isEnhanced && target.build.heightInInches < 96) || target.build.heightInInches < 84) && Utils.Rand(2) == 0)
			{
				int temp = Utils.Rand(5) + 3;
				//Slow rate of growth near ceiling
				if (target.build.heightInInches > 74)
				{
					temp = (int)Math.Floor(temp / 2.0);
				}
				//Never 0
				if (temp == 0)
				{
					temp = 1;
				}


				byte delta = target.build.IncreaseHeight((byte)temp);

				sb.Append(GrowTaller(target, delta));

				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}
			}
			//Give the player hoofs, if the player already has hoofs STRIP FUR
			if (!isPurified && target.lowerBody.type != LowerBodyType.HOOVED && target.ears.type == EarType.COW)
			{
				if (Utils.Rand(3) == 0)
				{
					var oldData = target.lowerBody.AsReadOnlyData();
					target.UpdateLowerBody(LowerBodyType.HOOVED);

					sb.Append(ChangedLowerBodyText(target, oldData));

					if (++totalChanges >= changeCount)
					{
						return FinalizeTransformation(target, sb, totalChanges);
					}
				}
			}
			//If the player's face is non-human, they gain a human face
			if (!isEnhanced && target.lowerBody.type == LowerBodyType.HOOVED && target.face.type != FaceType.HUMAN && Utils.Rand(4) == 0)
			{
				//Remove face before fur!
				var oldData = target.face.AsReadOnlyData();
				target.RestoreFace();
				sb.Append(RestoredFaceText(target, oldData));

				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}
			}
			//isEnhanced get shitty fur
			var targetFur = new FurColor(HairFurColors.BLACK, HairFurColors.WHITE, FurMulticolorPattern.SPOTTED);

			if (isEnhanced && (!target.body.IsFurBodyType() || !target.body.mainEpidermis.fur.Equals(targetFur)))
			{
				var oldData = target.body.AsReadOnlyData();

				if (target.body.type == BodyType.SIMPLE_FUR)
				{
					target.body.ChangeMainFur(targetFur);
				}
				else
				{
					target.UpdateBody(BodyType.SIMPLE_FUR, targetFur);
				}

				if (!oldData.IsFurBodyType())
				{
					sb.Append(ChangedBodyText(target, oldData));
				}
				else
				{
					sb.Append(ChandedFurToSpots(target, oldData));
				}

				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}
			}
			//if isEnhanced to probova give a shitty cow face
			else if (isEnhanced && target.face.type != FaceType.COW_MINOTAUR)
			{
				var oldData = target.face.AsReadOnlyData();
				target.UpdateFace(FaceType.COW_MINOTAUR);
				sb.Append(ChangedFaceText(target, oldData));

				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}
			}
			//Give the player bovine horns, or increase their size, same as the minotaur
			//New horns or expanding mino horns
			if (!isPurified && Utils.Rand(3) == 0)
			{
				var oldHorns = target.horns.AsReadOnlyData();
				if (target.horns.type != HornType.BOVINE)
				{
					target.UpdateHorns(HornType.BOVINE);
					sb.Append(ChangedHornsText(target, oldHorns));

					if (++totalChanges >= changeCount)
					{
						return FinalizeTransformation(target, sb, totalChanges);
					}
				}
				else if (target.horns.type == HornType.BOVINE && target.horns.CanStrengthen && target.horns.significantHornSize < 5)
				{
					target.horns.StrengthenTransform();
					sb.Append(MadeHornsBigger(target, oldHorns));

					if (++totalChanges >= changeCount)
					{
						return FinalizeTransformation(target, sb, totalChanges);
					}
				}
			}

			//Increase the size of the player's hips, if they are not already childbearing or larger
			if (Utils.Rand(2) == 0 && target.hips.size < 15)
			{
				if (isPurified && target.hips.size < 8 || !isPurified)
				{
					var oldHips = target.hips.AsReadOnlyData();

					if(target.build.GrowHips((byte)(1 + Utils.Rand(4))) > 0)
					{
						sb.Append(WidenedHipsText(target, oldHips));
						if (++totalChanges >= changeCount)
						{
							return FinalizeTransformation(target, sb, totalChanges);
						}
					}
				}
			}
			// Remove gills
			if (Utils.Rand(4) == 0 && target.gills.type != GillType.NONE)
			{
				var oldData = target.gills.AsReadOnlyData();
				target.RestoreGills();

				sb.Append(RestoredGillsText(target, oldData));

				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}
			}

			//Increase the size of the player's ass (less likely then hips), if it is not already somewhat big
			if (Utils.Rand(2) == 0 && target.butt.size < 8 || (!isPurified && target.butt.size < 13))
			{
				var oldButt = target.butt.AsReadOnlyData();
				if (target.butt.GrowButt((byte)(1 + Utils.Rand(2))) > 0)
				{
					sb.Append(GrewButtText(target, oldButt));
					if (++totalChanges >= changeCount)
					{
						return FinalizeTransformation(target, sb, totalChanges);
					}
				}
			}
			//Nipples Turn Back:
			if (target.genitals.hasBlackNipples && Utils.Rand(3) == 0)
			{
				target.genitals.SetBlackNipples(false);
				sb.Append(RemovedBlackNippleText(target));

				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}
			}
			//Debugcunt
			if (target.hasVagina && target.vaginas.Any(x=>x.type != VaginaType.defaultValue) && Utils.Rand(3) == 0)
			{
				var oldCollection = target.genitals.allVaginas.AsReadOnlyData();
				target.vaginas.ForEach(x => target.genitals.RestoreVagina(x));
				sb.Append(RestoreAllVaginasText(target, oldCollection));

				if (++totalChanges >= changeCount)
				{
					return FinalizeTransformation(target, sb, totalChanges);
				}
			}


			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return FinalizeTransformation(target, sb, changeCount - totalChanges);
		}


		private string FinalizeTransformation(Creature target, StringBuilder sb, int changesPerformed)
		{
			if (Utils.Rand(3) == 0)
			{
				var oldData = target.femininity.AsReadOnlyData();

				if (target.femininity.ChangeFemininityToward(79, 3) != 0)
				{
					sb.Append(AdjustFemininity(target, oldData));
				}
			}

			if (Utils.Rand(3) == 0)
			{
				var delta = target.build.ChangeThicknessToward(70, 4);
				sb.Append(AdjustThickness(target, delta));
			}

			if (Utils.Rand(5) == 0)
			{
				var delta = target.build.ChangeMuscleToneToward(10, 5);

				sb.Append(AdjustTone(target, delta));
			}


			return ApplyChangesAndReturn(target, sb, changesPerformed);
		}

		protected internal override string DoTransformationFromCombat(CombatCreature target, out bool isCombatLoss, out bool isBadEnd)
		{
			//implement this as you see fit. if there is no difference between this and the above function, you can safely remove this.
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target);

		protected abstract string StrengthUpText(double delta);
		protected abstract string ToughnessUpText(double delta);
		protected abstract string SpeedDownText(double decrease);

		protected abstract string MadeFemale(Creature target, CockData preChange, BallsData oldBalls);
		protected virtual string ShrunkOneCock(Creature target, CockData preChange, bool removedCock)
		{
			return target.genitals.allCocks.GenericChangeOneCockLengthText(preChange, removedCock, true);
		}

		protected abstract string GrewFirstBreastRow(Creature target, BreastData oldBreasts);

		protected abstract string RemovedFeatheryHairText(Creature target, HairData oldHair);

		//the original text only used the first row. it may make sense to give you the full collection so you can describe that.
		protected abstract string StartedLactatingText(Creature target, BreastData preLactation);

		protected abstract string GrantQuadNippleText(Creature target, BreastCollectionData oldBreasts);

		protected abstract string BoostedLactationText(Creature target, BreastCollectionData oldBreasts, double nippleLengthDelta);

		protected abstract string GainedFeederPerk(Creature target);

		protected abstract string LoosenedTwatText(Creature target, VaginaData preLoosened);

		protected virtual string RestoredNeckText(Creature target, NeckData oldData)
		{
			return target.neck.RestoredText(oldData);
		}

		protected virtual string RestoredBackText(Creature target, BackData oldData)
		{
			return target.back.RestoredText(oldData);
		}

		protected virtual string ClearOvipositionText(Creature target)
		{
			return RemovedOvipositionTextGeneric(target);
		}

		protected virtual string ChangedTailText(Creature target, TailData oldData)
		{
			return target.tail.TransformFromText(oldData);
		}

		protected virtual string ChangedEarsText(Creature target, EarData oldEars)
		{
			return target.ears.TransformFromText(oldEars);
		}

		protected abstract string GrowTaller(Creature target, byte delta);

		protected virtual string ChangedLowerBodyText(Creature target, LowerBodyData oldLowerBody)
		{
			return target.lowerBody.TransformFromText(oldLowerBody);
		}

		protected virtual string RestoredFaceText(Creature target, FaceData oldData)
		{
			return target.face.RestoredText(oldData);
		}

		protected virtual string ChangedBodyText(Creature target, BodyData oldData)
		{
			return target.body.TransformFromText(oldData);
		}

		protected abstract string ChandedFurToSpots(Creature target, BodyData oldData);

		protected virtual string ChangedFaceText(Creature target, FaceData oldData)
		{
			return target.face.TransformFromText(oldData);
		}

		protected virtual string ChangedHornsText(Creature target, HornData oldData)
		{
			return target.horns.TransformFromText(oldData);
		}

		protected abstract string MadeHornsBigger(Creature target, HornData oldHorns);

		protected abstract string WidenedHipsText(Creature target, HipData oldHips);

		protected virtual string RestoredGillsText(Creature target, GillData oldData)
		{
			return target.gills.RestoredText(oldData);
		}

		protected virtual string RestoreAllVaginasText(Creature target, VaginaCollectionData oldCollection)
		{
			return target.genitals.allVaginas.RestoredAllVaginasGeneric(oldCollection);
		}

		protected abstract string GrewButtText(Creature target, ButtData oldButt);

		protected virtual string RemovedBlackNippleText(Creature target)
		{
			return target.genitals.allBreasts.GenericRemovedBlackNipples();
		}

		protected virtual string AdjustFemininity(Creature target, FemininityData oldData)
		{
			return target.femininity.FemininityChangedText(oldData);
		}

		protected virtual string AdjustThickness(Creature target, short delta)
		{
			return target.build.GenericAdjustThickness(delta);
		}

		protected virtual string AdjustTone(Creature target, short delta)
		{
			return target.build.GenericAdjustTone(delta);
		}

	}
}