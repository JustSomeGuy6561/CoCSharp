using CoC.Backend.Engine.Time;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class Genitals
	{
		#region Notes:

		//Note on cocks: the computed values have different versions, one including clit-cock data, one not. I don't really know if that distinction is important or not,
		//so i've just said fuck it and kept both. 

		//This does not apply to the cock aggregate functions - the clit-cock only comes into play when the target does not have any normal cocks; it acts like a normal clit when the 
		//creature also has a normal cock, so it makes no sense to combine them. 

		//Cum has been updated: the formula has been altered to allow for items to either update a multiplier or a flat added value, instead of just a multiplier. the formula for 
		//total cum amount has also been overhauled; it now uses the ball size and count with physics (GASP!). Ok, rough physics (PHEW), with proper fudging to make the calculations simpler.
		//(An aside, if you have a hard-on for physics, just think of the multiplier as density, and somehow the cum is more dense than titanium or some shit, idk). 

		//By default, the add function in balls will add the given number of balls, regardless of whether or not the player has balls. specific functions have been added to provide a more
		//consistent behavior, only adding them if the player does or does not already have balls, depending on the function. 
		#endregion

		#region Cock Related Constants
		public const int MAX_COCKS = 10;
		#endregion

		#region Public Cum Related Members
		public float cumMultiplierTrue
		{
			get => _cumMultiplierTrue;
			private set => _cumMultiplierTrue = Utils.Clamp2(value, 1, ushort.MaxValue);
		}
		private float _cumMultiplierTrue = 1;

		public ushort additionalCum => (ushort)additionalCumTrue;

		public float additionalCumTrue
		{
			get => _additionalCumTrue;
			private set => _additionalCumTrue = Utils.Clamp2(value, ushort.MinValue, ushort.MaxValue);
		}
		private float _additionalCumTrue = 0;
		#endregion

		#region Private Cock Related Members
		//the number of times had sex with cocks that no longer exist;
		private uint missingCockSexCount;
		//number of times had cock sounded for cocks that no longer exist.
		private uint missingCockSoundCount;
		//times cock orgasmed for missing cocks.
		private uint missingCockOrgasmCount;
		//times cock orgasmed without any stimulation for missing cocks.
		private uint missingCockDryOrgasmCount;
		#endregion

		#region Private Cum Related Members
		private GameDateTime timeLastCum { get; set; }

		#endregion

		#region Public Cock Computed Values
		public int numCocks => _cocks.Count + (hasClitCock ? _vaginas.Count : 0);

		public uint anyCockSoundedCount => maleCockSoundedCount.add(clitCockSoundedCount);
		public uint maleCockSoundedCount => missingCockSoundCount + (uint)cocks.Sum(x => x.soundCount);

		public uint maleCockSexCount => missingCockSexCount + (uint)cocks.Sum(x => x.sexCount);
		public uint anyCockSexCount => maleCockSexCount.add(clitCockSexCount);

		public uint maleCockOrgasmCount => missingCockOrgasmCount.add((uint)cocks.Sum(x => x.orgasmCount));
		public uint anyCockOrgasmCount => maleCockOrgasmCount.add(clitCockOrgasmCount);

		public uint maleCockDryOrgasmCount => missingCockDryOrgasmCount.add((uint)cocks.Sum(x => x.dryOrgasmCount));
		public uint anyCockDryOrgasmCount => maleCockDryOrgasmCount.add(clitCockDryOrgasmCount);

		public bool cockVirgin => missingCockSexCount > 0 || missingClitCockSexCount > 0 ? false : anyCockSexCount == 0; //the first one means no aggregate calculation, for efficiency.
		public bool maleCockVirgin => missingCockSexCount > 0 ? false : maleCockSexCount == 0; //the first one means no aggregate calculation, for efficiency.
		#endregion

		#region Public Balls Computed Values
		public bool hasBalls => balls.hasBalls;
		public bool uniBall => balls.uniBall;

		public byte numberOfBalls => balls.count;
		public byte ballSize => balls.size;
		#endregion

		#region Public Cum Related Computed Values
		public ushort cumMultiplier => (ushort)Math.Round(cumMultiplierTrue); //0-65535 seems like a valid range imo. i don't think i need to cap it. 

		public int hoursSinceLastCum => timeLastCum.hoursToNow();

		//we use mL for amount here, but store ball size in inches. Let's do it right, no? 1cm^3 = 1mL
		public int totalCum
		{
			get
			{
				if (numCocks == 0)
				{
					return 0;
				}
				double baseValue = additionalCum;
				//each cock will add a small amount

				if (balls.hasBalls)
				{
					double ballSizeCm = balls.size * Measurement.TO_CENTIMETERS;
					baseValue += 4.0 / 3 * Math.PI * Math.Pow(ballSizeCm / 2, 3) * balls.count * cumMultiplier;
				}

				double multiplier = 1;
				if (!balls.hasBalls)
				{
					multiplier = 0.25;
				}
				if (hoursSinceLastCum < 12 && !alwaysProducesMaxCum) //i'd do 24 but this is Mareth, so.
				{
					int hoursOffset = hoursSinceLastCum;
					if (hoursSinceLastCum <= 0) //0 is possible and likely valid, but below 0 means we broke shit. this is just a catch-all. 
					{
						hoursOffset = 1;
					}
					multiplier *= hoursOffset / 12.0;
				}
				multiplier *= bonusCumMultiplier;
				baseValue = baseValue * multiplier + bonusCumAdded;

				if (baseValue > int.MaxValue)
				{
					return int.MaxValue;
				}

				//at absolute min, 2units per cock. some large cocks may increase this minimum further. this just makes the sex related functions easier to handle for multiple cocks being used at once
				double minimumValue = cocks.Sum(x => x.minCumAmount);

				if (baseValue < minimumValue)
				{
					baseValue = minimumValue;
				}
				return (int)Math.Floor(baseValue);
			}
		}

		#endregion

		#region Cock Aggregate Functions
		public float BiggestCockSize()
		{
			return _cocks.Max(x => x.area);
		}

		public float LongestCockLength()
		{
			return _cocks.Max(x => x.length);
		}

		public float WidestCockMeasure()
		{
			return _cocks.Max(x => x.girth);
		}

		public Cock BiggestCock()
		{
			return _cocks.MaxItem(x => x.area);
		}

		public Cock LongestCock()
		{
			return _cocks.MaxItem(x => x.length);
		}

		public Cock WidestCock()
		{
			return _cocks.MaxItem(x => x.girth);
		}

		public float AverageCockSize()
		{
			return _cocks.Average(x => x.area);
		}

		public float AverageCockLength()
		{
			return _cocks.Average(x => x.length);
		}

		public float AverageCockGirth()
		{
			return _cocks.Average(x => x.girth);
		}

		public CockData AverageCock()
		{
			if (_cocks.Count == 0)
			{
				return null;
			}
			var averageLength = _cocks.Average(x => x.length);
			var averageGirth = cocks.Average(x => x.girth);
			var averageKnot = cocks.Average(x => x.knotMultiplier);
			var averageKnotSize = cocks.Average(x => x.knotSize);
			//first initially gets the first group. the second call to first gets the first element of the first group.
			CockType type = _cocks.GroupBy(x => x.type).OrderByDescending(y => y.Count()).First().First().type; 
			return Cock.GenerateAggregate(creatureID, type, averageKnot, averageKnotSize, averageLength, averageGirth);
		}

		public float SmallestCockSize()
		{
			return _cocks.Min(x => x.area);
		}

		public float ShortestCockLength()
		{
			return _cocks.Min(x => x.length);
		}

		public float ThinnestCockMeasure()
		{
			return _cocks.Min(x => x.girth);
		}

		public Cock SmallestCock()
		{
			return _cocks.MinItem(x => x.area);
		}

		public Cock ShortestCock()
		{
			return _cocks.MinItem(x => x.length);
		}

		public Cock ThinnestCock()
		{
			return _cocks.MinItem(x => x.girth);
		}

		public int CountCocksOfType(CockType type)
		{
			return _cocks.Sum(x => x.type == type ? 1 : 0);
		}
		#endregion

		#region Add/Remove Cocks
		public bool AddCock(CockType newCockType)
		{
			if (numCocks == MAX_COCKS)
			{
				return false;
			}
			var oldGender = gender;

			_cocks.Add(new Cock(creatureID, GetCockPerkWrapper(), newCockType));

			CheckGenderChanged(oldGender);
			return true;
		}

		public bool AddCock(CockType newCockType, float length, float girth, float? knotMultiplier = null)
		{
			if (numCocks >= MAX_COCKS)
			{
				return false;
			}
			var oldGender = gender;

			_cocks.Add(new Cock(creatureID, GetCockPerkWrapper(), newCockType, length, girth, knotMultiplier));

			CheckGenderChanged(oldGender);
			return true;
		}

		public int RemoveCock(int count = 1)
		{
			if (numCocks == 0 || count <= 0)
			{
				return 0;
			}
			int oldCount = numCocks;
			var oldGender = gender;

			if (count > numCocks)
			{
				this.missingCockSexCount += (uint)cocks.Sum(x => x.sexCount);
				this.missingCockSoundCount += (uint)cocks.Sum(x => x.soundCount);
				missingCockOrgasmCount += (uint)cocks.Sum(x => x.orgasmCount);
				missingCockDryOrgasmCount += (uint)cocks.Sum(x => x.dryOrgasmCount);
				_cocks.Clear();
			}
			else
			{
				this.missingCockSexCount += (uint)cocks.Skip(numCocks-count).Sum(x => x.sexCount);
				this.missingCockSoundCount += (uint)cocks.Skip(numCocks - count).Sum(x => x.soundCount);
				this.missingCockOrgasmCount += (uint)cocks.Skip(numCocks - count).Sum(x => x.orgasmCount);
				this.missingCockDryOrgasmCount += (uint)cocks.Skip(numCocks - count).Sum(x => x.dryOrgasmCount);

				_cocks.RemoveRange(numCocks - count, count);
			}

			CheckGenderChanged(oldGender);
			return oldCount - numCocks;
		}

		public int RemoveExtraCocks()
		{
			return RemoveCock(numCocks - 1);
		}

		public int RemoveAllCocks()
		{
			return RemoveCock(numCocks);
		}
		#endregion
		
		#region Add/Remove Balls

		/// <summary>
		/// Tries to grow a pair of balls, failing if the creature already has balls. 
		/// </summary>
		/// <returns>True if the creature gained a pair of balls, false if they already had them.</returns>
		public bool GrowBalls()
		{
			return balls.growBalls();
		}

		/// <summary>
		/// Tries to grow the number of balls provided, rounded down to the nearest even number, at the given size, if applicable. This will create a minimum of two balls if successful.
		/// Fails if the target already has balls of any kind. 
		/// </summary>
		/// <param name="numberOfBalls"></param>
		/// <param name="ballSize"></param>
		/// <returns>true if the target didn't previously have balls and now does, false otherwise.</returns>
		/// <remarks>this function will never create a Uniball. If this is desired, use either GrowUniBall or GrowBallsAny.</remarks>
		public bool GrowBalls(byte numberOfBalls, byte ballSize = Balls.DEFAULT_BALLS_SIZE)
		{
			return balls.growBalls(numberOfBalls, ballSize);
		}

		/// <summary>
		/// Tries to grow a uniball. Fails if the target already has balls.
		/// </summary>
		/// <returns>True if the target did not have balls and now has a uniball, false otherwise.</returns>
		public bool GrowUniBall()
		{
			return balls.growUniBall();
		}

		/// <summary>
		/// Tries to grow the number of balls provided, at the given size, if applicable and possible. 
		/// </summary>
		/// <param name="numBalls"></param>
		/// <param name="newSize"></param>
		/// <returns></returns>
		/// <remarks>Any odd number of balls provided that is not 1 will be rounded down to the nearest even number. </remarks>
		public bool GrowBallsAny(byte numBalls, byte newSize = Balls.DEFAULT_BALLS_SIZE)
		{
			if (numBalls == 1)
			{
				return balls.growUniBall();
			}
			else
			{
				return balls.growBalls(numBalls, newSize);
			}
		}

		/// <summary>
		/// Grows the given amount of balls, even if the target currently does not have any balls. if they do, the two are added, then rounded down to the nearest even number. 
		/// The total amount grown is returned.
		/// </summary>
		/// <param name="ballsToAdd">The number of balls to add.</param>
		/// <returns>The number of balls successfully added.</returns>
		/// <remarks> The number of balls a target can have is capped; The return value will differ from the given value if this cap is reached.</remarks>
		public byte AddBalls(byte ballsToAdd, bool ignoreIfUniball = false)
		{
			if (balls.uniBall && !ignoreIfUniball)
			{
				return 0;
			}
			else
			{
				return balls.AddBalls(ballsToAdd);
			}
		}

		/// <summary>
		/// Adds the given total of balls to the current amount. If the target does not have balls or the target has a uniball and the optional ignore if uniball flag is set, 
		/// this will fail to add any balls. Returns the number of balls added. 
		/// </summary>
		/// <param name="additionalBalls">Number of balls to add.</param>
		/// <param name="ignoreIfUniball">Should this function respect a uniball, if applicable?</param>
		/// <returns>The number of balls successfully added.</returns>
		/// <remarks> The number of balls a target can have is capped; The return value will differ from the given value if this cap is reached.</remarks>
		public byte AddAdditionalBalls(byte additionalBalls, bool ignoreIfUniball = false)
		{
			if (!hasBalls || (balls.uniBall && !ignoreIfUniball))
			{
				return 0;
			}
			else
			{
				return balls.AddBalls(additionalBalls);
			}
		}

		public byte RemoveBalls(byte removeAmount)
		{
			return balls.RemoveBalls(removeAmount);
		}

		public byte RemoveExtraBalls()
		{
			return balls.RemoveExtraBalls();
		}

		public bool RemoveAllBalls()
		{
			return balls.removeAllBalls();
		}
		#endregion

		#region Update Cock Type

		public bool UpdateCock(int index, CockType newType)
		{
			return _cocks[index].UpdateType(newType);
		}

		public bool UpdateCockWithLength(int index, CockType newType, float newLength)
		{
			return _cocks[index].UpdateCockTypeWithLength(newType, newLength);
		}

		public bool UpdateCockWithLengthAndGirth(int index, CockType newType, float newLength, float newGirth)
		{
			return _cocks[index].UpdateCockTypeWithLengthAndGirth(newType, newLength, newGirth);
		}

		public bool UpdateCockWithKnot(int index, CockType newType, float newKnotMultiplier)
		{
			return _cocks[index].UpdateCockTypeWithKnotMultiplier(newType, newKnotMultiplier);
		}

		public bool UpdateCockWithAll(int index, CockType newType, float newLength, float newGirth, float newKnotMultiplier)
		{
			return _cocks[index].UpdateCockTypeWithAll(newType, newLength, newGirth, newKnotMultiplier);
		}

		#endregion

		#region Convert Ball Type
		public bool ConvertToNormalBalls()
		{
			return balls.makeStandard();
		}

		public bool ConvertToUniball()
		{
			return balls.makeUniBall();
		}

		#endregion
		
		#region Grow or Convert Balls
		public bool GrowOrConvertToUniball()
		{
			if (hasBalls)
			{
				return ConvertToUniball();
			}
			else
			{
				return GrowUniBall();
			}
		}

		public bool GrowOrConvertToNormalBalls()
		{
			if (hasBalls)
			{
				return ConvertToNormalBalls();
			}
			else
			{
				return GrowBalls();
			}
		}
		#endregion
		
		#region AllCocks Update Functions
		public void NormalizeDicks(bool untilEven = false)
		{
			if (numCocks == 1)
			{
				return;
			}
			float avgLength = AverageCockLength();
			float avgGirth = AverageCockGirth();
			if (untilEven)
			{
				foreach (var cock in _cocks)
				{
					cock.SetLengthAndGirth(avgLength, avgGirth);
				}
			}
			else
			{
				foreach (var cock in _cocks)
				{
					if (cock.girth < avgGirth - 0.5f)
					{
						cock.ThickenCock(0.5f);
					}
					else if (cock.girth > avgGirth + 0.5f)
					{
						cock.ThinCock(0.5f);
					}
					else
					{
						cock.SetGirth(avgGirth);
					}

					if (cock.length < avgLength - 1)
					{
						cock.LengthenCock(1);
					}
					else if (cock.length > avgLength + 1)
					{
						cock.ShortenCock(1);
					}
					else
					{
						cock.SetLength(avgLength);
					}
				}
			}
		}
		#endregion

		#region Change Balls Data

		public byte EnlargeBalls(byte enlargeAmount, bool respectUniball = false, bool ignorePerks = false)
		{
			if (!hasBalls || (balls.uniBall && respectUniball))
			{
				return 0;
			}
			else
			{
				return balls.EnlargeBalls(enlargeAmount, ignorePerks);
			}
		}

		public byte ShrinkBalls(byte shrinkAmount, bool ignorePerks = false)
		{
			if (!hasBalls)
			{
				return 0;
			}
			else
			{
				return balls.ShrinkBalls(shrinkAmount, ignorePerks);
			}
		}

		#endregion

		#region Cum Update Functions
		public float IncreaseCumMultiplier(float additionalMultiplier)
		{
			var oldValue = cumMultiplierTrue;
			cumMultiplierTrue += additionalMultiplier;
			return cumMultiplierTrue - oldValue;
		}

		public float AddFlatCumAmount(float additionalCum)
		{
			var oldValue = additionalCumTrue;
			additionalCumTrue += additionalCum;
			return additionalCumTrue - oldValue;
		}
		#endregion

		#region Cock Sex Related Functions

		internal void HandleCockSounding(int cockIndex, float penetratorLength, float penetratorWidth, float knotSize, float cumAmount, bool reachOrgasm)
		{
			_cocks[cockIndex].SoundCock(penetratorLength, penetratorWidth, knotSize, reachOrgasm);
			if (reachOrgasm)
			{
				timeLastCum = GameDateTime.Now;
			}
		}

		internal void HandleCockSounding(int cockIndex, Cock sourceCock, bool reachOrgasm)
		{
			HandleCockSounding(cockIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, sourceCock.cumAmount, reachOrgasm);
		}

		internal void HandleCockSounding(int cockIndex, Cock sourceCock, float cumAmountOverride, bool reachOrgasm)
		{
			HandleCockSounding(cockIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, cumAmountOverride, reachOrgasm);
		}

		internal void HandleCockPenetrate(int cockIndex, bool reachOrgasm)
		{
			_cocks[cockIndex].DoSex(reachOrgasm);
			if (reachOrgasm)
			{
				timeLastCum = GameDateTime.Now;
			}
		}

		internal void DoCockOrgasmGeneric(int cockIndex, bool dryOrgasm)
		{
			_cocks[cockIndex].OrgasmGeneric(dryOrgasm);
			timeLastCum = GameDateTime.Now;
		}

		#endregion
	}
}
