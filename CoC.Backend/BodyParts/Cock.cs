//Cock.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:55 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Tools;
using System.Collections.Generic;

namespace CoC.Backend.BodyParts
{
	public enum CockGroup { HUMAN, MAMMALIAN, CORRUPTED, AQUATIC, REPTILIAN, FLYING, OTHER }

	public enum CockPiercings
	{
		ALBERT,
		FRENUM_UPPER_1, FRENUM_UPPER_2, FRENUM_MIDDLE_1, FRENUM_MIDDLE_2,
		FRENUM_MIDDLE_3, FRENUM_MIDDLE_4, FRENUM_LOWER_1, FRENUM_LOWER_2
	}
	//whoever wrote the AS version of cock, thank you. your cock was awesome. (Obligatory: no homo)
	//i didn't have to search everywhere for the things that were part of it.
	//well, mostly. knots were still a pain.
	//whoever decided to use the AS3 equivalent hack for an enum, though, not so much. that was some ugly ass shit.

	public sealed class Cock : BehavioralSaveablePart<Cock, CockType>, IGrowShrinkable// IPerkAware.
	{
		//TODO: Make this iperkaware for big cock perk.

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

		public const JewelryType SUPPORTED_COCK_JEWELRY = JewelryType.BARBELL_STUD | JewelryType.HORSESHOE | JewelryType.RING | JewelryType.SPECIAL;

#warning Correct this when perks  are implemented.
		private bool hasBigCockPerk => SaveData.BackendSessionData.data.hasBigCockPerk;

		public float cockArea => cockGirth * cockLength;
		public float knotMultiplier
		{
			get => _knotMultiplier;
			private set
			{
				if (type.hasKnot)
				{
					_knotMultiplier = Utils.Clamp2(value, MIN_KNOT_MULTIPLIER, MAX_KNOT_MULTIPLIER);
				}
				else _knotMultiplier = 0;
			}
		}
		private float _knotMultiplier;

		public float cockLength => _cockLength;
		private float _cockLength;

		public float cockGirth => _cockGirth;
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

		public readonly Piercing<CockPiercings> cockPiercings;

		private Cock()
		{
			type = CockType.HUMAN;
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

			knotMultiplier = type.baseKnotMultiplier;

			cockPiercings = new Piercing<CockPiercings>(SUPPORTED_COCK_JEWELRY, PiercingLocationUnlocked);
		}

		private Cock(CockType cockType)
		{
			type = cockType;
			if (hasBigCockPerk)
			{
				updateLengthAndGirth(DEFAULT_BIG_COCK_LENGTH, DEFAULT_BIG_COCK_GIRTH);
			}
			else
			{
				updateLengthAndGirth(DEFAULT_COCK_LENGTH, DEFAULT_COCK_GIRTH);
			}
			knotMultiplier = type.baseKnotMultiplier;

			cockPiercings = new Piercing<CockPiercings>(SUPPORTED_COCK_JEWELRY, PiercingLocationUnlocked);
		}

		private Cock(CockType cockType, float length, float girth)
		{
			type = cockType;
			updateLengthAndGirth(length, girth);
			knotMultiplier = type.baseKnotMultiplier;

			cockPiercings = new Piercing<CockPiercings>(SUPPORTED_COCK_JEWELRY, PiercingLocationUnlocked);
		}

		public override bool isDefault => type == CockType.HUMAN;

		internal static Cock GenerateFromGender(Gender gender)
		{
			if (gender.HasFlag(Gender.MALE))
			{
				return new Cock();
			}
			else return null;
		}

		internal static Cock GenerateDefault()
		{
			return new Cock();
		}

		internal static Cock GenerateClitCock()
		{
			return new Cock(CockType.HUMAN, 5f, 1f);
		}

		internal static Cock GenerateDefaultOfType(CockType cockType)
		{
			return new Cock(cockType);
		}

		internal static Cock Generate(CockType cockType, float length, float girth, bool virginCock = true)
		{
			return new Cock(cockType, length, girth);
		}

		internal static Cock GenerateWithKnot(CockType cockType, float length, float girth, float knotMultiplier)
		{
			return new Cock(cockType, length, girth)
			{
				knotMultiplier = knotMultiplier
			};
		}

