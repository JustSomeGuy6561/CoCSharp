//PassiveBaseStatModifiers.cs
//Description:
//Author: JustSomeGuy
//6/30/2019, 7:45 PM
using System;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;

namespace CoC.Backend.Perks
{
	//these are required in the backend because they deal with stats and update functions defined in the backend. the frontend has its own class with extra perk data
	//that can be used in addition to these.

	//Perk modifier values are not designed to be saved - instead, on load, we recalculate the values by activating all the existing perks the creature has. this will let us
	//auto-correct any data that may not be correct with the perks given via save editing or version change or whatever.

	//similarly, modifier values are designed to be perk-aware - it shouldn't be up to each perk to manually store the amount they adjusted each perk modifier value, as that
	//gets ridiculous. similarly, how can we expect perks to take care of proper stacking mechanics without checking literally every other perk out there, which is also
	//tedious (not to mention, impossible). Each modifer value stores a reference to the perk that caused it to adjust, and the amount it was adjusted. if the perk is
	//removed from the collection, the modifier value updates accordingly.

	//Finally, modifiers have an optional OnChange parameter, which is a function that is called when a perk is added, updated, or removed which causes GetValue() to change.
	//it provides the old GetValue() as well, in case it is needed. Generally speaking, the only time we care about these is when it is a min or max value, which could cause
	//the respective data to become invalid. (i.e. if the max hp is 500, but drops to 450, the player cannot have 500/450 hp, so we need to validate that.)
	//This provides a means to do so. Note that there are other conditions that may cause you to want to check something when a value changes, feel free to do so.
	//Generally, things that affect values moving forward, like gain or loss rates, or default/min values for NEW items, don't need to notify anyone that they changed,
	//because it's only important when needed, not all the time.

	public sealed class BasePerkModifiers
	{
		private readonly Creature source;
		private CombatCreature combatSource => source as CombatCreature;
		private PlayerBase playerSource => source as PlayerBase;

		private PerkCollection collectionSource => source.perks;

		#region Creature Base Stats
		public readonly SignedBytePerkModifier minStrengthDelta;
		public readonly SignedBytePerkModifier maxStrengthDelta;
		public readonly DoublePerkModifier strengthGainMultiplier;
		public readonly DoublePerkModifier strengthLossMultiplier;

		public readonly SignedBytePerkModifier minSpeedDelta;
		public readonly SignedBytePerkModifier maxSpeedDelta;
		public readonly DoublePerkModifier speedGainMultiplier;
		public readonly DoublePerkModifier speedLossMultiplier;

		public readonly SignedBytePerkModifier minToughnessDelta;
		public readonly SignedBytePerkModifier maxToughnessDelta;
		public readonly DoublePerkModifier toughnessGainMultiplier;
		public readonly DoublePerkModifier toughnessLossMultiplier;

		public readonly SignedBytePerkModifier minIntelligenceDelta;
		public readonly SignedBytePerkModifier maxIntelligenceDelta;
		public readonly DoublePerkModifier intelligenceGainMultiplier;
		public readonly DoublePerkModifier intelligenceLossMultiplier;

		public readonly SignedBytePerkModifier minLustDelta;
		public readonly SignedBytePerkModifier maxLustDelta;
		public readonly DoublePerkModifier lustGainMultiplier;
		public readonly DoublePerkModifier lustLossMultiplier;

		public readonly SignedBytePerkModifier minLibidoDelta;
		public readonly SignedBytePerkModifier maxLibidoDelta;
		public readonly DoublePerkModifier libidoGainMultiplier;
		public readonly DoublePerkModifier libidoLossMultiplier;

		public readonly SignedBytePerkModifier minCorruptionDelta;
		public readonly SignedBytePerkModifier maxCorruptionDelta;
		public readonly DoublePerkModifier corruptionGainMultiplier;
		public readonly DoublePerkModifier corruptionLossMultiplier;

		public readonly SignedBytePerkModifier minSensitivityDelta;
		public readonly SignedBytePerkModifier maxSensitivityDelta;
		public readonly DoublePerkModifier sensitivityGainMultiplier;
		public readonly DoublePerkModifier sensitivityLossMultiplier;
		#endregion
		#region Creature Pregnancy-Related Stats
		public readonly BytePerkModifier bonusFertility;
		public readonly SignedBytePerkModifier pregnancySpeedModifier;

		#endregion
		#region Combat Creature stats
		public readonly SignedBytePerkModifier maxFatigueDelta;
		public readonly DoublePerkModifier fatigueGainMultiplier;
		public readonly DoublePerkModifier fatigueRecoveryMultiplier;

		public readonly DoublePerkModifier gemsGainRate;

		public readonly DoublePerkModifier healthGainMultiplier;

		public readonly DoublePerkModifier combatDamageModifier;

		public readonly DoublePerkModifier magicalSpellCost;
		public readonly DoublePerkModifier physicalSpellCost;
		public readonly DoublePerkModifier armorEffectivenessMultiplier;

