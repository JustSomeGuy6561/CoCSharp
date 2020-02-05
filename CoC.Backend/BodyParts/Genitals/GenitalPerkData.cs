using System;

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
			set => _NewCockDefaultSize = value;
		}
		private float _NewCockDefaultSize = Cock.DEFAULT_COCK_LENGTH;
		internal float MinCockLength
		{
			get => _MinCockLength;
			set => CheckChanged(ref _MinCockLength, value, () => source.cocks.ForEach(x => x.ValidateLength()));
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

		#endregion

		#region Cum Perks
		internal bool alwaysProducesMaxCum
		{
			get => _alwaysProducesMaxCum;
			set
			{
				if (value && !_alwaysProducesMaxCum)
				{
					_alwaysProducesMaxCum = value;
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
			set => _FemaleNewDefaultCup = value;
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
			set => _MaleNewDefaultCup = value;
		}
		private CupSize _MaleNewDefaultCup = Breasts.DEFAULT_MALE_SIZE;
		public CupSize FemaleMinCup
		{
			get => _FemaleMinCup;
			internal set => CheckChanged(ref _FemaleMinCup, value, () => source.breastRows.ForEach(x => x.ValidateCupSize()));
		}
		private CupSize _FemaleMinCup = CupSize.FLAT;
		public CupSize MaleMinCup
		{
			get => _MaleMinCup;
			internal set => CheckChanged(ref _MaleMinCup, value, () => source.breastRows.ForEach(x => x.ValidateCupSize()));
		}
		private CupSize _MaleMinCup = CupSize.FLAT;
		internal float TitsGrowthMultiplier
		{
			get => _TitsGrowthMultiplier;
			set => _TitsGrowthMultiplier = value;
		}
		private float _TitsGrowthMultiplier = 1;
		internal float TitsShrinkMultiplier
		{
			get => _TitsShrinkMultiplier;
			set => _TitsShrinkMultiplier = value;
		}
		private float _TitsShrinkMultiplier = 1;
		internal float NewNippleSizeDelta
		{
			get => _NewNippleSizeDelta;
			set => _NewNippleSizeDelta = value;
		}
		private float _NewNippleSizeDelta = 0;

		#endregion

		#region Nipples Perk Data

		internal float NippleGrowthMultiplier
		{
			get => _NippleGrowthMultiplier;
			set => _NippleGrowthMultiplier = value;
		}
		private float _NippleGrowthMultiplier = 1;
		internal float NippleShrinkMultiplier
		{
			get => _NippleShrinkMultiplier;
			set => _NippleShrinkMultiplier = value;
		}
		private float _NippleShrinkMultiplier = 1;
		internal float NewNippleDefaultLength
		{
			get => _NewNippleDefaultLength;
			set => _NewNippleDefaultLength = value;
		}
		private float _NewNippleDefaultLength = NippleAggregate.MIN_NIPPLE_LENGTH;
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
			//note: this has no effect, as of this moment. idk if we want an event to fire when capacity changes, so that may be something to handle here.
			set => CheckChanged(ref _perkBonusVaginalCapacity, value, () => { });
		}
		private ushort _perkBonusVaginalCapacity = 0;
		internal VaginalLooseness minVaginalLooseness
		{
			get => _minVaginalLooseness;
			set => CheckChanged(ref _minVaginalLooseness, value, () => source.vaginas.ForEach(x => x.OnLoosenessPerkValueChange()));
		}
		private VaginalLooseness _minVaginalLooseness = VaginalLooseness.TIGHT;
		internal VaginalLooseness maxVaginalLooseness
		{
			get => _maxVaginalLooseness;
			set => CheckChanged(ref _maxVaginalLooseness, value, () => source.vaginas.ForEach(x => x.OnLoosenessPerkValueChange()));
		}
		private VaginalLooseness _maxVaginalLooseness = VaginalLooseness.CLOWN_CAR_WIDE;
		internal VaginalWetness minVaginalWetness
		{
			get => _minVaginalWetness;
			set => CheckChanged(ref _minVaginalWetness, value, () => source.vaginas.ForEach(x => x.OnWetnessPerkValueChange()));
		}
		private VaginalWetness _minVaginalWetness = VaginalWetness.DRY;
		internal VaginalWetness maxVaginalWetness
		{
			get => _maxVaginalWetness;
			set => CheckChanged(ref _maxVaginalWetness, value, () => source.vaginas.ForEach(x => x.OnWetnessPerkValueChange()));
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
			set => _ClitGrowthMultiplier = value;
		}
		private float _ClitGrowthMultiplier = 1f;
		internal float ClitShrinkMultiplier
		{
			get => _ClitShrinkMultiplier;
			set => _ClitShrinkMultiplier = value;
		}
		private float _ClitShrinkMultiplier = 1;

		internal float DefaultNewClitSize
		{
			get => _MinNewClitSize;
			set => _MinNewClitSize = value;
		}
		private float _MinNewClitSize;
		internal float MinClitSize
		{
			get => _MinClitSize;
			set => CheckChanged(ref _MinClitSize, value, () => source.vaginas.ForEach(x => x.clit.CheckClitSize()));
		}
		private float _MinClitSize = Clit.MIN_CLIT_SIZE;
		#endregion
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
