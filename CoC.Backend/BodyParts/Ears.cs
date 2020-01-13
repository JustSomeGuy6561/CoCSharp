//Ears.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 12:22 AM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Materials;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{

	public sealed partial class EarPiercingLocation : PiercingLocation, IEquatable<EarPiercingLocation>
	{
		private static readonly List<EarPiercingLocation> _allLocations = new List<EarPiercingLocation>();

		public static readonly ReadOnlyCollection<EarPiercingLocation> allLocations;

		private readonly byte index;

		static EarPiercingLocation()
		{
			allLocations = new ReadOnlyCollection<EarPiercingLocation>(_allLocations);
		}

		public EarPiercingLocation(byte index, CompatibleWith allowsJewelryOfType, SimpleDescriptor btnText, SimpleDescriptor locationDesc)
			: base(allowsJewelryOfType, btnText, locationDesc)
		{
			this.index = index;

			if (!_allLocations.Contains(this))
			{
				_allLocations.Add(this);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is EarPiercingLocation earPiercing)
			{
				return Equals(earPiercing);
			}
			else return false;
		}

		public bool Equals(EarPiercingLocation other)
		{
			return !(other is null) && other.index == index;
		}

		public override int GetHashCode()
		{
			return index.GetHashCode();
		}
		/*
		* Here's to looking half of this shit up. Following your ear from bottom up around the edge:
		* LOBE, AURICLE, HELIX, ANTI_HELIX.
		* LOBE: Lowest part of ear. AURICLE: Thin part of the outside of the ear around the middle
		* HELIX: the thin upper outside part of your ear. ANTI-HELIX: the thin upper inside part of your ear.
		* I had more that i looked up but i can't realisticly make them part of anything not human.
		* I am aware everyone's ears are different and you may have tiny lobes and long-ass helix bits
		* (which i suppose is also true for elfin ears); i did what i could, sue me.
		* Also, each ear has a max of 14 values, as 14 is our max for one screen. right now, I use 13.
		* I suppose 13 is the max, assuming you want the option to switch ears at the same time as cancel. alternatively, you could use the provided list button tool
		* to create multiple pages if you want more than can fit on one page.
		*/

		public static readonly EarPiercingLocation LEFT_LOBE_1 = new EarPiercingLocation(0, SupportedLowerLobeJewelry, LeftLobe1Button, LeftLobe1Location);
		public static readonly EarPiercingLocation LEFT_LOBE_2 = new EarPiercingLocation(1, SupportedLowerLobeJewelry, LeftLobe2Button, LeftLobe2Location);
		public static readonly EarPiercingLocation LEFT_LOBE_3 = new EarPiercingLocation(2, SupportedUpperLobeJewelry, LeftLobe3Button, LeftLobe3Location);
		public static readonly EarPiercingLocation LEFT_UPPER_LOBE = new EarPiercingLocation(3, SupportedUpperLobeJewelry, LeftUpperLobeButton, LeftUpperLobeLocation);
		public static readonly EarPiercingLocation LEFT_AURICAL_1 = new EarPiercingLocation(4, SupportedAuricalJewelry, LeftAurical1Button, LeftAurical1Location);
		public static readonly EarPiercingLocation LEFT_AURICAL_2 = new EarPiercingLocation(5, SupportedAuricalJewelry, LeftAurical2Button, LeftAurical2Location);
		public static readonly EarPiercingLocation LEFT_AURICAL_3 = new EarPiercingLocation(6, SupportedAuricalJewelry, LeftAurical3Button, LeftAurical3Location);
		public static readonly EarPiercingLocation LEFT_AURICAL_4 = new EarPiercingLocation(7, SupportedAuricalJewelry, LeftAurical4Button, LeftAurical4Location);
		public static readonly EarPiercingLocation LEFT_HELIX_1 = new EarPiercingLocation(8, SupportedHelixJewelry, LeftHelix1Button, LeftHelix1Location);
		public static readonly EarPiercingLocation LEFT_HELIX_2 = new EarPiercingLocation(9, SupportedHelixJewelry, LeftHelix2Button, LeftHelix2Location);
		public static readonly EarPiercingLocation LEFT_HELIX_3 = new EarPiercingLocation(10, SupportedHelixJewelry, LeftHelix3Button, LeftHelix3Location);
		public static readonly EarPiercingLocation LEFT_HELIX_4 = new EarPiercingLocation(11, SupportedHelixJewelry, LeftHelix4Button, LeftHelix4Location);
		public static readonly EarPiercingLocation LEFT_ANTI_HELIX = new EarPiercingLocation(12, SupportedAntiHelixJewelry, LeftAntiHelixButton, LeftAntiHelixLocation);

		public static readonly EarPiercingLocation RIGHT_LOBE_1 = new EarPiercingLocation(13, SupportedLowerLobeJewelry, RightLobe1Button, RightLobe1Location);
		public static readonly EarPiercingLocation RIGHT_LOBE_2 = new EarPiercingLocation(14, SupportedLowerLobeJewelry, RightLobe2Button, RightLobe2Location);
		public static readonly EarPiercingLocation RIGHT_LOBE_3 = new EarPiercingLocation(15, SupportedUpperLobeJewelry, RightLobe3Button, RightLobe3Location);
		public static readonly EarPiercingLocation RIGHT_UPPER_LOBE = new EarPiercingLocation(16, SupportedUpperLobeJewelry, RightUpperLobeButton, RightUpperLobeLocation);
		public static readonly EarPiercingLocation RIGHT_AURICAL_1 = new EarPiercingLocation(17, SupportedAuricalJewelry, RightAurical1Button, RightAurical1Location);
		public static readonly EarPiercingLocation RIGHT_AURICAL_2 = new EarPiercingLocation(18, SupportedAuricalJewelry, RightAurical2Button, RightAurical2Location);
		public static readonly EarPiercingLocation RIGHT_AURICAL_3 = new EarPiercingLocation(19, SupportedAuricalJewelry, RightAurical3Button, RightAurical3Location);
		public static readonly EarPiercingLocation RIGHT_AURICAL_4 = new EarPiercingLocation(20, SupportedAuricalJewelry, RightAurical4Button, RightAurical4Location);
		public static readonly EarPiercingLocation RIGHT_HELIX_1 = new EarPiercingLocation(21, SupportedHelixJewelry, RightHelix1Button, RightHelix1Location);
		public static readonly EarPiercingLocation RIGHT_HELIX_2 = new EarPiercingLocation(22, SupportedHelixJewelry, RightHelix2Button, RightHelix2Location);
		public static readonly EarPiercingLocation RIGHT_HELIX_3 = new EarPiercingLocation(23, SupportedHelixJewelry, RightHelix3Button, RightHelix3Location);
		public static readonly EarPiercingLocation RIGHT_HELIX_4 = new EarPiercingLocation(24, SupportedHelixJewelry, RightHelix4Button, RightHelix4Location);
		public static readonly EarPiercingLocation RIGHT_ANTI_HELIX = new EarPiercingLocation(25, SupportedAntiHelixJewelry, RightAntiHelixButton, RightAntiHelixLocation);

		//these are grouped by "location." so some return identical things, but it's done this way to make it easier to update it if needed.
		private static bool SupportedLowerLobeJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.BARBELL_STUD || jewelryType ==JewelryType.DANGLER || jewelryType ==JewelryType.HOOP || jewelryType ==JewelryType.HORSESHOE || jewelryType ==JewelryType.RING || jewelryType ==JewelryType.SPECIAL;
		}

		private static bool SupportedUpperLobeJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.BARBELL_STUD || jewelryType ==JewelryType.DANGLER || jewelryType ==JewelryType.HORSESHOE || jewelryType ==JewelryType.RING || jewelryType ==JewelryType.SPECIAL;
		}

		private static bool SupportedAuricalJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.BARBELL_STUD || jewelryType == JewelryType.DANGLER || jewelryType == JewelryType.HORSESHOE || jewelryType == JewelryType.RING || jewelryType == JewelryType.SPECIAL;
		}

		private static bool SupportedHelixJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.BARBELL_STUD || jewelryType ==JewelryType.DANGLER || jewelryType ==JewelryType.HORSESHOE || jewelryType ==JewelryType.RING || jewelryType ==JewelryType.SPECIAL;
		}

		private static bool SupportedAntiHelixJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.BARBELL_STUD || jewelryType ==JewelryType.HORSESHOE || jewelryType ==JewelryType.RING || jewelryType ==JewelryType.SPECIAL;
		}
	}

	public sealed class EarPiercing : Piercing<EarPiercingLocation>
	{
		public EarPiercing(PiercingUnlocked LocationUnlocked, PlayerStr playerShortDesc, PlayerStr playerLongDesc) : base(LocationUnlocked, playerShortDesc, playerLongDesc)
		{
		}

		public override int MaxPiercings => EarPiercingLocation.allLocations.Count;

		public override IEnumerable<EarPiercingLocation> availableLocations => EarPiercingLocation.allLocations;
	}

	//lazy, so it actually wont fire any data changed events, even though the ear fur can change.
	public sealed partial class Ears : BehavioralSaveablePart<Ears, EarType, EarData>
	{
		public override string BodyPartName() => Name();

		private BodyData bodyData => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.body.AsReadOnlyData() : new BodyData(creatureID);

		private FurColor earFur => type.ParseFurColor(_earFur, bodyData);
		private readonly FurColor _earFur = new FurColor();

		public ReadOnlyFurColor earFurColor => new ReadOnlyFurColor(earFur);

		public readonly EarPiercing earPiercings;

		internal Ears(Guid creatureID) : this(creatureID, EarType.defaultValue) { }

		internal Ears(Guid creatureID, EarType earType) : base(creatureID)
		{
			type = earType ?? throw new ArgumentNullException(nameof(earType));
			earPiercings = new EarPiercing(PiercingLocationUnlocked, AllEarPiercingsShort, AllEarPiercingsLong);
		}

		public override EarData AsReadOnlyData()
		{
			return new EarData(this);
		}

		public override EarType type { get; protected set; }

		//i suppose an internal ear structure can't really have piercings. i dunno, i can't think of a way to lampshade it outside of "because video games".
		//maybe we bullshit and say the surrounding part on your head is cartilage, so it can be pierced? idk man.
		public bool isInternalEar => type.isInternalEar;

		public override EarType defaultType => EarType.defaultValue;

		//update, restore both fine as defaults.

		internal void Reset()
		{
			Restore();
			earPiercings.Reset();
		}

		internal override bool Validate(bool correctInvalidData)
		{
			EarType earType = type;
			bool valid = EarType.Validate(ref earType, correctInvalidData);
			type = earType;
			if (valid || correctInvalidData)
			{
				valid &= earPiercings.Validate(correctInvalidData);
			}
			return valid;
		}

		public string ShortDescription(bool plural) => type.ShortDescription(plural);

		public string LongDescription(bool alternateFormat, bool plural) => type.LongDescription(AsReadOnlyData(), alternateFormat, plural);

		public string LongDescriptionPrimary(bool plural) => type.LongDescriptionPrimary(AsReadOnlyData(), plural);

		public string LongDescriptionAlternate(bool plural) => type.LongDescriptionAlternate(AsReadOnlyData(), plural);


		private bool PiercingLocationUnlocked(EarPiercingLocation piercingLocation, out string whyNot)
		{
			whyNot = null;
			return true;
		}
	}

	public partial class EarType : SaveableBehavior<EarType, Ears, EarData>
	{
		private static int indexMaker = 0;
		private static readonly List<EarType> ears = new List<EarType>();
		public static readonly ReadOnlyCollection<EarType> availableTypes = new ReadOnlyCollection<EarType>(ears);
		private readonly int _index;

		public static EarType defaultValue => HUMAN;

		private readonly ShortPluralDescriptor shortPluralDesc;
		private readonly PluralPartDescriptor<EarData> longPluralDesc;

		public string ShortDescription(bool plural) => shortPluralDesc(plural);

		public string LongDescription(EarData data, bool alternateFormat, bool plural) => longPluralDesc(data, alternateFormat, plural);

		public string LongDescriptionPrimary(EarData data, bool plural) => longPluralDesc(data, false, plural);

		public string LongDescriptionAlternate(EarData data, bool plural) => longPluralDesc(data, true, plural);

		public readonly bool isInternalEar;

		protected EarType(bool internalEar, ShortPluralDescriptor shortDesc, SimpleDescriptor singleItemDesc, PluralPartDescriptor<EarData> longDesc,
			PlayerBodyPartDelegate<Ears> playerDesc, ChangeType<EarData> transform, RestoreType<EarData> restore)
			: base(PluralHelper(shortDesc), singleItemDesc, LongPluralHelper(longDesc), playerDesc, transform, restore)
		{
			shortPluralDesc = shortDesc;
			longPluralDesc = longDesc;

			_index = indexMaker++;
			ears.AddAt(this, id);

			isInternalEar = internalEar;
		}

		internal virtual FurColor ParseFurColor(FurColor current, in BodyData data)
		{
			return current;
		}

		internal static EarType Deserialize(int index)
		{
			if (index < 0 || index >= ears.Count)
			{
				throw new System.ArgumentException("index for back type deserialize out of range");
			}
			else
			{
				EarType ear = ears[index];
				if (ear != null)
				{
					return ear;
				}
				else
				{
					throw new System.ArgumentException("index for ear type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		internal static bool Validate(ref EarType earType, bool correctInvalidData)
		{
			if (ears.Contains(earType))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				earType = HUMAN;
			}
			return false;
		}

		public override int id => _index;

		public static readonly EarType HUMAN = new EarType(false, HumanDesc, HumanSingleDesc, HumanLongDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly EarType HORSE = new EarType(false, HorseDesc, HorseSingleDesc, HorseLongDesc, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly EarType DOG = new EarType(false, DogDesc, DogSingleDesc, DogLongDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly EarType COW = new EarType(false, CowDesc, CowSingleDesc, CowLongDesc, CowPlayerStr, CowTransformStr, CowRestoreStr);
		public static readonly EarType ELFIN = new EarType(false, ElfinDesc, ElfinSingleDesc, ElfinLongDesc, ElfinPlayerStr, ElfinTransformStr, ElfinRestoreStr);
		public static readonly EarType CAT = new EarType(false, CatDesc, CatSingleDesc, CatLongDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly EarType LIZARD = new EarType(true, LizardDesc, LizardSingleDesc, LizardLongDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly EarType BUNNY = new EarType(false, BunnyDesc, BunnySingleDesc, BunnyLongDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static readonly EarType KANGAROO = new EarType(false, KangarooDesc, KangarooSingleDesc, KangarooLongDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly EarType FOX = new EarType(false, FoxDesc, FoxSingleDesc, FoxLongDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		public static readonly EarType DRAGON = new EarType(true, DragonDesc, DragonSingleDesc, DragonLongDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly EarType RACCOON = new EarType(false, RaccoonDesc, RaccoonSingleDesc, RaccoonLongDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr);
		public static readonly EarType MOUSE = new EarType(false, MouseDesc, MouseSingleDesc, MouseLongDesc, MousePlayerStr, MouseTransformStr, MouseRestoreStr);
		public static readonly FurEarType FERRET = new FurEarType(false, DefaultValueHelpers.defaultFerretUnderFur, FerretDesc, FerretSingleDesc, FerretLongDesc, FerretPlayerStr,
			FerretTransformStr, FerretRestoreStr);
		public static readonly EarType PIG = new EarType(false, PigDesc, PigSingleDesc, PigLongDesc, PigPlayerStr, PigTransformStr, PigRestoreStr);
		public static readonly EarType RHINO = new EarType(false, RhinoDesc, RhinoSingleDesc, RhinoLongDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr);
		public static readonly EarType ECHIDNA = new EarType(true, EchidnaDesc, EchidnaSingleDesc, EchidnaLongDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly EarType DEER = new EarType(false, DeerDesc, DeerSingleDesc, DeerLongDesc, DeerPlayerStr, DeerTransformStr, DeerRestoreStr);
		public static readonly EarType WOLF = new EarType(false, WolfDesc, WolfSingleDesc, WolfLongDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly EarType SHEEP = new EarType(false, SheepDesc, SheepSingleDesc, SheepLongDesc, SheepPlayerStr, SheepTransformStr, SheepRestoreStr);
		public static readonly EarType IMP = new EarType(false, ImpDesc, ImpSingleDesc, ImpLongDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly EarType COCKATRICE = new EarType(true, CockatriceDesc, CockatriceSingleDesc, CockatriceLongDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly EarType RED_PANDA = new EarType(false, RedPandaDesc, RedPandaSingleDesc, RedPandaLongDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);


	}

	public class FurEarType : EarType
	{
		public readonly FurColor defaultFur;
		public FurEarType(bool internalEar, FurColor defaultColor, ShortPluralDescriptor shortDesc, SimpleDescriptor singleItemDesc, PluralPartDescriptor<EarData> longDesc,
			PlayerBodyPartDelegate<Ears> playerDesc, ChangeType<EarData> transform, RestoreType<EarData> restore)
			: base(internalEar, shortDesc, singleItemDesc, longDesc, playerDesc, transform, restore)
		{
			defaultFur = defaultColor;
		}

		internal override FurColor ParseFurColor(FurColor current, in BodyData bodyData)
		{
			FurColor color = defaultFur;
			if (!bodyData.main.fur.isEmpty)
			{
				color = bodyData.main.fur;
			}
			current.UpdateFurColor(color);
			return current;
		}
	}

	public sealed class EarData : BehavioralSaveablePartData<EarData, Ears, EarType>
	{
		public readonly ReadOnlyFurColor earFurColor;

		public readonly ReadOnlyPiercing<EarPiercingLocation> earPiercings;

		public bool isInternalEar => type.isInternalEar;

		public string ShortDescription(bool plural) => type.ShortDescription(plural);

		public string LongDescription(bool alternateFormat, bool plural) => type.LongDescription(this, alternateFormat, plural);

		public string LongDescriptionPrimary(bool plural) => type.LongDescriptionPrimary(this, plural);

		public string LongDescriptionAlternate(bool plural) => type.LongDescriptionAlternate(this, plural);


		public override EarData AsCurrentData()
		{
			return this;
		}

		internal EarData(Ears source) : base(GetID(source), GetBehavior(source))
		{
			earFurColor = source.earFurColor;

			earPiercings = source.earPiercings.AsReadOnlyData();
		}
	}

	public static class EarHelpers
	{
		public static PiercingJewelry GenerateEarJewelry(this Ears ears, EarPiercingLocation location, JewelryType jewelryType, JewelryMaterial jewelryMaterial)
		{
			if (ears.earPiercings.CanWearGenericJewelryOfType(location, jewelryType))
			{
				return new GenericPiercing(jewelryType, jewelryMaterial);
			}
			return null;
		}
	}
}