		public readonly UnsignedShortPerkModifier perkBonusMaxHp;
		#endregion
		#region PlayerBase stats
		public readonly SignedBytePerkModifier maxHungerDelta;
		public readonly DoublePerkModifier hungerGainMultiplier;
		public readonly DoublePerkModifier hungerRecoveryMultiplier;
		#endregion




		//default wetness
		public readonly VaginalWetnessPerkModifier defaultVaginalWetness;
		public readonly VaginalWetnessPerkModifier minVaginalWetness;
		public readonly VaginalWetnessPerkModifier maxVaginalWetness;

		public readonly AnalWetnessPerkModifier defaultAnalWetness;
		public readonly AnalWetnessPerkModifier minAnalWetness;
		public readonly AnalWetnessPerkModifier maxAnalWetness;

		public readonly VaginalLoosenessPerkModifier defaultVaginalLooseness;
		public readonly VaginalLoosenessPerkModifier minVaginalLooseness;
		public readonly VaginalLoosenessPerkModifier maxVaginalLooseness;

		public readonly AnalLoosenessPerkModifier defaultAnalLooseness;
		public readonly AnalLoosenessPerkModifier minAnalLooseness;
		public readonly AnalLoosenessPerkModifier maxAnalLooseness;


		public readonly ConditionalPerkModifier treatCalculationsAsIfAtMaxLust;
		public readonly ConditionalPerkModifier alwaysLactatingAtMaximumCapacity;

		#region Cock

		public readonly DoublePerkModifier newCockSizeDelta;
		public readonly DoublePerkModifier cockGrowthMultiplier;
		public readonly DoublePerkModifier cockShrinkMultiplier;
		public readonly DoublePerkModifier newCockDefaultSize;
		public readonly DoublePerkModifier minCockSize;
		public readonly DoublePerkModifier perkBonusVirilityMultiplier;
		public readonly SignedBytePerkModifier perkBonusVirility;

		private void MinCockChanged(double oldValue)
		{
			source.genitals.perkData.CockMinChange(oldValue);
		}
		#endregion
		#region Cum
		public readonly ConditionalPerkModifier alwaysProducingMaxCum;
		public readonly UnsignedIntegerPerkModifier bonusAdditionalCum;
		public readonly DoublePerkModifier bonusCumMultiplier;
		#endregion
		#region Vagina
		public readonly UnsignedShortPerkModifier perkBasedBonusVaginalCapacity;
		#endregion
		#region Clit
		public readonly DoublePerkModifier defaultNewClitSize;
		public readonly DoublePerkModifier newClitSizeDelta;

		public readonly DoublePerkModifier clitGrowthMultiplier;
		public readonly DoublePerkModifier clitShrinkMultiplier;

		public readonly DoublePerkModifier minClitSize;
		#endregion
		#region Breasts

		public readonly SignedBytePerkModifier femaleNewBreastCupSizeDelta;

		public readonly CupSizePerkModifier femaleNewBreastDefaultCupSize;

		public readonly SignedBytePerkModifier maleNewBreastCupSizeDelta;

		public readonly CupSizePerkModifier maleNewBreastDefaultCupSize;
		public readonly CupSizePerkModifier femaleMinCupSize;
		public readonly CupSizePerkModifier maleMinCupSize;
		public readonly DoublePerkModifier titsGrowthMultiplier;

		public readonly DoublePerkModifier titsShrinkMultiplier;
		#endregion
		#region Nipples
		public readonly DoublePerkModifier newNippleDefaultLength;
		public readonly DoublePerkModifier newNippleSizeDelta;

		public readonly DoublePerkModifier nippleGrowthMultiplier;
		public readonly DoublePerkModifier nippleShrinkMultiplier;
		#endregion
		#region Balls
		public readonly BytePerkModifier newBallsDefaultSize;
		public readonly SignedBytePerkModifier newBallsSizeDelta;

		public readonly DoublePerkModifier ballsGrowthMultiplier;
		public readonly DoublePerkModifier ballsShrinkMultiplier;
		#endregion
		#region Ass
		public readonly UnsignedShortPerkModifier perkBasedBonusAnalCapacity;
		#endregion

		public readonly BytePerkModifier minButtSize;
		public readonly BytePerkModifier minHipsSize;
#warning make the cap values not read-only, as these caps could change by difficulty. need a means to adjust them with difficulty.

