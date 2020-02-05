//LowerBody.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 10:09 PM

using CoC.Backend.Attacks;
using CoC.Backend.Attacks.BodyPartAttacks;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CoC.Backend.BodyParts
{
	public sealed partial class LowerBodyTattooLocation : TattooLocation
	{

		private static readonly List<LowerBodyTattooLocation> _allLocations = new List<LowerBodyTattooLocation>();

		public static readonly ReadOnlyCollection<LowerBodyTattooLocation> allLocations;

		private readonly byte index;

		static LowerBodyTattooLocation()
		{
			allLocations = new ReadOnlyCollection<LowerBodyTattooLocation>(_allLocations);
		}

		private LowerBodyTattooLocation(byte index, TattooSizeLimit limitSize, SimpleDescriptor btnText, SimpleDescriptor locationDesc) : base(limitSize, btnText, locationDesc)
		{
			this.index = index;
		}

		public static LowerBodyTattooLocation LEFT_FOOT = new LowerBodyTattooLocation(0, SmallTattoosOnly, LeftFootButton, LeftFootLocation);
		public static LowerBodyTattooLocation LEFT_ANKLE = new LowerBodyTattooLocation(1, SmallTattoosOnly, LeftAnkleButton, LeftAnkleLocation);
		public static LowerBodyTattooLocation LEFT_OUTER_THIGH = new LowerBodyTattooLocation(2, MediumTattoosOrSmaller, LeftOuterThighButton, LeftOuterThighLocation);
		public static LowerBodyTattooLocation LEFT_INNER_THIGH = new LowerBodyTattooLocation(3, MediumTattoosOrSmaller, LeftInnerThighButton, LeftInnerThighLocation);
		public static LowerBodyTattooLocation LEFT_THIGH = new LowerBodyTattooLocation(4, MediumTattoosOrSmaller, LeftThighButton, LeftThighLocation);
		public static LowerBodyTattooLocation LEFT_CALF = new LowerBodyTattooLocation(5, MediumTattoosOrSmaller, LeftCalfButton, LeftCalfLocation);
		public static LowerBodyTattooLocation LEFT_FULL = new LowerBodyTattooLocation(6, FullPartTattoo, LeftFullButton, LeftFullLocation);

		public static LowerBodyTattooLocation RIGHT_FOOT = new LowerBodyTattooLocation(7, SmallTattoosOnly, RightFootButton, RightFootLocation);
		public static LowerBodyTattooLocation RIGHT_ANKLE = new LowerBodyTattooLocation(8, SmallTattoosOnly, RightAnkleButton, RightAnkleLocation);
		public static LowerBodyTattooLocation RIGHT_OUTER_THIGH = new LowerBodyTattooLocation(9, MediumTattoosOrSmaller, RightOuterThighButton, RightOuterThighLocation);
		public static LowerBodyTattooLocation RIGHT_INNER_THIGH = new LowerBodyTattooLocation(10, MediumTattoosOrSmaller, RightInnerThighButton, RightInnerThighLocation);
		public static LowerBodyTattooLocation RIGHT_THIGH = new LowerBodyTattooLocation(11, MediumTattoosOrSmaller, RightThighButton, RightThighLocation);
		public static LowerBodyTattooLocation RIGHT_CALF = new LowerBodyTattooLocation(12, MediumTattoosOrSmaller, RightCalfButton, RightCalfLocation);
		public static LowerBodyTattooLocation RIGHT_FULL = new LowerBodyTattooLocation(13, FullPartTattoo, RightFullButton, RightFullLocation);

		public static bool LocationsCompatible(LowerBodyTattooLocation first, LowerBodyTattooLocation second)
		{
			//thigh and inner thigh are incompatible.
			//thigh and outer thigh are incompatible.
			//inner and outer thigh are compatible.
			//the remainder of these are compatible.run these checks accordingly.

			//if one is left thigh.
			if (first == LEFT_THIGH || second == LEFT_THIGH)
			{
				//check to see if other is left inner or left outer thigh.
				LowerBodyTattooLocation other = (first == LEFT_THIGH) ? second : first;
				return other != LEFT_INNER_THIGH && other != LEFT_OUTER_THIGH;
			}
			//ditto for right thigh.
			else if (first == RIGHT_THIGH || second == RIGHT_THIGH)
			{
				LowerBodyTattooLocation other = (first == RIGHT_THIGH) ? second : first;
				return other != RIGHT_INNER_THIGH && other != RIGHT_OUTER_THIGH;
			}
			//otherwise, we're good.s
			else
			{
				return true;
			}
		}
	}

	public sealed class LowerBodyTattoo : TattooablePart<LowerBodyTattooLocation>
	{
		public LowerBodyTattoo(IBodyPart source, GenericCreatureText allTattoosShort, GenericCreatureText allTattoosLong) : base(source, allTattoosShort, allTattoosLong)
		{
		}

		public override int MaxTattoos => LowerBodyTattooLocation.allLocations.Count;

		public override IEnumerable<LowerBodyTattooLocation> availableLocations => LowerBodyTattooLocation.allLocations;

		public override bool LocationsCompatible(LowerBodyTattooLocation first, LowerBodyTattooLocation second) => LowerBodyTattooLocation.LocationsCompatible(first, second);
	}

	//data changed event will never fire.
	public sealed partial class LowerBody : BehavioralSaveablePart<LowerBody, LowerBodyType, LowerBodyData>, ICanAttackWith
	{
		public override string BodyPartName() => Name();

		public readonly Feet feet;
		//No magic constants. Woo!
		//not even remotely necessary, but it makes it a hell of a lot easier to debug
		//when numbers aren't magic constants. (running grep with a string is much easier
		//than a regular expression looking for legs.count = [A-Za-z0-9]+ or something worse

		public const byte MONOPED_LEG_COUNT = 1;
		public const byte BIPED_LEG_COUNT = 2;
		public const byte QUADRUPED_LEG_COUNT = 4;
		public const byte SEXTOPED_LEG_COUNT = 6; //for squids/octopi, if i implement them. technically, an octopus is 2 legs and 6 arms, but i like the 6 legs 2 arms because legs have a count, arms dont.
		public const byte OCTOPED_LEG_COUNT = 8;

		private BodyData bodyData => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.body.AsReadOnlyData() : new BodyData(creatureID);

		//incorporeality is now implemented as an extension method in the frontend. this lets us extend the incorporeal perk to other body parts if we desire, and keeps the value
		//set to true as long as we have the incorporeality perk, so we don't need to fuss with that.


		public override LowerBodyType type
		{
			get => _type;
			protected set
			{
				_type = value;
				feet.UpdateType(value.footType);
			}
		}
		private LowerBodyType _type;

		public override LowerBodyType defaultType => LowerBodyType.defaultValue;

		public EpidermalData primaryEpidermis => type.ParseEpidermis(bodyData);
		public EpidermalData secondaryEpidermis => type.ParseEpidermis(bodyData);
		public int legCount => type.legCount;

		public bool isMonoped => legCount == MONOPED_LEG_COUNT;
		public bool isBiped => legCount == BIPED_LEG_COUNT;
		public bool isQuadruped => legCount == QUADRUPED_LEG_COUNT;
		public bool isSextoped => legCount == SEXTOPED_LEG_COUNT;
		public bool isOctoped => legCount == OCTOPED_LEG_COUNT;

		public readonly LowerBodyTattoo tattoos;

		internal LowerBody(Guid creatureID, LowerBodyType type) : base(creatureID)
		{
			_type = type ?? throw new ArgumentNullException(nameof(type));
			feet = new Feet(creatureID, type.footType);

			tattoos = new LowerBodyTattoo(this, AllTattoosShort, AllTattoosLong);
		}

		internal LowerBody(Guid creatureID) : this(creatureID, LowerBodyType.defaultValue)
		{ }

		protected internal override void PostPerkInit()
		{
			feet.PostPerkInit();
		}

		protected internal override void LateInit()
		{
			feet.LateInit();
		}

		//standard update, restore are fine.

		internal override bool Validate(bool correctInvalidData)
		{
			LowerBodyType lowerBodyType = type;
			bool valid = LowerBodyType.Validate(ref lowerBodyType, correctInvalidData);
			type = lowerBodyType;
			return valid;
		}

		public bool IsReptilian() => type.IsReptilian();

		#region Text
		public string ShortDescription(bool pluralIfApplicable) => type.ShortDescription(pluralIfApplicable);

		public string ShortDescription(bool pluralIfApplicable, out bool isPlural) => type.ShortDescription(pluralIfApplicable, out isPlural);

		public string LongDescription(bool alternateFormat, out bool isPlural) => type.LongDescription(AsReadOnlyData(), alternateFormat, out isPlural);
		public string LongDescription(bool alternateFormat, bool pluralIfApplicable) => type.LongDescription(AsReadOnlyData(), alternateFormat, pluralIfApplicable);
		public string LongDescription(bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
			=> type.LongDescription(AsReadOnlyData(), alternateFormat, pluralIfApplicable, out isPlural);

		public string LongDescriptionPrimary(bool pluralIfApplicable) => type.LongDescriptionPrimary(AsReadOnlyData(), pluralIfApplicable);
		public string LongDescriptionPrimary(bool pluralIfApplicable, out bool isPlural) => type.LongDescriptionPrimary(AsReadOnlyData(), pluralIfApplicable, out isPlural);

		public string LongDescriptionAlternate(bool pluralIfApplicable) => type.LongDescriptionAlternate(AsReadOnlyData(), pluralIfApplicable);
		public string LongDescriptionAlternate(bool pluralIfApplicable, out bool isPlural) => type.LongDescriptionAlternate(AsReadOnlyData(), pluralIfApplicable, out isPlural);

		public string FullDescription(bool alternateFormat, bool plural, bool includeToes, out bool isPlural)
			=> type.FullDescription(AsReadOnlyData(), alternateFormat, plural, includeToes, out isPlural);

		public string FullDescriptionPrimary(bool plural, bool includeToes, out bool isPlural) => type.FullDescriptionPrimary(AsReadOnlyData(), plural, includeToes, out isPlural);

		public string FullDescriptionAlternate(bool plural, bool includeToes, out bool isPlural) => type.FullDescriptionAlternate(AsReadOnlyData(), plural, includeToes, out isPlural);

		public string FullDescription(bool alternateFormat = false, bool plural = true, bool includeToes = false)
			=> type.FullDescription(AsReadOnlyData(), alternateFormat, plural, includeToes);

		public string FullDescriptionPrimary(bool plural = true, bool includeToes = false) => type.FullDescriptionPrimary(AsReadOnlyData(), plural, includeToes);

		public string FullDescriptionAlternate(bool plural = true, bool includeToes = false) => type.FullDescriptionAlternate(AsReadOnlyData(), plural, includeToes);

		public string OneOfLegsShort(string pronoun = "your")
		{
			return CommonBodyPartStrings.OneOfDescription(legCount > 1, pronoun, ShortDescription());
		}

		public string EachOfLegsShort(string pronoun = "your")
		{
			return EachOfLegsShort(pronoun, out bool _);
		}

		public string EachOfLegsShort(string pronoun, out bool isPlural)
		{
			isPlural = legCount != 1;

			return CommonBodyPartStrings.EachOfDescription(legCount > 1, pronoun, ShortDescription());
		}

		#endregion

		AttackBase ICanAttackWith.attack => type.attack;
		bool ICanAttackWith.canAttackWith() => type.canAttackWith;

		public override LowerBodyData AsReadOnlyData()
		{
			return new LowerBodyData(creatureID, type, primaryEpidermis, secondaryEpidermis);
		}
	}

	public abstract partial class LowerBodyType : SaveableBehavior<LowerBodyType, LowerBody, LowerBodyData>
	{

		private const int NOLEGS = 0;
		private const int MONOPED = 1;
		private const int BIPED = 2;
		private const int QUADRUPED = 4;
		private const int SEXTOPED = 6;
		private const int OCTOPED = 8;

		public readonly byte legCount;

		private static int indexMaker = 0;
		private static readonly List<LowerBodyType> lowerBodyTypes = new List<LowerBodyType>();
		public static ReadOnlyCollection<LowerBodyType> availableTypes => new ReadOnlyCollection<LowerBodyType>(lowerBodyTypes.Where(x => x != null).ToList());

		public static LowerBodyType defaultValue => HUMAN;

		private ShortMaybePluralDescriptor shortPluralDesc;
		private MaybePluralPartDescriptor<LowerBodyData> longPluralDesc;
		private LowerBodyAndFeetDescriptor descriptionWithFeet;

		public string ShortDescription(bool pluralIfApplicable) => shortPluralDesc(pluralIfApplicable, out bool _);
		public string ShortDescription(bool pluralIfApplicable, out bool isPlural) => shortPluralDesc(pluralIfApplicable, out isPlural);

		public string LongDescription(LowerBodyData data, bool alternateFormat, out bool isPlural) => longPluralDesc(data, alternateFormat, true, out isPlural);
		public string LongDescription(LowerBodyData data, bool alternateFormat, bool pluralIfApplicable) => longPluralDesc(data, alternateFormat, pluralIfApplicable, out bool _);
		public string LongDescription(LowerBodyData data, bool alternateFormat, bool pluralIfApplicable, out bool isPlural) => longPluralDesc(data, alternateFormat, pluralIfApplicable, out isPlural);

		public string LongDescriptionPrimary(LowerBodyData data, bool pluralIfApplicable) => longPluralDesc(data, false, pluralIfApplicable, out bool _);
		public string LongDescriptionPrimary(LowerBodyData data, bool pluralIfApplicable, out bool isPlural) => longPluralDesc(data, false, pluralIfApplicable, out isPlural);

		public string LongDescriptionAlternate(LowerBodyData data, bool pluralIfApplicable) => longPluralDesc(data, true, pluralIfApplicable, out bool _);
		public string LongDescriptionAlternate(LowerBodyData data, bool pluralIfApplicable, out bool isPlural) => longPluralDesc(data, true, pluralIfApplicable, out isPlural);

		public string FullDescription(LowerBodyData lowerBody, bool alternateFormat, bool plural, bool includeToes, out bool isPlural)
		{
			return descriptionWithFeet(lowerBody, alternateFormat, plural, includeToes, out isPlural);
		}
		public string FullDescriptionPrimary(LowerBodyData lowerBody, bool plural, bool includeToes, out bool isPlural)
		{
			return descriptionWithFeet(lowerBody, false, plural, includeToes, out isPlural);
		}
		public string FullDescriptionAlternate(LowerBodyData lowerBodys, bool plural, bool includeToes, out bool isPlural)
		{
			return descriptionWithFeet(lowerBodys, true, plural, includeToes, out isPlural);
		}

		public string FullDescription(LowerBodyData lowerBody, bool alternateFormat = false, bool plural = true, bool includeToes = false)
		{
			return descriptionWithFeet(lowerBody, alternateFormat, plural, includeToes, out bool _);
		}
		public string FullDescriptionPrimary(LowerBodyData lowerBody, bool plural = true, bool includeToes = false)
		{
			return descriptionWithFeet(lowerBody, false, plural, includeToes, out bool _);
		}
		public string FullDescriptionAlternate(LowerBodyData lowerBodys, bool plural = true, bool includeToes = false)
		{
			return descriptionWithFeet(lowerBodys, true, plural, includeToes, out bool _);
		}

		protected delegate string LowerBodyAndFeetDescriptor(LowerBodyData arms, bool alternateFormat, bool plural, bool includeToes, out bool isPlural);


		public readonly FootType footType;
		public readonly EpidermisType epidermisType;
		protected LowerBodyType(FootType foot, EpidermisType epidermis, byte numLegs,
			ShortMaybePluralDescriptor shortDesc, SimpleDescriptor singleItemDesc, MaybePluralPartDescriptor<LowerBodyData> longDesc, LowerBodyAndFeetDescriptor fullDesc,
			PlayerBodyPartDelegate<LowerBody> playerDesc, ChangeType<LowerBodyData> transform, RestoreType<LowerBodyData> restore)
			: base(PluralHelper(shortDesc), singleItemDesc, LongPluralHelper(longDesc), playerDesc, transform, restore)
		{
			_index = indexMaker++;
			lowerBodyTypes.AddAt(this, _index);
			footType = foot;
			epidermisType = epidermis;
			legCount = numLegs;

			shortPluralDesc = shortDesc;
			longPluralDesc = longDesc;

			descriptionWithFeet = fullDesc ?? throw new ArgumentNullException(nameof(fullDesc));
		}

		internal static bool Validate(ref LowerBodyType type, bool correctInvalidData)
		{
			if (lowerBodyTypes.Contains(type))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				type = HUMAN;
			}
			return false;
		}

		internal static LowerBodyType Deserialize(int index)
		{
			if (index < 0 || index >= lowerBodyTypes.Count)
			{
				throw new System.ArgumentException("index for lower body type desrialize out of range");
			}
			else
			{
				LowerBodyType lowerBody = lowerBodyTypes[index];
				if (lowerBody != null)
				{
					return lowerBody;
				}
				else
				{
					throw new System.ArgumentException("index for lower body type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		public bool isMonoped => legCount == MONOPED;
		public bool isBiped => legCount == BIPED;
		public bool isQuadruped => legCount == QUADRUPED;
		public bool isSextoped => legCount == SEXTOPED;
		public bool isOctoped => legCount == OCTOPED;

		internal abstract EpidermalData ParseEpidermis(in BodyData bodyData);

		internal virtual EpidermalData ParseSecondaryEpidermis(in BodyData bodyData)
		{
			return new EpidermalData();
		}

		//internal virtual AttackBase attack => AttackBase.NO_ATTACK;
		internal virtual AttackBase attack => AttackBase.NO_ATTACK;
		internal virtual bool canAttackWith => attack != AttackBase.NO_ATTACK;
		//internal abstract bool canAttackWith { get; }

		public override int id => _index;
		private readonly int _index;


		public static readonly LowerBodyType HUMAN = new ToneLowerBody(FootType.HUMAN, EpidermisType.SKIN, BIPED, DefaultValueHelpers.defaultHumanTone, SkinTexture.NONDESCRIPT, true, HumanDesc, HumanSingleDesc, HumanLongDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly LowerBodyType HOOVED = new FurLowerBodyWithKick(FootType.HOOVES, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultHorseFur, FurTexture.NONDESCRIPT, true, HoovedDesc, HoovedSingleDesc, HoovedLongDesc, HoovedFullDesc, HoovedPlayerStr, HoovedTransformStr, HoovedRestoreStr);
		public static readonly LowerBodyType DOG = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultDogFur, FurTexture.NONDESCRIPT, true, DogDesc, DogSingleDesc, DogLongDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly LowerBodyType CENTAUR = new FurLowerBodyWithKick(FootType.HOOVES, EpidermisType.FUR, QUADRUPED, DefaultValueHelpers.defaultHorseFur, FurTexture.NONDESCRIPT, true, HoovedDesc, HoovedSingleDesc, HoovedLongDesc, HoovedFullDesc, HoovedPlayerStr, HoovedTransformStr, HoovedRestoreStr);
		public static readonly LowerBodyType FERRET = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultFerretFur, FurTexture.NONDESCRIPT, false, FerretDesc, FerretSingleDesc, FerretLongDesc, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly LowerBodyType DEMONIC_HIGH_HEELS = new ToneLowerBody(FootType.DEMON_HEEL, EpidermisType.SKIN, BIPED, DefaultValueHelpers.defaultDemonTone, SkinTexture.NONDESCRIPT, true, DemonHiHeelsDesc, DemonHiHeelsSingleDesc, DemonHiHeelsLongDesc, DemonHiHeelsFullDesc, DemonHiHeelsPlayerStr, DemonHiHeelsTransformStr, DemonHiHeelsRestoreStr);
		public static readonly LowerBodyType DEMONIC_CLAWS = new ToneLowerBody(FootType.DEMON_CLAW, EpidermisType.SKIN, BIPED, DefaultValueHelpers.defaultDemonTone, SkinTexture.NONDESCRIPT, true, DemonClawDesc, DemonClawSingleDesc, DemonClawLongDesc, DemonClawFullDesc, DemonClawPlayerStr, DemonClawTransformStr, DemonClawRestoreStr);
		public static readonly LowerBodyType BEE = new ToneLowerBody(FootType.INSECTOID, EpidermisType.CARAPACE, BIPED, DefaultValueHelpers.defaultBeeTone, SkinTexture.SHINY, false, BeeDesc, BeeSingleDesc, BeeLongDesc, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		public static readonly LowerBodyType GOO = new ToneLowerBody(FootType.GOO_NONE, EpidermisType.GOO, MONOPED, DefaultValueHelpers.defaultGooTone, SkinTexture.NONDESCRIPT, true, GooDesc, GooSingleDesc, GooLongDesc, GooFullDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		public static readonly LowerBodyType CAT = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultCatFur, FurTexture.NONDESCRIPT, true, CatDesc, CatSingleDesc, CatLongDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly LowerBodyType LIZARD = new ToneLowerBody(FootType.LIZARD_CLAW, EpidermisType.SCALES, BIPED, DefaultValueHelpers.defaultLizardTone, SkinTexture.NONDESCRIPT, true, LizardDesc, LizardSingleDesc, LizardLongDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		//these things are a MLP easter egg for the bronies (just threw up in my mouth a little). I'm treating it like a centaur with rainbow butt tattoos or whatever they are called.
		//(or a lightning bolt or whatever - i've seen like 20 seconds of the show.) So, 4 legs, text basically identical to centaur but with 'cuter' flavor text (hurl).
		public static readonly LowerBodyType PONY = new FurLowerBodyWithKick(FootType.BRONY, EpidermisType.FUR, QUADRUPED, DefaultValueHelpers.defaultBronyFur, FurTexture.NONDESCRIPT, true, PonyDesc, PonySingleDesc, PonyLongDesc, PonyFullDesc, PonyPlayerStr, PonyTransformStr, PonyRestoreStr);
		public static readonly LowerBodyType BUNNY = new FurLowerBodyWithKick(FootType.RABBIT, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultBunnyFur, FurTexture.NONDESCRIPT, true, BunnyDesc, BunnySingleDesc, BunnyLongDesc, BunnyFullDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static readonly LowerBodyType HARPY = new FurLowerBody(FootType.HARPY_TALON, EpidermisType.FEATHERS, BIPED, DefaultValueHelpers.defaultHarpyFeathers, FurTexture.NONDESCRIPT, true, HarpyDesc, HarpySingleDesc, HarpyLongDesc, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly LowerBodyType KANGAROO = new FurLowerBodyWithKick(FootType.KANGAROO, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultKangarooFur, FurTexture.NONDESCRIPT, true, KangarooDesc, KangarooSingleDesc, KangarooLongDesc, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly LowerBodyType CHITINOUS_SPIDER = new ToneLowerBody(FootType.INSECTOID, EpidermisType.CARAPACE, BIPED, DefaultValueHelpers.defaultSpiderTone, SkinTexture.SHINY, false, SpiderDesc, SpiderSingleDesc, SpiderLongDesc, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr);
		public static readonly LowerBodyType DRIDER = new ToneLowerBody(FootType.INSECTOID, EpidermisType.CARAPACE, OCTOPED, DefaultValueHelpers.defaultSpiderTone, SkinTexture.SHINY, false, DriderDesc, DriderSingleDesc, DriderLongDesc, DriderFullDesc, DriderPlayerStr, DriderTransformStr, DriderRestoreStr);
		public static readonly LowerBodyType FOX = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultFoxFur, FurTexture.NONDESCRIPT, true, FoxDesc, FoxSingleDesc, FoxLongDesc, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		public static readonly LowerBodyType DRAGON = new ToneLowerBody(FootType.DRAGON_CLAW, EpidermisType.SCALES, BIPED, DefaultValueHelpers.defaultDragonTone, SkinTexture.NONDESCRIPT, true, DragonDesc, DragonSingleDesc, DragonLongDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly LowerBodyType RACCOON = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultRaccoonFur, FurTexture.NONDESCRIPT, true, RaccoonDesc, RaccoonSingleDesc, RaccoonLongDesc, RaccoonFullDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr);
		public static readonly LowerBodyType CLOVEN_HOOVED = new FurLowerBody(FootType.HOOVES, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultHorseFur, FurTexture.NONDESCRIPT, true, ClovenHoofDesc, ClovenHoofSingleDesc, ClovenHoofLongDesc, ClovenHoofFullDesc, ClovenHoofPlayerStr, ClovenHoofTransformStr, ClovenHoofRestoreStr);//?
		public static readonly LowerBodyType NAGA = new NagaLowerBody();
		public static readonly LowerBodyType ECHIDNA = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultEchidnaFur, FurTexture.NONDESCRIPT, true, EchidnaDesc, EchidnaSingleDesc, EchidnaLongDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly LowerBodyType SALAMANDER = new ToneLowerBody(FootType.MANDER_CLAW, EpidermisType.SCALES, BIPED, DefaultValueHelpers.defaultSalamanderTone, SkinTexture.NONDESCRIPT, true, SalamanderDesc, SalamanderSingleDesc, SalamanderLongDesc, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr);
		public static readonly LowerBodyType WOLF = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultWolfFurColor, FurTexture.NONDESCRIPT, true, WolfDesc, WolfSingleDesc, WolfLongDesc, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly LowerBodyType IMP = new ToneLowerBody(FootType.IMP_CLAW, EpidermisType.SCALES, BIPED, DefaultValueHelpers.defaultImpTone, SkinTexture.NONDESCRIPT, true, ImpDesc, ImpSingleDesc, ImpLongDesc, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		//granted, this easter egg is probably not much better, but to be fair, it's not called 'octoling' in game, so the only ones who get to see the easter egg are devs.
		public static readonly LowerBodyType OCTOLING = new ToneLowerBody(FootType.TENDRIL, EpidermisType.SKIN, SEXTOPED, DefaultValueHelpers.defaultTentacleTone, SkinTexture.SLIMY, true, OctoDesc, OctoSingleDesc, OctoLongDesc, OctoFullDesc, OctoPlayerStr, OctoTransformStr, OctoRestoreStr);
		public static readonly LowerBodyType COCKATRICE = new CockatriceLowerBody();
		public static readonly LowerBodyType RED_PANDA = new RedPandaLowerBody();

		public bool IsReptilian()
		{
			return this == SALAMANDER || this == LIZARD || this == DRAGON;
		}


		private class FurLowerBody : LowerBodyType
		{
			public readonly FurColor defaultColor;
			public readonly FurTexture defaultTexture;
			protected readonly bool mutable;

			protected FurBasedEpidermisType primaryEpidermis => (FurBasedEpidermisType)epidermisType;

			public FurLowerBody(FootType foot, FurBasedEpidermisType epidermis, byte numLegs, FurColor defaultFurColor, FurTexture defaultFurTexture, bool canChange,
				ShortMaybePluralDescriptor shortDesc, SimpleDescriptor singleDesc, MaybePluralPartDescriptor<LowerBodyData> longDesc, LowerBodyAndFeetDescriptor fullDesc, PlayerBodyPartDelegate<LowerBody> playerDesc,
				ChangeType<LowerBodyData> transform, RestoreType<LowerBodyData> restore) : base(foot, epidermis, numLegs, shortDesc, singleDesc, longDesc, fullDesc, playerDesc, transform, restore)
			{
				defaultColor = new FurColor(defaultFurColor);
				defaultTexture = defaultFurTexture;
				mutable = canChange;
			}

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				FurColor color = this.defaultColor;
				FurTexture texture = this.defaultTexture;
				if (mutable)
				{
					if (bodyData.supplementary.usesFurColor && !FurColor.IsNullOrEmpty(bodyData.supplementary.fur))
					{
						color = bodyData.supplementary.fur;
					}
					else if (!FurColor.IsNullOrEmpty(bodyData.main.fur))
					{
						color = bodyData.main.fur;
					}
					else if (!HairFurColors.IsNullOrEmpty(bodyData.hairColor))
					{
						color = new FurColor(bodyData.hairColor);
					}

					if (bodyData.supplementary.usesFurColor && bodyData.supplementary.furTexture != FurTexture.NONDESCRIPT)
					{
						texture = bodyData.supplementary.furTexture;
					}
					else if (bodyData.main.usesFurColor && bodyData.main.furTexture != FurTexture.NONDESCRIPT)
					{
						texture = bodyData.main.furTexture;
					}
				}
				return new EpidermalData(primaryEpidermis, color, defaultTexture);
			}
		}

		private class FurLowerBodyWithKick : FurLowerBody
		{
			public FurLowerBodyWithKick(FootType foot, FurBasedEpidermisType epidermis, byte numLegs, FurColor defaultFurColor, FurTexture defaultFurTexture, bool canChange,
				ShortMaybePluralDescriptor shortDesc, SimpleDescriptor singleDesc, MaybePluralPartDescriptor<LowerBodyData> longDesc, LowerBodyAndFeetDescriptor fullDesc,
				PlayerBodyPartDelegate<LowerBody> playerDesc, ChangeType<LowerBodyData> transform, RestoreType<LowerBodyData> restore)
				: base(foot, epidermis, numLegs, defaultFurColor, defaultFurTexture, canChange, shortDesc, singleDesc, longDesc, fullDesc, playerDesc, transform, restore) { }

			internal override AttackBase attack => _attack;
			private static readonly AttackBase _attack = new GenericKick();
		}

		private class ToneLowerBody : LowerBodyType
		{
			public readonly SkinTexture defaultTexture;
			public readonly bool mutable;
			public readonly Tones defaultTone;

			protected ToneBasedEpidermisType primaryEpidermis => (ToneBasedEpidermisType)epidermisType;

			public ToneLowerBody(FootType foot, ToneBasedEpidermisType epidermis, byte legCount, Tones defTone, SkinTexture defaultSkinTexture, bool canChange,
				 ShortMaybePluralDescriptor shortDesc, SimpleDescriptor singleDesc, MaybePluralPartDescriptor<LowerBodyData> longDesc, LowerBodyAndFeetDescriptor fullDesc, PlayerBodyPartDelegate<LowerBody> playerDesc,
				 ChangeType<LowerBodyData> transform, RestoreType<LowerBodyData> restore) : base(foot, epidermis, legCount, shortDesc, singleDesc, longDesc, fullDesc, playerDesc, transform, restore)
			{
				defaultTexture = defaultSkinTexture;
				defaultTone = defTone;
				mutable = canChange;
			}

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				Tones color = mutable ? bodyData.mainSkin.tone : defaultTone;
				SkinTexture texture = mutable && bodyData.main.usesTone ? bodyData.main.skinTexture : defaultTexture;

				return new EpidermalData(primaryEpidermis, color, texture);
			}
		}


		private class NagaLowerBody : ToneLowerBody
		{
			private Tones defaultUnderTone => DefaultValueHelpers.defaultNagaUnderTone;
			public NagaLowerBody() : base(FootType.NAGA_NONE, EpidermisType.SCALES, MONOPED, DefaultValueHelpers.defaultNagaTone,
				SkinTexture.NONDESCRIPT, true, NagaDesc, NagaSingleDesc, NagaLongDesc, NagaFullDesc, NagaPlayerStr, NagaTransformStr, NagaRestoreStr)
			{ }

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				Tones color = bodyData.type == BodyType.REPTILE && !Tones.IsNullOrEmpty(bodyData.supplementary.tone) ? bodyData.supplementary.tone : bodyData.mainSkin.tone;
				return new EpidermalData(primaryEpidermis, color, defaultTexture);
			}

			internal override EpidermalData ParseSecondaryEpidermis(in BodyData bodyData)
			{
				Tones color = bodyData.supplementary.tone;
				if (bodyData.type == BodyType.REPTILE && !Tones.IsNullOrEmpty(bodyData.supplementary.tone))
				{
					color = DefaultValueHelpers.GetNageUnderToneFrom(color);
				}
				else if (Tones.IsNullOrEmpty(color))
				{
					color = defaultUnderTone;
				}
				return new EpidermalData(primaryEpidermis, color, defaultTexture);
			}

			internal override AttackBase attack => _attack;
			private static readonly AttackBase _attack = new NagaConstrict();
		}

		private class CockatriceLowerBody : FurLowerBody
		{
			public CockatriceLowerBody() : base(FootType.HARPY_TALON, EpidermisType.FEATHERS, BIPED, DefaultValueHelpers.defaultCockatricePrimaryFeathers, FurTexture.NONDESCRIPT, true,
				CockatriceDesc, CockatriceSingleDesc, CockatriceLongDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr)
			{ }

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				FurColor color = this.defaultColor;
				FurTexture texture = this.defaultTexture;
				if (!FurColor.IsNullOrEmpty(bodyData.activeFur.fur))
				{
					color = bodyData.activeFur.fur;
				}
				else if (!HairFurColors.IsNullOrEmpty(bodyData.hairColor))
				{
					color = new FurColor(bodyData.hairColor);
				}
				return new EpidermalData(primaryEpidermis, color, bodyData.main.furTexture);
			}
		}

		private class RedPandaLowerBody : FurLowerBody
		{
			public RedPandaLowerBody() : base(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultRedPandaFur, FurTexture.NONDESCRIPT, true, RedPandaDesc,
				RedPandaSingleDesc, RedPandaLongDesc, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr)
			{ }

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				FurColor color = bodyData.supplementary.usesFurColor && !FurColor.IsNullOrEmpty(bodyData.supplementary.fur) ? bodyData.supplementary.fur : defaultColor;
				FurTexture texture = bodyData.supplementary.usesFurColor && bodyData.supplementary.furTexture != FurTexture.NONDESCRIPT ? bodyData.supplementary.furTexture : defaultTexture;
				return new EpidermalData(primaryEpidermis, color, texture);
			}
		}
	}

	public sealed class LowerBodyData : BehavioralSaveablePartData<LowerBodyData, LowerBody, LowerBodyType>
	{
		public readonly EpidermalData primaryEpidermis;
		public readonly EpidermalData secondaryEpidermis;

		public byte legCount => type.legCount;
		public FootType footType => type.footType;

		public bool isMonoped => legCount == LowerBody.MONOPED_LEG_COUNT;
		public bool isBiped => legCount == LowerBody.BIPED_LEG_COUNT;
		public bool isQuadruped => legCount == LowerBody.QUADRUPED_LEG_COUNT;
		public bool isSextoped => legCount == LowerBody.SEXTOPED_LEG_COUNT;
		public bool isOctoped => legCount == LowerBody.OCTOPED_LEG_COUNT;

		public override LowerBodyData AsCurrentData()
		{
			return this;
		}

		#region Text
		public string ShortDescription(bool pluralIfApplicable) => type.ShortDescription(pluralIfApplicable);

		public string ShortDescription(bool pluralIfApplicable, out bool isPlural) => type.ShortDescription(pluralIfApplicable, out isPlural);

		public string LongDescription(bool alternateFormat, out bool isPlural) => type.LongDescription(this, alternateFormat, out isPlural);
		public string LongDescription(bool alternateFormat, bool pluralIfApplicable) => type.LongDescription(this, alternateFormat, pluralIfApplicable);
		public string LongDescription(bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
			=> type.LongDescription(this, alternateFormat, pluralIfApplicable, out isPlural);

		public string LongDescriptionPrimary(bool pluralIfApplicable) => type.LongDescriptionPrimary(this, pluralIfApplicable);
		public string LongDescriptionPrimary(bool pluralIfApplicable, out bool isPlural) => type.LongDescriptionPrimary(this, pluralIfApplicable, out isPlural);

		public string LongDescriptionAlternate(bool pluralIfApplicable) => type.LongDescriptionAlternate(this, pluralIfApplicable);
		public string LongDescriptionAlternate(bool pluralIfApplicable, out bool isPlural) => type.LongDescriptionAlternate(this, pluralIfApplicable, out isPlural);

		public string FullDescription(bool alternateFormat, bool plural, bool includeToes, out bool isPlural)
			=> type.FullDescription(this, alternateFormat, plural, includeToes, out isPlural);

		public string FullDescriptionPrimary(bool plural, bool includeToes, out bool isPlural) => type.FullDescriptionPrimary(this, plural, includeToes, out isPlural);

		public string FullDescriptionAlternate(bool plural, bool includeToes, out bool isPlural) => type.FullDescriptionAlternate(this, plural, includeToes, out isPlural);

		public string FullDescription(bool alternateFormat = false, bool plural = true, bool includeToes = false)
			=> type.FullDescription(this, alternateFormat, plural, includeToes);

		public string FullDescriptionPrimary(bool plural = true, bool includeToes = false) => type.FullDescriptionPrimary(this, plural, includeToes);

		public string FullDescriptionAlternate(bool plural = true, bool includeToes = false) => type.FullDescriptionAlternate(this, plural, includeToes);

		public string OneOfLegsShort(string pronoun = "your")
		{
			return CommonBodyPartStrings.OneOfDescription(legCount > 1, pronoun, ShortDescription());
		}

		public string EachOfLegsShort(string pronoun = "your")
		{
			return EachOfLegsShort(pronoun, out bool _);
		}

		public string EachOfLegsShort(string pronoun, out bool isPlural)
		{
			isPlural = legCount != 1;

			return CommonBodyPartStrings.EachOfDescription(legCount > 1, pronoun, ShortDescription());
		}

		#endregion

		internal LowerBodyData(Guid id, LowerBodyType type, EpidermalData epidermis, EpidermalData secondary) : base(id, type)
		{
			primaryEpidermis = epidermis;
			secondaryEpidermis = secondary;
		}

		internal LowerBodyData(Guid id) : base(id, LowerBodyType.defaultValue)
		{
			primaryEpidermis = new Epidermis(type.epidermisType).AsReadOnlyData();
			secondaryEpidermis = new EpidermalData();
		}
	}
}
