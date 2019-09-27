using CoC.Backend.Engine.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class Genitals
	{
		public uint timesCockSounded => missingCockSoundCount + (uint)cocks.Sum(x => x.soundCount);

		public uint timesHadSexWithCock => missingCockSexCount + (uint)cocks.Sum(x => x.sexCount);

		public bool cockVirgin => missingCockSexCount > 0 ? true : timesHadSexWithCock == 0; //the first one means no aggregate calculation, for efficiency.


		internal float BiggestCockSize()
		{
			return _cocks.Max(x => x.area);
		}

		internal float LongestCockLength()
		{
			return _cocks.Max(x => x.length);
		}

		internal float WidestCockMeasure()
		{
			return _cocks.Max(x => x.girth);
		}

		internal Cock BiggestCock()
		{
			return _cocks.Aggregate((x, y) => y.area > x.area ? y : x);
		}

		internal Cock LongestCock()
		{
			return _cocks.MaxItem(x => x.length);
		}

		internal Cock WidestCock()
		{
			return _cocks.MaxItem(x => x.girth);
		}

		internal float AverageCockSize()
		{
			return _cocks.Average(x => x.area);
		}

		internal float AverageCockLength()
		{
			return _cocks.Average(x => x.length);
		}

		internal float AverageCockGirth()
		{
			return _cocks.Average(x => x.girth);
		}

		internal float SmallestCockSize()
		{
			return _cocks.Min(x => x.area);
		}

		internal float ShortestCockLength()
		{
			return _cocks.Min(x => x.length);
		}

		internal float ThinnestCockMeasure()
		{
			return _cocks.Min(x => x.girth);
		}

		internal Cock SmallestCock()
		{
			return _cocks.MinItem(x => x.area);
		}

		internal Cock ShortestCock()
		{
			return _cocks.MinItem(x => x.length);
		}

		internal Cock ThinnestCock()
		{
			return _cocks.MinItem(x => x.girth);
		}

		internal void NormalizeDicks(bool untilEven = false)
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

		//the number of times had sex with cocks that no longer exist;
		private uint missingCockSexCount;
		//number of times had cock sounded for cocks that no longer exist.
		private uint missingCockSoundCount;


		internal int RemoveCock(int count = 1)
		{
			if (numCocks == 0 || count <= 0)
			{
				return 0;
			}
			int oldCount = numCocks;

			if (count > numCocks)
			{
				this.missingCockSexCount += (uint)cocks.Sum(x => x.sexCount);
				this.missingCockSoundCount += (uint)cocks.Sum(x => x.soundCount);
				_cocks.Clear();
			}
			else
			{
				this.missingCockSexCount += (uint)cocks.Skip(numCocks-count).Sum(x => x.sexCount);
				this.missingCockSoundCount += (uint)cocks.Skip(numCocks - count).Sum(x => x.soundCount);
				_cocks.RemoveRange(numCocks - count, count);
			}

			return oldCount - numCocks;
		}

		internal int RemoveExtraCocks()
		{
			return RemoveCock(numCocks - 1);
		}

		internal int RemoveAllCocks()
		{
			return RemoveCock(numCocks);
		}

		internal bool AddCock(CockType newCockType)
		{
			if (numCocks == MAX_COCKS)
			{
				return false;
			}
			_cocks.Add(new Cock(creatureID, GetCockPerkData(), newCockType));

			return true;
		}

		internal bool AddCock(CockType newCockType, float length, float girth, float? knotMultiplier = null)
		{
			if (numCocks >= MAX_COCKS)
			{
				return false;
			}
			_cocks.Add(new Cock(creatureID, GetCockPerkData(), newCockType, length, girth, knotMultiplier));

			return true;
		}

		internal void HandleCockSounding(int cockIndex, float penetratorLength, float penetratorWidth, bool reachOrgasm)
		{
			_cocks[cockIndex].SoundCock(penetratorLength, penetratorWidth, reachOrgasm);
			if (reachOrgasm)
			{
				timeLastCum = GameDateTime.Now;
			}
		}

		internal void HandleCockPenetrate(int cockIndex, bool reachOrgasm)
		{
			_cocks[cockIndex].DoSex(reachOrgasm);
			if (reachOrgasm)
			{
				timeLastCum = GameDateTime.Now;
			}
		}
	}
}
