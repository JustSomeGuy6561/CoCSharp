﻿using System;

namespace CoC.Backend.BodyParts
{
	//doesn't need to be serialized - it's generated live when we create a creature.
	internal sealed class GenitalPerkData
	{
		private readonly Genitals source;

		internal GenitalPerkData(Genitals parent)
		{
			source = parent ?? throw new ArgumentNullException(nameof(parent));
		}


		#region Common
		internal bool TreatAllFluidsAsIfAtMaxLust
		{
			get => _treatAllFluidsAsIfAtMaxLust;
			set => _treatAllFluidsAsIfAtMaxLust = value;
		}
		private bool _treatAllFluidsAsIfAtMaxLust;
		#endregion

		#region Cock Perks
		internal float NewCockSizeDelta
		{
			get => _NewCockSizeDelta;
			set => _NewCockSizeDelta = value;
		}
		private float _NewCockSizeDelta = 0;
		internal float CockGrowthMultiplier
		{
			get => _CockGrowthMultiplier;
			set => _CockGrowthMultiplier = value;
		}
		private float _CockGrowthMultiplier = 1;
		internal float CockShrinkMultiplier
		{
			get => _CockShrinkMultiplier;
			set => _CockShrinkMultiplier = value;
		}
		private float _CockShrinkMultiplier = 1;
		internal float NewCockDefaultSize
		{
			get => _NewCockDefaultSize;
			set => CheckChanged(ref _NewCockDefaultSize, value, () => source.cocks.ForEach(x => x.newCockDefaultSize = value));
		}
		private float _NewCockDefaultSize = Cock.DEFAULT_COCK_LENGTH;
		internal float MinCockLength
		{
			get => _MinCockLength;
			set => CheckChanged(ref _MinCockLength, value, () => source.cocks.ForEach(x => x.minCockLength = value));
		}
		private float _MinCockLength = Cock.MIN_COCK_LENGTH;

		internal float perkBonusVirilityMultiplier
		{
			get => _perkBonusVirilityMultiplier;
			set => _perkBonusVirilityMultiplier = value;
		}
		private float _perkBonusVirilityMultiplier;

		internal sbyte perkBonusVirility
		{
			get => _perkBonusVirility;
			set => _perkBonusVirility = value;
		}
		private sbyte _perkBonusVirility;

		internal CockPerkHelper GetCockPerkWrapper()
		{
			return new CockPerkHelper(NewCockDefaultSize, NewCockSizeDelta, MinCockLength);
		}
		#endregion

		#region Cum Perks
		internal bool alwaysProducesMaxCum
		{
			get => _alwaysProducesMaxCum;
			set
			{
				if (value && !_alwaysProducesMaxCum)
				{
					value = alwaysProducesMaxCum;
					handleCumChange();
				}

			}
		}
		private bool _alwaysProducesMaxCum = false;
		internal uint bonusCumAdded
		{
			get => _bonusCumAdded;
			set => CheckChanged(ref _bonusCumAdded, value, handleCumChange);
		}
		private uint _bonusCumAdded = 0;
		internal float bonusCumMultiplier
		{
			get => _bonusCumMultiplier;
			set => CheckChanged(ref _bonusCumMultiplier, value, handleCumChange);
		}
		private float _bonusCumMultiplier = 1f;

