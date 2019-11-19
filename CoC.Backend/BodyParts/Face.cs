//Face.cs
//Description:
//Author: JustSomeGuy
//4/24/2019, 1:12 AM
using CoC.Backend.Attacks;
using CoC.Backend.Attacks.BodyPartAttacks;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Materials;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	//ngl, i can never recall if medusa is top and labret bottom, or if it's the opposite. 
	public enum LipPiercingLocation { LABRET, MEDUSA, MONROE_LEFT, MONROE_RIGHT, LOWER_LEFT_1, LOWER_LEFT_2, LOWER_RIGHT_1, LOWER_RIGHT_2 }

	public enum EyebrowPiercingLocation { LEFT_1, LEFT_2, RIGHT_1, RIGHT_2 }
	public enum NosePiercingLocation { LEFT_NOSTRIL, RIGHT_NOSTRIL, SEPTIMUS, BRIDGE }


	/*
	 * Faces are another instance of shit that breaks the epidermis stores only one things rule. You can have a two-tone face, but theres also skin underneath,
	 * and for some reason you need the skin tone to describe the face in detail. IDK, man. Regardless, you have the whole body data, just use that. if you need all three
	 * for something not in the face itself, it's available, i guess. We're not firing when it changes though.
	 */
	public sealed partial class Face : BehavioralSaveablePart<Face, FaceType, FaceData>, ILotionable, ICanAttackWith
	{

		public override string BodyPartName() => Name();

		private const JewelryType SUPPORTED_LIP_JEWELRY = JewelryType.HORSESHOE | JewelryType.BARBELL_STUD | JewelryType.RING | JewelryType.SPECIAL;
		//private const JewelryType SUPPORTED_NOSE_JEWELRY = JewelryType.BARBELL_STUD | JewelryType.RING | JewelryType.HORSESHOE; //can't use as nose is a pain.
		private const JewelryType SUPPORTED_EYEBROW_JEWELRY = JewelryType.BARBELL_STUD | JewelryType.HORSESHOE | JewelryType.RING | JewelryType.SPECIAL;

		public readonly Piercing<LipPiercingLocation> lipPiercings;
		public readonly Piercing<NosePiercingLocation> nosePiercings;
		public readonly Piercing<EyebrowPiercingLocation> eyebrowPiercings;

		private BodyData bodyData => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.body.AsReadOnlyData() : new BodyData(creatureID);

		public override FaceType type
		{
			get => _type;
			protected set
			{
				_type = value;
				isFullMorph = _type.MorphStrengthPostTransform(isFullMorph);
			}
		}
		private FaceType _type;

		public bool isFullMorph { get; private set; }


		public EpidermalData primary => type.ParseEpidermis(bodyData, isFullMorph, skinTexture);
		public EpidermalData secondary => type.ParseSecondaryEpidermis(bodyData, isFullMorph, skinTexture);
		public SkinTexture skinTexture
		{
			get => _skinTexture;
			private set
			{
				if (Enum.IsDefined(typeof(SkinTexture), value))
				{
					_skinTexture = value;
				}
			}
		}
		private SkinTexture _skinTexture = SkinTexture.NONDESCRIPT;

		public Tones epidermisTone => bodyData.mainSkin.tone;

		public uint oralCount { get; private set; } = 0;
		public uint orgasmCount { get; private set; } = 0;
		public uint dryOrgasmCount { get; private set; } = 0;

		//private GameDateTime timeLastIngestedCum;
		//public int hoursSinceIngestedCum => timeLastIngestedCum.hoursToNow();
		//public float lastCumIngestAmount { get; private set; } = 0;

		internal Face(Guid creatureID) : this(creatureID, FaceType.defaultValue)
		{ }

		internal Face(Guid creatureID, FaceType faceType) : base(creatureID)
		{
			type = faceType ?? throw new ArgumentNullException(nameof(faceType));
			isFullMorph = false;
			lipPiercings = new Piercing<LipPiercingLocation>(LipPiercingUnlocked, LipSupportedJewelry);
			nosePiercings = new Piercing<NosePiercingLocation>(NosePiercingUnlocked, NoseSupportedJewelryByLocation);
			eyebrowPiercings = new Piercing<EyebrowPiercingLocation>(EyebrowPiercingUnlocked, EyebrowSupportedJewelry);
		}

		internal Face(Guid creatureID, FaceType faceType, bool? fullMorph = null, SkinTexture complexion = SkinTexture.NONDESCRIPT) : this(creatureID, faceType)
		{
			skinTexture = complexion;
			if (faceType.hasSecondLevel && fullMorph is bool morph)
			{
				isFullMorph = morph;
			}
		}

		public override FaceType defaultType => FaceType.defaultValue;

		public override FaceData AsReadOnlyData()
		{
			return new FaceData(this);
		}

		internal override bool UpdateType(FaceType newType)
		{
			if (newType == null || type == newType)
			{
				return false;
			}
			var oldType = type;
			var oldData = AsReadOnlyData();
			type = newType;

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}

		internal bool UpdateFaceWithMorph(FaceType faceType, bool fullMorph)
		{
			if (faceType == null || type == faceType)
			{
				return false;
			}
			var oldType = type;
			var oldData = AsReadOnlyData();

			type = faceType;
			isFullMorph = type.hasSecondLevel ? fullMorph : false;
			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}

		internal bool UpdateFaceWithComplexion(FaceType faceType, SkinTexture complexion)
		{
			if (faceType == null || type == faceType)
			{
				return false;
			}
			var oldType = type;
			var oldData = AsReadOnlyData();

			type = faceType;
			skinTexture = complexion;
			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}

		internal bool UpdateFaceWithMorphAndComplexion(FaceType faceType, bool fullMorph, SkinTexture complexion)
		{
			if (faceType == null || type == faceType)
			{
				return false;
			}
			var oldType = type;
			var oldData = AsReadOnlyData();

			type = faceType;
			skinTexture = complexion;
			isFullMorph = type.hasSecondLevel ? fullMorph : false;
			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}

		internal bool StrengthenFacialMorph()
		{
			if (!isFullMorph && type.hasSecondLevel)
			{
				var oldData = AsReadOnlyData();
				isFullMorph = true;
				NotifyDataChanged(oldData);
				return true;
			}
			return false;
		}

		internal bool WeakenFacialMorph(bool restoreIfAlreadyLevelOne = true)
		{
			//if full morph, weaken it to half-morph level. 
			if (isFullMorph)
			{
				var oldData = AsReadOnlyData();
				isFullMorph = false;
				NotifyDataChanged(oldData);
				return true;
			}
			//otherwise, if we're not human and we are to restore back to human on weaken
			else if (restoreIfAlreadyLevelOne && type != FaceType.defaultValue)
			{
				return Restore();
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

		internal bool ChangeComplexion(SkinTexture complexion)
		{
			if (skinTexture == complexion)
			{
				return false;
			}
			var oldData = AsReadOnlyData();
			skinTexture = complexion;
			NotifyDataChanged(oldData);
			return true;
		}

		//default restore is fine.

		internal void HandleOralPenetration(float penetratorArea, float knotWidth, float cumAmount, bool reachOrgasm)
		{
			oralCount++;
			if (reachOrgasm)
			{
				orgasmCount++;
			}
		}

		//internal void IngestCum(float cumAmount, bool reachOrgasm, bool countTowardOrgasmTotal)
		//{
		//	if (cumAmount > 0)
		//	{
		//		timeLastIngestedCum = GameDateTime.Now;
		//		lastCumIngestAmount = cumAmount;

		//		CreatureStore.GetCreatureClean(creatureID)?.genitals.ObtainedCum(cumAmount);
		//	}
		//}

		internal void HandleOralPenetration(Cock penetrator, bool reachOrgasm)
		{
			HandleOralPenetration(penetrator.area, penetrator.knotSize, penetrator.cumAmount, reachOrgasm);
		}
		internal void HandleOralPenetration(Cock penetrator, float cumAmountOverride, bool reachOrgasm)
		{
			HandleOralPenetration(penetrator.area, penetrator.knotSize, cumAmountOverride, reachOrgasm);
		}
		internal void HandleOralOrgasmGeneric(bool dryOrgasm)
		{
			orgasmCount++;
			if (dryOrgasm) dryOrgasmCount++;
		}

		internal void HandleTonguePenetrate(bool reachOrgasm)
		{
			CreatureStore.GetCreatureClean(creatureID)?.tongue.DoPenetrate();
			if (reachOrgasm)
			{
				orgasmCount++;
			}
		}


		internal void Reset()
		{
			var oldData = AsReadOnlyData();
			type = FaceType.HUMAN;
			isFullMorph = false;
			CheckDataChanged(oldData);
			lipPiercings.Reset();
			nosePiercings.Reset();
			eyebrowPiercings.Reset();
		}

		private void CheckDataChanged(FaceData oldData)
		{
			if (!primary.Equals(oldData.primaryEpidermis) || !secondary.Equals(oldData.secondaryEpidermis) ||
				isFullMorph != oldData.isFullMorph || skinTexture != oldData.skinTexture)
			{
				NotifyDataChanged(oldData);
			}
		}

		private bool LipPiercingUnlocked(LipPiercingLocation piercingLocation)
		{
			return true;
		}

		private JewelryType LipSupportedJewelry(LipPiercingLocation piercingLocation)
		{
			return SUPPORTED_LIP_JEWELRY;
		}

		private bool NosePiercingUnlocked(NosePiercingLocation piercingLocation)
		{
			return true;
		}

		private JewelryType NoseSupportedJewelryByLocation(NosePiercingLocation piercingLocation)
		{
			switch (piercingLocation)
			{
				case NosePiercingLocation.BRIDGE:
					return JewelryType.BARBELL_STUD;
				case NosePiercingLocation.SEPTIMUS:
					return JewelryType.HORSESHOE | JewelryType.RING;
				default:
					return JewelryType.BARBELL_STUD | JewelryType.RING | JewelryType.SPECIAL;
			}
		}

		private bool EyebrowPiercingUnlocked(EyebrowPiercingLocation piercingLocation)
		{
			return true;
		}

		private JewelryType EyebrowSupportedJewelry(EyebrowPiercingLocation piercingLocation)
		{
			return SUPPORTED_EYEBROW_JEWELRY;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			bool valid = true;

			FaceType faceType = type;
			bool fullMorph = isFullMorph;
			valid = FaceType.Validate(ref faceType, ref fullMorph, correctInvalidData);
			valid &= ValidatePiercing(valid, nosePiercings, correctInvalidData);
			valid &= ValidatePiercing(valid, eyebrowPiercings, correctInvalidData);
			valid &= ValidatePiercing(valid, lipPiercings, correctInvalidData);
			if ((valid || correctInvalidData) && !Enum.IsDefined(typeof(SkinTexture), _skinTexture))
			{
				if (correctInvalidData)
				{
					_skinTexture = SkinTexture.NONDESCRIPT;
				}
				valid = false;
			}
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

		bool ILotionable.canLotion()
		{
			return type.canChangeComplexion;
		}

		bool ILotionable.isDifferentTexture(SkinTexture lotionTexture)
		{
			return lotionTexture != skinTexture || !Enum.IsDefined(typeof(SkinTexture), lotionTexture);
		}

		bool ILotionable.attemptToLotion(SkinTexture lotionTexture)
		{
			if (!((ILotionable)this).canLotion() || !((ILotionable)this).isDifferentTexture(lotionTexture))
			{
				return false;
			}
			skinTexture = lotionTexture;
			return true;
		}

		string ILotionable.buttonText()
		{
			return type.buttonText();
		}

		string ILotionable.locationDesc()
		{
			return type.locationDesc();
		}

		bool ICanAttackWith.canAttackWith()
		{
			return type.canAttackWith;
		}
		AttackBase ICanAttackWith.attack => type.attack;
	}


	public abstract partial class FaceType : SaveableBehavior<FaceType, Face, FaceData>
	{
		private static readonly List<FaceType> faces = new List<FaceType>();
		public static readonly ReadOnlyCollection<FaceType> availableTypes = new ReadOnlyCollection<FaceType>(faces);
		private static int indexMaker = 0;

		public readonly bool hasSecondLevel;
		public readonly SimpleDescriptor secondLevelShortDescription;
		public readonly EpidermisType epidermisType;

		public SimpleDescriptor weakenTransformText => () => morphText(false);
		public SimpleDescriptor strengthenTransformText => () => morphText(true);

		internal virtual AttackBase attack => AttackBase.NO_ATTACK;
		internal virtual bool canAttackWith => attack != AttackBase.NO_ATTACK;

		private readonly DescriptorWithArg<bool> morphText;

		private readonly bool firstLevelIsMuzzle;
		private readonly bool secondLevelIsMuzzle;

		public bool HasMuzzle(bool isSecondLevel)
		{
			return isSecondLevel ? secondLevelIsMuzzle : firstLevelIsMuzzle;
		}

		private protected FaceType(EpidermisType epidermisType, SimpleDescriptor firstLevelShortDesc, SimpleDescriptor secondLevelShortDesc,
			bool firstLevelMuzzle, bool secondLevelMuzzle, DescriptorWithArg<bool> strengthenWeakenMorphText,
			DescriptorWithArg<Face> fullDesc, PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform,
			RestoreType<FaceData> restore) : base(firstLevelShortDesc, fullDesc, playerStr, transform, restore)
		{
			_index = indexMaker++;
			secondLevelShortDescription = secondLevelShortDesc;
			morphText = strengthenWeakenMorphText;
			hasSecondLevel = true;

			firstLevelIsMuzzle = firstLevelMuzzle;
			secondLevelIsMuzzle = secondLevelMuzzle;

			faces.AddAt(this, _index);
		}

		private protected FaceType(EpidermisType epidermisType, bool isMuzzle,
			SimpleDescriptor shortDesc, DescriptorWithArg<Face> fullDesc, PlayerBodyPartDelegate<Face> playerStr,
			ChangeType<FaceData> transform, RestoreType<FaceData> restore) : base(shortDesc, fullDesc, playerStr, transform, restore)
		{
			_index = indexMaker++;
			secondLevelShortDescription = GlobalStrings.None;
			morphText = x => "";
			hasSecondLevel = false;

			firstLevelIsMuzzle = isMuzzle;
			secondLevelIsMuzzle = isMuzzle;

			faces.AddAt(this, _index);
		}
		internal virtual bool isHumanoid(bool isSecondLevel)
		{
			if (hasSecondLevel)
			{
				return !isSecondLevel;
			}
			return false;
		}

		internal virtual bool isMuzzleShaped(bool isSecondLevel)
		{
			if (hasSecondLevel)
			{
				return isSecondLevel;
			}
			return false;
		}

		internal virtual bool MorphStrengthPostTransform(bool previousWasFullMorph)
		{
			return false;
		}

		internal virtual bool canChangeComplexion => true;
		internal virtual SimpleDescriptor buttonText => FaceStr;
		internal virtual SimpleDescriptor locationDesc => YourFaceStr;
		internal abstract EpidermalData ParseEpidermis(BodyData bodyData, bool isFullMorph, SkinTexture complexion);
		internal virtual EpidermalData ParseSecondaryEpidermis(BodyData bodyData, bool isFullMorph, SkinTexture complexion)
		{
			return new EpidermalData();
		}

		public override int index => _index;

		public static FaceType defaultValue => HUMAN;

		private readonly int _index;

		internal static bool Validate(ref FaceType faceType, ref bool isFullMorph, bool correctInvalidData)
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


		public static readonly FaceType HUMAN = new ToneFace(EpidermisType.SKIN, false, HumanShortDesc, HumanLongDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly FaceType HORSE = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultHorseFur, true, HorseShortDesc, HorseLongDesc, HorsePlayerStr,
			HorseTransformStr, HorseRestoreStr); //muzzle. 
		public static readonly FaceType DOG = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultDogFur, true, DogShortDesc, DogLongDesc, DogPlayerStr, DogTransformStr, DogRestoreStr); //muzzle
		public static readonly FaceType COW_MINOTAUR = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultCowFur, CowShortDesc, MinotaurShortDesc, false, true,
			CowMorphText, Cow_MinotaurLongDesc, Cow_MinotaurPlayerStr, Cow_MinotaurTransformStr, Cow_MinotaurRestoreStr); //muzzle, second level. 
		public static readonly FaceType SHARK = new SharkFace();
		public static readonly FaceType SNAKE = new SnakeFace();
		public static readonly FaceType CAT = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultCatFur, CatGirlShortDesc, CatMorphShortDesc, false, true,
			CatMorphText, CatLongDesc, CatPlayerStr, CatTransformStr, CatRestoreStr); //muzzle, second level. 
		public static readonly FaceType LIZARD = new ToneFace(EpidermisType.SCALES, true, LizardShortDesc, LizardLongDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr); //muzzle.
		public static readonly FaceType BUNNY = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultBunnyFur, BunnyFirstLevelShortDesc, BunnySecondLevelShortDesc,
			false, true, BunnyMorphText, BunnyLongDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static readonly FaceType KANGAROO = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultKangarooFacialFur, true, KangarooShortDesc,
			KangarooLongDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly FaceType SPIDER = new SpiderFace();
		public static readonly FaceType FOX = new FoxFace();
		public static readonly FaceType DRAGON = new ToneFace(EpidermisType.SCALES, true, DragonShortDesc, DragonLongDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly FaceType RACCOON = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultRaccoonFur, RaccoonMaskShortDesc, RaccoonFaceShortDesc, false, false,
			RaccoonMorphText, RaccoonLongDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr);
		public static readonly FaceType MOUSE = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultMouseFur, MouseTeethShortDesc, MouseFaceShortDesc, false, false,
			MouseMorphText, MouseLongDesc, MousePlayerStr, MouseTransformStr, MouseRestoreStr);
		public static readonly FaceType FERRET = new MultiFurFace(EpidermisType.FUR, DefaultValueHelpers.defaultFerretFur, DefaultValueHelpers.defaultFerretSecondaryFacialFur,
			FerretMaskShortDesc, FerretFaceShortDesc, false, true, FerretMorphText, FerretLongDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly FaceType PIG = new PigFace(); //both pig and boar are not humanoid.
		public static readonly FaceType RHINO = new ToneFace(EpidermisType.SKIN, false, RhinoShortDesc, RhinoLongDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr); //muzzle
		public static readonly FaceType ECHIDNA = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultEchidnaFur, true, EchidnaShortDesc, EchidnaLongDesc,
			EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly FaceType DEER = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultDeerFur, true, DeerShortDesc, DeerLongDesc, DeerPlayerStr,
			DeerTransformStr, DeerRestoreStr); //muzzle
		public static readonly FaceType WOLF = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultWolfFacialFur, true, WolfShortDesc, WolfLongDesc,
			WolfPlayerStr, WolfTransformStr, WolfRestoreStr);//muzzle
		public static readonly FaceType COCKATRICE = new CockatriceFace();
		public static readonly FaceType RED_PANDA = new MultiFurFace(EpidermisType.FUR, DefaultValueHelpers.defaultRedPandaFur, DefaultValueHelpers.defaultRedPandaFaceEarTailFur,
			true, PandaShortDesc, PandaLongDesc, PandaPlayerStr, PandaTransformStr, PandaRestoreStr);
		//new type, broken out from standard because goo is now a full set or body parts.
		public static readonly FaceType GOO = new ToneFace(EpidermisType.GOO, false, GooShortDesc, GooLongDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		//placeholder.
		public static readonly FaceType BEAK = new FurFace(EpidermisType.FEATHERS, new FurColor(HairFurColors.WHITE), false, BeakShortDesc, BeakLongDesc,
			BeakPlayerStr, BeakTransformStr, BeakRestoreStr);

		private class ToneFace : FaceType
		{
			public ToneFace(ToneBasedEpidermisType epidermisType, bool isMuzzle, SimpleDescriptor shortDesc, DescriptorWithArg<Face> fullDesc, PlayerBodyPartDelegate<Face> playerStr,
				ChangeType<FaceData> transform, RestoreType<FaceData> restore) : base(epidermisType, isMuzzle, shortDesc, fullDesc, playerStr, transform, restore)
			{
			}

			public ToneFace(ToneBasedEpidermisType epidermisType, SimpleDescriptor firstLevelShortDesc, SimpleDescriptor secondLevelShortDesc, bool firstLevelMuzzle,
				bool secondLevelMuzzle, DescriptorWithArg<bool> strengthenWeakenMorphText, DescriptorWithArg<Face> fullDesc, PlayerBodyPartDelegate<Face> playerStr,
				ChangeType<FaceData> transform, RestoreType<FaceData> restore) : base(epidermisType, firstLevelShortDesc, secondLevelShortDesc, firstLevelMuzzle,
					secondLevelMuzzle, strengthenWeakenMorphText, fullDesc, playerStr, transform, restore)
			{
			}

			protected ToneBasedEpidermisType primaryEpidermis => (ToneBasedEpidermisType)epidermisType;

			internal override EpidermalData ParseEpidermis(BodyData bodyData, bool isFullMorph, SkinTexture complexion)
			{
				return new EpidermalData(primaryEpidermis, bodyData.mainSkin.tone, complexion);
			}
		}

		private class MultiToneFace : ToneFace
		{
			private readonly Tones defaultSecondaryTone;

			public MultiToneFace(ToneBasedEpidermisType epidermisType, Tones secondaryToneFallback, bool isMuzzle, SimpleDescriptor shortDesc, DescriptorWithArg<Face> fullDesc,
				PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform, RestoreType<FaceData> restore)
				: base(epidermisType, isMuzzle, shortDesc, fullDesc, playerStr, transform, restore)
			{
				defaultSecondaryTone = secondaryToneFallback;
			}


			public MultiToneFace(ToneBasedEpidermisType epidermisType, Tones secondaryToneFallback, SimpleDescriptor firstLevelShortDesc, SimpleDescriptor secondLevelShortDesc,
				bool firstLevelMuzzle, bool secondLevelMuzzle, DescriptorWithArg<bool> strengthenWeakenMorphText, DescriptorWithArg<Face> fullDesc,
				PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform, RestoreType<FaceData> restore)
				: base(epidermisType, firstLevelShortDesc, secondLevelShortDesc, firstLevelMuzzle, secondLevelMuzzle, strengthenWeakenMorphText, fullDesc, playerStr, transform, restore)
			{
				defaultSecondaryTone = secondaryToneFallback;
			}

			internal override EpidermalData ParseSecondaryEpidermis(BodyData bodyData, bool isFullMorph, SkinTexture complexion)
			{
				Tones tone = !bodyData.supplementary.tone.isEmpty ? bodyData.supplementary.tone : defaultSecondaryTone;
				SkinTexture texture = bodyData.supplementary.usesTone ? bodyData.supplementary.skinTexture : SkinTexture.NONDESCRIPT;
				return new EpidermalData(primaryEpidermis, tone, texture);
			}
		}

		private class FurFace : FaceType
		{
			public readonly FurColor defaultColor;

			protected FurBasedEpidermisType primaryEpidermis => (FurBasedEpidermisType)epidermisType;

			public FurFace(FurBasedEpidermisType epidermisType, FurColor fallbackColor, bool isMuzzle, SimpleDescriptor shortDesc, DescriptorWithArg<Face> fullDesc,
				PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform, RestoreType<FaceData> restore)
				: base(epidermisType, isMuzzle, shortDesc, fullDesc, playerStr, transform, restore)
			{
				defaultColor = fallbackColor;
			}

			public FurFace(FurBasedEpidermisType epidermisType, FurColor fallbackColor, SimpleDescriptor firstLevelShortDesc, SimpleDescriptor secondLevelShortDesc,
				bool firstLevelMuzzle, bool secondLevelMuzzle, DescriptorWithArg<bool> strengthenWeakenMorphText, DescriptorWithArg<Face> fullDesc,
				PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform, RestoreType<FaceData> restore)
				: base(epidermisType, firstLevelShortDesc, secondLevelShortDesc, firstLevelMuzzle, secondLevelMuzzle, strengthenWeakenMorphText, fullDesc, playerStr, transform, restore)
			{
				defaultColor = fallbackColor;
			}



			internal override EpidermalData ParseEpidermis(BodyData bodyData, bool isFullMorph, SkinTexture complexion)
			{
				FurColor color = defaultColor;
				FurTexture texture = FurTexture.NONDESCRIPT;
				if (!bodyData.activeFur.fur.isEmpty) //probably never null, but w/e i'll be safe.
				{
					color = bodyData.activeFur.fur;
					texture = bodyData.activeFur.furTexture;
				}
				else if (!bodyData.activeHairColor.isEmpty)
				{
					color = new FurColor(bodyData.activeHairColor);
				}
				return new EpidermalData(primaryEpidermis, color, texture);
			}
		}

		private class MultiFurFace : FurFace
		{
			private readonly FurColor secondaryDefaultColor;

			public MultiFurFace(FurBasedEpidermisType epidermisType, FurColor fallbackColor, FurColor secondaryFallbackColor, bool isMuzzle, SimpleDescriptor shortDesc,
				DescriptorWithArg<Face> fullDesc, PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform, RestoreType<FaceData> restore)
				: base(epidermisType, fallbackColor, isMuzzle, shortDesc, fullDesc, playerStr, transform, restore)
			{
				secondaryDefaultColor = secondaryFallbackColor;
			}

			public MultiFurFace(FurBasedEpidermisType epidermisType, FurColor fallbackColor, FurColor secondaryFallbackColor, SimpleDescriptor firstLevelShortDesc,
				SimpleDescriptor secondLevelShortDesc, bool firstLevelMuzzle, bool secondLevelMuzzle, DescriptorWithArg<bool> strengthenWeakenMorphText,
				DescriptorWithArg<Face> fullDesc, PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform, RestoreType<FaceData> restore)
				: base(epidermisType, fallbackColor, firstLevelShortDesc, secondLevelShortDesc, firstLevelMuzzle, secondLevelMuzzle, strengthenWeakenMorphText, fullDesc, playerStr, transform, restore)
			{
				secondaryDefaultColor = secondaryFallbackColor;
			}

			internal override EpidermalData ParseSecondaryEpidermis(BodyData bodyData, bool isFullMorph, SkinTexture complexion)
			{
				EpidermalData primary = ParseEpidermis(bodyData, isFullMorph, complexion);
				//Epidermis retVal = new Epidermis(epidermisType, true);
				FurColor color = secondaryDefaultColor;
				FurTexture texture = FurTexture.NONDESCRIPT;
				////make sure the fur color is not the same as the primary, and that it is valid.s
				if (!bodyData.supplementary.fur.isEmpty && !bodyData.supplementary.fur.Equals(primary.fur))
				{
					color = bodyData.supplementary.fur;
					texture = bodyData.supplementary.furTexture;
				}
				////check if the hair isn't empty. further, make sure the primary color isn't also using the hair color.
				else if (!bodyData.hairColor.isEmpty)
				{
					color = new FurColor(bodyData.hairColor);
				}

				return new EpidermalData(primaryEpidermis, color, texture);
			}
		}

		private sealed class PigFace : FurFace
		{
			public PigFace() : base(EpidermisType.FUR, DefaultValueHelpers.defaultPigFur, PigShortDesc, BoarShortDesc, false, false, PigMorphText, PigLongDesc, PigPlayerStr, PigTransformStr, PigRestoreStr) { }

			internal override bool isHumanoid(bool isSecondLevel)
			{
				return false;
			}
		}
		private sealed class SpiderFace : ToneFace
		{
			public SpiderFace() : base(EpidermisType.SKIN, false, SpiderShortDesc, SpiderLongDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr) { }

			internal override bool isHumanoid(bool isSecondLevel)
			{
				return true;
			}

			internal override AttackBase attack => _attack;
			private static readonly AttackBase _attack = new SpiderBite();
		}
		private sealed class SharkFace : ToneFace
		{
			public SharkFace() : base(EpidermisType.SKIN, false, SharkShortDesc, SharkLongDesc, SharkPlayerStr, SharkTransformStr, SharkRestoreStr) { }

			internal override bool isHumanoid(bool isSecondLevel)
			{
				return true;
			}

			internal override AttackBase attack => _attack;
			private static readonly AttackBase _attack = new GenericBite(SharkShortDesc, 4);
		}
		private sealed class SnakeFace : ToneFace
		{
			public SnakeFace() : base(EpidermisType.SCALES, false, SnakeShortDesc, SnakeLongDesc, SnakePlayerStr, SnakeTransformStr, SnakeRestoreStr) { }

			internal override bool isHumanoid(bool isSecondLevel)
			{
				return true;
			}

			internal override AttackBase attack => _attack;
			private static readonly AttackBase _attack = new NagaBite();
		}

		private sealed class CockatriceFace : FurFace
		{
			private readonly ToneBasedEpidermisType secondaryEpidermis = EpidermisType.SCALES;
			private readonly Tones defaultUnderTone = DefaultValueHelpers.defaultCockatriceScaleTone;
			public CockatriceFace() : base(EpidermisType.FEATHERS, DefaultValueHelpers.defaultCockatricePrimaryFeathers, false, CockatriceShortDesc,
				CockatriceLongDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr)
			{ }

			internal override EpidermalData ParseSecondaryEpidermis(BodyData bodyData, bool isFullMorph, SkinTexture complexion)
			{
				//Epidermis retVal = new Epidermis(secondaryEpidermis, true);
				Tones tone = !bodyData.supplementary.tone.isEmpty ? bodyData.supplementary.tone : defaultUnderTone;
				SkinTexture texture = bodyData.supplementary.usesTone ? bodyData.supplementary.skinTexture : SkinTexture.NONDESCRIPT;
				return new EpidermalData(secondaryEpidermis, tone, texture);
				//return retVal;
			}
		}

		private sealed class FoxFace : FurFace
		{
			private FurColor defaultKitsuneFur => DefaultValueHelpers.defaultKitsuneFacialFur;
			public FoxFace() : base(EpidermisType.FUR, DefaultValueHelpers.defaultFoxFacialFur, KitsuneShortDesc, FoxShortDesc, false, true, FoxMorphText, FoxLongDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr) { }

			internal override EpidermalData ParseEpidermis(BodyData bodyData, bool isFullMorph, SkinTexture complexion)
			{
				FurColor color = isFullMorph ? defaultColor : defaultKitsuneFur;
				if (!bodyData.activeFur.fur.isEmpty)
				{
					color = bodyData.activeFur.fur;
				}
				else if (!bodyData.activeHairColor.isEmpty)
				{
					color = new FurColor(bodyData.activeHairColor);
				}
				return new EpidermalData(primaryEpidermis, color, FurTexture.SOFT);
			}
		}
	}

	public static class FaceHelpers
	{
		public static PiercingJewelry GenerateEyebrowJewelry(this Face face, EyebrowPiercingLocation location, JewelryType jewelryType, JewelryMaterial jewelryMaterial)
		{
			if (face.eyebrowPiercings.CanWearThisJewelryType(location, jewelryType))
			{
				return new GenericPiercing(jewelryType, jewelryMaterial);
			}
			return null;
		}

		public static PiercingJewelry GenerateLipJewelry(this Face face, LipPiercingLocation location, JewelryType jewelryType, JewelryMaterial jewelryMaterial)
		{
			if (face.lipPiercings.CanWearThisJewelryType(location, jewelryType))
			{
				return new GenericPiercing(jewelryType, jewelryMaterial);
			}
			return null;
		}

		public static PiercingJewelry GenerateNoseJewelry(this Face face, NosePiercingLocation location, JewelryType jewelryType, JewelryMaterial jewelryMaterial)
		{
			if (face.nosePiercings.CanWearThisJewelryType(location, jewelryType))
			{
				return new GenericPiercing(jewelryType, jewelryMaterial);
			}
			return null;
		}
	}

	public sealed class FaceData : BehavioralSaveablePartData<FaceData, Face, FaceType>
	{
		public readonly bool isFullMorph;

		public readonly EpidermalData primaryEpidermis;
		public readonly EpidermalData secondaryEpidermis;
		public readonly SkinTexture skinTexture;

		public FaceData(Face source) : base(GetID(source), GetBehavior(source))
		{
			isFullMorph = source.isFullMorph;
			primaryEpidermis = source.primary;
			secondaryEpidermis = source.secondary;
			skinTexture = source.skinTexture;
		}
	}
}
