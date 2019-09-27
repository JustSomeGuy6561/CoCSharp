using System;
using System.Linq;

namespace CoC.Backend.BodyParts
{
	public partial class Genitals
	{
		public uint timesTitFucked => missingRowTitFuckCount + (uint)breastRows.Sum(x => x.titFuckCount);
		public uint timesNippleFucked => missingRowNippleFuckCount + (uint)breastRows.Sum(x => x.nippleFuckCount);
		public uint timesDickNippleFucked => missingRowDickNippleSexCount + (uint)breastRows.Sum(x => x.dickNippleFuckCount);

		private uint missingRowTitFuckCount = 0;
		private uint missingRowNippleFuckCount = 0;
		private uint missingRowDickNippleSexCount = 0;

		internal CupSize BiggestCupSize()
		{
			return (CupSize)_breasts.Max(x => (byte?)x?.cupSize);
		}

		internal CupSize AverageCupSize()
		{
			return (CupSize)(byte)Math.Ceiling(_breasts.Average(x => (double)x.cupSize));
		}

		internal CupSize SmallestCupSize()
		{
			return (CupSize)_breasts.Min(x => (byte?)x?.cupSize);
		}

		internal Breasts LargestBreast()
		{
			return _breasts.MaxItem(x => (byte)x.cupSize);
		}

		internal Breasts SmallestBreast()
		{
			return  _breasts.MinItem(x => (byte)x.cupSize);
		}

		internal bool AddBreastRow()
		{
			if (numBreastRows >= MAX_BREAST_ROWS)
			{
				return false;
			}

			var cup = _breasts[_breasts.Count - 1].cupSize;
			var length = _breasts[_breasts.Count-1].nipples.length;
			_breasts.Add(new Breasts(creatureID, GetBreastPerkData(), cup, length));
			return true;
		}
		internal bool AddBreastRowAverage()
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
			_breasts.Add(new Breasts(creatureID, GetBreastPerkData(), (CupSize)cup, (float)avgLength));
			return true;
		}

		internal bool AddBreastRow(CupSize cup)
		{
			if (numBreastRows >= MAX_BREAST_ROWS)
			{
				return false;
			}
			double avgLength = _breasts.Average((x) => (double)x.nipples.length);
			_breasts.Add(new Breasts(creatureID, GetBreastPerkData(), cup, (float)avgLength));
			return true;
		}

		internal int RemoveBreastRow(int count = 1)
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
				missingRowNippleFuckCount += _breasts[0].nippleFuckCount;
				missingRowDickNippleSexCount += _breasts[0].dickNippleFuckCount;

				_breasts[0].Reset();
				count = numBreastRows - 1;
			}
			
			missingRowTitFuckCount += (uint)breastRows.Skip(numBreastRows - count).Sum(x => x.titFuckCount);
			missingRowNippleFuckCount += (uint)breastRows.Skip(numBreastRows - count).Sum(x => x.nippleFuckCount);
			missingRowDickNippleSexCount += (uint)breastRows.Skip(numBreastRows - count).Sum(x => x.dickNippleFuckCount);
			_breasts.RemoveRange(numBreastRows - count, count);

			return oldCount - numBreastRows;
		}

		internal int RemoveExtraBreastRows()
		{
			return RemoveBreastRow(numBreastRows - 1);
		}

		/// <summary>
		/// Evens out all breast rows so they are closer to the average nipple length and cup size, rounding up.
		/// large ones are shrunk, small ones grow. only does one unit of change, unless until even is set, then
		/// will completely average all values.
		/// </summary>
		/// <param name="untilEven">if true, forces all breast rows to average value, if false, only one unit.</param>
		internal void NormalizeBreasts(bool untilEven = false)
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
					row.setCupSize(averageSize);
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

		internal void HandleNipplePenetration(int breastIndex, Cock sourceCock, bool reachOrgasm)
		{
			HandleNipplePenetration(breastIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, reachOrgasm);
		}

		internal void HandleNipplePenetration(int breastIndex, float length, float girth, float knotWidth, bool reachOrgasm)
		{
			Nipples nipple = _breasts[breastIndex].nipples;
			nipple.DoNippleFuck(length, girth, knotWidth, reachOrgasm);
		}

		internal void HandleNippleDickPenetrate(int breastIndex, bool reachesOrgasm)
		{
			Nipples nipple = _breasts[breastIndex].nipples;
			nipple.DoDickNippleSex(reachesOrgasm);
		}
	}
}
