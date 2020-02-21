//PassiveBaseStatModifiers.cs
//Description:
//Author: JustSomeGuy
//6/30/2019, 7:45 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using System;

namespace CoC.Backend.Perks
{
	//these are wired directly into the creature. unfortunately it's nigh impossible to wire

	public sealed class BasePerkModifiers
	{
		private readonly Creature source;
		private CombatCreature combatSource => source as CombatCreature;
		private PlayerBase playerSource => source as PlayerBase;

		public BasePerkModifiers(Creature parent)
		{
			source = parent ?? throw new ArgumentNullException(nameof(parent));
		}

		#region Stat Perk Helpers
		//These obviously do not apply to creatures that cannot participate in combat, like most NPCs.
		public ushort bonusMaxHP;

		public sbyte minStrength
		{
			get => source.bonusMinStrength;
			set => source.bonusMinStrength = value;
		}
		public sbyte bonusMaxStrength
		{
			get => source.bonusMaxStrength;
			set => source.bonusMaxStrength = value;
		}
		public double StrengthGainMultiplier
		{
			get => source.StrengthGainMultiplier;
			set => source.StrengthGainMultiplier = value;
		}
		public double StrengthLossMultiplier
		{
			get => source.StrengthLossMultiplier;
			set => source.StrengthLossMultiplier = value;
		}

		public sbyte minToughness
		{
			get => source.bonusMinToughness;
			set => source.bonusMinToughness = value;
		}
		public sbyte bonusMaxToughness
		{
			get => source.bonusMaxToughness;
			set => source.bonusMaxToughness = value;
		}
		public double ToughnessGainMultiplier
		{
			get => source.ToughnessGainMultiplier;
			set => source.ToughnessGainMultiplier = value;
		}
		public double ToughnessLossMultiplier
		{
			get => source.ToughnessLossMultiplier;
			set => source.ToughnessLossMultiplier = value;
		}

		public sbyte minSpeed
		{
			get => source.bonusMinSpeed;
			set => source.bonusMinSpeed = value;
		}
		public sbyte bonusMaxSpeed
		{
			get => source.bonusMaxSpeed;
			set => source.bonusMaxSpeed = value;
		}
		public double SpeedGainMultiplier
		{
			get => source.SpeedGainMultiplier;
			set => source.SpeedGainMultiplier = value;
		}
		public double SpeedLossMultiplier
		{
			get => source.SpeedLossMultiplier;
			set => source.SpeedLossMultiplier = value;
		}

		public sbyte minIntelligence
		{
			get => source.bonusMinIntelligence;
			set => source.bonusMinIntelligence = value;
		}
		public sbyte bonusMaxIntelligence
		{
			get => source.bonusMaxIntelligence;
			set => source.bonusMaxIntelligence = value;
		}
		public double IntelligenceGainMultiplier
		{
			get => source.IntelligenceGainMultiplier;
			set => source.IntelligenceGainMultiplier = value;
		}
		public double IntelligenceLossMultiplier
		{
			get => source.IntelligenceLossMultiplier;
			set => source.IntelligenceLossMultiplier = value;
		}

		public sbyte minSensitivity
		{
			get => source.bonusMinSensitivity;
			set => source.bonusMinSensitivity = value;
		}
		public sbyte bonusMaxSensitivity
		{
			get => source.bonusMaxSensitivity;
			set => source.bonusMaxSensitivity = value;
		}
		public double SensitivityGainMultiplier
		{
			get => source.SensitivityGainMultiplier;
			set => source.SensitivityGainMultiplier = value;
		}
		public double SensitivityLossMultiplier
		{
			get => source.SensitivityLossMultiplier;
			set => source.SensitivityLossMultiplier = value;
		}

		public sbyte minLust
		{
			get => source.bonusMinLust;
			set => source.bonusMinLust = value;
		}
		public sbyte bonusMaxLust
		{
			get => source.bonusMaxLust;
			set => source.bonusMaxLust = value;
		}
		public double LustGainMultiplier
		{
			get => source.LustGainMultiplier;
			set => source.LustGainMultiplier = value;
		}
		public double LustLossMultiplier
		{
			get => source.LustLossMultiplier;
			set => source.LustLossMultiplier = value;
		}

