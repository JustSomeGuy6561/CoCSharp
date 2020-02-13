using CoC.Backend.BodyParts.SpecialInteraction;
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
	public sealed partial class BreastCollection : SimpleSaveablePart<BreastCollection, BreastCollectionData>, IGrowable, IShrinkable
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

		#region Public Breast Related Members

		public readonly ReadOnlyCollection<Breasts> breastRows;


		public Breasts this[int index]
		{
			get => _breasts[index];
		}

		#endregion

		#region Public Nipple Related Members
		//public bool blackNipples { get; private set; } = false;
		//public bool quadNipples { get; private set; } = false;
		//public NippleStatus nippleType { get; private set; } = NippleStatus.NORMAL;

		//public float nippleLength { get; private set; } = 0.5f;

		internal readonly NippleAggregate nippleData;

		public bool hasBlackNipples => nippleData.hasBlackNipples;
		public bool hasQuadNipples => nippleData.hasQuadNipples;
		public NippleStatus nippleStatus => nippleData.nippleStatus;

		public float nippleLength => nippleData.length;

		public bool unlockedDickNipples
		{
			get => _unlockedDickNipples;
			private set => _unlockedDickNipples = value;
		}
		private bool _unlockedDickNipples = false;

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
		public float lactationProductionModifier
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

		public float lactationAmountPerBreast => currentLactationAmount / totalBreasts;
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

		private readonly List<Breasts> _breasts = new List<Breasts>(MAX_BREAST_ROWS);

		private uint missingRowTitFuckCount = 0;
		private uint missingRowSelfTitFuckCount = 0;
		private uint missingRowBreastOrgasmCount = 0;
		private uint missingRowBreastDryOrgasmCount = 0;
		#endregion

		#region Private Nipple Related Members

		private uint missingRowNippleFuckCount = 0;
		private uint missingRowSelfNippleFuckCount = 0;
		private uint missingRowDickNippleSexCount = 0;

		#endregion

		#region Private Lactation Related Members
		private GameDateTime timeLastMilked;

		private GameDateTime timeBecameFull;



		#endregion

		#region Public Breast Related Computed/Derived Values
		public int numBreastRows => _breasts.Count;

		public int totalBreasts => _breasts.Sum(x => x.numBreasts);

		internal CupSize maleNewDefaultCup => perkData.MaleNewDefaultCup;
		internal CupSize femaleNewDefaultCup => perkData.FemaleNewDefaultCup;
		internal sbyte maleNewCupDelta => perkData.MaleNewCupDelta;
		internal sbyte femaleNewCupDelta => perkData.FemaleNewCupDelta;

		public CupSize smallestPossibleMaleCupSize => perkData.MaleMinCup;
		public CupSize smallestPossibleFemaleCupSize => perkData.FemaleMinCup;

		public CupSize smallestPossibleCupSize => gender.HasFlag(Gender.FEMALE) ? smallestPossibleFemaleCupSize : smallestPossibleMaleCupSize;


		internal float titsGrowthMultiplier => perkData.TitsGrowthMultiplier;
		internal float titsShrinkMultiplier => perkData.TitsShrinkMultiplier;

		public uint totalTitFuckCount => missingRowTitFuckCount + (uint)breastRows.Sum(x => x.totalTitFuckCount);
		public uint selfTitFuckCount => missingRowSelfTitFuckCount + (uint)breastRows.Sum(x => x.selfTitFuckCount);

		public uint breastOrgasmCount => missingRowBreastOrgasmCount.add((uint)breastRows.Sum(x => x.orgasmCount));
		public uint breastDryOrgasmCount => missingRowBreastDryOrgasmCount.add((uint)breastRows.Sum(x => x.dryOrgasmCount));

		#endregion

		#region Public Nipple Related Computed Properties
		public int nippleCount => _breasts.Count * Breasts.NUM_BREASTS * (hasQuadNipples ? 4 : 1);


		public uint totalFuckableNippleSexCount => missingRowNippleFuckCount + (uint)breastRows.Sum(x => x.totalFuckableNippleSexCount);
		public uint selfFuckableNippleSexCount => missingRowSelfNippleFuckCount + (uint)breastRows.Sum(x => x.selfFuckableNippleSexCount);
		public uint totalDickNippleSexCount => missingRowDickNippleSexCount + (uint)breastRows.Sum(x => x.dickNippleSexCount);

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

		public float lactationRate => Utils.Lerp(LACTATION_THRESHOLD, EPIC_LACTATION_THRESHOLD, lactationProductionModifier, 0, 1.0f);

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
		public LactationStatus lactationStatus => StatusFromRate(lactationRate);

		internal static LactationStatus StatusFromRate(float lactationRate)
		{
			if (lactationRate < LACTATION_THRESHOLD)
			{
				return LactationStatus.NOT_LACTATING;
			}
			else if (lactationRate < MODERATE_LACTATION_THRESHOLD)
			{
				return LactationStatus.LIGHT;
			}
			else if (lactationRate < STRONG_LACTATION_THRESHOLD)
			{
				return LactationStatus.MODERATE;
			}
			else if (lactationRate < HEAVY_LACTATION_THRESHOLD)
			{
				return LactationStatus.STRONG;
			}
			else if (lactationRate < EPIC_LACTATION_THRESHOLD)
			{
				return LactationStatus.HEAVY;
			}
			else
			{
				return LactationStatus.EPIC;
			}
		}

		public bool isLactating => lactationStatus != LactationStatus.NOT_LACTATING;
		#endregion

		private readonly Genitals source;

		private uint currentBreastID = 0;

		private GenitalPerkData perkData => source.perkData;

		internal BodyType bodyType => CreatureStore.GetCreatureClean(creatureID)?.body.type ?? BodyType.defaultValue;

		public float relativeLust => source.relativeLust;

		public bool isPregnant => source.isPregnant;

		public Gender gender => source.gender;

		#region Constructor
		public BreastCollection(Genitals parent) : base(parent?.creatureID ?? throw new ArgumentNullException(nameof(parent)))
		{
			source = parent;

			breastRows = new ReadOnlyCollection<Breasts>(_breasts);

			nippleData = new NippleAggregate(creatureID, this);
		}
		#endregion

		#region LateInit

		internal void Initialize(BreastCreator[] breastCreators)
		{
			var rows = breastCreators.Where(x => x != null).Take(MAX_BREAST_ROWS);
			foreach (var row in rows)
			{
				_breasts.Add(new Breasts(this, currentBreastID, row.cupSize, row.nipplePiercings));
				currentBreastID++;
			}
		}

		#endregion

		#region Simple Body Part Data

		internal override bool Validate(bool correctInvalidData)
		{
			bool valid = true;
			if (_breasts.Count > MAX_BREAST_ROWS)
			{
				if (!correctInvalidData)
				{
					return false;
				}

				valid = false;
				_breasts.RemoveRange(MAX_BREAST_ROWS, _breasts.Count - MAX_BREAST_ROWS);
			}

			foreach (var breast in _breasts)
			{
				valid |= breast.Validate(correctInvalidData);
				if (!valid && !correctInvalidData)
				{
					return false;
				}
			}
			return valid;
		}

		public override BreastCollectionData AsReadOnlyData()
		{
			return new BreastCollectionData(this);
		}

		public override string BodyPartName()
		{
			return Name();
		}

		public override bool IsIdenticalTo(BreastCollectionData original, bool ignoreSexualMetaData)
		{
			if (original is null) return false;

			Dictionary<uint, Breasts> items = _breasts.ToDictionary(x => x.collectionID, x => x);
			Dictionary<uint, BreastData> dataItems = original.breasts.ToDictionary(x => (uint)x.collectionID, x => x);

			//this was a fucking bitch to figure out lmao.
			return nippleData.IsIdenticalTo(original.nippleData, ignoreSexualMetaData) && lactation_TotalCapacityMultiplier == original.lactation_TotalCapacityMultiplier
				&& lactation_CapacityMultiplier == original.lactation_CapacityMultiplier && lactationProductionModifier == original.lactationProductionModifier
				&& overfullBuffer == original.overfullBuffer && currentLactationAmount == original.currentLactationAmount && isOverfull == original.isOverfull
				&& canLessenCurrentLactationLevels == original.canLessenCurrentLactationLevels && hoursSinceLastMilked == original.hoursSinceLastMilked
				&& hoursOverfull == original.hoursOverfull && maximumLactationCapacity == original.maximumLactationCapacity && lactationRate == original.lactationRate
				&& currentLactationCapacity == original.currentLactationCapacity && lactationStatus == original.lactationStatus
				&& (ignoreSexualMetaData || (totalTitFuckCount == original.totalTitFuckCount && selfTitFuckCount == original.selfTitFuckCount
				&& totalFuckableNippleSexCount == original.totalFuckableNippleSexCount && selfFuckableNippleSexCount == original.selfFuckableNippleSexCount
				&& totalDickNippleSexCount == original.totalDickNippleSexCount))
				&& items.Keys.Count == dataItems.Keys.Count && items.All(x => dataItems.ContainsKey(x.Key) && x.Value.IsIdenticalTo(dataItems[x.Key], ignoreSexualMetaData));
		}

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

		public BreastData AverageBreasts()
		{
			if (_breasts.Count == 0)
			{
				return null;
			}
			var averageCup = AverageCupSize();
			return Breasts.GenerateAggregate(creatureID, averageCup, nippleLength, hasBlackNipples, hasQuadNipples, nippleData.dickNipplesEnabled, nippleStatus,
				lactationRate, lactationStatus, isOverfull, gender, bodyType, relativeLust, perkData.MaleMinCup);
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
			_breasts.Add(new Breasts(this, currentBreastID, cup));
			currentBreastID++;

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
			double avgCup = _breasts.Average((x) => (double)x.cupSize);
			byte cup = (byte)Math.Ceiling(avgCup);
			_breasts.Add(new Breasts(this, currentBreastID, (CupSize)cup));
			currentBreastID++;

			return true;
		}

		public bool AddBreastRow(CupSize cup)
		{
			if (numBreastRows >= MAX_BREAST_ROWS)
			{
				return false;
			}

			_breasts.Add(new Breasts(this, currentBreastID, cup));
			currentBreastID++;
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
				missingRowTitFuckCount += _breasts[0].totalTitFuckCount;
				missingRowSelfTitFuckCount += _breasts[0].selfTitFuckCount;
				missingRowBreastOrgasmCount += _breasts[0].orgasmCount;
				missingRowBreastDryOrgasmCount += _breasts[0].dryOrgasmCount;

				missingRowNippleFuckCount += _breasts[0].totalFuckableNippleSexCount;
				missingRowSelfNippleFuckCount += _breasts[0].selfFuckableNippleSexCount;
				missingRowDickNippleSexCount += _breasts[0].dickNippleSexCount;
				_breasts[0].Reset();

				count = numBreastRows - 1;
			}

			var toRemove = breastRows.Skip(numBreastRows - count);

			missingRowTitFuckCount += (uint)toRemove.Sum(x => x.totalTitFuckCount);
			missingRowSelfTitFuckCount += (uint)toRemove.Sum(x => x.selfTitFuckCount);
			missingRowBreastOrgasmCount += (uint)toRemove.Sum(x => x.orgasmCount);
			missingRowBreastDryOrgasmCount += (uint)toRemove.Sum(x => x.dryOrgasmCount);

			missingRowNippleFuckCount += (uint)toRemove.Sum(x => x.totalFuckableNippleSexCount);
			missingRowSelfNippleFuckCount += (uint)toRemove.Sum(x => x.selfFuckableNippleSexCount);
			missingRowDickNippleSexCount += (uint)toRemove.Sum(x => x.dickNippleSexCount);

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
		public bool NormalizeBreasts(bool untilEven = false)
		{
			if (numBreastRows == 1)
			{
				return false;
			}
			CupSize averageSize = AverageCupSize();

			bool changedAnything = false;

			if (untilEven)
			{
				foreach (var row in _breasts)
				{

					var oldSize = row.cupSize;
					row.SetCupSize(averageSize);

					changedAnything |= oldSize != row.cupSize;
				}
			}
			else
			{
				foreach (var row in _breasts)
				{
					if (row.cupSize > averageSize)
					{
						changedAnything |= row.ShrinkBreasts(1, true) != 0;
					}
					else if (row.cupSize < averageSize)
					{
						changedAnything |= row.GrowBreasts(1, true) != 0;
					}
				}
			}

			return changedAnything;
		}

		/// <summary>
		/// A variant of normalize that brings all closer to the average, but with the caveat that each breast row will be one larger than the next, if possible.
		/// the only time this cannot occur is when this would cause a breast row to go negative, and if this happens, all rows that would become negative instead remain flat (0).
		/// </summary>
		/// <param name="untilEven"></param>

		//i hated writing this function so much holy shit lol. i mean, now that it's in, feel free to use it anywhere, it works like a charm. but damn was it a pain to figure out.
		public bool AnthropomorphizeBreasts(bool untilEven = false)
		{
			//only one row or all are flat.
			if (numBreastRows == 1 || BiggestCupSize() == CupSize.FLAT)
			{
				return false;
			}

			byte[] target = new byte[_breasts.Count];

			int sequenceSum(int n) => (n * (n + 1)) / 2;
			int partialSequenceSum(int start, int count) => ((start - 1) * start - (start + count - 1) * (start + count)) / 2;

			//calculations for reversing the sequence formula, solving for start.
			//sequence(n to m) = ((m-1) * m - (n-1) * n) / 2
			//m = n + x -1. x is the number of elements to sum.
			//subbing for m:
			//c = ((n + x - 1) * (n + x) - n * (n - 1)) / 2
			//c = (n^2 + nx + nx + x^2 -n -x -n^2 + n) / 2
			//c = (nx + nx + x^2 -x) / 2

			//c = (2nx + x^2 -x)/2
			//2c = x * (x + 2n -1)

			//2c/x = x + 2n - 1
			//2c/x - x + 1 = 2n
			//n = c/x - x/2 + 1/2

			//generates a double. this is the same as the partial sequence sum, but solved for the starting point. see the proof above for how it works.
			//additionally, end is not known, but fortunately, we know the number of elements in the sequence, and thus we can represent end as start + nElems - 1.
			double sequenceN(int sum, int nElems) => sum * 1.0 / nElems - nElems / 2.0 + 0.5;

			int cupToInt(Breasts breast) => (byte)breast.cupSize;

			int breastSum = _breasts.Sum(cupToInt);

			int index = _breasts.Count - 1;

			//if the sum of all the breast rows' cup sizes is less than the sequence starting at 1 and ending at the number of rows, it means we have some flat rows. that's ok,
			//but we need to make sure the target value is set accordingly. by default, the byte array is already 0, so this is more or less redundant, but it is better to write
			//this code, because it makes the intent clear.
			while (index >= 0 && breastSum < sequenceSum(index))
			{
				target[index] = 0;
				index--;
			}

			//if, at this point, we still haven't calculated all of the target values (which should be the case, but whatever.).
			if (index >= 0)
			{
				//if it's easy, and we have the exact values for a sequence.
				if (breastSum == sequenceSum(index + 1))
				{
					//generate the sequence.
					byte size = 1;
					for (; index >= 0; index--)
					{
						target[index] = size++;
					}
				}
				else
				{
					//we need to fake a sequence.

					//this will find the starting value for the sequence. note that this will probably have a decimal value, so we need to take care of this too.
					double smallestCupValue = sequenceN(breastSum, index + 1);
					byte targetValue = (byte)Math.Floor(smallestCupValue);

					//this will help handle the decimal value if applicable.
					int extra = breastSum - partialSequenceSum(targetValue, _breasts.Count);

					//set the inital values, starting at the last row, moving up. each time we increment the targetValue by 1.
					for (int x = _breasts.Count - 1; x >= 0; x--)
					{
						target[x] = targetValue;
						targetValue++;
					}

					//then, handle the extra amount. we do this moving down.
					int extraIndex = 0;
					while (extra > 0)
					{
						target[extraIndex]++;

						extraIndex++;
						extra--;
					}
				}
			}

			bool changedAnything = false;

			if (untilEven)
			{
				for (int x = 0; x < _breasts.Count; x++)
				{
					var oldSize = _breasts[x].cupSize;

					_breasts[x].SetCupSize((CupSize)target[x]);

					changedAnything |= oldSize != _breasts[x].cupSize;
				}
			}
			else
			{
				//there's a bit of an edge case where where the amount we change could be, say, +3 in the top row and -1 in the remaining 3 rows.
				//we wouldn't want to only do +1 in that first row, because that wouldn't evenly distribute the mass.
				//so, we have this nonsense.

				var adders = _breasts.Where((x, y) => (byte)x.cupSize < target[y]);
				var subbers = _breasts.Where((x, y) => (byte)x.cupSize > target[y]);

				int addCount = adders.Count();
				int subCount = subbers.Count();

				//handle the easiest case first.
				if (addCount == subCount)
				{
					adders.ForEach(x => changedAnything |= x.GrowBreasts(1, true) != 0);
					subbers.ForEach(x => changedAnything |= x.ShrinkBreasts(1, true) != 0);
				}
				//more adds than subs, but our subs need to change more.
				else if (addCount > subCount)
				{
					adders.ForEach(x => changedAnything |= x.GrowBreasts(1, true) != 0);
					subbers.ForEach(x => changedAnything |= x.ShrinkBreasts(1, true) != 0);

					int extraSubs = addCount - subCount;

					//empty check probably not needed, but i don't want to infinite loop just in case.
					while (extraSubs > 0 && !subbers.IsEmpty())
					{
						subbers = _breasts.Where((x, y) => (byte)x.cupSize > target[y]);

						foreach (var sub in subbers)
						{
							if (extraSubs == 0)
							{
								break;
							}

							changedAnything |= sub.ShrinkBreasts(1, true) != 0;
							extraSubs--;
						}
					}
				}
				else //if (subCount > addCount)
				{
					subbers.ForEach(x => changedAnything |= x.ShrinkBreasts(1, true) != 0);
					adders.ForEach(x => changedAnything |= x.GrowBreasts(1, true) != 0);

					int extraAdds = subCount - addCount;

					//empty check probably not needed, but i don't want to infinite loop just in case.
					while (extraAdds > 0 && !adders.IsEmpty())
					{
						adders = _breasts.Where((x, y) => (byte)x.cupSize < target[y]);

						foreach (var add in adders)
						{
							if (extraAdds == 0)
							{
								break;
							}

							changedAnything |= add.GrowBreasts(1, true) != 0;
							extraAdds--;
						}
					}
				}
			}

			return changedAnything;
		}

		#endregion

		#region Nipple Mutators
		public bool SetNippleStatus(NippleStatus desiredStatus, bool limitToCurrentLength = false, bool toggleDickNippleFlagIfNeccesary = false) =>
			nippleData.SetNippleStatus(desiredStatus, limitToCurrentLength, toggleDickNippleFlagIfNeccesary);

		public bool SetQuadNipples(bool active) => nippleData.SetQuadNipples(active);

		public bool SetBlackNipples(bool active) => nippleData.SetBlackNipples(active);

		public bool UnlockDickNipples() => nippleData.UnlockDickNipples();

		public bool PreventDickNipples() => nippleData.PreventDickNipples();

		public bool SetDickNippleFlag(bool enabled) => nippleData.SetDickNippleFlag(enabled);

		public float GrowNipples(float amount, bool ignorePerks = false) => nippleData.GrowNipples(amount, ignorePerks);

		public float ShrinkNipples(float amount, bool ignorePerks = false) => nippleData.ShrinkNipples(amount, ignorePerks);

		public float ChangeNippleLength(float delta, bool ignorePerks = false) => nippleData.ChangeNippleLength(delta, ignorePerks);

		public bool SetNippleLength(float size) => nippleData.SetNippleLength(size);

		#endregion

		#region Lactation Update Functions
		/// <summary>
		///boost your lactation or decrease your lactation until it reaches the given threshold, if possible. Returns the actual threshold it reached.
		/// </summary>
		/// <param name="newStatus">The new lactation status desired.</param>
		/// <returns>The lactation status given, or the closest value it can reach do to other factors.</returns>
		public LactationStatus setLactationTo(LactationStatus newStatus)
		{
			lactationProductionModifier = newStatus.MinThreshold();
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
			var modifier = lactationProductionModifier;
			lactationProductionModifier += byAmount;
			if (lactationStatus < minimumLactationLevel)
			{
				setLactationTo(minimumLactationLevel);
			}
			else if (lactationProductionModifier < LACTATION_THRESHOLD && byAmount < 0)
			{
				lactationProductionModifier = 0;
			}
			return lactationProductionModifier - modifier;
		}

		/// <summary>
		/// If the character is lactating, boost lactation by the default amount. Otherwise, set the production level so the character starts lactating.
		/// </summary>
		public void StartOrBoostLactation()
		{

			var oldLactation = lactationProductionModifier;
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

		public bool CanTitFuck()
		{
			return breastRows.Any(x => x.TittyFuckable());
		}

		//to be frank, idk what would actually orgasm when being titty fucked, but, uhhhh... i guess it can be stored in stats or some shit?

		internal void HandleTittyFuck(int breastIndex, float length, float girth, float knotWidth, float cumAmount, bool reachOrgasm, bool sourceIsSelf)
		{
			breastRows[breastIndex].DoTittyFuck(length, girth, knotWidth, reachOrgasm, sourceIsSelf);
		}

		internal void HandleTitOrgasmGeneric(int breastIndex, bool dryOrgasm)
		{
			breastRows[breastIndex].OrgasmTits(dryOrgasm);
		}
		#endregion

		#region Nipple Sex Related Functions

		internal void HandleNipplePenetration(int breastIndex, float length, float girth, float knotWidth, float cumAmount, bool reachOrgasm, bool sourceIsSelf)
		{
			breastRows[breastIndex].DoNippleFuck(length, girth, knotWidth, cumAmount, reachOrgasm, sourceIsSelf);
		}

		internal void HandleNippleDickPenetrate(int breastIndex, bool reachOrgasm)
		{
			breastRows[breastIndex].DoDickNippleSex(reachOrgasm);
		}
		#endregion

		#region Lactation Sex Related Functions

		/// <summary>
		/// Attempt to milk the current character, with as much milk as they can provide. Can be called when the character is not lactating, which will help induce lactation.
		/// </summary>
		/// <returns>The amount of milk lactated</returns>
		internal float MilkOrSuckle()
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
		internal float MilkOrSuckle(float maxAmount)
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
		internal bool DoLazyLactationCheck(bool isPlayer, byte hoursPassed, out string results)
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
						boostLactation((float)((1 + totalBreasts / 8) * multiplier));

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
			else if (lactationProductionModifier != 0 && hoursSinceLastMilked >= 48)
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

		#region Breast Text

		public string AllBreastsShortDescription(bool alternateFormat = false) => BreastCollectionStrings.AllBreastsShortDescription(this, alternateFormat);

		public string AllBreastsLongDescription(bool alternateFormat = false) => BreastCollectionStrings.AllBreastsLongDescription(this, alternateFormat);

		public string AllBreastsFullDescription(bool alternateFormat = false) => BreastCollectionStrings.AllBreastsFullDescription(this, alternateFormat);

		public string ChestOrAllBreastsShort(bool alternateFormat = false) => BreastCollectionStrings.ChestOrAllBreastsShort(this, alternateFormat);

		public string ChestOrAllBreastsLong(bool alternateFormat = false) => BreastCollectionStrings.ChestOrAllBreastsLong(this, alternateFormat);

		public string ChestOrAllBreastsFull(bool alternateFormat = false) => BreastCollectionStrings.ChestOrAllBreastsFull(this, alternateFormat);




		#endregion

		#region Breast Collection Exclusive Text

		internal string AllBreastsPlayerDescription()
		{
			if (CreatureStore.GetCreatureClean(creatureID) is PlayerBase player)
			{
				return AllBreastsPlayerText(player);
			}
			else
			{
				return "";
			}

		}
		#endregion

		#region IGrowable

		bool IGrowable.CanGroPlus()
		{
			return _breasts.Any(x => (x as IGrowable).CanGroPlus());
		}

		float IGrowable.UseGroPlus()
		{
			return _breasts.Average(x => (x as IGrowable).UseGroPlus());
		}

		#endregion

		#region IShrinkable

		bool IShrinkable.CanReducto()
		{
			return _breasts.Any(x => (x as IShrinkable).CanReducto());
		}

		float IShrinkable.UseReducto()
		{
			return _breasts.Average(x => (x as IShrinkable).UseReducto());
		}

		#endregion

	}

	public sealed partial class BreastCollectionData : SimpleData
	{
		#region Public Breast Related Members

		public readonly ReadOnlyCollection<BreastData> breasts;
		public BreastData this[int index]
		{
			get => breasts[index];
		}
		#endregion

		#region Public Nipple Related Members

		internal NippleAggregateData nippleData;

		public bool blackNipples => nippleData.hasBlackNipples;
		public bool quadNipples => nippleData.hasQuadNipples;
		public NippleStatus nippleType => nippleData.nippleStatus;
		public float nippleLength => nippleData.length;

		public bool unlockedDickNipples => nippleData.dickNipplesEnabled;

		public int nippleCount => numBreasts * (quadNipples ? 4 : 1);

		public readonly uint totalFuckableNippleSexCount;
		public readonly uint selfFuckableNippleSexCount;
		public readonly uint totalDickNippleSexCount;

		#endregion

		#region Public Lactation Related Members

		//multiplies capacity volume by this value to determine actual amount you can lactate. completely breaks physics, but so does most of this game, so...
		public readonly float lactation_TotalCapacityMultiplier;

		public readonly float lactation_CapacityMultiplier;
		public readonly float lactationProductionModifier;

		//how much time does this character have at full capacity before their lactation modifier starts decreasing, stored in hours. Note that at epic level, this value is halved, rounded up.
		public readonly uint overfullBuffer;

		public readonly float currentLactationAmount;

		public float lactationAmountPerBreast => currentLactationAmount / numBreasts;
		#endregion

		#region Public Breast Related Computed Values
		public int numBreastRows => breasts.Count;

		public byte numBreasts => (byte)breasts.Sum(x => x.numBreasts);

		public readonly uint totalTitFuckCount;
		public readonly uint selfTitFuckCount;

		public readonly uint breastOrgasmCount;
		public readonly uint breastDryOrgasmCount;

		#endregion

		#region Public Lactation Related Computed Values
		public readonly bool canLessenCurrentLactationLevels;

		public readonly int hoursSinceLastMilked;

		public readonly bool isOverfull;

		public readonly int hoursOverfull;

		public readonly float maximumLactationCapacity;
		//current maximum capacity. if you aren't lactating, this is 0.
		public readonly float currentLactationCapacity;

		public readonly float lactationRate;

		public readonly LactationStatus lactationStatus;

		public bool isLactating => lactationStatus != LactationStatus.NOT_LACTATING;
		#endregion

		public readonly float relativeLust;

		private readonly bool isPregnant;

		private readonly Gender gender;

		private readonly BodyType bodyType;

		private readonly CupSize maleMinCupSize;

		internal BreastCollectionData(BreastCollection source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			this.breasts = new ReadOnlyCollection<BreastData>(source.breastRows.Select(x => x.AsReadOnlyData()).ToList());

			this.lactation_TotalCapacityMultiplier = source.lactation_TotalCapacityMultiplier;
			this.lactation_CapacityMultiplier = source.lactation_CapacityMultiplier;
			this.lactationProductionModifier = source.lactationProductionModifier;
			this.overfullBuffer = source.overfullBuffer;
			this.currentLactationAmount = source.currentLactationAmount;


			this.canLessenCurrentLactationLevels = source.canLessenCurrentLactationLevels;
			this.hoursSinceLastMilked = source.hoursSinceLastMilked;
			this.isOverfull = source.isOverfull;
			this.hoursOverfull = source.hoursOverfull;
			this.maximumLactationCapacity = source.maximumLactationCapacity;
			this.currentLactationCapacity = source.currentLactationCapacity;
			this.lactationRate = source.lactationRate;
			this.lactationStatus = source.lactationStatus;

			this.nippleData = source.nippleData.AsReadOnlyData();

			this.relativeLust = source.relativeLust;
			this.isPregnant = source.isPregnant;
			this.gender = source.gender;

			this.maleMinCupSize = source.breastRows[0].maleMinCup;

			bodyType = source.bodyType;

			this.breastOrgasmCount = source.breastOrgasmCount;
			this.breastDryOrgasmCount = source.breastDryOrgasmCount;

			totalFuckableNippleSexCount = source.totalFuckableNippleSexCount;
			selfFuckableNippleSexCount = source.selfFuckableNippleSexCount;
			totalDickNippleSexCount = source.totalDickNippleSexCount;

			totalTitFuckCount = source.totalTitFuckCount;
			selfTitFuckCount = source.selfTitFuckCount;

			breastOrgasmCount = source.breastOrgasmCount;
			breastDryOrgasmCount = source.breastDryOrgasmCount;
		}

		#region Breast Aggregate Functions

		public CupSize BiggestCupSize()
		{
			return (CupSize)breasts.Max(x => (byte?)x?.cupSize);
		}

		public CupSize AverageCupSize()
		{
			return (CupSize)(byte)Math.Ceiling(breasts.Average(x => (double)x.cupSize));
		}

		public CupSize SmallestCupSize()
		{
			return (CupSize)breasts.Min(x => (byte?)x?.cupSize);
		}

		public BreastData LargestBreast()
		{
			return breasts.MaxItem(x => (byte)x.cupSize);
		}

		public BreastData SmallestBreast()
		{
			return breasts.MinItem(x => (byte)x.cupSize);
		}

		public BreastData AverageBreasts()
		{
			if (breasts.Count == 0)
			{
				return null;
			}
			var averageCup = AverageCupSize();
			return Breasts.GenerateAggregate(creatureID, averageCup, nippleLength, blackNipples, quadNipples, unlockedDickNipples, nippleType, lactationRate, lactationStatus,
				isOverfull, gender, bodyType, relativeLust, maleMinCupSize);
		}

		#endregion

		#region Breast Text

		public string AllBreastsShortDescription(bool alternateFormat = false) => BreastCollectionStrings.AllBreastsShortDescription(this, alternateFormat);

		public string AllBreastsLongDescription(bool alternateFormat = false) => BreastCollectionStrings.AllBreastsLongDescription(this, alternateFormat);

		public string AllBreastsFullDescription(bool alternateFormat = false) => BreastCollectionStrings.AllBreastsFullDescription(this, alternateFormat);

		public string ChestOrAllBreastsShort(bool alternateFormat = false) => BreastCollectionStrings.ChestOrAllBreastsShort(this, alternateFormat);

		public string ChestOrAllBreastsLong(bool alternateFormat = false) => BreastCollectionStrings.ChestOrAllBreastsLong(this, alternateFormat);

		public string ChestOrAllBreastsFull(bool alternateFormat = false) => BreastCollectionStrings.ChestOrAllBreastsFull(this, alternateFormat);


		#endregion
	}

	public sealed class BreastCollectionChanged
	{


	}
}
