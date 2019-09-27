using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Engine.Time;
using CoC.Backend.Tools;
using System;
using System.Linq;

namespace CoC.Backend.BodyParts
{
	public enum LactationStatus { NOT_LACTATING, LIGHT, MODERATE, STRONG, HEAVY, EPIC }

	public partial class Genitals : IBodyPartTimeDaily
	{
#warning Add a means of preventing lactation decrease due to pregnancy that doesn't require pregnancy set the perk value. 
		#region Lactation Perk Data

#warning Consider adding text for lactation rate increase so that can be handled. 
		//increase or decrease the lactation capacity based on perk data.
		internal float perkLactationCapacityMultiplierOffset { get; set; } = 0;
		
		//prevent the character from decreasing their current production rate at all.
		internal bool preventLactationDecrease { get; set; } = false;

		//prevent the character from decreasing their current production rate below a certain level. 
		internal LactationStatus minimumLactationLevel { get; set; } = LactationStatus.NOT_LACTATING;

		//the character can store their absolute maximum lactation capacity, even if they aren't lactating heavily. 
		internal bool currentCapacityAlwaysMaxCapacity { get; set; } = false; 
		#endregion

		//Lactation: 
		//rework of lactation to allow common behavior between NPCs and PC. This is built from 3 parts - capacity, lactation rate, and overfullBuffer. Capacity is based on your form
		//and is always available, even when you aren't lactating. the lactation rate affects how much you actually lactate, and as a result, how fast you fill up.
		//overfullBuffer is a buffer that allows NPCs and players to have some leeway at max capacity before they start to be affected by their overfullness. Once at max capacity,
		//the character has the duration of the buffer to be milked or their production will slow down. This allows different characters to handle being overly full at different rates
		//without requiring crazy levels of customization. For example, the player has 48 hours between milkings before suffering any adverse effects, regardless of lactation amount. 
		//Katherine, however, has no real buffer, and has to be milked as often as she is full or will slow down production. AFAIK marble is just perma-lactating, so she could have 
		//a max value buffer, or simply just set prevent lactation decrease. 

		public const float MIN_LACTATION_MODIFIER = 0;
		public const float LACTATION_THRESHOLD = 1.0f; //below this: not lactating. above this: lactating.
		public const float MODERATE_LACTATION_THRESHOLD = 2.5f;
		public const float STRONG_LACTATION_THRESHOLD = 5f;
		public const float HEAVY_LACTATION_THRESHOLD = 7f;
		public const float EPIC_LACTATION_THRESHOLD = 9f;
		public const float MAX_LACTATION_MODIFIER = 10;


