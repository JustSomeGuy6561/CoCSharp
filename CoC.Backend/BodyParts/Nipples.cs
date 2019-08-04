//Nipples.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:27 AM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Perks;
using CoC.Backend.Tools;
using System;

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

	public enum NippleStatus { NORMAL, FULLY_INVERTED, SLIGHTLY_INVERTED, FUCKABLE, DICK_NIPPLE }
	public enum NipplePiercings { LEFT_HORIZONTAL, LEFT_VERTICAL, RIGHT_HORIZONTAL, RIGHT_VERTICAL }
	public sealed partial class Nipples : SimpleSaveablePart<Nipples>, IGrowable, IShrinkable, IBaseStatPerkAware
	{
		public const float MIN_NIPPLE_LENGTH = 0.25f;
		public const float MAX_NIPPLE_LENGTH = 50f;

		public const float FULLY_INVERTED_THRESHOLD = 1f; //above this, not fully inverted
		public const float FUCKABLE_NIPPLE_THRESHOLD = 3f; //above this, fuckable is possible.
		public const float DICK_NIPPLE_THRESHOLD = 5f; //above this, dick nipples possible.

		public const float LACTATION_THRESHOLD = 1f;

		internal const ushort INVERTED_COUNTDOWN_TIMER = 24 * 7 / 2; //3.5 Days.


		//i guess we'll call tassels danglers - idk. 

		public NippleStatus nippleStatus
		{
			get => _nippleStatus;
			private set
			{
				if (value != _nippleStatus && (value == NippleStatus.FULLY_INVERTED || value == NippleStatus.SLIGHTLY_INVERTED))
				{
					SetupPiercingMagic();
				}
				_nippleStatus = value;
			}
		}
		private NippleStatus _nippleStatus;
		public float length
		{
			get => _length;
			private set => _length = Utils.Clamp2(value, MIN_NIPPLE_LENGTH, MAX_NIPPLE_LENGTH);
		}
		private float _length;

		public bool quadNipples { get; private set; }
		public bool blackNipples { get; private set; }

		public readonly Piercing<NipplePiercings> nipplePiercing;

		public bool isPierced => nipplePiercing.isPierced;
		public bool wearingJewelry => nipplePiercing.wearingJewelry;

		private Nipples()
		{
			nippleStatus = NippleStatus.NORMAL;
			length = MIN_NIPPLE_LENGTH;
			blackNipples = false;
			quadNipples = false;

			nipplePiercing = new Piercing<NipplePiercings>(PiercingLocationUnlocked, SupportedJewelryByLocation);

			SetupPiercingMagic();
		}

		internal static Nipples Generate()
		{
			return new Nipples();
		}

		internal static Nipples GenerateWithLength(float nippleLength)
		{
			return new Nipples()
			{
				length = nippleLength
			};
		}


		internal float GrowNipple(float growAmount, bool ignorePerk = false)
		{
			float oldLength = length;
			if (!ignorePerk)
			{
				growAmount *= perkData().NippleGrowthMultiplier;
			}
			length += growAmount;
			return length - oldLength;
		}

		internal float ShrinkNipple(float shrinkAmount, bool ignorePerk = false)
		{
			float oldLength = length;
			if (!ignorePerk)
			{
				shrinkAmount *= perkData().NippleShrinkMultiplier;
			}
			length -= shrinkAmount;
			return oldLength - length;
		}

		internal void setQuadNipple(bool active)
		{
			quadNipples = active;
		}

		internal void setBlackNipple(bool active)
		{
			blackNipples = active;
		}

		internal void setNippleStatus(NippleStatus status)
		{
			nippleStatus = status;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			//self-validating.
			bool valid = true;
			length = length;
			if (!Enum.IsDefined(typeof(NippleStatus), nippleStatus))
			{
				if (correctInvalidData)
				{
					nippleStatus = NippleStatus.NORMAL;
				}
				valid = false;
			}
			if (valid || correctInvalidData)
			{
				valid &= nipplePiercing.Validate(correctInvalidData);
			}
			return valid;
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
			SetupPiercingMagic();
			return retVal;
		}
		public bool EquipPiercingJewelry(NipplePiercings piercingLocation, PiercingJewelry jewelry, bool forceIfEnabled = false)
		{
			bool retVal = nipplePiercing.EquipPiercingJewelry(piercingLocation, jewelry, forceIfEnabled);
			SetupPiercingMagic();
			return retVal;
		}
		public bool Pierce(NipplePiercings location, PiercingJewelry jewelry)
		{
			bool retVal = nipplePiercing.Pierce(location, jewelry);
			SetupPiercingMagic();
			return retVal;
		}

		public PiercingJewelry RemovePiercingJewelry(NipplePiercings location, bool forceRemove = false)
		{
			PiercingJewelry jewelry = nipplePiercing.RemovePiercingJewelry(location, forceRemove);
			SetupPiercingMagic();
			return jewelry;
		}

		private void SetupPiercingMagic()
		{
			if (wearingJewelry && (nippleStatus == NippleStatus.SLIGHTLY_INVERTED || nippleStatus == NippleStatus.FULLY_INVERTED))
			{
				if (invertedNippleCounter == null)
				{
					invertedNippleCounter = 0;
				}
			}
			else
			{
				invertedNippleCounter = null;
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

		#region Perk Aware
		private PerkStatBonusGetter perkData;
		void IBaseStatPerkAware.GetBasePerkStats(PerkStatBonusGetter getter)
		{
			perkData = getter;
		}
		private IBaseStatPerkAware aware => this;
		internal void GetBasePerkStats(PerkStatBonusGetter getter)
		{
			aware.GetBasePerkStats(getter);
		}

		internal void DoLateInit(Gender currGender, BasePerkModifiers statModifiers, bool isInit)
		{
			if (isInit)
			{
				length = statModifiers.NewNippleDefaultLength;
			}
			else
			{
				length += statModifiers.NewNippleSizeDelta;
			}
		}

		#endregion

		private ushort? invertedNippleCounter = null;

		internal bool DoPiercingTimeNonsense(bool isPlayer, byte hoursPassed, bool hasOtherBreastRows, out string output)
		{


			if (nippleStatus == NippleStatus.FULLY_INVERTED && wearingJewelry)
			{
				nippleStatus = NippleStatus.SLIGHTLY_INVERTED;
				output = NipplesLessInvertedDueToPiercingInThem(hasOtherBreastRows);
				invertedNippleCounter = 0;
				return true;
			}
			else if (invertedNippleCounter != null)
			{
				invertedNippleCounter = ((ushort)invertedNippleCounter).add(hoursPassed);
				if (invertedNippleCounter > INVERTED_COUNTDOWN_TIMER)
				{
					nippleStatus = NippleStatus.NORMAL;
					output = NipplesNoLongerInvertedDueToPiercingInThem(hasOtherBreastRows);
					invertedNippleCounter = null;
					return true;
				}
				else
				{
					output = "";
					return false;
				}
			}
			else
			{
				invertedNippleCounter = null;
				output = "";
				return false;
			}
		}

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
}
