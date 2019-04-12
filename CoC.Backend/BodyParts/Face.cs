using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Races;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;

namespace CoC.Backend.BodyParts
{
	public enum LipPiercingLocation { LABRET, MEDUSA, MONROE_LEFT, MONROE_RIGHT, LOWER_LEFT_1, LOWER_LEFT_2, LOWER_RIGHT_1, LOWER_RIGHT_2 }
	public enum EyebrowPiercingLocation { LEFT_1, LEFT_2, RIGHT_1, RIGHT_2 }
	public enum NosePiercingLocation { LEFT_NOSTRIL, RIGHT_NOSTRIL, SEPTIMUS, BRIDGE }


	public sealed class Face : BehavioralSaveablePart<Face, FaceType>, IBodyAware
	{
		private const JewelryType SUPPORTED_LIP_JEWELRY = JewelryType.CURVED_BARBELL | JewelryType.STUD | JewelryType.RING;
		private const JewelryType SUPPORTED_NOSE_JEWELRY = JewelryType.STUD | JewelryType.RING | JewelryType.CURVED_BARBELL;
		private const JewelryType SUPPORTED_EYEBROW_JEWELRY = JewelryType.STUD | JewelryType.CURVED_BARBELL | JewelryType.RING;

		public readonly Piercing<LipPiercingLocation> lipPiercings;
		public readonly Piercing<NosePiercingLocation> nosePiercings;
		public readonly Piercing<EyebrowPiercingLocation> eyebrowPiercings;

		public override FaceType type { get; protected set; }
		public bool isFullMorph { get; private set; }

		private Epidermis epidermis => type.ParseEpidermis(bodyData(), isFullMorph);
		private Epidermis secondaryEpidermis => type.ParseSecondaryEpidermis(bodyData(), isFullMorph);


		private Face()
		{
			type = FaceType.HUMAN;
			isFullMorph = false;
			lipPiercings = new Piercing<LipPiercingLocation>(SUPPORTED_LIP_JEWELRY, LipPiercingUnlocked);
			nosePiercings = new Piercing<NosePiercingLocation>(SUPPORTED_NOSE_JEWELRY, NosePiercingUnlocked);
			eyebrowPiercings = new Piercing<EyebrowPiercingLocation>(SUPPORTED_EYEBROW_JEWELRY, EyebrowPiercingUnlocked);
		}

		public override bool isDefault => type == FaceType.HUMAN;


		internal static Face GenerateDefault()
		{
			return new Face();
		}

		internal static Face GenerateDefaultOfType(FaceType faceType)
		{
			return new Face()
			{
				type = faceType
			};
		}

		internal static Face GenerateWithMorph(FaceType faceType, bool fullMorph)
		{
			return new Face()
			{
				type = faceType,
				isFullMorph = faceType.hasSecondLevel ? fullMorph : false
			};
		}

		internal bool UpdateFace(FaceType faceType)
		{
			if (type == faceType)
			{
				return false;
			}
			type = faceType;
			isFullMorph = type.MorphStrengthPostTransform(isFullMorph);
			return true;
		}

		internal bool UpdateFaceWithMorph(FaceType faceType, bool fullMorph)
		{
			if (type == faceType)
			{
				return false;
			}
			type = faceType;
			isFullMorph = type.hasSecondLevel ? fullMorph : false;
			return true;
		}

		internal bool StrengthenFacialMorph()
		{
			isFullMorph = type.hasSecondLevel;
			return isFullMorph;
		}

		internal bool WeakenFacialMorph(bool restoreIfAlreadyLevelOne = true)
		{
			//if full morph, weaken it to half-morph level. 
			if (isFullMorph)
			{
				isFullMorph = false;
				return true;
			}
			//otherwise, if we're not human and we are to restore back to human on weaken
			else if (restoreIfAlreadyLevelOne && type != FaceType.HUMAN)
			{
				type = FaceType.HUMAN;
				return true;
			}
			//we are already human or are told not to convert to human and at the weakest level.
			return false;
		}

		internal bool UpdateOrStrengthenFace(FaceType faceType, bool forceFullMorph = false)
		{
			if (type == faceType)
			{
				return StrengthenFacialMorph();
			}
			else return UpdateFaceWithMorph(faceType, forceFullMorph);
		}



		internal override bool Restore()
		{
			if (type == FaceType.HUMAN)
			{
				return false;
			}
			type = FaceType.HUMAN;
			return true;
		}

		internal void Reset()
		{
			type = FaceType.HUMAN;
			isFullMorph = false;
			lipPiercings.Reset();
			nosePiercings.Reset();
			eyebrowPiercings.Reset();
		}

