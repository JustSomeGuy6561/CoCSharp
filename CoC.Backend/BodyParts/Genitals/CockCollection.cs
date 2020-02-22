using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

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

		public double minimumCockLength => source.perkData.MinCockLength;

		#endregion

		#region Public Cum Related Members
		public double cumMultiplierTrue
		{
			get => _cumMultiplierTrue;
			private set => _cumMultiplierTrue = Utils.Clamp2(value, 1, CUM_MULTIPLIER_CAP);
		}
		private double _cumMultiplierTrue = 1;

		public ushort additionalCum => (ushort)additionalCumTrue;

		public double additionalCumTrue
		{
			get => _additionalCumTrue;
			private set => _additionalCumTrue = Utils.Clamp2(value, ushort.MinValue, ushort.MaxValue);
		}
		private double _additionalCumTrue = 0;

		public uint pentUpBonus
		{
			get => _pentUpBonus;
			private set
			{
				_pentUpBonus = Utils.Clamp2<uint>(value, 0, int.MaxValue);
			}
		}
		private uint _pentUpBonus = 0;
		#endregion

		#region Private Cock Related Members

		private readonly List<Cock> _cocks = new List<Cock>(MAX_COCKS);

		//the number of times had sex with cocks that no longer exist;
		private uint missingCockSexCount;
		//the number of times had sex with cocks that no longer exist, with themself
		private uint missingCockSelfSexCount;
		//number of times had cock sounded for cocks that no longer exist.
		private uint missingCockSoundCount;
		//times cock orgasmed for missing cocks.
		private uint missingCockOrgasmCount;
		//times cock orgasmed without any stimulation for missing cocks.
		private uint missingCockDryOrgasmCount;
		#endregion

		#region Private Cum Related Members
		private GameDateTime timeLastCum
		{
			get => _timeLastCum;
			set
			{
				if (value > _timeLastCum)
				{
					pentUpBonus = 0;
				}
				_timeLastCum = value;
			}
		}
		private GameDateTime _timeLastCum;

		#endregion

		#region Public Cock Computed Values
		public bool hasCock => _cocks.Count > 0;

		public int numCocks => _cocks.Count;

		public uint totalSoundCount => missingCockSoundCount.add((uint)_cocks.Sum(x => x.soundCount));
		public uint totalSexCount => missingCockSexCount.add((uint)_cocks.Sum(x => x.totalSexCount));
		public uint selfSexCount => missingCockSelfSexCount.add((uint)_cocks.Sum(x => x.selfSexCount));

		public uint totalOrgasmCount => missingCockOrgasmCount.add((uint)_cocks.Sum(x => x.orgasmCount));
		public uint dryOrgasmCount => missingCockDryOrgasmCount.add((uint)_cocks.Sum(x => x.dryOrgasmCount));

		public bool cockVirgin => missingCockSexCount > 0 ? false : totalSexCount == 0; //the first one means no aggregate calculation, for efficiency.

		public bool hasSheath => _cocks.Any(x => x.requiresASheath);

		#endregion

		#region Public Cum Related Computed Values
		public ushort cumMultiplier => (ushort)Math.Round(cumMultiplierTrue); //0-65535 seems like a valid range imo. i don't think i need to cap it.

		public int hoursSinceLastCum => timeLastCum.hoursToNow();

		public int simulatedHoursSinceLastCum => (int)(hoursSinceLastCum + pentUpBonus);

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
				if (simulatedHoursSinceLastCum < 12 && !perkData.alwaysProducesMaxCum) //i'd do 24 but this is Mareth, so.
				{
					int hoursOffset = simulatedHoursSinceLastCum;
					if (simulatedHoursSinceLastCum <= 0) //0 is possible and likely valid, but below 0 means we broke shit. this is just a catch-all.
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

		#region Perk Data

		internal double cockGrowthMultiplier => perkData.CockGrowthMultiplier;
		internal double cockShrinkMultiplier => perkData.CockShrinkMultiplier;

		//perk values that alter the size of any new cock.
		internal double newCockDefaultSize => perkData.NewCockDefaultSize;
		internal double newCockSizeDelta => perkData.NewCockSizeDelta;

		//perk value that sets the absolute minimum size for any cock. this is given priority, even over new size perk data.
		public double minCockLength => perkData.MinCockLength;

		//perk values used to alter virility of a creature (and by extension, all of its cocks).
		internal double perkBonusVirilityMultiplier => perkData.perkBonusVirilityMultiplier;
		internal sbyte perkBonusVirility => perkData.perkBonusVirility;

		#endregion

		private Genitals source;
		private uint currentCockID = 0;
		private GenitalPerkData perkData => source.perkData;

		internal Balls balls => source.balls;

		public Gender gender => source.gender;

		private Creature creature => CreatureStore.GetCreatureClean(creatureID);

		internal double relativeLust => source.relativeLust;

		internal byte currentLust => creature?.lust ?? Creature.DEFAULT_LUST;

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

			foreach (Cock breast in _cocks)
			{
				valid |= breast.Validate(correctInvalidData);
				if (!valid && !correctInvalidData)
				{
					return false;
				}
			}


			return valid;
		}

		public override bool IsIdenticalTo(CockCollectionData original, bool ignoreSexualMetaData)
		{
			if (original is null)
			{
				return false;
			}

			return cumMultiplierTrue == original.cumMultiplierTrue && additionalCumTrue == original.additionalCumTrue && totalCum == original.totalCum
				&& hoursSinceLastCum == original.hoursSinceLastCum && simulatedHoursSinceLastCum == original.simulatedHoursSinceLastCum
				&& (ignoreSexualMetaData || (totalSoundCount == original.totalSoundCount && totalSexCount == original.totalSexCount
				&& selfSexCount == original.selfSexCount && totalOrgasmCount == original.totalOrgasmCount && dryOrgasmCount == original.dryOrgasmCount))
				&& !CollectionChanged(original, ignoreSexualMetaData);
		}

		#endregion

		public bool CollectionChanged(CockCollectionData original, bool ignoreSexualMetaData)
		{
			if (original is null)
			{
				return false;
			}

			Dictionary<uint, Cock> items = _cocks.ToDictionary(x => x.collectionID, x => x);
			Dictionary<uint, CockData> dataItems = original.cocks.ToDictionary(x => (uint)x.collectionID, x => x);

			return items.Keys.Count != dataItems.Keys.Count || items.Any(x => !dataItems.ContainsKey(x.Key) || !x.Value.IsIdenticalTo(dataItems[x.Key], ignoreSexualMetaData));
		}

		public IEnumerable<Cock> AddedCocks(CockCollectionData original)
		{
			Dictionary<uint, Cock> items = _cocks.ToDictionary(x => x.collectionID, x => x);
			Dictionary<uint, CockData> dataItems = original.cocks.ToDictionary(x => (uint)x.collectionID, x => x);

			return items.Where(x => !dataItems.ContainsKey(x.Key)).Select(x => x.Value);
		}

		public IEnumerable<CockData> RemovedCocks(CockCollectionData original)
		{
			Dictionary<uint, Cock> items = _cocks.ToDictionary(x => x.collectionID, x => x);
			Dictionary<uint, CockData> dataItems = original.cocks.ToDictionary(x => (uint)x.collectionID, x => x);

			return dataItems.Where(x => !items.ContainsKey(x.Key)).Select(x => x.Value);
		}

		public IEnumerable<ValueDifference<CockData>> ChangedCocks(CockCollectionData original, bool ignoreSexualMetaData)
		{
			Dictionary<uint, Cock> items = _cocks.ToDictionary(x => x.collectionID, x => x);
			Dictionary<uint, CockData> dataItems = original.cocks.ToDictionary(x => (uint)x.collectionID, x => x);

			if (original.creatureID != creatureID)
			{
				throw new ArgumentException("this collection is from a different source from the original data provided. Behavior is undefined.");
			}

			return items.Where(x => dataItems.ContainsKey(x.Key) && !x.Value.IsIdenticalTo(dataItems[x.Key], ignoreSexualMetaData))
				.Select(x => new ValueDifference<CockData>(dataItems[x.Key], x.Value.AsReadOnlyData()));
		}

		public IEnumerable<Cock> UnchangedCocks(CockCollectionData original, bool ignoreSexualMetaData)
		{
			Dictionary<uint, Cock> items = _cocks.ToDictionary(x => x.collectionID, x => x);
			Dictionary<uint, CockData> dataItems = original.cocks.ToDictionary(x => (uint)x.collectionID, x => x);

			return items.Where(x => dataItems.ContainsKey(x.Key) && x.Value.IsIdenticalTo(dataItems[x.Key], ignoreSexualMetaData)).Select(x => x.Value);
		}

		internal void Initialize(CockCreator[] cockCreators)
		{

			IEnumerable<CockCreator> cocks = cockCreators.Where(x => x != null).Take(MAX_COCKS);

			foreach (CockCreator cock in cocks)
			{
				_cocks.Add(new Cock(this, currentCockID, cock.type, cock.validLength, cock.validGirth, cock.knot, cock.cockSock, cock.piercings));
				currentCockID++;
			}

		}

		#region Cock Aggregate Functions
		public double BiggestCockTotalSize()
		{
			return _cocks.Max(x => x.area);
		}

		public double LongestCockLength()
		{
			return _cocks.Max(x => x.length);
		}

		public double WidestCockMeasure()
		{
			return _cocks.Max(x => x.girth);
		}

		public Cock BiggestCockByArea()
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

		public double AverageCockSize()
		{
			return _cocks.Average(x => x.area);
		}

		public double AverageCockLength()
		{
			return _cocks.Average(x => x.length);
		}

		public double AverageCockGirth()
		{
			return _cocks.Average(x => x.girth);
		}

		public CockData AverageCock()
		{
			if (_cocks.Count == 0)
			{
				return null;
			}
			double averageLength = _cocks.Average(x => x.length);
			double averageGirth = cocks.Average(x => x.girth);
			double averageKnot = cocks.Average(x => x.knotMultiplier);
			double averageKnotSize = cocks.Average(x => x.knotSize);
			//first initially gets the first group. the second call to first gets the first element of the first group.
			CockType type = _cocks.GroupBy(x => x.type).OrderByDescending(y => y.Count()).First().First().type;
			return Cock.GenerateAggregate(creatureID, type, averageKnot, averageKnotSize, averageLength, averageGirth, totalCum, hasSheath, currentLust, relativeLust);
		}

		public double SmallestCockTotalSize()
		{
			return _cocks.Min(x => x.area);
		}

		public double ShortestCockLength()
		{
			return _cocks.Min(x => x.length);
		}

		public double ThinnestCockMeasure()
		{
			return _cocks.Min(x => x.girth);
		}

		public Cock SmallestCockByArea()
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

		public bool HasACockOfType(CockType type)
		{
			return _cocks.Any(x => x.type == type);
		}

		public bool OnlyHasCocksOfType(CockType type)
		{
			return _cocks.All(x => x.type == type);
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
		public bool AddCock()
		{
			if (numCocks == MAX_COCKS)
			{
				return false;
			}
			Gender oldGender = gender;

			_cocks.Add(new Cock(this, currentCockID));
			currentCockID++;

			source.CheckGenderChanged(oldGender);
			return true;
		}

		public bool AddCock(CockType newCockType)
		{
			if (numCocks == MAX_COCKS)
			{
				return false;
			}
			Gender oldGender = gender;

			_cocks.Add(new Cock(this, currentCockID, newCockType));
			currentCockID++;

			source.CheckGenderChanged(oldGender);
			return true;
		}

		public bool AddCock(CockType newCockType, double length, double girth, double? knotMultiplier = null)
		{
			if (numCocks >= MAX_COCKS)
			{
				return false;
			}
			Gender oldGender = gender;

			_cocks.Add(new Cock(this, currentCockID, newCockType, length, girth, knotMultiplier));
			currentCockID++;

			source.CheckGenderChanged(oldGender);
			return true;
		}

		public string AddedCockText(CockData addedCock)
		{
			int count = CountCocksOfType(addedCock.type);
			if (count <= 0 || numCocks == 0)
			{
				return "";
			}

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
			Gender oldGender = gender;

			if (count > numCocks)
			{
				missingCockSexCount += (uint)cocks.Sum(x => x.totalSexCount);
				missingCockSelfSexCount += (uint)cocks.Sum(x => x.selfSexCount);
				missingCockSoundCount += (uint)cocks.Sum(x => x.soundCount);
				missingCockOrgasmCount += (uint)cocks.Sum(x => x.orgasmCount);
				missingCockDryOrgasmCount += (uint)cocks.Sum(x => x.dryOrgasmCount);
				_cocks.Clear();
			}
			else
			{
				IEnumerable<Cock> toRemove = cocks.Skip(numCocks - count);

				missingCockSexCount += (uint)toRemove.Sum(x => x.totalSexCount);
				missingCockSelfSexCount += (uint)toRemove.Sum(x => x.selfSexCount);
				missingCockSoundCount += (uint)toRemove.Sum(x => x.soundCount);
				missingCockOrgasmCount += (uint)toRemove.Sum(x => x.orgasmCount);
				missingCockDryOrgasmCount += (uint)toRemove.Sum(x => x.dryOrgasmCount);

				_cocks.RemoveRange(numCocks - count, count);
			}

			source.CheckGenderChanged(oldGender);
			return oldCount - numCocks;
		}

		public int RemoveCockAt(int index, int count = 1)
		{
			if (numCocks == 0 || count <= 0 || index < 0 || index >= numCocks)
			{
				return 0;
			}


			int oldCount = numCocks;
			Gender oldGender = gender;


			missingCockSexCount += (uint)cocks.Skip(index).Take(count).Sum(x => x.totalSexCount);
			missingCockSoundCount += (uint)cocks.Skip(index).Take(count).Sum(x => x.soundCount);
			missingCockOrgasmCount += (uint)cocks.Skip(index).Take(count).Sum(x => x.orgasmCount);
			missingCockDryOrgasmCount += (uint)cocks.Skip(index).Take(count).Sum(x => x.dryOrgasmCount);

			_cocks.RemoveRange(index, count);

			source.CheckGenderChanged(oldGender);
			return oldCount - numCocks;
		}

		public bool RemoveCock(Cock cock)
		{
			if (cock.cockIndex >= _cocks.Count || cock.cockIndex < 0 || _cocks[cock.cockIndex] != cock)
			{
				return false;
			}
			return RemoveCockAt(cock.cockIndex, 1) == 1;
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

		public bool UpdateCockWithLength(int index, CockType newType, double newLength)
		{
			return _cocks[index].UpdateCockTypeWithLength(newType, newLength);
		}

		public bool UpdateCockWithLengthAndGirth(int index, CockType newType, double newLength, double newGirth)
		{
			return _cocks[index].UpdateCockTypeWithLengthAndGirth(newType, newLength, newGirth);
		}

		public bool UpdateCockWithKnot(int index, CockType newType, double newKnotMultiplier)
		{
			return _cocks[index].UpdateCockTypeWithKnotMultiplier(newType, newKnotMultiplier);
		}

		public bool UpdateCockWithAll(int index, CockType newType, double newLength, double newGirth, double newKnotMultiplier)
		{
			return _cocks[index].UpdateCockTypeWithAll(newType, newLength, newGirth, newKnotMultiplier);
		}

		public bool RestoreCock(int index)
		{
			return _cocks[index].Restore();
		}

		#endregion

		#region Shrink Cock With Remove

		public bool ShrinkCockAndRemoveIfTooSmall(int index, double shrinkAmount, bool ignorePerks = false)
		{
			if (shrinkAmount <= 0)
			{
				return false;
			}

			if (cocks[index].DecreaseLengthAndCheckIfNeedsRemoval(shrinkAmount, ignorePerks))
			{
				_cocks.RemoveAt(index);
				return true;
			}
			else
			{
				return false;
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
			double avgLength = AverageCockLength();
			double avgGirth = AverageCockGirth();
			if (untilEven)
			{
				foreach (Cock cock in _cocks)
				{
					cock.SetLengthAndGirth(avgLength, avgGirth);
				}
			}
			else
			{
				foreach (Cock cock in _cocks)
				{
					if (cock.girth < avgGirth - 0.5f)
					{
						cock.IncreaseThickness(0.5f);
					}
					else if (cock.girth > avgGirth + 0.5f)
					{
						cock.DecreaseThickness(0.5f);
					}
					else
					{
						cock.SetGirth(avgGirth);
					}

					if (cock.length < avgLength - 1)
					{
						cock.IncreaseLength(1);
					}
					else if (cock.length > avgLength + 1)
					{
						cock.DecreaseLength(1);
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
		public double IncreaseCumMultiplier(double additionalMultiplier = 1)
		{
			double oldValue = cumMultiplierTrue;
			cumMultiplierTrue += additionalMultiplier;
			return cumMultiplierTrue - oldValue;
		}

		public double DecreaseCumMultiplier(double decreaseInMultiplier = 1)
		{
			double oldValue = cumMultiplierTrue;
			cumMultiplierTrue -= decreaseInMultiplier;
			return oldValue - cumMultiplierTrue;
		}

		public double AddFlatCumAmount(double additionalCum)
		{
			double oldValue = additionalCumTrue;
			additionalCumTrue += additionalCum;
			return additionalCumTrue - oldValue;
		}

		public uint AddHoursPentUp(uint additionalTime)
		{
			uint oldValue = pentUpBonus;
			pentUpBonus = pentUpBonus.add(additionalTime);
			return pentUpBonus - oldValue;
		}

		public uint RemoveHoursPentUp(uint reliefTime)
		{
			uint oldValue = pentUpBonus;
			pentUpBonus = pentUpBonus.subtract(reliefTime);
			return oldValue - pentUpBonus;
		}
		#endregion

		#region Cock Sex Related Functions

		internal void HandleCockSounding(int cockIndex, double penetratorLength, double penetratorWidth, double knotSize, double cumAmount, bool reachOrgasm)
		{
			cocks[cockIndex].SoundCock(penetratorLength, penetratorWidth, knotSize, reachOrgasm);
			if (reachOrgasm)
			{
				timeLastCum = GameDateTime.Now;
			}
		}

		internal void HandleCockPenetrate(int cockIndex, bool reachOrgasm, bool selfPenetrate)
		{
			cocks[cockIndex].DoSex(reachOrgasm, selfPenetrate);
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


		public string AllCocksShortDescription() => CockCollectionStrings.AllCocksShortDescription(this, out bool _);
		public string AllCocksShortDescription(out bool isPlural) => CockCollectionStrings.AllCocksShortDescription(this, out isPlural);


		public string AllCocksLongDescription() => CockCollectionStrings.AllCocksLongDescription(this, out bool _);
		public string AllCocksLongDescription(out bool isPlural) => CockCollectionStrings.AllCocksLongDescription(this, out isPlural);


		public string AllCocksFullDescription() => CockCollectionStrings.AllCocksFullDescription(this, out bool _);
		public string AllCocksFullDescription(out bool isPlural) => CockCollectionStrings.AllCocksFullDescription(this, out isPlural);


		public string OneCockOrCocksNoun() => CockCollectionStrings.OneCockOrCocksNoun(this);
		public string OneCockOrCocksNoun(Conjugate conjugate) => CockCollectionStrings.OneCockOrCocksNoun(this, conjugate);


		public string OneCockOrCocksShort() => CockCollectionStrings.OneCockOrCocksShort(this);
		public string OneCockOrCocksShort(Conjugate conjugate) => CockCollectionStrings.OneCockOrCocksShort(this, conjugate);


		public string EachCockOrCocksNoun() => CockCollectionStrings.EachCockOrCocksNoun(this);
		public string EachCockOrCocksNoun(Conjugate conjugate) => CockCollectionStrings.EachCockOrCocksNoun(this, conjugate);


		public string EachCockOrCocksShort() => CockCollectionStrings.EachCockOrCocksShort(this);
		public string EachCockOrCocksShort(Conjugate conjugate) => CockCollectionStrings.EachCockOrCocksShort(this, conjugate);


		public string EachCockOrCocksNoun(Conjugate conjugate, out bool isPlural) => CockCollectionStrings.EachCockOrCocksNoun(this, conjugate, out isPlural);


		public string EachCockOrCocksShort(Conjugate conjugate, out bool isPlural) => CockCollectionStrings.EachCockOrCocksShort(this, conjugate, out isPlural);

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
		public readonly double cumMultiplierTrue;

		public readonly double additionalCumTrue;

		public readonly int hoursSinceLastCum;

		public readonly int simulatedHoursSinceLastCum;

		public readonly int totalCum;

		#region Sexual Metadata

		public readonly uint totalSoundCount;
		public readonly uint totalSexCount;
		public readonly uint selfSexCount;

		public readonly uint totalOrgasmCount;
		public readonly uint dryOrgasmCount;

		#endregion

		internal readonly byte currentLust;
		internal readonly double relativeLust;

		public readonly ReadOnlyCollection<CockData> cocks;
		ReadOnlyCollection<CockData> ICockCollection<CockData>.cocks => cocks;

		public CockData this[int index]
		{
			get => cocks[index];
		}

		public readonly BallsData balls;

		public CockCollectionData(CockCollection source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			cumMultiplierTrue = source.cumMultiplierTrue;
			additionalCumTrue = source.additionalCumTrue;
			hoursSinceLastCum = source.hoursSinceLastCum;
			simulatedHoursSinceLastCum = source.simulatedHoursSinceLastCum;
			totalCum = source.totalCum;

			totalSoundCount = source.totalSoundCount;
			totalSexCount = source.totalSexCount;
			selfSexCount = source.selfSexCount;
			totalOrgasmCount = source.totalOrgasmCount;
			dryOrgasmCount = source.dryOrgasmCount;

			currentLust = source.currentLust;
			relativeLust = source.relativeLust;

			balls = source.balls.AsReadOnlyData();
			cocks = new ReadOnlyCollection<CockData>(source.cocks.Select(x => x.AsReadOnlyData()).ToList());
		}

		#region Public Cock Computed Values
		public bool hasCock => cocks.Count > 0;

		public bool cockVirgin => totalSexCount == 0;

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

		#endregion

		#region Constructor
		//public CockCollectionData(CockCollection source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		//{
		//	balls = source.balls.AsReadOnlyData();

		//	cumMultiplierTrue = source.cumMultiplierTrue;
		//	additionalCumTrue = source.additionalCumTrue;
		//	cocks = new ReadOnlyCollection<CockData>(source.cocks.Select(x => x.AsReadOnlyData()).ToList());

		//this.hoursSinceLastCum = source.hoursSinceLastCum;
		//this.totalCum = source.totalCum;

		//currentLust = source.currentLust;
		//relativeLust = source.relativeLust;
		//}
		#endregion

		#region CockData Aggregate Functions
		public double BiggestCockSize()
		{
			return cocks.Max(x => x.area);
		}

		public double LongestCockLength()
		{
			return cocks.Max(x => x.length);
		}

		public double WidestCockMeasure()
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

		public double AverageCockSize()
		{
			return cocks.Average(x => x.area);
		}

		public double AverageCockLength()
		{
			return cocks.Average(x => x.length);
		}

		public double AverageCockGirth()
		{
			return cocks.Average(x => x.girth);
		}

		public CockData AverageCock()
		{
			if (cocks.Count == 0)
			{
				return null;
			}
			double averageLength = cocks.Average(x => x.length);
			double averageGirth = cocks.Average(x => x.girth);
			double averageKnot = cocks.Average(x => x.knotMultiplier);
			double averageKnotSize = cocks.Average(x => x.knotSize);
			//first initially gets the first group. the second call to first gets the first element of the first group.
			CockType type = cocks.GroupBy(x => x.type).OrderByDescending(y => y.Count()).First().First().type;
			return Cock.GenerateAggregate(creatureID, type, averageKnot, averageKnotSize, averageLength, averageGirth, totalCum, hasSheath, currentLust, relativeLust);
		}

		public double SmallestCockSize()
		{
			return cocks.Min(x => x.area);
		}

		public double ShortestCockLength()
		{
			return cocks.Min(x => x.length);
		}

		public double ThinnestCockMeasure()
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


		public string AllCocksShortDescription() => CockCollectionStrings.AllCocksShortDescription(this, out bool _);
		public string AllCocksShortDescription(out bool isPlural) => CockCollectionStrings.AllCocksShortDescription(this, out isPlural);


		public string AllCocksLongDescription() => CockCollectionStrings.AllCocksLongDescription(this, out bool _);
		public string AllCocksLongDescription(out bool isPlural) => CockCollectionStrings.AllCocksLongDescription(this, out isPlural);


		public string AllCocksFullDescription() => CockCollectionStrings.AllCocksFullDescription(this, out bool _);
		public string AllCocksFullDescription(out bool isPlural) => CockCollectionStrings.AllCocksFullDescription(this, out isPlural);


		public string OneCockOrCocksNoun() => CockCollectionStrings.OneCockOrCocksNoun(this);
		public string OneCockOrCocksNoun(Conjugate conjugate) => CockCollectionStrings.OneCockOrCocksNoun(this, conjugate);


		public string OneCockOrCocksShort() => CockCollectionStrings.OneCockOrCocksShort(this);
		public string OneCockOrCocksShort(Conjugate conjugate) => CockCollectionStrings.OneCockOrCocksShort(this, conjugate);


		public string EachCockOrCocksNoun() => CockCollectionStrings.EachCockOrCocksNoun(this);
		public string EachCockOrCocksNoun(Conjugate conjugate) => CockCollectionStrings.EachCockOrCocksNoun(this, conjugate);


		public string EachCockOrCocksShort() => CockCollectionStrings.EachCockOrCocksShort(this);
		public string EachCockOrCocksShort(Conjugate conjugate) => CockCollectionStrings.EachCockOrCocksShort(this, conjugate);


		public string EachCockOrCocksNoun(Conjugate conjugate, out bool isPlural) => CockCollectionStrings.EachCockOrCocksNoun(this, conjugate, out isPlural);


		public string EachCockOrCocksShort(Conjugate conjugate, out bool isPlural) => CockCollectionStrings.EachCockOrCocksShort(this, conjugate, out isPlural);
		#endregion

	}
}