		public sbyte minLibido
		{
			get => source.bonusMinLibido;
			set => source.bonusMinLibido = value;
		}
		public sbyte bonusMaxLibido
		{
			get => source.bonusMaxLibido;
			set => source.bonusMaxLibido = value;
		}
		public double LibidoGainMultiplier
		{
			get => source.LibidoGainMultiplier;
			set => source.LibidoGainMultiplier = value;
		}
		public double LibidoLossMultiplier
		{
			get => source.LibidoLossMultiplier;
			set => source.LibidoLossMultiplier = value;
		}

		public sbyte minCorruption
		{
			get => source.bonusMinCorruption;
			set => source.bonusMinCorruption = value;
		}
		public double CorruptionGainMultiplier
		{
			get => source.CorruptionGainMultiplier;
			set => source.CorruptionGainMultiplier = value;
		}
		public double CorruptionLossMultiplier
		{
			get => source.CorruptionLossMultiplier;
			set => source.CorruptionLossMultiplier = value;
		}
		public sbyte bonusMaxCorruption
		{
			get => source.bonusMaxCorruption;
			set => source.bonusMaxCorruption = value;
		}

		public sbyte bonusMaxFatigue
		{
			get => combatSource?.bonusMaxFatigue ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMaxFatigue = value;
			}
		}

		public sbyte bonusMaxHunger
		{
			get => playerSource?.bonusMaxHunger ?? 0;
			set
			{
				if (playerSource != null) playerSource.bonusMaxHunger = value;
			}
		}

		public double HungerGainRate
		{
			get => playerSource?.hungerGainRate ?? 0f;
			set
			{
				if (playerSource != null) playerSource.hungerGainRate = value;
			}
		}

		public double fatigueRegenMultiplier
		{
			get => combatSource?.FatigueRegenRate ?? 0;
			set
			{
				if (combatSource != null) combatSource.FatigueRegenRate = value;
			}
		}
		#endregion
		public double gemGainMultiplier = 1.0f;

		public double healingMultiplier = 1.0f;
		#region Combat Perks
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
		public double combatDamageModifier { get; set; } = 1;
		public double magicalSpellCost { get; set; } = 1;
		public double physicalSpellCost { get; set; } = 1;
		public double armorEffectivenessMultiplier { get; set; } = 1;
		#endregion

		#region Pregnancy/Fertility

		public byte bonusFertility
		{
			get => source.genitals.fertility.perkBonusFertility;
			set => source.genitals.fertility.perkBonusFertility = value;
		}

		public void incPregSpeedByOne()
		{
			source.genitals.womb.pregnancyMultiplierCounter += 2;
		}
		public void incPregSpeedByHalf()
		{
			source.genitals.womb.pregnancyMultiplierCounter += 1;
		}

		public void decPregSpeedByOne()
		{
			source.genitals.womb.pregnancyMultiplierCounter -= 2;
		}
		public void decPregSpeedByHalf()
		{
			source.genitals.womb.pregnancyMultiplierCounter -= 1;
		}

		public double pregnancyMultiplier => source.genitals.womb.pregnancyMultiplier;


		//below is the actual formula.

		#endregion
		//For the following endowments - the behavior is different based on how the endowment is generated -
		//if no size is provided, the size will be the default new value.
		//if a size is provided, the delta value will be added to it. if this value is still lower than the default new value, the default new value will be used instead.

		#region General

		internal bool CalculationsTreatAsIfMaxLust
		{
			get => _CalculationsTreatAsIfMaxLust;
			set
			{
				if (_CalculationsTreatAsIfMaxLust != value)
				{
					_CalculationsTreatAsIfMaxLust = value;
				}
				source.genitals.perkData.TreatAllFluidsAsIfAtMaxLust = _CalculationsTreatAsIfMaxLust;
			}
		}
		private bool _CalculationsTreatAsIfMaxLust = false;
		#endregion

		#region Cock
		//added to any non-default cock size.
		public double NewCockSizeDelta
		{
			get => source.genitals.perkData.NewCockSizeDelta;
			set => source.genitals.perkData.NewCockSizeDelta = value;
		}

