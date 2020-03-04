//BimBro.cs
//Description:
//Author: JustSomeGuy
//6/27/2019, 6:32 PM

using System;
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Perks;
using CoC.Backend.Reaction;
using CoC.Backend.Tools;

namespace CoC.Frontend.Perks
{
	//this isn't meant to be removed. if it is, we will simply leave the affected body parts as they are.
	public sealed class BimBro : StandardPerk
	{
		public bool hasBroEffect { get; private set; } = false;
		public bool hasBimboEffect { get; private set; } = false;

		public bool futaBody => hasBroEffect && hasBimboEffect;
		public bool bimboBody => hasBimboEffect && !hasBroEffect;
		public bool broBody => hasBroEffect && !hasBimboEffect;

		private Gender targetGender
		{
			get
			{
				Gender curr = Gender.GENDERLESS;
				curr |= hasBroEffect ? Gender.MALE : Gender.GENDERLESS;
				curr |= hasBimboEffect ? Gender.FEMALE : Gender.GENDERLESS;
				return curr;
			}
		}

		public bool bimbroBrains { get; private set; }

		private bool eventFiring;

		public BimBro(Gender gender, bool withBrains = true) : base()
		{
			if (gender == Gender.GENDERLESS)
			{
				gender = Gender.MALE;
			}
			if (gender.HasFlag(Gender.MALE))
			{
				hasBroEffect = true;
			}
			if (gender.HasFlag(Gender.FEMALE))
			{
				hasBimboEffect = true;
			}

			bimbroBrains = withBrains;

			sourceCreature.genitals.onGenderChanged += GenderChangeCheck;
			sourceCreature.breasts[0].dataChange += BreastChange;
			sourceCreature.butt.dataChange += ButtChange;
			sourceCreature.hips.dataChange += HipsChange;
		}

		private void HipsChange(object sender, SimpleDataChangeEvent<Hips, HipData> e)
		{
			if (hasBimboEffect)
			{
				if (e.newValues.size < 12)
				{
					FixBadData();
				}
			}
		}

		private void ButtChange(object sender, SimpleDataChangeEvent<Butt, ButtData> e)
		{
			if (hasBimboEffect)
			{
				if (e.newValues.size < 12)
				{
					FixBadData();
				}
			}
		}

		private void BreastChange(object sender, SimpleDataChangeEvent<Breasts, BreastData> e)
		{
			if (hasBimboEffect)
			{
				if (e.newValues.cupSize < CupSize.DD)
				{
					FixBadData();
				}
			}
		}

		private void GenderChangeCheck(object sender, GenderChangedEventArgs e)
		{
			if (futaBody)
			{
				if (e.newGender != Gender.HERM)
				{
					FixBadData();
				}
			}
			else if (hasBroEffect)
			{
				if (!e.newGender.HasFlag(Gender.MALE))
				{
					FixBadData();
				}
			}
			else if (hasBimboEffect)
			{
				if (!e.newGender.HasFlag(Gender.FEMALE))
				{
					FixBadData();
				}
			}
		}

		private void FixBadData()
		{
			if (!eventFiring)
			{
				eventFiring = true;
				GameEngine.AddOneOffReaction(new OneOffTimeReaction(new GenericSimpleReaction(CorrectGenderReaction), 4, true));
			}
		}