		public BasePerkModifiers(Creature sourceCreature)
		{
			source = sourceCreature ?? throw new ArgumentNullException(nameof(sourceCreature));

			//stats
			minStrengthDelta = new SignedBytePerkModifier(0, 0, 50, CheckStrengthChanged);
			maxStrengthDelta = new SignedBytePerkModifier(0, -50, 50, CheckStrengthChanged);
			strengthGainMultiplier = new DoublePerkModifier(1, 0.25, 4);
			strengthLossMultiplier = new DoublePerkModifier(1, 0.25, 4);

			minSpeedDelta = new SignedBytePerkModifier(0, 0, 50, CheckSpeedChanged);
			maxSpeedDelta = new SignedBytePerkModifier(0, -50, 50, CheckSpeedChanged);
			speedGainMultiplier = new DoublePerkModifier(1, 0.25, 4);
			speedLossMultiplier = new DoublePerkModifier(1, 0.25, 4);

			minToughnessDelta = new SignedBytePerkModifier(0, 0, 50, CheckToughnessChanged);
			maxToughnessDelta = new SignedBytePerkModifier(0, -50, 50, CheckToughnessChanged);
			toughnessGainMultiplier = new DoublePerkModifier(1, 0.25, 4);
			toughnessLossMultiplier = new DoublePerkModifier(1, 0.25, 4);

			minIntelligenceDelta = new SignedBytePerkModifier(0, 0, 50, CheckIntelligenceChanged);
			maxIntelligenceDelta = new SignedBytePerkModifier(0, -50, 50, CheckIntelligenceChanged);
			intelligenceGainMultiplier = new DoublePerkModifier(1, 0.25, 4);
			intelligenceLossMultiplier = new DoublePerkModifier(1, 0.25, 4);

			minLustDelta = new SignedBytePerkModifier(0, 0, 50, CheckLustChanged);
			maxLustDelta = new SignedBytePerkModifier(0, -50, 50, CheckLustChanged);
			lustGainMultiplier = new DoublePerkModifier(1, 0.25, 4);
			lustLossMultiplier = new DoublePerkModifier(1, 0.25, 4);

			minLibidoDelta = new SignedBytePerkModifier(0, 0, 50, CheckLibidoChanged);
			maxLibidoDelta = new SignedBytePerkModifier(0, -50, 50, CheckLibidoChanged);
			libidoGainMultiplier = new DoublePerkModifier(1, 0.25, 4);
			libidoLossMultiplier = new DoublePerkModifier(1, 0.25, 4);

			minCorruptionDelta = new SignedBytePerkModifier(0, 0, 50, CheckCorruptionChanged);
			maxCorruptionDelta = new SignedBytePerkModifier(0, -50, 50, CheckCorruptionChanged);
			corruptionGainMultiplier = new DoublePerkModifier(1, 0.25, 4);
			corruptionLossMultiplier = new DoublePerkModifier(1, 0.25, 4);

			minSensitivityDelta = new SignedBytePerkModifier(0, 0, 50, CheckSensitivityChanged);
			maxSensitivityDelta = new SignedBytePerkModifier(0, -50, 50, CheckSensitivityChanged);
			sensitivityGainMultiplier = new DoublePerkModifier(1, 0.25, 4);
			sensitivityLossMultiplier = new DoublePerkModifier(1, 0.25, 4);

			maxFatigueDelta = new SignedBytePerkModifier(0, -50, 50, CheckFatigueChanged);
			fatigueGainMultiplier = new DoublePerkModifier(1, 0.25, 4);
			fatigueRecoveryMultiplier = new DoublePerkModifier(1, 0.25, 4);

			maxHungerDelta = new SignedBytePerkModifier(0, -50, 50, CheckHungerChanged);
			hungerGainMultiplier = new DoublePerkModifier(1, 0.25, 4);
			hungerRecoveryMultiplier = new DoublePerkModifier(1, 0.25, 4);

			gemsGainRate = new DoublePerkModifier(1, 0.66667, 1.5);


			healthGainMultiplier = new DoublePerkModifier(1, 0.5, 3);

			//NYI
			combatDamageModifier = new DoublePerkModifier(1, 0.1, 5);
			//NYI
			magicalSpellCost = new DoublePerkModifier(1, .25, 2);
			physicalSpellCost = new DoublePerkModifier(1, .25, 2);
			//NYI
			armorEffectivenessMultiplier = new DoublePerkModifier(0.5, 3);
			//end NYIs.
			//Implemented.
			perkBonusMaxHp = new UnsignedShortPerkModifier(0, 0, 5000, CheckHpChanged);

			//default wetness
			defaultVaginalWetness = new VaginalWetnessPerkModifier(VaginalWetness.NORMAL, VaginalWetness.DRY, VaginalWetness.SLAVERING);
			minVaginalWetness = new VaginalWetnessPerkModifier(VaginalWetness.DRY, VaginalWetness.DRY, VaginalWetness.DROOLING, CheckVaginalWetnessChanged);
			maxVaginalWetness = new VaginalWetnessPerkModifier(VaginalWetness.SLAVERING, VaginalWetness.WET, VaginalWetness.SLAVERING, CheckVaginalWetnessChanged);

			defaultVaginalLooseness = new VaginalLoosenessPerkModifier(VaginalLooseness.TIGHT, VaginalLooseness.TIGHT, VaginalLooseness.CLOWN_CAR_WIDE);
			minVaginalLooseness = new VaginalLoosenessPerkModifier(VaginalLooseness.TIGHT, VaginalLooseness.TIGHT, VaginalLooseness.GAPING, CheckVaginalLoosenessChanged);
			maxVaginalLooseness = new VaginalLoosenessPerkModifier(VaginalLooseness.CLOWN_CAR_WIDE, VaginalLooseness.LOOSE, VaginalLooseness.CLOWN_CAR_WIDE, CheckVaginalLoosenessChanged);

			defaultAnalWetness = new AnalWetnessPerkModifier(AnalWetness.NORMAL, AnalWetness.NORMAL, AnalWetness.SLIME_DROOLING);
			minAnalWetness = new AnalWetnessPerkModifier(AnalWetness.NORMAL, AnalWetness.NORMAL, AnalWetness.DROOLING, CheckAnalWetnessChanged);
			maxAnalWetness = new AnalWetnessPerkModifier(AnalWetness.SLIME_DROOLING, AnalWetness.MOIST, AnalWetness.SLIME_DROOLING, CheckAnalWetnessChanged);

			defaultAnalLooseness = new AnalLoosenessPerkModifier(AnalLooseness.NORMAL, AnalLooseness.NORMAL, AnalLooseness.GAPING);
			minAnalLooseness = new AnalLoosenessPerkModifier(AnalLooseness.NORMAL, AnalLooseness.NORMAL, AnalLooseness.STRETCHED, CheckAnalLoosenessChanged);
			maxAnalLooseness = new AnalLoosenessPerkModifier(AnalLooseness.GAPING, AnalLooseness.LOOSE, AnalLooseness.GAPING, CheckAnalLoosenessChanged);


			treatCalculationsAsIfAtMaxLust = new ConditionalPerkModifier(false);
			alwaysLactatingAtMaximumCapacity = new ConditionalPerkModifier(false);

			alwaysProducingMaxCum = new ConditionalPerkModifier(false, CheckCumMax);
			bonusAdditionalCum = new UnsignedIntegerPerkModifier(0, 0, 5000, CheckCumAdd);
			bonusCumMultiplier = new DoublePerkModifier(1, 0.25, 100, CheckCumMult);

			newCockSizeDelta = new DoublePerkModifier(0);
			cockGrowthMultiplier = new DoublePerkModifier(1, 0.5, 3);
			cockShrinkMultiplier = new DoublePerkModifier(1, 0.5, 3);
			newCockDefaultSize = new DoublePerkModifier(Cock.DEFAULT_COCK_LENGTH, Cock.DEFAULT_COCK_LENGTH);
			minCockSize = new DoublePerkModifier(Cock.MIN_COCK_LENGTH, Cock.MIN_COCK_LENGTH, onChange: MinCockChanged);

			perkBonusVirilityMultiplier = new DoublePerkModifier(1, 0.2, 1.5);
			perkBonusVirility = new SignedBytePerkModifier(0, -50, 50);

			perkBasedBonusVaginalCapacity = new UnsignedShortPerkModifier(0, 0, ushort.MaxValue, VaginalCapacityChanged);

			defaultNewClitSize = new DoublePerkModifier(Clit.DEFAULT_CLIT_SIZE, Clit.DEFAULT_CLIT_SIZE, Clit.MAX_CLIT_SIZE);
			newClitSizeDelta = new DoublePerkModifier(0, -5, 5);

			clitGrowthMultiplier = new DoublePerkModifier(1, 0.5, 3);
			clitShrinkMultiplier = new DoublePerkModifier(1, 0.5, 3);

			minClitSize = new DoublePerkModifier(Clit.DEFAULT_CLIT_SIZE, Clit.DEFAULT_CLIT_SIZE, Clit.MAX_CLIT_SIZE, MinClitChanged);

			femaleNewBreastCupSizeDelta = new SignedBytePerkModifier(0, -5, 5);

			femaleNewBreastDefaultCupSize = new CupSizePerkModifier(CupSize.C, CupSize.FLAT, CupSize.JACQUES00);

			maleNewBreastCupSizeDelta = new SignedBytePerkModifier(0, -5, 5);

			maleNewBreastDefaultCupSize = new CupSizePerkModifier(CupSize.FLAT, CupSize.FLAT, CupSize.JACQUES00);
			femaleMinCupSize = new CupSizePerkModifier(CupSize.FLAT, CupSize.FLAT, CupSize.JACQUES00, MinFemaleCupChanged);
			maleMinCupSize = new CupSizePerkModifier(CupSize.FLAT, CupSize.FLAT, CupSize.JACQUES00, MinMaleCupChanged);

			titsGrowthMultiplier = new DoublePerkModifier(1, 0.5, 3);
			titsShrinkMultiplier = new DoublePerkModifier(1, 0.5, 3);

			newNippleDefaultLength = new DoublePerkModifier(NippleAggregate.MALE_DEFAULT_NIPPLE, NippleAggregate.MALE_DEFAULT_NIPPLE, NippleAggregate.MAX_NIPPLE_LENGTH);
			newNippleSizeDelta = new DoublePerkModifier(0, -2, 2);

			nippleGrowthMultiplier = new DoublePerkModifier(1, 0.5, 3);
			nippleShrinkMultiplier = new DoublePerkModifier(1, 0.5, 3);

			newBallsDefaultSize = new BytePerkModifier(Balls.DEFAULT_BALLS_SIZE, Balls.MIN_BALLS_SIZE, Balls.MAX_BALLS_SIZE);
			newBallsSizeDelta = new SignedBytePerkModifier(0, -4, 4);

			ballsGrowthMultiplier = new DoublePerkModifier(1, 0.5, 2);
			ballsShrinkMultiplier = new DoublePerkModifier(1, 0.5, 2);

			perkBasedBonusAnalCapacity = new UnsignedShortPerkModifier(0, 0, ushort.MaxValue, AnalCapacityChanged);

			//fertility is annoying b/c it fires off an event when this changes. normally i'd just make the property there a read, but it needs to know the previous data
			//and that's not always easy to get unless stored there and just copied when this updates.
			bonusFertility = new BytePerkModifier(0, 0, 50, FertilityChanged);

			pregnancySpeedModifier = new SignedBytePerkModifier(0, -5, 10);

			minButtSize = new BytePerkModifier(Butt.TIGHT, Butt.TIGHT, Butt.INCONCEIVABLY_BIG, MinButtChanged);
			minHipsSize = new BytePerkModifier(0, 0, Hips.INHUMANLY_WIDE, MinHipsChanged);
		}


