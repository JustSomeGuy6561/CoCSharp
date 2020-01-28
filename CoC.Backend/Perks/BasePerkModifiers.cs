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
			get => combatSource?.bonusMinStrength ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMinStrength = value;
			}
		}
		public sbyte bonusMaxStrength
		{
			get => combatSource?.bonusMaxStrength ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMaxStrength = value;
			}
		}
		public float StrengthGainMultiplier
		{
			get => combatSource?.StrengthGainMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.StrengthGainMultiplier = value;
			}
		}
		public float StrengthLossMultiplier
		{
			get => combatSource?.StrengthLossMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.StrengthLossMultiplier = value;
			}
		}

		public sbyte minToughness
		{
			get => combatSource?.bonusMinToughness ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMinToughness = value;
			}
		}
		public sbyte bonusMaxToughness
		{
			get => combatSource?.bonusMaxToughness ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMaxToughness = value;
			}
		}
		public float ToughnessGainMultiplier
		{
			get => combatSource?.ToughnessGainMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.ToughnessGainMultiplier = value;
			}
		}
		public float ToughnessLossMultiplier
		{
			get => combatSource?.ToughnessLossMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.ToughnessLossMultiplier = value;
			}
		}

		public sbyte minSpeed
		{
			get => combatSource?.bonusMinSpeed ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMinSpeed = value;
			}
		}
		public sbyte bonusMaxSpeed
		{
			get => combatSource?.bonusMaxSpeed ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMaxSpeed = value;
			}
		}
		public float SpeedGainMultiplier
		{
			get => combatSource?.SpeedGainMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.SpeedGainMultiplier = value;
			}
		}
		public float SpeedLossMultiplier
		{
			get => combatSource?.SpeedLossMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.SpeedLossMultiplier = value;
			}
		}

		public sbyte minIntelligence
		{
			get => combatSource?.bonusMinIntelligence ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMinIntelligence = value;
			}
		}
		public sbyte bonusMaxIntelligence
		{
			get => combatSource?.bonusMaxIntelligence ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMaxIntelligence = value;
			}
		}
		public float IntelligenceGainMultiplier
		{
			get => combatSource?.IntelligenceGainMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.IntelligenceGainMultiplier = value;
			}
		}
		public float IntelligenceLossMultiplier
		{
			get => combatSource?.IntelligenceLossMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.IntelligenceLossMultiplier = value;
			}
		}

		public sbyte minSensitivity
		{
			get => combatSource?.bonusMinSensitivity ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMinSensitivity = value;
			}
		}
		public sbyte bonusMaxSensitivity
		{
			get => combatSource?.bonusMaxSensitivity ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMaxSensitivity = value;
			}
		}
		public float SensitivityGainMultiplier
		{
			get => combatSource?.SensitivityGainMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.SensitivityGainMultiplier = value;
			}
		}
		public float SensitivityLossMultiplier
		{
			get => combatSource?.SensitivityLossMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.SensitivityLossMultiplier = value;
			}
		}

		public sbyte minLust
		{
			get => combatSource?.bonusMinLust ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMinLust = value;
			}
		}
		public sbyte bonusMaxLust
		{
			get => combatSource?.bonusMaxLust ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMaxLust = value;
			}
		}
		public float LustGainMultiplier
		{
			get => combatSource?.LustGainMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.LustGainMultiplier = value;
			}
		}
		public float LustLossMultiplier
		{
			get => combatSource?.LustLossMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.LustLossMultiplier = value;
			}
		}

		public sbyte minLibido
		{
			get => combatSource?.bonusMinLibido ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMinLibido = value;
			}
		}
		public sbyte bonusMaxLibido
		{
			get => combatSource?.bonusMaxLibido ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMaxLibido = value;
			}
		}
		public float LibidoGainMultiplier
		{
			get => combatSource?.LibidoGainMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.LibidoGainMultiplier = value;
			}
		}
		public float LibidoLossMultiplier
		{
			get => combatSource?.LibidoLossMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.LibidoLossMultiplier = value;
			}
		}

		public sbyte minCorruption
		{
			get => combatSource?.bonusMinCorruption ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMinCorruption = value;
			}
		}
		public float CorruptionGainMultiplier
		{
			get => combatSource?.CorruptionGainMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.CorruptionGainMultiplier = value;
			}
		}
		public float CorruptionLossMultiplier
		{
			get => combatSource?.CorruptionLossMultiplier ?? 0;
			set
			{
				if (combatSource != null) combatSource.CorruptionLossMultiplier = value;
			}
		}
		public sbyte bonusMaxCorruption
		{
			get => combatSource?.bonusMaxCorruption ?? 0;
			set
			{
				if (combatSource != null) combatSource.bonusMaxCorruption = value;
			}
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

		public float HungerGainRate
		{
			get => playerSource?.hungerGainRate ?? 0f;
			set
			{
				if (playerSource != null) playerSource.hungerGainRate = value;
			}
		}

		public float fatigueRegenMultiplier
		{
			get => combatSource?.FatigueRegenRate ?? 0;
			set
			{
				if (combatSource != null) combatSource.FatigueRegenRate = value;
			}
		}
		#endregion

		#region Combat Perks
		//Not Implemented
		//public float combatDamageModifier
		//{
		//	get => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//	set => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//}
		//public float magicalSpellCost
		//{
		//	get => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//	set => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//}
		//public float physicalSpellCost
		//{
		//	get => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//	set => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//}
		//public float armorEffectivenessMultiplier
		//{
		//	get => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//	set => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//}
		public float combatDamageModifier { get; set; } = 1;
		public float magicalSpellCost { get; set; } = 1;
		public float physicalSpellCost { get; set; } = 1;
		public float armorEffectivenessMultiplier { get; set; } = 1;
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

		public float pregnancyMultiplier => source.genitals.womb.pregnancyMultiplier;

		public bool hasDiapause
		{
			get => source.genitals.perkData.hasDiapause;
			set => source.genitals.perkData.hasDiapause = value;
		}


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
		public float NewCockSizeDelta
		{
			get => source.genitals.perkData.NewCockSizeDelta;
			set => source.genitals.perkData.NewCockSizeDelta = value;
		}

		//affects the rate at which the cock grows. this base growth amount is multiplied by this value.
		public float CockGrowthMultiplier
		{
			get => source.genitals.perkData.CockGrowthMultiplier;
			set => source.genitals.perkData.CockGrowthMultiplier = value;
		}
		//affects the rate at which the cock shrinks. this base shrink amount is multiplied by this value.
		public float CockShrinkMultiplier
		{
			get => source.genitals.perkData.CockShrinkMultiplier;
			set => source.genitals.perkData.CockShrinkMultiplier = value;
		}

		//if no size is provided, this value is used for a new cock.
		public float NewCockDefaultSize
		{
			get => source.genitals.perkData.NewCockDefaultSize;
			set => source.genitals.perkData.NewCockDefaultSize = value;
		}

		//minimum size for a cock. behavior for what happens when attempting to shrink below this depends on the source.
		public float MinCockSize
		{
			get => source.genitals.perkData.MinCockLength;
			set => source.genitals.perkData.MinCockLength = value;
		}

		public float perkBonusVirilityMultiplier
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
		public float NewClitSizeDelta
		{
			get => source.genitals.perkData.NewClitSizeDelta;
			set => source.genitals.perkData.NewClitSizeDelta = value;
		}

		//affects rate of growth of clit.
		public float ClitGrowthMultiplier
		{
			get => source.genitals.perkData.ClitGrowthMultiplier;
			set => source.genitals.perkData.ClitGrowthMultiplier = value;
		}

		//affects clit shrink rate.
		public float ClitShrinkMultiplier
		{
			get => source.genitals.perkData.ClitShrinkMultiplier;
			set => source.genitals.perkData.ClitShrinkMultiplier = value;
		}

		//affects new clit size.
		public float DefaultNewClitSize
		{
			get => source.genitals.perkData.DefaultNewClitSize;
			set => source.genitals.perkData.DefaultNewClitSize = value;
		}

		public float MinClitSize
		{
			get => source.genitals.perkData.MinClitSize;
			set => source.genitals.perkData.MinClitSize = value;
		}
		#endregion
		public VaginalWetness NewVaginaDefaultWetness
		{
			get => source.genitals.perkData.defaultNewVaginaWetness;
			set => source.genitals.perkData.defaultNewVaginaWetness = value;
		}
		public VaginalLooseness NewVaginaDefaultLooseness
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


		public float TitsGrowthMultiplier
		{
			get => source.genitals.perkData.TitsGrowthMultiplier;
			set => source.genitals.perkData.TitsGrowthMultiplier = value;
		}

		public float TitsShrinkMultiplier
		{
			get => source.genitals.perkData.TitsShrinkMultiplier;
			set => source.genitals.perkData.TitsShrinkMultiplier = value;
		}
		#region Nipples

		public float NewNippleSizeDelta
		{
			get => source.genitals.perkData.NewNippleSizeDelta;
			set => source.genitals.perkData.NewNippleSizeDelta = value;
		}

		public float NippleGrowthMultiplier
		{
			get => source.genitals.perkData.NippleGrowthMultiplier;
			set => source.genitals.perkData.NippleGrowthMultiplier = value;
		}

		public float NippleShrinkMultiplier
		{
			get => source.genitals.perkData.NippleShrinkMultiplier;
			set => source.genitals.perkData.NippleShrinkMultiplier = value;
		}

		public float NewNippleDefaultLength
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

		public float BallsGrowthMultiplier
		{
			get => source.genitals.balls.growthMultiplier;
			set => source.genitals.balls.growthMultiplier = value;
		}

		public float BallsShrinkMultiplier
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
		public float BonusCumStacked
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

		public float NewCockSizeDelta; //how much do we add or remove for new cocks? //big cock perk for now. would allow a small cock perk as well
		public float CockGrowthMultiplier = 1f; //how much more/less should we grow a cock over the base amount? //big cock perk, cockSock;
		public float CockShrinkMultiplier = 1f; //how much more/less should we shrink a cock over base amount? //big cock, cockSock;
		public float NewCockDefaultSize; //minimum size for any new cocks; //bro/futa perks for now
		public float NewClitSizeDelta; //how much do we add or remove to base amount for new Clits? //NYI, but BigClit Perks
		public float ClitGrowthMultiplier = 1f; //how much more/less should we grow a Clit over the base amount?
		public float ClitShrinkMultiplier = 1f; //how much more/less should we shrink a Clit over base amount?
		public float MinNewClitSize; //minimum size for any new Clits; //bro/futa perks for now
		public float MinClitSize; //minimum size for any Clit; //bro/futa perks for now
		//These are split by gender. Note that herms use female, and genderless use male.
		public sbyte FemaleNewBreastCupSizeDelta; //how much do we add or remove to base amount for new Breast Rows?// BigTits Perks
		public CupSize FemaleNewBreastDefaultCupSize; //minimum size for any new row of breasts; //bro/futa perks for now
		public sbyte MaleNewBreastCupSizeDelta; //how much do we add or remove to base amount for new Breast Rows?// BigTits Perks
		public CupSize MaleNewBreastDefaultCupSize; //minimum size for any new row of breasts; //bro/futa perks for now
		//these are used regardless of gender.
		public float TitsGrowthMultiplier = 1f; //how much more/less should we grow the breasts over the base amount?
		public float TitsShrinkMultiplier = 1f; //how much more/less should we shrink the breasts over base amount?
		public float NewNippleSizeDelta; //how much do we add or remove to base amount for new Nipples? //NYI, but BigNipple Perks
		public float NippleGrowthMultiplier = 1.0f; //how much more/less should we grow a Nipple over the base amount?
		public float NippleShrinkMultiplier = 1.0f; //how much more/less should we shrink a Nipple over base amount?
		public float NewNippleDefaultLength; //minimum size for any new Nipples; //bro/futa perks for now
		public byte NewBallsSizeDelta; //how much do we add or remove to base amount for new Balls? //note, will only go to max size for uniball if uniball.
		public float BallsGrowthMultiplier = 1.0f; //how much more/less should we grow the Balls over the base amount? 1-3, expecting roughly 1.5
		public float BallsShrinkMultiplier = 1.0f; //how much more/less should we shrink the Balls over base amount? 1-3, expecting roughly 1.5
		public byte NewBallsDefaultSize; //note: will only go to uniball max if uniball.
		public bool AlwaysProducesMaxCum; //pilgrim perk
		public float BonusCumStacked = 1; //muliplicative
		public uint BonusCumAdded = 0; //additive.
		public ushort PerkBasedBonusVaginalCapacity; //vag of holding, elastic innards
		public ushort PerkBasedBonusAnalCapacity; //elastic innards
		//values that would cause issues (min cock size, vag wetness, etc) are purposely excluded. see top argument.
		//it'd be possible to have a perk that forces this to course correct almost immediately, if that's your desire - create a reaction that fires ASAP.

	*/
