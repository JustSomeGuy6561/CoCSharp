//Nipples.cs
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

	/*
	 * Normal: standard nipple. Inverted: Two stages - Fully or Slightly. Fully inverted requires a small nipple, and the entirety of it is inside.
	 * Slightly inverted can be a little larger, but sticks out slightly. Fuckable is a wide nipple with the milk-hole (for lack of better term) inside large enough to fit dicks.
	 * dick nipple is a thin, long nipple, which appears normal, albeit oddly curved upward. when aroused, they grow and act like dicks. they can't impregnate, but do count as cum.
	 *
	 * as of now, there's only one way to revert any non-normal nipples - shrink them down to inverted, and have nipple piercings. after a period of 3.5 days, they'll be "pulled out"
	 * and be back to normal. it's still in the spirit of vanilla, which had no natural way of doing this, but it also didn't have inverted nipples. it's also "realistic" in that
	 * people actually do this, though i suppose magic eggs would also work if we wanted. ofc non-natural means like bro brew, omnibus gift, and ceraph breast steals still work.
	 *
	 * Note that all nipple (and its parent, breast) related functionality should be taken care of by the genitals class. for the sake of consistency and simplicity, all nipples will use the same
	 * status. The first breast row is the primary one - most of the old game architecture is designed around one breast row, and the rest are based on it. I suppose that it'd be possible to rework
	 * everything to factor in, but it's far simpler to just leave it as is. AS SUCH: we only care about the first breast row's nipples. that means the nipple status of the first row is mimicked across
	 * the remaining rows, and nipple size is set accordingly. Similarly, while technically possible to pierce each breast row's nipples, the only one that is used in logic is the first.
	 * Of course, this restriction is limited to these classes - you can always just hard-code in text that says a random NPC or monster has all their nipples pierced or whatever, or a myriad or
	 * nipple statuses - but doing it here is verboten. (German - forbidden, not possible).
	 *
	 * note that the above is just context - you dont really need to know this to understand what this class does. TL;DR: this class assumes the data sent to it is valid.
	 */

	//Also note: this class is created after perks have been initialized. it's post perk init is never called.

	public enum NippleStatus { NORMAL, FULLY_INVERTED, SLIGHTLY_INVERTED, FUCKABLE, DICK_NIPPLE }
	//public enum NipplePiercings { LEFT_HORIZONTAL, LEFT_VERTICAL, RIGHT_HORIZONTAL, RIGHT_VERTICAL }

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
			return jewelryType == JewelryType.RING || jewelryType ==JewelryType.BARBELL_STUD || jewelryType ==JewelryType.SPECIAL || jewelryType ==JewelryType.DANGLER || jewelryType ==JewelryType.HORSESHOE;
		}

		private static bool SupportedVerticalJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.BARBELL_STUD || jewelryType ==JewelryType.SPECIAL;
		}
	}

	public sealed class NipplePiercing : Piercing<NipplePiercingLocation>
	{
		public NipplePiercing(PiercingUnlocked LocationUnlocked, PlayerStr playerDesc) : base(LocationUnlocked, playerDesc)
		{
		}

		public override int MaxPiercings => NipplePiercingLocation.allLocations.Count;

		public override IEnumerable<NipplePiercingLocation> availableLocations => NipplePiercingLocation.allLocations;
	}

	public sealed partial class Nipples : SimpleSaveablePart<Nipples, NippleData>, IGrowable, IShrinkable
	{
		public override string BodyPartName()
		{
			return Name();
		}

		public const float MIN_NIPPLE_LENGTH = 0.25f;
		public const float MAX_NIPPLE_LENGTH = 50f;

		public const float FULLY_INVERTED_THRESHOLD = 1f; //above this, not fully inverted
		public const float FUCKABLE_NIPPLE_THRESHOLD = 3f; //above this, fuckable is possible.
		public const float DICK_NIPPLE_THRESHOLD = 5f; //above this, dick nipples possible.

		//public const float LACTATION_THRESHOLD = 1f;
		//internal const ushort INVERTED_COUNTDOWN_TIMER = 24 * 7 / 2; //3.5 Days.

		public const float MALE_DEFAULT_LENGTH = MIN_NIPPLE_LENGTH;
		public const float FEMALE_DEFAULT_LENGTH = 0.5f;

		private Creature creature
		{
			get
			{
				CreatureStore.TryGetCreature(creatureID, out Creature creatureSource);
				return creatureSource;
			}
		}


		private Gender currGender => creature?.genitals.gender ?? Gender.MALE;

		private int BreastRowIndex => creature?.genitals.breastRows.IndexOf(parent) ?? 0;
		private readonly Breasts parent;
		//i guess we'll call tassels danglers - idk.

		internal float growthMultiplier = 1;
		internal float shrinkMultiplier = 1;
		internal float minNippleLength = MIN_NIPPLE_LENGTH;
		internal float defaultNippleLength;

		public NippleStatus nippleStatus => creature?.genitals.nippleType ?? NippleStatus.NORMAL;
		public bool quadNipples => creature?.genitals.quadNipples ?? false;
		public bool blackNipples => creature?.genitals.blackNipples ?? false;
		internal BodyType bodyType => creature?.body.type ?? BodyType.defaultValue;

		public uint dickNippleFuckCount { get; private set; } = 0;
		public uint nippleFuckCount { get; private set; } = 0;

		public uint orgasmCount { get; private set; } = 0;
		public uint dryOrgasmCount { get; private set; } = 0;


		public bool unlockedDickNipples => creature?.genitals.unlockedDickNipples ?? false;

		public float length
		{
			get => _length;
			private set
			{
				Utils.Clamp(ref value, MIN_NIPPLE_LENGTH, MAX_NIPPLE_LENGTH);
				if (_length != value)
				{
					var oldData = AsReadOnlyData();
					_length = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private float _length;

		public float width => length < 1 ? length / 2 : length / 4;

		public readonly NipplePiercing nipplePiercing;

		public bool isPierced => nipplePiercing.isPierced;
		public bool wearingJewelry => nipplePiercing.wearingJewelry;

		public LactationStatus lactationStatus => creature?.genitals.lactationStatus ?? LactationStatus.NOT_LACTATING;
		public float lactationRate => creature?.genitals.lactationRate ?? 0;

		public float relativeLust => creature?.relativeLust ?? Creature.DEFAULT_LUST;

		internal Nipples(Guid creatureID, Breasts parent, BreastPerkHelper initialPerkData, Gender gender) : base(creatureID)
		{
			this.parent = parent ?? throw new ArgumentNullException(nameof(parent));

			length = initialPerkData.NewNippleDefaultLength;

			nipplePiercing = new NipplePiercing(PiercingLocationUnlocked, AllNipplePiercingsStr);

			//SetupPiercingMagic();

			growthMultiplier = initialPerkData.NippleGrowthMultiplier;
			shrinkMultiplier = initialPerkData.NippleShrinkMultiplier;
			defaultNippleLength = initialPerkData.NewNippleDefaultLength;
		}

		internal Nipples(Guid creatureID, Breasts parent, BreastPerkHelper initialPerkData, float nippleLength) : base(creatureID)
		{
			this.parent = parent ?? throw new ArgumentNullException(nameof(parent));

			length = nippleLength;

			nipplePiercing = new NipplePiercing(PiercingLocationUnlocked, AllNipplePiercingsStr);

			//SetupPiercingMagic();

			growthMultiplier = initialPerkData.NippleGrowthMultiplier;
			shrinkMultiplier = initialPerkData.NippleShrinkMultiplier;
			defaultNippleLength = initialPerkData.NewNippleDefaultLength;
		}

		public override NippleData AsReadOnlyData()
		{
			return new NippleData(this, BreastRowIndex);
		}

		internal float GrowNipple(float growAmount, bool ignorePerk = false)
		{
			float oldLength = length;
			if (!ignorePerk)
			{
				growAmount *= growthMultiplier;
			}
			length += growAmount;
			return length - oldLength;
		}

		internal float ShrinkNipple(float shrinkAmount, bool ignorePerk = false)
		{
			float oldLength = length;
			if (!ignorePerk)
			{
				shrinkAmount *= shrinkMultiplier;
			}
			length -= shrinkAmount;
			return oldLength - length;
		}
		#region Text
		public string NounText() => NippleStrings.NounText(this);
		public string NounText(bool plural, bool allowQuadNippleIfApplicable = false) => NippleStrings.NounText(this, plural, allowQuadNippleIfApplicable);

		public string ShortDescription() => NippleStrings.ShortDescription(this, true, true);

		public string ShortDescription(bool plural, bool allowQuadNippleTextIfApplicable = true) => NippleStrings.ShortDescription(this, plural, allowQuadNippleTextIfApplicable);

		public string SingleItemDescription() => NippleStrings.SingleItemDescription(this, true);
		public string SingleItemDescription(bool allowQuadNippleIfApplicable) => NippleStrings.SingleItemDescription(this, allowQuadNippleIfApplicable);

		public string LongDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
		{
			return NippleStrings.LongDescription(this, alternateFormat, plural, usePreciseMeasurements);
		}
		public string FullDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
		{
			return NippleStrings.FullDescription(this, alternateFormat, plural, usePreciseMeasurements);
		}

		public string OneNippleOrOneOfQuadNipplesShort(string pronoun = "your")
		{
			return CommonBodyPartStrings.OneOfDescription(quadNipples, pronoun, ShortDescription());
		}

		public string OneNippleOrEachOfQuadNipplesShort(string pronoun = "your")
		{
			return OneNippleOrEachOfQuadNipplesShort(pronoun, out bool _);
		}

		public string OneNippleOrEachOfQuadNipplesShort(string pronoun, out bool isPlural)
		{
			isPlural = quadNipples;

			return CommonBodyPartStrings.EachOfDescription(quadNipples, pronoun, ShortDescription());
		}


		#endregion

		internal override bool Validate(bool correctInvalidData)
		{
			//self-validating.
			length = length;

			return nipplePiercing.Validate(correctInvalidData);
		}

		internal void OrgasmNipplesGeneric(bool dryOrgasm)
		{
			orgasmCount++;
			if (dryOrgasm) dryOrgasmCount++;
		}

		private bool PiercingLocationUnlocked(NipplePiercingLocation piercingLocation, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public bool EquipOrPierceAt(NipplePiercingLocation piercingLocation, PiercingJewelry jewelry, bool forceIfEnabled = false)
		{
			bool retVal = nipplePiercing.EquipOrPierceAndEquip(piercingLocation, jewelry, forceIfEnabled);
			//SetupPiercingMagic();
			return retVal;
		}
		public bool EquipPiercingJewelry(NipplePiercingLocation piercingLocation, PiercingJewelry jewelry, bool forceIfEnabled = false)
		{
			bool retVal = nipplePiercing.EquipPiercingJewelry(piercingLocation, jewelry, forceIfEnabled);
			//SetupPiercingMagic();
			return retVal;
		}
		public bool Pierce(NipplePiercingLocation location)
		{
			bool retVal = nipplePiercing.Pierce(location);
			//SetupPiercingMagic();
			return retVal;
		}

		public PiercingJewelry RemovePiercingJewelry(NipplePiercingLocation location, bool forceRemove = false)
		{
			PiercingJewelry jewelry = nipplePiercing.RemovePiercingJewelry(location, forceRemove);
			//SetupPiercingMagic();
			return jewelry;
		}

		//private void SetupPiercingMagic()
		//{
		//	if (wearingJewelry && (nippleStatus == NippleStatus.SLIGHTLY_INVERTED || nippleStatus == NippleStatus.FULLY_INVERTED))
		//	{
		//		if (invertedNippleCounter == null)
		//		{
		//			invertedNippleCounter = 0;
		//		}
		//	}
		//	else
		//	{
		//		invertedNippleCounter = null;
		//	}
		//}

		internal void Reset(bool resetPiercings = false)
		{
			length = currGender.HasFlag(Gender.FEMALE) ? FEMALE_DEFAULT_LENGTH : MALE_DEFAULT_LENGTH;

			if (resetPiercings)
			{
				nipplePiercing.Reset();
			}
		}

		#region GrowShrink
		bool IGrowable.CanGroPlus()
		{
			return length < MAX_NIPPLE_LENGTH;
		}

		bool IShrinkable.CanReducto()
		{
			return length > MIN_NIPPLE_LENGTH;
		}

		float IGrowable.UseGroPlus()
		{
			if (!((IGrowable)this).CanGroPlus())
			{
				return 0;
			}
			float oldLength = length;
			length += Utils.Rand(6) / 20.0f + 0.25f; //ranges from 1/4- 1/2 inch.
			return length - oldLength; //returns that change in value. limited only if it reaches the max.
		}

		float IShrinkable.UseReducto()
		{
			if (!((IShrinkable)this).CanReducto())
			{
				return 0;
			}
			float oldLength = length;
			if (length > 0.5f)
			{
				length /= 2f;
			}
			else
			{
				length = 0.25f;
			}
			return oldLength - length;
		}
		#endregion

		internal void DoNippleFuck(float length, float girth, float knotWidth, float cumAmount, bool reachOrgasm)
		{
			nippleFuckCount++;
			if (reachOrgasm)
			{
				orgasmCount++;
			}
		}

		internal void DoDickNippleSex(bool reachOrgasm)
		{
			dickNippleFuckCount++;
			if (reachOrgasm)
			{
				orgasmCount++;
			}
		}

		//private ushort? invertedNippleCounter = null;

		//internal bool DoPiercingTimeNonsense(bool isPlayer, byte hoursPassed, bool hasOtherBreastRows, out string output)
		//{


		//	if (nippleStatus == NippleStatus.FULLY_INVERTED && wearingJewelry)
		//	{
		//		nippleStatus = NippleStatus.SLIGHTLY_INVERTED;
		//		output = NipplesLessInvertedDueToPiercingInThem(hasOtherBreastRows);
		//		invertedNippleCounter = 0;
		//		return true;
		//	}
		//	else if (invertedNippleCounter != null)
		//	{
		//		invertedNippleCounter = ((ushort)invertedNippleCounter).add(hoursPassed);
		//		if (invertedNippleCounter > INVERTED_COUNTDOWN_TIMER)
		//		{
		//			nippleStatus = NippleStatus.NORMAL;
		//			output = NipplesNoLongerInvertedDueToPiercingInThem(hasOtherBreastRows);
		//			invertedNippleCounter = null;
		//			return true;
		//		}
		//		else
		//		{
		//			output = "";
		//			return false;
		//		}
		//	}
		//	else
		//	{
		//		invertedNippleCounter = null;
		//		output = "";
		//		return false;
		//	}
		//}

		//	//logic: if not normal or inverted, but small enough to be fully inverted => fully inverted
		//	//same as above, but too large for fully inverted, but too small to be fuckable => slightly inverted
		//	//if dick nipple and too small and the above two cases didn't proc => fuckable
		//	//if inverted in any way and of fuckable size => fuckable
		//	//if fully inverted and too large and not above => slightly inverted
		//	//if normal and too large => normal (75%), fuckable (25%)

		//	private void UpdateNippleStatus()
		//	{
		//		if (length < FULLY_INVERTED_THRESHOLD && nippleStatus != NippleStatus.NORMAL && !nippleStatus.IsInverted())
		//		{
		//			nippleStatus = NippleStatus.FULLY_INVERTED;
		//		}
		//		else if (length < FUCKABLE_NIPPLE_THRESHOLD && !nippleStatus.IsInverted() && nippleStatus != NippleStatus.NORMAL)
		//		{
		//			nippleStatus = NippleStatus.SLIGHTLY_INVERTED;
		//		}
		//		else if (length < DICK_NIPPLE_THRESHOLD && nippleStatus == NippleStatus.DICK_NIPPLE)
		//		{
		//			nippleStatus = NippleStatus.FUCKABLE;
		//		}
		//		else if (length > FUCKABLE_NIPPLE_THRESHOLD && nippleStatus.IsInverted())
		//		{
		//			nippleStatus = NippleStatus.FUCKABLE;
		//		}
		//		else if (length > FULLY_INVERTED_THRESHOLD && nippleStatus == NippleStatus.FULLY_INVERTED)
		//		{
		//			nippleStatus = NippleStatus.SLIGHTLY_INVERTED;
		//		}
		//		else if (length > FUCKABLE_NIPPLE_THRESHOLD && nippleStatus == NippleStatus.NORMAL && Utils.Rand(4) == 0)
		//		{
		//			nippleStatus = NippleStatus.FUCKABLE;
		//		}
		//	}
	}

	public sealed partial class NippleData : SimpleData
	{
		public readonly bool quadNipples;
		public readonly bool blackNipples;
		public readonly NippleStatus status;
		public readonly float length;
		public readonly int breastRowIndex;

		public readonly float lactationRate;
		public readonly LactationStatus lactationStatus;

		public readonly ReadOnlyPiercing<NipplePiercingLocation> nipplePiercings;

		public readonly float relativeLust;

		internal readonly BodyType bodyType;
		#region Text
		public string NounText() => NippleStrings.NounText(this);
		public string NounText(bool plural, bool allowQuadNippleIfApplicable = false) => NippleStrings.NounText(this, plural, allowQuadNippleIfApplicable);

		public string ShortDescription() => NippleStrings.ShortDescription(this, true, true);

		public string ShortDescription(bool plural, bool allowQuadNippleTextIfApplicable = true) => NippleStrings.ShortDescription(this, plural, allowQuadNippleTextIfApplicable);

		public string SingleItemDescription() => NippleStrings.SingleItemDescription(this, true);
		public string SingleItemDescription(bool allowQuadNippleIfApplicable) => NippleStrings.SingleItemDescription(this, allowQuadNippleIfApplicable);

		public string LongDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
		{
			return NippleStrings.LongDescription(this, alternateFormat, plural, usePreciseMeasurements);
		}
		public string FullDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
		{
			return NippleStrings.FullDescription(this, alternateFormat, plural, usePreciseMeasurements);
		}

		public string OneNippleOrOneOfQuadNipplesShort(string pronoun = "your")
		{
			return CommonBodyPartStrings.OneOfDescription(quadNipples, pronoun, ShortDescription());
		}

		public string OneNippleOrEachOfQuadNipplesShort(string pronoun = "your")
		{
			return OneNippleOrEachOfQuadNipplesShort(pronoun, out bool _);
		}

		public string OneNippleOrEachOfQuadNipplesShort(string pronoun, out bool isPlural)
		{
			isPlural = quadNipples;

			return CommonBodyPartStrings.EachOfDescription(quadNipples, pronoun, ShortDescription());
		}
		#endregion
		public NippleData(Guid creatureID, float length, int breastIndex, float lactationRate = 0, bool quadNipples = false, bool blackNipples = false,
			NippleStatus nippleStatus = NippleStatus.NORMAL, ReadOnlyPiercing<NipplePiercingLocation> piercings = null, float relativeLust = Creature.DEFAULT_LUST)
			: this(creatureID, length, breastIndex, BodyType.defaultValue, lactationRate, quadNipples, blackNipples, nippleStatus, piercings, relativeLust)
		{ }

		public NippleData(Guid creatureID, float length, int breastIndex, BodyType bodyType, float lactationRate = 0, bool quadNipples = false, bool blackNipples = false,
			NippleStatus nippleStatus = NippleStatus.NORMAL, ReadOnlyPiercing<NipplePiercingLocation> piercings = null, float relativeLust = Creature.DEFAULT_LUST) : base(creatureID)
		{
			this.length = length;
			this.breastRowIndex = breastIndex;
			this.blackNipples = blackNipples;
			this.quadNipples = quadNipples;
			this.status = nippleStatus;

			this.lactationRate = lactationRate;
			lactationStatus = Genitals.StatusFromRate(lactationRate);

			nipplePiercings = piercings ?? new ReadOnlyPiercing<NipplePiercingLocation>();

			this.relativeLust = Utils.Clamp2(relativeLust, 0, 100);

			this.bodyType = bodyType;
		}

		public NippleData(Guid creatureID, Gender currentGender) : base(creatureID)
		{
			length = currentGender.HasFlag(Gender.FEMALE) ? Nipples.FEMALE_DEFAULT_LENGTH : Nipples.MALE_DEFAULT_LENGTH;
			breastRowIndex = 0;
			status = NippleStatus.NORMAL;
			quadNipples = false;
			blackNipples = false;

			lactationRate = 0;
			lactationStatus = LactationStatus.NOT_LACTATING;

			nipplePiercings = new ReadOnlyPiercing<NipplePiercingLocation>();

			relativeLust = Creature.DEFAULT_LUST;

			bodyType = BodyType.defaultValue;
		}


		internal NippleData(Nipples source, int currbreastRowIndex) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			blackNipples = source.blackNipples;
			quadNipples = source.quadNipples;
			status = source.nippleStatus;
			length = source.length;

			breastRowIndex = currbreastRowIndex;

			nipplePiercings = source.nipplePiercing.AsReadOnlyData();

			lactationRate = source.lactationRate;
			lactationStatus = source.lactationStatus;

			nipplePiercings = new ReadOnlyPiercing<NipplePiercingLocation>();

			bodyType = source.bodyType;
		}
	}
}