		private bool LipPiercingUnlocked(LipPiercingLocation piercingLocation)
		{
			return true;
		}
		private bool NosePiercingUnlocked(NosePiercingLocation piercingLocation)
		{
			return true;
		}

		private bool EyebrowPiercingUnlocked(EyebrowPiercingLocation piercingLocation)
		{
			return true;
		}

		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			bool valid = true;

			FaceType faceType = type;
			bool fullMorph = isFullMorph;
			valid = FaceType.Validate(ref faceType, ref fullMorph);
			valid &= ValidatePiercing(valid, nosePiercings, correctDataIfInvalid);
			valid &= ValidatePiercing(valid, eyebrowPiercings, correctDataIfInvalid);
			valid &= ValidatePiercing(valid, lipPiercings, correctDataIfInvalid);
			return valid;
		}
		private bool ValidatePiercing<T>(bool valid, Piercing<T> piercing, bool correctInvalidData) where T : Enum
		{
			if (!valid && !correctInvalidData)
			{
				return false;
			}
			return piercing.Validate(correctInvalidData);
		}

		void IBodyAware.GetBodyData(BodyDataGetter getter)
		{
			bodyData = getter;
		}
		private BodyDataGetter bodyData;
	}


	public abstract partial class FaceType : SaveableBehavior<FaceType, Face>
	{
		private static List<FaceType> faces = new List<FaceType>();
		private static int indexMaker = 0;

		public readonly bool hasSecondLevel;
		public readonly SimpleDescriptor secondLevelShortDescription;
		public readonly EpidermisType epidermisType;

		public SimpleDescriptor weakenTransformText => () => morphText(false);
		public SimpleDescriptor strengthenTransformText => () => morphText(true);

		private readonly DescriptorWithArg<bool> morphText;

		private protected FaceType(EpidermisType epidermisType, SimpleDescriptor firstLevelShortDesc, SimpleDescriptor secondLevelShortDesc, DescriptorWithArg<bool> strengthenWeakenMorphText, 
			DescriptorWithArg<Face> fullDesc, TypeAndPlayerDelegate<Face> playerStr, ChangeType<Face> transform,
			RestoreType<Face> restore) : base(firstLevelShortDesc, fullDesc, playerStr, transform, restore)
		{
			_index = indexMaker++;
			secondLevelShortDescription = secondLevelShortDesc;
			morphText = strengthenWeakenMorphText;
			faces.AddAt(this, _index);
		}

		private protected FaceType(EpidermisType epidermisType, //only one level short desc
			SimpleDescriptor shortDesc, DescriptorWithArg<Face> fullDesc, TypeAndPlayerDelegate<Face> playerStr,
			ChangeType<Face> transform, RestoreType<Face> restore) : base(shortDesc, fullDesc, playerStr, transform, restore)
		{
			_index = indexMaker++;
			secondLevelShortDescription = GlobalStrings.None;
			morphText = x => "";
			faces.AddAt(this, _index);
		}
		public virtual bool isHumanoid(bool isSecondLevel)
		{
			if (hasSecondLevel)
			{
				return isSecondLevel;
			}
			return false;
		}

		public virtual bool MorphStrengthPostTransform(bool previousWasFullMorph)
		{
			return false;
		}


		internal abstract Epidermis ParseEpidermis(BodyData bodyData, bool isFullMorph);
		internal virtual Epidermis ParseSecondaryEpidermis(BodyData bodyData, bool isFullMorph)
		{
			return Epidermis.GenerateEmpty();
		}

		public override int index => _index;
		private readonly int _index;

		internal static bool Validate(ref FaceType faceType, ref bool isFullMorph, bool correctInvalidData = false)
		{
			bool valid = true;
			if (!faces.Contains(faceType))
			{
				if (!correctInvalidData)
				{
					return false;
				}
				valid = false;
				faceType = HUMAN;
			}
			if (isFullMorph && !faceType.hasSecondLevel)
			{
				valid = false;
				if (correctInvalidData)
				{
					isFullMorph = false;
				}
			}
			return valid;
		}


		public static readonly FaceType HUMAN = new ToneFace(EpidermisType.SKIN, HumanShortDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly FaceType HORSE = new FurFace(EpidermisType.FUR, Species.HORSE.defaultFacialFur, HorseShortDesc, HorseFullDesc, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly FaceType DOG = new FurFace(EpidermisType.FUR, Species.DOG.defaultFacialFur, DogShortDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly FaceType COW_MINOTAUR = new FurFace(EpidermisType.FUR, Species.COW.defaultFacialFur, CowShortDesc, MinotaurShortDesc, CowMorphText, MinotaurFullDesc, MinotaurPlayerStr, MinotaurTransformStr, MinotaurRestoreStr);
		public static readonly FaceType SHARK = new SharkFace();
		public static readonly FaceType SNAKE = new SnakeFace();
		public static readonly FaceType CAT = new FurFace(EpidermisType.FUR, Species.CAT.defaultFacialFur, CatGirlShortDesc, CatMorphShortDesc, CatMorphText, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly FaceType LIZARD = new ToneFace(EpidermisType.SCALES, LizardShortDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly FaceType BUNNY = new FurFace(EpidermisType.FUR, Species.BUNNY.defaultFacialFur, BunnyFirstLevelShortDesc, BunnySecondLevelShortDesc, BunnyMorphText, BunnyFullDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static readonly FaceType KANGAROO = new FurFace(EpidermisType.FUR, Species.KANGAROO.defaultFacialFur, KangarooShortDesc, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly FaceType SPIDER = new SpiderFace();
		public static readonly FaceType FOX = new FoxFace();
		public static readonly FaceType DRAGON = new ToneFace(EpidermisType.SCALES, DragonShortDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly FaceType RACCOON = new FurFace(EpidermisType.FUR, Species.RACCOON.defaultFacialFur, RaccoonMaskShortDesc, RaccoonFaceShortDesc, RaccoonMorphText, RaccoonFullDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr);
		public static readonly FaceType MOUSE = new FurFace(EpidermisType.FUR, Species.MOUSE.defaultFacialFur, MouseTeethShortDesc, MouseFaceShortDesc, MouseMorphText, MouseFullDesc, MousePlayerStr, MouseTransformStr, MouseRestoreStr);
		public static readonly FaceType FERRET = new MultiFurFace(EpidermisType.FUR, Species.FERRET.defaultFacialFur, Species.FERRET.defaultSecondaryFacialFur, FerretMaskShortDesc, FerretFaceShortDesc, FerretMorphText, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly FaceType PIG = new PigFace(); //both pig and boar are not humanoid.
		public static readonly FaceType RHINO = new ToneFace(EpidermisType.SKIN, RhinoShortDesc, RhinoFullDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr);
		public static readonly FaceType ECHIDNA = new FurFace(EpidermisType.FUR, Species.ECHIDNA.defaultFacialFur, EchidnaShortDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly FaceType DEER = new FurFace(EpidermisType.FUR, Species.DEER.defaultFacialFur, DeerShortDesc, DeerFullDesc, DeerPlayerStr, DeerTransformStr, DeerRestoreStr);
		public static readonly FaceType WOLF = new FurFace(EpidermisType.FUR, Species.WOLF.defaultFacialFur, WolfShortDesc, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly FaceType COCKATRICE = new CockatriceFace();
		public static readonly FaceType RED_PANDA = new MultiFurFace(EpidermisType.FUR, Species.RED_PANDA.defaultFur, Species.RED_PANDA.defaultFaceEarTailFur, PandaShortDesc, PandaFullDesc, PandaPlayerStr, PandaTransformStr, PandaRestoreStr);
		//placeholder.
		public static readonly FaceType BEAK = new FurFace(EpidermisType.FEATHERS, new FurColor(HairFurColors.WHITE), BeakShortDesc, BeakFullDesc, BeakPlayerStr, BeakTransformStr, BeakRestoreStr);

		private class ToneFace : FaceType
		{
			public ToneFace(EpidermisType epidermisType, SimpleDescriptor shortDesc, DescriptorWithArg<Face> fullDesc, TypeAndPlayerDelegate<Face> playerStr,
				ChangeType<Face> transform, RestoreType<Face> restore) : base(epidermisType, shortDesc, fullDesc, playerStr, transform, restore) { }

			public ToneFace(EpidermisType epidermisType, SimpleDescriptor firstLevelShortDesc, SimpleDescriptor secondLevelShortDesc, DescriptorWithArg<bool> strengthenWeakenMorphText, 
				DescriptorWithArg<Face> fullDesc, TypeAndPlayerDelegate<Face> playerStr, ChangeType<Face> transform, RestoreType<Face> restore) 
				: base(epidermisType, firstLevelShortDesc, secondLevelShortDesc, strengthenWeakenMorphText, fullDesc, playerStr, transform, restore)
			{
			}

			internal override Epidermis ParseEpidermis(BodyData bodyData, bool isFullMorph)
			{
				//primary epidermis is guarenteed to have a skin tone.
				Epidermis retVal = Epidermis.Generate(bodyData.primary);
				retVal.UpdateEpidermis(epidermisType); //if it's already this type it'll return false, but we dont really care.
				return retVal;
			}
		}

		private class MultiToneFace : ToneFace
		{
			private readonly Tones defaultSecondaryTone;

			public MultiToneFace(Tones secondaryToneFallback,
				EpidermisType epidermisType, SimpleDescriptor shortDesc, DescriptorWithArg<Face> fullDesc, TypeAndPlayerDelegate<Face> playerStr,
				ChangeType<Face> transform, RestoreType<Face> restore) : base(epidermisType, shortDesc, fullDesc, playerStr, transform, restore)
			{
				defaultSecondaryTone = secondaryToneFallback;
			}

			public MultiToneFace(Tones secondaryToneFallback,
				EpidermisType epidermisType, SimpleDescriptor firstLevelShortDesc, SimpleDescriptor secondLevelShortDesc, DescriptorWithArg<bool> strengthenWeakenMorphText,
				DescriptorWithArg<Face> fullDesc, TypeAndPlayerDelegate<Face> playerStr, ChangeType<Face> transform, RestoreType<Face> restore)
				: base(epidermisType, firstLevelShortDesc, secondLevelShortDesc, strengthenWeakenMorphText, fullDesc, playerStr, transform, restore)
			{
				defaultSecondaryTone = secondaryToneFallback;
			}

			internal override Epidermis ParseSecondaryEpidermis(BodyData bodyData, bool isFullMorph)
			{
				Epidermis retVal = Epidermis.GenerateDefaultOfType(epidermisType);
				retVal.ChangeTone(!Tones.isNullOrEmpty(bodyData.secondary.tone) ? bodyData.secondary.tone : defaultSecondaryTone);
				return retVal;
			}
		}

		private class FurFace : FaceType
		{
			public readonly FurColor defaultColor;

			public FurFace(FurBasedEpidermisType epidermisType, FurColor fallbackColor,
				SimpleDescriptor shortDesc, DescriptorWithArg<Face> fullDesc, TypeAndPlayerDelegate<Face> playerStr, ChangeType<Face> transform,
				RestoreType<Face> restore) : base(epidermisType, shortDesc, fullDesc, playerStr, transform, restore)
			{
				defaultColor = fallbackColor;
			}

			public FurFace(FurBasedEpidermisType epidermisType, FurColor fallbackColor,
				SimpleDescriptor firstLevelShortDesc, SimpleDescriptor secondLevelShortDesc, DescriptorWithArg<bool> strengthenWeakenMorphText, DescriptorWithArg<Face> fullDesc, TypeAndPlayerDelegate<Face> playerStr,
				ChangeType<Face> transform, RestoreType<Face> restore) : base(epidermisType, firstLevelShortDesc, secondLevelShortDesc, strengthenWeakenMorphText, fullDesc, playerStr, transform, restore)
			{
				defaultColor = fallbackColor;
			}

			internal override Epidermis ParseEpidermis(BodyData bodyData, bool isFullMorph)
			{
				FurColor color = defaultColor;
				if (!FurColor.IsNullOrEmpty(bodyData.primary.fur)) //probably never null, but w/e i'll be safe.
				{
					color = bodyData.primary.fur;
				}
				else if (!HairFurColors.isNullOrEmpty(bodyData.hairColor))
				{
					color = new FurColor(bodyData.hairColor);
				}
				Epidermis retVal = Epidermis.GenerateDefaultOfType(epidermisType);
				retVal.ChangeFur(color);
				retVal.ChangeTone(bodyData.primary.tone);
				return retVal;
			}
		}

		private class MultiFurFace : FurFace
		{
			private readonly FurColor secondaryDefaultColor;

			public MultiFurFace(FurBasedEpidermisType epidermisType, FurColor fallbackColor, FurColor secondaryFallbackColor,
				SimpleDescriptor shortDesc, DescriptorWithArg<Face> fullDesc, TypeAndPlayerDelegate<Face> playerStr, ChangeType<Face> transform,
				RestoreType<Face> restore) : base(epidermisType, fallbackColor, shortDesc, fullDesc, playerStr, transform, restore)
			{
				secondaryDefaultColor = secondaryFallbackColor;
			}

			public MultiFurFace(FurBasedEpidermisType epidermisType, FurColor fallbackColor, FurColor secondaryFallbackColor,
				SimpleDescriptor firstLevelShortDesc, SimpleDescriptor secondLevelShortDesc, DescriptorWithArg<bool> strengthenWeakenMorphText,
				DescriptorWithArg<Face> fullDesc, TypeAndPlayerDelegate<Face> playerStr, ChangeType<Face> transform, RestoreType<Face> restore)
				: base(epidermisType, fallbackColor, firstLevelShortDesc, secondLevelShortDesc, strengthenWeakenMorphText, fullDesc, playerStr, transform, restore)
			{
				secondaryDefaultColor = secondaryFallbackColor;
			}

			internal override Epidermis ParseSecondaryEpidermis(BodyData bodyData, bool isFullMorph)
			{
				Epidermis retVal = Epidermis.GenerateDefaultOfType(epidermisType);
				FurColor color = secondaryDefaultColor;
				//make sure the fur color is not the same as the primary, and that it is valid.s
				if (!FurColor.IsNullOrEmpty(bodyData.secondary.fur) && bodyData.secondary.fur != bodyData.primary.fur)
				{
					color = bodyData.secondary.fur;
				}
				//check if the hair isn't empty. further, make sure the primary color isn't also using the hair color.
				else if (!HairFurColors.isNullOrEmpty(bodyData.hairColor) && !FurColor.IsNullOrEmpty(bodyData.primary.fur))
				{
					color = new FurColor(bodyData.hairColor);
				}

				retVal.ChangeFur(color);
				return retVal;
			}
		}

		private sealed class PigFace : FurFace
		{
			public PigFace() : base(EpidermisType.FUR, Species.PIG.defaultFacialFur, PigShortDesc, BoarShortDesc, PigMorphText, PigFullDesc, PigPlayerStr, PigTransformStr, PigRestoreStr) { }

			public override bool isHumanoid(bool isSecondLevel)
			{
				return false;
			}
		}
		private sealed class SpiderFace : ToneFace
		{
			public SpiderFace() : base(EpidermisType.SKIN, SpiderShortDesc, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr) { }

			public override bool isHumanoid(bool isSecondLevel)
			{
				return true;
			}
		}
		private sealed class SharkFace : ToneFace
		{
			public SharkFace() : base(EpidermisType.SKIN, SharkShortDesc, SharkFullDesc, SharkPlayerStr, SharkTransformStr, SharkRestoreStr) { }

			public override bool isHumanoid(bool isSecondLevel)
			{
				return true;
			}
		}
		private sealed class SnakeFace : ToneFace
		{
			public SnakeFace() : base(EpidermisType.SCALES, SnakeShortDesc, SnakeFullDesc, SnakePlayerStr, SnakeTransformStr, SnakeRestoreStr) { }

			public override bool isHumanoid(bool isSecondLevel)
			{
				return true;
			}
		}

		private sealed class CockatriceFace : FurFace
		{
			private readonly EpidermisType secondaryEpidermis = EpidermisType.SCALES;
			private readonly Tones defaultUnderTone = Species.COCKATRICE.defaultScaleTone;
			public CockatriceFace() : base(EpidermisType.FEATHERS, Species.COCKATRICE.defaultPrimaryFeathers, CockatriceShortDesc, 
				CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr) {}

			internal override Epidermis ParseSecondaryEpidermis(BodyData bodyData, bool isFullMorph)
			{
				Epidermis retVal = Epidermis.GenerateDefaultOfType(secondaryEpidermis);
				retVal.ChangeTone(!Tones.isNullOrEmpty(bodyData.secondary.tone) ? bodyData.secondary.tone : defaultUnderTone);
				return retVal;
			}
		}

		private sealed class FoxFace : FurFace
		{
			private FurColor defaultKitsuneFur => Species.KITSUNE.defaultFacialFur;
			public FoxFace() : base(EpidermisType.FUR, Species.FOX.defaultFacialFur, KitsuneShortDesc, FoxShortDesc, FoxMorphText, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr)	{}

			internal override Epidermis ParseEpidermis(BodyData bodyData, bool isFullMorph)
			{
				FurColor color = isFullMorph ? defaultColor : defaultKitsuneFur;
				if (!FurColor.IsNullOrEmpty(bodyData.primary.fur)) //probably never null, but w/e i'll be safe.
				{
					color = bodyData.primary.fur;
				}
				else if (!HairFurColors.isNullOrEmpty(bodyData.hairColor))
				{
					color = new FurColor(bodyData.hairColor);
				}
				Epidermis retVal = Epidermis.GenerateDefaultOfType(epidermisType);
				retVal.ChangeFur(color);
				retVal.ChangeTone(bodyData.primary.tone);
				return retVal;
			}
		}
	}
}