		private void handleCumChange()
		{
			//throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		#endregion

		#region Breast Perk Data
		internal sbyte FemaleNewCupDelta
		{
			get => _FemaleNewCupDelta;
			set => _FemaleNewCupDelta = value;
		}
		private sbyte _FemaleNewCupDelta = 0;
		internal CupSize FemaleNewDefaultCup
		{
			get => _FemaleNewDefaultCup;
			set => CheckChanged(ref _FemaleNewDefaultCup, value, () => source.breastRows.ForEach(x => x.femaleDefaultCup = value));
		}
		private CupSize _FemaleNewDefaultCup = Breasts.DEFAULT_FEMALE_SIZE;
		internal sbyte MaleNewCupDelta
		{
			get => _MaleNewCupDelta;
			set => _MaleNewCupDelta = value;
		}
		private sbyte _MaleNewCupDelta = 0;
		internal CupSize MaleNewDefaultCup
		{
			get => _MaleNewDefaultCup;
			set => CheckChanged(ref _MaleNewDefaultCup, value, () => source.breastRows.ForEach(x => x.maleDefaultCup = value));
		}
		private CupSize _MaleNewDefaultCup = Breasts.DEFAULT_MALE_SIZE;
		public CupSize FemaleMinCup
		{
			get => _FemaleMinCup;
			internal set => CheckChanged(ref _FemaleMinCup, value, () => source.breastRows.ForEach(x => x.femaleMinCup = value));
		}
		private CupSize _FemaleMinCup = CupSize.FLAT;
		public CupSize MaleMinCup
		{
			get => _MaleMinCup;
			internal set => CheckChanged(ref _MaleMinCup, value, () => source.breastRows.ForEach(x => x.maleMinCup = value));
		}
		private CupSize _MaleMinCup = CupSize.FLAT;
		internal float TitsGrowthMultiplier
		{
			get => _TitsGrowthMultiplier;
			set => CheckChanged(ref _TitsGrowthMultiplier, value, () => source.breastRows.ForEach(x => x.bigTiddyMultiplier = value));
		}
		private float _TitsGrowthMultiplier = 1;
		internal float TitsShrinkMultiplier
		{
			get => _TitsShrinkMultiplier;
			set => CheckChanged(ref _TitsShrinkMultiplier, value, () => source.breastRows.ForEach(x => x.tinyTiddyMultiplier = value));
		}
		private float _TitsShrinkMultiplier = 1;
		internal float NewNippleSizeDelta
		{
			get => _NewNippleSizeDelta;
			set => _NewNippleSizeDelta = value;
		}
		private float _NewNippleSizeDelta = 0;


		internal BreastPerkHelper GetBreastPerkWrapper()
		{
			return new BreastPerkHelper(FemaleNewCupDelta, FemaleNewDefaultCup, MaleNewCupDelta, MaleNewDefaultCup, FemaleMinCup, MaleMinCup, TitsGrowthMultiplier,
				TitsShrinkMultiplier, NewNippleSizeDelta, NippleGrowthMultiplier, NippleShrinkMultiplier, NewNippleDefaultLength);
		}
		#endregion

		#region Nipples Perk Data

		internal float NippleGrowthMultiplier
		{
			get => _NippleGrowthMultiplier;
			set => CheckChanged(ref _NippleGrowthMultiplier, value, () => source.breastRows.ForEach(x => x.nipples.growthMultiplier = value));
		}
		private float _NippleGrowthMultiplier = 1;
		internal float NippleShrinkMultiplier
		{
			get => _NippleShrinkMultiplier;
			set => CheckChanged(ref _NippleShrinkMultiplier, value, () => source.breastRows.ForEach(x => x.nipples.shrinkMultiplier = value));
		}
		private float _NippleShrinkMultiplier = 1;
		internal float NewNippleDefaultLength
		{
			get => _NewNippleDefaultLength;
			set => _NewNippleDefaultLength = value;
		}
		private float _NewNippleDefaultLength = Nipples.MIN_NIPPLE_LENGTH;
		#endregion

		#region Vaginas
		internal VaginalWetness defaultNewVaginaWetness
		{
			get => _defaultNewVaginaWetness;
			set => _defaultNewVaginaWetness = value;
		}
		private VaginalWetness _defaultNewVaginaWetness = VaginalWetness.NORMAL;
		internal VaginalLooseness defaultNewVaginaLooseness
		{
			get => _defaultNewVaginaLooseness;
			set => _defaultNewVaginaLooseness = value;
		}
		private VaginalLooseness _defaultNewVaginaLooseness = VaginalLooseness.TIGHT;
		internal ushort perkBonusVaginalCapacity
		{
			get => _perkBonusVaginalCapacity;
			set => CheckChanged(ref _perkBonusVaginalCapacity, value, () => source.vaginas.ForEach(x => x.perkBonusVaginalCapacity = value));
		}
		private ushort _perkBonusVaginalCapacity = 0;
		internal VaginalLooseness minVaginalLooseness
		{
			get => _minVaginalLooseness;
			set => CheckChanged(ref _minVaginalLooseness, value, () => source.vaginas.ForEach(x => x.minLooseness = value));
		}
		private VaginalLooseness _minVaginalLooseness = VaginalLooseness.TIGHT;
		internal VaginalLooseness maxVaginalLooseness
		{
			get => _maxVaginalLooseness;
			set => CheckChanged(ref _maxVaginalLooseness, value, () => source.vaginas.ForEach(x => x.maxLooseness = value));
		}
		private VaginalLooseness _maxVaginalLooseness = VaginalLooseness.CLOWN_CAR_WIDE;
		internal VaginalWetness minVaginalWetness
		{
			get => _minVaginalWetness;
			set => CheckChanged(ref _minVaginalWetness, value, () => source.vaginas.ForEach(x => x.minWetness = value));
		}
		private VaginalWetness _minVaginalWetness = VaginalWetness.DRY;
		internal VaginalWetness maxVaginalWetness
		{
			get => _maxVaginalWetness;
			set => CheckChanged(ref _maxVaginalWetness, value, () => source.vaginas.ForEach(x => x.maxWetness = value));
		}
		private VaginalWetness _maxVaginalWetness = VaginalWetness.SLAVERING;

		#region Clits
		internal float NewClitSizeDelta
		{
			get => _NewClitSizeDelta;
			set => _NewClitSizeDelta = value;
		}
		private float _NewClitSizeDelta = 0;

		internal float ClitGrowthMultiplier
		{
			get => _ClitGrowthMultiplier;
			set => CheckChanged(ref _ClitGrowthMultiplier, value, () => source.vaginas.ForEach(x => x.clit.clitGrowthMultiplier = value));
		}
		private float _ClitGrowthMultiplier = 1f;
		internal float ClitShrinkMultiplier
		{
			get => _ClitShrinkMultiplier;
			set => CheckChanged(ref _ClitShrinkMultiplier, value, () => source.vaginas.ForEach(x => x.clit.clitShrinkMultiplier = value));
		}
		private float _ClitShrinkMultiplier = 1;

		internal float MinNewClitSize
		{
			get => _MinNewClitSize;
			set => CheckChanged(ref _MinNewClitSize, value, () => source.vaginas.ForEach(x => x.clit.minNewClitSize = value));
		}
		private float _MinNewClitSize;
		internal float MinClitSize
		{
			get => _MinClitSize;
			set => CheckChanged(ref _MinClitSize, value, () => source.vaginas.ForEach(x => x.clit.minClitSize = value));
		}
		private float _MinClitSize = Clit.MIN_CLIT_SIZE;
		#endregion

		internal VaginaPerkHelper GetVaginaPerkWrapper()
		{
			return new VaginaPerkHelper(NewClitSizeDelta, ClitGrowthMultiplier, ClitShrinkMultiplier, MinNewClitSize, MinClitSize, defaultNewVaginaWetness,
				defaultNewVaginaLooseness, perkBonusVaginalCapacity, minVaginalLooseness, maxVaginalLooseness, minVaginalWetness, maxVaginalWetness);
		}
		#endregion

		#region Helpers
		private void CheckChanged(ref bool target, bool newValue, Action ifDifferentCallback)
		{
			if (target != newValue)
			{
				target = newValue;
				ifDifferentCallback();
			}
		}

		private void CheckChanged(ref float target, float newValue, Action ifDifferentCallback)
		{
			if (target != newValue)
			{
				target = newValue;
				ifDifferentCallback();
			}
		}

		private void CheckChanged(ref byte target, byte newValue, Action ifDifferentCallback)
		{
			if (target != newValue)
			{
				target = newValue;
				ifDifferentCallback();
			}
		}

		private void CheckChanged(ref ushort target, ushort newValue, Action ifDifferentCallback)
		{
			if (target != newValue)
			{
				target = newValue;
				ifDifferentCallback();
			}
		}

		private void CheckChanged(ref uint target, uint newValue, Action ifDifferentCallback)
		{
			if (target != newValue)
			{
				target = newValue;
				ifDifferentCallback();
			}
		}

		private void CheckChanged(ref CupSize target, CupSize newValue, Action ifDifferentCallback)
		{
			if (target != newValue)
			{
				target = newValue;
				ifDifferentCallback();
			}
		}

		private void CheckChanged(ref VaginalWetness target, VaginalWetness newValue, Action ifDifferentCallback)
		{
			if (target != newValue)
			{
				target = newValue;
				ifDifferentCallback();
			}
		}

		private void CheckChanged(ref VaginalLooseness target, VaginalLooseness newValue, Action ifDifferentCallback)
		{
			if (target != newValue)
			{
				target = newValue;
				ifDifferentCallback();
			}
		}
		#endregion
	}
}