		public float maximumLactationCapacity => lactation_TotalCapacityMultiplier * (float)breastRows.Sum(x => volumeFromCupSize(x.cupSize) * x.numBreasts);
		//current maximum capacity. if you aren't lactating, this is 0. 
		public float currentLactationCapacity => maximumLactationCapacity * lactationLevel;
		private double volumeFromCupSize(CupSize cup)
		{
			//fun fact, i'm now more versed in how cup size works than most women - apparently 60% don't get proper fitting cup/bra measures. The more you know. 
			//Short version: actual capacity varies by figure, notably how wide the person is. basically, cupsize is the delta from measurement at sternum and across the breasts at the nipple level.
			//for our calculations, we're gonna assume a "true" cup size - that is, ~a 30inch measurement at sternum, or a 34band size. 

			//we'll simulate storage by using breast size combined with the internals that don't exactly show on a person's physique, so flat/a cups will still have some lactation, albeit very low amounts.

			byte cupByte = (byte)cup;

			//fudging this - basically, cupsize ~= measurement@nipples - (measurement@sternum+4) which roughly means breast height is 1/2 the cupsize value, but b/c breasts are curved, it's actually even less
			//so we fudge it to ~1/2.54. then we convert that to CM, and it cancels out. i figure that conversion is probably a little off, so i fudge back in an extra .25cm. 
			double height = cupByte + .25;

			//radius of the breast where it meets the chest. we'll assume the figure gets slightly more accomodating as the breasts get bigger, so we widen the radius slightly per cup size.
			double chestRadius = 2.5 + cupByte / 40.0;

			//note: 

			//tiny, not enough volume to be spherical. using an ellipsoid instead. the heights are small enough here that we can use a full ellipsoid and chalk it up to internal storage. 
			if (cupByte < 3)
			{
				return 2 * chestRadius * chestRadius * height * 2 / 3 * Math.PI;
			}
			//big enough to have volume, but still small enough to remain firm and thus are roughly spherical. 
			if (cupByte < 5)
			{
				double breastRadius = cupByte;
				//this might over-estimate internal storage a little, but this isn't precision math for rocket science, so cut me some slack lol. 
				return 4 / 3 * Math.PI * Math.Pow(breastRadius, 3);
			}
			//biggest formula. no longer spherical, because gravity. we'll use a truncated sphere (think cylinder that gets wider as it goes along), that ends in a half-ellipsoid. 
			//i'm more or less ignoring internal storage here because we have so much of it outside. i just add an extra 10 and call it a day. 
			else
			{
				//ellipsoid at the end.
				double breastRadius = 5 + cupByte / 16.0f;
				double breastHeight = breastRadius / 2;
				//the length of the cone part
				double breastLength = height - breastRadius;
				//radius of cone at base: chestRadius. radius at end: breastRadius

				return 10 + Math.PI / 3 * (2 * breastHeight * Math.Pow(breastRadius, 2) + Math.Pow(breastRadius + chestRadius, 2) * breastLength);
			}

		}
		//multiplies capacity volume by this value to determine actual amount you can lactate. completely breaks physics, but so does most of this game, so...
		public float lactation_TotalCapacityMultiplier => lactation_CapacityMultiplier + perkLactationCapacityMultiplierOffset;

		//some items may make your milk more "dense" for lack of better word, allowing you to lactate more without altering your capacity. this is that value. 
		public float lactation_CapacityMultiplier { get; private set; } = 1;

		//This is the internal value that helps determine how much you're lactating. It's a range from 0-10, with 0 being not lactating and 10 being lactating at an ungodly rate.
		//it will update automatically based on how often you breastfeed and how full you are. Boosting lactation will directly update this value.
		//This value is used in calculations, and you should try not to use it in your logic. Lactation Status is much less arbitrary and thus are better to use.
		//however, there may be cases where you want to boost lactation based on the current value, so this is available to you. 
		public float lactation_ProductionModifier
		{
			get => _lactationModifier;
			private set
			{
				_lactationModifier = Utils.Clamp2(value, MIN_LACTATION_MODIFIER, MAX_LACTATION_MODIFIER);
			}
		}
		private float _lactationModifier = 0;

		public float lactationRate => Utils.Lerp(LACTATION_THRESHOLD, EPIC_LACTATION_THRESHOLD, lactation_ProductionModifier, 0, 1.0f);

		//when you boost lactation to certain thresholds, your body can carry a larger amount of the full capacity. 
		private float lactationLevel
		{
			get
			{
				if (currentCapacityAlwaysMaxCapacity)
				{
					return isLactating ? 1 : 0;
				}
				else
				{
					float currLevel = (int)lactationStatus * 0.25f;
					if (currLevel > 1) return 1;
					else return currLevel;
				}
			}
		}

		//how much time does this character have at full capacity before their lactation modifier starts decreasing, stored in hours. Note that at epic level, this value is halved, rounded up.
		public uint overfullBuffer { get; private set; } = 0;


