//DryadTransformations.cs
//Description:
//Author: JustSomeGuy
//2/10/2020 2:47:30 PM
using System;
using System.Collections;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;

namespace CoC.Frontend.Transformations
{
	//This one was a bitch to implement. the idea (i think, at least) was non-sequential RNG, as opposed to our standard sequential RNG. this is accomplished by randomly
	//selecting a function callback. the text is written using a StringBuilder, because i can pass that around and not need to worry about using the 'ref' keyword.

	internal abstract class DryadTransformations : GenericTransformationBase
	{

		//a collection of simple effects. the callback will append its results (if applicable) to the stringbuilder, and return a boolean telling us if it did anything.
		protected readonly Func<Creature, StringBuilder, bool>[] simpleEffects;
		//a collection of full effects. the callback will append its results (if applicable) to the stringbuilder, and return a boolean telling us if it did anything.
		protected readonly Func<Creature, StringBuilder, bool>[] fullEffects;

		protected readonly bool allowsMultipleTransformations;

		protected DryadTransformations() : this(true)
		{

		}

		protected DryadTransformations(bool allowMultipleTransformations)
		{
			allowsMultipleTransformations = allowMultipleTransformations;

			simpleEffects = new Func<Creature, StringBuilder, bool>[]
			{
				ChangeHips,
				ChangeButt,
				ChangeCock,
				ChangeBreasts,
				ChangeFemininity,
			};

			fullEffects = new Func<Creature, StringBuilder, bool>[]
			{
				ChangeEars,
				ChangeBody,
				ChangeLegs,
				ChangeArms,
				ChangeFace,
				ChangeHair,
			};
		}

		protected bool ChangeHips(Creature target, StringBuilder content)
		{
			HipData hipData = target.hips.AsReadOnlyData();
			bool changed = false;
			if (target.hips.size < 5)
			{
				changed = target.hips.GrowHips(1) != 0;
			}
			else if (target.hips.size > 5)
			{
				changed = target.hips.ShrinkHips(1) != 0;
			}

			if (changed)
			{
				content.Append(HipChangeText(target, hipData));
				return true;
			}
			else
			{
				return false;
			}
		}

		protected bool ChangeButt(Creature target, StringBuilder sb)
		{
			ButtData buttData = target.butt.AsReadOnlyData();
			bool changed = false;
			if (target.butt.size < 5)
			{
				changed = target.butt.GrowButt(1) != 0;
			}
			else if (target.butt.size > 5)
			{
				changed = target.butt.ShrinkButt(1) != 0;
			}

			if (changed)
			{
				sb.Append(ButtChangeText(target, buttData));
				return true;
			}
			else
			{
				return false;
			}
		}

		protected bool ChangeCock(Creature target, StringBuilder sb)
		{
			if (HyperHappySettings.isEnabled)
			{
				return false;
			}

			//find the largest cock. shrink it. if it gets small enough to remove it, do so. if it's the only cock the creature has, grow a vagina in its place.
			if (target.hasCock)
			{
				GenitalsData oldGenitals = target.genitals.AsReadOnlyData();
				Cock largest = target.genitals.LongestCock();

				if (largest.DecreaseLengthAndCheckIfNeedsRemoval(Utils.Rand(3) + 1))
				{
					target.genitals.RemoveCock(largest);

					if (!target.hasCock && !target.hasVagina)
					{
						target.AddVagina(.25);
						target.IncreaseCorruption();

						BallsData oldBalls = target.balls.AsReadOnlyData();
						target.balls.RemoveAllBalls();
					}
				}

				sb.Append(CockChangedText(target, oldGenitals, largest.cockIndex));
				return true;
			}
			return false;
		}

		protected bool ChangeBreasts(Creature target, StringBuilder sb)
		{
			bool changed = false;

			BreastCollectionData oldData = target.genitals.allBreasts.AsReadOnlyData();

			if (target.breasts.Count > 1)
			{
				changed = target.genitals.RemoveExtraBreastRows() > 0;
			}

			CupSize targetSize = EnumHelper.Max(target.genitals.smallestPossibleCupSize, CupSize.D);

			if (target.breasts[0].cupSize != targetSize)
			{
				changed |= target.breasts[0].SetCupSize(targetSize) != 0;
			}

			if (changed)
			{
				sb.Append(BreastsChangedText(target, oldData));
				return true;
			}

			return false;
		}

		protected bool ChangeFemininity(Creature target, StringBuilder sb)
		{
			FemininityData oldFem = target.femininity.AsReadOnlyData();
			if (target.femininity.ChangeFemininityToward(70, 2) != 0)
			{
				sb.Append(FemininityChangedText(target, oldFem));
				return true;
			}
			return false;
		}

		protected bool ChangeEars(Creature target, StringBuilder sb)
		{
			EarData oldData = target.ears.AsReadOnlyData();
			if (target.UpdateEars(EarType.ELFIN))
			{
				sb.Append(ChangeEarText(target, oldData));
				return true;
			}
			return false;
		}

		protected bool ChangeBody(Creature target, StringBuilder sb)
		{
			BodyData oldBody = target.body.AsReadOnlyData();
			bool changed = false;
			if (!target.body.isDefault && target.body.type != BodyType.WOODEN)
			{
				changed = target.RestoreBody();
			}
			else if (target.body.type != BodyType.WOODEN)
			{
				changed = target.UpdateBody(BodyType.WOODEN);
			}

			if (changed)
			{
				sb.Append(ChangeSkinText(target, oldBody));
			}

			return changed;
		}

		protected bool ChangeLegs(Creature target, StringBuilder sb)
		{
			LowerBodyData oldLowerBody = target.lowerBody.AsReadOnlyData();
			if (target.RestoreLowerBody())
			{
				sb.Append(RestoreLegsText(target, oldLowerBody));
				return true;
			}
			return false;
		}

