//Tail.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:26 AM
using CoC.BodyParts.SpecialInteraction;
using CoC.Creatures;
using CoC.Engine.Combat.Attacks;
using CoC.EpidermalColors;
using CoC.Tools;
using static CoC.UI.TextOutput;

namespace CoC.BodyParts
{
	public enum TailPiercings { SUCCUBUS_SPADE }
	internal class Tail : PiercableBodyPart<Tail, TailType, TailPiercings>, ICanAttackWith, IFurAware, IToneAware
	{
		//these are here. i haven't done any restrictions on them, which would be implemented in the type.
		//updating and reading these would depend on the type. as of now, these only update if the body is using it.
		//this means that you'll always have a valid fur color even if the pc looses their fur. same with tone.
		//undertone, however, may be not_applicable. 
#warning consider adding an epidermis to this shit.
		public readonly FurColor furColor;
		public Tones tone { get; protected set; }
		public Tones underTone { get; protected set; }

#warning add tail update and update with message functions
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

		protected Tail(PiercingFlags flags) : base(flags)
		{
			type = TailType.NONE;
			furColor = FurColor.GenerateEmpty();
			tone = Tones.NOT_APPLICABLE;
			underTone = Tones.NOT_APPLICABLE;
		}



		public override TailType type
		{
			get => _type;
			protected set
			{
				if (_type != value)
				{
					tailCount = value.initialTailCount;
				}
				_type = value;
			}
		}
		private TailType _type;


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

		private bool GrowAdditionalTail()
		{
			if (!type.hasMultipleTails || tailCount >= type.maxTailCount)
			{
				return false;
			}
			tailCount++;
			return true;
		}



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
			OutputText(restoreString(player));
			return Restore();
		}

		protected override bool PiercingLocationUnlocked(TailPiercings piercingLocation)
		{
			return type.supportsTailPiercing && piercingFlags.piercingFetishEnabled;
		}

		public bool canAttackWith()
		{
			return type.canAttackWith;
		}

		public void reactToChangeInFurColor(object sender, FurAwareEventArg e)
		{
			furColor.UpdateFurColor(e.primaryColor);
		}

		public void reactToChangeInSkinTone(object sender, ToneAwareEventArg e)
		{
			if (e.primaryToneActive || tone == Tones.NOT_APPLICABLE)
			{
				tone = e.primaryTone;
			}
			if (e.secondaryTone != Tones.NOT_APPLICABLE)
			{
				underTone = e.secondaryTone;
			}
		}

