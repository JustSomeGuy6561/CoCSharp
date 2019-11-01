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

		public float BiggestCockSize(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Max(x => x.area);
		}

		public float LongestCockLength(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Max(x => x.length);
		}

		public float WidestCockMeasure(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Max(x => x.girth);
		}

		public Cock BiggestCock(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.MaxItem(x => x.area);
		}

		public Cock LongestCock(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.MaxItem(x => x.length);
		}

		public Cock WidestCock(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.MaxItem(x => x.girth);
		}

		public float AverageCockSize(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Average(x => x.area);
		}

		public float AverageCockLength(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Average(x => x.length);
		}

		public float AverageCockGirth(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Average(x => x.girth);
		}

		public float SmallestCockSize(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Min(x => x.area);
		}

		public float ShortestCockLength(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Min(x => x.length);
		}

		public float ThinnestCockMeasure(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.Min(x => x.girth);
		}

		public Cock SmallestCock(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.MinItem(x => x.area);
		}

		public Cock ShortestCock(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.MinItem(x => x.length);
		}

		public Cock ThinnestCock(bool allowClitCock = true)
		{
			IEnumerable<Cock> cockCollection = _cocks;
			if (allowClitCock && hasClitCock)
			{
				cockCollection = cockCollection.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock()));
			}
			return cockCollection.MinItem(x => x.girth);
		}

		public int CountCocksOfType(CockType type)
		{
			return _cocks.Sum(x => x.type == type ? 1 : 0);
		}

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

		public bool AddCock(CockType newCockType)
		{
			if (numCocks == MAX_COCKS)
			{
				return false;
			}
			var oldGender = gender;

			_cocks.Add(new Cock(creatureID, GetCockPerkData(), newCockType));

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

			_cocks.Add(new Cock(creatureID, GetCockPerkData(), newCockType, length, girth, knotMultiplier));

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

		public int RemoveExtraCocks()
		{
			return RemoveCock(numCocks - 1);
		}

		public int RemoveAllCocks()
		{
			return RemoveCock(numCocks);
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

		public bool hasBalls => balls.hasBalls;
		public bool uniBall => balls.uniBall;

		public byte numberOfBalls => balls.count;
		public byte ballSize => balls.size;

		public bool GrowBalls()
		{
			return balls.growBalls();
		}

		public bool GrowBalls(byte numBalls, byte newSize = Balls.DEFAULT_BALLS_SIZE)
		{
			return balls.growBalls(numBalls, newSize);
		}

		public bool GrowUniBall()
		{
			return balls.growUniBall();
		}

		public bool ChangeBallsUniBall()
		{
			return balls.makeUniBall();
		}

		public bool GrowOrChangeUniBall()
		{
			if (hasBalls)
			{
				return ChangeBallsUniBall();
			}
			else
			{
				return GrowUniBall();
			}
		}

		public bool ChangeBallsNormal()
		{
			return balls.makeStandard();
		}

		public bool GrowOrChangeBallsNormal()
		{
			if (hasBalls)
			{
				return ChangeBallsNormal();
			}
			else
			{
				return GrowBalls();
			}
		}

		public byte AddBalls(byte addAmount)
		{
			return balls.addBalls(addAmount);
		}

		//
		public byte RemoveBalls(byte removeAmount)
		{
			return balls.removeBalls(removeAmount);
		}
		public bool RemoveAllBalls()
		{
			return balls.removeAllBalls();
		}

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
	}
}