		private string CorrectGenderReaction(bool currentlyIdling, bool hasIdleHours)
		{
			StringBuilder sb = new StringBuilder();
			//male checks.
			if (hasBroEffect)
			{ //Futa checks
				if (!sourceCreature.hasCock)
				{ //(Dick regrowth)
					sourceCreature.AddCock(CockType.defaultValue, 10, 2.75);
					sb.Append(Environment.NewLine + "<b>As time passes, your loins grow itchy for a moment. A split-second later, a column of flesh erupts from your crotch. " +
						"Your new, 10-inch cock pulses happily.");
					if (!sourceCreature.balls.hasBalls)
					{
						sb.Append(" A pair of heavy balls drop into place below it, churning to produce cum.");
						sourceCreature.balls.GrowBalls(2, 3);
					}

					sourceCreature.DeltaCreatureStats(inte: -1, sens: 5, lus: 15);
					sb.Append("</b>" + Environment.NewLine);
				}
				if (sourceCreature.cocks[0].length < 8)
				{ //(Dick rebiggening)
					sb.Append(Environment.NewLine + "<b>As time passes, your cock engorges, flooding with blood and growing until it's at 8 inches long. " +
						"You really have no control over your dick.</b>" + Environment.NewLine);
					sourceCreature.cocks[0].SetLength(8);
					if (sourceCreature.cocks[0].girth < 2)
					{
						sourceCreature.cocks[0].SetGirth(2);
					}
				}
				if (sourceCreature.balls.count == 0)
				{ //(Balls regrowth)
					sb.Append(Environment.NewLine + "<b>As time passes, a pressure in your loins intensifies to near painful levels. The skin beneath "
						+ sourceCreature.genitals.AllCocksShortDescription() + " grows loose and floppy, and then two testicles roll down to fill your scrotum.</b>"
						+ Environment.NewLine);
					sourceCreature.balls.GrowBalls(2, 3);
				}
			}
			if (hasBimboEffect)
			{
				if (sourceCreature.breasts[0].cupSize < CupSize.DD)
				{ //Tits!
					BreastData oldBreastRowData = sourceCreature.breasts[0].AsReadOnlyData();
					sourceCreature.breasts[0].SetCupSize(CupSize.DD);

					if (bimbroBrains)
					{
						if (hasBroEffect)
						{
							sb.Append(Environment.NewLine + "<b>Your tits get nice and full again. You'll have lots of fun now that your breasts are back to being big, " +
								"swollen knockers!</b>" + Environment.NewLine);
						}
						else
						{
							sb.Append(Environment.NewLine + "<b>Your boobies like, get all big an' wobbly again! You'll have lots of fun now that your tits are back " +
								"to being big, yummy knockers!</b>" + Environment.NewLine);
						}

					}
					else
					{
						sb.Append(Environment.NewLine + "<b>Your " + oldBreastRowData.LongDescription() + " have regained their former bimbo-like size. " +
						"It looks like you'll be stuck with large, sensitive breasts forever, but at least it'll help you tease your enemies into submission!</b>"
						+ Environment.NewLine);
					}

					sourceCreature.DeltaCreatureStats(inte: -1, lus: 15);
				}
				if (!sourceCreature.hasVagina)
				{ //Vagoo
					sourceCreature.AddVagina();

					if (bimbroBrains && futaBody)
					{
						sb.Append(Environment.NewLine + "<b>Your crotch is like, all itchy an' stuff. Damn! There's a wet little slit opening up, and it's all tingly! It feels so good, why would you have ever gotten rid of it?</b>" + Environment.NewLine);

					}
					else if (bimbroBrains)
					{
						sb.Append(Environment.NewLine + "<b>Your crotch is like, all itchy an' stuff. Omigawsh! There's a wet little slit opening up, and it's all tingly! It feels so good, maybe like, someone could put something inside there!</b>" + Environment.NewLine);

					}
					else
					{
						sb.Append(Environment.NewLine + "<b>Your crotch tingles for a second, and when you reach down to feel, your " + sourceCreature.lowerBody.LongDescription() +
							" fold underneath you, limp. You've got a vagina - the damned thing won't go away and it feels twice as sensitive this time. ");

						sb.Append((futaBody ? "Fucking futa-causing shit!" : "Fucking bimbo-causing shit!") + "</b>" + Environment.NewLine);

					}
					sourceCreature.DeltaCreatureStats(inte: -1, sens: 10, lus: 15);
				}

				if (!futaBody)
				{
					if (sourceCreature.hips.size < 12)
					{
						if (bimbroBrains)
						{
							sb.Append(Environment.NewLine + "Whoah! As you move, your " + sourceCreature.build.HipsLongDescription() + " sway farther and farther to each side, " +
								"expanding with every step, soft new flesh filling in as your hips spread into something more appropriate on a tittering bimbo. " +
								"You giggle when you realize you can't walk any other way. At least it makes you look, like, super sexy!" + Environment.NewLine);
						}
						else
						{
							sb.Append(Environment.NewLine + "Oh, no! As you move, your " + sourceCreature.build.HipsLongDescription() + " sway farther and farther " +
								"to each side, expanding with every step, soft new flesh filling in as your hips spread into something more appropriate for a bimbo. " +
								"Once you realize that you can't walk any other way, you sigh heavily, your only consolation the fact that your widened hips " +
								"can be used to tease more effectively." + Environment.NewLine);
						}

						sourceCreature.DeltaCreatureStats(inte: -1);
						sourceCreature.hips.SetHipSize(12);
					}
					if (sourceCreature.butt.size < 12)
					{
						if (bimbroBrains)
						{
							sb.Append(Environment.NewLine + "Gradually warming, you find that your " + sourceCreature.build.ButtLongDescription() + " is practically " +
								"sizzling with erotic energy. You smile to yourself, imagining how much you wish you had a nice, plump, bimbo-butt again, " +
								"your hands finding their way to the flesh on their own. Like, how did they get down there? You bite your lip when you realize " +
								"how good your tush feels in your hands, particularly when it starts to get bigger. Are butts supposed to do that? Happy pink thoughts " +
								"wash that concern away - it feels good, and you want a big, sexy butt! The growth stops eventually, and you pout disconsolately " +
								"when the lusty warmth's last lingering touches dissipate. Still, you smile when you move and feel your new booty jiggling along behind you. " +
								"This will be fun!" + Environment.NewLine);
						}
						else
						{
							sb.Append(Environment.NewLine + "Gradually warming, you find that your " + sourceCreature.build.ButtLongDescription() + " is practically " +
								"sizzling with erotic energy. Oh, no! You thought that having a big, bloated bimbo-butt was a thing of the past, but with how " +
								"it's tingling under your groping fingertips, you have no doubt that you're about to see the second coming of your sexy ass. Wait, " +
								"how did your fingers get down there? You pull your hands away somewhat guiltily as you feel your buttcheeks expanding. Each time " +
								"you bounce and shake your new derriere, you moan softly in enjoyment. Damnit! You force yourself to stop just as your ass does, " +
								"but when you set off again, you can feel it bouncing behind you with every step. At least it'll help you tease your foes a " +
								"little more effectively..." + Environment.NewLine);
						}

						sourceCreature.DeltaCreatureStats(inte: -1, lus: 10);
						sourceCreature.butt.SetButtSize(12);
					}


				}

			}


			eventFiring = false;
			return sb.ToString();
		}