		private void CheckStrengthChanged(sbyte oldValue)
		{
			source.ValidateStrength();
		}


		private void CheckSpeedChanged(sbyte oldValue)
		{
			source.ValidateSpeed();
		}


		private void CheckToughnessChanged(sbyte oldValue)
		{
			source.ValidateToughness();
		}


		private void CheckIntelligenceChanged(sbyte oldValue)
		{
			source.ValidateIntelligence();
		}


		private void CheckLustChanged(sbyte oldValue)
		{
			source.ValidateLust();
		}


		private void CheckLibidoChanged(sbyte oldValue)
		{
			source.ValidateLibido();
		}


		private void CheckCorruptionChanged(sbyte oldValue)
		{
			source.ValidateCorruption();
		}


		private void CheckSensitivityChanged(sbyte oldValue)
		{
			source.ValidateSensitivity();
		}

		private void CheckFatigueChanged(sbyte oldValue)
		{
			combatSource?.ValidateFatigue();
		}

		private void CheckHungerChanged(sbyte oldValue)
		{
			playerSource?.ValidateHunger();
		}

		private void CheckHpChanged(ushort oldValue)
		{
			playerSource?.ValidateHP();
		}


		private void CheckVaginalWetnessChanged(VaginalWetness oldValue)
		{
			source.genitals.perkData.ValidateVaginalWetness();
		}


