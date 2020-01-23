using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CoC.Backend.BodyParts
{
	public sealed partial class CockCollection : BodyParts.SimpleSaveablePart<CockCollection, CockCollectionData>
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

		#region Cum Related Constants

		public const ushort CUM_MULTIPLIER_CAP = 20000;

		#endregion

		#region Public Cock Related Members

		public readonly ReadOnlyCollection<Cock> cocks;
		public Cock this[int index]
		{
			get => _cocks[index];
		}
		#endregion

		#region Public Cum Related Members
		public float cumMultiplierTrue
		{
			get => _cumMultiplierTrue;
			private set => _cumMultiplierTrue = Utils.Clamp2(value, 1, CUM_MULTIPLIER_CAP);
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

		private readonly List<Cock> _cocks = new List<Cock>(MAX_COCKS);

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
		public bool hasCock => _cocks.Count > 0;

		public int numCocks => _cocks.Count;

		public uint cockSoundedCount => missingCockSoundCount + (uint)cocks.Sum(x => x.soundCount);

		public uint cockSexCount => missingCockSexCount + (uint)cocks.Sum(x => x.sexCount);

		public uint cockOrgasmCount => missingCockOrgasmCount.add((uint)cocks.Sum(x => x.orgasmCount));

		public uint cockDryOrgasmCount => missingCockDryOrgasmCount.add((uint)cocks.Sum(x => x.dryOrgasmCount));

		public bool cockVirgin => missingCockSexCount > 0 ? false : cockSexCount == 0; //the first one means no aggregate calculation, for efficiency.


		public bool hasSheath => _cocks.Any(x => x.requiresASheath);

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
				if (hoursSinceLastCum < 12 && !perkData.alwaysProducesMaxCum) //i'd do 24 but this is Mareth, so.
				{
					int hoursOffset = hoursSinceLastCum;
					if (hoursSinceLastCum <= 0) //0 is possible and likely valid, but below 0 means we broke shit. this is just a catch-all.
					{
						hoursOffset = 1;
					}
					multiplier *= hoursOffset / 12.0;
				}
				multiplier *= perkData.bonusCumMultiplier;
				baseValue = baseValue * multiplier + perkData.bonusCumAdded;

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

		private Genitals source;

		private GenitalPerkData perkData => source.perkData;

		internal Balls balls => source.balls;

		public Gender gender => source.gender;

		private Creature creature => CreatureStore.GetCreatureClean(creatureID);

		#region Constructor
		internal CockCollection(Genitals parent) : base(parent?.creatureID ?? throw new ArgumentNullException(nameof(parent)))
		{
			source = parent;

			cocks = new ReadOnlyCollection<Cock>(_cocks);
		}
		#endregion

		#region Simple Part Data
		public override CockCollectionData AsReadOnlyData()
		{
			return new CockCollectionData(this);
		}

		public override string BodyPartName()
		{
			return Name();
		}

		internal override bool Validate(bool correctInvalidData)
		{
			bool valid = true;
			if (_cocks.Count > MAX_COCKS)
			{
				if (!correctInvalidData)
				{
					return false;
				}

				valid = false;
				_cocks.RemoveRange(MAX_COCKS, _cocks.Count - MAX_COCKS);
			}

			foreach (var breast in _cocks)
			{
				valid |= breast.Validate(correctInvalidData);
				if (!valid && !correctInvalidData)
				{
					return false;
				}
			}


			return valid;
		}
		#endregion

		internal void Initialize(CockCreator[] cockCreators)
		{
			_cocks.AddRange(cockCreators.Where(x => x != null).Select(x => new Cock(creatureID,x.type, x.validLength, x.validGirth,
				x.knot, x.cockSock, x.piercings)).Take(MAX_COCKS));
		}

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

		public bool OtherCocksUseSheath(int excludedCockIndex)
		{
			return _cocks.Any(x => x.cockIndex != excludedCockIndex && x.requiresASheath);
		}

		public bool LostSheath(CockData previousCockData)
		{
			return previousCockData.currentlyHasSheath && !hasSheath;
		}

		public bool GainedSheath(CockData previousCockData)
		{
			return !previousCockData.currentlyHasSheath && hasSheath;
		}

		public bool HasSheathChanged(CockData previousCockData)
		{
			return previousCockData.currentlyHasSheath != hasSheath;
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

			_cocks.Add(new Cock(creatureID, newCockType));

			source.CheckGenderChanged(oldGender);
			return true;
		}

		public bool AddCock(CockType newCockType, float length, float girth, float? knotMultiplier = null)
		{
			if (numCocks >= MAX_COCKS)
			{
				return false;
			}
			var oldGender = gender;

			_cocks.Add(new Cock(creatureID, newCockType, length, girth, knotMultiplier));

			source.CheckGenderChanged(oldGender);
			return true;
		}

		public string AddedCockText(CockData addedCock)
		{
			int count = CountCocksOfType(addedCock.type);
			if (count == 0 || numCocks == 0) return "";

			if (creature is PlayerBase player)
			{
				return addedCock.type.GrewCockText(player, (byte)(numCocks - 1));
			}
			else
			{
				return "";
			}
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
				this.missingCockSexCount += (uint)cocks.Skip(numCocks - count).Sum(x => x.sexCount);
				this.missingCockSoundCount += (uint)cocks.Skip(numCocks - count).Sum(x => x.soundCount);
				this.missingCockOrgasmCount += (uint)cocks.Skip(numCocks - count).Sum(x => x.orgasmCount);
				this.missingCockDryOrgasmCount += (uint)cocks.Skip(numCocks - count).Sum(x => x.dryOrgasmCount);

				_cocks.RemoveRange(numCocks - count, count);
			}

			source.CheckGenderChanged(oldGender);
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

		public bool RestoreCock(int index)
		{
			return _cocks[index].Restore();
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
						cock.IncreaseThickness(0.5f);
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
			cocks[cockIndex].SoundCock(penetratorLength, penetratorWidth, knotSize, reachOrgasm);
			if (reachOrgasm)
			{
				timeLastCum = GameDateTime.Now;
			}
		}

		internal void HandleCockPenetrate(int cockIndex, bool reachOrgasm)
		{
			cocks[cockIndex].DoSex(reachOrgasm);
			if (reachOrgasm)
			{
				timeLastCum = GameDateTime.Now;
			}
		}

		internal void DoCockOrgasmGeneric(int cockIndex, bool dryOrgasm)
		{
			cocks[cockIndex].OrgasmGeneric(dryOrgasm);
			timeLastCum = GameDateTime.Now;
		}

		#endregion

		#region Cock Text
		public string SheathOrBaseStr() => CockCollectionStrings.SheathOrBaseStr(this);


		public string AllCocksShortDescription() => CockCollectionStrings.AllCocksShortDescription(this);


		public string AllCocksLongDescription() => CockCollectionStrings.AllCocksLongDescription(this);


		public string AllCocksFullDescription() => CockCollectionStrings.AllCocksFullDescription(this);


		public string OneCockOrCocksNoun(string pronoun = "your") => CockCollectionStrings.OneCockOrCocksNoun(this, pronoun);


		public string OneCockOrCocksShort(string pronoun = "your") => CockCollectionStrings.OneCockOrCocksShort(this, pronoun);


		public string EachCockOrCocksNoun(string pronoun = "your") => CockCollectionStrings.EachCockOrCocksNoun(this, pronoun);


		public string EachCockOrCocksShort(string pronoun = "your") => CockCollectionStrings.EachCockOrCocksShort(this, pronoun);


		public string EachCockOrCocksNoun(string pronoun, out bool isPlural) => CockCollectionStrings.EachCockOrCocksNoun(this, pronoun, out isPlural);


		public string EachCockOrCocksShort(string pronoun, out bool isPlural) => CockCollectionStrings.EachCockOrCocksShort(this, pronoun, out isPlural);

		#endregion
		internal string AllCocksPlayerDescription()
		{
			if (creature is PlayerBase player)
			{
				return AllCocksPlayerDesc(player);
			}
			else
			{
				return "";
			}
		}
	}

	public sealed partial class CockCollectionData : SimpleData, ICockCollection<CockData>
	{
		public readonly float cumMultiplierTrue;

		public float additionalCumTrue;

		ReadOnlyCollection<CockData> ICockCollection<CockData>.cocks => cocks;

		public readonly ReadOnlyCollection<CockData> cocks;

		public CockData this[int index]
		{
			get => cocks[index];
		}

		public readonly BallsData balls;

		#region Public Cock Computed Values
		public readonly int numCocks;

		public bool hasCock => numCocks > 0;

		public readonly uint cockSoundedCount;

		public readonly uint cockSexCount;

		public readonly uint cockOrgasmCount;

		public readonly uint cockDryOrgasmCount;

		public bool cockVirgin => cockSexCount == 0;


		public bool hasSheath => cocks.Any(x => x.requiresSheath);

		#endregion

		#region Public Balls Computed Values
		public bool hasBalls => balls.hasBalls;
		public bool uniBall => balls.uniBall;

		public byte numberOfBalls => balls.count;
		public byte ballSize => balls.size;
		#endregion

		#region Public Cum Related Computed Values
		public ushort cumMultiplier => (ushort)Math.Round(cumMultiplierTrue); //0-65535 seems like a valid range imo. i don't think i need to cap it.

		public ushort additionalCum => (ushort)additionalCumTrue;

		public readonly int hoursSinceLastCum;

		//we use mL for amount here, but store ball size in inches. Let's do it right, no? 1cm^3 = 1mL
		public readonly int totalCum;

		public CockCollectionData(CockCollection source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			this.balls = source.balls.AsReadOnlyData();

			this.cumMultiplierTrue = source.cumMultiplierTrue;
			this.additionalCumTrue = source.additionalCumTrue;
			this.cocks = new ReadOnlyCollection<CockData>(source.cocks.Select(x=>x.AsReadOnlyData()).ToList());
			this.numCocks = source.numCocks;
			//this.anyCockSoundedCount = source.anyCockSoundedCount;
			this.cockSoundedCount = source.cockSoundedCount;
			this.cockSexCount = source.cockSexCount;
			//this.anyCockSexCount = source.anyCockSexCount;
			this.cockOrgasmCount = source.cockOrgasmCount;
			//this.anyCockOrgasmCount = source.anyCockOrgasmCount;
			this.cockDryOrgasmCount = source.cockDryOrgasmCount;
			//this.anyCockDryOrgasmCount = source.anyCockDryOrgasmCount;
			this.hoursSinceLastCum = source.hoursSinceLastCum;
			this.totalCum = source.totalCum;


		}
		#endregion

		#region CockData Aggregate Functions
		public float BiggestCockSize()
		{
			return cocks.Max(x => x.area);
		}

		public float LongestCockLength()
		{
			return cocks.Max(x => x.length);
		}

		public float WidestCockMeasure()
		{
			return cocks.Max(x => x.girth);
		}

		public CockData BiggestCock()
		{
			return cocks.MaxItem(x => x.area);
		}

		public CockData LongestCock()
		{
			return cocks.MaxItem(x => x.length);
		}

		public CockData WidestCock()
		{
			return cocks.MaxItem(x => x.girth);
		}

		public float AverageCockSize()
		{
			return cocks.Average(x => x.area);
		}

		public float AverageCockLength()
		{
			return cocks.Average(x => x.length);
		}

		public float AverageCockGirth()
		{
			return cocks.Average(x => x.girth);
		}

		public CockData AverageCock()
		{
			if (cocks.Count == 0)
			{
				return null;
			}
			var averageLength = cocks.Average(x => x.length);
			var averageGirth = cocks.Average(x => x.girth);
			var averageKnot = cocks.Average(x => x.knotMultiplier);
			var averageKnotSize = cocks.Average(x => x.knotSize);
			//first initially gets the first group. the second call to first gets the first element of the first group.
			CockType type = cocks.GroupBy(x => x.type).OrderByDescending(y => y.Count()).First().First().type;
			return Cock.GenerateAggregate(creatureID, type, averageKnot, averageKnotSize, averageLength, averageGirth);
		}

		public float SmallestCockSize()
		{
			return cocks.Min(x => x.area);
		}

		public float ShortestCockLength()
		{
			return cocks.Min(x => x.length);
		}

		public float ThinnestCockMeasure()
		{
			return cocks.Min(x => x.girth);
		}

		public CockData SmallestCock()
		{
			return cocks.MinItem(x => x.area);
		}

		public CockData ShortestCock()
		{
			return cocks.MinItem(x => x.length);
		}

		public CockData ThinnestCock()
		{
			return cocks.MinItem(x => x.girth);
		}

		public int CountCocksOfType(CockType type)
		{
			return cocks.Sum(x => x.type == type ? 1 : 0);
		}

		public bool OtherCocksUseSheath(int excludedCockIndex)
		{
			return cocks.Any(x => x.cockIndex != excludedCockIndex && x.requiresSheath);
		}


		#endregion

		#region Cock Text
		public string SheathOrBaseStr() => CockCollectionStrings.SheathOrBaseStr(this);


		public string AllCocksShortDescription() => CockCollectionStrings.AllCocksShortDescription(this);


		public string AllCocksLongDescription() => CockCollectionStrings.AllCocksLongDescription(this);


		public string AllCocksFullDescription() => CockCollectionStrings.AllCocksFullDescription(this);


		public string OneCockOrCocksNoun(string pronoun = "your") => CockCollectionStrings.OneCockOrCocksNoun(this, pronoun);


		public string OneCockOrCocksShort(string pronoun = "your") => CockCollectionStrings.OneCockOrCocksShort(this, pronoun);


		public string EachCockOrCocksNoun(string pronoun = "your") => CockCollectionStrings.EachCockOrCocksNoun(this, pronoun);


		public string EachCockOrCocksShort(string pronoun = "your") => CockCollectionStrings.EachCockOrCocksShort(this, pronoun);


		public string EachCockOrCocksNoun(string pronoun, out bool isPlural) => CockCollectionStrings.EachCockOrCocksNoun(this, pronoun, out isPlural);


		public string EachCockOrCocksShort(string pronoun, out bool isPlural) => CockCollectionStrings.EachCockOrCocksShort(this, pronoun, out isPlural);
		#endregion

	}
}
