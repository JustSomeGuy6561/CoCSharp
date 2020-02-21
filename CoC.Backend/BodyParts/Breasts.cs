//Breasts.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:27 AM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	public enum CupSize : byte
	{
		FLAT, A, B, C, D, DD, DD_BIG, E, E_BIG, EE,
		EE_BIG, F, F_BIG, FF, FF_BIG, G, G_BIG, GG, GG_BIG, H,
		H_BIG, HH, HH_BIG, HHH, I, I_BIG, II, II_BIG, J, J_BIG,
		JJ, JJ_BIG, K, K_BIG, KK, KK_BIG, L, L_BIG, LL, LL_BIG,
		M, M_BIG, MM, MM_BIG, MMM, MMM_LARGE, N, N_LARGE, NN, NN_LARGE,
		O, O_LARGE, OO, OO_LARGE, P, P_LARGE, PP, PP_LARGE, Q, Q_LARGE,
		QQ, QQ_LARGE, R, R_LARGE, RR, RR_LARGE, S, S_LARGE, SS, SS_LARGE,
		T, T_LARGE, TT, TT_LARGE, U, U_LARGE, UU, UU_LARGE, V, V_LARGE,
		VV, VV_LARGE, W, W_LARGE, WW, WW_LARGE, X, X_LARGE, XX, XX_LARGE,
		Y, Y_LARGE, YY, YY_LARGE, Z, Z_LARGE, ZZ, ZZ_LARGE, ZZZ, ZZZ_LARGE,
		HYPER_A, HYPER_B, HYPER_C, HYPER_D, HYPER_DD, HYPER_DD_BIG, HYPER_E, HYPER_E_BIG, HYPER_EE, HYPER_EE_BIG, //it was supposed to be 109 here, it's was only 108. idk man. i've moved the one up so it really is 109.
		HYPER_F, HYPER_F_BIG, HYPER_FF, HYPER_FF_BIG, HYPER_G, HYPER_G_BIG, HYPER_GG, HYPER_GG_BIG, HYPER_H, HYPER_H_BIG,
		HYPER_HH, HYPER_HH_BIG, HYPER_HHH, HYPER_I, HYPER_I_BIG, HYPER_II, HYPER_II_BIG, HYPER_J, HYPER_J_BIG, HYPER_JJ,
		HYPER_JJ_BIG, HYPER_K, HYPER_K_BIG, HYPER_KK, HYPER_KK_BIG, HYPER_L, HYPER_L_BIG, HYPER_LL, HYPER_LL_BIG, HYPER_M,
		HYPER_M_BIG, HYPER_MM, HYPER_MM_BIG, HYPER_MMM, HYPER_MMM_LARGE, HYPER_N, HYPER_N_LARGE, HYPER_NN, HYPER_NN_LARGE, HYPER_O,
		HYPER_O_LARGE, HYPER_OO, HYPER_OO_LARGE, HYPER_P, HYPER_P_LARGE, HYPER_PP, HYPER_PP_LARGE, HYPER_Q, HYPER_Q_LARGE, HYPER_QQ,
		HYPER_QQ_LARGE, HYPER_R, HYPER_R_LARGE, HYPER_RR, HYPER_RR_LARGE, HYPER_S, HYPER_S_LARGE, HYPER_SS, HYPER_SS_LARGE, HYPER_T,
		HYPER_T_LARGE, HYPER_TT, HYPER_TT_LARGE, HYPER_U, HYPER_U_LARGE, HYPER_UU, HYPER_UU_LARGE, HYPER_V, HYPER_V_LARGE, HYPER_VV,
		HYPER_VV_LARGE, HYPER_W, HYPER_W_LARGE, HYPER_WW, HYPER_WW_LARGE, HYPER_X, HYPER_X_LARGE, HYPER_XX, HYPER_XX_LARGE, HYPER_Y,
		HYPER_Y_LARGE, HYPER_YY, HYPER_YY_LARGE, HYPER_Z, HYPER_Z_LARGE, HYPER_ZZ, HYPER_ZZ_LARGE, HYPER_ZZZ, HYPER_ZZZ_LARGE, JACQUES00
	}

	public sealed partial class NipplePiercingLocation : PiercingLocation, IEquatable<NipplePiercingLocation>
	{
		private static readonly List<NipplePiercingLocation> _allLocations = new List<NipplePiercingLocation>();

		public static readonly ReadOnlyCollection<NipplePiercingLocation> allLocations;

		private readonly byte index;

		static NipplePiercingLocation()
		{
			allLocations = new ReadOnlyCollection<NipplePiercingLocation>(_allLocations);
		}

		public NipplePiercingLocation(byte index, CompatibleWith allowsJewelryOfType, SimpleDescriptor btnText, SimpleDescriptor locationDesc)
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
			if (obj is NipplePiercingLocation nipplePiercing)
			{
				return Equals(nipplePiercing);
			}
			else return false;
		}

		public bool Equals(NipplePiercingLocation other)
		{
			return !(other is null) && other.index == index;
		}

		public override int GetHashCode()
		{
			return index.GetHashCode();
		}

		public static readonly NipplePiercingLocation LEFT_HORIZONTAL = new NipplePiercingLocation(0, SupportedHorizontalJewelry, LeftHorizontalButton, LeftHorizontalLocation);
		public static readonly NipplePiercingLocation LEFT_VERTICAL = new NipplePiercingLocation(1, SupportedVerticalJewelry, LeftVerticalButton, LeftVerticalLocation);

		public static readonly NipplePiercingLocation RIGHT_HORIZONTAL = new NipplePiercingLocation(2, SupportedHorizontalJewelry, RightHorizontalButton, RightHorizontalLocation);
		public static readonly NipplePiercingLocation RIGHT_VERTICAL = new NipplePiercingLocation(3, SupportedVerticalJewelry, RightVerticalButton, RightVerticalLocation);


		private static bool SupportedHorizontalJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.RING || jewelryType == JewelryType.BARBELL_STUD || jewelryType == JewelryType.SPECIAL || jewelryType == JewelryType.DANGLER || jewelryType == JewelryType.HORSESHOE;
		}

		private static bool SupportedVerticalJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.BARBELL_STUD || jewelryType == JewelryType.SPECIAL;
		}
	}

	public sealed class NipplePiercing : Piercing<NipplePiercingLocation>
	{
		public NipplePiercing(IBodyPart source, PiercingUnlocked LocationUnlocked, GenericCreatureText playerShortDesc, GenericCreatureText playerLongDesc) : base(source, LocationUnlocked, playerShortDesc, playerLongDesc)
		{
		}

		public override int MaxPiercings => NipplePiercingLocation.allLocations.Count;

		public override IEnumerable<NipplePiercingLocation> availableLocations => NipplePiercingLocation.allLocations;
	}

	/* Note: Nipples were originally a child of this class, but it turns out that all nipple data is identical per breast row. It seems like an oversight, but it makes a lot
	 * the logic a great deal simpler. Thus, we pull all of that data from an aggregate class.
	 *
	 * It's technically possible for multiple cocks or vaginas to have unique piercings, so we're mimicking that here - instead of nipple piercings being handled by the aggregate,
	 * they're handled here. Now, you could argue this is unnecessary because the current game behavior is to only read and respect the first row for cock/vagina/breast piercings,
	 * but multi-row is still supported here in the event that this ever changes.
	 *
	 * tattoos, however, are not - all tattoos for genitals are handled together - it could be done separately, with unique tattoo classes for breast/cock/vagina, but that seemed
	 * excessive (well, moreso than it already is, at least. IMO the whole tattoo system is overly complex, but it's a carbon copy of piercings, so it's not like it required any
	 * additional work to implement. whatever).
	 */

	//Note: Breasts aren't generated until after perks have been created. Thus, their post perk init is never called, but initial constructor can use perk data without fail.

	public sealed partial class Breasts : SimpleSaveablePart<Breasts, BreastData>, IGrowable, IShrinkable
	{

		#region Breast Row Constants
		public const byte NUM_BREASTS = 2;

		public const CupSize DEFAULT_MALE_SIZE = CupSize.FLAT;
		public const CupSize DEFAULT_FEMALE_SIZE = CupSize.C;
		public const CupSize MIN_SIZE = CupSize.FLAT;
		#endregion
		public override string BodyPartName() => Name();

		private Creature creature => CreatureStore.GetCreatureClean(creatureID);

		private readonly BreastCollection source;
		internal readonly uint collectionID; //used so we can easily determine how the collection changed. it's a huge hassle otherwise.

		#region Breast Data From Collection
		public double lactationRate => source.lactationRate;
		public LactationStatus lactationStatus => source.lactationStatus;

		public bool isOverFull => source.isOverfull;
		public int rowIndex => source.breastRows.IndexOf(this);

		#endregion
		#region Nipple Data From Collection

		internal NippleAggregate nippleData => source.nippleData;

		public bool hasBlackNipples => nippleData.hasBlackNipples;
		public bool hasQuadNipples => nippleData.hasQuadNipples;
		public NippleStatus nippleStatus => nippleData.nippleStatus;
		public double nippleLength => nippleData.length;

		#endregion

		#region Breast Perk Data

		public CupSize maleMinCup => source.smallestPossibleMaleCupSize;
		public CupSize femaleMinCup => source.smallestPossibleFemaleCupSize;

		private CupSize maleDefaultNewCup => source.maleNewDefaultCup;
		private CupSize femaleDefaultNewCup => source.femaleNewDefaultCup;
		private sbyte maleNewCupDelta => source.maleNewCupDelta;
		private sbyte femaleNewCupDelta => source.femaleNewCupDelta;

		private CupSize femaleDefaultCup => EnumHelper.Max(femaleDefaultNewCup.ByteEnumOffset(femaleNewCupDelta), femaleMinCup);
		private CupSize maleDefaultCup => EnumHelper.Max(maleDefaultNewCup.ByteEnumOffset(maleNewCupDelta), maleMinCup);

		//perk values for tit size growth/shrink. defaults to 1 if not applicable.
		private double bigTiddyMultiplier => source.titsGrowthMultiplier;
		private double tinyTiddyMultiplier => source.titsShrinkMultiplier;

		#endregion

		//Nipple Perk Data irrelevant, because all nipple related math done by collection.

		#region Creature Data
		internal Gender currGender => creature?.genitals.gender ?? Gender.MALE;

		//used to describe the nipples.
		internal double relativeLust => creature?.relativeLust ?? Creature.DEFAULT_LUST;
		internal BodyType bodyType => creature?.body.type ?? BodyType.defaultValue;
		#endregion

		//called whenever the perks change their min cup size, for either gender. checks to see if the cupsize is > the new minimum (which factors in the current gender, if applicable)
		//and goes from there. note that while gender defaults to male, this only occurs if this isn't attached to a Creature, which would mean it also doesn't have any perks and
		//therefore all of this is moot because none of this will ever be called anyway.
		internal void ValidateCupSize()
		{
			if (cupSize < minimumCupSize)
			{
				var oldValue = AsReadOnlyData();
				cupSize = minimumCupSize;
				NotifyDataChanged(oldValue);
			}
		}

		private CupSize resetSize => currGender.HasFlag(Gender.FEMALE) ? femaleDefaultCup : maleDefaultCup;
		private CupSize minimumCupSize => currGender.HasFlag(Gender.FEMALE) ? femaleMinCup : maleMinCup;

		private bool makeBigTits => bigTiddyMultiplier > 1.1f;
		private bool makeSmallTits => tinyTiddyMultiplier > 1.1f;

		public double lactationAmount(bool perBreast)
		{
			if (!perBreast)
			{
				return source.currentLactationAmount;
			}
			else
			{
				return source.currentLactationAmount / source.totalBreasts;
			}
		}

		public byte numBreasts => NUM_BREASTS;

		public override BreastData AsReadOnlyData()
		{
			return new BreastData(this, rowIndex);
		}

		public CupSize cupSize
		{
			get => _cupSize;
			private set
			{
				Utils.ClampEnum(ref value, minimumCupSize, CupSize.JACQUES00); //enums: icomparable, but not really. woooo!
				if (_cupSize != value)
				{
					var oldData = AsReadOnlyData();
					_cupSize = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private CupSize _cupSize;

		public readonly NipplePiercing nipplePiercings;

		public uint orgasmCount { get; private set; } = 0;
		public uint dryOrgasmCount { get; private set; } = 0;

		public uint totalTitFuckCount { get; private set; } = 0;
		public uint selfTitFuckCount { get; private set; } = 0;
		public uint dickNippleSexCount { get; private set; } = 0;
		public uint totalFuckableNippleSexCount { get; private set; } = 0;
		public uint selfFuckableNippleSexCount { get; private set; } = 0;

		public bool isMale => cupSize == maleMinCup && nippleLength <= .5f;

		internal Breasts(BreastCollection source, uint id, Gender initialGender) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			this.source = source;
			collectionID = id;

			if (initialGender.HasFlag(Gender.FEMALE))
			{
				_cupSize = FemaleNewLength();
			}
			else
			{
				_cupSize = MaleNewLength();
			}


			CupSize min = currGender.HasFlag(Gender.FEMALE) ? femaleMinCup : maleMinCup;
			if (cupSize < min)
			{
				cupSize = min;
			}

			this.nipplePiercings = new NipplePiercing(this, PiercingLocationUnlocked, AllNipplePiercingsShort, AllNipplePiercingsLong);
		}
		internal Breasts(BreastCollection source, uint id, CupSize cup) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			this.source = source;
			collectionID = id;
			_cupSize = Utils.ClampEnum2(cup, CupSize.FLAT, CupSize.JACQUES00);

			this.nipplePiercings = new NipplePiercing(this, PiercingLocationUnlocked, AllNipplePiercingsShort, AllNipplePiercingsLong);
		}

		internal Breasts(BreastCollection source, uint id, CupSize cup, IEnumerable<KeyValuePair<NipplePiercingLocation, PiercingJewelry>> nipplePiercings)
			: base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			this.source = source;
			collectionID = id;

			_cupSize = Utils.ClampEnum2(cup, CupSize.FLAT, CupSize.JACQUES00);

			this.nipplePiercings = new NipplePiercing(this, PiercingLocationUnlocked, AllNipplePiercingsShort, AllNipplePiercingsLong);
			this.nipplePiercings.InitializePiercings(nipplePiercings);

		}

		private CupSize MaleNewLength(CupSize? givenCup = null)
		{
			CupSize minCup = Utils.ClampEnum2(maleDefaultCup, CupSize.FLAT, CupSize.JACQUES00);
			if (!(givenCup is null))
			{
				((CupSize)givenCup).ByteEnumOffset(maleNewCupDelta);
			}


			if (givenCup is null || givenCup < minCup)
			{
				return minCup;
			}
			return (CupSize)givenCup;
		}

		private CupSize FemaleNewLength(CupSize? givenCup = null)
		{
			var minCup = Utils.ClampEnum2(femaleDefaultCup, CupSize.FLAT, CupSize.JACQUES00);
			if (!(givenCup is null))
			{
				((CupSize)givenCup).ByteEnumOffset(femaleNewCupDelta);
			}

			if (givenCup is null || givenCup < minCup)
			{
				return minCup;
			}
			return (CupSize)givenCup;
		}

		private void OnGenderChanged(object sender, EventHelpers.GenderChangedEventArgs e)
		{
			CupSize min = e.newGender.HasFlag(Gender.FEMALE) ? femaleMinCup : maleMinCup;
			if (cupSize < min)
			{
				cupSize = min;
			}
		}

		protected internal override void LateInit()
		{
			if (creature != null) creature.genitals.onGenderChanged += OnGenderChanged;
		}

		public byte GrowBreasts(byte byAmount = 1, bool ignorePerks = false)
		{
			if (cupSize >= CupSize.JACQUES00)
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			if (!ignorePerks)
			{
				ushort val = (ushort)Math.Round(byAmount * bigTiddyMultiplier);
				byAmount = val > byte.MaxValue ? byte.MaxValue : (byte)val;
			}

			var oldData = AsReadOnlyData();
			cupSize = cupSize.ByteEnumAdd(byAmount);
			if (cupSize != oldSize)
			{
				NotifyDataChanged(oldData);
			}
			return cupSize - oldSize;
		}

		public byte ShrinkBreasts(byte byAmount = 1, bool ignorePerks = false)
		{
			if (cupSize <= CupSize.FLAT)
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			if (!ignorePerks)
			{
				ushort val = (ushort)Math.Round(byAmount * tinyTiddyMultiplier);
				byAmount = val > byte.MaxValue ? byte.MaxValue : (byte)val;
			}
			var oldData = AsReadOnlyData();

			cupSize = cupSize.ByteEnumSubtract(byAmount);
			if (cupSize != oldSize)
			{
				NotifyDataChanged(oldData);
			}
			return cupSize - oldSize;
		}

		public short SetCupSize(CupSize size)
		{
			var oldSize = (byte)cupSize;
			Utils.ClampEnum(ref size, CupSize.FLAT, CupSize.JACQUES00);
			if (cupSize != size)
			{
				var oldData = AsReadOnlyData();
				cupSize = size;
				NotifyDataChanged(oldData);
			}

			return oldSize.delta((byte)cupSize);
		}

		public bool MakeMale(bool removeStatus = true)
		{
			if (isMale)
			{
				return false;
			}
			var oldData = AsReadOnlyData();

			cupSize = maleMinCup;
			NotifyDataChanged(oldData);
			return true;
		}

		public bool MakeFemale(bool removeStatus = true)
		{
			if (!isMale)
			{
				return false;
			}
			var oldData = AsReadOnlyData();
			if (cupSize < femaleMinCup)
			{
				cupSize = femaleMinCup;
			}
			else if (cupSize == CupSize.FLAT)
			{
				cupSize = CupSize.B;
			}

			NotifyDataChanged(oldData);
			return true;
		}

		public void Reset(bool resetPiercings = false)
		{
			cupSize = currGender.HasFlag(Gender.FEMALE) ? femaleMinCup : maleMinCup;

			if (resetPiercings)
			{
				nipplePiercings.Reset();
			}

			//nippleFuckCount = 0;
			totalTitFuckCount = 0;
			//dickNippleFuckCount = 0;
		}

		public static BreastData GenerateAggregate(Guid creatureID, CupSize averageCup, double averageNippleLength, bool blackNipples, bool quadNipples,
			bool largeNipplesAreDickNipples, NippleStatus nippleType, double lactationRate, LactationStatus lactationStatus, bool overfull, Gender gender,
			BodyType bodyType, double relativeLust, CupSize maleMinCupSize)
		{
			return new BreastData(creatureID, -1, NUM_BREASTS, averageCup, averageNippleLength, blackNipples, quadNipples, largeNipplesAreDickNipples, nippleType,
				lactationRate, lactationStatus, overfull, gender, bodyType, relativeLust:relativeLust, maleMinCupSize:maleMinCupSize);
		}

		private bool PiercingLocationUnlocked(NipplePiercingLocation piercingLocation, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		#region Breast Text
		//by default, short description simply returns the noun, in plural format.
		//it's possible via overloads to have this return singular or plural, with/without an article, or any combination thereof.
		//Note that this description will only assume you're talking about this particular set of breasts, not all of them (if applicable)
		//use the strings in genitals for descriptions that require all breast rows.
		//also note: it is possible to describe all breasts by simply using the short description in conjunction with whatever flavor text you want
		//(i.e. "several pairs of " {ShortDescription()}) in fact, this is how the all rows is done, but with an aggregate of all breast rows.

		public string ShortDescription() => BreastHelpers.ShortDesc(this, true);
		public string ShortDescription(bool plural) => BreastHelpers.ShortDesc(this, plural);

		public string SingleItemDescription() => BreastHelpers.SingleItemDesc(this);

		public string LongDescription(bool alternateFormat = false, bool plural = true, bool preciseMeasurements = false)
		{
			return BreastHelpers.Desc(this, alternateFormat, plural, preciseMeasurements, false);
		}
		public string FullDescription(bool alternateFormat = false, bool plural = true, bool preciseMeasurements = false)
		{
			return BreastHelpers.Desc(this, alternateFormat, preciseMeasurements, plural, true);
		}

		#endregion

		#region NippleText
		public string NippleNoun() => nippleData.NippleNoun();
		public string NippleNoun(bool plural, bool allowQuadNippleIfApplicable = false) => nippleData.NippleNoun(plural, allowQuadNippleIfApplicable);

		public string ShortNippleDescription() => nippleData.ShortNippleDescription(this);

		public string ShortNippleDescription(bool plural, bool allowQuadNippleTextIfApplicable = true) => nippleData.ShortNippleDescription(this, plural, allowQuadNippleTextIfApplicable);

		public string SingleNippleDescription() => nippleData.SingleNippleDescription(this);
		public string SingleNipplpeDescription(bool allowQuadNippleIfApplicable) => nippleData.SingleNipplpeDescription(this, allowQuadNippleIfApplicable);

		public string LongNippleDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false) => nippleData.LongNippleDescription(this, alternateFormat, plural, usePreciseMeasurements);

		public string FullNippleDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false) => nippleData.FullNippleDescription(this, alternateFormat, plural, usePreciseMeasurements);


		public string OneNippleOrOneOfQuadNipplesShort(string pronoun = "your") => nippleData.OneNippleOrOneOfQuadNipplesShort(this, pronoun);


		public string OneNippleOrEachOfQuadNipplesShort(string pronoun = "your") => nippleData.OneNippleOrEachOfQuadNipplesShort(this, pronoun);


		public string OneNippleOrEachOfQuadNipplesShort(string pronoun, out bool isPlural) => nippleData.OneNippleOrEachOfQuadNipplesShort(this, pronoun, out isPlural);


		#endregion

		public override bool IsIdenticalTo(BreastData original, bool ignoreSexualMetaData)
		{
			return cupSize == original.cupSize && nippleData.IsIdenticalTo(original.nippleData, ignoreSexualMetaData) && numBreasts == original.numBreasts
				&& (ignoreSexualMetaData || (orgasmCount == original.orgasmCount && dryOrgasmCount == original.dryOrgasmCount
				&& totalTitFuckCount == original.totalTitFuckCount && selfTitFuckCount == original.selfTitFuckCount && dickNippleSexCount == original.dickNippleSexCount
				&& totalFuckableNippleSexCount == original.totalFuckableNippleSexCount && selfFuckableNippleSexCount == original.selfFuckableNippleSexCount));
		}

		internal override bool Validate(bool correctInvalidData)
		{
			cupSize = cupSize;
			return nipplePiercings.Validate(correctInvalidData);
		}

		//to be frank, idk what would actually orgasm when being titty fucked, but, uhhhh... i guess it can be stored in stats or some shit?
		internal void DoTittyFuck(double length, double girth, double knotWidth, bool reachOrgasm, bool sourceIsSelf)
		{
			totalTitFuckCount++;

			if (sourceIsSelf)
			{
				selfTitFuckCount++;
			}

			if (reachOrgasm)
			{
				orgasmCount++;
			}
		}

		internal void DoNippleFuck(double length, double girth, double knotWidth, double cumAmount, bool reachOrgasm, bool sourceIsSelf)
		{
			totalFuckableNippleSexCount++;

			if (sourceIsSelf)
			{
				selfFuckableNippleSexCount++;
			}

			if (reachOrgasm)
			{
				orgasmCount++;
			}
		}

		internal void DoDickNippleSex(bool reachOrgasm)
		{
			dickNippleSexCount++;
			if (reachOrgasm)
			{
				orgasmCount++;
			}
		}

		internal void OrgasmTits(bool dryOrgasm)
		{
			orgasmCount++;
			if (dryOrgasm) dryOrgasmCount++;
		}

		#region IGrowShrinkable
		bool IGrowable.CanGroPlus()
		{
			return cupSize < CupSize.JACQUES00;
		}

		bool IShrinkable.CanReducto()
		{
			return cupSize > minimumCupSize;
		}

		double IGrowable.UseGroPlus()
		{
			if (!((IGrowable)this).CanGroPlus())
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			this.cupSize += (byte)(Utils.Rand(2) + 1);
			//c# is a bitch in that all numbers are treated as ints or doubles unless explicitly cast - byte me
			this.cupSize += (byte)(makeBigTits && Utils.RandBool() ? 1 : 0); //add one for big tits perk 50% of the time
			return cupSize - oldSize;
		}

		double IShrinkable.UseReducto()
		{
			if (!((IShrinkable)this).CanReducto())
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			if (cupSize.ByteEnumSubtract(1) == minimumCupSize || !makeSmallTits || Utils.RandBool())
			{
				cupSize--;
			}
			else
			{
				cupSize -= 2;
			}
			return oldSize - cupSize;
		}
		#endregion
		public bool TittyFuckable()
		{
			return cupSize > CupSize.A && numBreasts > 1;
		}
	}

	public sealed partial class BreastData : SimpleData
	{
		public readonly CupSize cupSize;
		public readonly int currBreastRowIndex;
		public readonly byte numBreasts;


		internal readonly uint? collectionID;

		internal readonly NippleAggregateData nippleData;

		public double nippleLength => nippleData.length;
		public bool hasQuadNipples => nippleData.hasQuadNipples;
		public bool hasBlackNipples => nippleData.hasBlackNipples;
		public NippleStatus nippleType => nippleData.nippleStatus;

		public readonly ReadOnlyPiercing<NipplePiercingLocation> nipplePiercings;

		public readonly double lactationRate;
		public readonly LactationStatus lactationStatus;
		public readonly bool isOverFull;

		public readonly CupSize maleMinSize;

		#region Sexual MetaData

		public readonly uint orgasmCount;
		public readonly uint dryOrgasmCount;

		public readonly uint totalTitFuckCount;
		public readonly uint selfTitFuckCount;
		public readonly uint dickNippleSexCount;
		public readonly uint totalFuckableNippleSexCount;
		public readonly uint selfFuckableNippleSexCount;

		#endregion
		internal readonly Gender gender;
		internal readonly BodyType bodyType;
		internal readonly double relativeLust;

		public bool isMaleBreasts => cupSize <= maleMinSize && nippleLength <= 0.5f;

		#region Breast Text
		//by default, short description simply returns the noun, in plural format.
		//it's possible via overloads to have this return singular or plural, with/without an article, or any combination thereof.
		//Note that this description will only assume you're talking about this particular set of breasts, not all of them (if applicable)
		//use the strings in genitals for descriptions that require all breast rows.
		//also note: it is possible to describe all breasts by simply using the short description in conjunction with whatever flavor text you want
		//(i.e. "several pairs of " {ShortDescription()}) in fact, this is how the all rows is done, but with an aggregate of all breast rows.

		public string ShortDescription() => BreastHelpers.ShortDesc(this, true);
		public string ShortDescription(bool plural) => BreastHelpers.ShortDesc(this, plural);

		public string SingleItemDescription() => BreastHelpers.SingleItemDesc(this);

		public string LongDescription(bool alternateFormat = false, bool plural = true, bool preciseMeasurements = false)
		{
			return BreastHelpers.Desc(this, alternateFormat, plural, preciseMeasurements, false);
		}
		public string FullDescription(bool alternateFormat = false, bool plural = true, bool preciseMeasurements = false)
		{
			return BreastHelpers.Desc(this, alternateFormat, preciseMeasurements, plural, true);
		}

		#endregion

		#region NippleText
		public string NippleNoun() => nippleData.NippleNoun();
		public string NippleNoun(bool plural, bool allowQuadNippleIfApplicable = false) => nippleData.NippleNoun(plural, allowQuadNippleIfApplicable);

		public string ShortNippleDescription() => nippleData.ShortNippleDescription(this);

		public string ShortNippleDescription(bool plural, bool allowQuadNippleTextIfApplicable = true) => nippleData.ShortNippleDescription(this, plural, allowQuadNippleTextIfApplicable);

		public string SingleNippleDescription() => nippleData.SingleNippleDescription(this);
		public string SingleNipplpeDescription(bool allowQuadNippleIfApplicable) => nippleData.SingleNipplpeDescription(this, allowQuadNippleIfApplicable);

		public string LongNippleDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false) => nippleData.LongNippleDescription(this, alternateFormat, plural, usePreciseMeasurements);

		public string FullNippleDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false) => nippleData.FullNippleDescription(this, alternateFormat, plural, usePreciseMeasurements);


		public string OneNippleOrOneOfQuadNipplesShort(string pronoun = "your") => nippleData.OneNippleOrOneOfQuadNipplesShort(this, pronoun);


		public string OneNippleOrEachOfQuadNipplesShort(string pronoun = "your") => nippleData.OneNippleOrEachOfQuadNipplesShort(this, pronoun);


		public string OneNippleOrEachOfQuadNipplesShort(string pronoun, out bool isPlural) => nippleData.OneNippleOrEachOfQuadNipplesShort(this, pronoun, out isPlural);


		#endregion

		internal BreastData(Breasts breasts, int currentBreastRow) : base(breasts?.creatureID ?? throw new ArgumentNullException(nameof(breasts)))
		{
			cupSize = breasts.cupSize;
			currBreastRowIndex = currentBreastRow;
			numBreasts = breasts.numBreasts;

			nipplePiercings = breasts.nipplePiercings.AsReadOnlyData();

			lactationStatus = breasts.lactationStatus;
			lactationRate = breasts.lactationRate;
			isOverFull = breasts.isOverFull;

			gender = breasts.currGender;
			relativeLust = breasts.relativeLust;
			bodyType = breasts.bodyType;

			maleMinSize = breasts.maleMinCup;

			nippleData = breasts.nippleData.AsReadOnlyData();

			collectionID = breasts.collectionID;

			orgasmCount = breasts.orgasmCount;
			dryOrgasmCount = breasts.dryOrgasmCount;

			totalTitFuckCount = breasts.totalTitFuckCount;
			selfTitFuckCount = breasts.selfTitFuckCount;
			dickNippleSexCount = breasts.dickNippleSexCount;
			totalFuckableNippleSexCount = breasts.totalFuckableNippleSexCount;
			selfFuckableNippleSexCount = breasts.selfFuckableNippleSexCount;

		}

		public BreastData(Guid creatureID, int rowIndex, byte totalNumberOfBreasts, CupSize cupSize, double nippleLength, bool blackNipples, bool quadNipples,
			bool largeNipplesBecomeDickNipples, NippleStatus nippleType, double lactationRate, LactationStatus lactationStatus, bool overfull, Gender gender,
			uint orgasmCount = 0, uint dryOrgasmCount = 0, uint totalTitFuckCount = 0, uint selfTitFuckCount = 0, uint dickNippleSexCount = 0,
			uint totalFuckableNippleSexCount = 0, uint selfFuckableNippleSexCount = 0, ReadOnlyPiercing<NipplePiercingLocation> piercings = null,
			double relativeLust = Creature.DEFAULT_LUST, CupSize maleMinCupSize = CupSize.FLAT) : this(creatureID, rowIndex, totalNumberOfBreasts,
				cupSize, nippleLength, blackNipples, quadNipples, largeNipplesBecomeDickNipples, nippleType, lactationRate, lactationStatus, overfull,
				gender, BodyType.defaultValue, orgasmCount, dryOrgasmCount, totalTitFuckCount, selfTitFuckCount, dickNippleSexCount,
				totalFuckableNippleSexCount, selfFuckableNippleSexCount, piercings, relativeLust, maleMinCupSize)
		{ }

		public BreastData(Guid creatureID, int rowIndex, byte totalNumberOfBreasts, CupSize cupSize, double nippleLength, bool blackNipples, bool quadNipples,
			bool largeNipplesBecomeDickNipples, NippleStatus nippleType, double lactationRate, LactationStatus lactationStatus, bool overfull, Gender gender,
			BodyType bodyType, uint orgasmCount = 0, uint dryOrgasmCount = 0, uint totalTitFuckCount = 0, uint selfTitFuckCount = 0, uint dickNippleSexCount = 0,
			uint totalFuckableNippleSexCount = 0, uint selfFuckableNippleSexCount = 0, ReadOnlyPiercing<NipplePiercingLocation> piercings = null,
			double relativeLust = Creature.DEFAULT_LUST, CupSize maleMinCupSize = CupSize.FLAT) : base(creatureID)
		{
			this.currBreastRowIndex = rowIndex;
			this.numBreasts = totalNumberOfBreasts;
			this.cupSize = cupSize;

			nippleData = new NippleAggregateData(creatureID, nippleType, quadNipples, blackNipples, largeNipplesBecomeDickNipples, nippleLength, bodyType, relativeLust, lactationRate, lactationStatus);

			this.nipplePiercings = piercings ?? new ReadOnlyPiercing<NipplePiercingLocation>();

			this.lactationRate = lactationRate;
			this.lactationStatus = lactationStatus;
			this.isOverFull = overfull;

			this.gender = gender;
			this.bodyType = bodyType;
			this.relativeLust = relativeLust;

			this.maleMinSize = maleMinCupSize;

			collectionID = null;
		}
	}
}