		private void CheckVaginalLoosenessChanged(VaginalLooseness oldValue)
		{
			source.genitals.perkData.ValidateVaginalLooseness();
		}


		private void CheckAnalWetnessChanged(AnalWetness oldValue)
		{
			source.genitals.ass.CheckWetness();
		}


		private void CheckAnalLoosenessChanged(AnalLooseness oldValue)
		{
			source.genitals.ass.CheckLooseness();
		}

		private void CheckCumMax(bool oldValue)
		{
			source.genitals.perkData.HandleCumChange();
		}

		private void CheckCumAdd(uint oldValue)
		{
			source.genitals.perkData.HandleCumChange();
		}

		private void CheckCumMult(double oldValue)
		{
			source.genitals.perkData.HandleCumChange();
		}

		private void VaginalCapacityChanged(ushort oldValue)
		{
			source.genitals.perkData.OnVaginalCapacityChanged();
		}

		private void MinClitChanged(double oldValue)
		{
			source.genitals.perkData.ValidateClitLength();
		}

		private void MinFemaleCupChanged(CupSize oldValue)
		{
			source.genitals.perkData.ValidateCupSize();
		}

		private void MinMaleCupChanged(CupSize oldValue)
		{
			source.genitals.perkData.ValidateCupSize();
		}

		private void AnalCapacityChanged(ushort oldValue)
		{
			source.ass.OnPerkAnalCapacityChanged(oldValue);
		}

		private void FertilityChanged(byte oldValue)
		{
			source.fertility.OnPerkFertilityChange(oldValue);
		}

		private void MinButtChanged(byte oldValue)
		{
			source.butt.CheckButt();
		}

		private void MinHipsChanged(byte oldValue)
		{
			source.hips.CheckHips();
		}

		//public double gemGainMultiplier = 1.0f;

		//public double healingMultiplier = 1.0f;
		//#region Combat Perks
		//Not Implemented
		//public double combatDamageModifier
		//{
		//	get => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//	set => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//}
		//public double magicalSpellCost
		//{
		//	get => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//	set => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//}
		//public double physicalSpellCost
		//{
		//	get => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//	set => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//}
		//public double armorEffectivenessMultiplier
		//{
		//	get => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//	set => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//}
		//public double combatDamageModifier { get; set; } = 1;
		//public double magicalSpellCost { get; set; } = 1;
		//public double physicalSpellCost { get; set; } = 1;
		//public double armorEffectivenessMultiplier { get; set; } = 1;
		//#endregion

