//DragonTransformations.cs
//Description:
//Author: JustSomeGuy
//1/18/2020 2:00:58 PM
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Perks;
using CoC.Frontend.Perks.SpeciesPerks;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;

namespace CoC.Frontend.Transformations
{
	internal abstract class DragonTransformations : GenericTransformationBase
	{
		//full strength unlocks additional tfs if true. for reference, ember's blood is true, drake's heart is false.
		protected readonly bool fullStrength;

		//this is probably set by retrieving the value from Ember, but for the sake of clean code here, i'm just going to make you pass that value in.
		protected readonly bool draconicFace;
		//same as above.
		protected readonly bool draconicBackIsMane;

		//gender of the source (if applicable). this gender will affect whether or not the consumer will enter rut or heat; the source must have the opposite genitalia
		//of the consumer. this only applies when the full strength flag is set; when it is not set, the consumer will enter heat or rut, regardless.
		protected readonly Gender sourceGender;

		protected DragonTransformations(bool isFullStrength, bool allowDraconicFace, bool backUsesMane, Gender genderOfSource = Gender.HERM)
		{
			fullStrength = isFullStrength;
			draconicFace = allowDraconicFace;

			draconicBackIsMane = backUsesMane;

			sourceGender = genderOfSource;
		}

		private bool hyperHappy => HyperHappySettings.isEnabled;

		//technically, this could also work with a reptilian body considering the rest of the checks, but whatever.
		private bool BacksideDraconicEnough(Creature target)
		{
			return target.wings.type == WingType.DRACONIC && target.wings.isLarge && target.body.type == BodyType.DRAGON && target.tail.IsReptilian()
				&& target.arms.IsReptilian() && target.lowerBody.IsReptilian();
		}

		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			int changeCount = GenerateChangeCount(target, new int[] { 2, 2 });
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

			sb.Append(InitialTransformationText(target));

			//No free changes, as of current implementation.