		//affects the rate at which the cock grows. this base growth amount is multiplied by this value.
		public double CockGrowthMultiplier
		{
			get => source.genitals.perkData.CockGrowthMultiplier;
			set => source.genitals.perkData.CockGrowthMultiplier = value;
		}
		//affects the rate at which the cock shrinks. this base shrink amount is multiplied by this value.
		public double CockShrinkMultiplier
		{
			get => source.genitals.perkData.CockShrinkMultiplier;
			set => source.genitals.perkData.CockShrinkMultiplier = value;
		}

		//if no size is provided, this value is used for a new cock.
		public double NewCockDefaultSize
		{
			get => source.genitals.perkData.NewCockDefaultSize;
			set => source.genitals.perkData.NewCockDefaultSize = value;
		}

		//minimum size for a cock. behavior for what happens when attempting to shrink below this depends on the source.
		public double MinCockSize
		{
			get => source.genitals.perkData.MinCockLength;
			set => source.genitals.perkData.MinCockLength = value;
		}

		public double perkBonusVirilityMultiplier
		{
			get => source.genitals.perkData.perkBonusVirilityMultiplier;
			set => source.genitals.perkData.perkBonusVirilityMultiplier = value;
		}

		public sbyte perkBonusVirility
		{
			get => source.genitals.perkData.perkBonusVirility;
			set => source.genitals.perkData.perkBonusVirility = value;
		}
		#endregion
		#region Vagina
		#region Clit
		//offset for new clits with non-default value.
		public double NewClitSizeDelta
		{
			get => source.genitals.perkData.NewClitSizeDelta;
			set => source.genitals.perkData.NewClitSizeDelta = value;
		}

		//affects rate of growth of clit.
		public double ClitGrowthMultiplier
		{
			get => source.genitals.perkData.ClitGrowthMultiplier;
			set => source.genitals.perkData.ClitGrowthMultiplier = value;
		}

		//affects clit shrink rate.
		public double ClitShrinkMultiplier
		{
			get => source.genitals.perkData.ClitShrinkMultiplier;
			set => source.genitals.perkData.ClitShrinkMultiplier = value;
		}

		//affects new clit size.
		public double DefaultNewClitSize
		{
			get => source.genitals.perkData.DefaultNewClitSize;
			set => source.genitals.perkData.DefaultNewClitSize = value;
		}

		public double MinClitSize
		{
			get => source.genitals.perkData.MinClitSize;
			set => source.genitals.perkData.MinClitSize = value;
		}
		#endregion
		public VaginalWetness? NewVaginaDefaultWetness
		{
			get => source.genitals.perkData.defaultNewVaginaWetness;
			set => source.genitals.perkData.defaultNewVaginaWetness = value;
		}
		public VaginalLooseness? NewVaginaDefaultLooseness
		{
			get => source.genitals.perkData.defaultNewVaginaLooseness;
			set => source.genitals.perkData.defaultNewVaginaLooseness = value;
		}

		public VaginalLooseness minVaginalLooseness
		{
			get => source.genitals.perkData.minVaginalLooseness;
			set => source.genitals.perkData.minVaginalLooseness = value;
		}

		public VaginalLooseness maxVaginalLooseness
		{
			get => source.genitals.perkData.maxVaginalLooseness;
			set => source.genitals.perkData.maxVaginalLooseness = value;
		}

		public VaginalWetness minVaginalWetness
		{
			get => source.genitals.perkData.minVaginalWetness;
			set => source.genitals.perkData.minVaginalWetness = value;
		}

		public VaginalWetness maxVaginalWetness
		{
			get => source.genitals.perkData.maxVaginalWetness;
			set => source.genitals.perkData.maxVaginalWetness = value;
		}

		public ushort PerkBasedBonusVaginalCapacity
		{
			get => source.genitals.perkData.perkBonusVaginalCapacity;
			set => source.genitals.perkData.perkBonusVaginalCapacity = value;
		}
		#endregion
		#region Breasts
		//These are split by gender. Note that herms use female, and genderless use male.

		public sbyte FemaleNewBreastCupSizeDelta
		{
			get => source.genitals.perkData.FemaleNewCupDelta;
			set => source.genitals.perkData.FemaleNewCupDelta = value;
		}

		//how much do we add or remove to base amount for new Breast Rows?// BigTits Perks
		public CupSize FemaleNewBreastDefaultCupSize
		{
			get => source.genitals.perkData.FemaleNewDefaultCup;
			set => source.genitals.perkData.FemaleNewDefaultCup = value;
		}