		//#region Pregnancy/Fertility

		//public byte bonusFertility
		//{
		//	get => source.genitals.fertility.perkBonusFertility;
		//	set => source.genitals.fertility.perkBonusFertility = value;
		//}

		//#region General

		//internal bool CalculationsTreatAsIfMaxLust
		//{
		//	get => _CalculationsTreatAsIfMaxLust;
		//	set
		//	{
		//		if (_CalculationsTreatAsIfMaxLust != value)
		//		{
		//			_CalculationsTreatAsIfMaxLust = value;
		//		}
		//		source.genitals.perkData.TreatAllFluidsAsIfAtMaxLust = _CalculationsTreatAsIfMaxLust;
		//	}
		//}
		//private bool _CalculationsTreatAsIfMaxLust = false;
		//#endregion

		//#region Cock
		//added to any non-default cock size.
		//public double NewCockSizeDelta
		//{
		//	get => source.genitals.perkData.NewCockSizeDelta;
		//	set => source.genitals.perkData.NewCockSizeDelta = value;
		//}

		////affects the rate at which the cock grows. this base growth amount is multiplied by this value.
		//public double CockGrowthMultiplier
		//{
		//	get => source.genitals.perkData.CockGrowthMultiplier;
		//	set => source.genitals.perkData.CockGrowthMultiplier = value;
		//}
		////affects the rate at which the cock shrinks. this base shrink amount is multiplied by this value.
		//public double CockShrinkMultiplier
		//{
		//	get => source.genitals.perkData.CockShrinkMultiplier;
		//	set => source.genitals.perkData.CockShrinkMultiplier = value;
		//}

		////if no size is provided, this value is used for a new cock.
		//public double NewCockDefaultSize
		//{
		//	get => source.genitals.perkData.NewCockDefaultSize;
		//	set => source.genitals.perkData.NewCockDefaultSize = value;
		//}

		////minimum size for a cock. behavior for what happens when attempting to shrink below this depends on the source.
		//public double MinCockSize
		//{
		//	get => source.genitals.perkData.MinCockLength;
		//	set => source.genitals.perkData.MinCockLength = value;
		//}

		//public double perkBonusVirilityMultiplier
		//{
		//	get => source.genitals.perkData.perkBonusVirilityMultiplier;
		//	set => source.genitals.perkData.perkBonusVirilityMultiplier = value;
		//}

		//public sbyte perkBonusVirility
		//{
		//	get => source.genitals.perkData.perkBonusVirility;
		//	set => source.genitals.perkData.perkBonusVirility = value;
		//}
		//#endregion
		//#region Vagina
		//#region Clit
		////offset for new clits with non-default value.
		//public double NewClitSizeDelta
		//{
		//	get => source.genitals.perkData.NewClitSizeDelta;
		//	set => source.genitals.perkData.NewClitSizeDelta = value;
		//}

		////affects rate of growth of clit.
		//public double ClitGrowthMultiplier
		//{
		//	get => source.genitals.perkData.ClitGrowthMultiplier;
		//	set => source.genitals.perkData.ClitGrowthMultiplier = value;
		//}

		////affects clit shrink rate.
		//public double ClitShrinkMultiplier
		//{
		//	get => source.genitals.perkData.ClitShrinkMultiplier;
		//	set => source.genitals.perkData.ClitShrinkMultiplier = value;
		//}

		////affects new clit size.
		//public double DefaultNewClitSize
		//{
		//	get => source.genitals.perkData.DefaultNewClitSize;
		//	set => source.genitals.perkData.DefaultNewClitSize = value;
		//}

		//public double MinClitSize
		//{
		//	get => source.genitals.perkData.MinClitSize;
		//	set => source.genitals.perkData.MinClitSize = value;
		//}
		//#endregion



		//public VaginalWetness? NewVaginaDefaultWetness
		//{
		//	get => source.genitals.perkData.defaultNewVaginaWetness;
		//	set => source.genitals.perkData.defaultNewVaginaWetness = value;
		//}
		//public VaginalLooseness? NewVaginaDefaultLooseness
		//{
		//	get => source.genitals.perkData.defaultNewVaginaLooseness;
		//	set => source.genitals.perkData.defaultNewVaginaLooseness = value;
		//}

		//public VaginalLooseness minVaginalLooseness
		//{
		//	get => source.genitals.perkData.minVaginalLooseness;
		//	set => source.genitals.perkData.minVaginalLooseness = value;
		//}

		//public VaginalLooseness maxVaginalLooseness
		//{
		//	get => source.genitals.perkData.maxVaginalLooseness;
		//	set => source.genitals.perkData.maxVaginalLooseness = value;
		//}

		//public VaginalWetness minVaginalWetness
		//{
		//	get => source.genitals.perkData.minVaginalWetness;
		//	set => source.genitals.perkData.minVaginalWetness = value;
		//}