		protected bool ChangeArms(Creature target, StringBuilder sb)
		{
			ArmData oldArms = target.arms.AsReadOnlyData();
			if (target.RestoreArms())
			{
				sb.Append(RestoredArmsText(target, oldArms));
				return true;
			}
			return false;
		}

		protected bool ChangeFace(Creature target, StringBuilder sb)
		{
			FaceData oldFace = target.face.AsReadOnlyData();
			if (target.RestoreFace())
			{
				sb.Append(RestoredFaceText(target, oldFace));
				return true;
			}
			return false;
		}

		protected bool ChangeHair(Creature target, StringBuilder sb)
		{
			HairData oldHair = target.hair.AsReadOnlyData();
			if (target.UpdateHair(HairType.LEAF))
			{
				//silently start hair growth again.
				target.hair.ResumeNaturalGrowth();
				sb.Append(ChangedHairText(target, oldHair));
				return true;
			}

			return false;
		}

		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		protected bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;
			StringBuilder sb = new StringBuilder();

			sb.Append(InitialTransformationText(target));

			//dryad is weird, because it's not written like literally every other tf out there: it may only let one change occur at a time,
			//and there are no requirements for a change to occur, unlike the stacking mechanic most other tfs have. additionally, the effects
			//are randomized instead of sequential. finally, it will not check to see if it can do a change before attempting it, but instead try
			//it, then handle any flavor text according to whether or not it did something.
			//So, i've come up with what i think is the best way to handle this:

			//we randomly select a number of simple changes to run. this starts at 0, but is affected by creature perks and two rolls.
			//if we are only allowing one change, this value is 1.
			int simpleChangeCount = allowsMultipleTransformations ? GenerateChangeCount(target, new int[] { 3, 4 }, 0, 1) : 1;
			int remainingChanges = simpleChangeCount;

			//then, we randomly select a simple change and do it, and decrease our remaining simple changes. we repeat this process until the remaining simple changes is 0.
			//any repeats that we hit are ignored, as are any simple changes that cannot occur because the creature already has the desired value.

			BitArray noRepeats = new BitArray(simpleEffects.Length);

			for (int x = 0; x < simpleChangeCount; x++)
			{
				int rand = Utils.Rand(simpleEffects.Length);

				if (!noRepeats[rand])
				{
					noRepeats[rand] = true;

					if (simpleEffects[rand](target, sb) == true)
					{
						remainingChanges--;
					}
				}
			}


			//if we have done any simple changes and we don't allow multiple tfs, immediately exit.
			if (!allowsMultipleTransformations && remainingChanges != simpleChangeCount)
			{
				return ApplyChangesAndReturn(target, sb, 0);
			}

			//otherwise, we go on to the complicated effects. we will carry over any simple changes that were ignored, up to 2, using the following rules:
			//if we don't allow multiple transformations or didn't ignore any simple tfs, don't carry anything over.
			//if we succeeded in doing any simple tf but have more we could have done, carry over 1.
			//if we failed to do any simple tfs, carry over 2 (or 1, if we only could have done 1)

			//the number of full changes to do. starts at 1, could go up to 3 if no simple changes occured.
			int fullChangeCount = 1;

			//if we've done one or no changes, and would have done more if possible.
			if (allowsMultipleTransformations && remainingChanges > 0 && remainingChanges + 1 >= simpleChangeCount)
			{
				//no simple changes, and we were attempting to do 2 or more.
				if (remainingChanges == simpleChangeCount && simpleChangeCount >= 2)
				{
					fullChangeCount += 2;
				}
				//we did at least one. only carryover 1.
				else
				{
					fullChangeCount += 1;
				}
			}

			//reset our change count remaining. remember, simple tfs are free.
			remainingChanges = fullChangeCount;

			//then repeat the process with RNG again, but with full tfs.
			noRepeats = new BitArray(fullEffects.Length);

			for (int x = 0; x < fullChangeCount; x++)
			{
				int rand = Utils.Rand(fullEffects.Length);

				if (!noRepeats[rand])
				{
					noRepeats[rand] = true;

					if (fullEffects[rand](target, sb) == true)
					{
						remainingChanges--;
					}
				}
			}

			//apply the changes and return.
			return ApplyChangesAndReturn(target, sb, simpleChangeCount - remainingChanges);
		}

		protected abstract string InitialTransformationText(Creature target);

		protected abstract string HipChangeText(Creature target, HipData oldHips);

		protected abstract string ButtChangeText(Creature target, ButtData oldButt);

		protected abstract string CockChangedText(Creature target, GenitalsData oldGenitalData, int changedCock);

		protected abstract string BreastsChangedText(Creature target, BreastCollectionData oldData);

		protected abstract string FemininityChangedText(Creature target, FemininityData oldFem);

		protected virtual string ChangeEarText(Creature target, EarData oldData)
		{
			return target.ears.TransformFromText(oldData);
		}

		protected virtual string ChangeSkinText(Creature target, BodyData oldBody)
		{
			return target.body.TransformFromText(oldBody);
		}

		protected virtual string RestoreLegsText(Creature target, LowerBodyData oldLowerBody)
		{
			return target.lowerBody.RestoredText(oldLowerBody);
		}

		protected virtual string RestoredArmsText(Creature creature, ArmData oldArms)
		{
			return creature.arms.RestoredText(oldArms);
		}

		protected virtual string RestoredFaceText(Creature target, FaceData oldFace)
		{
			return target.face.RestoredText(oldFace);
		}

		protected virtual string ChangedHairText(Creature target, HairData oldHair)
		{
			return target.hair.TransformFromText(oldHair);
		}
	}
}