		public override string Name()
		{
			if (futaBody)
			{
				return "Futa Form";
			}
			else if (hasBimboEffect)
			{
				return "Bimbo Body";
			}
			else
			{
				return "Bro Body";
			}
		}

		public override string HasPerkText()
		{
			if (futaBody)
			{
				return "Ensures that your body fits the Futa look (Tits DD+, Dick 8\"+, & Pussy). Also keeps your lusts burning bright and improves the tease skill.";
			}
			else if (hasBimboEffect)
			{
				return "Gives the body of a bimbo. Tits will never stay below a 'DD' cup, libido is raised, lust resistance is raised, and upgrades tease.";
			}
			else
			{
				return "Grants an ubermasculine body that's sure to impress, and ensures your dick is equally impressive. Raises your lust and upgrades the tease skill.";
			}
		}

		protected override bool keepOnAscension => false;

		protected override void OnActivation()
		{
			if (sourceCreature is CombatCreature combat && bimbroBrains)
			{
				// drop intelligence to 1/5 of current value, minimum of 10.
				byte intell = (byte)Math.Round(combat.intelligenceTrue / 5);
				if (intell < 10)
				{
					intell = 10;
				}

				combat.SetIntelligence(intell);
			}

			if (hasBimboEffect && hasBroEffect)
			{
				DoFuta(false);
			}
			else if (hasBimboEffect)
			{
				DoBimbo(false);
			}
			else if (hasBroEffect)
			{
				DoBro(false);
			}

			if (bimbroBrains)
			{
				MakeDum();
			}
		}

		public void Broify(bool withBrains = true)
		{
			if (hasBimboEffect)
			{
				DoFuta(true);
			}
			else
			{
				DoBro(hasBroEffect);
			}
			//make dum only fires if not already under the effects of bimbo brain
			if (withBrains)
			{
				MakeDum();
			}
		}

