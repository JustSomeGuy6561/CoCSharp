//Cock.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:55 PM
using CoC.Creatures;
using CoC.Tools;
using  CoC.BodyParts.SpecialInteraction;
using static   CoC.BodyParts.CockNBallzStrings;
using static CoC.UI.TextOutput;
namespace  CoC.BodyParts
{
	public enum CockGroup { HUMAN, MAMMALIAN, CORRUPTED, AQUATIC, REPTILIAN, FLYING, OTHER }

	public enum CockPiercing
	{
		ALBERT,
		FRENUM_UPPER_1, FRENUM_UPPER_2, FRENUM_MIDDLE_1, FRENUM_MIDDLE_2,
		FRENUM_MIDDLE_3, FRENUM_MIDDLE_4, FRENUM_LOWER_1, FRENUM_LOWER_2
	}
	//whoever wrote the AS version of cock, thank you. your cock was awesome.
	//i didn't have to search everywhere for the things that were part of it.
	//well, mostly. knots were still a pain.
	public class Cock : PiercableBodyPart<Cock, CockType, CockPiercing>, IGrowShrinkable
	{
		public const float MAX_COCK_LENGTH = 240f;
		public const float MAX_COCK_THICKNESS = 50f;
		public const float MIN_COCK_THICKNESS = 0.2f;
		public const float MAX_KNOT_MULTIPLIER = 10f;
		public const float MIN_KNOT_MULTIPLIER = 1.1f;
		public const float MIN_COCK_LENGTH = 3.0f;

		public const float DEFAULT_COCK_LENGTH = 5.5f;
		public const float DEFAULT_BIG_COCK_LENGTH = 8f;
		public const float DEFAULT_COCK_GIRTH = 1.25f;
		public const float DEFAULT_BIG_COCK_GIRTH = 1.5f;


		public float cockArea => cockGirth * cockLength;
		public float knotMultiplier
		{
			get => _knotMultiplier;
			protected set
			{
				if (type.hasKnot)
				{
					Utils.Clamp(ref value, MIN_KNOT_MULTIPLIER, MAX_KNOT_MULTIPLIER);
					_knotMultiplier = value;
				}
				else _knotMultiplier = 0;
			}
		}
		private float _knotMultiplier;
		public float cockLength
		{
			get => _cockLength;
			protected set
			{
				_cockLength = value;
				UpdateCockLengthAndGirth(true);
			}
		}
		public float _cockLength;
		public float cockGirth
		{
			get => _cockGirth;
			protected set
			{
				_cockGirth = value;
				UpdateCockLengthAndGirth(false);
			}
		}
		private float _cockGirth;
		public float knotLength => cockGirth * knotMultiplier;

		public override CockType type
		{
			get => _type;
			protected set
			{
				if (_type != value && value.hasKnot != _type.hasKnot)
				{
					knotMultiplier = value.baseKnotMultiplier;
				}
				_type = value;
			}
		}
		private CockType _type = CockType.HUMAN;

		private readonly bool hasBigCockPerk;

		protected Cock(PiercingFlags flags, bool bigCockPerk) : base(flags)
		{	
			type = CockType.HUMAN;
			hasBigCockPerk = bigCockPerk;
			if (hasBigCockPerk)
			{
				_cockLength = DEFAULT_BIG_COCK_LENGTH;
				_cockGirth = DEFAULT_BIG_COCK_GIRTH;
			}
			else
			{
				_cockLength = DEFAULT_COCK_LENGTH;
				_cockGirth = DEFAULT_COCK_GIRTH;
			}
		}

		public static Cock Generate(CockType cockType, PiercingFlags piercingFlags, bool bigCockPerk)
		{
			return new Cock(piercingFlags, bigCockPerk)
			{
				type = cockType,
			};
		}

		public static Cock Generate(CockType cockType, PiercingFlags piercingFlags, bool bigCockPerk, float length, float girth)
		{
			return new Cock(piercingFlags, bigCockPerk)
			{
				type = cockType,
				cockLength = length,
				cockGirth = girth
			};
		}

		public override bool Restore()
		{
			if (type == CockType.HUMAN)
			{
				return false;
			}
			type = CockType.HUMAN;
			cockLength = DEFAULT_COCK_LENGTH;
			cockGirth = DEFAULT_COCK_GIRTH;
			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == CockType.HUMAN)
			{
				return false;
			}
			OutputText(restoreString(player));
			return Restore();
		}

		protected override bool PiercingLocationUnlocked(CockPiercing piercingLocation)
		{
			return true;
		}

		public bool CanReducto()
		{
			return cockArea > 6;
		}

		public float UseReducto()
		{
			if (!CanReducto())
			{
				return 0;
			}
			float oldLength = cockLength;
			cockLength *= 2.0f / 3;
			return oldLength - cockLength;

		}

		public bool CanGrowPlus()
		{
			return cockLength < MAX_COCK_LENGTH;
		}

		//grows cock 1-2 inches, in increments of 0.25. 
		//automatically increases cockGirth to min value.
		//if possible, will also increase cockGirth, up to 0.5 inches
		public float UseGroPlus()
		{
			if (!CanGrowPlus())
			{
				return 0;
			}
			float oldCockLength = cockLength;
			if (hasBigCockPerk)
			{
				cockLength += 1 + Utils.Rand(9) / 4.0f;
			}
			else
			{
				cockLength += 1 + Utils.Rand(4) / 4.0f;
			}
			if ((cockGirth + 0.5f) < maxGirth)
			{
				cockGirth += 0.5f;
			}
			else if (cockGirth < maxGirth)
			{
				cockGirth = maxGirth;
			}
			return cockLength - oldCockLength;
		}


