//BimBro.cs
//Description:
//Author: JustSomeGuy
//6/27/2019, 6:32 PM

using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Events;
using CoC.Backend.Engine.Time;
using CoC.Backend.Perks;
using CoC.Backend.Tools;
using System;
using System.Linq;
using System.Text;

namespace CoC.Frontend.Perks
{
	public sealed class BimBro : PerkBase
	{
		public bool broBody { get; private set; } = false;
		public bool bimboBody { get; private set; } = false;

		public bool futaBody => broBody && bimboBody;

		private Gender targetGender
		{
			get
			{
				Gender curr = Gender.GENDERLESS;
				curr |= broBody ? Gender.MALE : Gender.GENDERLESS;
				curr |= bimboBody ? Gender.FEMALE : Gender.GENDERLESS;
				return curr;
			}
		}

		public bool bimbroBrains { get; private set; }

		private sbyte breastDelta = 0;
		private float cockDelta = 0;

		private CupSize? breastNew = null;
		private float? cockNew = null;

		private float intGainDelta = 0;
		private float intLossDelta = 0;

		private bool eventFiring;

		private bool activated = false;

		public BimBro(Gender gender) : base(Name, Text)
		{
			if (gender == Gender.GENDERLESS)
			{
				gender = Gender.MALE;
			}
			if (gender.HasFlag(Gender.MALE))
			{
				broBody = true;
			}
			if (gender.HasFlag(Gender.FEMALE))
			{
				bimboBody = true;
			}

			bimbroBrains = true;

			sourceCreature.genitals.onGenderChanged += GenderChangeCheck;
		}

		private void GenderChangeCheck(object sender, Backend.BodyParts.EventHelpers.GenderChangedEventArgs e)
		{
			if (futaBody)
			{
				if (e.newGender != Gender.HERM)
				{
					AddCorrectGenderReaction();
				}
			}
			else if (broBody)
			{
				if (!e.newGender.HasFlag(Gender.MALE))
				{
					AddCorrectGenderReaction();
				}
			}
			else if (bimboBody)
			{
				if (!e.newGender.HasFlag(Gender.FEMALE))
				{
					AddCorrectGenderReaction();
				}
			}
		}

		private void AddCorrectGenderReaction()
		{
			if (!eventFiring)
			{
				eventFiring = true;
				GameEngine.AddTimeReaction(new TimeReaction(OnCorrectEvent, 4, true));
			}
		}

		private EventWrapper OnCorrectEvent()
		{
			bool grewBalls = false;
			bool enlargedBreasts = false;
			//bool shrunkBreasts = false; //add in if event is to fix tits to male size in reaction.
			bool grewCock = false;
			bool grewVagina = false;
			bool enlargedCock = false;

			EventWrapper retVal;

			Gender target = targetGender;
			if (sourceCreature.genitals.gender != target)
			{
				if (targetGender.HasFlag(Gender.MALE)) //male, but could be herm.
				{
					if (!sourceCreature.genitals.balls.hasBalls)
					{
						grewBalls = true;
						sourceCreature.genitals.GrowBalls(2, 3);
					}

					if (sourceCreature.genitals.numCocks == 0)
					{
						grewCock = true;
						sourceCreature.genitals.AddCock(CockType.defaultValue, 10, 2);
					}
					else if (sourceCreature.genitals.ShortestCockLength(false) < 8)
					{
						enlargedCock = true;
						sourceCreature.cocks.ForEach(x => { if (x.length < 8) x.SetLength(8); });
					}
				}

				if (targetGender.HasFlag(Gender.FEMALE)) //female or herm.
				{
					if (sourceCreature.genitals.SmallestCupSize() < CupSize.C)
					{
						enlargedBreasts = true;
						sourceCreature.breasts.ForEach(x => { if (x.cupSize < CupSize.C) x.setCupSize(CupSize.C); });
					}

					if (sourceCreature.genitals.numVaginas == 0)
					{
						grewVagina = true;
						sourceCreature.AddVagina(VaginaType.defaultValue);
					}
				}
				//we don't correct tits apparently for male only in player events.

				retVal = (EventWrapper)CorrectedGenderText(grewCock, grewBalls, enlargedCock, enlargedBreasts, grewVagina);

			}
			else
			{
				retVal = EventWrapper.Empty;
			}
			eventFiring = false;
			return retVal;
		}

		private string CorrectedGenderText(bool grewCock, bool grewBalls, bool enlargedCock, bool enlargedBreasts, bool grewVagina)
		{
			throw new NotImplementedException();
		}

		public BimBro() : this(Gender.MALE) { }

		private static string Name()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Text()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		protected override bool KeepOnAscension => false;

		protected override void OnActivation()
		{
			if (sourceCreature is CombatCreature combat && bimbroBrains)
			{
				// drop intelligence to 1/5 of current value, minimum of 10.
				byte intell = (byte)Math.Round(combat.intelligenceTrue / 5);
				if (intell < 10) intell = 10;
				combat.SetIntelligence(intell);
			}

			if (bimboBody && broBody)
			{
				DoFuta();
			}
			else if (bimboBody)
			{
				DoBimbo();
			}
			else if (broBody)
			{
				DoBro();
			}

			if (bimbroBrains)
			{
				MakeDum();
			}




			activated = true;


		}

