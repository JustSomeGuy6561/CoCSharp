//SpiderTransformations.cs
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

			int changeCount = GenerateChangeCount(target, new int[] { 2, 2 });
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

			sb.Append(InitialTransformationText(target));

			//*************
			//Stat Changes
			//*************
			//(increases sensitivity)
			if (Utils.Rand(3) == 0)
			{
				target.ChangeSensitivity(1);

			}
			//(Increase libido)
			if (Utils.Rand(3) == 0)
			{
				target.ChangeLibido(1);
			}
			//MOD: speed now has a common roll.
			if (Utils.Rand(3) == 0)
			{
				//(If speed<70, increases speed)
				if (target.relativeSpeed < 70)
				{
					target.ChangeSpeed(1.5f);
				}
				//(If speed>80, decreases speed down to minimum of 80)
				else if (target.relativeSpeed > 80)
				{
					target.ChangeSpeed(-1.5f);
				}
			}

			//(increase toughness to 60)
			if (Utils.Rand(3) == 0 && target.relativeToughness < 60)
			{
				target.ChangeToughness(1);
			}
			//(decrease strength to 70)
			if (target.relativeStrength > 70 && Utils.Rand(3) == 0)
			{
				target.ChangeStrength(-1);
			}


			//this will handle the edge case where the change count starts out as 0.
			if (remainingChanges <= 0)
			{
				return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
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
			if (target.hasVagina && Utils.Rand(3) == 0)
			{
				//respects any perks that prevent us from dropping too loose (like marae perk).
				//shrink largest if possible. if 2 (or more if that happens) and both(all) at min, remove extra one.
				VaginalLooseness minVaginalLooseness = EnumHelper.Max(VaginalLooseness.NORMAL, target.genitals.minVaginalLooseness);
				if (target.genitals.LargestVaginalLooseness() > minVaginalLooseness)
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
				else if (target.vaginas.Count > 1)
				{
					target.RemoveExtraVaginas();
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

			//***************
			//Appearance Changes
			//***************
			//(Ears become pointed if not human)
			if (target.ears.type != EarType.HUMAN && target.ears.type != EarType.ELFIN && Utils.Rand(4) == 0)
			{
				EarData oldData = target.ears.AsReadOnlyData();
				target.UpdateEars(EarType.ELFIN);
				sb.Append(UpdateEarsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//(Fur/Scales fall out)
			if (target.body.type != BodyType.HUMANOID && (target.ears.type == EarType.HUMAN || target.ears.type == EarType.ELFIN) && Utils.Rand(4) == 0)
			{
				BodyData oldData = target.body.AsReadOnlyData();
				target.UpdateBody(BodyType.HUMANOID, Tones.PALE);
				sb.Append(UpdateBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//(Gain human face)
			if (target.body.type == BodyType.HUMANOID && (target.face.type != FaceType.SPIDER && target.face.type != FaceType.HUMAN) && Utils.Rand(4) == 0)
			{
				FaceData oldData = target.face.AsReadOnlyData();
				target.UpdateFace(FaceType.HUMAN);
				sb.Append(UpdateFaceText(target, oldData));

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
				target.IncreaseIntelligence(5);
				EyeData oldData = target.eyes.AsReadOnlyData();
				target.UpdateEyes(EyeType.SPIDER);
				sb.Append(UpdateEyesText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//(Gain spider fangs)
			if (target.face.type == FaceType.HUMAN && target.body.type == BodyType.HUMANOID && Utils.Rand(4) == 0)
			{
				FaceData oldData = target.face.AsReadOnlyData();
				target.UpdateFace(FaceType.SPIDER);
				sb.Append(UpdateFaceText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//(Arms to carapace-covered arms)
			if (target.arms.type != ArmType.SPIDER && Utils.Rand(4) == 0)
			{
				ArmData oldData = target.arms.AsReadOnlyData();
				target.UpdateArms(ArmType.SPIDER);
				sb.Append(UpdateArmsText(target, oldData));

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
				//Opens up drider ovipositor scenes from available mobs. The character begins producing unfertilized eggs in their arachnid abdomen. Egg buildup raises minimum lust and eventually lowers speed until the target has gotten rid of them. This perk may only be used with the drider lower body, so your scenes should reflect that.
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
				LowerBodyData oldData = target.lowerBody.AsReadOnlyData();
				target.UpdateLowerBody(LowerBodyType.CHITINOUS_SPIDER);
				sb.Append(UpdateLowerBodyText(target, oldData));

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
				TailData oldData = target.tail.AsReadOnlyData();
				target.UpdateTail(TailType.SPIDER_SPINNERET);
				sb.Append(UpdateTailText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//(Drider Item Only: Carapace-Clad Legs to Drider Legs)
			if (isDrider && target.lowerBody.type == LowerBodyType.CHITINOUS_SPIDER && Utils.Rand(4) == 0 && target.tail.type == TailType.SPIDER_SPINNERET)
			{
				LowerBodyData oldData = target.lowerBody.AsReadOnlyData();
				target.UpdateLowerBody(LowerBodyType.DRIDER);
				sb.Append(UpdateLowerBodyText(target, oldData));

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

		protected virtual string ClearOvipositionText(Creature target)
{
return RemovedOvipositionTextGeneric(target);
}

		protected virtual string UpdateEarsText(Creature target, EarData oldData)
		{
			return target.ears.TransformFromText(oldData);
		}
		protected virtual string UpdateBodyText(Creature target, BodyData oldData)
		{
			return target.body.TransformFromText(oldData);
		}

		protected virtual string UpdateEyesText(Creature target, EyeData oldData)
		{
			return target.eyes.TransformFromText(oldData);
		}

		protected virtual string UpdateFaceText(Creature target, FaceData oldData)
		{
			return target.face.TransformFromText(oldData);
		}

		protected virtual string UpdateArmsText(Creature target, ArmData oldData)
		{
			return target.arms.TransformFromText(oldData);
		}

		protected virtual string UpdateTailText(Creature target, TailData oldTail)
		{
			return target.tail.TransformFromText(oldTail);
		}
		protected virtual string UpdateLowerBodyText(Creature target, LowerBodyData oldData)
		{
			return target.lowerBody.TransformFromText(oldData);
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