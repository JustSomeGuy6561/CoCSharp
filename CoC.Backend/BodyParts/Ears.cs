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
	/*
	 * Here's to looking half of this shit up. Following your ear from bottom up around the edge:
	 * LOBE, AURICLE, HELIX, ANTI_HELIX.
	 * LOBE: Lowest part of ear. AURICLE: Thin part of the outside of the ear around the middle
	 * HELIX: the thin upper outside part of your ear. ANTI-HELIX: the thin upper inside part of your ear.
	 * I had more that i looked up but i can't realisticly make them part of anything not human.
	 * I am aware everyone's ears are different and you may have tiny lobes and long-ass helix bits
	 * (which i suppose is also true for elfin ears); i did what i could, sue me.
	 * Also, each ear has a max of 14 values, as 14 is our max for one screen. right now, I use 13.
	 */
	public enum EarPiercings
	{
		LEFT_LOBE_1, LEFT_LOBE_2, LEFT_LOBE_3, LEFT_UPPER_LOBE,
		LEFT_AURICAL_1, LEFT_AURICAL_2, LEFT_AURICAL_3, LEFT_AURICAL_4,
		LEFT_HELIX_1, LEFT_HELIX_2, LEFT_HELIX_3, LEFT_HELIX_4,
		LEFT_ANTI_HELIX,
		RIGHT_LOBE_1, RIGHT_LOBE_2, RIGHT_LOBE_3, RIGHT_UPPER_LOBE,
		RIGHT_AURICAL_1, RIGHT_AURICAL_2, RIGHT_AURICAL_3, RIGHT_AURICAL_4,
		RIGHT_HELIX_1, RIGHT_HELIX_2, RIGHT_HELIX_3, RIGHT_HELIX_4,
		RIGHT_ANTI_HELIX
	}

	//lazy, so it actually wont fire any data changed events, even though the ear fur can change.
	public sealed partial class Ears : BehavioralSaveablePart<Ears, EarType, EarData>
	{
		public override string BodyPartName() => Name();

		private BodyData bodyData => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.body.AsReadOnlyData() : new BodyData(creatureID);

		private FurColor earFur => type.ParseFurColor(_earFur, bodyData);
		private readonly FurColor _earFur = new FurColor();

		public ReadOnlyFurColor earFurColor => new ReadOnlyFurColor(earFur);

		public readonly Piercing<EarPiercings> earPiercings;

		internal Ears(Guid creatureID) : this(creatureID, EarType.defaultValue) { }

		internal Ears(Guid creatureID, EarType earType) : base(creatureID)
		{
			type = earType ?? throw new ArgumentNullException(nameof(earType));
			earPiercings = new Piercing<EarPiercings>(PiercingLocationUnlocked, SupportedJewelryByLocation);
		}

		public override EarData AsReadOnlyData()
		{
			return new EarData(this);
		}

		public override EarType type { get; protected set; }

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

		public string LongDescription(bool alternateForm, bool plural) => type.LongDescription(AsReadOnlyData(), alternateForm, plural);

		public string LongDescriptionPrimary(bool plural) => type.LongDescriptionPrimary(AsReadOnlyData(), plural);

		public string LongDescriptionAlternate(bool plural) => type.LongDescriptionAlternate(AsReadOnlyData(), plural);


		private bool PiercingLocationUnlocked(EarPiercings piercingLocation)
		{
			return true;
		}

		private JewelryType SupportedJewelryByLocation(EarPiercings piercingLocation)
		{
			switch (piercingLocation)
			{
				//these are grouped by "location." so some return identical things, but it's done this way to make it easier to update it if needed.
				case EarPiercings.LEFT_LOBE_1:
				case EarPiercings.RIGHT_LOBE_1:
				case EarPiercings.LEFT_LOBE_2:
				case EarPiercings.RIGHT_LOBE_2:
					return JewelryType.BARBELL_STUD | JewelryType.DANGLER | JewelryType.HOOP | JewelryType.HORSESHOE | JewelryType.RING | JewelryType.SPECIAL;
				case EarPiercings.LEFT_LOBE_3:
				case EarPiercings.RIGHT_LOBE_3:
				case EarPiercings.LEFT_UPPER_LOBE:
				case EarPiercings.RIGHT_UPPER_LOBE:
					return JewelryType.BARBELL_STUD | JewelryType.DANGLER | JewelryType.HORSESHOE | JewelryType.RING | JewelryType.SPECIAL;
				case EarPiercings.LEFT_HELIX_1:
				case EarPiercings.RIGHT_HELIX_1:
				case EarPiercings.LEFT_HELIX_2:
				case EarPiercings.RIGHT_HELIX_2:
				case EarPiercings.LEFT_HELIX_3:
				case EarPiercings.RIGHT_HELIX_3:
					return JewelryType.BARBELL_STUD | JewelryType.DANGLER | JewelryType.HORSESHOE | JewelryType.RING | JewelryType.SPECIAL;
				case EarPiercings.LEFT_ANTI_HELIX:
				case EarPiercings.RIGHT_ANTI_HELIX:
					return JewelryType.BARBELL_STUD | JewelryType.HORSESHOE | JewelryType.RING | JewelryType.SPECIAL;
				default:
					return JewelryType.BARBELL_STUD | JewelryType.HORSESHOE | JewelryType.RING | JewelryType.SPECIAL;
			}
		}
	}

	public partial class EarType : SaveableBehavior<EarType, Ears, EarData>
	{
		private static int indexMaker = 0;
		private static readonly List<EarType> ears = new List<EarType>();
		public static readonly ReadOnlyCollection<EarType> availableTypes = new ReadOnlyCollection<EarType>(ears);
		private readonly int _index;

		public static EarType defaultValue => HUMAN;

		private readonly SimplePluralDescriptor shortPluralDesc;
		private readonly LongPluralDescriptor<EarData> longPluralDesc;

		public string ShortDescription(bool plural) => shortPluralDesc(plural);

		public string LongDescription(EarData data, bool alternateForm, bool plural) => longPluralDesc(data, alternateForm, plural);

		public string LongDescriptionPrimary(EarData data, bool plural) => longPluralDesc(data, false, plural);

		public string LongDescriptionAlternate(EarData data, bool plural) => longPluralDesc(data, true, plural);


		protected EarType(SimplePluralDescriptor shortDesc, LongPluralDescriptor<EarData> longDesc, PlayerBodyPartDelegate<Ears> playerDesc,
			ChangeType<EarData> transform, RestoreType<EarData> restore) : base(PluralHelper(shortDesc), LongPluralHelper(longDesc), playerDesc, transform, restore)
		{
			shortPluralDesc = shortDesc;
			longPluralDesc = longDesc;

			_index = indexMaker++;
			ears.AddAt(this, index);
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

		public override int index => _index;

		public static readonly EarType HUMAN = new EarType(HumanDesc, HumanLongDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly EarType HORSE = new EarType(HorseDesc, HorseLongDesc, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly EarType DOG = new EarType(DogDesc, DogLongDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly EarType COW = new EarType(CowDesc, CowLongDesc, CowPlayerStr, CowTransformStr, CowRestoreStr);
		public static readonly EarType ELFIN = new EarType(ElfinDesc, ElfinLongDesc, ElfinPlayerStr, ElfinTransformStr, ElfinRestoreStr);
		public static readonly EarType CAT = new EarType(CatDesc, CatLongDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly EarType LIZARD = new EarType(LizardDesc, LizardLongDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly EarType BUNNY = new EarType(BunnyDesc, BunnyLongDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static readonly EarType KANGAROO = new EarType(KangarooDesc, KangarooLongDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly EarType FOX = new EarType(FoxDesc, FoxLongDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		public static readonly EarType DRAGON = new EarType(DragonDesc, DragonLongDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly EarType RACCOON = new EarType(RaccoonDesc, RaccoonLongDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr);
		public static readonly EarType MOUSE = new EarType(MouseDesc, MouseLongDesc, MousePlayerStr, MouseTransformStr, MouseRestoreStr);
		public static readonly FurEarType FERRET = new FurEarType(DefaultValueHelpers.defaultFerretUnderFur, FerretDesc, FerretLongDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly EarType PIG = new EarType(PigDesc, PigLongDesc, PigPlayerStr, PigTransformStr, PigRestoreStr);
		public static readonly EarType RHINO = new EarType(RhinoDesc, RhinoLongDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr);
		public static readonly EarType ECHIDNA = new EarType(EchidnaDesc, EchidnaLongDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly EarType DEER = new EarType(DeerDesc, DeerLongDesc, DeerPlayerStr, DeerTransformStr, DeerRestoreStr);
		public static readonly EarType WOLF = new EarType(WolfDesc, WolfLongDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly EarType SHEEP = new EarType(SheepDesc, SheepLongDesc, SheepPlayerStr, SheepTransformStr, SheepRestoreStr);
		public static readonly EarType IMP = new EarType(ImpDesc, ImpLongDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly EarType COCKATRICE = new EarType(CockatriceDesc, CockatriceLongDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly EarType RED_PANDA = new EarType(RedPandaDesc, RedPandaLongDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);


	}

	public class FurEarType : EarType
	{
		public readonly FurColor defaultFur;
		public FurEarType(FurColor defaultColor, SimplePluralDescriptor shortDesc, LongPluralDescriptor<EarData> longDesc, PlayerBodyPartDelegate<Ears> playerDesc,
			ChangeType<EarData> transform, RestoreType<EarData> restore) : base(shortDesc, longDesc, playerDesc, transform, restore)
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

		public readonly ReadOnlyPiercing<EarPiercings> earPiercings;


		public string ShortDescription(bool plural) => type.ShortDescription(plural);

		public string LongDescription(bool alternateForm, bool plural) => type.LongDescription(this, alternateForm, plural);

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
		public static PiercingJewelry GenerateEarJewelry(this Ears ears, EarPiercings location, JewelryType jewelryType, JewelryMaterial jewelryMaterial)
		{
			if (ears.earPiercings.CanWearThisJewelryType(location, jewelryType))
			{
				return new GenericPiercing(jewelryType, jewelryMaterial);
			}
			return null;
		}
	}
}