		//takes 48 hours to fill when less than strong. 24 when strong, 12 when heavy, 6 when epic. 
		private float hourlyFillRate
		{
			get
			{
				if (lactationStatus == LactationStatus.NOT_LACTATING)
				{
					return 0;
				}
				else if (lactationStatus < LactationStatus.STRONG)
				{
					return currentLactationCapacity / 48;
				}
				else if (lactationStatus < LactationStatus.HEAVY)
				{
					return currentLactationCapacity / 24;
				}
				else if (lactationStatus < LactationStatus.EPIC)
				{
					return currentLactationCapacity / 12;
				}
				else
				{
					return currentLactationCapacity / 6;
				}
			}
		}
		//converts the lactation modifier into something that is less arbitrary from a human interpretation standpoint. running a check against this means you don't have magic numbers
		//and your intent is much clearer. 
		public LactationStatus lactationStatus
		{
			get
			{
				if (lactation_ProductionModifier < LACTATION_THRESHOLD)
				{
					return LactationStatus.NOT_LACTATING;
				}
				else if (lactation_ProductionModifier < MODERATE_LACTATION_THRESHOLD)
				{
					return LactationStatus.LIGHT;
				}
				else if (lactation_ProductionModifier < STRONG_LACTATION_THRESHOLD)
				{
					return LactationStatus.MODERATE;
				}
				else if (lactation_ProductionModifier < HEAVY_LACTATION_THRESHOLD)
				{
					return LactationStatus.STRONG;
				}
				else if (lactation_ProductionModifier < EPIC_LACTATION_THRESHOLD)
				{
					return LactationStatus.HEAVY;
				}
				else
				{
					return LactationStatus.EPIC;
				}
			}
		}

		public float currentLactationAmount { get; private set; }

		public bool isLactating => lactationStatus != LactationStatus.NOT_LACTATING;


		//
		/// <summary>
		///boost your lactation or decrease your lactation until it reaches the given threshold, if possible. Returns the actual threshold it reached.
		/// </summary>
		/// <param name="newStatus">The new lactation status desired.</param>
		/// <returns>The lactation status given, or the closest value it can reach do to other factors.</returns>
		public LactationStatus setLactationTo(LactationStatus newStatus)
		{
			lactation_ProductionModifier = newStatus.MinThreshold();
			return lactationStatus;
		}

		/// <summary>
		/// attempts to set the current lactation value to 0, clearing the lactation. will fail to do so if other factors prevent this.
		/// </summary>
		/// <returns>True if the character is no longer lactating, false if some other factor prevents this. </returns>
		public bool clearLactation()
		{
			return setLactationTo(LactationStatus.NOT_LACTATING) == LactationStatus.NOT_LACTATING;
		}

		/// <summary>
		/// Attempts to boost the current lactation modifier by the given value, and returns the amount it actually boosted. if the value is negative and causes the character to drop below the 
		/// lactation threshold, sets it to 0. can be called if the current 
		/// </summary>
		/// <param name="byAmount">The amount to change the lactation modifier</param>
		/// <returns>the amount the modifier changed.</returns>
		public float boostLactation(float byAmount = 0.1f)
		{
			if (!preventLactationDecrease && byAmount < 0)
			{
				return 0;
			}
			var modifier = lactation_ProductionModifier;
			lactation_ProductionModifier += byAmount;
			if (lactationStatus < minimumLactationLevel)
			{
				setLactationTo(minimumLactationLevel);
			}
			else if (lactation_ProductionModifier < LACTATION_THRESHOLD && byAmount < 0)
			{
				lactation_ProductionModifier = 0;
			}
			return lactation_ProductionModifier - modifier;
		}

		/// <summary>
		/// If the character is lactating, boost lactation by the default amount. Otherwise, set the production level so the character starts lactating.
		/// </summary>
		public void StartOrBoostLactation()
		{

			var oldLactation = lactation_ProductionModifier;
			if (!isLactating)
			{
				setLactationTo(LactationStatus.LIGHT);
			}
			else
			{
				boostLactation();
			}
		}

		/// <summary>
		/// Attempt to milk the current character, with as much milk as they can provide. Can be called when the character is not lactating, which will help induce lactation.
		/// </summary>
		/// <returns>The amount of milk lactated</returns>
		public float MilkOrSuckle()
		{
			timeLastMilked = GameDateTime.Now;
			var retVal = currentLactationAmount;
			currentLactationAmount = 0;
			timeBecameFull = null;
			boostLactation();
			return retVal;
		}

