//Arms.cs
//Description: Arm Body Part class.
//Author: JustSomeGuy
//12/26/2018, 7:58 PM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Wearables.Tattoos;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CoC.Backend.BodyParts
{
	/*
	 * Arm covering (skin, scales, etc) Note:
	 * Arms now have a consistent logic - if the arm is furry, it will first try to use the secondary (underbody) color, if the body has one. if not, it will fallback to the
	 * body's regular fur color, if one exists. It will fallback to hair if not available, and if THAT is not available, fallback to the default for whatever type of arm it is.
	 * Tones will simply use the primary skin tone - i cannot think of a reason arms would have a special tone different from the body.
	 * Since this logic is implemented in the arm type, a derived class can override this behavior for custom arm types. currently, Ferrets do this.
	 *
	 * This epidermis data is LAZY! meaning its value will only be determined when it's called, and recalculated every time. This way, we don't need to keep track of what is happening
	 * to the body until we actually care. This is implemented by using a property to retrieve the body data (unless this is a standalone, in which case we use the default body data)
	 * when we need it.
	 *
	 * A side effect, however, is that this means it is recalculated on the fly. most of the time, you only query it once when dealing with it, so there's virtually no cost.
	 * however, if you are running a series of if/else/whatever on it, assign the result to a local variable and use that for your conditionals, so the value is only calculated once.
	 *
	 * An aside: normally, to get around this, devs will 'memoize' this data, which basically means they do the local reference trick internally. it persists for each call until
	 * the dev determines the source data changed in some way, at which point the whole thing is recalculated. unfortunately, this isn't easy to do without forcing each type
	 * to handle the memoization (which i really don't want to do because it makes it harder for content creators - they shouldn't need to know advanced programming tricks)
	 */

	//Note: Never fires a data change event, as it has no data that can be changed. Note that technically claws could fire a change, but whatever.

	//ordered by size. all are compatible except sleeve, which is incompatible with everything.

	public sealed partial class ArmTattooLocation : TattooLocation
	{

		private static readonly List<ArmTattooLocation> _allLocations = new List<ArmTattooLocation>();

		public static readonly ReadOnlyCollection<ArmTattooLocation> allLocations;

		public static IList<ArmTattooLocation> allLeftArmTattoos
		{
			get
			{
				if (_leftArmTattoos is null)
				{
					_leftArmTattoos = new ArmTattooLocation[] { LEFT_HAND, LEFT_WRIST, LEFT_INNER_FOREARM, LEFT_OUTER_FOREARM, LEFT_FOREARM, LEFT_UPPER_ARM, LEFT_SHOULDER, LEFT_SLEEVE };
				}
				return _leftArmTattoos;
			}
		}
		private static ArmTattooLocation[] _leftArmTattoos = null;

		public static IList<ArmTattooLocation> allRightArmTattoos
		{
			get
			{
				if (_rightArmTattoos is null)
				{
					_rightArmTattoos = new ArmTattooLocation[] { RIGHT_HAND, RIGHT_WRIST, RIGHT_INNER_FOREARM, RIGHT_OUTER_FOREARM, RIGHT_FOREARM, RIGHT_UPPER_ARM, RIGHT_SHOULDER, RIGHT_SLEEVE };
				}
				return _rightArmTattoos;
			}
		}
		private static ArmTattooLocation[] _rightArmTattoos = null;

		public static IList<ArmTattooLocation> LeftArmTattoos(bool fullForearm)
		{
			if (fullForearm)
			{
				return new ArmTattooLocation[] { LEFT_HAND, LEFT_WRIST, LEFT_FOREARM, LEFT_UPPER_ARM, LEFT_SHOULDER, LEFT_SLEEVE };
			}
			else //if (!fullForearm)
			{
				return new ArmTattooLocation[] { LEFT_HAND, LEFT_WRIST, LEFT_INNER_FOREARM, LEFT_OUTER_FOREARM, LEFT_UPPER_ARM, LEFT_SHOULDER, LEFT_SLEEVE };
			}
		}

		public static IList<ArmTattooLocation> RightArmTattoos(bool fullForearm)
		{
			if (fullForearm)
			{
				return new ArmTattooLocation[] { RIGHT_HAND, RIGHT_WRIST, RIGHT_FOREARM, RIGHT_UPPER_ARM, RIGHT_SHOULDER, RIGHT_SLEEVE };
			}
			else //if (!fullForearm)
			{
				return new ArmTattooLocation[] { RIGHT_HAND, RIGHT_WRIST, RIGHT_INNER_FOREARM, RIGHT_OUTER_FOREARM, RIGHT_UPPER_ARM, RIGHT_SHOULDER, RIGHT_SLEEVE };
			}
		}

		private readonly byte index;

		static ArmTattooLocation()
		{
			allLocations = new ReadOnlyCollection<ArmTattooLocation>(_allLocations);
		}

		private ArmTattooLocation(byte index, TattooSizeLimit limitSize, SimpleDescriptor btnText, SimpleDescriptor locationDesc) : base(limitSize, btnText, locationDesc)
		{
			this.index = index;
		}

		public static ArmTattooLocation LEFT_HAND = new ArmTattooLocation(0, SmallTattoosOnly, LeftHandButton, LeftHandLocation);
		public static ArmTattooLocation LEFT_WRIST = new ArmTattooLocation(1, SmallTattoosOnly, LeftWristButton, LeftWristLocation);
		public static ArmTattooLocation LEFT_INNER_FOREARM = new ArmTattooLocation(2, MediumTattoosOrSmaller, LeftInnerArmButton, LeftInnerArmLocation);
		public static ArmTattooLocation LEFT_OUTER_FOREARM = new ArmTattooLocation(3, MediumTattoosOrSmaller, LeftOuterArmButton, LeftOuterArmLocation);
		public static ArmTattooLocation LEFT_FOREARM = new ArmTattooLocation(4, MediumTattoosOrSmaller, LeftForearmButton, LeftForearmLocation);
		public static ArmTattooLocation LEFT_UPPER_ARM = new ArmTattooLocation(5, MediumTattoosOrSmaller, LeftUpperArmButton, LeftUpperArmLocation);
		public static ArmTattooLocation LEFT_SHOULDER = new ArmTattooLocation(6, MediumTattoosOrSmaller, LeftShoulderButton, LeftShoulderLocation);
		public static ArmTattooLocation LEFT_SLEEVE = new ArmTattooLocation(7, FullPartTattoo, LeftSleeveButton, LeftSleeveLocation);

		public static ArmTattooLocation RIGHT_HAND = new ArmTattooLocation(8, SmallTattoosOnly, RightHandButton, RightHandLocation);
		public static ArmTattooLocation RIGHT_WRIST = new ArmTattooLocation(9, SmallTattoosOnly, RightWristButton, RightWristLocation);
		public static ArmTattooLocation RIGHT_INNER_FOREARM = new ArmTattooLocation(10, MediumTattoosOrSmaller, RightInnerArmButton, RightInnerArmLocation);
		public static ArmTattooLocation RIGHT_OUTER_FOREARM = new ArmTattooLocation(11, MediumTattoosOrSmaller, RightOuterArmButton, RightOuterArmLocation);
		public static ArmTattooLocation RIGHT_FOREARM = new ArmTattooLocation(12, MediumTattoosOrSmaller, RightForearmButton, RightForearmLocation);
		public static ArmTattooLocation RIGHT_UPPER_ARM = new ArmTattooLocation(13, MediumTattoosOrSmaller, RightUpperArmButton, RightUpperArmLocation);
		public static ArmTattooLocation RIGHT_SHOULDER = new ArmTattooLocation(14, MediumTattoosOrSmaller, RightShoulderButton, RightShoulderLocation);
		public static ArmTattooLocation RIGHT_SLEEVE = new ArmTattooLocation(15, FullPartTattoo, RightSleeveButton, RightSleeveLocation);

		public static bool LocationsCompatible(ArmTattooLocation first, ArmTattooLocation second)
		{
			//forearm and inner forearm are incompatible.
			//forearm and outer forearm are incompatible.
			//inner and outer forearm are compatible.
			//the remainder of these are compatible.run these checks accordingly.

			//if one is left forearm.
			if (first == LEFT_FOREARM || second == LEFT_FOREARM)
			{
				//check to see if other is left inner or left outer forearm.
				ArmTattooLocation other = (first == LEFT_FOREARM) ? second : first;
				return other != LEFT_INNER_FOREARM && other != LEFT_OUTER_FOREARM;
			}
			//ditto for right forearm.
			else if (first == RIGHT_FOREARM || second == RIGHT_FOREARM)
			{
				ArmTattooLocation other = (first == RIGHT_FOREARM) ? second : first;
				return other != RIGHT_INNER_FOREARM && other != RIGHT_OUTER_FOREARM;
			}
			//otherwise, we're good.s
			else
			{
				return true;
			}
		}

	}

	public sealed class ArmTattoo : TattooablePart<ArmTattooLocation>
	{
		public ArmTattoo(IBodyPart source, CreatureStr allTattoosShort, CreatureStr allTattoosLong) : base(source, allTattoosShort, allTattoosLong) { }

		public override int MaxTattoos => ArmTattooLocation.allLocations.Count;

		public int MaxLeftArmCount => ArmTattooLocation.LeftArmTattoos(TattooedAt(ArmTattooLocation.LEFT_FOREARM)).Count;
		public int MaxRightArmCount => ArmTattooLocation.RightArmTattoos(TattooedAt(ArmTattooLocation.RIGHT_FOREARM)).Count;

		public override bool LocationsCompatible(ArmTattooLocation first, ArmTattooLocation second) => ArmTattooLocation.LocationsCompatible(first, second);

		public override IEnumerable<ArmTattooLocation> availableLocations => ArmTattooLocation.allLocations;

		public ArmTattooLocation[] currentLeftArmTattoos => currentTattoos.Intersect(ArmTattooLocation.allLeftArmTattoos).ToArray();
		public ArmTattooLocation[] currentRightArmTattoos => currentTattoos.Intersect(ArmTattooLocation.allRightArmTattoos).ToArray();

		public bool onlyOnLeftArm => currentTattooCount > 0 && ArmTattooLocation.allRightArmTattoos.All(x => !TattooedAt(x));

		public bool onlyOnRightArm => currentTattooCount > 0 && ArmTattooLocation.allLeftArmTattoos.All(x => !TattooedAt(x));

		public bool allOnLeftArm => currentTattooCount == currentLeftArmTattoos.Length && onlyOnLeftArm;
		public bool allOnRightArm => currentTattooCount == currentRightArmTattoos.Length && onlyOnRightArm;

		public bool OnlySleeveTattoos => currentTattooCount == 2 && TattooedAt(ArmTattooLocation.LEFT_SLEEVE) && TattooedAt(ArmTattooLocation.RIGHT_SLEEVE);
		public bool OnlyShoulderTattoos => currentTattooCount == 2 && TattooedAt(ArmTattooLocation.LEFT_SHOULDER) && TattooedAt(ArmTattooLocation.RIGHT_SHOULDER);
		public bool OnlyInnerForearmTattoos => currentTattooCount == 2 && TattooedAt(ArmTattooLocation.LEFT_INNER_FOREARM) && TattooedAt(ArmTattooLocation.RIGHT_INNER_FOREARM);
		public bool OnlyOuterForearmTattoos => currentTattooCount == 2 && TattooedAt(ArmTattooLocation.LEFT_OUTER_FOREARM) && TattooedAt(ArmTattooLocation.RIGHT_OUTER_FOREARM);
		public bool OnlyForearmTattoos => currentTattooCount == 2 && TattooedAt(ArmTattooLocation.LEFT_FOREARM) && TattooedAt(ArmTattooLocation.RIGHT_FOREARM);
		public bool OnlyUpperArmTattoos => currentTattooCount == 2 && TattooedAt(ArmTattooLocation.LEFT_UPPER_ARM) && TattooedAt(ArmTattooLocation.RIGHT_UPPER_ARM);
		public bool OnlyWristTattoos => currentTattooCount == 2 && TattooedAt(ArmTattooLocation.LEFT_WRIST) && TattooedAt(ArmTattooLocation.RIGHT_WRIST);
		public bool OnlyHandTattoos => currentTattooCount == 2 && TattooedAt(ArmTattooLocation.LEFT_HAND) && TattooedAt(ArmTattooLocation.RIGHT_HAND);

		public bool MatchingSleeveTattoos()
		{
			return TattooBase.MatchingTattoos(this[ArmTattooLocation.LEFT_SLEEVE], this[ArmTattooLocation.RIGHT_SLEEVE]);
		}

		public bool MatchingSleeveTattoosIgnoreColor()
		{
			return TattooBase.MatchingTattoosIgnoreColor(this[ArmTattooLocation.LEFT_SLEEVE], this[ArmTattooLocation.RIGHT_SLEEVE]);
		}

		public bool MatchingShoulderTattoos()
		{
			return TattooBase.MatchingTattoos(this[ArmTattooLocation.LEFT_SHOULDER], this[ArmTattooLocation.RIGHT_SHOULDER]);
		}

		public bool MatchingShoulderTattoosIgnoreColor()
		{
			return TattooBase.MatchingTattoosIgnoreColor(this[ArmTattooLocation.LEFT_SHOULDER], this[ArmTattooLocation.RIGHT_SHOULDER]);
		}

		public bool MatchingUpperArmTattoos()
		{
			return TattooBase.MatchingTattoos(this[ArmTattooLocation.LEFT_UPPER_ARM], this[ArmTattooLocation.RIGHT_UPPER_ARM]);
		}

		public bool MatchingUpperArmTattoosIgnoreColor()
		{
			return TattooBase.MatchingTattoosIgnoreColor(this[ArmTattooLocation.LEFT_UPPER_ARM], this[ArmTattooLocation.RIGHT_UPPER_ARM]);
		}

		public bool MatchingForearmTattoos()
		{
			return TattooBase.MatchingTattoos(this[ArmTattooLocation.LEFT_FOREARM], this[ArmTattooLocation.RIGHT_FOREARM]);
		}

		public bool MatchingForearmTattoosIgnoreColor()
		{
			return TattooBase.MatchingTattoosIgnoreColor(this[ArmTattooLocation.LEFT_FOREARM], this[ArmTattooLocation.RIGHT_FOREARM]);
		}

		public bool MatchingOuterForearmTattoos()
		{
			return TattooBase.MatchingTattoos(this[ArmTattooLocation.LEFT_OUTER_FOREARM], this[ArmTattooLocation.RIGHT_OUTER_FOREARM]);
		}

		public bool MatchingOuterForearmTattoosIgnoreColor()
		{
			return TattooBase.MatchingTattoosIgnoreColor(this[ArmTattooLocation.LEFT_OUTER_FOREARM], this[ArmTattooLocation.RIGHT_OUTER_FOREARM]);
		}

		public bool MatchingInnerForearmTattoos()
		{
			return TattooBase.MatchingTattoos(this[ArmTattooLocation.LEFT_INNER_FOREARM], this[ArmTattooLocation.RIGHT_INNER_FOREARM]);
		}

		public bool MatchingInnerForearmTattoosIgnoreColor()
		{
			return TattooBase.MatchingTattoosIgnoreColor(this[ArmTattooLocation.LEFT_INNER_FOREARM], this[ArmTattooLocation.RIGHT_INNER_FOREARM]);
		}

		public bool MatchingWristTattoos()
		{
			return TattooBase.MatchingTattoos(this[ArmTattooLocation.LEFT_WRIST], this[ArmTattooLocation.RIGHT_WRIST]);
		}

		public bool MatchingWristTattoosIgnoreColor()
		{
			return TattooBase.MatchingTattoosIgnoreColor(this[ArmTattooLocation.LEFT_WRIST], this[ArmTattooLocation.RIGHT_WRIST]);
		}

		//matching tattoos.
		public bool MatchingHandTattoos()
		{
			return TattooBase.MatchingTattoos(this[ArmTattooLocation.LEFT_HAND], this[ArmTattooLocation.RIGHT_HAND]);
		}

		//matching tattoos, but different colors.
		public bool MatchingHandTattooIgnoreColor()
		{
			return TattooBase.MatchingTattoosIgnoreColor(this[ArmTattooLocation.LEFT_HAND], this[ArmTattooLocation.RIGHT_HAND]);
		}



	}

	public sealed partial class Arms : FullBehavioralPart<Arms, ArmType, ArmData>
	{
		public override string BodyPartName() => Name();

		public readonly Hands hands;

		private BodyData bodyData => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.body.AsReadOnlyData() : new BodyData(creatureID);

		public EpidermalData epidermis => type.GetPrimaryEpidermis(bodyData);
		public EpidermalData secondaryEpidermis => type.GetSecondaryEpidermis(bodyData);


		public override ArmType type
		{
			get => _type;
			protected set
			{
				_type = value;
				hands.UpdateType(value.handType);
			}
		}
		private ArmType _type;

		public readonly ArmTattoo tattoos;



		public override ArmType defaultType => ArmType.defaultValue;

		public override ArmData AsReadOnlyData()
		{
			return new ArmData(this);
		}

		protected internal override void PostPerkInit()
		{
			hands.PostPerkInit();
		}

		protected internal override void LateInit()
		{
			hands.LateInit();
		}

		public bool usesPrimaryTone => type.hasPrimaryTone;
		public bool usesPrimaryFur => type.hasPrimaryFur;
		public bool usesSecondaryTone => type.hasSecondaryTone;
		public bool usesSecondaryFur => type.hasSecondaryFur;

		public bool usesAnyTone => usesPrimaryTone || usesSecondaryTone;
		public bool usesAnyFur => usesPrimaryFur || usesSecondaryFur;

		internal Arms(Guid creatureID) : this(creatureID, ArmType.defaultValue) { }

		internal Arms(Guid creatureID, ArmType armType) : base(creatureID)
		{
			_type = armType ?? throw new ArgumentNullException(nameof(armType));
			hands = new Hands(creatureID, type.handType, (x) => x ? epidermis : secondaryEpidermis);

			tattoos = new ArmTattoo(this, AllTattoosShort, AllTattoosLong);
		}

		//default implementation of update and restore are fine

		public override bool IsIdenticalTo(ArmData original, bool ignoreSexualMetaData)
		{
			return !(original is null) && type == original.type && epidermis.Equals(original.epidermis) && secondaryEpidermis.Equals(original.secondaryEpidermis)
				&& tattoos.IsIdenticalTo(original.tattoos) && hands.IsIdenticalTo(original.hands, ignoreSexualMetaData);
		}

		//description overloads.
		public string ShortDescription(bool plural) => type.ShortDescription(plural);

		public string LongDescription(bool alternateFormat, bool plural) => type.LongDescription(AsReadOnlyData(), alternateFormat, plural);

		public string LongDescriptionPrimary(bool plural) => type.LongDescriptionPrimary(AsReadOnlyData(), plural);

		public string LongDescriptionAlternate(bool plural) => type.LongDescriptionAlternate(AsReadOnlyData(), plural);

		internal override bool Validate(bool correctInvalidData)
		{
			ArmType armType = type;
			bool retVal = ArmType.Validate(ref armType, correctInvalidData);
			type = armType; //automatically sets hand.
			return retVal;
		}

		public string EpidermisDescription()
		{
			return ArmType.ArmEpidermisDescription(epidermis, secondaryEpidermis);
		}

		public bool IsPredatorArms() => type.IsPredatorArms();
		public bool IsReptilian() => type.IsReptilian();
	}

	public abstract partial class ArmType : FullBehavior<ArmType, Arms, ArmData>
	{
		private static int indexMaker = 0;
		private static readonly List<ArmType> arms = new List<ArmType>();
		public static readonly ReadOnlyCollection<ArmType> availableTypes = new ReadOnlyCollection<ArmType>(arms);

		public static ArmType defaultValue => HUMAN;


		public readonly HandType handType;
		public readonly EpidermisType epidermisType;

		private readonly ShortPluralDescriptor armDesc;
		private readonly PluralPartDescriptor<ArmData> longArmDesc;
		private ArmAndHandsDescriptor descriptionWithHands;

		public string ShortDescription(bool plural) => armDesc(plural);

		public string LongDescription(ArmData arms, bool alternateFormat, bool plural) => longArmDesc(arms, alternateFormat, plural);
		public string LongDescriptionPrimary(ArmData arms, bool plural) => longArmDesc(arms, false, plural);
		public string LongDescriptionAlternate(ArmData arms, bool plural) => longArmDesc(arms, true, plural);

		public string FullDescription(ArmData arms, bool alternateFormat = false, bool plural = true, bool includeFingers = false)
		{
			return descriptionWithHands(arms, alternateFormat, plural, includeFingers);
		}
		public string FullDescriptionPrimary(ArmData arms, bool plural = true, bool includeFingers = false)
		{
			return descriptionWithHands(arms, false, plural, includeFingers);
		}
		public string FullDescriptionAlternate(ArmData arms, bool plural = true, bool includeFingers = false)
		{
			return descriptionWithHands(arms, true, plural, includeFingers);
		}

		//update the original and secondary original based on the current data.

		//internal abstract Epidermis GetEpidermis(bool primary, in BodyData bodyData);
		internal abstract EpidermalData GetPrimaryEpidermis(in BodyData bodyData);
		internal virtual EpidermalData GetSecondaryEpidermis(in BodyData bodyData)
		{
			return new EpidermalData();
		}

		public abstract bool hasPrimaryFur { get; }
		public virtual bool hasSecondaryFur => false;
		public abstract bool hasPrimaryTone { get; }
		public virtual bool hasSecondaryTone => false;

		public override int id => _index;
		private readonly int _index;

		protected internal delegate string ArmAndHandsDescriptor(ArmData arms, bool alternateFormat, bool plural, bool includeFingers);


		private protected ArmType(HandType hand, EpidermisType epidermis,
			ShortPluralDescriptor shortDesc, SimpleDescriptor singleDesc, PluralPartDescriptor<ArmData> longDesc, ArmAndHandsDescriptor fullDesc, PlayerBodyPartDelegate<Arms> playerDesc,
			ChangeType<ArmData> transform, RestoreType<ArmData> restore) : base(PluralHelper(shortDesc), singleDesc, LongPluralHelper(longDesc), playerDesc, transform, restore)
		{
			_index = indexMaker++;
			handType = hand;

			armDesc = shortDesc;
			longArmDesc = longDesc;

			descriptionWithHands = fullDesc ?? throw new ArgumentNullException(nameof(fullDesc));

			epidermisType = epidermis;
			arms.AddAt(this, _index);
		}
		internal static ArmType Deserialize(int index)
		{
			if (index < 0 || index >= arms.Count)
			{
				throw new System.ArgumentException("index for arm type deserialize out of range");
			}
			else
			{
				ArmType arm = arms[index];
				if (arm != null)
				{
					return arm;
				}
				else
				{
					throw new System.ArgumentException("index for arm type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		internal static bool Validate(ref ArmType armType, bool correctInvalidData)
		{
			if (arms.Contains(armType))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				armType = HUMAN;
			}
			return false;
		}


		//DO NOT REORDER THESE (Under penalty of death lol)
		public static readonly ToneArms HUMAN = new ToneArms(HandType.HUMAN, EpidermisType.SKIN, DefaultValueHelpers.defaultHumanTone, SkinTexture.NONDESCRIPT, true, HumanDesc, HumanSingleDesc, HumanLongDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly FurArms HARPY = new FurArms(HandType.HUMAN, EpidermisType.FEATHERS, DefaultValueHelpers.defaultHarpyFeathers, FurTexture.NONDESCRIPT, true, HarpyDesc, HarpySingleDesc, HarpyLongDesc, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly ToneArms SPIDER = new ToneArms(HandType.HUMAN, EpidermisType.CARAPACE, DefaultValueHelpers.defaultSpiderTone, SkinTexture.SHINY, false, SpiderDesc, SpiderSingleDesc, SpiderLongDesc, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr);
		public static readonly ToneArms BEE = new ToneArms(HandType.HUMAN, EpidermisType.CARAPACE, DefaultValueHelpers.defaultBeeTone, SkinTexture.SHINY, false, BeeDesc, BeeSingleDesc, BeeLongDesc, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		//I broke up predator arms to make the logic here easier. now all arms have one hand/claw type.
		//you still have the ability to check for predator arms via a function below. no functionality has been lost.
		public static readonly ToneArms DRAGON = new ToneArms(HandType.DRAGON, EpidermisType.SCALES, DefaultValueHelpers.defaultDragonTone, SkinTexture.NONDESCRIPT, true, DragonDesc, DragonSingleDesc, DragonLongDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly ToneArms IMP = new ToneArms(HandType.IMP, EpidermisType.SCALES, DefaultValueHelpers.defaultImpTone, SkinTexture.NONDESCRIPT, true, ImpDesc, ImpSingleDesc, ImpLongDesc, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly ToneArms LIZARD = new ToneArms(HandType.LIZARD, EpidermisType.SCALES, DefaultValueHelpers.defaultLizardTone, SkinTexture.NONDESCRIPT, true, LizardDesc, LizardSingleDesc, LizardLongDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly ToneArms SALAMANDER = new ToneArms(HandType.SALAMANDER, EpidermisType.SCALES, DefaultValueHelpers.defaultSalamanderTone, SkinTexture.NONDESCRIPT, false, SalamanderDesc, SalamanderSingleDesc, SalamanderLongDesc, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr);
		public static readonly FurArms WOLF = new FurArms(HandType.DOG, EpidermisType.FUR, DefaultValueHelpers.defaultDogFur, FurTexture.NONDESCRIPT, true, WolfDesc, WolfSingleDesc, WolfLongDesc, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly FurArms COCKATRICE = new CockatriceArms();
		public static readonly FurArms RED_PANDA = new FurArms(HandType.RED_PANDA, EpidermisType.FUR, DefaultValueHelpers.defaultRedPandaUnderFur, FurTexture.SOFT, false, RedPandaDesc, RedPandaSingleDesc, RedPandaLongDesc, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);
		public static readonly FurArms FERRET = new FerretArms();
		public static readonly FurArms CAT = new FurArms(HandType.CAT, EpidermisType.FUR, DefaultValueHelpers.defaultCatFur, FurTexture.NONDESCRIPT, true, CatDesc, CatSingleDesc, CatLongDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly FurArms DOG = new FurArms(HandType.DOG, EpidermisType.FUR, DefaultValueHelpers.defaultDogFur, FurTexture.NONDESCRIPT, true, DogDesc, DogSingleDesc, DogLongDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly FurArms FOX = new FurArms(HandType.FOX, EpidermisType.FUR, DefaultValueHelpers.defaultFoxFur, FurTexture.NONDESCRIPT, true, FoxDesc, FoxSingleDesc, FoxLongDesc, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		//added gooey arms - it was a weird case where claws could be goopy, and that complicates things.
		public static readonly ToneArms GOO = new ToneArms(HandType.GOO, EpidermisType.GOO, DefaultValueHelpers.defaultGooTone, SkinTexture.SLIMY, true, GooDesc, GooSingleDesc, GooLongDesc, GooFullDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		//Add new Arm Types Here.

		private sealed class FerretArms : FurArms
		{
			private readonly FurColor defaultSecondaryColor = DefaultValueHelpers.defaultFerretUnderFur;
			public FerretArms() : base(HandType.FERRET, EpidermisType.FUR, DefaultValueHelpers.defaultFerretFur, FurTexture.NONDESCRIPT,
				true, FerretDesc, FerretSingleDesc, FerretLongDesc, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr)
			{ }

			internal override EpidermalData GetPrimaryEpidermis(in BodyData bodyData)
			{
				FurColor color = defaultColor;
				if (bodyData.main.usesFurColor && !bodyData.main.fur.isEmpty)
				{
					color = bodyData.main.fur;
				}
				else if (!bodyData.activeFur.fur.isEmpty)
				{
					color = new FurColor(bodyData.activeFur.fur);
				}
				return new EpidermalData(primaryEpidermis, color, defaultTexture);
			}

			public override bool hasSecondaryFur => true;

			internal override EpidermalData GetSecondaryEpidermis(in BodyData bodyData)
			{
				FurColor color = defaultColor;
				if (bodyData.supplementary.usesFurColor && !bodyData.supplementary.fur.isEmpty)
				{
					color = bodyData.main.fur;
				}
				else if (!bodyData.activeFur.fur.isEmpty) //
				{
					color = new FurColor(bodyData.activeFur.fur);
				}

				//make sure the ferret is actually two-toned. not strictly speaking necessary, but whatever.
				EpidermalData primary = GetPrimaryEpidermis(bodyData);
				if (primary.fur.Equals(color))
				{
					color = color.Equals(defaultSecondaryColor) ? defaultColor : defaultSecondaryColor;
				}
				return new EpidermalData(primaryEpidermis, color, defaultTexture);
			}
		}

		//upper half of arm - primary color, as fur. lower half: scales, uses secondary tone.
		private sealed class CockatriceArms : FurArms
		{
			private readonly ToneBasedEpidermisType secondaryType = EpidermisType.SCALES;
			private readonly Tones defaultScales = DefaultValueHelpers.defaultCockatriceScaleTone;
			private readonly SkinTexture defaultScaleTexture = SkinTexture.NONDESCRIPT;

			public CockatriceArms() : base(HandType.COCKATRICE, EpidermisType.FEATHERS, DefaultValueHelpers.defaultCockatricePrimaryFeathers, FurTexture.NONDESCRIPT, true,
				CockatriceDesc, CockatriceSingleDesc, CockatriceLongDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr)
			{ }

			public override bool hasSecondaryTone => true;

			internal override EpidermalData GetSecondaryEpidermis(in BodyData bodyData)
			{
				Tones color = bodyData.supplementary.usesTone && !bodyData.supplementary.tone.isEmpty ? bodyData.supplementary.tone : defaultScales;
				return new EpidermalData(secondaryType, color, defaultScaleTexture);
			}

			internal override EpidermalData GetPrimaryEpidermis(in BodyData bodyData)
			{
				FurColor color = defaultColor;
				if (bodyData.type == BodyType.COCKATRICE && !bodyData.main.fur.isEmpty)
				{
					color = bodyData.main.fur;
				}
				else if (!bodyData.hairColor.isEmpty)
				{
					color = new FurColor(bodyData.hairColor);
				}
				return new EpidermalData(primaryEpidermis, color, defaultTexture);
			}
		}
		public bool IsPredatorArms()
		{
			return this == DRAGON || this == IMP || this == LIZARD;
		}

		public bool IsReptilian()
		{
			return this == SALAMANDER || this == LIZARD || this == DRAGON;
		}
	}

	public class FurArms : ArmType
	{
		public readonly FurColor defaultColor;
		public readonly FurTexture defaultTexture;
		protected FurBasedEpidermisType primaryEpidermis => (FurBasedEpidermisType)epidermisType;
		protected readonly bool mutable;


		internal FurArms(HandType hand, FurBasedEpidermisType epidermis, FurColor defaultFurColor, FurTexture defaultFurTexture, bool canChange,
			ShortPluralDescriptor shortDesc, SimpleDescriptor singleDesc, PluralPartDescriptor<ArmData> longDesc, ArmAndHandsDescriptor fullDesc, PlayerBodyPartDelegate<Arms> playerDesc,
			ChangeType<ArmData> transform, RestoreType<ArmData> restore) : base(hand, epidermis, shortDesc, singleDesc, longDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultColor = new FurColor(defaultFurColor);
			defaultTexture = defaultFurTexture;
			mutable = canChange;
		}

		public override bool hasPrimaryFur => true;
		public override bool hasPrimaryTone => false;

		internal override EpidermalData GetPrimaryEpidermis(in BodyData bodyData)
		{
			FurColor color = this.defaultColor;

			if (mutable)
			{
				if (bodyData.supplementary.usesFurColor && !bodyData.supplementary.fur.isEmpty) // can't be null
				{
					color = bodyData.supplementary.fur;
				}
				else if (bodyData.main.usesFurColor && !bodyData.main.fur.isEmpty) //can't be null
				{
					color = bodyData.main.fur;
				}
				else if (!bodyData.hairColor.isEmpty) //can't be null
				{
					color = new FurColor(bodyData.hairColor);
				}
			}
			return new EpidermalData(primaryEpidermis, color, defaultTexture);
		}


	}

	public class ToneArms : ArmType
	{
		public readonly SkinTexture defaultTexture;
		public readonly bool mutable;
		public readonly Tones defaultTone;
		protected ToneBasedEpidermisType primaryEpidermis => (ToneBasedEpidermisType)epidermisType;
		internal ToneArms(HandType hand, ToneBasedEpidermisType epidermis, Tones defTone, SkinTexture defaultSkinTexture, bool canChange,
			ShortPluralDescriptor shortDesc, SimpleDescriptor singleDesc, PluralPartDescriptor<ArmData> longDesc, ArmAndHandsDescriptor fullDesc, PlayerBodyPartDelegate<Arms> playerDesc,
			ChangeType<ArmData> transform, RestoreType<ArmData> restore) : base(hand, epidermis, shortDesc, singleDesc, longDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultTexture = defaultSkinTexture;
			defaultTone = defTone;
			mutable = canChange;
		}

		public override bool hasPrimaryFur => false;
		public override bool hasPrimaryTone => true;

		internal override EpidermalData GetPrimaryEpidermis(in BodyData bodyData)
		{
			Tones color = mutable ? bodyData.mainSkin.tone : defaultTone;
			return new EpidermalData(primaryEpidermis, color, defaultTexture);
		}
	}

	public sealed class ArmData : FullBehavioralData<ArmData, Arms, ArmType>
	{
		public readonly EpidermalData epidermis;
		public readonly EpidermalData secondaryEpidermis;

		public readonly HandData hands;

		public readonly ReadOnlyTattooablePart<ArmTattooLocation> tattoos;

		public bool usesPrimaryTone => type.hasPrimaryTone;
		public bool usesPrimaryFur => type.hasPrimaryFur;
		public bool usesSecondaryTone => type.hasSecondaryTone;
		public bool usesSecondaryFur => type.hasSecondaryFur;

		public bool usesAnyTone => usesPrimaryTone || usesSecondaryTone;
		public bool usesAnyFur => usesPrimaryFur || usesSecondaryFur;

		//description overloads.
		public string ShortDescription(bool plural) => type.ShortDescription(plural);


		public string LongDescription(bool alternateFormat, bool plural) => type.LongDescription(this, alternateFormat, plural);

		public string LongDescriptionPrimary(bool plural) => type.LongDescriptionPrimary(this, plural);

		public string LongDescriptionAlternate(bool plural) => type.LongDescriptionAlternate(this, plural);


		public string EpidermisDescription()
		{
			return ArmType.ArmEpidermisDescription(epidermis, secondaryEpidermis);
		}

		public override ArmType defaultType => ArmType.defaultValue;

		public override ArmData AsCurrentData()
		{
			return this;
		}


		public ArmData(Arms source) : base(GetID(source), GetBehavior(source))
		{
			hands = source.hands.AsReadOnlyData();
			epidermis = source.epidermis;
			secondaryEpidermis = source.secondaryEpidermis;

			tattoos = source.tattoos.AsReadOnlyData();
		}
	}

}