		public void Bimbify(bool withBrains = true)
		{
			if (hasBroEffect)
			{
				DoFuta(true);
			}
			else
			{
				DoBimbo(hasBimboEffect);
			}
			//make dum only fires if not already under the effects of bimbo brain
			if (withBrains)
			{
				MakeDum();
			}
		}

		private void MakeDum()
		{
			if (!bimbroBrains)
			{
				bimbroBrains = true;
				AddModifierToPerk(baseModifiers.intelligenceGainMultiplier, new ValueModifierStore<double>(ValueModifierType.MINIMUM, 0.25));
			}
		}

		public void NegateBimbroBrains()
		{
			if (bimbroBrains)
			{
				RemoveModifierFromPerk(baseModifiers.intelligenceGainMultiplier);
			}
		}

		private void DoBro(bool alreadyApplied)
		{
			//og game had repeats more or less ignored. personally i think it makes more
			//sense to reapply it, so that's what i'm doing here. however, the inessentials
			//will be skipped over, hence the check above.

			if (sourceCreature.genitals.numCocks == 0)
			{
				sourceCreature.genitals.AddCock(CockType.defaultValue, 12, 2.75);
			}
			else if (sourceCreature.genitals.ShortestCockLength() < 10)
			{
				sourceCreature.cocks.ForEach(x =>
				{
					if (x.length < 10)
					{
						x.SetLength(10);
					}

					if (x.girth < 2.75)
					{
						x.SetGirth(2.75);
					}
				});
			}

			if (!sourceCreature.genitals.balls.hasBalls)
			{
				sourceCreature.genitals.GrowBalls(2, 3);
			}

			if (sourceCreature.hasVagina)
			{
				sourceCreature.RemoveAllVaginas();
			}

			if (sourceCreature.genitals.BiggestCupSize() > sourceCreature.genitals.smallestPossibleMaleCupSize)
			{
				sourceCreature.breasts.ForEach(x => x.MakeMale());
				sourceCreature.genitals.SetQuadNipples(false);
				sourceCreature.genitals.SetNippleStatus(NippleStatus.NORMAL);
			}
			if (!alreadyApplied)
			{
				if (sourceCreature.build.heightInInches < 77)
				{
					sourceCreature.build.SetHeight(77);
				}

				if (sourceCreature.genitals.numBreastRows > 1)
				{
					sourceCreature.genitals.RemoveExtraBreastRows();
				}

				if (sourceCreature.genitals.femininity != Femininity.MOST_MASCULINE)
				{
					sourceCreature.genitals.SetFemininity(Femininity.MOST_MASCULINE);
				}

				sourceCreature.build.SetMuscleTone(Build.TONE_PERFECTLY_DEFINED);

				sourceCreature.build.ChangeThicknessToward(80, 50);

				sourceCreature.DeltaCreatureStats(str: 33, tou: 33, lib: 4, lus: 40, ignorePerks: false);

				//update min size and delta, adding 4.5.

				AddModifierToPerk(baseModifiers.newCockSizeDelta, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 4.5));
				AddModifierToPerk(baseModifiers.minCockSize, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 4.5));
			}
			if (basicData.HasPerk<Feeder>())
			{
				basicData.RemovePerk<Feeder>();
			}

