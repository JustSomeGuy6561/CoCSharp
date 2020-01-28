//KitsuneTransformations.cs
//Description:
//Author: JustSomeGuy
//1/20/2020 3:04:30 AM
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Items.Wearables.Tattoos;
using CoC.Frontend.Perks.SpeciesPerks;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;
using System.Text;
using System.Linq;

namespace CoC.Frontend.Transformations
{
	internal abstract class KitsuneTransformations : GenericTransformationBase
	{
		protected readonly bool enhanced;

		protected KitsuneTransformations(bool potent)
		{
			enhanced = potent;
		}

		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;

		/** Original Credits:
		 *
		* @since March 31, 2018
		* @author Stadler76
		*
		* Note: Any modifications since this point will be marked with MOD:
		* Also Note: this is built from a template, so some comments may remain from that template that have not been marked correctly :(. should not happen, but obligatory disclaimer.
		* This is built from the old fox jewel code, which was primarily used for kitsunes. if you notice any similarities to fox tf, it's because it's basically a hybrid fox/human
		* transformation.
		*
		* OVERARCHING MOD NOTE: Body types have been implemented, so there's now a lot of checks that reference it more effectively. keep this in mind, i guess.
		*/
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
			sb.Append(InitialTransformationText(target));

			//Add any free changes here - these can occur even if the change count is 0. these include things such as change in stats (intelligence, etc)
			//change in height, hips, and/or butt, or other similar stats.


