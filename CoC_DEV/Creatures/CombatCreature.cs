//CombatCreature.cs
//Description:
//Author: JustSomeGuy
//1/22/2019, 1:26 AM
using CoC.Tools;

namespace CoC.Creatures
{
	//class defining combat traits for creatures that do combat. 
	public abstract class CombatCreature : Creature
	{
		protected CombatCreature(string creatureName, Species startingRace) : base(creatureName, startingRace)
		{
			CombatCreatureCreator ccc = combatCreator;

			level = ccc?.level ?? 1;
			experience = ccc?.experience ?? 0;
			strength = ccc?.strength ?? 15;
			toughness = ccc?.toughness ?? 15;
			speed = ccc?.speed ?? 15;
			intelligence = ccc?.intelligence ?? 15;
			corruption = ccc?.corruption ?? 15;
			hp = ccc?.hp ?? ;
			lust = ccc?.lust ?? 0;
			fatigue = ccc?.fatigue ?? 0;
			satiety = ccc?.satiety ?? 50;
			clothing = Clothing.Generate(ccc?.armor, ccc?.upperGarment, ccc?.lowerGarment);
			weapon = ccc?.weapon ?? Weapon.GenerateEmpty();
			shield = ccc?.shield ?? Shield.GenerateEmpty();
			accessories = Accessories.Generate(ccc?.hairAccessory, ccc?.neckAccessory, ccc?.armAccessory);
		}
		
		//these are clamped automatically from 0 to max Value. Note, however, that if you override the max value,
		//and that max value is not constant, this may lead to an edge case where if the value is at its max
		//then the max is decreased - the value itself will not decrease.
		//The solution is to update the value when the max decreases, but that is up to the implemented class to do, not here.s
		public virtual int level
		{
			get => _level;
			protected set
			{
				Utils.Clamp(ref value, 0, Constants.LEVEL_CAP);
				_level = value;
			}
		}
		private int _level;
		public virtual float experience
		{
			get => _experience;
			protected set
			{
				Utils.Clamp(ref value, 0, Constants.EXPERIENCE_CAP);
				_experience = value;
			}
		}
		private float _experience;
		public virtual float strength
		{
			get => _strength;
			protected set
			{
				Utils.Clamp(ref value, 0, maxStrength);
				_strength = value;
			}
		}
		private float _strength;
		public virtual float toughness
		{
			get => _toughness;
			protected set
			{
				Utils.Clamp(ref value, 0, maxToughness);
				_toughness = value;
			}
		}
		private float _toughness;
		public virtual float speed
		{
			get => _speed;
			protected set
			{
				Utils.Clamp(ref value, 0, maxSpeed);
				_speed = value;
			}
		}
		private float _speed;
		public virtual float intelligence
		{
			get => _intelligence;
			protected set
			{
				Utils.Clamp(ref value, 0, maxIntelligence);
				_intelligence = value;
			}
		}
		private float _intelligence;
		public virtual float corruption
		{
			get => _corruption;
			protected set
			{
				Utils.Clamp(ref value, 0, maxCorruption);
				_corruption = value;
			}
		}
		private float _corruption;
		public virtual int hp
		{
			get => _hp;
			protected set
			{
				Utils.Clamp(ref value, 0, maxHP);
				_hp = value;
			}
		}
		private int _hp;

		public virtual float lust
		{
			get => _lust;
			protected set
			{
				Utils.Clamp(ref value, 0, maxLust);
				_lust = value;
			}
		}
		private float _lust;
		public virtual float fatigue
		{
			get => _fatigue;
			protected set
			{
				Utils.Clamp(ref value, 0, maxFatigue);
				_fatigue = value;
			}
		}
		private float _fatigue;
		public virtual float satiety
		{
			get => _satiety;
			protected set
			{
				Utils.Clamp(ref value, 0, maxSatiety);
				_satiety = value;
			}
		}
		private float _satiety;

		//these will always generate the correct value relative to 100,
		//though i suppose you could break it if you actively tried (i.e. setting max to 0). 
		//regardless, they are not virtual.
		public float percentStrength100 => strength / maxStrength * 100;
		public float percentToughness100 => toughness / maxToughness * 100;
		public float percentSpeed100 => speed / maxSpeed * 100;
		public float percentIntelligence100 => intelligence / maxIntelligence * 100;
		public float percentCorruption100 => corruption / maxCorruption * 100;
		public float percentHP100 => hp * 1.0f / maxHP * 100;
		public float percentLust100 => lust / maxLust * 100;
		public float percentFatigue100 => fatigue / maxFatigue * 100;
		public float percentSatiety100 => satiety / maxSatiety * 100;

		public int baseHP;
		public abstract int maxHP { get; } //=> 50 + 2 * toughness + 15 * level;



		//max values. default to 100 for everything that does so. 
		//i expect them to be overridden if character has a perk to change these or is a boss.
		public virtual float maxStrength => 100;
		public virtual float maxToughness => 100;
		public virtual float maxSpeed => 100;
		public virtual float maxIntelligence => 100;
		public virtual float maxCorruption => 100;
		public virtual float maxLust => 100;
		public virtual float maxFatigue => 100;
		public virtual float maxSatiety => 100;

		public virtual int gems
		{
			get => _gems;
			set
			{
				Utils.Clamp(ref value, 0, Engine.maxGems);
				_gems = value;
			}
		}
		private int _gems;

		public readonly Clothing clothing;
		public readonly WeaponSlot weapon;
		public readonly ShieldSlot shield;
		public readonly Accessories accessories;

		public override GenericCreatureCreator creator => combatCreator;
		public abstract CombatCreatureCreator combatCreator { get; }
	}
}