		/// <summary>
		/// Attempt to milk the current character, up to a certain max threshold. negative numbers are treated as max value. 
		/// Can be called when the character is not lactating, which will help induce lactation.
		/// </summary>
		/// <param name="maxAmount">The maximum amount allowed to be lactated</param>
		/// <returns>The amount of milk lactated</returns>
		public float MilkOrSuckle(float maxAmount)
		{
			if (maxAmount < 0)
			{
				maxAmount = float.MaxValue;
			}
			if (maxAmount == 0)
			{
				return 0;
			}


			timeBecameFull = null;
			timeLastMilked = GameDateTime.Now;
			currentLactationAmount -= maxAmount;
			if (currentLactationAmount < 0)
			{
				var amountConsumed = maxAmount + currentLactationAmount;
				currentLactationAmount = 0;
				boostLactation();
				return amountConsumed;
			}
			else
			{
				return maxAmount;
			}
		}

		private string DoLactationCheck(bool isPlayer, byte hoursPassed)
		{
			if (currentLactationAmount < currentLactationCapacity)
			{
				float hoursRequiredToFill = (currentLactationCapacity - currentLactationAmount) / hourlyFillRate;
				if (hoursRequiredToFill > hoursPassed)
				{
					currentLactationAmount += hoursPassed * hourlyFillRate;
					timeBecameFull = null;
				}
				else if (Math.Ceiling(hoursRequiredToFill) == hoursPassed)
				{
					currentLactationAmount = currentLactationCapacity;
					timeBecameFull = GameDateTime.Now;
				}
				else
				{
					hoursPassed -= (byte)Math.Floor(hoursRequiredToFill);
					currentLactationAmount = currentLactationCapacity;
					timeBecameFull = GameDateTime.HoursFromNow(-hoursPassed);
				}
			}

			int hoursSinceFull = timeBecameFull?.hoursToNow() ?? 0;
			uint buffer = lactationStatus != LactationStatus.EPIC ? overfullBuffer : (overfullBuffer + 1) / 2;

			if (timeBecameFull != null && currentLactationAmount >= currentLactationCapacity && hoursSinceFull > buffer)
			{
				hoursSinceFull -= (int)buffer;

				var oldStatus = lactationStatus;
				if (hoursSinceFull > hoursPassed)
				{
					boostLactation((float)((hoursPassed + (breastRows.Count-1) / 2.0) * -0.1));
				}
				else
				{
					boostLactation((float)((hoursSinceFull + (breastRows.Count-1) / 2.0) * -0.1));
				}
				if ((lactationStatus != oldStatus || hoursPassed > hoursSinceFull) && isPlayer)
				{
					if (lactationStatus != oldStatus)
					{
						return LactationSlowedDownDueToInactivity(hoursPassed > hoursSinceFull, oldStatus);
					}
					else
					{
						return LactationFullWarning();
					}
				}
			}

			return "";
		}

		//check if the character is not lactating, but attempting to induce lactation - if they haven't tried to induce it in the last 48 hours, decrease the production modifer
		//back toward 0. 
		string IBodyPartTimeDaily.reactToDailyTrigger(bool isPlayer)
		{
			if (GameDateTime.Now.day %2 == 0 && !isLactating && hoursSinceLastMilked > 48 && lactation_ProductionModifier != 0)
			{
				boostLactation(-0.1f);
			}
			return "";
		}

		private GameDateTime timeLastMilked;

		private GameDateTime timeBecameFull;
		public int hoursSinceLastMilked => timeLastMilked.hoursToNow();

		byte IBodyPartTimeDaily.hourToTrigger => 0;

#warning TODO: implement time aware for this to fill up over time, a milking and/or suckling method. a means to min/max lactationStatus. 
		//likely need a times ran out of m
	}

	public static class LactationHelper
	{
		public static float MinThreshold(this LactationStatus lactationStatus)
		{
			switch (lactationStatus)
			{
				case LactationStatus.EPIC:
					return Genitals.EPIC_LACTATION_THRESHOLD;
				case LactationStatus.HEAVY:
					return Genitals.HEAVY_LACTATION_THRESHOLD;
				case LactationStatus.STRONG:
					return Genitals.STRONG_LACTATION_THRESHOLD;
				case LactationStatus.MODERATE:
					return Genitals.MODERATE_LACTATION_THRESHOLD;
				case LactationStatus.LIGHT:
					return Genitals.LACTATION_THRESHOLD;
				case LactationStatus.NOT_LACTATING:
				default:
					return 0;
			}
		}
	}
}