			hasBroEffect = true;
		}

		private void DoBimbo(bool alreadyApplied)
		{

			if (!alreadyApplied)
			{
				AddModifierToPerk(baseModifiers.femaleMinCupSize, new ValueModifierStore<byte>(ValueModifierType.MINIMUM, (byte)CupSize.DD));
				AddModifierToPerk(baseModifiers.minButtSize, new ValueModifierStore<byte>(ValueModifierType.MINIMUM, 12));
				AddModifierToPerk(baseModifiers.minHipsSize, new ValueModifierStore<byte>(ValueModifierType.MINIMUM, 12));

				//restore any hair that would prevent us from growing it.
				if (!sourceCreature.hair.type.growsOverTime || sourceCreature.hair.type.isFixedLength)
				{
					sourceCreature.RestoreHair();
				}

				//then remove any hair stoppage via artificial means
				sourceCreature.hair.ResumeNaturalGrowth();
				sourceCreature.hair.SetHairColor(HairFurColors.PLATINUM_BLONDE);

				if (sourceCreature.hair.length < 36)
				{
					sourceCreature.hair.SetHairLength(36);
				}

				if (sourceCreature.build.heightInInches < 77)
				{
					sourceCreature.build.SetHeight(77);
				}

				if (sourceCreature.relativeStrength > 30)
				{
					double temp = (sourceCreature.relativeStrength - 30) / 2 + 5;
					sourceCreature.DecreaseStrength(temp);
				}

				sourceCreature.build.SetMuscleTone(Build.TONE_FLABBY);

			}

			if (sourceCreature.breasts[0].cupSize < CupSize.E)
			{ //Tits!
				sourceCreature.breasts[0].SetCupSize(CupSize.E);
				sourceCreature.DeltaCreatureStats(inte: -1, lus: 15);
			}
			if (!sourceCreature.hasVagina)
			{ //Vagoo
				sourceCreature.AddVagina(0.25, VaginalLooseness.NORMAL, VaginalWetness.SLICK);

			}
			else if (sourceCreature.genitals.SmallestVaginalWetness() < VaginalWetness.SLICK)
			{
				sourceCreature.vaginas.Where(x => x.wetness < VaginalWetness.SLICK).ForEach(x => x.SetVaginalWetness(VaginalWetness.SLICK));
			}
			if (sourceCreature.hips.size < 12)
			{
				//sb.Append(Environment.NewLine + "Whoah! As you move, your " + sourceCreature.build.HipsLongDescription() + " sway farther and farther to each side, expanding with every step, soft new flesh filling in as your hips spread into something more appropriate on a tittering bimbo. You giggle when you realize you can't walk any other way. At least it makes you look, like, super sexy!" + Environment.NewLine);
				sourceCreature.DeltaCreatureStats(inte: -1);
			}
			if (sourceCreature.butt.size < 12)
			{
				//sb.Append(Environment.NewLine + "Gradually warming, you find that your " + sourceCreature.build.ButtLongDescription() + " is practically sizzling with erotic energy. You smile to yourself, imagining how much you wish you had a nice, plump, bimbo-butt again, your hands finding their way to the flesh on their own. Like, how did they get down there? You bite your lip when you realize how good your tush feels in your hands, particularly when it starts to get bigger. Are butts supposed to do that? Happy pink thoughts wash that concern away - it feels good, and you want a big, sexy butt! The growth stops eventually, and you pout disconsolately when the lusty warmth's last lingering touches dissipate. Still, you smile when you move and feel your new booty jiggling along behind you. This will be fun!" + Environment.NewLine);
				sourceCreature.DeltaCreatureStats(inte: -1, lus: 10);
			}


			hasBimboEffect = true;
		}

		private void DoFuta(bool activatedBefore)
		{
			//first activation. somehow, an item caused you to directly get the futa perk.
			if (!activatedBefore)
			{
				//restore any hair that would prevent us from growing it.
				if (!sourceCreature.hair.type.growsOverTime || sourceCreature.hair.type.isFixedLength)
				{
					sourceCreature.RestoreHair();
				}

				//then remove any hair stoppage via artificial means
				sourceCreature.hair.ResumeNaturalGrowth();
				sourceCreature.hair.SetHairColor(HairFurColors.PLATINUM_BLONDE);

				if (sourceCreature.hair.length < 36)
				{
					sourceCreature.hair.SetHairLength(36);
				}

				if (sourceCreature.build.heightInInches < 77)
				{
					sourceCreature.build.SetHeight(77);
				}
				if (sourceCreature.breasts[0].cupSize < CupSize.DD)
				{ //Tits!
					sourceCreature.breasts[0].SetCupSize(CupSize.DD);
					sourceCreature.DeltaCreatureStats(inte: -1, lus: 15);
				}
				if (!sourceCreature.hasVagina)
				{ //Vagoo
					sourceCreature.AddVagina(0.25, VaginalLooseness.NORMAL, VaginalWetness.SLICK);

				}
				else if (sourceCreature.genitals.SmallestVaginalWetness() < VaginalWetness.SLICK)
				{
					sourceCreature.vaginas.Where(x => x.wetness < VaginalWetness.SLICK).ForEach(x => x.SetVaginalWetness(VaginalWetness.SLICK));
				}
				if (sourceCreature.genitals.numCocks == 0)
				{
					sourceCreature.genitals.AddCock(CockType.defaultValue, 10, 2);
				}
				else if (sourceCreature.genitals.ShortestCockLength() < 10)
				{
					foreach (Cock x in sourceCreature.cocks)
					{
						if (x.length < 10)
						{
							x.SetLength(10);
						}

						if (x.girth < 2)
						{
							x.SetGirth(2);
						}
					}
				}

				if (!sourceCreature.genitals.balls.hasBalls)
				{
					sourceCreature.genitals.GrowBalls(2, 3);
				}

				AddModifierToPerk(baseModifiers.femaleMinCupSize, new ValueModifierStore<byte>(ValueModifierType.MINIMUM, (byte)CupSize.DD));
				AddModifierToPerk(baseModifiers.newCockSizeDelta, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 4.5));
				AddModifierToPerk(baseModifiers.minCockSize, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 4.5));
			}
			//already active. already a futa.
			else if (!futaBody)
			{
				//already active. already a bimbo, need bro related values.
				if (hasBimboEffect)
				{
					if (sourceCreature.genitals.numCocks == 0)
					{
						sourceCreature.genitals.AddCock(CockType.defaultValue, 10, 2);
					}
					else if (sourceCreature.genitals.ShortestCockLength() < 10)
					{
						foreach (Cock x in sourceCreature.cocks)
						{
							if (x.length < 10)
							{
								x.SetLength(10);
							}

							if (x.girth < 2)
							{
								x.SetGirth(2);
							}
						}
					}

					if (!sourceCreature.genitals.balls.hasBalls)
					{
						sourceCreature.genitals.GrowBalls(2, 3);
					}
					//apparently, these aren't required for futas. removing them for now.
					RemoveModifierFromPerk(baseModifiers.minButtSize);
					RemoveModifierFromPerk(baseModifiers.minHipsSize);
					//
					AddModifierToPerk(baseModifiers.newCockSizeDelta, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 4.5));
					AddModifierToPerk(baseModifiers.minCockSize, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 4.5));
				}
				//already active. already a bro, need bimbo related values.
				else
				{
					//restore any hair that would prevent us from growing it.
					if (!sourceCreature.hair.type.growsOverTime || sourceCreature.hair.type.isFixedLength)
					{
						sourceCreature.RestoreHair();
					}

					//then remove any hair stoppage via artificial means
					sourceCreature.hair.ResumeNaturalGrowth();
					sourceCreature.hair.SetHairColor(HairFurColors.PLATINUM_BLONDE);

					if (sourceCreature.hair.length < 36)
					{
						sourceCreature.hair.SetHairLength(36);
					}

					if (sourceCreature.build.heightInInches < 77)
					{
						sourceCreature.build.SetHeight(77);
					}


					if (!sourceCreature.hasVagina)
					{ //Vagoo
						sourceCreature.AddVagina(0.25, VaginalLooseness.NORMAL, VaginalWetness.SLICK);

					}
					else if (sourceCreature.genitals.SmallestVaginalWetness() < VaginalWetness.SLICK)
					{
						sourceCreature.vaginas.Where(x => x.wetness < VaginalWetness.SLICK).ForEach(x => x.SetVaginalWetness(VaginalWetness.SLICK));
					}
					if (sourceCreature.breasts[0].cupSize < CupSize.DD)
					{ //Tits!
						sourceCreature.breasts[0].SetCupSize(CupSize.DD);
						sourceCreature.DeltaCreatureStats(inte: -1, lus: 15);
					}

					AddModifierToPerk(baseModifiers.femaleMinCupSize, new ValueModifierStore<byte>(ValueModifierType.MINIMUM, (byte)CupSize.DD));
				}
			}
			//else, already a futa. do nothing.

			hasBimboEffect = true;
			hasBroEffect = true;
		}

		protected override void OnRemoval()
		{ }
	}
}
