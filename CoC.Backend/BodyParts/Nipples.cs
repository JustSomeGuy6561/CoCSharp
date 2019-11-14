﻿//Nipples.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:27 AM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.UI;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Tools;
using System;
using WeakEvent;
using CoC.Backend.BodyParts.EventHelpers;

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
	public enum NipplePiercings { LEFT_HORIZONTAL, LEFT_VERTICAL, RIGHT_HORIZONTAL, RIGHT_VERTICAL }


	public sealed partial class Nipples : SimpleSaveablePart<Nipples, NippleWrapper>, IGrowable, IShrinkable
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
					var oldData = AsData();
					_length = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private float _length;

		public float width => length < 1 ? length / 2 : length / 4;

		public readonly Piercing<NipplePiercings> nipplePiercing;

		public bool isPierced => nipplePiercing.isPierced;
		public bool wearingJewelry => nipplePiercing.wearingJewelry;

		internal Nipples(Guid creatureID, Breasts parent, BreastPerkHelper initialPerkWrapper, Gender gender) : base(creatureID)
		{
			this.parent = parent ?? throw new ArgumentNullException(nameof(parent));

			length = initialPerkWrapper.NewNippleDefaultLength;

			nipplePiercing = new Piercing<NipplePiercings>(PiercingLocationUnlocked, SupportedJewelryByLocation);

			//SetupPiercingMagic();

			growthMultiplier = initialPerkWrapper.NippleGrowthMultiplier;
			shrinkMultiplier = initialPerkWrapper.NippleShrinkMultiplier;
			defaultNippleLength = initialPerkWrapper.NewNippleDefaultLength;
		}

		internal Nipples(Guid creatureID, Breasts parent, BreastPerkHelper initialPerkWrapper, float nippleLength) : base(creatureID)
		{
			this.parent = parent ?? throw new ArgumentNullException(nameof(parent));

			length = nippleLength;

			nipplePiercing = new Piercing<NipplePiercings>(PiercingLocationUnlocked, SupportedJewelryByLocation);

			//SetupPiercingMagic();

			growthMultiplier = initialPerkWrapper.NippleGrowthMultiplier;
			shrinkMultiplier = initialPerkWrapper.NippleShrinkMultiplier;
			defaultNippleLength = initialPerkWrapper.NewNippleDefaultLength;
		}

		public override NippleWrapper AsReadOnlyReference()
		{
			return new NippleWrapper(this, BreastRowIndex);
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

		

		private bool PiercingLocationUnlocked(NipplePiercings piercingLocation)
		{
			return true;
		}

		private JewelryType SupportedJewelryByLocation(NipplePiercings piercingLocation)
		{
			switch (piercingLocation)
			{
				case NipplePiercings.LEFT_HORIZONTAL:
				case NipplePiercings.RIGHT_HORIZONTAL:
					//i guess we'll consider tassels danglers, idk.
					return JewelryType.RING | JewelryType.BARBELL_STUD | JewelryType.SPECIAL | JewelryType.DANGLER | JewelryType.HORSESHOE;
				default:
					return JewelryType.BARBELL_STUD | JewelryType.SPECIAL;
			}
		}

		public bool EquipOrPierceAt(NipplePiercings piercingLocation, PiercingJewelry jewelry, bool forceIfEnabled = false)
		{
			bool retVal = nipplePiercing.EquipPiercingJewelryAndPierceIfNotPierced(piercingLocation, jewelry, forceIfEnabled);
			//SetupPiercingMagic();
			return retVal;
		}
		public bool EquipPiercingJewelry(NipplePiercings piercingLocation, PiercingJewelry jewelry, bool forceIfEnabled = false)
		{
			bool retVal = nipplePiercing.EquipPiercingJewelry(piercingLocation, jewelry, forceIfEnabled);
			//SetupPiercingMagic();
			return retVal;
		}
		public bool Pierce(NipplePiercings location, PiercingJewelry jewelry)
		{
			bool retVal = nipplePiercing.Pierce(location, jewelry);
			//SetupPiercingMagic();
			return retVal;
		}

		public PiercingJewelry RemovePiercingJewelry(NipplePiercings location, bool forceRemove = false)
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

		public NippleData AsData()
		{
			return new NippleData(this, BreastRowIndex);
		}

		private readonly WeakEventSource<SimpleDataChangedEvent<NippleWrapper, NippleData>> dataChangeSource =
			new WeakEventSource<SimpleDataChangedEvent<NippleWrapper, NippleData>>();

		public event EventHandler<SimpleDataChangedEvent<NippleWrapper, NippleData>> dataChanged
		{
			add => dataChangeSource.Subscribe(value);
			remove => dataChangeSource.Unsubscribe(value);
		}

		private void NotifyDataChanged(NippleData oldData)
		{
			dataChangeSource.Raise(this, new SimpleDataChangedEvent<NippleWrapper, NippleData>(AsReadOnlyReference(), oldData));
		}

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

	public sealed partial class NippleWrapper : SimpleWrapper<NippleWrapper, Nipples>
	{
		public bool quadNipples => sourceData.quadNipples;
		public bool blackNipples => sourceData.blackNipples;
		public NippleStatus status => sourceData.nippleStatus;
		public float length => sourceData.length;

		internal float growthMultiplier => sourceData.growthMultiplier;
		internal float shrinkMultiplier => sourceData.shrinkMultiplier;
		internal float minNippleLength => sourceData.minNippleLength;
		internal float defaultNippleLength => sourceData.defaultNippleLength;

		public uint dickNippleFuckCount => sourceData.dickNippleFuckCount;
		public uint nippleFuckCount => sourceData.nippleFuckCount;
		public uint orgasmCount => sourceData.orgasmCount;
		public uint dryOrgasmCount => sourceData.dryOrgasmCount;
		public float width => sourceData.width;
		public ReadOnlyPiercing<NipplePiercings> nipplePiercing => sourceData.nipplePiercing.AsReadOnlyCopy();
		public bool isPierced => sourceData.isPierced;
		public bool wearingJewelry => sourceData.wearingJewelry;


		public readonly int breastRowIndex;

		public string ShortDescription() => sourceData.ShortDescription();

		public string LongDescription() => sourceData.LongDescription();

		internal NippleWrapper(Nipples source, int currbreastRowIndex) : base(source)
		{
			breastRowIndex = currbreastRowIndex;
		}
	}

	public sealed class NippleData
	{
		public readonly bool quadNipples;
		public readonly bool blackNipples;
		public readonly NippleStatus status;
		public readonly float length;
		public readonly int breastRowIndex;

		internal NippleData(Nipples source, int currbreastRowIndex)
		{
			blackNipples = source.blackNipples;
			quadNipples = source.quadNipples;
			status = source.nippleStatus;
			length = source.length;

			breastRowIndex = currbreastRowIndex;
		}
	}
}
