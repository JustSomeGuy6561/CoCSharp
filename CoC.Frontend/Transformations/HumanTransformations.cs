//HumanTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:50:39 PM
using System;
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Perks.SpeciesPerks;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;

namespace CoC.Frontend.Transformations
{
	internal abstract class HumanTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			int changeCount = GenerateChangeCount(target, new int[] { 2, 2 });
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

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

			//-----------------------
			// MAJOR TRANSFORMATIONS
			//-----------------------
			//1st priority: Change lower body to bipedal.
			if (Utils.Rand(4) == 0)
			{
				target.RestoreLowerBody();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Remove Oviposition Perk
			if (target.womb.canRemoveOviposition && Utils.Rand(5) == 0)
			{
				target.womb.ClearOviposition();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Remove Incorporeality Perk, if not permanent
			if (target.HasPerk<Incorporeal>() && Utils.Rand(4) == 0)
			{
				target.RemovePerk<Incorporeal>();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Restore neck
			if (target.neck.type != NeckType.HUMANOID && Utils.Rand(5) == 0)
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
			//-Skin color change – light, fair, olive, dark, ebony, mahogany, russet
			if (!Species.HUMAN.availableTones.Contains(target.body.primarySkin.tone) && Utils.Rand(5) == 0)
			{
				target.body.ChangeAllSkin(Utils.RandomChoice(Species.HUMAN.availableTones));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Change skin to normal
			if (target.body.type != BodyType.HUMANOID && (target.ears.type == EarType.HUMAN || target.ears.type == EarType.ELFIN) && Utils.Rand(4) == 0)
			{
				target.RestoreBody();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Restore arms to become human arms again
			if (Utils.Rand(4) == 0)
			{
				target.RestoreArms();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-----------------------
			// MINOR TRANSFORMATIONS
			//-----------------------
			//-Human face
			if (target.face.type != FaceType.HUMAN && Utils.Rand(4) == 0)
			{
				target.UpdateFace(FaceType.HUMAN);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Human tongue
			if (target.tongue.type != TongueType.HUMAN && Utils.Rand(4) == 0)
			{
				target.UpdateTongue(TongueType.HUMAN);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Remove odd eyes
			if (Utils.Rand(5) == 0 && target.eyes.type != EyeType.HUMAN)
			{
				target.RestoreEyes();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Gain human ears (If you have human face)
			if (target.ears.type != EarType.HUMAN && target.face.type == FaceType.HUMAN && Utils.Rand(4) == 0)
			{
				target.UpdateEars(EarType.HUMAN);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Removes gills
			if (Utils.Rand(4) == 0 && !target.gills.isDefault)
			{
				target.RestoreGills();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Nipples Turn Back:
			if (target.genitals.hasBlackNipples && Utils.Rand(3) == 0)
			{
				target.genitals.SetBlackNipples(false);
			}
			//Remove extra nipples
			if (target.genitals.hasQuadNipples && Utils.Rand(3) == 0)
			{
				target.genitals.SetQuadNipples(false);
			}
			//Hair turns normal
			//Restart hair growth, if hair's normal but growth isn't on. Or just over turning hair normal. The power of rng.
			if ((target.hair.type != HairType.NORMAL || target.hair.growthArtificallyDisabled) && Utils.Rand(3) == 0)
			{
				target.UpdateHair(HairType.NORMAL);
				if (target.hair.growthArtificallyDisabled)
				{
					target.hair.SetHairGrowthStatus(true);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-----------------------
			// EXTRA PARTS REMOVAL
			//-----------------------
			//Removes antennae
			if (target.antennae.type != AntennaeType.NONE && Utils.Rand(3) == 0)
			{
				target.RestoreAntennae();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Removes horns
			if ((target.horns.type != HornType.NONE) && Utils.Rand(5) == 0)
			{
				target.RestoreHorns();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Removes wings
			if (target.wings.type != WingType.NONE && Utils.Rand(5) == 0)
			{
				target.RestoreWings();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Removes tail
			if (target.tail.type != TailType.NONE && Utils.Rand(5) == 0)
			{
				target.RestoreTail();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Increase height up to 4ft 10in.
			if (Utils.Rand(2) == 0 && target.build.heightInInches < 58)
			{
				int temp = Utils.Rand(5) + 3;
				//Flavor texts. Flavored like 1950's cigarettes. Yum.
				target.build.IncreaseHeight((byte)temp);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Decrease height down to a maximum of 6ft 2in.
			if (Utils.Rand(2) == 0 && target.build.heightInInches > 74)
			{
				target.build.DecreaseHeight((byte)(3 + Utils.Rand(5)));
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-----------------------
			// SEXUAL TRANSFORMATIONS
			//-----------------------
			//Remove additional cocks
			if (target.cocks.Count > 1 && Utils.Rand(3) == 0)
			{
				target.RemoveCock();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Remove additional balls/remove uniball
			if (target.balls.hasBalls && (target.balls.count != 2 || target.balls.uniBall) && Utils.Rand(3) == 0)
			{
				if (target.balls.size > 2)
				{
					if (target.balls.size > 5)
					{
						target.balls.ShrinkBalls((byte)(1 + Utils.Rand(3)));
					}

					target.balls.ShrinkBalls(1);
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
				else if (target.balls.count > 2)
				{
					target.balls.RemoveExtraBalls();

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
				else //if (target.balls.count == 1 || target.balls.uniBall)
				{
					target.balls.MakeStandard();

					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}

			//remove second vagina.
			if (target.vaginas.Count > 1 && Utils.Rand(3) == 0)
			{
				target.genitals.RemoveExtraVaginas();
			}

			//Change cock back to normal
			if (target.hasCock && !target.genitals.OnlyHasCocksOfType(CockType.HUMAN) && Utils.Rand(3) == 0)
			{
				//Select first non-human cock
				var firstNonHuman = target.cocks.First(x => x.type != CockType.HUMAN);

				target.genitals.UpdateCock(firstNonHuman, CockType.HUMAN);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			var longest = target.genitals.LongestCock();
			float targetSize = Math.Max(7, target.genitals.minimumCockLength);

			//Shrink oversized cocks
			if (target.hasCock && longest.length > targetSize && Utils.Rand(3) == 0)
			{
				longest.DecreaseLength((Utils.Rand(10) + 2) / 10f);
				if (longest.girth > 1)
				{
					longest.DecreaseThickness((Utils.Rand(4) + 1) / 10f);
				}
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Remove additional breasts
			if (target.breasts.Count > 1 && Utils.Rand(3) == 0)
			{
				target.RemoveExtraBreastRows();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			var biggestCup = target.genitals.LargestBreast();
			var targetCup = EnumHelper.Max(CupSize.D, target.genitals.smallestPossibleFemaleCupSize);
			//Shrink tits!
			if (Utils.Rand(3) == 0 && biggestCup.cupSize > targetCup)
			{
				foreach (var tits in target.breasts)
				{
					tits.ShrinkBreasts();
				}
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Change vagina back to normal
			if (Utils.Rand(3) == 0 && target.hasVagina && !target.genitals.OnlyHasVaginasOfType(VaginaType.defaultValue))
			{
				foreach (var vag in target.vaginas)
				{
					target.genitals.RestoreVagina(vag);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			var targetWetness = EnumHelper.Max(VaginalWetness.WET, target.genitals.minVaginalWetness);
			//Reduce wetness down to a minimum of 2
			if (Utils.Rand(3) == 0 && target.hasVagina && target.genitals.LargestVaginalWetness() > targetWetness)
			{
				foreach (var vag in target.vaginas)
				{
					if (vag.wetness > targetWetness)
					{
						vag.DecreaseWetness();
					}
				}
					if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Fertility Decrease:
			if (target.hasVagina && target.fertility.baseFertility > 10 && Utils.Rand(3) == 0)
			{
				//High fertility:
				//Average fertility:

				target.fertility.DecreaseFertility((byte)(1 + Utils.Rand(3)));
				if (target.fertility.baseFertility < 10)
				{
					target.fertility.SetFertility(10);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Cum Multiplier Decrease:
			if (target.hasCock && target.genitals.cumMultiplier > 5 && Utils.Rand(3) == 0)
			{
				target.genitals.DecreaseCumMultiplier(1 + (Utils.Rand(20) / 10f));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Anal wetness decrease
			if (target.ass.wetness > 0 && Utils.Rand(3) == 0)
			{
				target.ass.DecreaseWetness();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}


			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target);
	}
}