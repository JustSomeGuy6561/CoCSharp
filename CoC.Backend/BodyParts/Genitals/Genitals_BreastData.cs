using CoC.Backend.Engine.Time;
using CoC.Backend.Tools;
using System;
using System.Linq;

namespace CoC.Backend.BodyParts
{
	public partial class Genitals
	{
		#region Notes:

		//Lactation: 
		//rework of lactation to allow common behavior between NPCs and PC. This is built from 3 parts - capacity, lactation rate, and overfullBuffer. Capacity is based on your form
		//and is always available, even when you aren't lactating. the lactation rate affects how much you actually lactate, and as a result, how fast you fill up.
		//overfullBuffer is a buffer that allows NPCs and players to have some leeway at max capacity before they start to be affected by their overfullness. Once at max capacity,
		//the character has the duration of the buffer to be milked or their production will slow down. This allows different characters to handle being overly full at different rates
		//without requiring crazy levels of customization. For example, the player has 48 hours between milkings before suffering any adverse effects, regardless of lactation amount. 
		//Katherine, however, has no real buffer, and has to be milked as often as she is full or will slow down production. AFAIK marble is just perma-lactating, so she could have 
		//a max value buffer, or simply just set prevent lactation decrease. 

		#endregion

		#region Breast Constants
		//max in game that i can find is 5, but they only ever use 4 rows.
		//apparently Fenoxo said 3 rows, but then after it went open, some shit got 4 rows.
		//i'm not being a dick and reverting that. 4 it is.
		public const int MAX_BREAST_ROWS = 4;
		#endregion

		#region Lactation Related Constants

		public const float MIN_LACTATION_MODIFIER = 0;
		public const float LACTATION_THRESHOLD = 1.0f; //below this: not lactating. above this: lactating.
		public const float MODERATE_LACTATION_THRESHOLD = 2.5f;
		public const float STRONG_LACTATION_THRESHOLD = 5f;
		public const float HEAVY_LACTATION_THRESHOLD = 7f;
		public const float EPIC_LACTATION_THRESHOLD = 9f;
		public const float MAX_LACTATION_MODIFIER = 10;

		#endregion

		#region Public Lactation Related Members

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

		//how much time does this character have at full capacity before their lactation modifier starts decreasing, stored in hours. Note that at epic level, this value is halved, rounded up.
		public uint overfullBuffer { get; private set; } = 0;

		public float currentLactationAmount { get; private set; }
		#endregion

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

		#region Private Breast Related Members
		private uint missingRowTitFuckCount = 0;
		private uint missingRowBreastOrgasmCount = 0;
		private uint missingRowBreastDryOrgasmCount = 0;
		#endregion

		#region Private Lactation Related Members
		private GameDateTime timeLastMilked;

		private GameDateTime timeBecameFull;

		#endregion

		#region Public Breast Related Computed Values
		public int numBreastRows => _breasts.Count;

		public int numBreasts => _breasts.Sum(x => x.numBreasts);

		public uint titFuckCount => missingRowTitFuckCount + (uint)breastRows.Sum(x => x.titFuckCount);

		public uint breastOrgasmCount => missingRowBreastOrgasmCount.add((uint)breastRows.Sum(x => x.orgasmCount));
		public uint breastDryOrgasmCount => missingRowBreastDryOrgasmCount.add((uint)breastRows.Sum(x => x.dryOrgasmCount));

		#endregion

		#region Public Lactation Related Computed Values
		public bool canLessenCurrentLactationLevels => !preventLactationDecrease && !isPregnant;

		public int hoursSinceLastMilked => timeLastMilked.hoursToNow();

		public bool isOverfull => timeBecameFull?.hoursToNow() >= 0;

		public int hoursOverfull => timeBecameFull?.hoursToNow() ?? -1;

