//Nipples.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:27 AM
using CoC.BodyParts.SpecialInteraction;
using CoC.Engine;
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts
{

	/*
	 * Normal: standard nipple. Inverted: Two stages - Fully or Slightly. Fully inverted requires a small nipple, and the entirety of it is inside. 
	 * Slightly inverted can be a little larger, but sticks out slightly. Fuckable is a wide nipple with the milk-hole (for lack of better term) inside large enough to fit dicks.
	 * dick nipple is a thin, long nipple, which appears normal, albeit oddly curved upward. when aroused, they grow and act like dicks. they can't impregnate, but do count as cum.
	 * as of now, there's only one way to revert any non-normal nipples - shrink them down to inverted, and have nipple piercings. after a period of 3.5 days, they'll be "pulled out"
	 * and be back to normal. it's still in the spirit of vanilla, which had no natural way of doing this, but it also didn't have inverted nipples. it's also "realistic" in that 
	 * people actually do this, though i suppose magic eggs would also work if we wanted. ofc non-natural means like bro brew, omnibus gift, and ceraph breast steals still work.
	 */
	public enum NippleStatus { NORMAL, FULLY_INVERTED, SLIGHTLY_INVERTED, FUCKABLE, DICK_NIPPLE }
	public enum NipplePiercings { LEFT_HORIZONTAL, LEFT_VERTICAL, RIGHT_HORIZONTAL, RIGHT_VERTICAL }
	public class Nipples : SimplePiercing<NipplePiercings>, IGrowShrinkable, ITimeAware
	{
		public const float MIN_NIPPLE_LENGTH = 0.25f;
		public const float MAX_NIPPLE_LENGTH = 50f;
		public const float FULLY_INVERTED_THRESHOLD = 1f; //above this, not fully inverted
		public const float FUCKABLE_NIPPLE_THRESHOLD = 3f; //above this, fuckable is possible.
		public const float DICK_NIPPLE_THRESHOLD = 5f; //above this, dick nipples possible.

		private const short INVERTED_COUNTDOWN_TIMER = 24 * 7 / 2; //3.5 Days.
		public NippleStatus nippleStatus
		{
			get => _nippleStatus;
			protected set
			{
				//make sure we initialize the inverted timer. 
				if (value.IsInverted() && !_nippleStatus.IsInverted())
				{
					invertedNippleCountDown = INVERTED_COUNTDOWN_TIMER;
				}
				_nippleStatus = value;
			}
		}
		private NippleStatus _nippleStatus;
		public float length
		{
			get => _length;
			protected set
			{
				Utils.Clamp(ref value, MIN_NIPPLE_LENGTH, MAX_NIPPLE_LENGTH);
				_length = value;
				UpdateNippleStatus();
				//set nipple status if it changes.
			}
		}
		private float _length;
		public bool quadNipples { get; protected set; }
		public bool blackNipples { get; protected set; }

		protected short invertedNippleCountDown;
		protected Nipples(PiercingFlags flags) : base(flags)
		{
			nippleStatus = NippleStatus.NORMAL;
			length = MIN_NIPPLE_LENGTH;
			blackNipples = false;
			quadNipples = false;
		}

		public GenericDescription ShortDescription => () => CoC.Strings.BodyParts.BreastNippleStrings.NippleShortDescription(this);
		public GenericDescription Description => () => CoC.Strings.BodyParts.BreastNippleStrings.NippleDescription(this);
		public GenericDescription DescriptionWithLength => () => CoC.Strings.BodyParts.BreastNippleStrings.NippleDescriptionWithLength(this);

		public static Nipples Generate(PiercingFlags flags)
		{
			return new Nipples(flags);
		}

		public static Nipples GenerateWithLength(PiercingFlags flags, float nippleLength)
		{
			return new Nipples(flags)
			{
				length = nippleLength
			};
		}

		public static Nipples GenerateForceType(PiercingFlags flags, float nippleLength, NippleStatus status)
		{
			return new Nipples(flags)
			{
				length = nippleLength,
				nippleStatus = status
			};
		}

		public void ForceNippleStatus(NippleStatus status)
		{
			nippleStatus = status;
			if (status == NippleStatus.DICK_NIPPLE && length < DICK_NIPPLE_THRESHOLD)
			{
				length = DICK_NIPPLE_THRESHOLD;
			}
			else if (status == NippleStatus.FULLY_INVERTED && length > FULLY_INVERTED_THRESHOLD)
			{
				length = FULLY_INVERTED_THRESHOLD / 2;
			}
			else if (status == NippleStatus.SLIGHTLY_INVERTED && length > FUCKABLE_NIPPLE_THRESHOLD)
			{
				length = FULLY_INVERTED_THRESHOLD;
			}
			else if (status == NippleStatus.FUCKABLE && length < FUCKABLE_NIPPLE_THRESHOLD)
			{
				length = FUCKABLE_NIPPLE_THRESHOLD;
			}
		}

		public float GrowNipple(float growAmount)
		{
			float oldLength = length;
			length += growAmount;
			return length - oldLength;
		}

		public float ShrinkNipple(float shrinkAmount)
		{
			float oldLength = length;
			length -= shrinkAmount;
			return oldLength - length;
		}

		public bool ActivateBlackNipples()
		{
			if (blackNipples)
			{
				return false;
			}
			blackNipples = true;
			return true;
		}

		public bool DeactivateBlackNipples()
		{
			if (!blackNipples)
			{
				return false;
			}
			blackNipples = false;
			return true;
		}

		public bool ActivateQuadNipples()
		{
			if (quadNipples)
			{
				return false;
			}
			quadNipples = true;
			return true;
		}
		public bool DeactivateQuadNipples()
		{
			if (!quadNipples)
			{
				return false;
			}
			quadNipples = false;
			return true;
		}
		public bool CanGrowPlus()
		{
			return length < MAX_NIPPLE_LENGTH;
		}

		public bool CanReducto()
		{
			return length > MIN_NIPPLE_LENGTH;
		}

		public float UseGroPlus()
		{
			if (!CanGrowPlus())
			{
				return 0;
			}
			float oldLength = length;
			length += Utils.Rand(6) / 20.0f + 0.25f; //ranges from 1/4- 1/2 inch.
			return length - oldLength; //returns that change in value. limited only if it reaches the max. 
		}

		public float UseReducto()
		{
			if (!CanReducto())
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

		protected override bool PiercingLocationUnlocked(NipplePiercings piercingLocation)
		{
			return true;
		}

		//revert inverted nipple with piercing countdown/countup timer.
		public void ReactToTimePassing(uint hoursPassed)
		{
			//if you're fully inverted, pull it out slightly immediately.
			if (nippleStatus == NippleStatus.FULLY_INVERTED && currentJewelryCount > 0)
			{
				nippleStatus = NippleStatus.SLIGHTLY_INVERTED;
			}
			//if it's slightly inverted, pierced, and the countdown is > 0, decrement the counter
			else if (nippleStatus.IsInverted() && currentJewelryCount > 0 && invertedNippleCountDown > 0)
			{
				invertedNippleCountDown-= (short)hoursPassed;
				if (invertedNippleCountDown < 0)
				{
					invertedNippleCountDown = 0;
					nippleStatus = NippleStatus.NORMAL;
				}
			}
			//if slightly inverted, pierced, and countdown is <= 0, revert to normal.
			else if (nippleStatus.IsInverted() && currentJewelryCount > 0)
			{
				nippleStatus = NippleStatus.NORMAL;
			}
			//if slightly inverted, countdown started, but it's no longer pierced, increment the count.
			else if (nippleStatus.IsInverted() && currentJewelryCount == 0 && invertedNippleCountDown < INVERTED_COUNTDOWN_TIMER)
			{
				invertedNippleCountDown+= (short)hoursPassed;
				if (invertedNippleCountDown > INVERTED_COUNTDOWN_TIMER)
				{
					invertedNippleCountDown = INVERTED_COUNTDOWN_TIMER;
				}
			}
		}

		//logic: if not normal or inverted, but small enough to be fully inverted => fully inverted
		//same as above, but too large for fully inverted, but too small to be fuckable => slightly inverted
		//if dick nipple and too small and the above two cases didn't proc => fuckable
		//if inverted in any way and of fuckable size => fuckable
		//if fully inverted and too large and not above => slightly inverted
		//if normal and too large => normal (75%), fuckable (25%)

		private void UpdateNippleStatus()
		{
			if (length < FULLY_INVERTED_THRESHOLD && nippleStatus != NippleStatus.NORMAL && !nippleStatus.IsInverted())
			{
				nippleStatus = NippleStatus.FULLY_INVERTED;
			}
			else if (length < FUCKABLE_NIPPLE_THRESHOLD && !nippleStatus.IsInverted() && nippleStatus != NippleStatus.NORMAL)
			{
				nippleStatus = NippleStatus.SLIGHTLY_INVERTED;
			}
			else if (length < DICK_NIPPLE_THRESHOLD && nippleStatus == NippleStatus.DICK_NIPPLE)
			{
				nippleStatus = NippleStatus.FUCKABLE;
			}
			else if (length > FUCKABLE_NIPPLE_THRESHOLD && nippleStatus.IsInverted())
			{
				nippleStatus = NippleStatus.FUCKABLE;
			}
			else if (length > FULLY_INVERTED_THRESHOLD && nippleStatus == NippleStatus.FULLY_INVERTED)
			{
				nippleStatus = NippleStatus.SLIGHTLY_INVERTED;
			}
			else if (length > FUCKABLE_NIPPLE_THRESHOLD && nippleStatus == NippleStatus.NORMAL && Utils.Rand(4) == 0)
			{
				nippleStatus = NippleStatus.FUCKABLE;
			}
		}
	}
}
