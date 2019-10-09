using System;
using System.Linq;

namespace CoC.Backend.BodyParts
{
	public partial class Genitals
	{
		private uint missingRowTitFuckCount = 0;
		private uint missingRowBreastOrgasmCount = 0;
		private uint missingRowBreastDryOrgasmCount = 0;

		private uint missingRowNippleFuckCount = 0;
		private uint missingRowDickNippleSexCount = 0;
		private uint missingRowNippleOrgasmCount = 0;
		private uint missingRowNippleDryOrgasmCount = 0;


		public uint titFuckCount => missingRowTitFuckCount + (uint)breastRows.Sum(x => x.titFuckCount);
		public uint nippleFuckCount => missingRowNippleFuckCount + (uint)breastRows.Sum(x => x.nippleFuckCount);
		public uint dickNippleSexCount => missingRowDickNippleSexCount + (uint)breastRows.Sum(x => x.dickNippleFuckCount);

		public uint nippleOrgasmCount => missingRowNippleOrgasmCount.add((uint)breastRows.Sum(x => x.nippleOrgasmCount));
		public uint nippleDryOrgasmCount => missingRowNippleDryOrgasmCount.add((uint)breastRows.Sum(x => x.nippleDryOrgasmCount));
		public uint breastOrgasmCount => missingRowBreastOrgasmCount.add((uint)breastRows.Sum(x => x.orgasmCount));
		public uint breastDryOrgasmCount => missingRowBreastDryOrgasmCount.add((uint)breastRows.Sum(x => x.dryOrgasmCount));

		

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

		internal void HandleNipplePenetration(int breastIndex, Cock sourceCock, bool reachOrgasm)
		{
			HandleNipplePenetration(breastIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, sourceCock.cumAmount, reachOrgasm);
		}

		internal void HandleNipplePenetration(int breastIndex, Cock sourceCock, float cumAmountOverride, bool reachOrgasm)
		{
			HandleNipplePenetration(breastIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, cumAmountOverride, reachOrgasm);
		}

		internal void HandleNipplePenetration(int breastIndex, float length, float girth, float knotWidth, float cumAmount, bool reachOrgasm)
		{
			Nipples nipple = _breasts[breastIndex].nipples;
			nipple.DoNippleFuck(length, girth, knotWidth, cumAmount, reachOrgasm);
		}

		internal void HandleNippleDickPenetrate(int breastIndex, bool reachOrgasm)
		{
			Nipples nipple = _breasts[breastIndex].nipples;
			nipple.DoDickNippleSex(reachOrgasm);
		}

		internal void HandleNippleOrgasmGeneric(int breastIndex, bool dryOrgasm)
		{
			_breasts[breastIndex].nipples.OrgasmNipplesGeneric(dryOrgasm);
		}
	}
}