		//note: it's possible to actually go over the max capacity if you're overfull, and going at the highest lactation level. Original game set this to 1.5; i'm going to cap it at 1.1
		public float maximumLactationCapacity => lactation_TotalCapacityMultiplier * (float)breastRows.Sum(x => volumeFromCupSize(x.cupSize) * x.numBreasts);
		//current maximum capacity. if you aren't lactating, this is 0. 
		public float currentLactationCapacity => maximumLactationCapacity * lactationLevel * (isOverfull ? 1.1f : 1.0f);

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

		public bool isLactating => lactationStatus != LactationStatus.NOT_LACTATING;
		#endregion

		#region Breast Aggregate Functions

		public CupSize BiggestCupSize()
		{
			return (CupSize)_breasts.Max(x => (byte?)x?.cupSize);
		}

		public CupSize AverageCupSize()
		{
			return (CupSize)(byte)Math.Ceiling(_breasts.Average(x => (double)x.cupSize));
		}

		public CupSize SmallestCupSize()
		{
			return (CupSize)_breasts.Min(x => (byte?)x?.cupSize);
		}

		public Breasts LargestBreast()
		{
			return _breasts.MaxItem(x => (byte)x.cupSize);
		}

		public Breasts SmallestBreast()
		{
			return _breasts.MinItem(x => (byte)x.cupSize);
		}

		#endregion

		#region Add/Remove Breasts

		public bool AddBreastRow()
		{
			if (numBreastRows >= MAX_BREAST_ROWS)
			{
				return false;
			}

			var cup = _breasts[_breasts.Count - 1].cupSize;
			var length = _breasts[_breasts.Count - 1].nipples.length;
			_breasts.Add(new Breasts(creatureID, GetBreastPerkWrapper(), cup, length));
			return true;
		}
		public bool AddBreastRowAverage()
		{
			if (numBreastRows >= MAX_BREAST_ROWS)
			{
				return false;
			}
			//linq ftw!
			//i find it funny that linq was created for databases, but it really is used for functional programming.
			double avgLength = _breasts.Average((x) => (double)x.nipples.length);
			double avgCup = _breasts.Average((x) => (double)x.cupSize);
			byte cup = (byte)Math.Ceiling(avgCup);
			_breasts.Add(new Breasts(creatureID, GetBreastPerkWrapper(), (CupSize)cup, (float)avgLength));
			return true;
		}

		public bool AddBreastRow(CupSize cup)
		{
			if (numBreastRows >= MAX_BREAST_ROWS)
			{
				return false;
			}
			double avgLength = _breasts.Average((x) => (double)x.nipples.length);
			_breasts.Add(new Breasts(creatureID, GetBreastPerkWrapper(), cup, (float)avgLength));
			return true;
		}

		public int RemoveBreastRows(int count = 1)
		{
			if (count < 0 || numBreastRows == 1 && _breasts[0].isMale)
			{
				return 0;
			}

			int oldCount = numBreastRows;
			//if over the number of breasts, reset the first one and set the number to remove to one less than the total.
			if (count >= numBreastRows)
			{
				missingRowTitFuckCount += _breasts[0].titFuckCount;
				missingRowBreastOrgasmCount += _breasts[0].orgasmCount;
				missingRowBreastDryOrgasmCount += _breasts[0].dryOrgasmCount;

				missingRowNippleFuckCount += _breasts[0].nippleFuckCount;
				missingRowDickNippleSexCount += _breasts[0].dickNippleFuckCount;
				missingRowNippleOrgasmCount += _breasts[0].nippleOrgasmCount;
				missingRowNippleDryOrgasmCount += _breasts[0].nippleDryOrgasmCount;
				_breasts[0].Reset();

				count = numBreastRows - 1;
			}

			missingRowTitFuckCount += (uint)breastRows.Skip(numBreastRows - count).Sum(x => x.titFuckCount);
			missingRowBreastOrgasmCount += (uint)breastRows.Skip(numBreastRows - count).Sum(x => x.orgasmCount);
			missingRowBreastDryOrgasmCount += (uint)breastRows.Skip(numBreastRows - count).Sum(x => x.dryOrgasmCount);

			missingRowNippleFuckCount += (uint)breastRows.Skip(numBreastRows - count).Sum(x => x.nippleFuckCount);
			missingRowDickNippleSexCount += (uint)breastRows.Skip(numBreastRows - count).Sum(x => x.dickNippleFuckCount);
			missingRowNippleOrgasmCount += (uint)breastRows.Skip(numBreastRows - count).Sum(x => x.nippleOrgasmCount);
			missingRowNippleDryOrgasmCount += (uint)breastRows.Skip(numBreastRows - count).Sum(x => x.nippleDryOrgasmCount);

			_breasts.RemoveRange(numBreastRows - count, count);

			return oldCount - numBreastRows;
		}