		//public VaginalWetness maxVaginalWetness
		//{
		//	get => source.genitals.perkData.maxVaginalWetness;
		//	set => source.genitals.perkData.maxVaginalWetness = value;
		//}

		//public ushort PerkBasedBonusVaginalCapacity
		//{
		//	get => source.genitals.perkData.perkBonusVaginalCapacity;
		//	set => source.genitals.perkData.perkBonusVaginalCapacity = value;
		//}
		//#endregion
		//#region Breasts
		////These are split by gender. Note that herms use female, and genderless use male.

		//public sbyte FemaleNewBreastCupSizeDelta
		//{
		//	get => source.genitals.perkData.FemaleNewCupDelta;
		//	set => source.genitals.perkData.FemaleNewCupDelta = value;
		//}

		////how much do we add or remove to base amount for new Breast Rows?// BigTits Perks
		//public CupSize FemaleNewBreastDefaultCupSize
		//{
		//	get => source.genitals.perkData.FemaleNewDefaultCup;
		//	set => source.genitals.perkData.FemaleNewDefaultCup = value;
		//}

		//public sbyte MaleNewBreastCupSizeDelta
		//{
		//	get => source.genitals.perkData.MaleNewCupDelta;
		//	set => source.genitals.perkData.MaleNewCupDelta = value;
		//}

		//public CupSize MaleNewBreastDefaultCupSize
		//{
		//	get => source.genitals.perkData.MaleNewDefaultCup;
		//	set => source.genitals.perkData.MaleNewDefaultCup = value;
		//}


		//public CupSize FemaleMinCupSize
		//{
		//	get => source.genitals.perkData.FemaleMinCup;
		//	set => source.genitals.perkData.FemaleMinCup = value;
		//}
		//public CupSize MaleMinCupSize
		//{
		//	get => source.genitals.perkData.MaleMinCup;
		//	set => source.genitals.perkData.MaleMinCup = value;
		//}


		//public double TitsGrowthMultiplier
		//{
		//	get => source.genitals.perkData.TitsGrowthMultiplier;
		//	set => source.genitals.perkData.TitsGrowthMultiplier = value;
		//}

		//public double TitsShrinkMultiplier
		//{
		//	get => source.genitals.perkData.TitsShrinkMultiplier;
		//	set => source.genitals.perkData.TitsShrinkMultiplier = value;
		//}
		//#region Nipples

		//public double NewNippleSizeDelta
		//{
		//	get => source.genitals.perkData.NewNippleSizeDelta;
		//	set => source.genitals.perkData.NewNippleSizeDelta = value;
		//}

		//public double NippleGrowthMultiplier
		//{
		//	get => source.genitals.perkData.NippleGrowthMultiplier;
		//	set => source.genitals.perkData.NippleGrowthMultiplier = value;
		//}

		//public double NippleShrinkMultiplier
		//{
		//	get => source.genitals.perkData.NippleShrinkMultiplier;
		//	set => source.genitals.perkData.NippleShrinkMultiplier = value;
		//}

		//public double NewNippleDefaultLength
		//{
		//	get => source.genitals.perkData.NewNippleDefaultLength;
		//	set => source.genitals.perkData.NewNippleDefaultLength = value;
		//}
		//#endregion
		//#endregion
		//#region Balls
		//public sbyte NewBallsSizeDelta
		//{
		//	get => source.genitals.balls.newSizeOffset;
		//	set => source.genitals.balls.newSizeOffset = value;
		//}

		//public double BallsGrowthMultiplier
		//{
		//	get => source.genitals.balls.growthMultiplier;
		//	set => source.genitals.balls.growthMultiplier = value;
		//}

		//public double BallsShrinkMultiplier
		//{
		//	get => source.genitals.balls.shrinkMultiplier;
		//	set => source.genitals.balls.shrinkMultiplier = value;
		//}

		//public byte NewBallsDefaultSize
		//{
		//	get => source.genitals.balls.defaultNewSize;
		//	set => source.genitals.balls.defaultNewSize = value;
		//}
		//#endregion
		//#region Ass
		////public AnalLooseness minAnalLooseness
		////{
		////	get => source.ass.minLooseness;
		////	set => source.ass.minLooseness = value;
		////}

		////public AnalLooseness maxAnalLooseness
		////{
		////	get => source.ass.maxLooseness;
		////	set => source.ass.maxLooseness = value;
		////}

		////public AnalWetness minAnalWetness
		////{
		////	get => source.ass.minWetness;
		////	set => source.ass.minWetness = value;
		////}

		////public AnalWetness maxAnalWetness
		////{
		////	get => source.ass.maxWetness;
		////	set => source.ass.maxWetness = value;
		////}
		//public ushort PerkBasedBonusAnalCapacity
		//{
		//	get => source.ass.perkBonusAnalCapacity;
		//	set => source.ass.perkBonusAnalCapacity = value;
		//}
		//#endregion
		//#region Cum
		////pilgrim's bounty.
		//public bool AlwaysProducesMaxCum
		//{
		//	get => source.genitals.perkData.alwaysProducesMaxCum;
		//	set => source.genitals.perkData.alwaysProducesMaxCum = value;
		//}

