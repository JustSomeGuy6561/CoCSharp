using CoC.Backend.Engine.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class Genitals
	{
		//the number of times had sex with cocks that no longer exist;
		private uint missingCockSexCount;
		//number of times had cock sounded for cocks that no longer exist.
		private uint missingCockSoundCount;
		//times cock orgasmed for missing cocks.
		private uint missingCockOrgasmCount;
		//times cock orgasmed without any stimulation for missing cocks.
		private uint missingCockDryOrgasmCount;

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


		internal float BiggestCockSize(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Max(x => x.area);
		}

		internal float LongestCockLength(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Max(x => x.length);
		}

		internal float WidestCockMeasure(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Max(x => x.girth);
		}

		internal Cock BiggestCock(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.MaxItem(x => x.area);
		}

		internal Cock LongestCock(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.MaxItem(x => x.length);
		}

		internal Cock WidestCock(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.MaxItem(x => x.girth);
		}

		internal float AverageCockSize(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Average(x => x.area);
		}

		internal float AverageCockLength(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Average(x => x.length);
		}

		internal float AverageCockGirth(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Average(x => x.girth);
		}

		internal float SmallestCockSize(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Min(x => x.area);
		}

		internal float ShortestCockLength(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Min(x => x.length);
		}

		internal float ThinnestCockMeasure(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Min(x => x.girth);
		}

		internal Cock SmallestCock(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.MinItem(x => x.area);
		}

		internal Cock ShortestCock(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.MinItem(x => x.length);
		}

		internal Cock ThinnestCock(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.MinItem(x => x.girth);
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
	}
}
