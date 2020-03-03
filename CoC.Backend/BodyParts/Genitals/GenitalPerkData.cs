using System;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Perks;

namespace CoC.Backend.BodyParts
{
#warning ToDo: Finish rewiring all modifiers in here to their base modifiers. Also need to do this for others like butt, hips, infertile conditional.

	//doesn't need to be serialized - it's generated live when we create a creature.


	//Update: i don't actually think this class is necessary anymore - with the addition of modifiers, we don't need to calculate any values in the
	//base perk class manually, so this doesn't need to help handle that. still, i think having all the data aliased here makes it easier to maintain
	//because the genital related classes only have to update this - the base data can simply keep doing nothing.
	internal sealed class GenitalPerkData
	{
		private readonly Genitals source;

		private BasePerkModifiers baseModifiers => CreatureStore.GetCreatureClean(source.creatureID)?.perks.baseModifiers;

		internal GenitalPerkData(Genitals parent)
		{
			source = parent ?? throw new ArgumentNullException(nameof(parent));
		}


		#region Common
		internal bool TreatAllFluidsAsIfAtMaxLust => baseModifiers?.treatCalculationsAsIfAtMaxLust.GetValue() ?? false;
		#endregion

		#region Cock Perks
		internal double NewCockSizeDelta => baseModifiers?.newCockSizeDelta.GetValue() ?? 0;
		internal double CockGrowthMultiplier => baseModifiers?.cockGrowthMultiplier.GetValue() ?? 1;
		internal double CockShrinkMultiplier => baseModifiers?.cockShrinkMultiplier.GetValue() ?? 1;
		internal double NewCockDefaultSize => baseModifiers?.newCockDefaultSize.GetValue() ?? Cock.DEFAULT_COCK_LENGTH;
		internal double MinCockLength => baseModifiers?.minCockSize.GetValue() ?? Cock.MIN_COCK_LENGTH;

		internal void CockMinChange(double oldMinCockLen)
		{
			source.cocks.ForEach(x => x.ValidateLength());
		}

		internal double perkBonusVirilityMultiplier => baseModifiers?.perkBonusVirilityMultiplier.GetValue() ?? 1;

		internal sbyte perkBonusVirility => baseModifiers?.perkBonusVirility.GetValue() ?? 0;

		#endregion

		#region Cum Perks
		internal bool alwaysProducesMaxCum => baseModifiers?.alwaysProducingMaxCum.GetValue() ?? false;
		internal uint bonusCumAdded => baseModifiers?.bonusAdditionalCum.GetValue() ?? 0;
		internal double bonusCumMultiplier => baseModifiers?.bonusCumMultiplier.GetValue() ?? 0;

		internal void HandleCumChange()
		{
			//throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		#endregion

		#region Breast Perk Data
		internal sbyte FemaleNewCupDelta => baseModifiers?.femaleNewBreastCupSizeDelta.GetValue() ?? 0;
		internal CupSize FemaleNewDefaultCup => baseModifiers?.femaleNewBreastDefaultCupSize.GetValue() ?? CupSize.C;

		public CupSize FemaleMinCup => baseModifiers?.femaleMinCupSize.GetValue() ?? CupSize.FLAT;

		internal sbyte MaleNewCupDelta => baseModifiers?.maleNewBreastCupSizeDelta.GetValue() ?? 0;
		internal CupSize MaleNewDefaultCup => baseModifiers?.maleNewBreastDefaultCupSize.GetValue() ?? CupSize.FLAT;

		public CupSize MaleMinCup => baseModifiers?.maleMinCupSize.GetValue() ?? CupSize.FLAT;

		internal void ValidateCupSize()
		{
			source.breastRows.ForEach(x => x.ValidateCupSize());
		}

		internal double TitsGrowthMultiplier => baseModifiers?.titsGrowthMultiplier.GetValue() ?? 1;
		internal double TitsShrinkMultiplier => baseModifiers?.titsShrinkMultiplier.GetValue() ?? 1;


		#endregion

		#region Nipples Perk Data

		internal double NippleGrowthMultiplier => baseModifiers?.nippleGrowthMultiplier.GetValue() ?? 1;

		internal double NippleShrinkMultiplier => baseModifiers?.nippleShrinkMultiplier.GetValue() ?? 1;

		internal double NewNippleDefaultLength => baseModifiers?.newNippleDefaultLength.GetValue() ?? NippleAggregate.MALE_DEFAULT_NIPPLE;

		internal double NewNippleSizeDelta => baseModifiers?.newNippleSizeDelta.GetValue() ?? 0;

		#endregion

		#region Vaginas
		internal VaginalWetness? defaultNewVaginaWetness
		{
			get
			{
				if (baseModifiers is null || !baseModifiers.defaultVaginalWetness.hasAnyActiveMembers)
				{
					return null;
				}
				else
				{
					return baseModifiers.defaultVaginalWetness.GetValue();
				}
			}

		}
		internal VaginalLooseness? defaultNewVaginaLooseness
		{
			get
			{
				if (baseModifiers is null || !baseModifiers.defaultVaginalLooseness.hasAnyActiveMembers)
				{
					return null;
				}
				else
				{
					return baseModifiers.defaultVaginalLooseness.GetValue();
				}
			}
		}
		internal ushort perkBonusVaginalCapacity => baseModifiers?.perkBasedBonusVaginalCapacity.GetValue() ?? 0;

		internal void OnVaginalCapacityChanged()
		{
			//note: this has no effect, as of this moment. idk if we want an event to fire when capacity changes, so that may be something to handle here.
		}

		internal VaginalLooseness minVaginalLooseness => baseModifiers?.minVaginalLooseness.GetValue() ?? VaginalLooseness.TIGHT;
		internal VaginalLooseness maxVaginalLooseness => baseModifiers?.maxVaginalLooseness.GetValue() ?? VaginalLooseness.CLOWN_CAR_WIDE;
		internal void ValidateVaginalLooseness()
		{
			source.vaginas.ForEach(x => x.OnLoosenessPerkValueChange());
		}
		internal VaginalWetness minVaginalWetness => baseModifiers?.minVaginalWetness.GetValue() ?? VaginalWetness.DRY;
		internal VaginalWetness maxVaginalWetness => baseModifiers?.maxVaginalWetness.GetValue() ?? VaginalWetness.SLAVERING;
		internal void ValidateVaginalWetness()
		{
			source.vaginas.ForEach(x => x.OnWetnessPerkValueChange());
		}

		#region Clits
		internal double NewClitSizeDelta => baseModifiers?.newClitSizeDelta.GetValue() ?? 0;

		internal double ClitGrowthMultiplier => baseModifiers?.clitGrowthMultiplier.GetValue() ?? 1;
		internal double ClitShrinkMultiplier => baseModifiers?.clitShrinkMultiplier.GetValue() ?? 1;

		internal double DefaultNewClitSize => baseModifiers?.defaultNewClitSize.GetValue() ?? Clit.DEFAULT_CLIT_SIZE;
		internal double MinClitSize => baseModifiers?.minClitSize.GetValue() ?? Clit.MIN_CLIT_SIZE;

		internal void ValidateClitLength()
		{
			source.vaginas.ForEach(x => x.clit.CheckClitSize());
		}
		#endregion
		#endregion
	}
}