		public int RemoveExtraBreastRows()
		{
			return RemoveBreastRows(numBreastRows - 1);
		}

		#endregion

		#region Update All Breasts Functions
		/// <summary>
		/// Evens out all breast rows so they are closer to the average nipple length and cup size, rounding up.
		/// large ones are shrunk, small ones grow. only does one unit of change, unless until even is set, then
		/// will completely average all values.
		/// </summary>
		/// <param name="untilEven">if true, forces all breast rows to average value, if false, only one unit.</param>
		public void NormalizeBreasts(bool untilEven = false)
		{
			if (numBreastRows == 1)
			{
				return;
			}
			CupSize averageSize = AverageCupSize();
			if (untilEven)
			{
				foreach (var row in _breasts)
				{
					row.SetCupSize(averageSize);
				}
			}
			else
			{
				foreach (var row in _breasts)
				{
					if (row.cupSize > averageSize)
					{
						row.ShrinkBreasts(1);
					}
					else if (row.cupSize < averageSize)
					{
						row.GrowBreasts(1);
					}
				}
			}
		}
		#endregion

		#region Lactation Update Functions
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
		/// <remarks>This will return 0 if the creature is currently pregnant or if they have a perk that prevents this number from decreasing.</remarks>
		public float boostLactation(float byAmount = 0.1f)
		{
			if (!canLessenCurrentLactationLevels && byAmount < 0)
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
		#endregion

		#region Breast Sex Related Functions
		//to be frank, idk what would actually orgasm when being titty fucked, but, uhhhh... i guess it can be stored in stats or some shit?
		internal void HandleTittyFuck(int breastIndex, Cock sourceCock, bool reachOrgasm)
		{
			HandleTittyFuck(breastIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, sourceCock.cumAmount, reachOrgasm);
		}

		internal void HandleTittyFuck(int breastIndex, Cock sourceCock, float cumAmountOverride, bool reachOrgasm)
		{
			HandleTittyFuck(breastIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, cumAmountOverride, reachOrgasm);
		}

		internal void HandleTittyFuck(int breastIndex, float length, float girth, float knotWidth, float cumAmount, bool reachOrgasm)
		{
			_breasts[breastIndex].DoTittyFuck(length, girth, knotWidth, reachOrgasm);
		}

		internal void HandleTitOrgasmGeneric(int breastIndex, bool dryOrgasm)
		{
			_breasts[breastIndex].OrgasmTits(dryOrgasm);
		}
		#endregion

		#region Lactation Sex Related Functions

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
		#endregion

		#region Private Lactation Helper Functions
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


		#endregion

		#region Lazy Listener
		//This is written in this format to match the rest of the code in genitals for lazy listeners. it could just return a string, but this is more consistent. 

		//There's actually a strange behavior here in that the moment you become full, you get the overfull bonus (1.1x capacity), even if the amount you are adding would not cause 
		//you to reach the overfull capacity. frankly, considering the old method was just multiply it by 1.5 regardless of if you just became full or have been for hours,
		//i think this is fine.
		private bool DoLazyLactationCheck(bool isPlayer, byte hoursPassed, out string results)
		{
			//if we're lactating, handle any filling or overfilling, and any related changes to lactation rate.
			if (isLactating)
			{

				//if we aren't initially full.
				if (currentLactationAmount < currentLactationCapacity)
				{

					float hoursRequiredToFill = (currentLactationCapacity - currentLactationAmount) / hourlyFillRate;
					//and still aren't full.
					if (hoursRequiredToFill > hoursPassed)
					{
						currentLactationAmount += hoursPassed * hourlyFillRate;
						timeBecameFull = null;
					}
					//and we just became full this hour.
					else if (Math.Ceiling(hoursRequiredToFill) == hoursPassed)
					{
						timeBecameFull = GameDateTime.Now;
						currentLactationAmount = currentLactationCapacity;
					}
					//and we became full over the hours passed.
					else
					{
						hoursPassed -= (byte)Math.Floor(hoursRequiredToFill);
						timeBecameFull = GameDateTime.HoursFromNow(-hoursPassed);
						currentLactationAmount = currentLactationCapacity;
					}
				}

				//if we're full
				if (timeBecameFull != null && currentLactationAmount >= currentLactationCapacity)
				{
					int buffer = (int)(lactationStatus != LactationStatus.EPIC ? overfullBuffer : (overfullBuffer + 1) / 2); //
					int hoursSinceOverranBuffer = timeBecameFull.hoursToNow() - buffer;

					if (hoursSinceOverranBuffer > 0)
					{
						LactationStatus oldStatus = lactationStatus;

						double multiplier = 0.1;
						bool overfullThisPass;
						if (hoursSinceOverranBuffer > hoursPassed)
						{
							overfullThisPass = false;
							multiplier *= hoursPassed;
						}
						else
						{
							overfullThisPass = true;
							multiplier *= hoursSinceOverranBuffer;
						}
						//formula:
						//(1 + breastCount / 8) * hours / 10. so .1 to .2 per hour, assuming 2 breasts per row. higher if that's not the case anymore. 
						boostLactation((float)((1 + numBreasts / 8) * multiplier));

						if (lactationStatus < oldStatus)
						{
							results = LactationSlowedDownDueToInactivity(overfullThisPass, oldStatus);
							return true;
						}

					}
					else if (hoursOverfull > 0 && hoursOverfull < hoursPassed)
					{
						results = LactationFullWarning();
						return false;
					}
				}
			}
			//otherwise, handle cases for induced lactation (or some other non-zero modifier below the lactation threshold)
			//we decrease this by 0.1 every 48 hours since the last time milked/attempted to induce lactation. 
			else if (lactation_ProductionModifier != 0 && hoursSinceLastMilked >= 48)
			{
				//we do this by seeing if the increase in hours passed has caused us to reach a new multiple of 48.
				//so, if we were previously at 43 hours and now we're at 51, for example. we do this via modulus of 48. 

				//this can be written as hourseSinceLastMilked % 48 < hoursPassed.

				//The math:
				//48x <= a < 48(x+1).        a - b < 48x. a, b > 0. x>= 0.  let c = a - 48x, or a = c + 48x. This is the same as c = a % 48.
				//48x <= c + 48x < 48x+48.   c + 48x - b < 48x.             remove 48x from the equation.
				//0 <= c < 48.               c -b < 0.                      solve for b.  
				//c < b. THEREFORE: (a%48) < b

				//however, we first need to handle cases where we need to proc more than once. this only occurs if hoursPassed > 48. Should never happen, but whatever.
				if (hoursPassed > 48)
				{
					int timesToRun = hoursPassed / 48;
					hoursPassed %= 48;
					boostLactation(-0.1f * timesToRun);
				}

				var check = hoursSinceLastMilked % 48;
				if (hoursPassed > check)
				{
					boostLactation(-0.1f);
				}
			}
			results = "";
			return false;
		}
			#endregion
#warning TODO:  a means to min/max lactationStatus?
	}
}
