//CombatCreature.cs
//Description:
//Author: JustSomeGuy
//2/20/2019, 4:14 PM
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.Creatures
{

	public abstract class CombatCreature : Creature
	{
		public const byte DEFAULT_STRENGTH = 15;
		public const byte DEFAULT_TOUGHNESS = 15;
		public const byte DEFAULT_SPEED = 15;
		public const byte DEFAULT_INTELLIGENCE = 15;
		public const byte DEFAULT_LIBIDO = 0;
		public const byte DEFAULT_SENSITIVITY = 0;
		public const byte DEFAULT_CORRUPTION = 0;
		//always start as these values;
		public const byte DEFAULT_LUST = 0;
		public const byte DEFAULT_FATIGUE = 0;
		//public const byte DEFAULT_HUNGER = 0;

		internal const byte BASE_MAX_STRENGTH = 100;
		internal const byte BASE_MAX_TOUGHNESS = 100;
		internal const byte BASE_MAX_SPEED = 100;
		internal const byte BASE_MAX_INTELLIGENCE = 100;
		internal const byte BASE_MAX_LIBIDO = 100;
		internal const byte BASE_MAX_SENSITIVITY = 100;
		internal const byte BASE_MAX_CORRUPTION = 100;
		internal const byte BASE_MAX_LUST = 100;
		internal const byte BASE_MAX_FATIGUE = 100;

		public const byte LOWEST_POSSIBLE_MAX = 50;

		private const byte ZERO = 0;

		//Note: Min stat is given priority for all of these - if a computed max value is less than the current minimum, the minimum is the maximum. 
		//all max stats are floored (capped below) to 50, meaning they cannot drop below 50. 
		//ideally i'd prefer to cap mins to the same value, but that doesn't seem to be the case in given code. 

		public byte level
		{
			get;
			private protected set;
		} = 1;

		public virtual byte maxLevel => 50; //idk, could be anything really. iirc the player max is 30. 

		public uint totalExperience
		{
			get => _experience;
			private protected set => _experience = Math.Min(value, maxExperience);
		}
		private uint _experience = 0;

#warning fix me!
		public uint maxExperience => ushort.MaxValue; 

		public uint currentExperience => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease(); //computed based on level and total experience.
		public uint experienceRequiredForNextLevel => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease(); //computed based on level.

		public uint currentHealth
		{
			get => _currentHealth;
			private protected set => _currentHealth = Utils.Clamp2(value, (uint)0, maxHealth);
		}
		private uint _currentHealth = 0;
		public byte strength
		{
			get => _strength;
			private protected set => _strength = Utils.Clamp2(value, minStrength, maxStrength);
		}
		private byte _strength = 0;

		public byte toughness
		{
			get => _toughness;
			private protected set => _toughness = Utils.Clamp2(value, minToughness, maxToughness);
		}
		private byte _toughness = 0;
		public byte speed
		{
			get => _speed;
			private protected set => _speed = Utils.Clamp2(value, minSpeed, maxSpeed);
		}
		private byte _speed = 0;
		public byte intelligence
		{
			get => _intelligence;
			private protected set => _intelligence = Utils.Clamp2(value, minIntelligence, maxIntelligence);
		}
		private byte _intelligence = 0;

		public byte libido
		{
			get => _libido;
			private protected set => _libido = Utils.Clamp2(value, minLibido, maxLibido);
		}
		private byte _libido = 0;

		public byte sensitivity
		{
			get => _sensitivity;
			private protected set => _sensitivity = Utils.Clamp2(value, minSensitivity, maxSensitivity);
		}
		private byte _sensitivity = 0;

		public byte corruption
		{
			get => _corruption;
			private protected set => _corruption = Utils.Clamp2(value, minCorruption, maxCorruption);
		}
		private byte _corruption = 0;

		public byte lust
		{
			get => _lust;
			private protected set => _lust = Utils.Clamp2(value, minLust, maxLust);
		}
		private byte _lust = 0;

		public byte fatigue
		{
			get => _fatigue;
			private protected set => _fatigue = Utils.Clamp2(value, minFatigue, maxFatigue);
		}
		private byte _fatigue = 0;
		//public byte hunger
		//{
		//	get => _hunger;
		//	private protected set => _hunger = Utils.Clamp2(value, minHunger, maxHunger);
		//}
		//private byte _hunger = 0;

		protected internal virtual sbyte bonusMinStrength { get; set; }
		protected virtual byte baseMinStrength => 0;
		public byte minStrength => baseMinStrength.delta(bonusMinStrength);

		

		protected internal virtual sbyte bonusMinToughness { get; set; }
		protected virtual byte baseMinToughness => 0;
		public byte minToughness => baseMinToughness.delta(bonusMinToughness);



		protected internal virtual sbyte bonusMinSpeed { get; set; }
		protected virtual byte baseMinSpeed => 0;
		public byte minSpeed => baseMinSpeed.delta(bonusMinSpeed);


		protected internal virtual sbyte bonusMinIntelligence { get; set; }
		protected virtual byte baseMinIntelligence => 0;
		public byte minIntelligence => baseMinIntelligence.delta(bonusMinIntelligence);


		protected internal virtual sbyte bonusMinLibido { get; set; }
		protected virtual byte baseMinLibido => 0;
		public byte minLibido => baseMinLibido.delta(bonusMinLibido);


		protected internal virtual sbyte bonusMinSensitivity { get; set; }
		protected virtual byte baseMinSensitivity => 0;
		public byte minSensitivity => baseMinSensitivity.delta(bonusMinSensitivity);


		protected internal virtual sbyte bonusMinCorruption { get; set; }
		protected virtual byte baseMinCorruption => 0;
		public byte minCorruption => baseMinCorruption.delta(bonusMinCorruption);


		protected internal virtual sbyte bonusMinLust { get; set; }
		protected virtual byte baseMinLust => 0;
		public byte minLust => baseMinLust.delta(bonusMinLust);


		protected internal virtual sbyte bonusMinFatigue { get; set; }
		protected virtual byte baseMinFatigue => 0;
		public byte minFatigue => baseMinFatigue.delta(bonusMinFatigue);




		//public byte minHunger => 0;

		public abstract uint maxHealth { get; }

		protected internal int perkBonusHealth { get; set; }

		protected internal virtual byte baseMaxStrength => BASE_MAX_STRENGTH;
		protected internal virtual sbyte bonusMaxStrength { get; set; } = 0;
		public byte maxStrength => HandleMaxStat(baseMaxStrength.delta(bonusMaxStrength), minStrength);

		protected internal virtual byte baseMaxToughness => BASE_MAX_STRENGTH;
		protected internal virtual sbyte bonusMaxToughness { get; set; } = 0;
		public byte maxToughness => HandleMaxStat(baseMaxToughness.delta(bonusMaxToughness), minToughness);

		protected internal virtual byte baseMaxSpeed => BASE_MAX_STRENGTH;
		protected internal virtual sbyte bonusMaxSpeed { get; set; } = 0;
		public byte maxSpeed => HandleMaxStat(baseMaxSpeed.delta(bonusMaxSpeed), minSpeed);

		protected internal virtual byte baseMaxIntelligence => BASE_MAX_STRENGTH;
		protected internal virtual sbyte bonusMaxIntelligence { get; set; } = 0;
		public byte maxIntelligence => HandleMaxStat(baseMaxIntelligence.delta(bonusMaxIntelligence), minIntelligence);

		protected internal virtual byte baseMaxLibido => BASE_MAX_STRENGTH;
		protected internal virtual sbyte bonusMaxLibido { get; set; } = 0;
		public byte maxLibido => HandleMaxStat(baseMaxLibido.delta(bonusMaxLibido), minLibido);

		protected internal virtual byte baseMaxSensitivity => BASE_MAX_STRENGTH;
		protected internal virtual sbyte bonusMaxSensitivity { get; set; } = 0;
		public byte maxSensitivity => HandleMaxStat(baseMaxSensitivity.delta(bonusMaxSensitivity), minSensitivity);

		protected internal virtual byte baseMaxCorruption => BASE_MAX_STRENGTH;
		protected internal virtual sbyte bonusMaxCorruption { get; set; } = 0;
		public byte maxCorruption => HandleMaxStat(baseMaxCorruption.delta(bonusMaxCorruption), minCorruption);

		protected internal virtual byte baseMaxLust => BASE_MAX_STRENGTH;
		protected internal virtual sbyte bonusMaxLust { get; set; } = 0;
		public byte maxLust => HandleMaxStat(baseMaxLust.delta(bonusMaxLust), minLust);

		protected internal virtual byte baseMaxFatigue => BASE_MAX_STRENGTH;
		protected internal virtual sbyte bonusMaxFatigue { get; set; } = 0;
		public byte maxFatigue => HandleMaxStat(baseMaxFatigue.delta(bonusMaxFatigue), minFatigue);

		protected internal float StrengthGainMultiplier = 1.0f;
		protected internal float StrengthLossMultiplier = 1.0f;

		protected internal float ToughnessGainMultiplier = 1.0f;
		protected internal float ToughnessLossMultiplier = 1.0f;

		protected internal float SpeedGainMultiplier = 1.0f;
		protected internal float SpeedLossMultiplier = 1.0f;

		protected internal float IntelligenceGainMultiplier = 1.0f;
		protected internal float IntelligenceLossMultiplier = 1.0f;

		protected internal float LibidoGainMultiplier = 1.0f;
		protected internal float LibidoLossMultiplier = 1.0f;

		protected internal float SensitivityGainMultiplier = 1.0f;
		protected internal float SensitivityLossMultiplier = 1.0f;

		protected internal float CorruptionGainMultiplier = 1.0f;
		protected internal float CorruptionLossMultiplier = 1.0f;

		protected internal float LustGainMultiplier = 1.0f;
		protected internal float LustLossMultiplier = 1.0f;

		protected internal float FatigueRegenRate = 1.0f;

		protected byte HandleMaxStat(byte computedValue, byte minValue)
		{
			if (computedValue >= minValue && computedValue >= LOWEST_POSSIBLE_MAX)
			{
				return computedValue;
			}
			else if (minValue > LOWEST_POSSIBLE_MAX)
			{
				return minValue;
			}
			else return LOWEST_POSSIBLE_MAX;
		}

		//public virtual byte maxHunger => BASE_MAX_HUNGER.delta(modifiers.bonusMaxHunger);

		//will, etc are player-specific. 

		//Combat Attributes - taunt strength, bow skill, magic skill, magical aptitude, etc.
		//Equipment
		//Inventory

		public int gems { get; protected set; }

		public int addGems(uint amount)
		{
			int oldGems = gems;
			gems += amount > int.MaxValue ? int.MaxValue : (int)amount;
			return gems - oldGems;
		}

		public int removeGems(uint amount, bool canGoNegative = false)
		{
			int lossAmt = amount > int.MaxValue ? int.MaxValue : (int)amount;
			int oldGems = gems;
			if (gems < 0 && !canGoNegative)
			{
				return 0;
			}
			gems -= lossAmt;
			if (gems < 0 && !canGoNegative)
			{
				gems = 0;
			}
			return gems - oldGems; //i can flip this if we want.

		}

		//public void dynStats(sbyte str = 0, sbyte tou = 0, sbyte spd = 0, sbyte inte = 0, sbyte lib = 0, sbyte sens = 0, sbyte corr = 0, sbyte lus = 0)
		//{
		//	strength = strength.delta(str);
		//	toughness = toughness.delta(tou);
		//	speed = speed.delta(spd);
		//	intelligence = intelligence.delta(inte);
		//	libido = libido.delta(lib);
		//	sensitivity = sensitivity.delta(sens);
		//	corruption = corruption.delta(corr);
		//	lust = lust.delta(lus);
		//}

		public CombatCreature(CombatCreatureCreator creator) : base(creator)
		{
			//modifiers is valid already because perks is not null.
			//creator is not null or the base would have thrown.
			totalExperience = creator.initialXP;
			level = creator.initialLevel;

			strength = creator.strength ?? DEFAULT_STRENGTH;
			toughness = creator.toughness ?? DEFAULT_TOUGHNESS;
			speed = creator.speed ?? DEFAULT_SPEED;
			intelligence = creator.intelligence ?? DEFAULT_INTELLIGENCE;
			libido = creator.libido ?? DEFAULT_LIBIDO;
			sensitivity = creator.sensitivity ?? DEFAULT_SENSITIVITY;
			corruption = creator.corruption ?? DEFAULT_CORRUPTION;
			lust = DEFAULT_LUST;
			fatigue = DEFAULT_FATIGUE;
			//hunger = DEFAULT_HUNGER;

			currentHealth = maxHealth;
		}

		public float spellCost(double baseCost)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		public float physicalCost(double baseCost)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		//public float availableStamina => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();

		public bool hasEnoughStamina(double baseCost, bool isPhysical)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		//internal CombatCreature(SurrogateCombatCreator surrogateCreator) : base(surrogateCreator)
		//{

		//}
	}
}
