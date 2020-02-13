//Face.cs
//Description:
//Author: JustSomeGuy
//4/24/2019, 1:12 AM
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CoC.Backend.Attacks;
using CoC.Backend.Attacks.BodyPartAttacks;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Materials;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	//ngl, i can never recall if medusa is top and labret bottom, or if it's the opposite.
	public sealed partial class LipPiercingLocation : PiercingLocation, IEquatable<LipPiercingLocation>
	{
		private static readonly List<LipPiercingLocation> _allLocations = new List<LipPiercingLocation>();

		public static readonly ReadOnlyCollection<LipPiercingLocation> allLocations;

		private readonly byte index;

		static LipPiercingLocation()
		{
			allLocations = new ReadOnlyCollection<LipPiercingLocation>(_allLocations);
		}

		public LipPiercingLocation(byte index, CompatibleWith allowsJewelryOfType, SimpleDescriptor btnText, SimpleDescriptor locationDesc)
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
			if (obj is LipPiercingLocation lipPiercing)
			{
				return Equals(lipPiercing);
			}
			else
			{
				return false;
			}
		}

		public bool Equals(LipPiercingLocation other)
		{
			return !(other is null) && other.index == index;
		}

		public override int GetHashCode()
		{
			return index.GetHashCode();
		}

		public static readonly LipPiercingLocation LABRET = new LipPiercingLocation(0, SupportedJewelry, LabretButton, LabretLocation);
		public static readonly LipPiercingLocation MEDUSA = new LipPiercingLocation(1, SupportedJewelry, MedusaButton, MedusaLocation);
		public static readonly LipPiercingLocation MONROE_LEFT = new LipPiercingLocation(2, SupportedJewelry, MonroeLeftButton, MonroeLeftLocation);
		public static readonly LipPiercingLocation MONROE_RIGHT = new LipPiercingLocation(3, SupportedJewelry, MonroeRightButton, MonroeRightLocation);
		public static readonly LipPiercingLocation LOWER_LEFT_1 = new LipPiercingLocation(4, SupportedJewelry, LowerLeft1Button, LowerLeft1Location);
		public static readonly LipPiercingLocation LOWER_LEFT_2 = new LipPiercingLocation(5, SupportedJewelry, LowerLeft2Button, LowerLeft2Location);
		public static readonly LipPiercingLocation LOWER_RIGHT_1 = new LipPiercingLocation(6, SupportedJewelry, LowerRight1Button, LowerRight1Location);
		public static readonly LipPiercingLocation LOWER_RIGHT_2 = new LipPiercingLocation(7, SupportedJewelry, LowerRight2Button, LowerRight2Location);

		private static bool SupportedJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.HORSESHOE || jewelryType == JewelryType.BARBELL_STUD || jewelryType == JewelryType.RING || jewelryType == JewelryType.SPECIAL;
		}
	}

	public sealed class LipPiercing : Piercing<LipPiercingLocation>
	{
		public LipPiercing(IBodyPart source, PiercingUnlocked LocationUnlocked, GenericCreatureText playerShortDesc, GenericCreatureText playerLongDesc) : base(source, LocationUnlocked, playerShortDesc, playerLongDesc)
		{
		}

		public override int MaxPiercings => LipPiercingLocation.allLocations.Count;

		public override IEnumerable<LipPiercingLocation> availableLocations => LipPiercingLocation.allLocations;
	}

	public sealed partial class EyebrowPiercingLocation : PiercingLocation, IEquatable<EyebrowPiercingLocation>
	{
		private static readonly List<EyebrowPiercingLocation> _allLocations = new List<EyebrowPiercingLocation>();

		public static readonly ReadOnlyCollection<EyebrowPiercingLocation> allLocations;

		private readonly byte index;

		static EyebrowPiercingLocation()
		{
			allLocations = new ReadOnlyCollection<EyebrowPiercingLocation>(_allLocations);
		}

		public EyebrowPiercingLocation(byte index, CompatibleWith allowsJewelryOfType, SimpleDescriptor btnText, SimpleDescriptor locationDesc)
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
			if (obj is EyebrowPiercingLocation eyebrowPiercing)
			{
				return Equals(eyebrowPiercing);
			}
			else
			{
				return false;
			}
		}

		public bool Equals(EyebrowPiercingLocation other)
		{
			return !(other is null) && other.index == index;
		}

		public override int GetHashCode()
		{
			return index.GetHashCode();
		}

		public static readonly EyebrowPiercingLocation LEFT_1 = new EyebrowPiercingLocation(0, SupportedJewelry, Left1Button, Left1Location);
		public static readonly EyebrowPiercingLocation LEFT_2 = new EyebrowPiercingLocation(1, SupportedJewelry, Left2Button, Left2Location);
		public static readonly EyebrowPiercingLocation RIGHT_1 = new EyebrowPiercingLocation(2, SupportedJewelry, Right1Button, Right1Location);
		public static readonly EyebrowPiercingLocation RIGHT_2 = new EyebrowPiercingLocation(3, SupportedJewelry, Right2Button, Right2Location);

		private static bool SupportedJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.HORSESHOE || jewelryType == JewelryType.BARBELL_STUD || jewelryType == JewelryType.RING || jewelryType == JewelryType.SPECIAL;
		}
	}

	public sealed class EyebrowPiercing : Piercing<EyebrowPiercingLocation>
	{
		public EyebrowPiercing(IBodyPart source, PiercingUnlocked LocationUnlocked, GenericCreatureText playerShortDesc, GenericCreatureText playerLongDesc) : base(source, LocationUnlocked, playerShortDesc, playerLongDesc)
		{
		}

		public override int MaxPiercings => EyebrowPiercingLocation.allLocations.Count;

		public override IEnumerable<EyebrowPiercingLocation> availableLocations => EyebrowPiercingLocation.allLocations;
	}

	public sealed partial class NosePiercingLocation : PiercingLocation, IEquatable<NosePiercingLocation>
	{
		private static readonly List<NosePiercingLocation> _allLocations = new List<NosePiercingLocation>();

		public static readonly ReadOnlyCollection<NosePiercingLocation> allLocations;

		private readonly byte index;

		static NosePiercingLocation()
		{
			allLocations = new ReadOnlyCollection<NosePiercingLocation>(_allLocations);
		}

		public NosePiercingLocation(byte index, CompatibleWith allowsJewelryOfType, SimpleDescriptor btnText, SimpleDescriptor locationDesc)
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
			if (obj is NosePiercingLocation nosePiercing)
			{
				return Equals(nosePiercing);
			}
			else
			{
				return false;
			}
		}

		public bool Equals(NosePiercingLocation other)
		{
			return !(other is null) && other.index == index;
		}

		public override int GetHashCode()
		{
			return index.GetHashCode();
		}

		public static readonly NosePiercingLocation LEFT_NOSTRIL = new NosePiercingLocation(0, SupportedNostrilJewelry, LeftButton, LeftLocation);
		public static readonly NosePiercingLocation RIGHT_NOSTRIL = new NosePiercingLocation(1, SupportedNostrilJewelry, RightButton, RightLocation);
		public static readonly NosePiercingLocation SEPTIMUS = new NosePiercingLocation(2, SupportedSeptimusJewelry, SeptimusButton, SeptimusLocation);
		public static readonly NosePiercingLocation BRIDGE = new NosePiercingLocation(3, SupportedBridgeJewelry, BridgeButton, BridgeLocation);

		private static bool SupportedNostrilJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.BARBELL_STUD || jewelryType == JewelryType.RING || jewelryType == JewelryType.SPECIAL;
		}

		private static bool SupportedBridgeJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.BARBELL_STUD;
		}

		private static bool SupportedSeptimusJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.HORSESHOE || jewelryType == JewelryType.RING;
		}

	}

	public sealed class NosePiercing : Piercing<NosePiercingLocation>
	{
		public NosePiercing(IBodyPart source, PiercingUnlocked LocationUnlocked, GenericCreatureText playerShortDesc, GenericCreatureText playerLongDesc) : base(source, LocationUnlocked, playerShortDesc, playerLongDesc)
		{
		}

		public override int MaxPiercings => NosePiercingLocation.allLocations.Count;

		public override IEnumerable<NosePiercingLocation> availableLocations => NosePiercingLocation.allLocations;
	}

	public sealed partial class FaceTattooLocation : TattooLocation
	{

		private static readonly List<FaceTattooLocation> _allLocations = new List<FaceTattooLocation>();

		public static readonly ReadOnlyCollection<FaceTattooLocation> allLocations;

		private readonly byte index;

		static FaceTattooLocation()
		{
			allLocations = new ReadOnlyCollection<FaceTattooLocation>(_allLocations);
		}

		private FaceTattooLocation(byte index, TattooSizeLimit limitSize, SimpleDescriptor btnText, SimpleDescriptor locationDesc) : base(limitSize, btnText, locationDesc)
		{
			this.index = index;
		}

		public static FaceTattooLocation LEFT_CHEEKBONE = new FaceTattooLocation(0, SmallTattoosOnly, LeftCheekButton, LeftCheekLocation);
		//face, around eye. i don't know what else to call it lol.
		public static FaceTattooLocation LEFT_TYSON = new FaceTattooLocation(1, MediumTattoosOrSmaller, LeftTysonButton, LeftTysonLocation);

		public static FaceTattooLocation RIGHT_CHEEKBONE = new FaceTattooLocation(2, SmallTattoosOnly, RightCheekButton, RightCheekLocation);
		public static FaceTattooLocation RIGHT_TYSON = new FaceTattooLocation(3, MediumTattoosOrSmaller, RightTysonButton, RightTysonLocation);

		//there could be some combo tattoo that wraps along the cheekbones to the lower jaw or something. if so, mark it incompatible with them and add it here or whatever.

		public static FaceTattooLocation LOWER_JAW = new FaceTattooLocation(4, SmallTattoosOnly, LowerJawButton, LowerJawLocation);
		public static FaceTattooLocation FOREHEAD = new FaceTattooLocation(5, MediumTattoosOrSmaller, ForeheadButton, ForeheadLocation);
		public static FaceTattooLocation FULL_FACE = new FaceTattooLocation(6, FullPartTattoo, FullFaceButton, FullFaceLocation);

		public static bool LocationsCompatible(FaceTattooLocation first, FaceTattooLocation second)
		{
			return true;
		}
	}

	public sealed class FaceTattoo : TattooablePart<FaceTattooLocation>
	{
		public FaceTattoo(IBodyPart source, GenericCreatureText allTattoosShort, GenericCreatureText allTattoosLong) : base(source, allTattoosShort, allTattoosLong)
		{
		}

		public override int MaxTattoos => FaceTattooLocation.allLocations.Count;

		public override IEnumerable<FaceTattooLocation> availableLocations => FaceTattooLocation.allLocations;

		public override bool LocationsCompatible(FaceTattooLocation first, FaceTattooLocation second) => FaceTattooLocation.LocationsCompatible(first, second);
	}

	/*
	 * Faces are another instance of shit that breaks the epidermis stores only one things rule. You can have a two-tone face, but theres also skin underneath,
	 * and for some reason you need the skin tone to describe the face in detail. IDK, man. Regardless, you have the whole body data, just use that. if you need all three
	 * for something not in the face itself, it's available, i guess. We're not firing when it changes though.
	 */
	public sealed partial class Face : FullBehavioralPart<Face, FaceType, FaceData>, ILotionable, ICanAttackWith
	{

		public override string BodyPartName() => Name();

		public readonly LipPiercing lipPiercings;
		public readonly NosePiercing nosePiercings;
		public readonly EyebrowPiercing eyebrowPiercings;

		public EpidermalData facialSkin
		{
			get
			{
				EpidermalData prime = primary;
				if (prime.type is ToneBasedEpidermisType toneType)
				{
					return new EpidermalData(toneType, prime.tone, skinTexture);

				}
				else
				{
					return new EpidermalData(EpidermisType.SKIN, type.FacialSkinTone(bodyData), skinTexture);
				}
			}
		}

		private BodyData bodyData => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.body.AsReadOnlyData() : new BodyData(creatureID);

		public override FaceType type
		{
			get => _type;
			protected set
			{
				isFullMorph = value.MorphStrengthPostTransform(_type, isFullMorph);
				_type = value;
			}
		}
		private FaceType _type;

		public bool isFullMorph { get; private set; }

		public bool isHumanoid => type.IsHumanoid(isFullMorph);

		public bool hasBeak => type.HasBeak(isFullMorph);

		public bool hasMuzzle => type.HasMuzzle(isFullMorph);

		public bool hasSecondLevel => type.hasSecondLevel;

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

		public uint totalOralCount { get; private set; } = 0;
		public uint selfOralCount { get; private set; } = 0;

		public uint orgasmCount { get; private set; } = 0;
		public uint dryOrgasmCount { get; private set; } = 0;

		//private GameDateTime timeLastIngestedCum;
		//public int hoursSinceIngestedCum => timeLastIngestedCum.hoursToNow();
		//public float lastCumIngestAmount { get; private set; } = 0;

		public readonly FaceTattoo tattoos;

		public FacialStructure facialStructure => type.GetFacialStructure(isFullMorph);

		internal Face(Guid creatureID) : this(creatureID, FaceType.defaultValue)
		{ }

		internal Face(Guid creatureID, FaceType faceType) : base(creatureID)
		{
			type = faceType ?? throw new ArgumentNullException(nameof(faceType));
			isFullMorph = false;

			lipPiercings = new LipPiercing(this, LipPiercingUnlocked, AllLipPiercingsShort, AllLipPiercingsLong);
			nosePiercings = new NosePiercing(this, NosePiercingUnlocked, AllNosePiercingsShort, AllNosePiercingsLong);
			eyebrowPiercings = new EyebrowPiercing(this, EyebrowPiercingUnlocked, AllEyebrowPiercingsShort, AllEyebrowPiercingsLong);

			tattoos = new FaceTattoo(this, AllTattoosShort, AllTattoosLong);
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

		public override string ShortDescription()
		{
			if (isFullMorph)
			{
				return type.secondLevelShortDescription();
			}
			else
			{
				return type.ShortDescription();
			}
		}

		internal override bool UpdateType(FaceType newType)
		{
			if (newType == null || type == newType)
			{
				return false;
			}
			FaceType oldType = type;
			FaceData oldData = AsReadOnlyData();
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
			FaceType oldType = type;
			FaceData oldData = AsReadOnlyData();

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
			FaceType oldType = type;
			FaceData oldData = AsReadOnlyData();

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
			FaceType oldType = type;
			FaceData oldData = AsReadOnlyData();

			type = faceType;
			skinTexture = complexion;
			isFullMorph = type.hasSecondLevel ? fullMorph : false;
			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}

		public bool StrengthenFacialMorph()
		{
			if (!isFullMorph && type.hasSecondLevel)
			{
				FaceData oldData = AsReadOnlyData();
				isFullMorph = true;
				NotifyDataChanged(oldData);
				return true;
			}
			return false;
		}

		public bool WeakenFacialMorph(bool restoreIfAlreadyLevelOne = true)
		{
			//if full morph, weaken it to half-morph level.
			if (isFullMorph)
			{
				FaceData oldData = AsReadOnlyData();
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
			else
			{
				return UpdateFaceWithMorph(faceType, forceFullMorph);
			}
		}

		public bool ChangeComplexion(SkinTexture complexion)
		{
			if (skinTexture == complexion)
			{
				return false;
			}
			FaceData oldData = AsReadOnlyData();
			skinTexture = complexion;
			NotifyDataChanged(oldData);
			return true;
		}

		//default restore is fine.

		internal void HandleOralPenetration(float penetratorArea, float knotWidth, float cumAmount, bool reachOrgasm, bool selfOral = false)
		{
			totalOralCount++;
			if (selfOral)
			{
				selfOralCount++;
			}


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

		public override bool IsIdenticalTo(FaceData original, bool ignoreSexualMetaData)
		{
			return !(original is null) && original.type == type && original.isFullMorph == isFullMorph && primary.Equals(original.primaryEpidermis)
				&& secondary.Equals(original.secondaryEpidermis) && skinTexture == original.skinTexture && nosePiercings.IsIdenticalTo(original.nosePiercings)
				&& eyebrowPiercings.IsIdenticalTo(original.eyebrowPiercings) && lipPiercings.IsIdenticalTo(original.lipPiercings)
				&& tattoos.IsIdenticalTo(original.tattoos) && (ignoreSexualMetaData || (totalOralCount == original.totalOralCount && selfOralCount == original.selfOralCount
				&& orgasmCount == original.orgasmCount && dryOrgasmCount == original.dryOrgasmCount));
		}

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
			if (dryOrgasm)
			{
				dryOrgasmCount++;
			}
		}

		internal void HandleTonguePenetrate(bool reachOrgasm, bool selfPenetrating = false)
		{
			CreatureStore.GetCreatureClean(creatureID)?.tongue.DoPenetrate(selfPenetrating);
			if (reachOrgasm)
			{
				orgasmCount++;
			}
		}


		internal void Reset()
		{
			FaceData oldData = AsReadOnlyData();
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

		private bool LipPiercingUnlocked(LipPiercingLocation piercingLocation, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public bool wearingCowNoseRing => nosePiercings.WearingJewelryAt(NosePiercingLocation.SEPTIMUS) && nosePiercings[NosePiercingLocation.SEPTIMUS].jewelryType == JewelryType.RING;

		private bool NosePiercingUnlocked(NosePiercingLocation piercingLocation, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		private bool EyebrowPiercingUnlocked(EyebrowPiercingLocation piercingLocation, out string whyNot)
		{
			whyNot = null;
			return true;
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
		private bool ValidatePiercing<T>(bool valid, Piercing<T> piercing, bool correctInvalidData) where T : PiercingLocation
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

		string ILotionable.locationDesc(out bool isPlural)
		{
			return type.locationDesc(out isPlural);
		}
		//<postLocationDescription>
		SkinTexture ILotionable.postUseSkinTexture()
		{
			return skinTexture;
		}

		bool ICanAttackWith.canAttackWith()
		{
			return type.canAttackWith;
		}

		AttackBase ICanAttackWith.attack => type.attack;

		public bool IsReptilian()
		{
			return type == FaceType.LIZARD || type == FaceType.DRAGON;
		}
	}

	//feel free to add more types. if you need a single instance to support multiple types at once, you could mark this with the [flags] attribute.
	//if you do that, the default (which should be 0) is UNIQUE. You'll need to change the checks to use HasFlag instead of '=='
	//if this becomes necessary and you need more info, it can be found here: https://docs.microsoft.com/en-us/dotnet/api/system.flagsattribute?view=netframework-4.8

	//humanoid: does this face appear human, but with (possibly) some non-human characteristics like abnormal teeth or additional patches of fur or scales?
	//muzzle: does this creature have a long, animalistic muzzle in place of a nose, and thus is distinctly different from a human's
	//beak: does this creature have something that would classify as a beak for mouth and/or nose? this (probably) would make it distinctly not human
	//unique: default, uncategorized. use this when none of the others apply.

	//generally, if the face type has two levels, the first is some human-type hybrid (like cat-girl, or kitsune, etc), which would appear humanoid, then the second level is a full tf
	//which would not. note that there are some exceptions to this (pig, for example, is not human on any level)
	//it's also possible for a single level face type to be any of these values, including humanoid for non-human types. examples include the spider/snake types, which are just human
	//faces with fangs. shark is the same way, but with shark-teeth instead.
	public enum FacialStructure { UNIQUE, HUMANOID, MUZZLE, BEAK }


	public abstract partial class FaceType : FullBehavior<FaceType, Face, FaceData>
	{
		private static readonly List<FaceType> faces = new List<FaceType>();
		public static readonly ReadOnlyCollection<FaceType> availableTypes = new ReadOnlyCollection<FaceType>(faces);
		private static int indexMaker = 0;

		public readonly bool hasSecondLevel;
		public readonly ShortDescriptor secondLevelShortDescription;
		public readonly EpidermisType epidermisType;

		public SimpleDescriptor weakenTransformText => () => morphText(false);
		public SimpleDescriptor strengthenTransformText => () => morphText(true);

		internal virtual AttackBase attack => AttackBase.NO_ATTACK;
		internal virtual bool canAttackWith => attack != AttackBase.NO_ATTACK;

		private readonly DescriptorWithArg<bool> morphText;

		private readonly FacialStructure firstLevelFacialStructure;
		private readonly FacialStructure secondLevelFacialStructure;

		public virtual Tones FacialSkinTone(BodyData body) => body.mainSkin.tone;

		public bool HasMuzzle(bool isSecondLevel)
		{
			return hasSecondLevel && isSecondLevel ? secondLevelFacialStructure == FacialStructure.MUZZLE : firstLevelFacialStructure == FacialStructure.MUZZLE;
		}

		public bool HasBeak(bool isSecondLevel)
		{
			return hasSecondLevel && isSecondLevel ? secondLevelFacialStructure == FacialStructure.BEAK : firstLevelFacialStructure == FacialStructure.BEAK;
		}

		public bool IsHumanoid(bool isSecondLevel)
		{
			return hasSecondLevel && isSecondLevel ? secondLevelFacialStructure == FacialStructure.HUMANOID : firstLevelFacialStructure == FacialStructure.HUMANOID;
		}

		private protected FaceType(EpidermisType epidermisType, ShortDescriptor firstLevelShortDesc, ShortDescriptor secondLevelShortDesc,
			FacialStructure firstLevelStructure, FacialStructure secondLevelStructure, DescriptorWithArg<bool> strengthenWeakenMorphText,
			PartDescriptor<FaceData> longDesc, PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform,
			RestoreType<FaceData> restore) : base(firstLevelShortDesc, longDesc, playerStr, transform, restore)
		{
			_index = indexMaker++;
			secondLevelShortDescription = secondLevelShortDesc;
			morphText = strengthenWeakenMorphText;
			hasSecondLevel = true;

			firstLevelFacialStructure = firstLevelStructure;
			secondLevelFacialStructure = secondLevelStructure;

			faces.AddAt(this, _index);
		}

		private protected FaceType(EpidermisType epidermisType, FacialStructure structure,
			ShortDescriptor shortDesc, PartDescriptor<FaceData> longDesc, PlayerBodyPartDelegate<Face> playerStr,
			ChangeType<FaceData> transform, RestoreType<FaceData> restore) : base(shortDesc, longDesc, playerStr, transform, restore)
		{
			_index = indexMaker++;
			secondLevelShortDescription = shortDesc;
			morphText = x => "";
			hasSecondLevel = false;

			firstLevelFacialStructure = structure;
			secondLevelFacialStructure = structure;

			faces.AddAt(this, _index);
		}


		//by default, just converts it to first level.
		internal virtual bool MorphStrengthPostTransform(FaceType previousType, bool previousWasFullMorph)
		{
			return false;
		}

		internal virtual bool canChangeComplexion => true;
		internal virtual string buttonText() => FaceStr();
		internal virtual string locationDesc(out bool isPlural) => YourFaceStr(out isPlural);



		internal abstract EpidermalData ParseEpidermis(BodyData bodyData, bool isFullMorph, SkinTexture complexion);
		internal virtual EpidermalData ParseSecondaryEpidermis(BodyData bodyData, bool isFullMorph, SkinTexture complexion)
		{
			return new EpidermalData();
		}

		public override int id => _index;

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

		public FacialStructure GetFacialStructure(bool isFullMorph)
		{
			return isFullMorph ? secondLevelFacialStructure : firstLevelFacialStructure;
		}

		public static readonly FaceType HUMAN = new ToneFace(EpidermisType.SKIN, FacialStructure.HUMANOID, HumanShortDesc, HumanLongDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly FaceType HORSE = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultHorseFur, FacialStructure.MUZZLE, HorseShortDesc, HorseLongDesc, HorsePlayerStr,
			HorseTransformStr, HorseRestoreStr); //muzzle.
		public static readonly FaceType DOG = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultDogFur, FacialStructure.MUZZLE, DogShortDesc, DogLongDesc, DogPlayerStr, DogTransformStr,
			DogRestoreStr); //muzzle
		public static readonly FaceType COW_MINOTAUR = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultCowFur, FacialStructure.HUMANOID, FacialStructure.MUZZLE, CowShortDesc,
			MinotaurShortDesc, CowMorphText, Cow_MinotaurLongDesc, Cow_MinotaurPlayerStr, Cow_MinotaurTransformStr, Cow_MinotaurRestoreStr); //muzzle, second level.
		public static readonly FaceType SHARK = new SharkFace();
		public static readonly FaceType SNAKE = new SnakeFace();
		public static readonly FaceType CAT = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultCatFur, FacialStructure.HUMANOID, FacialStructure.MUZZLE, CatGirlShortDesc,
			CatMorphShortDesc, CatMorphText, CatLongDesc, CatPlayerStr, CatTransformStr, CatRestoreStr); //muzzle, second level.
		public static readonly FaceType LIZARD = new MultiToneFace(EpidermisType.SCALES, DefaultValueHelpers.defaultLizardTone, FacialStructure.MUZZLE, LizardShortDesc, LizardLongDesc,
			LizardPlayerStr, LizardTransformStr, LizardRestoreStr); //muzzle.);
		public static readonly FaceType BUNNY = new BunnyMouseFace(DefaultValueHelpers.defaultBunnyFur, FacialStructure.MUZZLE, BunnyFirstLevelShortDesc,
			BunnySecondLevelShortDesc, BunnyMorphText, BunnyLongDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static readonly FaceType KANGAROO = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultKangarooFacialFur, FacialStructure.MUZZLE, KangarooShortDesc,
			KangarooLongDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly FaceType SPIDER = new SpiderFace();
		public static readonly FaceType FOX = new FoxFace();
		public static readonly FaceType DRAGON = new MultiToneFace(EpidermisType.SCALES, DefaultValueHelpers.defaultDragonTone, FacialStructure.MUZZLE, DragonShortDesc, DragonLongDesc, DragonPlayerStr,
			DragonTransformStr, DragonRestoreStr);
		public static readonly FaceType RACCOON = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultRaccoonFur, FacialStructure.HUMANOID, FacialStructure.UNIQUE, RaccoonMaskShortDesc,
			RaccoonFaceShortDesc, RaccoonMorphText, RaccoonLongDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr);
		public static readonly FaceType MOUSE = new BunnyMouseFace(DefaultValueHelpers.defaultMouseFur, FacialStructure.UNIQUE, MouseTeethShortDesc, MouseFaceShortDesc,
			MouseMorphText, MouseLongDesc, MousePlayerStr, MouseTransformStr, MouseRestoreStr);
		public static readonly FaceType FERRET = new FerretFace();
		public static readonly FaceType PIG = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultPigFur, FacialStructure.UNIQUE, FacialStructure.UNIQUE, PigShortDesc, BoarShortDesc, PigMorphText,
				PigLongDesc, PigPlayerStr, PigTransformStr, PigRestoreStr); //both pig and boar are not humanoid.
		public static readonly FaceType RHINO = new ToneFace(EpidermisType.SKIN, FacialStructure.UNIQUE, RhinoShortDesc, RhinoLongDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr); //muzzle
		public static readonly FaceType ECHIDNA = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultEchidnaFur, FacialStructure.MUZZLE, EchidnaShortDesc, EchidnaLongDesc,
			EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly FaceType DEER = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultDeerFur, FacialStructure.MUZZLE, DeerShortDesc, DeerLongDesc, DeerPlayerStr,
			DeerTransformStr, DeerRestoreStr); //muzzle
		public static readonly FaceType WOLF = new FurFace(EpidermisType.FUR, DefaultValueHelpers.defaultWolfFacialFur, FacialStructure.MUZZLE, WolfShortDesc, WolfLongDesc,
			WolfPlayerStr, WolfTransformStr, WolfRestoreStr);//muzzle
		public static readonly FaceType COCKATRICE = new CockatriceFace();
		public static readonly FaceType RED_PANDA = new MultiFurFace(EpidermisType.FUR, DefaultValueHelpers.defaultRedPandaFur, DefaultValueHelpers.defaultRedPandaFaceEarTailFur,
			FacialStructure.MUZZLE, PandaShortDesc, PandaLongDesc, PandaPlayerStr, PandaTransformStr, PandaRestoreStr);
		//new type, broken out from standard because goo is now a full set or body parts.
		public static readonly FaceType GOO = new ToneFace(EpidermisType.GOO, FacialStructure.UNIQUE, GooShortDesc, GooLongDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		//placeholder.
		//public static readonly FaceType BEAK = new FurFace(EpidermisType.FEATHERS, new FurColor(HairFurColors.WHITE), FacialStructure.BEAK, BeakShortDesc, BeakLongDesc,
		//	BeakPlayerStr, BeakTransformStr, BeakRestoreStr);

		private class ToneFace : FaceType
		{
			public ToneFace(ToneBasedEpidermisType epidermisType, FacialStructure structure, ShortDescriptor shortDesc, PartDescriptor<FaceData> longDesc,
				PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform, RestoreType<FaceData> restore)
				: base(epidermisType, structure, shortDesc, longDesc, playerStr, transform, restore) { }

			public ToneFace(ToneBasedEpidermisType epidermisType, FacialStructure firstLevelStructure, FacialStructure secondLevelStructure, ShortDescriptor firstLevelShortDesc,
				ShortDescriptor secondLevelShortDesc, DescriptorWithArg<bool> strengthenWeakenMorphText, PartDescriptor<FaceData> longDesc, PlayerBodyPartDelegate<Face> playerStr,
				ChangeType<FaceData> transform, RestoreType<FaceData> restore) : base(epidermisType, firstLevelShortDesc, secondLevelShortDesc, firstLevelStructure,
					secondLevelStructure, strengthenWeakenMorphText, longDesc, playerStr, transform, restore)
			{ }

			protected ToneBasedEpidermisType primaryEpidermis => (ToneBasedEpidermisType)epidermisType;

			internal override EpidermalData ParseEpidermis(BodyData bodyData, bool isFullMorph, SkinTexture complexion)
			{
				return new EpidermalData(primaryEpidermis, bodyData.mainSkin.tone, complexion);
			}
		}

		private class MultiToneFace : ToneFace
		{
			protected readonly Tones defaultSecondaryTone;

			public MultiToneFace(ToneBasedEpidermisType epidermisType, Tones secondaryToneFallback, FacialStructure structure, ShortDescriptor shortDesc,
				PartDescriptor<FaceData> longDesc, PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform, RestoreType<FaceData> restore)
				: base(epidermisType, structure, shortDesc, longDesc, playerStr, transform, restore)
			{
				defaultSecondaryTone = secondaryToneFallback;
			}


			public MultiToneFace(ToneBasedEpidermisType epidermisType, Tones secondaryToneFallback, FacialStructure firstLevelStructure, FacialStructure secondLevelStructure,
				ShortDescriptor firstLevelShortDesc, ShortDescriptor secondLevelShortDesc, DescriptorWithArg<bool> strengthenWeakenMorphText, PartDescriptor<FaceData> longDesc,
				PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform, RestoreType<FaceData> restore) : base(epidermisType, firstLevelStructure,
					secondLevelStructure, firstLevelShortDesc, secondLevelShortDesc, strengthenWeakenMorphText, longDesc, playerStr, transform, restore)
			{
				defaultSecondaryTone = secondaryToneFallback;
			}

			internal override EpidermalData ParseSecondaryEpidermis(BodyData bodyData, bool isFullMorph, SkinTexture complexion)
			{
				if (bodyData.supplementary.tone.isEmpty && defaultSecondaryTone.isEmpty)
				{
					return new EpidermalData();
				}
				else
				{
					Tones tone = !bodyData.supplementary.tone.isEmpty ? bodyData.supplementary.tone : defaultSecondaryTone;
					SkinTexture texture = bodyData.supplementary.usesTone ? bodyData.supplementary.skinTexture : SkinTexture.NONDESCRIPT;
					return new EpidermalData(primaryEpidermis, tone, texture);
				}
			}
		}

		private class FurFace : FaceType
		{
			public readonly FurColor defaultColor;

			protected FurBasedEpidermisType primaryEpidermis => (FurBasedEpidermisType)epidermisType;

			public FurFace(FurBasedEpidermisType epidermisType, FurColor fallbackColor, FacialStructure structure, ShortDescriptor shortDesc, PartDescriptor<FaceData> longDesc,
				PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform, RestoreType<FaceData> restore)
				: base(epidermisType, structure, shortDesc, longDesc, playerStr, transform, restore)
			{
				defaultColor = fallbackColor;
			}

			public FurFace(FurBasedEpidermisType epidermisType, FurColor fallbackColor, FacialStructure firstLevelStructure, FacialStructure secondLevelStructure,
				ShortDescriptor firstLevelShortDesc, ShortDescriptor secondLevelShortDesc, DescriptorWithArg<bool> strengthenWeakenMorphText, PartDescriptor<FaceData> longDesc,
				PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform, RestoreType<FaceData> restore) : base(epidermisType, firstLevelShortDesc,
					secondLevelShortDesc, firstLevelStructure, secondLevelStructure, strengthenWeakenMorphText, longDesc, playerStr, transform, restore)
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

			public MultiFurFace(FurBasedEpidermisType epidermisType, FurColor fallbackColor, FurColor secondaryFallbackColor, FacialStructure structure, ShortDescriptor shortDesc,
				PartDescriptor<FaceData> longDesc, PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform, RestoreType<FaceData> restore)
				: base(epidermisType, fallbackColor, structure, shortDesc, longDesc, playerStr, transform, restore)
			{
				secondaryDefaultColor = secondaryFallbackColor;
			}

			public MultiFurFace(FurBasedEpidermisType epidermisType, FurColor fallbackColor, FurColor secondaryFallbackColor, FacialStructure firstLevelStructure,
				FacialStructure secondLevelStructure, ShortDescriptor firstLevelShortDesc, ShortDescriptor secondLevelShortDesc, DescriptorWithArg<bool> strengthenWeakenMorphText,
				PartDescriptor<FaceData> longDesc, PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform, RestoreType<FaceData> restore)
				: base(epidermisType, fallbackColor, firstLevelStructure, secondLevelStructure, firstLevelShortDesc, secondLevelShortDesc, strengthenWeakenMorphText, longDesc, playerStr, transform, restore)
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
				else if (!bodyData.hairColor.isEmpty && !primary.fur.IsIdenticalTo(bodyData.hairColor))
				{
					color = new FurColor(bodyData.hairColor);
				}

				return new EpidermalData(primaryEpidermis, color, texture);
			}
		}

		private sealed class SpiderFace : ToneFace
		{
			public SpiderFace() : base(EpidermisType.SKIN, FacialStructure.HUMANOID, SpiderShortDesc, SpiderLongDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr) { }

			internal override AttackBase attack => _attack;
			private static readonly AttackBase _attack = new SpiderBite();
		}
		private sealed class SharkFace : ToneFace
		{
			public SharkFace() : base(EpidermisType.SKIN, FacialStructure.HUMANOID, SharkShortDesc, SharkLongDesc, SharkPlayerStr, SharkTransformStr, SharkRestoreStr) { }

			internal override AttackBase attack => _attack;
			private static readonly AttackBase _attack = new GenericBite(SharkShortDesc, 4);
		}
		private sealed class SnakeFace : ToneFace
		{
			public SnakeFace() : base(EpidermisType.SCALES, FacialStructure.HUMANOID, SnakeShortDesc, SnakeLongDesc, SnakePlayerStr, SnakeTransformStr, SnakeRestoreStr) { }

			internal override AttackBase attack => _attack;
			private static readonly AttackBase _attack = new NagaBite();
		}

		private class BunnyMouseFace : FurFace
		{
			public BunnyMouseFace(FurColor fallbackColor, FacialStructure secondLevelStructure, ShortDescriptor firstLevelShortDesc, ShortDescriptor secondLevelShortDesc,
				DescriptorWithArg<bool> strengthenWeakenMorphText, PartDescriptor<FaceData> longDesc, PlayerBodyPartDelegate<Face> playerStr, ChangeType<FaceData> transform, RestoreType<FaceData> restore)
				: base(EpidermisType.FUR, fallbackColor, FacialStructure.HUMANOID, secondLevelStructure, firstLevelShortDesc, secondLevelShortDesc, strengthenWeakenMorphText, longDesc, playerStr, transform, restore)
			{ }

			//overridden - since bunny and mouse are basically the same (except one has a longer nose, i guess) i'm keeping the old level.
			internal override bool MorphStrengthPostTransform(FaceType previousType, bool previousWasFullMorph)
			{
				if (previousType == BUNNY || previousType == MOUSE)
				{
					return previousWasFullMorph;
				}
				return base.MorphStrengthPostTransform(previousType, previousWasFullMorph);
			}
		}

		private sealed class CockatriceFace : FurFace
		{
			private readonly ToneBasedEpidermisType secondaryEpidermis = EpidermisType.SCALES;
			private readonly Tones defaultUnderTone = DefaultValueHelpers.defaultCockatriceScaleTone;
			public CockatriceFace() : base(EpidermisType.FEATHERS, DefaultValueHelpers.defaultCockatricePrimaryFeathers, FacialStructure.BEAK, CockatriceShortDesc,
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

		private sealed class FoxFace : MultiFurFace
		{
			private FurColor defaultKitsuneFur => DefaultValueHelpers.defaultKitsuneFacialFur;
			public FoxFace() : base(EpidermisType.FUR, DefaultValueHelpers.defaultFoxFacialFur, DefaultValueHelpers.defaultFoxSecondaryFacialFur, FacialStructure.HUMANOID,
				FacialStructure.MUZZLE, KitsuneShortDesc, FoxShortDesc, FoxMorphText, FoxLongDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr)
			{ }

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

			internal override EpidermalData ParseSecondaryEpidermis(BodyData bodyData, bool isFullMorph, SkinTexture complexion)
			{
				if (!isFullMorph)
				{
					return new EpidermalData();
				}
				else
				{
					return base.ParseSecondaryEpidermis(bodyData, isFullMorph, complexion);
				}
			}
		}

		private sealed class FerretFace : MultiFurFace
		{
			public FerretFace() : base(EpidermisType.FUR, DefaultValueHelpers.defaultFerretFur, DefaultValueHelpers.defaultFerretSecondaryFacialFur, FacialStructure.HUMANOID,
			FacialStructure.MUZZLE, FerretMaskShortDesc, FerretFaceShortDesc, FerretMorphText, FerretLongDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr)
			{ }

			internal override EpidermalData ParseEpidermis(BodyData bodyData, bool isFullMorph, SkinTexture complexion)
			{
				FurColor color = defaultColor;
				FurTexture texture = FurTexture.SOFT;
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
	}

	public static class FaceHelpers
	{
		public static PiercingJewelry GenerateEyebrowJewelry(this Face face, EyebrowPiercingLocation location, JewelryType jewelryType, JewelryMaterial jewelryMaterial)
		{
			if (face.eyebrowPiercings.CanWearGenericJewelryOfType(location, jewelryType))
			{
				return new GenericPiercing(jewelryType, jewelryMaterial);
			}
			return null;
		}

		public static PiercingJewelry GenerateLipJewelry(this Face face, LipPiercingLocation location, JewelryType jewelryType, JewelryMaterial jewelryMaterial)
		{
			if (face.lipPiercings.CanWearGenericJewelryOfType(location, jewelryType))
			{
				return new GenericPiercing(jewelryType, jewelryMaterial);
			}
			return null;
		}

		public static PiercingJewelry GenerateNoseJewelry(this Face face, NosePiercingLocation location, JewelryType jewelryType, JewelryMaterial jewelryMaterial)
		{
			if (face.nosePiercings.CanWearGenericJewelryOfType(location, jewelryType))
			{
				return new GenericPiercing(jewelryType, jewelryMaterial);
			}
			return null;
		}
	}

	public sealed class FaceData : FullBehavioralData<FaceData, Face, FaceType>
	{
		public readonly bool isFullMorph;

		public readonly EpidermalData primaryEpidermis;
		public readonly EpidermalData secondaryEpidermis;
		public readonly SkinTexture skinTexture;

		public FacialStructure facialStructure => type.GetFacialStructure(isFullMorph);

		public readonly ReadOnlyPiercing<LipPiercingLocation> lipPiercings;
		public readonly ReadOnlyPiercing<NosePiercingLocation> nosePiercings;
		public readonly ReadOnlyPiercing<EyebrowPiercingLocation> eyebrowPiercings;

		public readonly ReadOnlyTattooablePart<FaceTattooLocation> tattoos;

		#region Sexual Metadata
		public readonly uint totalOralCount;
		public readonly uint selfOralCount;
		public readonly uint orgasmCount;
		public readonly uint dryOrgasmCount;
		#endregion

		public override string ShortDescription()
		{
			if (isFullMorph)
			{
				return type.secondLevelShortDescription();
			}
			else
			{
				return type.ShortDescription();
			}
		}

		public bool isHumanoid => type.IsHumanoid(isFullMorph);

		public bool hasBeak => type.HasBeak(isFullMorph);

		public bool hasMuzzle => type.HasMuzzle(isFullMorph);

		public bool hasSecondLevel => type.hasSecondLevel;

		public override FaceData AsCurrentData()
		{
			return this;
		}

		public FaceData(Face source) : base(GetID(source), GetBehavior(source))
		{
			isFullMorph = source.isFullMorph;
			primaryEpidermis = source.primary;
			secondaryEpidermis = source.secondary;
			skinTexture = source.skinTexture;

			lipPiercings = source.lipPiercings.AsReadOnlyData();
			nosePiercings = source.nosePiercings.AsReadOnlyData();
			eyebrowPiercings = source.eyebrowPiercings.AsReadOnlyData();

			tattoos = source.tattoos.AsReadOnlyData();

			totalOralCount = source.totalOralCount;
			selfOralCount = source.selfOralCount;
			orgasmCount = source.orgasmCount;
			dryOrgasmCount = source.dryOrgasmCount;
		}
	}
}
