﻿//SpiderTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:45:31 PM
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
	internal abstract class SpiderTransformations : GenericTransformationBase
	{
		protected readonly bool isDrider;
		protected SpiderTransformations(bool drider)
		{
			isDrider = drider;
		}

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

			//Consuming Text

			//*************
			//Stat Changes
			//*************
			//(increases sensitivity)
			if (Utils.Rand(3) == 0)
			{
				target.DeltaCreatureStats(sens: 1);
			}
			//(Increase libido)
			if (Utils.Rand(3) == 0)
			{
				target.DeltaCreatureStats(lib: 1);
			}
			if (target is CombatCreature cc)
			{
				//MOD: speed now has a common roll.
				if (Utils.Rand(3) == 0)
				{
					//(If speed<70, increases speed)
					if (cc.relativeSpeed < 70)
					{
						cc.DeltaCombatCreatureStats(spe: 1.5f);
					}
					//(If speed>80, decreases speed down to minimum of 80)
					else if (cc.relativeSpeed > 80)
					{
						cc.DeltaCombatCreatureStats(spe: -1.5f);
					}
				}

				//(increase toughness to 60)
				if (Utils.Rand(3) == 0 && cc.relativeToughness < 60)
				{
					cc.DeltaCombatCreatureStats(tou: 1);
				}
				//(decrease strength to 70)
				if (cc.relativeStrength > 70 && Utils.Rand(3) == 0)
				{
					cc.DeltaCombatCreatureStats(str: -1);
				}
			}
			//****************
			//Sexual Changes
			//****************
			//Increase venom recharge
			if (target.tail.type == TailType.SPIDER_SPINNERET && target.tail.regenRate < target.tail.maxRegen)
			{
				target.tail.UpdateResources(regenRateDelta: 5);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//(tightens vagina to 1, increases lust/libido)
			if (target.hasVagina)
			{
				//respects any perks that prevent us from dropping too loose (like marae perk).
				VaginalLooseness minVaginalLooseness = EnumHelper.Max(VaginalLooseness.NORMAL, target.genitals.minVaginalLooseness);
				if (target.genitals.LargestVaginalLooseness() > minVaginalLooseness && Utils.Rand(3) == 0)
				{
					foreach (Vagina vagina in target.vaginas)
					{
						if (vagina.looseness > minVaginalLooseness)
						{
							vagina.DecreaseLooseness();
						}
					}

					target.DeltaCreatureStats(lib: 2, lus: 25);

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}
			AnalLooseness minAnalLooseness = EnumHelper.Max(AnalLooseness.LOOSE, target.genitals.minAnalLooseness);

			//(tightens asshole to 1, increases lust)
			if (target.ass.looseness > minAnalLooseness && Utils.Rand(3) == 0)
			{
				target.ass.DecreaseLooseness();
				target.DeltaCreatureStats(lib: 2, lus: 25);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//[Requires penises]
			//(Thickens all cocks to a ratio of 1\" thickness per 5.5\"

			//use a handly linq conversion to grab all the cocks that are below this threshold.
			Cock[] toThicken = target.cocks.Where(x => x.girth * 5.5 < x.length).ToArray();

			if (toThicken.Length > 0 && Utils.Rand(4) == 0)
			{
				int amountChanged = toThicken.Sum(x => x.IncreaseThickness(0.1f) != 0 ? 1 : 0);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//[Increase to Breast Size] - up to Large DD
			if (target.genitals.SmallestCupSize() < CupSize.DD_BIG && Utils.Rand(4) == 0)
			{
				target.genitals.SmallestBreast().GrowBreasts();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//[Increase to Ass Size] - to 11
			if (target.butt.size < 11 && Utils.Rand(4) == 0)
			{
				target.butt.GrowButt();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Neck restore
			if (target.neck.type != NeckType.HUMANOID && Utils.Rand(4) == 0)
			{
				target.RestoreNeck();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Rear body restore
			if (!target.back.isDefault && Utils.Rand(5) == 0)
			{
				target.RestoreBack();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Ovi perk loss
			if (target.womb.canRemoveOviposition && Utils.Rand(5) == 0)
			{
				target.womb.ClearOviposition();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//***************
			//Appearance Changes
			//***************
			//(Ears become pointed if not human)
			if (target.ears.type != EarType.HUMAN && target.ears.type != EarType.ELFIN && Utils.Rand(4) == 0)
			{
				target.UpdateEars(EarType.ELFIN);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//(Fur/Scales fall out)
			if (target.body.type != BodyType.HUMANOID && (target.ears.type == EarType.HUMAN || target.ears.type == EarType.ELFIN) && Utils.Rand(4) == 0)
			{
				target.UpdateBody(BodyType.HUMANOID, Tones.PALE);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//(Gain human face)
			if (target.body.type == BodyType.HUMANOID && (target.face.type != FaceType.SPIDER && target.face.type != FaceType.HUMAN) && Utils.Rand(4) == 0)
			{
				target.UpdateFace(FaceType.HUMAN);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Remove breast rows over 2.
			if (target.breasts.Count > 2 && Utils.Rand(3) == 0 && !hyperHappy)
			{
				target.RemoveExtraBreastRows();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Nipples reduction to 1 per tit.
			if (target.genitals.hasQuadNipples && Utils.Rand(4) == 0)
			{
				target.genitals.SetQuadNipples(false);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Nipples Turn Black:
			if (!target.genitals.hasBlackNipples && Utils.Rand(6) == 0)
			{
				target.genitals.SetBlackNipples(true);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//eyes!
			if (target.body.type == BodyType.HUMANOID && target.eyes.type != EyeType.SPIDER && Utils.Rand(4) == 0)
			{
				if (target is CombatCreature eyeInt)
				{
					eyeInt.IncreaseIntelligence(5);
				}
				target.UpdateEyes(EyeType.SPIDER);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//(Gain spider fangs)
			if (target.face.type == FaceType.HUMAN && target.body.type == BodyType.HUMANOID && Utils.Rand(4) == 0)
			{
				target.UpdateFace(FaceType.SPIDER);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//(Arms to carapace-covered arms)
			if (target.arms.type != ArmType.SPIDER && Utils.Rand(4) == 0)
			{
				target.UpdateArms(ArmType.SPIDER);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			if (Utils.Rand(4) == 0 && target.lowerBody.type != LowerBodyType.DRIDER && target.lowerBody.type != LowerBodyType.CHITINOUS_SPIDER)
			{
				target.RestoreLowerBody();
			}
			//Drider butt
			if (isDrider && target.tail.type == TailType.SPIDER_SPINNERET && target.tail.ovipositor.type != OvipositorType.SPIDER && target.lowerBody.type == LowerBodyType.DRIDER
				&& Utils.Rand(3) == 0 && (target.hasVagina || Utils.Rand(2) == 0))
			{
				//V1 - Egg Count
				//V2 - Fertilized Count
				target.tail.GrantOvipositor();
				//Opens up drider ovipositor scenes from available mobs. The character begins producing unfertilized eggs in their arachnid abdomen. Egg buildup raises minimum lust and eventually lowers speed until the target has gotten rid of them.  This perk may only be used with the drider lower body, so your scenes should reflect that.
				//Any PC can get an Ovipositor perk, but it will be much rarer for characters without vaginas.
				//Eggs are unfertilized by default, but can be fertilized:
				//-female/herm characters can fertilize them by taking in semen; successfully passing a pregnancy check will convert one level ofunfertilized eggs to fertilized, even if the PC is already pregnant.
				//-male/herm characters will have a sex dream if they reach stage three of unfertilized eggs; this will represent their bee/drider parts drawing their own semen from their body to fertilize the eggs, and is accompanied by a nocturnal emission.
				//-unsexed characters cannot currently fertilize their eggs.
				//Even while unfertilized, eggs can be deposited inside NPCs - obviously, unfertilized eggs will never hatch and cannot lead to any egg-birth scenes that may be written later.
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//(Normal Biped Legs -> Carapace-Clad Legs)
			if (((isDrider && target.lowerBody.type != LowerBodyType.DRIDER && target.lowerBody.type != LowerBodyType.CHITINOUS_SPIDER) || (!isDrider && target.lowerBody.type != LowerBodyType.CHITINOUS_SPIDER)) && target.lowerBody.isBiped && Utils.Rand(4) == 0)
			{
				target.UpdateLowerBody(LowerBodyType.CHITINOUS_SPIDER);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//(Tail becomes spider abdomen GRANT WEB ATTACK)
			if (target.tail.type != TailType.SPIDER_SPINNERET && (target.lowerBody.type == LowerBodyType.CHITINOUS_SPIDER || target.lowerBody.type == LowerBodyType.DRIDER) && target.arms.type == ArmType.SPIDER && Utils.Rand(4) == 0)
			{
				//(Pre-existing tails)
				//(No tail)
				target.UpdateTail(TailType.SPIDER_SPINNERET);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//(Drider Item Only: Carapace-Clad Legs to Drider Legs)
			if (isDrider && target.lowerBody.type == LowerBodyType.CHITINOUS_SPIDER && Utils.Rand(4) == 0 && target.tail.type == TailType.SPIDER_SPINNERET)
			{
				target.UpdateLowerBody(LowerBodyType.DRIDER);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			// Remove gills
			if (Utils.Rand(4) == 0 && !target.gills.isDefault)
			{
				target.RestoreGills();
			}

			if (remainingChanges == changeCount && target is CombatCreature fatigueCheck)
			{
				fatigueCheck.RecoverFatigue(33);
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