		public sbyte MaleNewBreastCupSizeDelta
		{
			get => source.genitals.perkData.MaleNewCupDelta;
			set => source.genitals.perkData.MaleNewCupDelta = value;
		}

		public CupSize MaleNewBreastDefaultCupSize
		{
			get => source.genitals.perkData.MaleNewDefaultCup;
			set => source.genitals.perkData.MaleNewDefaultCup = value;
		}


		public CupSize FemaleMinCupSize
		{
			get => source.genitals.perkData.FemaleMinCup;
			set => source.genitals.perkData.FemaleMinCup = value;
		}
		public CupSize MaleMinCupSize
		{
			get => source.genitals.perkData.MaleMinCup;
			set => source.genitals.perkData.MaleMinCup = value;
		}


		public double TitsGrowthMultiplier
		{
			get => source.genitals.perkData.TitsGrowthMultiplier;
			set => source.genitals.perkData.TitsGrowthMultiplier = value;
		}

		public double TitsShrinkMultiplier
		{
			get => source.genitals.perkData.TitsShrinkMultiplier;
			set => source.genitals.perkData.TitsShrinkMultiplier = value;
		}
		#region Nipples

		public double NewNippleSizeDelta
		{
			get => source.genitals.perkData.NewNippleSizeDelta;
			set => source.genitals.perkData.NewNippleSizeDelta = value;
		}

		public double NippleGrowthMultiplier
		{
			get => source.genitals.perkData.NippleGrowthMultiplier;
			set => source.genitals.perkData.NippleGrowthMultiplier = value;
		}

		public double NippleShrinkMultiplier
		{
			get => source.genitals.perkData.NippleShrinkMultiplier;
			set => source.genitals.perkData.NippleShrinkMultiplier = value;
		}

		public double NewNippleDefaultLength
		{
			get => source.genitals.perkData.NewNippleDefaultLength;
			set => source.genitals.perkData.NewNippleDefaultLength = value;
		}
		#endregion
		#endregion
		#region Balls
		public sbyte NewBallsSizeDelta
		{
			get => source.genitals.balls.newSizeOffset;
			set => source.genitals.balls.newSizeOffset = value;
		}

		public double BallsGrowthMultiplier
		{
			get => source.genitals.balls.growthMultiplier;
			set => source.genitals.balls.growthMultiplier = value;
		}

		public double BallsShrinkMultiplier
		{
			get => source.genitals.balls.shrinkMultiplier;
			set => source.genitals.balls.shrinkMultiplier = value;
		}

		public byte NewBallsDefaultSize
		{
			get => source.genitals.balls.defaultNewSize;
			set => source.genitals.balls.defaultNewSize = value;
		}
		#endregion
		#region Ass
		public AnalLooseness minAnalLooseness
		{
			get => source.ass.minLooseness;
			set => source.ass.minLooseness = value;
		}

		public AnalLooseness maxAnalLooseness
		{
			get => source.ass.maxLooseness;
			set => source.ass.maxLooseness = value;
		}

		public AnalWetness minAnalWetness
		{
			get => source.ass.minWetness;
			set => source.ass.minWetness = value;
		}

		public AnalWetness maxAnalWetness
		{
			get => source.ass.maxWetness;
			set => source.ass.maxWetness = value;
		}
		public ushort PerkBasedBonusAnalCapacity
		{
			get => source.ass.perkBonusAnalCapacity;
			set => source.ass.perkBonusAnalCapacity = value;
		}
		#endregion
		#region Cum
		//pilgrim's bounty.
		public bool AlwaysProducesMaxCum
		{
			get => source.genitals.perkData.alwaysProducesMaxCum;
			set => source.genitals.perkData.alwaysProducesMaxCum = value;
		}

		//multiplies the base cum and all non-perk bonus cum by the stack amount.
		public double BonusCumStacked
		{
			get => source.genitals.perkData.bonusCumMultiplier;
			set => source.genitals.perkData.bonusCumMultiplier = value;
		}

		//After the stacking is done, this value is added in.
		public uint BonusCumAdded
		{
			get => source.genitals.perkData.bonusCumAdded;
			set => source.genitals.perkData.bonusCumAdded = value;
		}
		#endregion
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