		internal override bool Restore()
		{
			if (type == CockType.HUMAN)
			{
				return false;
			}
			type = CockType.HUMAN;
			updateLengthAndGirth(DEFAULT_COCK_LENGTH, DEFAULT_COCK_GIRTH);
			return true;
		}

		internal float LengthenCock(float lengthenAmount)
		{
			float oldLength = cockLength;
			updateLength(cockLength + lengthenAmount);
			return cockLength - oldLength;
		}

		internal float ShortenCock(float shortenAmount)
		{
			float oldLength = cockLength;
			updateLength(cockLength - shortenAmount);
			return oldLength - cockLength;
		}

		internal void SetLength(float newLength)
		{
			updateLength(newLength);
		}

		internal float ThickenCock(float thickenAmount)
		{
			float oldGirth = cockGirth;
			updateGirth(cockGirth + thickenAmount);
			return cockGirth - oldGirth;
		}

		internal float ThinCock(float thinAmount)
		{
			float oldGirth = cockGirth;
			updateGirth(cockGirth - thinAmount);
			return oldGirth - cockGirth;
		}

		internal void SetGirth(float newGirth)
		{
			updateGirth(newGirth);
		}

		internal void SetLengthAndGirth(float newLength, float newGirth)
		{
			updateLengthAndGirth(newLength, newGirth);
		}


		internal override bool Validate(bool correctInvalidData)
		{
			bool valid = true;
			CockType cockType = type;
			valid = CockType.Validate(ref cockType, correctInvalidData);
			//should auto-Validate length and girth.
			type = cockType;
			if (valid || correctInvalidData)
			{
				valid &= cockPiercings.Validate(correctInvalidData);
			}
			//give length priority.
			if (valid || correctInvalidData)
			{
				updateLengthAndGirth(cockLength, cockGirth);
			}
			return valid;
		}


		private bool PiercingLocationUnlocked(CockPiercings piercingLocation)
		{
			return true;
		}

		bool IGrowShrinkable.CanReducto()
		{
			return cockArea > 6;
		}

		float IGrowShrinkable.UseReducto()
		{
			if (!((IGrowShrinkable)this).CanReducto())
			{
				return 0;
			}
			float oldLength = cockLength;
			updateLength(cockLength * 2.0f / 3);
			return oldLength - cockLength;

		}

		bool IGrowShrinkable.CanGrowPlus()
		{
			return cockLength < MAX_COCK_LENGTH;
		}

		//grows cock 1-2 inches, in increments of 0.25. 
		//automatically increases cockGirth to min value.
		//if possible, will also increase cockGirth, up to 0.5 inches
		float IGrowShrinkable.UseGroPlus()
		{
			if (!((IGrowShrinkable)this).CanGrowPlus())
			{
				return 0;
			}
			float oldCockLength = cockLength;
			if (hasBigCockPerk)
			{
				updateLength(cockLength + 1 + Utils.Rand(9) / 4.0f);
			}
			else
			{
				updateLength(cockLength + 1 + Utils.Rand(4) / 4.0f);
			}
			if ((cockGirth + 0.5f) < maxGirth)
			{
				updateGirth(cockGirth + 0.5f);
			}
			else if (cockGirth < maxGirth)
			{
				updateGirth(maxGirth);
			}
			return cockLength - oldCockLength;
		}

		private void updateLength(float newLength)
		{
			_cockLength = newLength;
			Utils.Clamp(ref _cockLength, MIN_COCK_LENGTH, MAX_COCK_LENGTH);
			Utils.Clamp(ref _cockGirth, minGirth, maxGirth);
		}

		private void updateGirth(float newGirth)
		{
			_cockGirth = newGirth;
			Utils.Clamp(ref _cockGirth, MIN_COCK_THICKNESS, MAX_COCK_THICKNESS);
			Utils.Clamp(ref _cockLength, minLength, maxLength);
		}

		private void updateLengthAndGirth(float newLength, float newGirth)
		{
			_cockLength = newLength;
			_cockGirth = newGirth;
			Utils.Clamp(ref _cockLength, MIN_COCK_LENGTH, MAX_COCK_LENGTH);
			Utils.Clamp(ref _cockGirth, minGirth, maxGirth);
		}


