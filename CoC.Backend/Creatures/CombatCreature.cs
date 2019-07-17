//CombatCreature.cs
//Description:
//Author: JustSomeGuy
//2/20/2019, 4:14 PM
using CoC.Backend.Tools;

namespace CoC.Backend.Creatures
{

	public abstract class CombatCreature : Creature
	{
#warning Consider further clamping max/min values to prevent unacceptable min or max values (for example, HP is capped at 9999 in game)

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
		//internal const byte BASE_MAX_HUNGER = 100;



		public byte level { get; private protected set; } = 1;

		public uint experience { get; private protected set; } = 0;


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

		public virtual byte minStrength => modifiers.minStrength;
		public virtual byte minToughness => modifiers.minToughness;
		public virtual byte minSpeed => modifiers.minSpeed;
		public virtual byte minIntelligence => modifiers.minIntelligence;
		public virtual byte minLibido => modifiers.minLibido;
		public virtual byte minSensitivity => modifiers.minSensitivity;
		public virtual byte minCorruption => modifiers.minCorruption;
		public virtual byte minLust => modifiers.minLust;

		public byte minFatigue => 0;
		//public byte minHunger => 0;

		public abstract uint maxHealth { get; }
		public virtual byte maxStrength => BASE_MAX_STRENGTH.delta(modifiers.bonusMaxStrength);
		public virtual byte maxToughness => BASE_MAX_TOUGHNESS.delta(modifiers.bonusMaxToughness);
		public virtual byte maxSpeed => BASE_MAX_SPEED.delta(modifiers.bonusMaxSpeed);
		public virtual byte maxIntelligence => BASE_MAX_INTELLIGENCE.delta(modifiers.bonusMaxIntelligence);
		public virtual byte maxLibido => BASE_MAX_LIBIDO.delta(modifiers.bonusMaxLibido);
		public virtual byte maxSensitivity => BASE_MAX_SENSITIVITY.delta(modifiers.bonusMaxSensitivity);
		public virtual byte maxCorruption => BASE_MAX_CORRUPTION.delta(modifiers.bonusMaxCorruption);
		public virtual byte maxLust => BASE_MAX_LUST.delta(modifiers.bonusMaxLust);
		public virtual byte maxFatigue => BASE_MAX_FATIGUE.delta(modifiers.bonusMaxFatigue);

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

		public CombatCreature(CombatCreatureCreator creator) : base(creator)
		{
			//modifiers is valid already because perks is not null.
			//creator is not null or the base would have thrown.
			experience = creator.initialXP;
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

#warning reset this once debugging is done.
			//currentHealth = maxHealth;
			_currentHealth = 1; //
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
