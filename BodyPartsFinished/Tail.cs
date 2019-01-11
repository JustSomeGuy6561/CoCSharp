//Tail.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:26 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;
using CoC.Engine;
using CoC.BodyParts.SpecialInteraction;
using CoC.Strings;
using static CoC.Strings.BodyParts.TailStrings;

namespace CoC.BodyParts
{
	public enum TailPiercings { SUCCUBUS_SPADE }
	public class Tail : PiercableBodyPart<Tail, TailType, TailPiercings>, ITimeAware, ICanAttackWith<Tail>
	{
		public const int MAX_ATTACK_CHARGES = 100;
		public int tailCount
		{
			get => _tailCount;
			protected set
			{
				Utils.Clamp(ref value, type.initialTailCount, type.maxTailCount);
				_tailCount = value;
			}
		}
		private int _tailCount = 0;
		//how many attack charges you regain per hour.
		public int rechargeRate
		{
			get => _rechargeRate;
			protected set
			{
				Utils.Clamp(ref value, type.initialChargeRate, type.maxChargeRate);
				_rechargeRate = value;
			}
		}
		private int _rechargeRate;
		public int attackCharges
		{
			get => _attackCharges;
			protected set
			{
				Utils.Clamp(ref value, 0, MAX_ATTACK_CHARGES);
				_attackCharges = value;
			}
		}
		private int _attackCharges = 0;
		protected Tail(PiercingFlags flags) : base(flags)
		{
			type = TailType.NONE;
		}



		public override TailType type
		{
			get => _type;
			protected set
			{
				if (_type != value)
				{
					tailCount = value.initialTailCount;
					attackCharges = value.initialResourceCount;
					rechargeRate = value.initialChargeRate;
				}
				_type = value;
			}
		}

		public static Tail GenerateNoTail(PiercingFlags flags)
		{
			return new Tail(flags);
		}

		public static Tail GenerateTail(PiercingFlags flags, TailType tailType)
		{
			return new Tail(flags)
			{
				type = tailType
			};
		}

		public bool AdjustSpecialResourceRateAndCount(int changeInResource, int changeInRate = 0)
		{
			if (type.hasSpecialAttack && type.specialAttackRequiresResources)
			{
				int oldRate = rechargeRate;
				int oldCharges = attackCharges;
				rechargeRate += changeInRate;
				attackCharges += changeInResource;
				return oldCharges != rechargeRate || oldCharges != attackCharges;
			}
			return false;
		}

		private bool GrowAdditionalTail()
		{
			if (!type.hasMultipleTails || tailCount >= type.maxTailCount)
			{
				return false;
			}
			tailCount++;
			return true;
		}

		public GenericDescription attackName
		{
			get
			{
				if (type.canTailWhip) return TailWhipName;
				else return () => GlobalStrings.CantAttackName(this);
			}
		}

		public TypeAndPlayerDelegate<Tail> attackHint
		{
			get
			{
				if (type.canTailWhip) return TailWhipHint;
				else return GlobalStrings.CantAttackWith<Tail, TailType>;
			}
		}

		public GenericDescription specialAttackName => type.specialAttackName;

		public TypeAndPlayerDelegate<Tail> specialAttackHint => type.specialAttackHint;

		private TailType _type;

		public override bool Restore()
		{
			if (type == TailType.NONE)
			{
				return false;
			}
			type = TailType.NONE; //type resets everything.
			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == TailType.NONE)
			{
				return false;
			}
			restoreString(this, player);
			return Restore();
		}

		protected override bool PiercingLocationUnlocked(TailPiercings piercingLocation)
		{
			return type.supportsTailPiercing && piercingFlags.piercingFetishEnabled;
		}

		//may want to see if there's a way to wrap setting this class so it can be removed from
		//the list of time aware shit when not in use. idk how much it'll cost.
		public void ReactToTimePassing(uint hoursPassed)
		{
			int val = attackCharges + rechargeRate * (int)hoursPassed;
			Utils.Clamp(ref val, 0, MAX_ATTACK_CHARGES);
			attackCharges = val;
		}

		public bool canAttackWith()
		{
			return type.canAttackWith;
		}

		public bool hasSpecialAttack()
		{
			return type.hasSpecialAttack;
		}

