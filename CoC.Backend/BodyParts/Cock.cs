//Cock.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:55 PM

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Wearables.Accessories.CockSocks;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	public enum CockGroup { HUMAN, MAMMALIAN, CORRUPTED, AQUATIC, REPTILIAN, FLYING, OTHER }

	public sealed partial class CockPiercingLocation : PiercingLocation, IEquatable<CockPiercingLocation>
	{
		private static readonly List<CockPiercingLocation> _allLocations = new List<CockPiercingLocation>();

		public static readonly ReadOnlyCollection<CockPiercingLocation> allLocations;

		private readonly byte index;

		static CockPiercingLocation()
		{
			allLocations = new ReadOnlyCollection<CockPiercingLocation>(_allLocations);
		}

		private CockPiercingLocation(byte index, CompatibleWith allowsJewelryOfType, SimpleDescriptor btnText, SimpleDescriptor locationDesc)
			: base(allowsJewelryOfType, btnText, locationDesc)
		{
			this.index = index;


			if (!_allLocations.Contains(this))
			{
				_allLocations.Add(this);
			}
		}



		#region Useful Functions
		public bool Equals(CockPiercingLocation other)
		{
			return !(other is null) && other.index == index;
		}

		public override int GetHashCode()
		{
			return index.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is CockPiercingLocation cockPiercing)
			{
				return Equals(cockPiercing);
			}
			else
			{
				return false;
			}
		}
		#endregion

		#region Implementations

		public static readonly CockPiercingLocation PRINCE_ALBERT = new CockPiercingLocation(0, AlbertAllows, AlbertButton, AlbertLocation);
		public static readonly CockPiercingLocation FRENUM_UPPER_1 = new CockPiercingLocation(1, FrenumAllows, UpperFrenum1Button, UpperFrenum1Location);
		public static readonly CockPiercingLocation FRENUM_UPPER_2 = new CockPiercingLocation(2, FrenumAllows, UpperFrenum2Button, UpperFrenum2Location);
		public static readonly CockPiercingLocation FRENUM_MIDDLE_1 = new CockPiercingLocation(3, FrenumAllows, MiddleFrenum1Button, MiddleFrenum1Location);
		public static readonly CockPiercingLocation FRENUM_MIDDLE_2 = new CockPiercingLocation(4, FrenumAllows, MiddleFrenum2Button, MiddleFrenum2Location);
		public static readonly CockPiercingLocation FRENUM_MIDDLE_3 = new CockPiercingLocation(5, FrenumAllows, MiddleFrenum3Button, MiddleFrenum3Location);
		public static readonly CockPiercingLocation FRENUM_MIDDLE_4 = new CockPiercingLocation(6, FrenumAllows, MiddleFrenum4Button, MiddleFrenum4Location);
		public static readonly CockPiercingLocation FRENUM_LOWER_1 = new CockPiercingLocation(7, FrenumAllows, LowerFrenum1Button, LowerFrenum1Location);
		public static readonly CockPiercingLocation FRENUM_LOWER_2 = new CockPiercingLocation(8, FrenumAllows, LowerFrenum2Button, LowerFrenum2Location);
		#endregion
		#region Implementation Helpers
		private static bool AlbertAllows(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.BARBELL_STUD || jewelryType == JewelryType.HORSESHOE || jewelryType == JewelryType.RING || jewelryType == JewelryType.SPECIAL;
		}
		private static bool FrenumAllows(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.BARBELL_STUD || jewelryType == JewelryType.RING;
		}


		#endregion
	}

	public sealed class CockPiercing : Piercing<CockPiercingLocation>
	{
		public CockPiercing(IBodyPart source, PiercingUnlocked LocationUnlocked, GenericCreatureText playerShortDesc, GenericCreatureText playerLongDesc)
			: base(source, LocationUnlocked, playerShortDesc, playerLongDesc) { }

		public int MaxFrenumPiercings => MaxPiercings - 1;

		public override int MaxPiercings => CockPiercingLocation.allLocations.Count;

		//determines if you have at least 7 different color piercings in your cock. i think i use this for an achievement, idk.
		public bool rainbow => availableLocations.Where(x => WearingJewelryAt(x)).Select(x => this[x].jewelryMaterial.hueDescriptor()).Distinct().Count() >= 7;

		public override IEnumerable<CockPiercingLocation> availableLocations => CockPiercingLocation.allLocations;


	}

	//cock tattoos in genitals.

	//whoever wrote the AS version of cock, thank you. your cock was awesome. (Obligatory: no homo)
	//i didn't have to search everywhere for the things that were part of it.
	//well, mostly. knots were still a pain.
	//whoever decided to use the AS3 equivalent hack for an enum, though, not so much. that was some ugly ass shit.

	//Dev Note: the player descriptions for some cocks seem to suggest they could fit in vags' (or other orifices) that with a smaller capacity then their size,
	//due to being tapered or whatever. AFAIK, this isn't supported in any of the original code. It's possible to do here - simply give the cocktype class a virtual function
	//called 'fits in hole' or something and take a ushort for capacity, and a CockData for the current cock, and returns a boolean. By default, simply return true if size <= capacity.
	//any type that allows crazy shit, like maybe goo or tentacle or the dragon cock, override it and return true based on whatever conditions you want.
	//add an alias for this function in both cock and cockdata. Note that any helper functions that do this already would need to be updated too.

	//Note: this class exists after perks have been created, so it's postperk init is not called.
	public sealed partial class Cock : FullBehavioralPart<Cock, CockType, CockData>, IGrowable, IShrinkable
	{

		public override string BodyPartName() => Name();

		#region Consts

		public const double MAX_COCK_LENGTH = 240f;
		public const double MAX_COCK_THICKNESS = 50f;
		public const double MIN_COCK_THICKNESS = 0.2f;
		public const double MAX_KNOT_MULTIPLIER = 10f;
		public const double MIN_KNOT_MULTIPLIER = 1.1f;
		public const double MIN_COCK_LENGTH = 3.0f;

		public const double DEFAULT_COCK_LENGTH = 5.5f;
		//public const double DEFAULT_BIG_COCK_LENGTH = 8f;
		public const double DEFAULT_COCK_GIRTH = 1.25f;
		//public const double DEFAULT_BIG_COCK_GIRTH = 1.5f;

		public const double MIN_URETHRA_WIDTH = (double)(0.5 * Measurement.TO_INCHES);
		public const double MIN_CUM = 2;

		#endregion

		#region Properties

		private readonly CockCollection source;
		internal readonly uint collectionID;

		public int cockIndex => source.cocks.IndexOf(this);

		//perk values that alter how the cock changes size.
		private double cockGrowthMultiplier => source.cockGrowthMultiplier + (cockSock?.cockGrowthMultiplier ?? 0);
		private double cockShrinkMultiplier => source.cockShrinkMultiplier + (cockSock?.cockShrinkMultiplier ?? 0);

		//perk values that alter the size of any new cock.
		private double newCockDefaultSize => source.newCockDefaultSize;
		private double newCockSizeDelta => source.newCockSizeDelta;

		//perk value that sets the absolute minimum size for any cock. this is given priority, even over new size perk data.
		public double minCockLength => source.minCockLength;

		//perk values used to alter virility of a creature (and by extension, all of its cocks).
		private double perkBonusVirilityMultiplier => source.perkBonusVirilityMultiplier;
		private sbyte perkBonusVirility => source.perkBonusVirility;

		//used to validate the length whenever a perk changes the min or max length. will fire off length changed as needed.
		internal void ValidateLength()
		{
			double newLength = Utils.Clamp2(_cockLength, minCockLength, maxLength);
			if (newLength != _cockLength)
			{
				CockData oldData = AsReadOnlyData();
				_cockLength = newLength;
				NotifyDataChanged(oldData);
			}
		}

		private Creature creature => CreatureStore.GetCreatureClean(creatureID);

		public byte currentLust => creature?.lust ?? Creature.DEFAULT_LUST;
		public double currentRelativeLust => creature?.relativeLust ?? Creature.DEFAULT_LUST;

		private double resetLength => Math.Max(newCockDefaultSize, minCockLength);

		public readonly CockPiercing piercings;

		public double urethraWidth => Math.Max(type.UrethraWidth(girth), MIN_URETHRA_WIDTH);

		public double minCumAmount => Math.Max(MIN_CUM, (double)(Math.Pow((urethraWidth * Measurement.TO_CENTIMETERS), 2) * Math.PI * length));

		public double knotMultiplier
		{
			get => _knotMultiplier;
			private set
			{
				if (type.hasKnot)
				{
					_knotMultiplier = Utils.Clamp2(value, MIN_KNOT_MULTIPLIER, MAX_KNOT_MULTIPLIER);
				}
				else
				{
					_knotMultiplier = 0;
				}
			}
		}
		private double _knotMultiplier;

		public bool hasKnot => knotMultiplier >= 1.1f;

		public double knotSize => type.knotSize(girth, knotMultiplier);
		public double relativeKnotSize => type.relativeKnotSize(knotMultiplier);

		public double length => _cockLength;
		private double _cockLength;

		public double girth => _cockGirth;
		private double _cockGirth;
		public double area => girth * length;

		public CockSockBase cockSock { get; private set; } = null;

		#region Sexual MetaData
		public uint soundCount { get; private set; } = 0;
		public uint totalSexCount { get; private set; } = 0;
		public uint selfSexCount { get; private set; } = 0;

		public uint orgasmCount { get; private set; } = 0;
		public uint dryOrgasmCount { get; private set; } = 0;

		#endregion
		public bool hasSheath => requiresASheath || source.hasSheath;

		public bool requiresASheath => type.usesASheath;

		public double cumAmount => source.totalCum;

		public override CockType type
		{
			get => _type;
			protected set
			{
				if (_type != value && value.hasKnot != _type.hasKnot)
				{
					_knotMultiplier = value.baseKnotMultiplier;
				}
				_type = value;
			}
		}
		private CockType _type = CockType.HUMAN;

		public bool isPierced => piercings.isPierced;


		public override CockType defaultType => CockType.defaultValue;
		#endregion
		#region Constructors

		//NOTE: when called from genitals (or its members), the perk data class must already be initialized in the genitals. otherwise, this will break. if they are created during
		//the post perk init phase (or later, like in normal gameplay), which as of this writing, they are, this will never be an issue.

		internal Cock(CockCollection source, uint id) : this(source, id, CockType.defaultValue)
		{ }

		internal Cock(CockCollection parent, uint id, CockType cockType) : base(parent?.creatureID ?? throw new ArgumentNullException(nameof(parent)))
		{
			source = parent;
			collectionID = id;

			type = cockType ?? throw new ArgumentNullException(nameof(cockType));
			updateLength(NewLength());

			knotMultiplier = type.baseKnotMultiplier;

			cockSock = null;
			piercings = new CockPiercing(this, PiercingLocationUnlocked, AllCockPiercingsShort, AllCockPiercingsLong);
		}

		internal Cock(CockCollection parent, uint id, CockType cockType, double length, double girth, double? initialKnotMultiplier = null, CockSockBase cockSock = null,
			ReadOnlyDictionary<CockPiercingLocation, PiercingJewelry> piercings = null) : base(parent?.creatureID ?? throw new ArgumentNullException(nameof(parent)))
		{
			source = parent;
			collectionID = id;

			type = cockType ?? throw new ArgumentNullException(nameof(cockType));
			length = NewLength();

			updateLengthAndGirth(length, girth);

			knotMultiplier = initialKnotMultiplier ?? type.baseKnotMultiplier;


			this.cockSock = cockSock;

			this.piercings = new CockPiercing(this, PiercingLocationUnlocked, AllCockPiercingsShort, AllCockPiercingsLong);
			this.piercings.InitializePiercings(piercings);
		}

		private Cock(CockCollection parent, uint id, CockType cockType, double length, double girth,
			double? initialKnotMultiplier) : base(parent?.creatureID ?? throw new ArgumentNullException(nameof(parent)))
		{
			source = parent;
			collectionID = id;

			type = cockType ?? throw new ArgumentNullException(nameof(cockType));

			updateLengthAndGirth(length, girth);

			knotMultiplier = initialKnotMultiplier ?? type.baseKnotMultiplier;

			cockSock = null;

			piercings = new CockPiercing(this, PiercingLocationUnlocked, AllCockPiercingsShort, AllCockPiercingsLong);
		}

		private double NewLength(double? givenLength = null)
		{
			double minLength = Utils.Clamp2(Math.Max(minCockLength, newCockDefaultSize), Cock.MIN_COCK_LENGTH, Cock.MAX_COCK_LENGTH);
			if (givenLength != null)
			{
				givenLength += newCockSizeDelta;
			}
			if (givenLength is null || givenLength < minLength)
			{
				return minLength;
			}
			else
			{
				return (double)givenLength;
			}
		}

		#endregion

		#region Generate
		internal static CockData GenerateAggregate(Guid creatureID, CockType type, double averageKnotMultiplier, double averageKnotSize, double averageLength, double averageGirth,
			double cumAmount, bool currentlyHasSheath, byte currentLust, double currentRelativeLust)
		{
			return new CockData(creatureID, type, -1, averageLength, averageGirth, averageKnotMultiplier, averageKnotSize, cumAmount, currentlyHasSheath, currentLust, currentRelativeLust);
		}
		#endregion

		internal void InitializePiercings(Dictionary<CockPiercingLocation, PiercingJewelry> initialPiercings)
		{
			piercings.InitializePiercings(initialPiercings);
		}


		internal CockSockBase ChangeCockSock(CockSockBase cockSock)
		{
			var oldSock = this.cockSock;
			this.cockSock = cockSock;

			return oldSock;
		}



		public override CockData AsReadOnlyData()
		{
			return new CockData(this, cockIndex);
		}

		private void CheckDataChanged(CockData oldData)
		{
			if (length != oldData.length || girth != oldData.girth || knotMultiplier != oldData.knotMultiplier || knotSize != oldData.knotSize)
			{
				NotifyDataChanged(oldData);
			}
		}

		#region Update

		internal override bool UpdateType(CockType newType)
		{
			return UpdateCock(newType, () => SetLength(length));
		}

		internal bool UpdateCockTypeWithLength(CockType cockType, double newLength)
		{
			return UpdateCock(cockType, () => SetLength(newLength));
		}

		internal bool UpdateCockTypeWithLengthAndGirth(CockType cockType, double newLength, double newGirth)
		{
			return UpdateCock(cockType, () => SetLengthAndGirth(newLength, newGirth));
		}

		internal bool UpdateCockTypeWithKnotMultiplier(CockType cockType, double newKnotMultiplier)
		{
			return UpdateCock(cockType, () => knotMultiplier = newKnotMultiplier);
		}

		internal bool UpdateCockTypeWithAll(CockType cockType, double newLength, double newGirth, double newKnotMultiplier)
		{
			return UpdateCock(cockType, () =>
			{
				SetLengthAndGirth(newLength, newGirth);
				knotMultiplier = newKnotMultiplier;
			});
		}

		private bool UpdateCock(CockType newType, Action callback)
		{
			if (newType == null || type == newType)
			{
				return false;
			}
			CockData oldData = AsReadOnlyData();
			CockType oldType = type;
			type = newType;
			callback?.Invoke();

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}
		#endregion

		#region Restore
		//standard restore is fine.

		internal void Reset(bool resetPiercings = true)
		{
			type = CockType.HUMAN;
			double length = resetLength;
			double girth = DEFAULT_COCK_GIRTH;
			updateLengthAndGirth(length, girth);

			if (resetPiercings)
			{
				piercings.Reset();
			}
		}

		#endregion

		#region Attribute Updates

		public double IncreaseLength(double lengthenAmount = 1, bool ignorePerk = false)
		{
			if (lengthenAmount <= 0)
			{
				return 0;
			}

			double oldLength = length;
			if (!ignorePerk)
			{
				lengthenAmount *= cockGrowthMultiplier;
			}
			CockData oldData = AsReadOnlyData();
			updateLength(length + lengthenAmount);
			CheckDataChanged(oldData);
			return length - oldLength;
		}

		//calling this will not cause a cock to be removed, should it become too small. Note that perks may prevent this from shrinking beyond a certain size, so
		//you'll need to keep that in mind when checking for that.
		public double DecreaseLength(double shortenAmount = 1, bool ignorePerk = false)
		{
			if (shortenAmount <= 0)
			{
				return 0;
			}

			double oldLength = length;
			if (!ignorePerk)
			{
				shortenAmount *= cockShrinkMultiplier;
			}
			CockData oldData = AsReadOnlyData();
			updateLength(length - shortenAmount);
			CheckDataChanged(oldData);
			return oldLength - length;
		}

		//this is a variant of the decrease length function that spits out a boolean telling you if you should remove it after this procs. This disctinction is important because
		//nearly all of the code (incorrectly) assumes a minimum length of 3, as the minimum length was previously constant. As such, most code that does not want to accidently
		//cause this to occur is written in such a way that it will never cause the length to drop below 3. this, however, is no longer correct. As such, when you attempt to shrink
		//a cock below it's minimum length, it will simply stop there and return the amount it did shrink, unless you call this variant. this variant will stop at the minimum length
		//like the other check, but it will also set the should remove boolean to true when you are trying to shrink well beyond the possible length.
		//if you are certain you wish to remove the cock when it tries to shrink beyond it's minimum length, use this variant.
		public bool DecreaseLengthAndCheckIfNeedsRemoval(double shortenAmount, bool ignorePerks = false)
		{

			if (shortenAmount <= 0)
			{
				return false;
			}

			double oldLength = length;
			if (!ignorePerks)
			{
				shortenAmount *= cockShrinkMultiplier;
			}
			CockData oldData = AsReadOnlyData();
			updateLength(length - shortenAmount);
			CheckDataChanged(oldData);

			//if the amount we tried to shorten is one or more than the amount we did actually shrink, we should remove it.
			//alternatively, if it drops below the ABSOLUTE MINIMUM (3), then we should remove, regardless of amount we're trying to drop it (>0, of course)
			return oldLength - length + 1 < shortenAmount || oldLength - shortenAmount < MIN_COCK_LENGTH;
		}

		public double SetLength(double newLength)
		{
			CockData oldData = AsReadOnlyData();
			updateLength(newLength);
			CheckDataChanged(oldData);
			return length;
		}

		public double IncreaseThickness(double thickenAmount)
		{
			double oldGirth = girth;
			CockData oldData = AsReadOnlyData();
			updateGirth(girth + thickenAmount);
			CheckDataChanged(oldData);
			return girth - oldGirth;
		}

		public double DecreaseThickness(double thinAmount)
		{
			double oldGirth = girth;
			CockData oldData = AsReadOnlyData();
			updateGirth(girth - thinAmount);
			CheckDataChanged(oldData);
			return oldGirth - girth;
		}

		public double SetGirth(double newGirth)
		{
			CockData oldData = AsReadOnlyData();
			updateGirth(newGirth);
			CheckDataChanged(oldData);
			return girth;
		}

		public Pair<double> DeltaLengthAndGirth(double lengthDelta, double girthDelta)
		{
			double oldLength = length;
			double oldGirth = girth;

			CockData oldData = AsReadOnlyData();
			updateLengthAndGirth(length + lengthDelta, girth + girthDelta);
			CheckDataChanged(oldData);

			return new Pair<double>(length - oldLength, girth - oldGirth);
		}

		public void SetLengthAndGirth(double newLength, double newGirth)
		{
			CockData oldData = AsReadOnlyData();
			updateLengthAndGirth(newLength, newGirth);
			CheckDataChanged(oldData);
		}

		public double IncreaseKnotMultiplier(double amount)
		{
			if (!type.hasKnot)
			{
				return 0;
			}
			double oldMultiplier = knotMultiplier;
			CockData oldData = AsReadOnlyData();
			knotMultiplier += amount;
			CheckDataChanged(oldData);
			return knotMultiplier - oldMultiplier;
		}

		public double DecreaseKnotMultiplier(double amount)
		{
			if (!type.hasKnot)
			{
				return 0;
			}
			double oldMultiplier = knotMultiplier;
			CockData oldData = AsReadOnlyData();
			knotMultiplier -= amount;
			CheckDataChanged(oldData);
			return oldMultiplier - knotMultiplier;
		}

		public double SetKnotMultiplier(double multiplier)
		{
			Utils.Clamp(ref multiplier, MIN_KNOT_MULTIPLIER, MAX_KNOT_MULTIPLIER);
			if (type.hasKnot)
			{
				CockData oldData = AsReadOnlyData();
				knotMultiplier = multiplier;
				CheckDataChanged(oldData);
			}
			return knotMultiplier;
		}

		public void SetAll(double newLength, double newGirth, double newKnotMultiplier)
		{
			CockData oldData = AsReadOnlyData();
			updateLengthAndGirth(newLength, newGirth);
			Utils.Clamp(ref newKnotMultiplier, MIN_KNOT_MULTIPLIER, MAX_KNOT_MULTIPLIER);
			if (type.hasKnot)
			{
				knotMultiplier = newKnotMultiplier;
			}
			CheckDataChanged(oldData);
		}
		#endregion
		#region Sex-Related
		internal void SoundCock(double penetratorLength, double penetratorWidth, double penetratorKnotSize, bool reachOrgasm)
		{
			soundCount++;
			//i guess we could do stuff with the cock being sore af or whatever, but whatever.
		}

		internal void DoSex(bool reachOrgasm, bool withSelf)
		{
			totalSexCount++;

			if (withSelf)
			{
				selfSexCount++;
			}

			if (reachOrgasm)
			{
				orgasmCount++;
			}
		}

		internal void OrgasmGeneric(bool dryOrgasm)
		{
			orgasmCount++;
			if (dryOrgasm)
			{
				dryOrgasmCount++;
			}
		}


		#endregion
		public override bool IsIdenticalTo(CockData original, bool ignoreSexualMetaData)
		{
			return !(original is null) && type == original.type && length == original.length && girth == original.girth
				&& this.hasKnot == original.hasKnot && (!hasKnot || this.knotMultiplier == original.knotMultiplier) && cockSock.Equals(original.cockSock)
				&& cumAmount == original.cumAmount && piercings.IsIdenticalTo(original.cockPiercings)
				&& (ignoreSexualMetaData || (orgasmCount == original.orgasmCount && dryOrgasmCount == original.dryOrgasmCount
				&& soundCount == original.soundCount && totalSexCount == original.totalSexCount && selfSexCount == original.selfSexCount));
		}
		#region Validate
		internal override bool Validate(bool correctInvalidData)
		{
			bool valid = true;
			CockType cockType = type;
			valid = CockType.Validate(ref cockType, correctInvalidData);
			//should auto-Validate length and girth.
			type = cockType;
			if (valid || correctInvalidData)
			{
				valid &= piercings.Validate(correctInvalidData);
			}
			//give length priority.
			if (valid || correctInvalidData)
			{
				updateLengthAndGirth(length, girth);
			}
			return valid;
		}
		#endregion
		#region Unique Functions

#warning implement cocksock equip and remove and replace.

		#endregion
		#region Text

		public string HeadDescription() => type.HeadDescription();

		public string AdjectiveText(bool multipleAdjectives)
		{
			return CockType.CockAdjectiveText(AsReadOnlyData(), multipleAdjectives);
		}


		public string ShortDescriptionNoAdjective() => type.ShortDescriptionNoAdjective();
		public string ShortDescriptionNoAdjective(bool plural) => type.ShortDescriptionNoAdjective(plural);

		public string ShortDescription(bool noAdjective, bool plural) => type.ShortDescription(noAdjective, plural);


		public string FullDescription(bool alternateFormat) => type.FullDescription(AsReadOnlyData(), alternateFormat);

		public string FullDescription() => type.FullDescription(AsReadOnlyData());

		public string FullDescriptionPrimary() => type.FullDescriptionPrimary(AsReadOnlyData());

		public string FullDescriptionAlternate() => type.FullDescriptionAlternate(AsReadOnlyData());

		#endregion

		#region Piercings
		private bool PiercingLocationUnlocked(CockPiercingLocation piercingLocation, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		#endregion
		#region IGrowShrinkable

		bool IShrinkable.CanReducto()
		{
			return area > 6;
		}

		double IShrinkable.UseReducto()
		{
			if (!((IShrinkable)this).CanReducto())
			{
				return 0;
			}
			double oldLength = length;
			double multiplier = 2.0f / 3 * cockShrinkMultiplier;
			updateLength(length * multiplier);
			return oldLength - length;

		}

		bool IGrowable.CanGroPlus()
		{
			return length < MAX_COCK_LENGTH;
		}

		//grows cock 1-2 inches, in increments of 0.25.
		//automatically increases cockGirth to min value.
		//if possible, will also increase cockGirth, up to 0.5 inches
		double IGrowable.UseGroPlus()
		{
			if (!((IGrowable)this).CanGroPlus())
			{
				return 0;
			}
			double oldCockLength = length;

			if (cockGrowthMultiplier != 1)
			{
				double multiplier = cockGrowthMultiplier;
				int rand;
				if (multiplier < 1)
				{
					rand = (int)Math.Floor(multiplier * 4);
				}
				else
				{
					rand = (int)Math.Ceiling(multiplier * 4) + 1;
				}
				updateLength(length + 1 + Utils.Rand(rand) / 4.0f);
			}
			else
			{
				updateLength(length + 1 + Utils.Rand(4) / 4.0f);
			}
			if ((girth + 0.5f) < maxGirth)
			{
				updateGirth(girth + 0.5f);
			}
			else if (girth < maxGirth)
			{
				updateGirth(maxGirth);
			}
			return length - oldCockLength;
		}
		#endregion
		#region Helpers
		private void updateLength(double newLength)
		{
			_cockLength = newLength;
			Utils.Clamp(ref _cockLength, minCockLength, MAX_COCK_LENGTH);
			Utils.Clamp(ref _cockGirth, minGirth, maxGirth);
		}

		private void updateGirth(double newGirth)
		{
			_cockGirth = newGirth;
			Utils.Clamp(ref _cockGirth, MIN_COCK_THICKNESS, MAX_COCK_THICKNESS);
			Utils.Clamp(ref _cockLength, minCockLength, maxLength);
		}

		private void updateLengthAndGirth(double newLength, double newGirth)
		{
			_cockLength = newLength;
			_cockGirth = newGirth;
			Utils.Clamp(ref _cockLength, minCockLength, MAX_COCK_LENGTH);
			Utils.Clamp(ref _cockGirth, minGirth, maxGirth);
		}


		private double maxGirth => _cockLength * type.maxGirthToLengthRatio;
		private double minGirth => _cockLength * type.minGirthToLengthRatio;
		private double minLength => _cockGirth / type.maxGirthToLengthRatio;
		private double maxLength => _cockGirth / type.minGirthToLengthRatio;

		public byte virility
		{
			get
			{
				double value;
				if (cumAmount < 250)
				{
					value = 1;
				}
				else if (cumAmount < 800)
				{
					value = 2;
				}
				else if (cumAmount < 1600)
				{
					value = 3;
				}
				else
				{
					value = 5;
				}

				value *= perkBonusVirilityMultiplier + perkBonusVirility;
				return (byte)Utils.Clamp2(Math.Round(value), 0, 100);
			}
		}
		#endregion
	}



	public partial class CockType : FullBehavior<CockType, Cock, CockData>
	{

		private static int indexMaker = 0;
		private static readonly List<CockType> types = new List<CockType>();
		public static readonly ReadOnlyCollection<CockType> availableTypes = new ReadOnlyCollection<CockType>(types);
		public override int id => _index;
		private readonly int _index;

		public const double MAX_INITIAL_MULTIPLIER = 2.5f;

		public static CockType defaultValue => HUMAN;

		public bool hasKnot => baseKnotMultiplier > 1.0f;
		public readonly double baseKnotMultiplier;

		//vanilla game limited cock width to 2/5 the length. i realize that some cocks may require that ratio be lower, so if it irks you, you
		//now have the option to change that.

		public virtual double maxGirthToLengthRatio => 0.4f;

		//similarly, you now have the option to mess with min ratio. by default, any cock will at the very least, have one inch in width per 10 inches in length.
		public virtual double minGirthToLengthRatio => 0.1f;

		//allows a straightforward way to check for flexible, extendable cocks, like tentacles. also allows other cocks to act like tentacles (see the newly added goo-cock)
		//i'm aware this breaks all the rules with length and girth, but tentacles need to tentacle things, so.
		//I'm hanging the lampshade in the tentacle and goo descriptions, saying that they can extend to crazy lengths,
		//but they only feel pleasure at the tip (which happens to be length long and girth wide)
		public virtual bool flexibleOrStretchyCock => false;

		//you can now alter the formula for knots. Iirc, the text for dragon cock knots said it didn't swell with size like the others, but still used the formula. if it's important enough to you to change this,
		//you can.
		public double knotSize(double girth, double multiplier) => girth * relativeKnotSize(multiplier);

		public virtual double relativeKnotSize(double baseMultiplier) => baseMultiplier;

		public readonly bool usesASheath;

		protected delegate string CockDescriptor(bool noAdjective, bool plural = false);
		protected delegate string CockSingleDescriptor(bool noAdjective);

		//a convenient descriptor for when you grow a cock, not transform it. cocks are weird because you can get more of them, not just transform them.
		//generally, you'll want to mention any other cocks you already have, and the effect on your gender (i.e. if it makes you a herm or a male from female/genderless)
		protected delegate string GrowCockDescriptor(PlayerBase player, byte grownCockIndex);
		protected delegate string RemoveCockDescriptor(CockData removedCock, PlayerBase player);

		//some cocks have a built-in adjective to their short description - this is the way it was before. This can cause grammatic weirdness, so cocks use a custom delegate that
		//allows this adjective to be disabled, in addition to the plural flag. just like vagina, cock defaults to singular instead of plural.
		private readonly CockDescriptor shortDescWithAdjArg;
		private readonly CockSingleDescriptor singleDescWithAdjArg;

		private readonly GrowCockDescriptor grewCockStr;
		private readonly RemoveCockDescriptor removedCockStr;
		private readonly SimpleDescriptor headStr;

		public string HeadDescription()
		{
			return headStr();
		}

		//aliased in the genitals class.
		//you may choose to use your own that is unique to whatever is causing it, but this exists for simplicity's sake.
		internal string GrewCockText(PlayerBase player, byte grownCockIndex) => grewCockStr(player, grownCockIndex);
		public string RemovedCockText(CockData removedCock, PlayerBase player) => removedCockStr(removedCock, player);

		private protected CockType(CockGroup cockGroup, bool hasSheath, SimpleDescriptor headDesc,
			CockDescriptor shortDescWithAdjFlag, CockSingleDescriptor singleDescWithAdjFlag, PartDescriptor<CockData> longDesc, PlayerBodyPartDelegate<Cock> playerDesc,
			ChangeType<CockData> transform, GrowCockDescriptor growCockText, RestoreType<CockData> restore, RemoveCockDescriptor removeCockText)
			: base(shortDescMaker(shortDescWithAdjFlag), singleDescMaker(singleDescWithAdjFlag), longDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			baseKnotMultiplier = 0;

			usesASheath = hasSheath;

			shortDescWithAdjArg = shortDescWithAdjFlag ?? throw new ArgumentNullException(nameof(shortDescWithAdjFlag));
			singleDescWithAdjArg = singleDescWithAdjFlag ?? throw new ArgumentNullException(nameof(singleDescWithAdjFlag));

			types.AddAt(this, _index);

			headStr = headDesc ?? throw new ArgumentNullException(nameof(headDesc));
			grewCockStr = growCockText ?? throw new ArgumentNullException(nameof(growCockText));
			removedCockStr = removeCockText ?? throw new ArgumentNullException(nameof(removeCockText));
		}

		private protected CockType(CockGroup cockGroup, bool hasSheath, double initialKnotMultiplier, SimpleDescriptor headDesc,
			CockDescriptor shortDescWithAdjFlag, CockSingleDescriptor singleDescWithAdjFlag, PartDescriptor<CockData> longDesc, PlayerBodyPartDelegate<Cock> playerDesc,
			ChangeType<CockData> transform, GrowCockDescriptor growCockText, RestoreType<CockData> restore, RemoveCockDescriptor removeCockText)
			: base(shortDescMaker(shortDescWithAdjFlag), singleDescMaker(singleDescWithAdjFlag), longDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			Utils.Clamp(ref initialKnotMultiplier, Cock.MIN_KNOT_MULTIPLIER, MAX_INITIAL_MULTIPLIER);
			baseKnotMultiplier = initialKnotMultiplier;
			usesASheath = hasSheath;

			shortDescWithAdjArg = shortDescWithAdjFlag ?? throw new ArgumentNullException(nameof(shortDescWithAdjFlag));
			singleDescWithAdjArg = singleDescWithAdjFlag ?? throw new ArgumentNullException(nameof(singleDescWithAdjFlag));

			types.AddAt(this, _index);

			headStr = headDesc ?? throw new ArgumentNullException(nameof(headDesc));
			grewCockStr = growCockText ?? throw new ArgumentNullException(nameof(growCockText));
			removedCockStr = removeCockText ?? throw new ArgumentNullException(nameof(removeCockText));
		}

		private static SimpleDescriptor shortDescMaker(CockDescriptor shortAdjDesc)
		{
			if (shortAdjDesc is null)
			{
				throw new ArgumentNullException(nameof(shortAdjDesc));
			}

			return () => shortAdjDesc(false, false);
		}

		private static SimpleDescriptor singleDescMaker(CockSingleDescriptor singleDesc)
		{
			if (singleDesc is null)
			{
				throw new ArgumentNullException(nameof(singleDesc));
			}

			return () => singleDesc(false);
		}

		public string ShortDescriptionNoAdjective() => shortDescWithAdjArg(true);
		public string ShortDescriptionNoAdjective(bool plural) => shortDescWithAdjArg(true, plural);

		public string ShortDescription(bool noAdjective, bool plural) => shortDescWithAdjArg(noAdjective, plural);


		public virtual string FullDescription(CockData cock, bool alternateFormat)
		{
			return GenericFullDescription(cock, alternateFormat);
		}

		public string FullDescription(CockData cock)
		{
			return FullDescription(cock, false);
		}

		public string FullDescriptionPrimary(CockData data)
		{
			return FullDescription(data, false);
		}

		public string FullDescriptionAlternate(CockData data)
		{
			return FullDescription(data, true);
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

		internal virtual double UrethraWidth(double girth)
		{
			return girth / 8f;
		}

		//note: fox existed in the original enum but was never used. i mean, ever. so for now, it's still here, just commented out. feel free to implement it if it ever gets a use.

		public static readonly CockType HUMAN = new CockType(CockGroup.HUMAN, false, HumanCockHeadDesc, HumanDesc, HumanSingleDesc, HumanLongDesc, HumanPlayerStr, HumanTransformStr, HumanGrewCockStr, HumanRestoreStr, HumanRemoveCockStr);
		public static readonly CockType HORSE = new CockType(CockGroup.MAMMALIAN, true, HorseCockHeadDesc, HorseDesc, HorseSingleDesc, HorseLongDesc, HorsePlayerStr, HorseTransformStr, HorseGrewCockStr, HorseRestoreStr, HorseRemoveCockStr);
		public static readonly CockType DOG = new CockType(CockGroup.MAMMALIAN, true, 1.1f, DogCockHeadDesc, DogDesc, DogSingleDesc, DogLongDesc, DogPlayerStr, DogTransformStr, DogGrewCockStr, DogRestoreStr, DogRemoveCockStr);// can range up to 2.1 depending on item.
		public static readonly CockType DEMON = new CockType(CockGroup.CORRUPTED, false, DemonCockHeadDesc, DemonDesc, DemonSingleDesc, DemonLongDesc, DemonPlayerStr, DemonTransformStr, DemonGrewCockStr, DemonRestoreStr, DemonRemoveCockStr);
		public static readonly CockType TENTACLE = new FlexiCock(CockGroup.CORRUPTED, false, 1.1f, TentacleCockHeadDesc, TentacleDesc, TentacleSingleDesc, TentacleLongDesc, TentaclePlayerStr, TentacleTransformStr, TentacleGrewCockStr, TentacleRestoreStr, TentacleRemoveCockStr);
		public static readonly CockType CAT = new CockType(CockGroup.MAMMALIAN, true, CatCockHeadDesc, CatDesc, CatSingleDesc, CatLongDesc, CatPlayerStr, CatTransformStr, CatGrewCockStr, CatRestoreStr, CatRemoveCockStr);
		public static readonly CockType LIZARD = new CockType(CockGroup.REPTILIAN, false, LizardCockHeadDesc, LizardDesc, LizardSingleDesc, LizardLongDesc, LizardPlayerStr, LizardTransformStr, LizardGrewCockStr, LizardRestoreStr, LizardRemoveCockStr);
		public static readonly CockType ANEMONE = new CockType(CockGroup.AQUATIC, false, AnemoneCockHeadDesc, AnemoneDesc, AnemoneSingleDesc, AnemoneLongDesc, AnemonePlayerStr, AnemoneTransformStr, AnemoneGrewCockStr, AnemoneRestoreStr, AnemoneRemoveCockStr);
		public static readonly CockType KANGAROO = new CockType(CockGroup.MAMMALIAN, true, KangarooCockHeadDesc, KangarooDesc, KangarooSingleDesc, KangarooLongDesc, KangarooPlayerStr, KangarooTransformStr, KangarooGrewCockStr, KangarooRestoreStr, KangarooRemoveCockStr);
		public static readonly CockType DRAGON = new CockType(CockGroup.REPTILIAN, false, 1.3f, DragonCockHeadDesc, DragonDesc, DragonSingleDesc, DragonLongDesc, DragonPlayerStr, DragonTransformStr, DragonGrewCockStr, DragonRestoreStr, DragonRemoveCockStr);
		public static readonly CockType DISPLACER = new CockType(CockGroup.OTHER, true, 1.5f, DisplacerCockHeadDesc, DisplacerDesc, DisplacerSingleDesc, DisplacerLongDesc, DisplacerPlayerStr, DisplacerTransformStr, DisplacerGrewCockStr, DisplacerRestoreStr, DisplacerRemoveCockStr);
		//public static readonly CockType FOX = new CockType(CockGroup.MAMMALIAN, true, 1.25f, FoxCockHeadDesc, FoxDesc, FoxSingleDesc, FoxLongDesc, FoxPlayerStr, FoxTransformStr, FoxGrewCockStr, FoxRestoreStr, FoxRemoveCockStr);
		public static readonly CockType BEE = new CockType(CockGroup.FLYING, false, BeeCockHeadDesc, BeeDesc, BeeSingleDesc, BeeLongDesc, BeePlayerStr, BeeTransformStr, BeeGrewCockStr, BeeRestoreStr, BeeRemoveCockStr);
		public static readonly CockType PIG = new CockType(CockGroup.MAMMALIAN, false, PigCockHeadDesc, PigDesc, PigSingleDesc, PigLongDesc, PigPlayerStr, PigTransformStr, PigGrewCockStr, PigRestoreStr, PigRemoveCockStr);
		public static readonly CockType AVIAN = new CockType(CockGroup.FLYING, true, AvianCockHeadDesc, AvianDesc, AvianSingleDesc, AvianLongDesc, AvianPlayerStr, AvianTransformStr, AvianGrewCockStr, AvianRestoreStr, AvianRemoveCockStr);
		public static readonly CockType RHINO = new CockType(CockGroup.MAMMALIAN, false, RhinoCockHeadDesc, RhinoDesc, RhinoSingleDesc, RhinoLongDesc, RhinoPlayerStr, RhinoTransformStr, RhinoGrewCockStr, RhinoRestoreStr, RhinoRemoveCockStr);
		public static readonly CockType ECHIDNA = new CockType(CockGroup.MAMMALIAN, true, EchidnaCockHeadDesc, EchidnaDesc, EchidnaSingleDesc, EchidnaLongDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaGrewCockStr, EchidnaRestoreStr, EchidnaRemoveCockStr);
		public static readonly CockType WOLF = new CockType(CockGroup.MAMMALIAN, true, 1.5f, WolfCockHeadDesc, WolfDesc, WolfSingleDesc, WolfLongDesc, WolfPlayerStr, WolfTransformStr, WolfGrewCockStr, WolfRestoreStr, WolfRemoveCockStr);
		public static readonly CockType RED_PANDA = new CockType(CockGroup.MAMMALIAN, true, RedPandaCockHeadDesc, RedPandaDesc, RedPandaSingleDesc, RedPandaLongDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaGrewCockStr, RedPandaRestoreStr, RedPandaRemoveCockStr);
		public static readonly CockType FERRET = new CockType(CockGroup.MAMMALIAN, true, FerretCockHeadDesc, FerretDesc, FerretSingleDesc, FerretLongDesc, FerretPlayerStr, FerretTransformStr, FerretGrewCockStr, FerretRestoreStr, FerretRemoveCockStr);
		public static readonly CockType GOO = new FlexiCock(CockGroup.AQUATIC, false, GooCockHeadDesc, GooDesc, GooSingleDesc, GooLongDesc, GooPlayerStr, GooTransformStr, GooGrewCockStr, GooRestoreStr, GooRemoveCockStr);

		private class FlexiCock : CockType
		{
			public FlexiCock(CockGroup cockGroup, bool hasSheath, SimpleDescriptor headDesc, CockDescriptor shortDesc, CockSingleDescriptor singleDesc,
				PartDescriptor<CockData> longDesc, PlayerBodyPartDelegate<Cock> playerDesc, ChangeType<CockData> transform, GrowCockDescriptor growCockText, RestoreType<CockData> restore,
				RemoveCockDescriptor removeCockText) : base(cockGroup, hasSheath, headDesc, shortDesc, singleDesc, longDesc, playerDesc, transform, growCockText, restore, removeCockText)
			{ }

			public FlexiCock(CockGroup cockGroup, bool hasSheath, double initialKnotMultiplier, SimpleDescriptor headDesc, CockDescriptor shortDescWithAdjFlag,
				CockSingleDescriptor singleDesc, PartDescriptor<CockData> longDesc, PlayerBodyPartDelegate<Cock> playerDesc, ChangeType<CockData> transform,
				GrowCockDescriptor growCockText, RestoreType<CockData> restore, RemoveCockDescriptor removeCockText)
				: base(cockGroup, hasSheath, initialKnotMultiplier, headDesc, shortDescWithAdjFlag, singleDesc, longDesc, playerDesc, transform, growCockText, restore, removeCockText) { }

			public override bool flexibleOrStretchyCock => true;


		}
	}

	public sealed class CockData : FullBehavioralData<CockData, Cock, CockType>, ICock
	{
		public readonly double knotMultiplier;
		public readonly double knotSize;
		public readonly double length;
		public readonly double girth;
		public readonly int cockIndex;

		internal readonly uint? collectionID;

		public readonly byte currentLust;
		public readonly double currentRelativeLust;

		public readonly double cumAmount;

		public readonly CockSockBase cockSock;

		public readonly ReadOnlyPiercing<CockPiercingLocation> cockPiercings;

		public readonly bool currentlyHasSheath;
		public bool requiresSheath => type.usesASheath;

		public bool hasKnot => knotMultiplier >= 1.1f;

		public double area => length * girth;

		#region Sexual MetaData
		public readonly uint soundCount;
		public readonly uint totalSexCount;
		public readonly uint selfSexCount;

		public readonly uint orgasmCount;
		public readonly uint dryOrgasmCount;

		#endregion

		CockType ICock.type => type;

		public string HeadDescription() => type.HeadDescription();

		public string AdjectiveText(bool multipleAdjectives)
		{
			return CockType.CockAdjectiveText(this, multipleAdjectives);
		}


		public string ShortDescriptionNoAdjective() => type.ShortDescriptionNoAdjective();
		public string ShortDescriptionNoAdjective(bool plural) => type.ShortDescriptionNoAdjective(plural);

		public string ShortDescription(bool noAdjective, bool plural) => type.ShortDescription(noAdjective, plural);


		public string FullDescription(bool alternateFormat) => type.FullDescription(this, alternateFormat);

		public string FullDescription() => type.FullDescription(this);

		public string FullDescriptionPrimary() => type.FullDescriptionPrimary(this);

		public string FullDescriptionAlternate() => type.FullDescriptionAlternate(this);

		public override CockData AsCurrentData()
		{
			return this;
		}

		public CockData(Cock source, int currIndex) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)), GetBehavior(source))
		{
			knotMultiplier = source.knotMultiplier;
			length = source.length;
			girth = source.girth;
			knotSize = source.knotSize;
			cockIndex = currIndex;
			currentLust = source.currentLust;
			currentRelativeLust = source.currentRelativeLust;

			cumAmount = source.cumAmount;

			cockSock = source.cockSock;

			cockPiercings = source.piercings.AsReadOnlyData();

			currentlyHasSheath = source.hasSheath;

			collectionID = source.collectionID;

			soundCount = source.soundCount;
			totalSexCount = source.totalSexCount;
			selfSexCount = source.selfSexCount;

			orgasmCount = source.orgasmCount;
			dryOrgasmCount = source.dryOrgasmCount;
		}

		public CockData(Guid creatureID, CockType cockType, int cockIndex, double length, double girth, double knotMultiplier, double knotSize, double cumAmount,
			bool currentlyHasSheath, byte currentLust = Creature.DEFAULT_LUST, double currentRelativeLust = Creature.DEFAULT_LUST, CockSockBase cockSock = null,
			ReadOnlyPiercing<CockPiercingLocation> cockPiercings = null) : base(creatureID, cockType)
		{
			this.knotMultiplier = knotMultiplier;
			this.knotSize = knotSize;
			this.length = length;
			this.girth = girth;
			this.cockIndex = cockIndex;
			this.currentLust = currentLust;
			this.currentRelativeLust = currentRelativeLust;
			this.cumAmount = cumAmount;
			this.cockSock = cockSock;
			this.cockPiercings = cockPiercings ?? new ReadOnlyPiercing<CockPiercingLocation>();
			this.currentlyHasSheath = currentlyHasSheath;

			collectionID = null;
		}

		public CockData(Guid creatureID, CockType cockType, int cockIndex, double length, double girth, double knotMultiplier, double knotSize, double cumAmount,
			bool currentlyHasSheath, uint soundCount, uint totalSexCount, uint selfSexCount, uint orgasmCount, uint dryOrgasmCount,
			byte currentLust = Creature.DEFAULT_LUST, double currentRelativeLust = Creature.DEFAULT_LUST, CockSockBase cockSock = null,
			ReadOnlyPiercing<CockPiercingLocation> cockPiercings = null) : base(creatureID, cockType)
		{
			this.knotMultiplier = knotMultiplier;
			this.knotSize = knotSize;
			this.length = length;
			this.girth = girth;
			this.cockIndex = cockIndex;
			this.currentLust = currentLust;
			this.currentRelativeLust = currentRelativeLust;
			this.cumAmount = cumAmount;
			this.cockSock = cockSock;
			this.cockPiercings = cockPiercings ?? new ReadOnlyPiercing<CockPiercingLocation>();
			this.currentlyHasSheath = currentlyHasSheath;

			collectionID = null;

			this.soundCount = soundCount;
			this.totalSexCount = totalSexCount;
			this.selfSexCount = selfSexCount;

			this.orgasmCount = orgasmCount;
			this.dryOrgasmCount = dryOrgasmCount;
		}
	}
}
