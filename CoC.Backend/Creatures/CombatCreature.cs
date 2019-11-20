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

		public const byte DEFAULT_FATIGUE = 0;
		//public const byte DEFAULT_HUNGER = 0;

		internal const byte BASE_MAX_STRENGTH = 100;
		internal const byte BASE_MAX_TOUGHNESS = 100;
		internal const byte BASE_MAX_SPEED = 100;
		internal const byte BASE_MAX_INTELLIGENCE = 100;
		internal const byte BASE_MAX_FATIGUE = 100;

		public void AddHP(uint flatAmount)
		{
			throw new NotImplementedException();
		}

		public void AddHPPercent(float percent)
		{
			throw new NotImplementedException();
		}

		public void TakeDamage(uint amount)
		{
			throw new NotImplementedException();
		}

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
		public byte strength => (byte)Math.Floor(strengthTrue);
		public float strengthTrue
		{
			get => _strength;
			private protected set => _strength = Utils.Clamp2(value, minStrength, maxStrength);
		}
		private float _strength = 0;

		public float relativeStrength => strengthTrue * (100f / maxStrength);

		public byte toughness => (byte)Math.Floor(toughnessTrue);
		public float toughnessTrue
		{
			get => _toughness;
			private protected set => _toughness = Utils.Clamp2(value, minToughness, maxToughness);
		}
		private float _toughness = 0;

		public float relativeToughness => toughnessTrue * (100f / maxToughness);

		public byte speed => (byte)Math.Floor(speedTrue);
		public float speedTrue
		{
			get => _speed;
			private protected set => _speed = Utils.Clamp2(value, minSpeed, maxSpeed);
		}
		private float _speed = 0;

		public float relativeSpeed => speedTrue * (100f / maxSpeed);

		public byte intelligence => (byte)Math.Floor(intelligenceTrue);
		public float intelligenceTrue
		{
			get => _intelligence;
			private protected set => _intelligence = Utils.Clamp2(value, minIntelligence, maxIntelligence);
		}
		private float _intelligence = 0;

		public float relativeIntelligence => intelligenceTrue * (100f / maxIntelligence);

		public byte fatigue => (byte)Math.Floor(fatigueTrue);
		public float fatigueTrue
		{
			get => _fatigue;
			private protected set => _fatigue = Utils.Clamp2(value, minFatigue, maxFatigue);
		}
		private float _fatigue = 0;

		public float relativeFatigue => fatigueTrue * (100f / maxFatigue);

		protected internal virtual sbyte bonusMinStrength { get; set; }
		protected virtual byte baseMinStrength => 0;
		public byte minStrength => baseMinStrength.offset(bonusMinStrength);



		protected internal virtual sbyte bonusMinToughness { get; set; }
		protected virtual byte baseMinToughness => 0;
		public byte minToughness => baseMinToughness.offset(bonusMinToughness);



		protected internal virtual sbyte bonusMinSpeed { get; set; }
		protected virtual byte baseMinSpeed => 0;
		public byte minSpeed => baseMinSpeed.offset(bonusMinSpeed);


		protected internal virtual sbyte bonusMinIntelligence { get; set; }
		protected virtual byte baseMinIntelligence => 0;
		public byte minIntelligence => baseMinIntelligence.offset(bonusMinIntelligence);

		protected internal virtual sbyte bonusMinFatigue { get; set; }
		protected virtual byte baseMinFatigue => 0;
		public byte minFatigue => baseMinFatigue.offset(bonusMinFatigue);




		//public byte minHunger => 0;

		public abstract uint maxHealth { get; }

		protected internal int perkBonusHealth { get; set; }

		protected internal virtual byte baseMaxStrength => BASE_MAX_STRENGTH;
		protected internal virtual sbyte bonusMaxStrength { get; set; } = 0;
		public byte maxStrength => HandleMaxStat(baseMaxStrength.offset(bonusMaxStrength), minStrength);

		protected internal virtual byte baseMaxToughness => BASE_MAX_TOUGHNESS;
		protected internal virtual sbyte bonusMaxToughness { get; set; } = 0;
		public byte maxToughness => HandleMaxStat(baseMaxToughness.offset(bonusMaxToughness), minToughness);

		protected internal virtual byte baseMaxSpeed => BASE_MAX_SPEED;
		protected internal virtual sbyte bonusMaxSpeed { get; set; } = 0;
		public byte maxSpeed => HandleMaxStat(baseMaxSpeed.offset(bonusMaxSpeed), minSpeed);

		protected internal virtual byte baseMaxIntelligence => BASE_MAX_INTELLIGENCE;
		protected internal virtual sbyte bonusMaxIntelligence { get; set; } = 0;
		public byte maxIntelligence => HandleMaxStat(baseMaxIntelligence.offset(bonusMaxIntelligence), minIntelligence);

		protected internal virtual byte baseMaxFatigue => BASE_MAX_FATIGUE;
		protected internal virtual sbyte bonusMaxFatigue { get; set; } = 0;
		public byte maxFatigue => HandleMaxStat(baseMaxFatigue.offset(bonusMaxFatigue), minFatigue);

		protected internal float StrengthGainMultiplier = 1.0f;
		protected internal float StrengthLossMultiplier = 1.0f;

		protected internal float ToughnessGainMultiplier = 1.0f;
		protected internal float ToughnessLossMultiplier = 1.0f;

		protected internal float SpeedGainMultiplier = 1.0f;
		protected internal float SpeedLossMultiplier = 1.0f;

		protected internal float IntelligenceGainMultiplier = 1.0f;
		protected internal float IntelligenceLossMultiplier = 1.0f;

		protected internal float FatigueRegenRate = 1.0f;

		//public virtual byte maxHunger => BASE_MAX_HUNGER.offset(modifiers.bonusMaxHunger);

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
		//	strength = strength.offset(str);
		//	toughness = toughness.offset(tou);
		//	speed = speed.offset(spd);
		//	intelligence = intelligence.offset(inte);
		//	libido = libido.offset(lib);
		//	sensitivity = sensitivity.offset(sens);
		//	corruption = corruption.offset(corr);
		//	lust = lust.offset(lus);
		//}

		public CombatCreature(CombatCreatureCreator creator) : base(creator)
		{
			//modifiers is valid already because perks is not null.
			//creator is not null or the base would have thrown.
			totalExperience = creator.initialXP;
			level = creator.initialLevel;

			strengthTrue = creator.strength ?? DEFAULT_STRENGTH;
			toughnessTrue = creator.toughness ?? DEFAULT_TOUGHNESS;
			speedTrue = creator.speed ?? DEFAULT_SPEED;
			intelligenceTrue = creator.intelligence ?? DEFAULT_INTELLIGENCE;

			fatigueTrue = DEFAULT_FATIGUE;
			//hunger = DEFAULT_HUNGER;

			currentHealth = maxHealth;
		}

		//public uint currentHealth
		//{
		//	get => _currentHealth;
		//	private protected set => _currentHealth = Utils.Clamp2(value, (uint)0, maxHealth);
		//}
		//private uint _currentHealth = 0;
		public float IncreaseStrengthBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = strengthTrue;
			float delta = percent * maxStrength;
			if (!ignorePerks)
			{
				delta *= StrengthGainMultiplier;
			}
			strengthTrue += delta;
			return strengthTrue - oldValue;
		}

		public float IncreaseStrength(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= StrengthGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = strengthTrue;
			strengthTrue += amount;
			return strengthTrue - oldValue;
		}

		public float IncreaseToughnessBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = toughnessTrue;
			float delta = percent * maxToughness;
			if (!ignorePerks)
			{
				delta *= ToughnessGainMultiplier;
			}
			toughnessTrue += delta;
			return toughnessTrue - oldValue;
		}

		public float IncreaseToughness(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= ToughnessGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = toughnessTrue;
			toughnessTrue += amount;
			return toughnessTrue - oldValue;
		}

		public float IncreaseSpeedBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = speedTrue;
			float delta = percent * maxSpeed;
			if (!ignorePerks)
			{
				delta *= SpeedGainMultiplier;
			}
			speedTrue += delta;
			return speedTrue - oldValue;
		}

		public float IncreaseSpeed(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= SpeedGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = speedTrue;
			speedTrue += amount;
			return speedTrue - oldValue;
		}

		public float IncreaseIntelligenceBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = intelligenceTrue;
			float delta = percent * maxIntelligence;
			if (!ignorePerks)
			{
				delta *= IntelligenceGainMultiplier;
			}
			intelligenceTrue += delta;
			return intelligenceTrue - oldValue;
		}

		public float IncreaseIntelligence(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= IntelligenceGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = intelligenceTrue;
			intelligenceTrue += amount;
			return intelligenceTrue - oldValue;
		}

		public float DecreaseStrengthBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = strengthTrue;
			float delta = percent * maxStrength;
			if (!ignorePerks)
			{
				delta *= StrengthLossMultiplier;
			}
			strengthTrue -= delta;
			return oldValue - strengthTrue;
		}

		public float DecreaseStrength(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= StrengthGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = strengthTrue;
			strengthTrue -= amount;
			return oldValue - strengthTrue;
		}

		public float DecreaseToughnessBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = toughnessTrue;
			float delta = percent * maxToughness;
			if (!ignorePerks)
			{
				delta *= ToughnessLossMultiplier;
			}
			toughnessTrue -= delta;
			return oldValue - toughnessTrue;
		}

		public float DecreaseToughness(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= ToughnessGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = toughnessTrue;
			toughnessTrue -= amount;
			return oldValue - toughnessTrue;
		}

		public float DecreaseSpeedBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = speedTrue;
			float delta = percent * maxSpeed;
			if (!ignorePerks)
			{
				delta *= SpeedLossMultiplier;
			}
			speedTrue -= delta;
			return oldValue - speedTrue;
		}

		public float DecreaseSpeed(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= SpeedGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = speedTrue;
			speedTrue -= amount;
			return oldValue - speedTrue;
		}

		public float DecreaseIntelligenceBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = intelligenceTrue;
			float delta = percent * maxIntelligence;
			if (!ignorePerks)
			{
				delta *= IntelligenceLossMultiplier;
			}
			intelligenceTrue -= delta;
			return oldValue - intelligenceTrue;
		}

		public float DecreaseIntelligence(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= IntelligenceGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = intelligenceTrue;
			intelligenceTrue -= amount;
			return oldValue - intelligenceTrue;
		}

		public float SetStrengthPercent(float percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			float value = maxStrength * percent;
			strengthTrue = value;
			return strengthTrue;
		}

		public float SetStrength(byte value)
		{
			strengthTrue = value;
			return strengthTrue;
		}
		public float SetToughnessPercent(float percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			float value = maxToughness * percent;
			toughnessTrue = value;
			return toughnessTrue;
		}

		public float SetToughness(byte value)
		{
			toughnessTrue = value;
			return toughnessTrue;
		}
		public float SetSpeedPercent(float percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			float value = maxSpeed * percent;
			speedTrue = value;
			return speedTrue;
		}

		public float SetSpeed(byte value)
		{
			speedTrue = value;
			return speedTrue;
		}
		public float SetIntelligencePercent(float percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			float value = maxIntelligence * percent;
			intelligenceTrue = value;
			return intelligenceTrue;
		}

		public float SetIntelligence(byte value)
		{
			intelligenceTrue = value;
			return intelligenceTrue;
		}

		public void IncreaseCombatCreatureStats(byte lus = 0, byte lib = 0, byte sens = 0, byte corr = 0, byte str = 0, byte tou = 0, byte spe = 0, byte inte = 0, bool ignorePerks = false)
		{
			IncreaseCreatureStats(lust, lib, sens, corr);

			float amount;
			if (str != 0)
			{
				amount = str;
				if (!ignorePerks) amount *= StrengthGainMultiplier;
				strengthTrue += amount;
			}
			if (tou != 0)
			{
				amount = tou;
				if (!ignorePerks) amount *= ToughnessGainMultiplier;
				toughnessTrue += amount;
			}
			if (spe != 0)
			{
				amount = spe;
				if (!ignorePerks) amount *= SpeedGainMultiplier;
				speedTrue += amount;
			}
			if (inte != 0)
			{
				amount = inte;
				if (!ignorePerks) amount *= IntelligenceGainMultiplier;
				intelligenceTrue += amount;
			}
		}

		public void DecreaseCombatCreatureStats(byte lus = 0, byte lib = 0, byte sens = 0, byte corr = 0, byte str = 0, byte tou = 0, byte spe = 0, byte inte = 0, bool ignorePerks = false)
		{
			DecreaseCreatureStats(lust, lib, sens, corr);

			float amount;
			if (str != 0)
			{
				amount = str;
				if (!ignorePerks) amount *= StrengthLossMultiplier;
				strengthTrue -= amount;
			}
			if (tou != 0)
			{
				amount = tou;
				if (!ignorePerks) amount *= ToughnessLossMultiplier;
				toughnessTrue -= amount;
			}
			if (spe != 0)
			{
				amount = spe;
				if (!ignorePerks) amount *= SpeedLossMultiplier;
				speedTrue -= amount;
			}
			if (inte != 0)
			{
				amount = inte;
				if (!ignorePerks) amount *= IntelligenceLossMultiplier;
				intelligenceTrue -= amount;
			}
		}

		public void SetCombatCreatureStats(byte? lus = null, byte? lib = null, byte? sens = null, byte? corr = null, byte? str = null, byte? tou = null, byte? spe = null, byte? inte = null)
		{
			SetCreatureStats(lust, lib, sens, corr);

			if (str is byte ltb)
			{
				strengthTrue = ltb;
			}
			if (tou is byte lbb)
			{
				toughnessTrue = lbb;
			}
			if (spe is byte sb)
			{
				speedTrue = sb;
			}
			if (inte is byte cb)
			{
				intelligenceTrue = cb;
			}
		}

		public void DeltaCombatCreatureStats(float lus = 0, float lib = 0, float sens = 0, float corr = 0, float str = 0, float tou = 0, float spe = 0, float inte = 0, bool ignorePerks = false)
		{
			DeltaCreatureStats(lust, lib, sens, corr);

			float amount;
			if (str < 0)
			{
				amount = str;
				if (!ignorePerks) amount *= StrengthLossMultiplier;
				strengthTrue += amount;
			}
			else if (str > 0)
			{
				amount = str;
				if (!ignorePerks) amount *= StrengthGainMultiplier;
				strengthTrue += amount;
			}
			if (tou < 0)
			{
				amount = tou;
				if (!ignorePerks) amount *= ToughnessLossMultiplier;
				toughnessTrue += amount;
			}
			else if (tou > 0)
			{
				amount = tou;
				if (!ignorePerks) amount *= ToughnessGainMultiplier;
				toughnessTrue += amount;
			}
			if (spe < 0)
			{
				amount = spe;
				if (!ignorePerks) amount *= SpeedLossMultiplier;
				speedTrue += amount;
			}
			else if (spe > 0)
			{
				amount = spe;
				if (!ignorePerks) amount *= SpeedGainMultiplier;
				speedTrue += amount;
			}
			if (inte < 0)
			{
				amount = inte;
				if (!ignorePerks) amount *= IntelligenceLossMultiplier;
				intelligenceTrue += amount;
			}
			else if (inte > 0)
			{
				amount = inte;
				if (!ignorePerks) amount *= IntelligenceGainMultiplier;
				intelligenceTrue += amount;
			}
		}

		public float GainFatigue(float amount, bool ignorePerks = false)
		{
			var oldValue = fatigueTrue;
			fatigueTrue += amount;
			return fatigueTrue - oldValue;
		}

		public float RecoverFatigue(float amount, bool ignorePerks = false)
		{
			var oldValue = fatigueTrue;
			if (!ignorePerks)
			{
				amount *= FatigueRegenRate;
			}
			fatigueTrue -= amount;
			return oldValue - fatigueTrue;
		}
		public float ChangeFatigue(short delta, bool ignorePerks = false)
		{
			bool lose = delta < 0;

			if (delta < 0)
			{
				delta *= -1;
			}
			Utils.Clamp(ref delta, byte.MinValue, byte.MaxValue);
			byte amount = (byte)delta;
			if (lose)
			{

				return RecoverFatigue(amount, ignorePerks);
			}
			else
			{
				return GainFatigue(amount, ignorePerks);
			}
		}
		public float spellCost(double baseCost)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}


		public float physicalCost(double baseCost)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		public bool hasEnoughStamina(double baseCost, bool isPhysical)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		//internal CombatCreature(SurrogateCombatCreator surrogateCreator) : base(surrogateCreator)
		//{

		//}
	}
}