		public AttackBase attack => type.attack;

	}

	internal partial class TailType : PiercableBodyPartBehavior<TailType, Tail, TailPiercings>
	{
		public static int indexMaker = 0;

		public SimpleDescriptor tailWhipAttackName;
		public SimpleDescriptor tailWhipAttackHint;

		public readonly AttackBase attack;

		private readonly int _index;
		//holy shit, fuck tails!
		public bool canAttackWith => attack != AttackBase.NO_ATTACK;
		public bool hasMultipleTails => maxTailCount > 1;
		public readonly bool supportsTailPiercing;


		public readonly int initialTailCount;
		public readonly int maxTailCount;

		//tail constructor that uses venom
		protected TailType(
			SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc, TypeAndPlayerDelegate<Tail> playerDesc, ChangeType<Tail> transform,
			RestoreType<Tail> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			supportsTailPiercing = false;
			initialTailCount = 1;
			maxTailCount = 1;
			attack = AttackBase.NO_ATTACK;
		}

		//a tail that you can special attack with
		protected TailType(AttackBase attackData,
			SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc, TypeAndPlayerDelegate<Tail> playerDesc, ChangeType<Tail> transform,
			RestoreType<Tail> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			supportsTailPiercing = false;

			initialTailCount = 1;
			maxTailCount = 1;
			attack = attackData;
		}

		//multi-tail constructor - no attack. create a new one if you need that shit.
		protected TailType(uint initialCount, uint maxCount,
			SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc, TypeAndPlayerDelegate<Tail> playerDesc, ChangeType<Tail> transform,
			RestoreType<Tail> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			supportsTailPiercing = false;

			Utils.Clamp<uint>(ref initialCount, 1, int.MaxValue);
			Utils.Clamp<uint>(ref maxCount, initialCount, int.MaxValue);
			initialTailCount = (int)initialCount;
			maxTailCount = (int)maxCount;
			attack = AttackBase.NO_ATTACK;
		}

		//constructor for a single delicate tail.
		protected TailType(bool canPierce,
			SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc, TypeAndPlayerDelegate<Tail> playerDesc, ChangeType<Tail> transform,
			RestoreType<Tail> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			supportsTailPiercing = canPierce;

			initialTailCount = 1;
			maxTailCount = 1;
			attack = AttackBase.NO_ATTACK;
		}

		public override int index => _index;

		public static readonly TailType NONE = new TailType(NoTailShortDesc, NoTailFullDesc, NoTailPlayerStr, NoTailTransformStr, NoTailRestoreStr);
		public static readonly TailType HORSE = new TailType(false, HorseShortDesc, HorseFullDesc, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly TailType DOG = new TailType(false, DogShortDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly TailType DEMONIC = new TailType(true, DemonShortDesc, DemonFullDesc, DemonPlayerStr, DemonTransformStr, DemonRestoreStr);
		public static readonly TailType COW = new TailType(false, CowShortDesc, CowFullDesc, CowPlayerStr, CowTransformStr, CowRestoreStr);
		public static readonly TailType SPIDER_ABDOMEN = new TailType(TailAttack.WEB, SpiderShortDesc, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr); //web
		public static readonly TailType BEE_ABDOMEN = new TailType(TailAttack.BEE_STING, BeeShortDesc, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr); //sting
		public static readonly TailType SHARK = new TailType(TailAttack.SLAM, SharkShortDesc, SharkFullDesc, SharkPlayerStr, SharkTransformStr, SharkRestoreStr); //slam
		public static readonly TailType CAT = new TailType(false, CatShortDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly TailType LIZARD = new TailType(TailAttack.SLAP, LizardShortDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr); //whip
		public static readonly TailType RABBIT = new TailType(false, RabbitShortDesc, RabbitFullDesc, RabbitPlayerStr, RabbitTransformStr, RabbitRestoreStr);
		public static readonly TailType HARPY = new TailType(false, HarpyShortDesc, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly TailType KANGAROO = new TailType(TailAttack.SLAP, KangarooShortDesc, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr); //whip
		public static readonly TailType FOX = new TailType(TailAttack.SLAP, FoxShortDesc, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr); //whip. not in original, but you know a kitsune would do that shit.
		public static readonly TailType DRACONIC = new TailType(TailAttack.DRAGON_SLAM, DragonShortDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr); //slam (dragon)
		public static readonly TailType RACCOON = new TailType(TailAttack.COON_SLAP, RaccoonShortDesc, RaccoonFullDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr); //whip
		public static readonly TailType MOUSE = new TailType(false, MouseShortDesc, MouseFullDesc, MousePlayerStr, MouseTransformStr, MouseRestoreStr);
		public static readonly TailType FERRET = new TailType(TailAttack.SLAP, FerretShortDesc, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr); //whip
		public static readonly TailType BEHEMOTH = new TailType(TailAttack.SLAM, BehemothShortDesc, BehemothFullDesc, BehemothPlayerStr, BehemothTransformStr, BehemothRestoreStr); //slam
		public static readonly TailType PIG = new TailType(false, PigShortDesc, PigFullDesc, PigPlayerStr, PigTransformStr, PigRestoreStr);
		public static readonly TailType SCORPION = new TailType(TailAttack.SCORPION_STING, ScorpionShortDesc, ScorpionFullDesc, ScorpionPlayerStr, ScorpionTransformStr, ScorpionRestoreStr); //sting
		public static readonly TailType GOAT = new TailType(false, GoatShortDesc, GoatFullDesc, GoatPlayerStr, GoatTransformStr, GoatRestoreStr);
		public static readonly TailType RHINO = new TailType(false, RhinoShortDesc, RhinoFullDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr);
		public static readonly TailType ECHIDNA = new TailType(false, EchidnaShortDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly TailType DEER = new TailType(false, DeerShortDesc, DeerFullDesc, DeerPlayerStr, DeerTransformStr, DeerRestoreStr);
		public static readonly TailType SALAMANDER = new TailType(TailAttack.SALAMANDER_WHIP, SalamanderShortDesc, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr); //whip, also slap
		public static readonly TailType WOLF = new TailType(false, WolfShortDesc, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly TailType SHEEP = new TailType(false, SheepShortDesc, SheepFullDesc, SheepPlayerStr, SheepTransformStr, SheepRestoreStr);
		public static readonly TailType IMP = new TailType(false, ImpShortDesc, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly TailType COCKATRICE = new TailType(false, CockatriceShortDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly TailType RED_PANDA = new TailType(false, RedPandaShortDesc, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);
	}
}
