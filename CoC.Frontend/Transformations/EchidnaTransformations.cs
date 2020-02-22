using System.Linq;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Perks.Endowment;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;
using CoC.Frontend.StatusEffect;
using CoC.Frontend.UI;

namespace CoC.Frontend.Transformations
{

	internal abstract class EchidnaTransformations : GenericTransformationBase
	{
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			int changeCount = GenerateChangeCount(target, new int[] { 2, 2 });
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

			//Zero Cost/Stat changes.

			//None at the moment

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

			// Normal TFs
			//------------
			if (Utils.Rand(4) == 0 && target.hair.type != HairType.NORMAL && target.hair.type != HairType.NO_HAIR && target.hair.type != HairType.QUILL)
			{
				//sb.Append("\n\nYour scalp feels really strange, but the sensation is brief. You feel your hair, and you immediately notice the change. <b>It would seem that your hair is normal again!</b>");
				HairData oldData = target.hair.AsReadOnlyData();
				target.UpdateHair(HairType.NORMAL);
				sb.Append(UpdateHairText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(4) == 0 && target.arms.type == ArmType.HARPY)
			{
				//sb.Append("\n\nYour arm feathers fall out completely, <b>leaving only the " + target.skinFurScales() + " underneath.</b>");
				ArmData oldData = target.arms.AsReadOnlyData();
				target.RestoreArms();
				sb.Append(RestoredArmsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Remove gills
			if (Utils.Rand(3) == 0 && target.gills.type != GillType.NONE)
			{
				GillData oldData = target.gills.AsReadOnlyData();
				target.RestoreGills();
				sb.Append(RestoredGillsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//restore spider eyes.
			if (Utils.Rand(3) == 0 && target.eyes.type == EyeType.SPIDER)
			{
				//sb.Append("\n\nYour eyes start throbbing painfully, your sight in them eventually going dark. You touch your head to inspect your eyes, only to find out that they have changed. <b>You have human eyes now!</b>");
				EyeData oldData = target.eyes.AsReadOnlyData();
				target.UpdateEyes(EyeType.HUMAN);
				sb.Append(UpdateEyesText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(3) == 0 && target.genitals.hasQuadNipples)
			{
				//sb.Append("\n\nA tightness arises in your nipples as three out of four on each breast recede completely, the leftover nipples migrating to the middle of your breasts. <b>You are left with only one nipple on each breast.</b>");
				target.genitals.SetQuadNipples(false);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			// Main TFs
			//------------
			//Change to fur
			if (Utils.Rand(3) == 0 && !target.body.IsFurBodyType())
			{
				target.UpdateBody(BodyType.SIMPLE_FUR, new FurColor(HairFurColors.BROWN));
				//sb.Append("\n\nYou shiver, feeling a bit cold. Just as you begin to wish for something to cover up with, it seems your request is granted; <b>fur begins to grow all over your body!</b> You tug at the tufts in alarm, but they're firmly rooted and... actually pretty soft. Huh. ");

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Gain Echidna ears
			if (Utils.Rand(3) == 0 && target.ears.type != EarType.ECHIDNA)
			{
				target.UpdateEars(EarType.ECHIDNA);
				//sb.Append("\n\n");
				//sb.Append(" <b>You now have echidna ears!</b>");

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Gain Echidna tail
			if (Utils.Rand(3) == 0 && target.ears.type == EarType.ECHIDNA && target.tail.type != TailType.ECHIDNA)
			{
				target.UpdateTail(TailType.ECHIDNA);
				//sb.Append("\n\n");
				//sb.Append(" <b>You now have an echidna tail!</b>");

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Gain Echidna legs
			if (Utils.Rand(3) == 0 && target.ears.type == EarType.ECHIDNA && target.tail.type == TailType.ECHIDNA && target.lowerBody.type != LowerBodyType.ECHIDNA)
			{
				target.UpdateLowerBody(LowerBodyType.ECHIDNA);
				//sb.Append("\n\n");
				//sb.Append(" <b>They actually look like the feet of an echidna!</b>");

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Convert one existing cock to Echidna
			if (Utils.Rand(3) == 0 && target.hasCock && target.genitals.CountCocksOfType(CockType.ECHIDNA) < target.cocks.Count)
			{
				int firstNonEchidna = target.cocks.FirstIndexOf(x => x.type != CockType.ECHIDNA);
				target.genitals.UpdateCock(firstNonEchidna, CockType.ECHIDNA);

				//sb.Append("\n\n");
				//if (target.cocks.Count == 1) //sb.Append("Your [cock] suddenly becomes rock hard out of nowhere. You " + target.clothedOrNakedLower("pull it out from your [armor], right in the middle of the food tent, watching", "watch") + " as it begins to shift and change. It becomes pink in color, and you feel a pinch at the head as it splits to become four heads. " + (target.hasSheath() ? "" : "The transformation finishes off with a fleshy sheath forming at the base.") + " It ejaculates before going limp, retreating into your sheath.");
				//else //sb.Append("One of your penises begins to feel strange. You " + target.clothedOrNakedLower("pull the offending cock out from your [armor], right in the middle of the food tent, watching", "watch") + " as it begins to shift and change. It becomes pink in color, and you feel a pinch at the head as it splits to become four heads. " + (target.hasSheath() ? "" : "The transformation finishes off with a fleshy sheath forming at the base.") + " It ejaculates before going limp, retreating into your sheath.");
				//sb.Append(" <b>You now have an echidna penis!</b>");

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Gain Echidna tongue
			if (Utils.Rand(3) == 0 && Species.ECHIDNA.Score(target) >= 2 && target.tongue.type != TongueType.ECHIDNA)
			{
				target.UpdateTongue(TongueType.ECHIDNA);
				//sb.Append("\n\nYou feel an uncomfortable pressure in your tongue as it begins to shift and change. Within moments, you are able to behold your long, thin tongue. It has to be at least a foot long. <b>You now have an echidna tongue!</b>");

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Gain quill hair
			if (Utils.Rand(4) == 0 && ((target.hair.type == HairType.NORMAL && !target.hair.isGrowing) || target.hair.type == HairType.NO_HAIR))
			{
				target.UpdateHair(HairType.QUILL);
				//sb.Append("\n\nYour scalp begins to tingle as your hair falls out in clumps, leaving you with a bald head. You aren't bald for long, though. An uncomfortable pressure racks the entirety of your scalp as hard quills begin to sprout from your hair pores. Their growth stops as they reach shoulder length. <b>You now have quills in place of your hair!</b>");

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Gain Echidna face if you have the right conditions.
			if (Utils.Rand(4) == 0 && target.body.isFurry && target.ears.type == EarType.ECHIDNA && target.tail.type == TailType.ECHIDNA && target.tongue.type == TongueType.ECHIDNA)
			{
				target.UpdateFace(FaceType.ECHIDNA);
				//sb.Append("You groan loudly as the bones in your face begin to reshape and rearrange. Most notable, you feel your mouth lengthening into a long, thin snout. <b>You now have an echidna face!</b>");

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			// Other Changes
			//------------
			//Hair stops growing
			if (Utils.Rand(4) == 0 && Species.ECHIDNA.Score(target) >= 2 && target.hair.isGrowing && !target.hair.growthArtificallyDisabled)
			{
				if (target.hair.StopNaturalGrowth())
				{
					//sb.Append("\n\nYour scalp tingles oddly. In a panic, you reach up to your " + target.hairDescript() + ", but thankfully it appears unchanged.\n");
					//sb.Append("(<b>Your hair has stopped growing.</b>)");
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}

			//Sexual changes
			if (Utils.Rand(3) == 0 && target.hasCock && target.genitals.cumMultiplierTrue < 25)
			{
				double temp = 1 + Utils.Rand(5);
				//really not a fan of this hard coded check b/c its not future proof, but it seems to be limited in scope enough i guess i can allow it.
				//Lots of cum raises cum multiplier cap to 2 instead of 1.5
				if (target.HasPerk<MessyOrgasms>())
				{
					temp += 1 + Utils.Rand(10);
				}
				temp *= 0.1f;

				target.genitals.IncreaseCumMultiplier(temp);
				//Flavor text
				//if (target.balls.count == 0) //sb.Append("\n\nYou feel a churning inside your gut as something inside you changes.");
				//else //if (target.balls.count > 0) //sb.Append("\n\nYou feel a churning in your " + target.ballsDescriptLight() + ". It quickly settles, leaving them feeling somewhat more dense.");
				//sb.Append(" A bit of milky pre dribbles from your " + target.multiCockDescriptLight() + ", pushed out by the change.");

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(3) == 0 && target.gender == Gender.MALE && target.genitals.AverageCupSize() > target.genitals.smallestPossibleMaleCupSize && !hyperHappy)
			{
				//sb.Append("\n\nYou cup your tits as they begin to tingle strangely. You can actually feel them getting smaller in your hands!");
				foreach (Breasts tit in target.breasts.Where(x => x.cupSize > target.genitals.smallestPossibleMaleCupSize))
				{
					tit.ShrinkBreasts(1);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Neck restore
			if (target.neck.type != NeckType.defaultValue && Utils.Rand(4) == 0)
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
			if (target.back.type != BackType.defaultValue && Utils.Rand(5) == 0)
			{
				BackData oldData = target.back.AsReadOnlyData();
				target.RestoreBack();
				sb.Append(RestoredBackText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Ovi perk gain
			if (Utils.Rand(4) == 0 && Species.ECHIDNA.Score(target) >= 3 && target.hasVagina && target.womb.canObtainOviposition)
			{
				if (target.womb.GrantOviposition())
				{
					sb.Append(GrantOvipositionText(target));
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}
			if ((target.hasVagina && !target.HasTimedEffect<Heat>() && Utils.Rand(3) == 0) ||
					(target.perks.HasTimedEffect<Heat>() && Utils.RandBool() && target.GetTimedEffectData<Heat>().totalAddedLibido < 30))
			{
				target.GoIntoHeat();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Thickness and hip modifier
			if (Utils.Rand(2) == 0 && target.build.thickness < 90)
			{
				target.build.GetThicker(2);
			}
			//old was rand(2.4) what the actual fuck. i assume that means a 1 in 2.4 chance, but why??? regardless, it's now a 5 in 12 chance (which is 1:2.4)
			if (Utils.Rand(12) < 5 && target.hasVagina && !target.genitals.AppearsMoreMaleThanFemale() && target.hips.size < 14)
			{
				target.build.GrowHips();
				//sb.Append("\n\nAfter finishing, you find that your gait has changed. Did your [hips] widen?");

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

		}

		protected virtual string GrantOvipositionText(Creature target)
		{
			return GainedOvipositionTextGeneric(target);
		}

		protected virtual string UpdateHairText(Creature target, HairData oldData)
		{
			return target.hair.TransformFromText(oldData);
		}

		protected virtual string UpdateEyesText(Creature target, EyeData oldData)
		{
			return target.eyes.TransformFromText(oldData);
		}

		protected virtual string RestoredArmsText(Creature target, ArmData oldData)
		{
			return target.arms.RestoredText(oldData);
		}

		protected virtual string RestoredGillsText(Creature target, GillData oldData)
		{
			return target.gills.RestoredText(oldData);
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
	}
}