			//this will handle the edge case where the change count starts out as 0.
			if (remainingChanges <= 0)
			{
				return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Transformation Changes

			//Gain Dragon Dick
			if (target.genitals.CountCocksOfType(CockType.DRAGON) < target.cocks.Count && Utils.Rand(3) == 0)
			{
				int temp = target.cocks.Count;

				//find all the non-dragon cocks, and choose one randomly. the beauty of linq and useful helper functions.
				Cock toChange = Utils.RandomChoice(target.cocks.Where(x => x.type != CockType.DRAGON).ToArray());
				CockData oldData = toChange.AsReadOnlyData();
				if (target.genitals.UpdateCockWithKnot(toChange, CockType.DRAGON, 1.3f))
				{
					//lose lust if sens>=50, gain lust if else
					target.IncreaseCreatureStats(sens: 10, lus: 10);

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

			}
			//-Remove 1 extra breast row
			if (target.breasts.Count > 1 && Utils.Rand(3) == 0 && !hyperHappy)
			{
				target.genitals.RemoveBreastRows(1);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//no effect on oviposition. thus removed.

			//Gain Dragon Head
			if (Utils.Rand(3) == 0 && target.face.type != FaceType.DRAGON && draconicFace)
			{
				//OutputText("\n\nYou scream as your face is suddenly twisted; your facial bones begin rearranging themselves under your skin, restructuring into a long, narrow muzzle. Spikes of agony rip through your jaws as your teeth are brutally forced from your gums, giving you new rows of fangs - long, narrow and sharp. Your jawline begins to sprout strange growths; small spikes grow along the underside of your muzzle, giving you an increasingly inhuman visage.\n\nFinally, the pain dies down, and you look for a convenient puddle to examine your changed appearance.\n\nYour head has turned into a reptilian muzzle, with small barbs on the underside of the jaw. <b>You now have a dragon's face.</b>"));
				FaceData oldData = target.face.AsReadOnlyData();
				target.UpdateFace(FaceType.DRAGON);
				sb.Append(UpdateFaceText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Existing horns become draconic, max of 4, max length of 1'
			if (target.horns.type != HornType.DRACONIC || target.horns.CanStrengthen && Utils.Rand(5) == 0)
			{
				if (target.UpdateOrStrengthenHorns(HornType.DRACONIC))
				{

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}
			//Gain Dragon Ears
			if (Utils.Rand(3) == 0 && target.ears.type != EarType.DRAGON)
			{
				target.UpdateEars(EarType.DRAGON);
				//OutputText("\n\nA prickling sensation suddenly fills your ears; unpleasant, but hardly painful. It grows and grows until you can't stand it any more, and reach up to scratch at them. To your surprise, you find them melting away like overheated candles. You panic as they fade into nothingness, leaving you momentarily deaf and dazed, stumbling around in confusion. Then, all of a sudden, hearing returns to you. Gratefully investigating, you find you now have a pair of reptilian ear-holes, one on either side of your head. A sudden pain strikes your temples, and you feel bony spikes bursting through the sides of your head, three on either side, which are quickly sheathed in folds of skin to resemble fins. With a little patience, you begin to adjust these fins just like ears to aid your hearing. <b>You now have dragon ears!</b>"));
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Gain Dragon Tongue
			if (Utils.Rand(3) == 0 && target.tongue.type != TongueType.DRACONIC)
			{
				//OutputText("\n\nYour tongue suddenly falls out of your mouth and begins undulating as it grows longer. For a moment it swings wildly, completely out of control; but then settles down and you find you can control it at will, almost like a limb. You're able to stretch it to nearly 4 feet and retract it back into your mouth to the point it looks like a normal human tongue. <b>You now have a draconic tongue.</b>"));
				TongueData oldData = target.tongue.AsReadOnlyData();
				target.UpdateTongue(TongueType.DRACONIC);
				sb.Append(UpdateTongueText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
				//Note: This type of tongue should be eligible for all things you can do with demon tongue... Dunno if it's best attaching a boolean just to change the description or creating a whole new tongue type.
			}
			//(Pending Tongue Masturbation Variants; if we ever get around to doing that.)
			//Gain Dragon Scales
			if (target.body.type != BodyType.DRAGON && Utils.Rand(3) == 0)
			{
				BodyData oldData = target.body.AsReadOnlyData();
				target.UpdateBody(BodyType.DRAGON);
				sb.Append(UpdateBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//<mod name="Reptile eyes" author="Stadler76">
			//Gain Dragon Eyes
			if (target.eyes.type != EyeType.DRAGON && target.body.type == BodyType.DRAGON && target.ears.type == EarType.DRAGON && target.horns.type == HornType.DRACONIC && Utils.Rand(4) == 0)
			{
				EyeData oldData = target.eyes.AsReadOnlyData();
				target.UpdateEyes(EyeType.DRAGON);
				sb.Append(UpdateEyesText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//</mod>
			//Gain Dragon Legs
			if (target.lowerBody.type != LowerBodyType.DRAGON && Utils.Rand(3) == 0)
			{
				LowerBodyData oldData = target.lowerBody.AsReadOnlyData();
				target.UpdateLowerBody(LowerBodyType.DRAGON);
				sb.Append(UpdateLowerBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Gain Dragon Tail
			if (target.tail.type != TailType.DRACONIC && Utils.Rand(3) == 0)
			{
				TailData oldData = target.tail.AsReadOnlyData();
				target.UpdateTail(TailType.DRACONIC);
				sb.Append(UpdateTailText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Grow Dragon Wings or Enlarge small ones.
			if ((target.wings.type != WingType.DRACONIC || !target.wings.isLarge) && Utils.Rand(3) == 0)
			{
				//set to draconic wings. this will fail if we already have draconic wings.
				if (target.UpdateWings(WingType.DRACONIC))
				{
					//do text, maybe? idk.
				}
				//it failed, which means we already have draconic wings.
				else
				{
					target.wings.GrowLarge();
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			// <mod name="BodyParts.RearBody" author="Stadler76">
			//Gain Dragonic back
			//you need to have a reptilian arm, leg, and tail type, and, if we are allowing draconic face tfs, you must also have a draconic neck.
			//additionally, you must have a decent dragon score, which basically means some of those reptilian body parts must be draconic.
			if (fullStrength && !target.back.IsDraconic() && (target.neck.type == NeckType.DRACONIC || !draconicFace)
				&& BacksideDraconicEnough(target) && Species.DRAGON.Score(target) >= 4 && Utils.Rand(3) == 0)
			{
				if (draconicBackIsMane)
				{
					target.UpdateBack(BackType.DRACONIC_MANE);
				}
				else
				{
					target.UpdateBack(BackType.DRACONIC_SPIKES);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			// </mod>
			//Restore non dragon neck
			if (target.neck.type != NeckType.DRACONIC && !target.neck.isDefault && Utils.Rand(4) == 0)
			{
				NeckData oldData = target.neck.AsReadOnlyData();
				target.RestoreNeck();
				sb.Append(RestoredNeckText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Gain Dragon Neck

			//mod of a mod (again). it now handles other necks a little cleaner.
			//If you are considered a dragon-morph and if your backside is dragon-ish enough, your neck is eager to allow you to take a look at it, right? ;-)
			if (fullStrength && (target.neck.type != NeckType.DRACONIC || target.neck.canGrowNeck) && Species.DRAGON.Score(target) >= 6 &&
				BacksideDraconicEnough(target) && target.face.type == FaceType.DRAGON)
			{

				if (target.neck.type != NeckType.DRACONIC)
				{
					target.UpdateNeck(NeckType.DRACONIC);
				}
				else
				{
					//4-8
					byte nlChange = (byte)(4 + Utils.Rand(5));

					// Note: hasNormalNeck checks the length, not the type!
					target.neck.GrowNeck(nlChange);
				}

				//note: draconic neck now positions behind the head regardless of length, though it probably wouldn't be noticable until neck is sufficiently long.
				//feel free to denote that when it hits full length here.

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//predator arms. (a mod of a mod. woo!. original mod: Stadler76.)
			//Gain Dragon Arms (Derived from ArmType.SALAMANDER)

			//requires draconic body, non-draconic arms, and either predator arms or draconic legs. let the arm update text handle the transformation (or do it manually, if you want)
			if (target.arms.type != ArmType.DRAGON && target.body.type == BodyType.DRAGON && (target.arms.type.IsPredatorArms() || target.lowerBody.type == LowerBodyType.DRAGON)
				&& Utils.Rand(3) == 0)
			{
				target.UpdateArms(ArmType.DRAGON);

				//explain the change.
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			// </mod>
			//Get Dragon Breath (Tainted version)
			//Can only be obtained if you are considered a dragon-morph, once you do get it though, it won't just go away even if you aren't a dragon-morph anymore.

			if (Species.DRAGON.Score(target) >= 4 && !target.HasPerk<DragonFire>())
			{
				target.perks.AddPerk<DragonFire>();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//heat or rut, depending on gender. note that for full strength tfs, this uses the source gender.
			if (Species.DRAGON.Score(target) >= 4 && Utils.Rand(3) == 0 && target.gender > 0 && (!fullStrength || target.gender.CanHaveSexWith(sourceGender)))
			{
				if (target.hasCock && (sourceGender.HasFlag(Gender.FEMALE) || !fullStrength) && (!target.hasVagina || Utils.RandBool()))
				{ //If hermaphrodite, the chance is 50/50.
					target.GoIntoRut();
				}
				else
				{
					target.GoIntoHeat();
				}
			}

			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		protected virtual string UpdateFaceText(Creature target, FaceData oldData)
{
return target.face.TransformFromText(oldData);
}

		protected virtual string UpdateTongueText(Creature target, TongueData oldData)
{
return target.tongue.TransformFromText(oldData);
}

		protected virtual string UpdateBodyText(Creature target, BodyData oldData)
{
return target.body.TransformFromText(oldData);
}

		protected virtual string UpdateEyesText(Creature target, EyeData oldData)
{
return target.eyes.TransformFromText(oldData);
}

		protected virtual string UpdateLowerBodyText(Creature target, LowerBodyData oldData)
{
return target.lowerBody.TransformFromText(oldData);
}

		protected virtual string UpdateTailText(Creature target, TailData oldTail)
{
return target.tail.TransformFromText(oldTail);
}
		protected virtual string RestoredNeckText(Creature target, NeckData oldData)
{
return target.neck.RestoredText(oldData);
}


		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target);
	}
}