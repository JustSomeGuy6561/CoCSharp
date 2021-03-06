﻿//KangarooTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:21:54 PM
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Perks.SpeciesPerks;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;

namespace CoC.Frontend.Transformations
{
	internal abstract class KangarooTransformations : GenericTransformationBase
	{
		protected readonly bool isEnhanced;
		protected KangarooTransformations(bool enhanced)
		{
			isEnhanced = enhanced;
		}

		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		/** Original Credits:
		 * @since March 26, 2018
		 * @author Stadler76
		 */
		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 2 }, isEnhanced ? 3 : 1);
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

			//For all of these, any text regarding the transformation should be instead abstracted out as an abstract string function. append the result of this abstract function
			//to the string builder declared above (aka sb.Append(FunctionCall(variables));) string builder is just a fancy way of telling the compiler that you'll be creating a
			//long string, piece by piece, so don't do any crazy optimizations first.

			//the initial text for starting the transformation. feel free to add additional variables to this if needed.
			sb.Append(InitialTransformationText(target));

			//Add any free changes here - these can occur even if the change count is 0. these include things such as change in stats (intelligencelligence, etc)
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

			//Used as a holding variable for biggest dicks and the like
			//****************
			//General Effects:
			//****************
			//-Int less than 10
			if (target is IExtendedCreature extendedCreature && !extendedCreature.extendedData.resistsTFBadEnds && target.intelligence < 10)
			{
				if (target.intelligence < 8 && Species.KANGAROO.Score(target) >= 5)
				{
					isBadEnd = true;
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//-Speed to 70
			if (target.relativeSpeed < 70 && Utils.Rand(3) == 0)
			{
				//2 points up if below 40!
				if (target.relativeSpeed < 40)
				{
					target.ChangeSpeed(1);
				}

				target.ChangeSpeed(1);

			}
			//-Int to 10
			if (target.intelligence > 2 && Utils.Rand(3) == 0)
			{
				//Gain dumb (smart!)
				//gain dumb (30-10 int):

				//gain dumb (10-1 int):
				target.ChangeIntelligence(-1);
			}

			//****************
			//Appearance Effects:
			//****************
			//-Hip widening funtimes
			if (Utils.Rand(4) == 0 && target.hips.size < 40)
			{
				target.hips.GrowHips();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Restore arms to become human arms again
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
			//-Remove feathery hair
			if (RemoveFeatheryHair(target))
			{
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Remove odd eyes
			if (Utils.Rand(5) == 0 && !target.eyes.isDefault)
			{
				EyeData oldData = target.eyes.AsReadOnlyData();
				target.RestoreEyes();
				sb.Append(RestoredEyesText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//****************
			//Sexual:
			//****************
			//-Shrink balls down to reasonable size (3?)
			if (target.balls.size >= 4 && Utils.Rand(2) == 0)
			{
				target.balls.ShrinkBalls();
				target.genitals.IncreaseCumMultiplier();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Shorten clits to reasonable size
			//MOD NOTE: lets do one at a time. i generally do all because that's the easiest way to port it, but fuck it.
			Vagina largestClit = target.genitals.LargestVaginaByClitSize();

			if (target.hasVagina && largestClit.clit.length >= 4 && Utils.Rand(5) == 0)
			{
				largestClit.ShrinkClit(largestClit.clit.length / 2);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Find biggest dick!
			Cock biggestCock = target.genitals.LongestCock();
			//-Shrink dicks down to 8\" max.
			if (target.hasCock && biggestCock.length >= 16 && Utils.Rand(5) == 0)
			{
				biggestCock.DecreaseLength(biggestCock.length / 2);
				biggestCock.DecreaseThickness(2 * biggestCock.length / 3);

				if (biggestCock.girth * 6 > biggestCock.length)
				{
					biggestCock.DecreaseThickness(.4);
				}

				else if (biggestCock.girth * 8 > biggestCock.length)
				{
					biggestCock.DecreaseThickness(.2);
				}

				if (biggestCock.girth < .5)
				{
					biggestCock.SetGirth(0.5);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//COCK TF!
			if (target.cocks.Any(x => x.type != CockType.KANGAROO) && Utils.Rand(isEnhanced ? 2 : 3) == 0)
			{
				Cock notRoo = target.cocks.First(x => x.type != CockType.KANGAROO);
				target.genitals.UpdateCock(notRoo, CockType.KANGAROO);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
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
			//****************
			//Big Kanga Morphs
			//type 1 ignores normal restrictions
			//****************
			//-Face (Req: Fur + Feet)
			if (target.face.type != FaceType.KANGAROO && ((target.body.IsFurBodyType() && target.lowerBody.type == LowerBodyType.KANGAROO) || isEnhanced) && Utils.Rand(4) == 0)
			{
				FaceData oldData = target.face.AsReadOnlyData();
				target.UpdateFace(FaceType.KANGAROO);
				sb.Append(UpdateFaceText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}

			}
			//-Fur (Req: Footsies)
			if (!target.body.IsFurBodyType() && (target.lowerBody.type == LowerBodyType.KANGAROO || isEnhanced) && Utils.Rand(4) == 0)
			{
				BodyData oldData = target.body.AsReadOnlyData();
				target.UpdateBody(BodyType.SIMPLE_FUR, new FurColor(HairFurColors.BROWN));
				sb.Append(UpdateBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Roo footsies (Req: Tail)
			if (target.lowerBody.type != LowerBodyType.KANGAROO && (isEnhanced || target.tail.type == TailType.KANGAROO) && Utils.Rand(4) == 0)
			{
				LowerBodyData oldData = target.lowerBody.AsReadOnlyData();
				target.UpdateLowerBody(LowerBodyType.KANGAROO);
				sb.Append(UpdateLowerBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Roo tail (Req: Ears)
			if (target.tail.type != TailType.KANGAROO && Utils.Rand(4) == 0 && (isEnhanced || target.ears.type == EarType.KANGAROO))
			{
				TailData oldData = target.tail.AsReadOnlyData();
				target.UpdateTail(TailType.KANGAROO);
				sb.Append(UpdateTailText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Roo ears
			if (target.ears.type != EarType.KANGAROO && Utils.Rand(4) == 0)
			{
				EarData oldData = target.ears.AsReadOnlyData();
				target.UpdateEars(EarType.KANGAROO);
				sb.Append(UpdateEarsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//UBEROOOO
			//kangaroo perk: - any liquid or food intake will accelerate a pregnancy, but it will not progress otherwise
			if (target.womb.canObtainDiapause && Species.KANGAROO.Score(target) > 4 && Utils.Rand(4) == 0 && target.hasVagina)
			{
				target.womb.EnableDiapause();

				//Perk name and description:
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
				//trigger effect: Your body reacts to the influx of nutrition, accelerating your pregnancy. Your belly bulges outward slightly.
			}
			// Remove gills
			if (Utils.Rand(4) == 0 && !target.gills.isDefault)
			{
				target.RestoreGills();
			}
			if (remainingChanges == changeCount)
			{
				(target as CombatCreature)?.RecoverFatigue(40);
			}


			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		protected virtual string ClearOvipositionText(Creature target)
{
return RemovedOvipositionTextGeneric(target);
}

		protected virtual string UpdateFaceText(Creature target, FaceData oldData)
		{
			return target.face.TransformFromText(oldData);
		}

		protected virtual string UpdateBodyText(Creature target, BodyData oldData)
		{
			return target.body.TransformFromText(oldData);
		}

		protected virtual string UpdateLowerBodyText(Creature target, LowerBodyData oldData)
		{
			return target.lowerBody.TransformFromText(oldData);
		}

		protected virtual string UpdateTailText(Creature target, TailData oldTail)
		{
			return target.tail.TransformFromText(oldTail);
		}
		protected virtual string UpdateEarsText(Creature target, EarData oldData)
		{
			return target.ears.TransformFromText(oldData);
		}
		protected virtual string RestoredArmsText(Creature target, ArmData oldData)
		{
			return target.arms.RestoredText(oldData);
		}

		protected virtual string RestoredEyesText(Creature target, EyeData oldData)
		{
			return target.eyes.RestoredText(oldData);
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