		private void UpdateCockLengthAndGirth(bool length)
		{
			if (length)
			{
				Utils.Clamp(ref _cockLength, MIN_COCK_LENGTH, MAX_COCK_LENGTH);
				Utils.Clamp(ref _cockGirth, minGirth, maxGirth);
			}
			else
			{
				Utils.Clamp(ref _cockGirth, MIN_COCK_THICKNESS, MAX_COCK_THICKNESS);
				Utils.Clamp(ref _cockLength, minLength, maxLength);
			}
		}

		private float maxGirth => _cockLength * type.maxGirthToLengthRatio;
		private float minGirth => _cockLength * type.minGirthToLengthRatio;
		private float minLength => _cockGirth / type.maxGirthToLengthRatio;
		private float maxLength => _cockGirth / type.minGirthToLengthRatio;
	}



	public class CockType : PiercableBodyPartBehavior<CockType, Cock, CockPiercing>
	{

		private static int indexMaker = 0;
		public override int index => _index;
		private readonly int _index;

		public bool hasKnot => baseKnotMultiplier > 1.0f;
		public readonly float baseKnotMultiplier;

		//vanilla game limited cock width to 2/5 the length. i realize that some cocks may require that ratio be lower, so if it irks you, you
		//now have the option to change that.

		public virtual float maxGirthToLengthRatio => 0.4f;

		//similarly, you now have the option to mess with min ratio. by default, any cock will at the very least, have one inch in width per 10 inches in length.
		public virtual float minGirthToLengthRatio => 0.1f;


		protected CockType(CockGroup cockGroup,
			SimpleDescriptor shortDesc, DescriptorWithArg<Cock> fullDesc, TypeAndPlayerDelegate<Cock> playerDesc,
			ChangeType<Cock> transform, RestoreType<Cock> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			baseKnotMultiplier = 0;

		}

		protected CockType(CockGroup cockGroup, float initialKnotMultiplier, //any cocktype specific values.
			SimpleDescriptor shortDesc, DescriptorWithArg<Cock> fullDesc, TypeAndPlayerDelegate<Cock> playerDesc,
			ChangeType<Cock> transform, RestoreType<Cock> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			Utils.Clamp(ref initialKnotMultiplier, 1.1f, 2.5f);
			baseKnotMultiplier = initialKnotMultiplier;
		}

		public static readonly CockType HUMAN = new CockType(CockGroup.HUMAN, HumanDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly CockType HORSE = new CockType(CockGroup.MAMMALIAN, HorseDesc, HorseFullDesc, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly CockType DOG = new CockType(CockGroup.MAMMALIAN, 1.1f, DogDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);// can range up to 2.1 depending on item.
		public static readonly CockType DEMON = new CockType(CockGroup.CORRUPTED, DemonDesc, DemonFullDesc, DemonPlayerStr, DemonTransformStr, DemonRestoreStr);
		public static readonly CockType TENTACLE = new CockType(CockGroup.CORRUPTED, TentacleDesc, TentacleFullDesc, TentaclePlayerStr, TentacleTransformStr, TentacleRestoreStr);
		public static readonly CockType CAT = new CockType(CockGroup.MAMMALIAN, CatDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly CockType LIZARD = new CockType(CockGroup.REPTILIAN, LizardDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly CockType ANEMONE = new CockType(CockGroup.AQUATIC, AnemoneDesc, AnemoneFullDesc, AnemonePlayerStr, AnemoneTransformStr, AnemoneRestoreStr);
		public static readonly CockType KANGAROO = new CockType(CockGroup.MAMMALIAN, KangarooDesc, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly CockType DRAGON = new CockType(CockGroup.REPTILIAN, 1.3f, DragonDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly CockType DISPLACER = new CockType(CockGroup.OTHER, 1.5f, DisplacerDesc, DisplacerFullDesc, DisplacerPlayerStr, DisplacerTransformStr, DisplacerRestoreStr);
		public static readonly CockType FOX = new CockType(CockGroup.MAMMALIAN, 1.25f, FoxDesc, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		public static readonly CockType BEE = new CockType(CockGroup.FLYING, BeeDesc, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		public static readonly CockType PIG = new CockType(CockGroup.MAMMALIAN, PigDesc, PigFullDesc, PigPlayerStr, PigTransformStr, PigRestoreStr);
		public static readonly CockType AVIAN = new CockType(CockGroup.FLYING, AvianDesc, AvianFullDesc, AvianPlayerStr, AvianTransformStr, AvianRestoreStr);
		public static readonly CockType RHINO = new CockType(CockGroup.MAMMALIAN, RhinoDesc, RhinoFullDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr);
		public static readonly CockType ECHIDNA = new CockType(CockGroup.MAMMALIAN, EchidnaDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly CockType WOLF = new CockType(CockGroup.MAMMALIAN, 1.5f, WolfDesc, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly CockType RED_PANDA = new CockType(CockGroup.MAMMALIAN, RedPandaDesc, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);
		public static readonly CockType FERRET = new CockType(CockGroup.MAMMALIAN, FerretDesc, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
	}
}