		public void Broify(bool withBrains = true)
		{
			//if (withBrains && !bimbroBrains)
			//{
			//	bimbroBrains = true;
			//}
			if (activated)
			{
				if (bimboBody)
				{
					DoFuta();
				}
				else
				{
					DoBro();
				}
				//make dum only fires if not already under the effects of bimbo brain
				if (withBrains)
				{
					MakeDum();
				}
			}
			else
			{
				bimbroBrains |= withBrains; //if already true, keep true. if not and withbrains, make true.
				broBody = true;
			}
		}

		public void Bimbify (bool withBrains = true)
		{
			bimboBody = true;

			if (withBrains)
			{
				bimbroBrains = true;
			}
			if (activated)
			{
				if (broBody)
				{
					DoFuta();
				}
				else
				{
					DoBimbo();
				}
				//make dum only fires if not already under the effects of bimbo brain
				if (withBrains)
				{
					MakeDum();
				}
			}
			else
			{
				bimbroBrains |= withBrains;
				bimboBody = true;
			}
		}

		private void MakeDum()
		{
			if (!activated || !bimbroBrains)
			{
				float curr;
				curr = baseModifiers.IntelligenceGainMultiplier;
				baseModifiers.IntelligenceGainMultiplier -= 0.5f;
				intGainDelta = curr - baseModifiers.IntelligenceGainMultiplier;

				curr = baseModifiers.IntelligenceLossMultiplier;
				baseModifiers.IntelligenceLossMultiplier += 0.5f;
				intLossDelta = baseModifiers.IntelligenceLossMultiplier - curr;

				bimbroBrains = true;
			}
		}

		public void NegateBimbroBrains()
		{
			if (bimbroBrains)
			{
				bimbroBrains = false;
				baseModifiers.IntelligenceGainMultiplier += intGainDelta;
				baseModifiers.IntelligenceLossMultiplier -= intLossDelta;
			}
		}

		private void DoBro()
		{
			//if we've already applied bro body
			bool alreadyApplied = activated && broBody;

			//og game had repeats more or less ignored. personally i think it makes more 
			//sense to reapply it, so that's what i'm doing here. however, the inessentials
			//will be skipped over, hence the check above.

			if (sourceCreature.genitals.numCocks == 0)
			{
				sourceCreature.genitals.AddCock(CockType.defaultValue, 10, 2);
			}

			if (!sourceCreature.genitals.balls.hasBalls)
			{
				sourceCreature.genitals.GrowBalls(2, 3);
			}			
			else if (sourceCreature.genitals.ShortestCockLength(false) < 10)
			{
				sourceCreature.cocks.ForEach(x => 
				{
					if (x.length < 10) x.SetLength(10);
					if (x.girth < 2.75) x.SetGirth(2.75f);
				});
			}

			if (sourceCreature.genitals.BiggestCupSize() > sourceCreature.genitals.MaleMinCup)
			{
				sourceCreature.breasts.ForEach(x => x.MakeMale());
			}
			if (!alreadyApplied)
			{
				if (sourceCreature.genitals.numBreastRows > 1)
				{
					sourceCreature.genitals.RemoveExtraBreastRows();
				}

				if (sourceCreature.genitals.femininity != Femininity.MOST_MASCULINE)
				{
					sourceCreature.genitals.SetFemininity(Femininity.MOST_MASCULINE);
				}

				sourceCreature.build.SetMuscleTone(Build.TONE_PERFECTLY_DEFINED);

				//attempt to reach 80 thickness, by adding 50. if over 80, cap at 80.
				byte newThickness = Math.Min(sourceCreature.build.thickness.add(50), Build.THICKNESS_HUGE);
				sourceCreature.build.SetThickness(newThickness);

				if (sourceCreature is CombatCreature combat)
				{
					combat.DeltaCombatCreatureStats(str: 33, tou: 33, lib: 4, lus: 40, ignorePerks: false);
				}
				else
				{
					sourceCreature.IncreaseCreatureStats(lus: 40, lib: 4, ignorePerks: false);
				}

				//update min size and delta, adding 4.5.

				float temp;
				temp = baseModifiers.NewCockDefaultSize;
				baseModifiers.NewCockDefaultSize += 4.5f; //5.5 -> 10. stacks with big cock (12.5)
				cockNew = baseModifiers.NewCockDefaultSize - temp;

				temp = baseModifiers.NewCockSizeDelta;
				baseModifiers.NewCockSizeDelta += 4.5f;
				cockDelta = baseModifiers.NewCockSizeDelta - temp;

			}
			if (this.basicData.HasPerk<Feeder>())
			{
				this.basicData.RemovePerk<Feeder>();
			}
		}

		private void DoBimbo()
		{
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private void DoFuta()
		{
			bool appliedBro = activated && broBody;
			bool appliedBimbo = activated && bimboBody;
			bool appliedFuta = appliedBimbo && appliedBro;

			if (appliedFuta)
			{

			}
			else if (appliedBro)
			{

			}
			else
			{

			}
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		protected override void OnRemoval()
		{
			baseModifiers.FemaleNewBreastCupSizeDelta -= breastDelta;
			baseModifiers.FemaleNewBreastDefaultCupSize -= breastNew ?? 0;

			baseModifiers.NewCockDefaultSize -= cockNew ?? 0;
			baseModifiers.NewCockSizeDelta -= cockDelta;

			if (bimbroBrains)
			{
				bimbroBrains = false;
				baseModifiers.IntelligenceGainMultiplier += intGainDelta;
				baseModifiers.IntelligenceLossMultiplier -= intLossDelta;
			}

			bimboBody = false;
			broBody = false;

			activated = false;
		}
	}
}