			//**********************
			//BASIC STATS
			//**********************
			//[increase Intelligence, Libido and Sensitivity]
			if (target is CombatCreature cc)
			{
				if (cc.relativeIntelligence < 100 && Utils.Rand(enhanced ? 2 : 4) == 0)
				{
					//Raise INT, Lib, Sens. and +10 LUST
					cc.DeltaCombatCreatureStats(inte: 2, lib: 1, sens: 2, lus: 10);
				}
				//[decrease Strength toward 15]
				if (cc.relativeStrength > 15 && Utils.Rand(enhanced ? 2 : 3) == 0)
				{
					//MOD NOTE: Changed this from flat delta to percent delta. feel free to change back to flat.
					if (cc.relativeStrength > 70)
					{
						cc.DecreaseStrengthByPercent(4);
					}
					else if (cc.relativeStrength > 50)
					{
						cc.DecreaseStrengthByPercent(3);
					}
					else if (cc.relativeStrength > 30)
					{
						cc.DecreaseStrengthByPercent(2);
					}
					else
					{
						cc.DecreaseStrengthByPercent(1);
					}
				}
				//[decrease Toughness toward 20]
				if (cc.relativeToughness > 20 && Utils.Rand(enhanced ? 2 : 3) == 0)
				{
					//MOD NOTE: Changed this from flat delta to percent delta. feel free to change back to flat.
					if (cc.relativeToughness > 66)
					{
						cc.DecreaseToughnessByPercent(2);
					}
					else
					{
						cc.DecreaseToughnessByPercent(1);
					}
				}
			}
			if (enhanced && Utils.Rand(2) == 0 && target.corruption < 100)
			{

				target.IncreaseCorruption(1);
			}
			//this will handle the edge case where the change count starts out as 0.
			if (remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			//**********************
			//MEDIUM/SEXUAL CHANGES
			//**********************
			//[adjust Femininity toward 50]
			//from low to high
			//Your facial features soften as your body becomes more androgynous.
			//from high to low
			//Your facial features harden as your body becomes more androgynous.
			if (Utils.Rand(enhanced ? 2 : 4) == 0 && target.femininity != 50)
			{

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//[decrease muscle tone toward 40]
			if (target.build.muscleTone >= 40 && Utils.Rand(enhanced ? 2 : 4) == 0)
			{
				target.build.DecreaseMuscleTone((byte)(2 + Utils.Rand(3)));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//MOD NOTE: This now is one check - if hips aren't 10, and we roll a 0, adjust them, regardless of size.

			//[Adjust hips toward 10 – wide/curvy/flared]
			if (target.hips.size != 10 && Utils.Rand(enhanced ? 2 : 3) == 0)
			{
				//from narrow to wide
				if (target.hips.size < 4)
				{
					target.build.GrowHips(3);
				}
				else if (target.hips.size < 7)
				{
					target.build.GrowHips(2);
				}
				else if (target.hips.size < 10)
				{
					target.build.GrowHips();
				}
				//from wide to narrower
				else if (target.hips.size < 14)
				{
					target.build.ShrinkHips();
				}
				else if (target.hips.size < 18)
				{
					target.build.ShrinkHips(2);
				}
				else
				{
					target.build.ShrinkHips(3);
				}

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//[Adjust hair length toward range of 16-26 – very long to ass-length]
			if ((target.hair.length < 16 || target.hair.length > 26) && Utils.Rand(enhanced ? 2 : 3) == 0)
			{
				bool changedLength = false;
				//from short to long
				if (target.hair.length < 16)
				{
					changedLength = target.hair.GrowHair(3 + Utils.Rand(3)) > 0;
				}
				//from long to short
				else
				{
					changedLength = target.hair.ShortenHair(3 + Utils.Rand(3)) != 0;
				}
				if (changedLength)
				{
					remainingChanges--;
					if (remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//[Increase Vaginal Capacity] - requires vagina, of course
			if (target.hasVagina && Utils.Rand(enhanced ? 2 : 3) == 0 && target.genitals.standardBonusVaginalCapacity < 200)
			{

				target.genitals.IncreaseBonusVaginalCapacity((ushort)(10 + Utils.Rand(10)));

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			else if (Utils.Rand(enhanced ? 2 : 3) == 0 && target.genitals.standardBonusAnalCapacity < 150)
			{
				target.genitals.IncreaseBonusAnalCapacity((ushort)(10 + Utils.Rand(10)));


				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}


			//[Vag of Holding] - rare effect, only if PC has high vaginal looseness
			//MOD NOTE: Vag of holding is now a perk, and directly linked to kitsune score. The perk subscribes to a creature score checker, and when the kitsune score drops to 0,
			//the player loses the perk. to obtain the perk, the creature must not have it (duh), have a decent kitsune score, and full bonus capacity for anal and vaginal (defined above)
			if (target.hasVagina && !target.HasPerk<VagOfHolding>() && (enhanced || Utils.Rand(5) == 0) && target.genitals.standardBonusVaginalCapacity >= 200
				&& target.genitals.standardBonusAnalCapacity >= 150 && Species.KITSUNE.Score(target) > 4)
			{
				target.perks.AddPerk<VagOfHolding>();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}


			//**********************
			//BIG APPEARANCE CHANGES
			//**********************
			//Neck restore
			if (!target.neck.isDefault && Utils.Rand(4) == 0)
			{
				target.RestoreNeck();
			}
			//Rear body restore
			if (!target.back.isDefault && Utils.Rand(5) == 0)
			{
				target.RestoreBack();
			}
			//Ovi perk loss
			if (target.womb is PlayerWomb playerWomb && playerWomb.canClearOviposition && Utils.Rand(5) == 0)
			{
				playerWomb.ClearOviposition();
			}
			//[Grow Fox Tail]
			if (target.tail.type != TailType.FOX && Utils.Rand(enhanced ? 2 : 4) == 0)
			{
				target.UpdateTail(TailType.FOX);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//[Grow Addtl. Fox Tail]
			//(rare effect, up to max of 8 tails, requires PC level and int*10 = number of tail to be added)
			//MOD Note: now works with non-combat creatures if that ever happens.
			if (target.tail.type == TailType.FOX && target.tail.tailCount < TailType.FOX.maxTailCount - 1 && (!(target is CombatCreature tailCheck) ||
				(tailCheck.level > target.tail.tailCount && tailCheck.intelligence / 10 > target.tail.tailCount)) && Utils.Rand(enhanced ? 2 : 3) == 0)
			{
				target.tail.GrowAdditionalTail();
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//MOD NOTE: moved below previous check for logical consistency. Informs players on how to get the 9th tail.
			else if (!enhanced && target.ears.type == EarType.FOX && target.tail.type == TailType.FOX && target.tail.tailCount == TailType.FOX.maxTailCount - 1 && Utils.Rand(3) == 0)
			{

			}
			//[Grow 9th tail and gain Corrupted Nine-tails perk]
			//MOD NOTE: Note that this check requires fox ears, but the others do not. is this a mistake - should they all have the ear check?
			else if (enhanced && Utils.Rand(4) == 0 && target.tail.type == TailType.FOX && target.ears.type == EarType.FOX && target.tail.tailCount == TailType.FOX.maxTailCount - 1 &&
				(!(target is CombatCreature corruptedPerkCheck) || (corruptedPerkCheck.level >= 9 && corruptedPerkCheck.intelligence >= 90)))
			{

				target.tail.GrowAdditionalTail();

				if (!target.HasPerk<NineTails>())
				{
					target.perks.AddPerk(new NineTails(false));
				}

				target.IncreaseCreatureStats(lib: 2, lus: 10, corr: 10);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//[Grow Fox Ears]
			if (target.tail.type == TailType.FOX && Utils.Rand(enhanced ? 2 : 4) == 0 && target.ears.type != EarType.FOX)
			{
				target.UpdateEars(EarType.FOX);
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//[Change Hair Color: Golden-blonde, SIlver Blonde, White, Black, Red]
			if (Utils.Rand(enhanced ? 2 : 4) == 0 && !Species.KITSUNE.elderKitsuneHairColors.Contains(target.hair.hairColor) &&
				!Species.KITSUNE.kitsuneHairColors.Contains(target.hair.hairColor))
			{
				HairFurColors color;
				if (target.tail.type == TailType.FOX && target.tail.tailCount == 9)
				{
					color = Utils.RandomChoice(Species.KITSUNE.elderKitsuneHairColors);
				}
				else
				{
					color = Utils.RandomChoice(Species.KITSUNE.kitsuneHairColors);
				}

				target.hair.SetHairColor(color);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//MOD: Body Changes. with the body changes, this has been fully reworked. If enhanced, always runs. otherwise, 50% chance.
			//if we have a non-furry body and it's not humanoid, revert back to human, picking a valid kitsune skin tone.
			//if we are humanoid or furry (and not kitsune), convert to kitsune, picking valid skin tone and fur color if needed.
			//if we are kitsune, we have a 50% chance of picking up runic tattoos on our entire body. this only occurs if the creature does not already have a full body tattoo.
			//for those worried about tats, this one is relatively innocuous; it's simply vague, runic markings across the body, but not very obtrusive. it also respects other tats.
			//if you want to remove it, it can easily be done at the tattoo shop/parlor/whatever. i suppose we could make it free if that makes you feel better.
			//or put in a gameplay setting like piercing fetish that prevents this item from proccing it. alternatively, we could also lower the proc rate.
			if (enhanced || Utils.RandBool())
			{

				if (target.body.type != BodyType.KITSUNE && target.body.type != BodyType.HUMANOID && !target.body.IsFurBodyType())
				{
					Tones targetTone = target.body.primarySkin.tone;
					if (!Species.KITSUNE.ElderKitsuneTones.Contains(targetTone) && !Species.KITSUNE.KitsuneTones.Contains(targetTone))
					{
						targetTone = Utils.RandomChoice(enhanced ? Species.KITSUNE.ElderKitsuneTones : Species.KITSUNE.KitsuneTones);
					}

					target.UpdateBody(BodyType.HUMANOID, targetTone, SkinTexture.SMOOTH);
				}
				else if (target.body.type != BodyType.KITSUNE)
				{
					//we're doing the full transform here. check for skin tone, fur color, and update both if necessary.
					Tones targetTone = target.body.primarySkin.tone;
					FurColor targetFur = target.body.activeFur.fur;

					if (!Species.KITSUNE.ElderKitsuneTones.Contains(targetTone) && !Species.KITSUNE.KitsuneTones.Contains(targetTone))
					{
						targetTone = Utils.RandomChoice(enhanced ? Species.KITSUNE.ElderKitsuneTones : Species.KITSUNE.KitsuneTones);
					}

					if (FurColor.IsNullOrEmpty(targetFur) || (!Species.KITSUNE.elderKitsuneFurColors.Contains(targetFur) && !Species.KITSUNE.allKitsuneColors.Contains(targetFur)))
					{
						targetFur = Utils.RandomChoice(enhanced ? Species.KITSUNE.elderKitsuneFurColors : Species.KITSUNE.allKitsuneColors);
					}

					target.UpdateBody(BodyType.KITSUNE, targetTone, targetFur, SkinTexture.SEXY, FurTexture.SOFT);
				}
				else if (!target.body.fullCreatureTattoos.TattooedAt(FullBodyTattooLocation.MAIN) && Utils.Rand(2) == 0)
				{
					target.body.fullCreatureTattoos.GetTattoo(FullBodyTattooLocation.MAIN, new RunicBodyTattoo(), true);
				}
			}

			//Nipples Turn Back:
			if (target.KitsuneLikeBody() && target.genitals.hasBlackNipples && Utils.Rand(3) == 0)
			{
				target.genitals.SetBlackNipples(false);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Debugcunt(s)
			if (target.KitsuneLikeBody() && target.hasVagina && target.vaginas.Any(x=>!x.isDefault) && Utils.Rand(3) == 0)
			{
				foreach (var vagina in target.vaginas)
				{
					if (!vagina.isDefault)
					{
						target.genitals.RestoreVagina(vagina);
					}
				}
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			// Kitsunes should have normal arms and legs. especially skinny arms with claws are somewhat weird (Stadler76).
			//MOD NOTE: not really a thing anymore since we made all the body parts have their own rules. but i guess i could make it so that any arms or legs that don't use skin
			//revert to human - JSG

			//MOD: humanoid body or kitsune body reverts arms and legs. it's possible to do this based on the arms/legs themselves by checking their epidermis data, but this
			//is more in the spirit of the original code, i guess.
			if (target.KitsuneLikeBody() && !target.arms.isDefault && Utils.Rand(4) == 0)
			{
				target.RestoreArms();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			if (target.KitsuneLikeBody() && !target.lowerBody.isDefault && Utils.Rand(4) == 0)
			{
				target.RestoreLowerBody();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
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