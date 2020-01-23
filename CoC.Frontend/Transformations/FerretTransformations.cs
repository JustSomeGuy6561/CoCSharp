//FerretTransformations.cs
//Description:
//Author: JustSomeGuy
//1/18/2020 9:04:46 PM
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Perks;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;
using System.Text;

namespace CoC.Frontend.Transformations
{
	internal abstract class FerretTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;

		//Original Credit: (author text):
		//
		//Coalsack (for revisions)
		//it was originally just the author credit, which presumably just means the text, not the code. still here, just in case.
		//
		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//1/12:+3. 1/3:+2, 5/12:+1, 1/6:+0.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 2, 3 });
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

			//fun fact: ferret fruit has a 1/100th chance of doing nothing. but i can't put that here because it makes no sense for non ferret fruit. also,
			//this had an author credit. afaik, no other items do. alas, i still need to support it. fun times.

			sb.Append(InitialTransformationText(target));

			//also fun fact: this is the only code i've seen (so far, this is relatively early on - NOTE TO SELF: remove this if it proves inaccurate) that resets the bad end warning
			//when it drops below a certain threshold, so you don't get a surprise bad end if you've already reached it once and forgotten about it. seems like a good idea, no?

			//BAD END:
			if (target.face.type == FaceType.FERRET && target.ears.type == EarType.FERRET && target.tail.type == TailType.FERRET && target.lowerBody.type == LowerBodyType.FERRET &&
				target.body.IsFurBodyType() && target is IExtendedCreature extended && !extended.extendedData.resistsTFBadEnds)
			{
				//Get warned!
				if (!extended.extendedData.hasFerretWarning)
				{
					if (target is CombatCreature badEndCC)
					{
						badEndCC.DecreaseIntelligence(5 + Utils.Rand(3));

						if (badEndCC.intelligence < 5) badEndCC.SetIntelligence(5);
					}

					extended.extendedData.hasFerretWarning = true;
				}
				//BEEN WARNED! BAD END! DUN DUN DUN
				else if (Utils.Rand(3) == 0)
				{
					isBadEnd = true;

					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Reset the warning if ferret score drops.
			else if (target is IExtendedCreature resetFlagCheck)
			{
				resetFlagCheck.extendedData.hasFerretWarning = true;
			}

			//if (remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			if (target is CombatCreature cc && cc.relativeSpeed < 100 && Utils.Rand(3) == 0)
			{
				cc.IncreaseSpeed(2 + Utils.Rand(2));
			}

			//this will handle the edge case where the change count starts out as 0.
			if (remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			//Ferret Fruit Effects
			//- + Thin:
			if (target.build.thickness > 15 && Utils.Rand(3) == 0)
			{
				target.build.GetThinner(2);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//- If speed is < 100, increase speed:

			//- If male with a hip rating >4 or a female/herm with a hip rating >6:
			if ((target.hips.size > (target.gender.HasFlag(Gender.FEMALE) ? 6 : 4)) && Utils.Rand(3) == 0)
			{
				if (target.hips.size > 15)
				{
					target.build.ShrinkHips(3);
				}
				else if (target.hips.size > 10)
				{
					target.build.ShrinkHips(2);
				}
				else
				{
					target.build.ShrinkHips(1);
				}

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//- If butt rating is greater than \"petite\":
			if (target.butt.size > Butt.NOTICEABLE && Utils.Rand(3) == 0)
			{
				if (target.butt.size > 15)
				{
					target.build.ShrinkButt(3);
				}
				else if (target.butt.size > 10)
				{
					target.build.ShrinkButt(2);
				}
				else
				{
					target.build.ShrinkButt(1);
				}

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			var biggestCupSize = target.genitals.BiggestCupSize();

			//-If we can shrink the breasts at all (smallest possible cup size takes current gender and any perks into consideration), and the largest cup is greater than a b cup if female/herm.
			if (!hyperHappy && biggestCupSize > target.genitals.smallestPossibleCupSize && (!target.hasVagina || target.genitals.smallestPossibleCupSize > CupSize.B) && Utils.Rand(2) == 0)
			{
				//if we have a vag, check if our min size is below B-Cup. this will only bring us down to B-Cup.
				CupSize targetSize = target.hasVagina && target.genitals.smallestPossibleCupSize < CupSize.B ? CupSize.B : target.genitals.smallestPossibleCupSize;

				foreach (var breastRow in target.breasts)
				{
					if (breastRow.cupSize > targetSize)
					{
						breastRow.ShrinkBreasts(1);
					}
				}

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				//(this will occur incrementally until they become flat, manly breasts for males, or until they are A or B cups for females/herms)
			}

			//Remove additional cocks
			if (target.cocks.Count > 1 && Utils.Rand(3) == 0)
			{
				target.genitals.RemoveCock();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Remove additional balls/remove uniball
			if (target.balls.count > 0 && Utils.Rand(3) == 0)
			{
				bool changedBalls = false;
				if (target.balls.size > 2)
				{
					if (target.balls.size > 5)
					{
						changedBalls |= target.balls.ShrinkBalls((byte)(1 + Utils.Rand(3))) > 0;
					}
					else
					{
						changedBalls |= target.balls.ShrinkBalls(1) > 0;
					}
				}

				if (target.balls.count != 2)
				{
					changedBalls |= target.balls.MakeStandard();
				}

				if (changedBalls)
				{
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			var longestCock = target.genitals.LongestCock();

			//-If penis size is > 6 inches:
			if (target.hasCock && longestCock.length > 6 && !hyperHappy)
			{
				var shortenedCock = false;
				if (longestCock.length >= 10)
				{
					shortenedCock |= longestCock.ShortenCock(Utils.Rand(4) + 2) > 0;
				}
				else
				{
					shortenedCock |= longestCock.ShortenCock(Utils.RandBool() ? 2 : 1) > 0;
				}

				if (longestCock.girth > longestCock.length / 6.0f)
				{
					shortenedCock |= longestCock.SetGirth(longestCock.length / 6.0f) == longestCock.length / 6.0f;
				}

				if (shortenedCock)
				{
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//converts first cock to ferret, no others.
			if (target.hasCock && target.cocks[0].type != CockType.FERRET && Utils.Rand(3) == 0)
			{
				target.genitals.UpdateCock(0, CockType.FERRET);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//-If the PC has quad nipples:
			if (target.genitals.hasQuadNipples && Utils.Rand(4) == 0)
			{
				target.genitals.SetQuadNipples(false);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//If the PC has gills:
			if (!target.gills.isDefault && Utils.Rand(4) == 0)
			{
				target.RestoreGills();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Hair
			var setFerretHairColor = !Species.FERRET.availableHairColors.Contains(target.hair.hairColor);
			if ((target.hair.type != HairType.NORMAL || setFerretHairColor || target.hair.length <= 0) && Utils.Rand(4) == 0)
			{
				HairData oldHairData = target.hair.AsReadOnlyData();

				float length = target.hair.length == 0 ? 1 : target.hair.length;
				HairFurColors color = setFerretHairColor ? Utils.RandomChoice(Species.FERRET.availableHairColors) : target.hair.hairColor;

				if (target.hair.type != HairType.NORMAL)
				{
					target.UpdateHair(HairType.NORMAL, true, color, newHairLength: length);
				}
				else
				{
					target.hair.SetAll(length, true, color);
				}

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//If the PC has four eyes or one eye (if ever implemented):
			if (target.eyes.count != 2 && Utils.Rand(3) == 0)
			{

				target.RestoreEyes();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Go into heat
			if (target.hasVagina && Utils.Rand(3) == 0)
			{
				if (target.GoIntoHeat())
				{
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Neck restore
			if (!target.neck.isDefault && Utils.Rand(4) == 0)
			{
				target.RestoreNeck();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Rear body restore
			if (!target.back.isDefault && Utils.Rand(5) == 0)
			{
				target.RestoreBack();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Ovi perk loss
			if (target.womb is PlayerWomb playerWomb && playerWomb.canClearOviposition && Utils.Rand(5) == 0)
			{
				playerWomb.ClearOviposition();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//face TFs. they all now use the same roll.

			//Turn ferret mask to full furface.

			if (Utils.Rand(3) == 0)
			{
				if (target.face.type == FaceType.HUMAN)
				{
					target.UpdateFace(FaceType.FERRET);

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
				else if (target.face.type != FaceType.HUMAN && target.face.type != FaceType.FERRET)
				{
					target.RestoreFace();

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
				else if (target.face.type == FaceType.FERRET && !target.face.isFullMorph && target.body.isFurry && target.ears.type == EarType.FERRET
					&& target.tail.type == TailType.FERRET && target.lowerBody.type == LowerBodyType.FERRET && Utils.Rand(4) < 3)
				{
					if (target.face.StrengthenFacialMorph())
					{
						if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

			}
			//ferret uses the full underbody fur.
			if (target.body.type != BodyType.UNDERBODY_FUR)
			{
				//if they have full fur, but it doesn't allow an underbody, or the weird kitsune fur/skin combo, silently update it to have an underbody.
				if (target.body.type == BodyType.SIMPLE_FUR || target.body.type == BodyType.KITSUNE)
				{
					Species.FERRET.GetFurColorsFrom(target.body.mainEpidermis.fur, out FurColor primary, out FurColor secondary);
					target.UpdateBody(BodyType.UNDERBODY_FUR, primary, secondary);

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
				//otherwise, require some other parts, and rng.
				else if (target.ears.type == EarType.FERRET && target.tail.type == TailType.FERRET && target.lowerBody.type == LowerBodyType.FERRET && Utils.Rand(4) == 0)
				{
					Species.FERRET.GetRandomFurColor(out FurColor primary, out FurColor secondary);
					target.UpdateBody(BodyType.UNDERBODY_FUR, primary, secondary);

					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//rubber skin is now a perk. try to remove a stack. if this removes the perk, that's cool too.
			if (target.HasPerk<RubberySkin>())
			{
				if (target.GetPerk<RubberySkin>().attemptStackDecrease())
				{
					if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Tail TFs!
			if (target.tail.type != TailType.FERRET && target.ears.type == EarType.FERRET && Utils.Rand(3) == 0)
			{
				target.UpdateTail(TailType.FERRET);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//If legs are not ferret, has ferret ears and tail
			if (target.lowerBody.type != LowerBodyType.FERRET && target.ears.type == EarType.FERRET && target.tail.type == TailType.FERRET && Utils.Rand(4) == 0)
			{
				target.UpdateLowerBody(LowerBodyType.FERRET);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Arms
			if (target.arms.type != ArmType.FERRET && target.tail.type == TailType.FERRET && target.lowerBody.type == LowerBodyType.FERRET && Utils.Rand(4) == 0)
			{
				target.UpdateArms(ArmType.FERRET);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//If ears are not ferret:
			if (target.ears.type != EarType.FERRET && Utils.Rand(4) == 0)
			{
				target.UpdateEars(EarType.FERRET);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Remove antennae, if insectile
			if (target.antennae.type == AntennaeType.BEE && Utils.Rand(4) == 0)
			{
				target.RestoreAntennae();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//If no other effect occurred, fatigue decreases:
			if (changeCount == remainingChanges && target is CombatCreature fatigueCheck)
			{
				fatigueCheck.RecoverFatigue(20);
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