		////multiplies the base cum and all non-perk bonus cum by the stack amount.
		//public double BonusCumStacked
		//{
		//	get => source.genitals.perkData.bonusCumMultiplier;
		//	set => source.genitals.perkData.bonusCumMultiplier = value;
		//}

		////After the stacking is done, this value is added in.
		//public uint BonusCumAdded
		//{
		//	get => source.genitals.perkData.bonusCumAdded;
		//	set => source.genitals.perkData.bonusCumAdded = value;
		//}
		//#endregion
		//i can't think of anything else for this.
		//a bunch of old perks can just be attributes of classes, or fire a one-off "Reaction" instead of existing (looking at you Post-Akbal submit/whatever "perks")
		//it may be possible to get a perk that prioritizes certain fur colors or skin tones, and that realistically could/should be handled here.
		//but that's not implemented, and i don't have any idea how they'd want it to work.
	}
}
/*

	//The game uses the following variables to
		//unless otherwise noted, these are added to the base stats. If you fuck up and the min > max, behavior is undefined.
		//note that regardless of bonus values here, some stats may be capped at an absolute bonus level. Note that all of these could change over development (particularly maxes, notably lust)
		//new way of dealing with initial endowments - they are permanent. so if you pick smart, you get +5 int and your min intelligence is now 5.
		//For the following endowments - the behavior is different based on how the endowment is generated -
		//if no size is provided, the size will be the default new value.
		//if a size is provided, the delta value will be added to it. if this value is still lower than the default new value, the default new value will be used instead.

		public double NewCockSizeDelta; //how much do we add or remove for new cocks? //big cock perk for now. would allow a small cock perk as well
		public double CockGrowthMultiplier = 1f; //how much more/less should we grow a cock over the base amount? //big cock perk, cockSock;
		public double CockShrinkMultiplier = 1f; //how much more/less should we shrink a cock over base amount? //big cock, cockSock;
		public double NewCockDefaultSize; //minimum size for any new cocks; //bro/futa perks for now
		public double NewClitSizeDelta; //how much do we add or remove to base amount for new Clits? //NYI, but BigClit Perks
		public double ClitGrowthMultiplier = 1f; //how much more/less should we grow a Clit over the base amount?
		public double ClitShrinkMultiplier = 1f; //how much more/less should we shrink a Clit over base amount?
		public double MinNewClitSize; //minimum size for any new Clits; //bro/futa perks for now
		public double MinClitSize; //minimum size for any Clit; //bro/futa perks for now
		//These are split by gender. Note that herms use female, and genderless use male.
		public sbyte FemaleNewBreastCupSizeDelta; //how much do we add or remove to base amount for new Breast Rows?// BigTits Perks
		public CupSize FemaleNewBreastDefaultCupSize; //minimum size for any new row of breasts; //bro/futa perks for now
		public sbyte MaleNewBreastCupSizeDelta; //how much do we add or remove to base amount for new Breast Rows?// BigTits Perks
		public CupSize MaleNewBreastDefaultCupSize; //minimum size for any new row of breasts; //bro/futa perks for now
		//these are used regardless of gender.
		public double TitsGrowthMultiplier = 1f; //how much more/less should we grow the breasts over the base amount?
		public double TitsShrinkMultiplier = 1f; //how much more/less should we shrink the breasts over base amount?
		public double NewNippleSizeDelta; //how much do we add or remove to base amount for new Nipples? //NYI, but BigNipple Perks
		public double NippleGrowthMultiplier = 1.0f; //how much more/less should we grow a Nipple over the base amount?
		public double NippleShrinkMultiplier = 1.0f; //how much more/less should we shrink a Nipple over base amount?
		public double NewNippleDefaultLength; //minimum size for any new Nipples; //bro/futa perks for now
		public byte NewBallsSizeDelta; //how much do we add or remove to base amount for new Balls? //note, will only go to max size for uniball if uniball.
		public double BallsGrowthMultiplier = 1.0f; //how much more/less should we grow the Balls over the base amount? 1-3, expecting roughly 1.5
		public double BallsShrinkMultiplier = 1.0f; //how much more/less should we shrink the Balls over base amount? 1-3, expecting roughly 1.5
		public byte NewBallsDefaultSize; //note: will only go to uniball max if uniball.
		public bool AlwaysProducesMaxCum; //pilgrim perk
		public double BonusCumStacked = 1; //muliplicative
		public uint BonusCumAdded = 0; //additive.
		public ushort PerkBasedBonusVaginalCapacity; //vag of holding, elastic innards
		public ushort PerkBasedBonusAnalCapacity; //elastic innards
		//values that would cause issues (min cock size, vag wetness, etc) are purposely excluded. see top argument.
		//it'd be possible to have a perk that forces this to course correct almost immediately, if that's your desire - create a reaction that fires ASAP.

	*/