		private float maxGirth => _cockLength * type.maxGirthToLengthRatio;
		private float minGirth => _cockLength * type.minGirthToLengthRatio;
		private float minLength => _cockGirth / type.maxGirthToLengthRatio;
		private float maxLength => _cockGirth / type.minGirthToLengthRatio;
	}



	public partial class CockType : SaveableBehavior<CockType, Cock>
	{

		private static int indexMaker = 0;
		private static readonly List<CockType> types = new List<CockType>();
		public override int index => _index;
		private readonly int _index;

		public bool hasKnot => baseKnotMultiplier != 0f;
		public readonly float baseKnotMultiplier;

		//vanilla game limited cock width to 2/5 the length. i realize that some cocks may require that ratio be lower, so if it irks you, you
		//now have the option to change that.

		public virtual float maxGirthToLengthRatio => 0.4f;

		//similarly, you now have the option to mess with min ratio. by default, any cock will at the very least, have one inch in width per 10 inches in length.
		public virtual float minGirthToLengthRatio => 0.1f;

		//allows a straightforward way to check for flexible, extendable cocks, like tentacles. also allows other cocks to act like tentacles (see the newly added goo-cock)
		//i'm aware this breaks all the rules with length and girth, but tentacles need to tentacle things, so. 
		//I'm hanging the lampshade in the tentacle and goo descriptions, saying that they can extend to crazy lengths, but they only feel pleasure at the tip (which happens to be length long and girth wide)
		public virtual bool flexibleOrStretchyCock => false;

		private protected CockType(CockGroup cockGroup,
			SimpleDescriptor shortDesc, DescriptorWithArg<Cock> fullDesc, TypeAndPlayerDelegate<Cock> playerDesc,
			ChangeType<Cock> transform, RestoreType<Cock> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			baseKnotMultiplier = 0;
			types.AddAt(this, _index);

		}

		private protected CockType(CockGroup cockGroup, float initialKnotMultiplier, //any cocktype specific values.
			SimpleDescriptor shortDesc, DescriptorWithArg<Cock> fullDesc, TypeAndPlayerDelegate<Cock> playerDesc,
			ChangeType<Cock> transform, RestoreType<Cock> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			Utils.Clamp(ref initialKnotMultiplier, 1.1f, 2.5f);
			baseKnotMultiplier = initialKnotMultiplier;
			types.AddAt(this, _index);
		}

		internal static bool Validate(ref CockType cockType, bool correctInvalidData)
		{
			if (types.Contains(cockType))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				cockType = HUMAN;
			}
			return false;
		}

		public static readonly CockType HUMAN = new CockType(CockGroup.HUMAN, HumanDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly CockType HORSE = new CockType(CockGroup.MAMMALIAN, HorseDesc, HorseFullDesc, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly CockType DOG = new CockType(CockGroup.MAMMALIAN, 1.1f, DogDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);// can range up to 2.1 depending on item.
		public static readonly CockType DEMON = new CockType(CockGroup.CORRUPTED, DemonDesc, DemonFullDesc, DemonPlayerStr, DemonTransformStr, DemonRestoreStr);
		public static readonly CockType TENTACLE = new FlexiCock(CockGroup.CORRUPTED, 1.1f, TentacleDesc, TentacleFullDesc, TentaclePlayerStr, TentacleTransformStr, TentacleRestoreStr);
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
		public static readonly CockType GOO = new FlexiCock(CockGroup.AQUATIC, GooDesc, GooFullDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);

		private class FlexiCock : CockType
		{
			public override bool flexibleOrStretchyCock => true;
			public FlexiCock(CockGroup cockGroup, SimpleDescriptor shortDesc, DescriptorWithArg<Cock> fullDesc, TypeAndPlayerDelegate<Cock> playerDesc, 
				ChangeType<Cock> transform, RestoreType<Cock> restore) : base(cockGroup, shortDesc, fullDesc, playerDesc, transform, restore) {}

			public FlexiCock(CockGroup cockGroup, float initialKnotMultiplier, SimpleDescriptor shortDesc, DescriptorWithArg<Cock> fullDesc, TypeAndPlayerDelegate<Cock> playerDesc, 
				ChangeType<Cock> transform, RestoreType<Cock> restore) : base(cockGroup, initialKnotMultiplier, shortDesc, fullDesc, playerDesc, transform, restore) {}
		}
	}
}