		public bool hasStandardAttackForThisPart()
		{
			return type.canTailWhip;
		}
	}

	public class TailType : PiercableBodyPartBehavior<TailType, Tail, TailPiercings>
	{
		public static int indexMaker = 0;

		public GenericDescription tailWhipAttackName;
		public GenericDescription tailWhipAttackHint;


		private readonly int _index;
		//holy shit, fuck tails!
		public bool canAttackWith => canTailWhip || hasSpecialAttack;
		public bool hasMultipleTails => maxTailCount > 1;
		public readonly bool supportsTailPiercing;

		public readonly bool canTailWhip;
		public readonly bool specialAttackRequiresResources;
		public readonly bool hasSpecialAttack;

		public readonly int initialResourceCount;
		public readonly int initialChargeRate;
		public readonly int maxChargeRate;
		public readonly int resourcesUsedPerAttack;

		public readonly GenericDescription specialAttackName;
		public readonly TypeAndPlayerDelegate<Tail> specialAttackHint;

		public readonly int initialTailCount;
		public readonly int maxTailCount;

		//tail constructor that uses venom
		protected TailType(bool tailWhip, int defaultVenom, int initialChargeSpeed, int maxChargeSpeed, int amountUsedPerAttack, GenericDescription attackNme, TypeAndPlayerDelegate<Tail> attackHnt,
			GenericDescription shortDesc, FullDescription<Tail> fullDesc, PlayerDescription<Tail> playerDesc, ChangeType<Tail> transform,
			ChangeType<Tail> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			supportsTailPiercing = false;

			specialAttackRequiresResources = true;
			hasSpecialAttack = true;
			initialResourceCount = defaultVenom;
			initialChargeRate = initialChargeSpeed;
			maxChargeRate = maxChargeSpeed;
			resourcesUsedPerAttack = amountUsedPerAttack;
			specialAttackName = attackNme;
			specialAttackHint = attackHnt;
			initialTailCount = 1;
			maxTailCount = 1;
			canTailWhip = tailWhip;
		}

		//a tail that you can special attack with, but doesn't require any tail resources. may still cost fatigue.
		protected TailType(bool tailWhip, GenericDescription attackNme, TypeAndPlayerDelegate<Tail> attackHnt,
			GenericDescription shortDesc, FullDescription<Tail> fullDesc, PlayerDescription<Tail> playerDesc, ChangeType<Tail> transform,
			ChangeType<Tail> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			supportsTailPiercing = false;

			specialAttackRequiresResources = false;
			hasSpecialAttack = true;
			initialResourceCount = 0;
			initialChargeRate = 0;
			maxChargeRate = 0;
			resourcesUsedPerAttack = 0;
			specialAttackName = attackNme;
			specialAttackHint = attackHnt;
			initialTailCount = 1;
			maxTailCount = 1;
			canTailWhip = tailWhip;
		}

		//multi-tail constructor - no attack. create a new one if you need that shit.
		protected TailType(uint initialCount, uint maxCount,
			GenericDescription shortDesc, FullDescription<Tail> fullDesc, PlayerDescription<Tail> playerDesc, ChangeType<Tail> transform,
			ChangeType<Tail> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			supportsTailPiercing = false;

			hasSpecialAttack = false;
			specialAttackRequiresResources = false;
			initialResourceCount = 0;
			initialChargeRate = 0;
			maxChargeRate = 0;
			resourcesUsedPerAttack = 0;
			specialAttackName = () => GlobalStrings.CantAttackName(this);
			specialAttackHint = GlobalStrings.CantAttackWith<Tail, TailType>;
			Utils.Clamp<uint>(ref initialCount, 1, int.MaxValue);
			Utils.Clamp<uint>(ref maxCount, initialCount, int.MaxValue);
			initialTailCount = (int)initialCount;
			maxTailCount = (int)maxCount;
			canTailWhip = false;
		}

		//constructor for a single delicate tail.
		protected TailType(bool canTailWhip, bool canPierce,
			GenericDescription shortDesc, FullDescription<Tail> fullDesc, PlayerDescription<Tail> playerDesc, ChangeType<Tail> transform,
			ChangeType<Tail> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			supportsTailPiercing = canPierce;

			hasSpecialAttack = false;
			specialAttackRequiresResources = false;
			initialResourceCount = 0;
			initialChargeRate = 0;
			maxChargeRate = 0;
			resourcesUsedPerAttack = 0;
			specialAttackName = () => GlobalStrings.CantAttackName(this);
			specialAttackHint = GlobalStrings.CantAttackWith<Tail, TailType>;
			initialTailCount = 1;
			maxTailCount = 1;
			this.canTailWhip = canTailWhip;
		}

		//Constructor for no tail.
		protected TailType(
			GenericDescription shortDesc, FullDescription<Tail> fullDesc, PlayerDescription<Tail> playerDesc, ChangeType<Tail> transform,
			ChangeType<Tail> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			supportsTailPiercing = false;

			hasSpecialAttack = false;
			specialAttackRequiresResources = false;
			initialResourceCount = 0;
			initialChargeRate = 0;
			maxChargeRate = 0;
			resourcesUsedPerAttack = 0;
			specialAttackName = () => GlobalStrings.CantAttackName(this);
			specialAttackHint = GlobalStrings.CantAttackWith<Tail, TailType>;
			initialTailCount = 0;
			maxTailCount = 0;
			canTailWhip = false;
		}

		public override int index => _index;

		public static readonly TailType NONE = new TailType(NoTailShortDesc, NoTailFullDesc, NoTailPlayerStr, NoTailTransformStr, NoTailRestoreStr);
		public static readonly TailType HORSE = new TailType(false, false, HorseShortDesc, HorseFullDesc, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly TailType DOG = new TailType(false, false, DogShortDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly TailType DEMONIC = new TailType(false, true, DemonShortDesc, DemonFullDesc, DemonPlayerStr, DemonTransformStr, DemonRestoreStr);
		public static readonly TailType COW = new TailType(false, false, CowShortDesc, CowFullDesc, CowPlayerStr, CowTransformStr, CowRestoreStr);
		public static readonly TailType SPIDER_ABDOMEN = new TailType(false, 5, 5, 25, 33, SpecialWebName, SpecialWebHint, SpiderShortDesc, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr); //web
		public static readonly TailType BEE_ABDOMEN = new TailType(false, 10, 2, 15, 25, SpecialStingName, SpecialStingHint, BeeShortDesc, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr); //sting
		public static readonly TailType SHARK = new TailType(true, SpecialSlamName, SpecialSlamHint, SharkShortDesc, SharkFullDesc, SharkPlayerStr, SharkTransformStr, SharkRestoreStr); //slam
		public static readonly TailType CAT = new TailType(false, false, CatShortDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly TailType LIZARD = new TailType(true, false, LizardShortDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr); //whip
		public static readonly TailType RABBIT = new TailType(false, false, RabbitShortDesc, RabbitFullDesc, RabbitPlayerStr, RabbitTransformStr, RabbitRestoreStr);
		public static readonly TailType HARPY = new TailType(false, false, HarpyShortDesc, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly TailType KANGAROO = new TailType(true, false, KangarooShortDesc, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr); //whip
		public static readonly TailType FOX = new TailType(true, false, FoxShortDesc, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr); //whip. not in original, but you know a kitsune would do that shit.
		public static readonly TailType DRACONIC = new TailType(false, SpecialDraconicSlamName, SpecialDraconicSlamHint, DragonShortDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr); //slam (dragon)
		public static readonly TailType RACCOON = new TailType(true, false, RaccoonShortDesc, RaccoonFullDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr); //whip
		public static readonly TailType MOUSE = new TailType(false, false, MouseShortDesc, MouseFullDesc, MousePlayerStr, MouseTransformStr, MouseRestoreStr);
		public static readonly TailType FERRET = new TailType(true, false, FerretShortDesc, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr); //whip
		public static readonly TailType BEHEMOTH = new TailType(false, SpecialSlamName, SpecialSlamHint, BehemothShortDesc, BehemothFullDesc, BehemothPlayerStr, BehemothTransformStr, BehemothRestoreStr); //slam
		public static readonly TailType PIG = new TailType(false, false, PigShortDesc, PigFullDesc, PigPlayerStr, PigTransformStr, PigRestoreStr);
		public static readonly TailType SCORPION = new TailType(false, 5, 1, 5, 10, ScorpionStingName, ScorpionStingHint, ScorpionShortDesc, ScorpionFullDesc, ScorpionPlayerStr, ScorpionTransformStr, ScorpionRestoreStr); //sting
		public static readonly TailType GOAT = new TailType(false, false, GoatShortDesc, GoatFullDesc, GoatPlayerStr, GoatTransformStr, GoatRestoreStr);
		public static readonly TailType RHINO = new TailType(false, false, RhinoShortDesc, RhinoFullDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr);
		public static readonly TailType ECHIDNA = new TailType(false, false, EchidnaShortDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly TailType DEER = new TailType(false, false, DeerShortDesc, DeerFullDesc, DeerPlayerStr, DeerTransformStr, DeerRestoreStr);
		public static readonly TailType SALAMANDER = new TailType(true, SalamanderSlapName, SalamanderSlapHint, SalamanderShortDesc, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr); //whip, also slap
		public static readonly TailType WOLF = new TailType(false, false, WolfShortDesc, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly TailType SHEEP = new TailType(false, false, SheepShortDesc, SheepFullDesc, SheepPlayerStr, SheepTransformStr, SheepRestoreStr);
		public static readonly TailType IMP = new TailType(false, false, ImpShortDesc, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly TailType COCKATRICE = new TailType(false, false, CockatriceShortDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly TailType RED_PANDA = new TailType(false, false, RedPandaShortDesc, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);
	}
}
