using CoC.Backend.BodyParts.SpecialInteraction;
using System.Linq;

namespace CoC.Backend.BodyParts
{

	public partial class Genitals
	{
		#region Public Nipple Related Members
		public bool blackNipples
		{
			get => _blackNipples;
			private set
			{
				_blackNipples = value;
			}
		}
		private bool _blackNipples;
		public bool quadNipples
		{
			get => _quadNipples;
			private set
			{
				_quadNipples = value;
			}
		}
		private bool _quadNipples;
		public NippleStatus nippleType
		{
			get => _nippleType;
			private set
			{
				_nippleType = value;
			}
		}
		private NippleStatus _nippleType;

		public bool unlockedDickNipples
		{
			get => _unlockedDickNipples;
			private set => _unlockedDickNipples = value;
		}
		private bool _unlockedDickNipples = false;

		#endregion

		#region Private Nipple Related Members

		private uint missingRowNippleFuckCount = 0;
		private uint missingRowDickNippleSexCount = 0;
		private uint missingRowNippleOrgasmCount = 0;
		private uint missingRowNippleDryOrgasmCount = 0;


		#endregion

		#region Public Nipple Related Computed Properties
		public int nippleCount => _breasts.Count * Breasts.NUM_BREASTS * (quadNipples ? 4 : 1);


		public uint nippleFuckCount => missingRowNippleFuckCount + (uint)breastRows.Sum(x => x.nippleFuckCount);
		public uint dickNippleSexCount => missingRowDickNippleSexCount + (uint)breastRows.Sum(x => x.dickNippleFuckCount);

		public uint nippleOrgasmCount => missingRowNippleOrgasmCount.add((uint)breastRows.Sum(x => x.nippleOrgasmCount));
		public uint nippleDryOrgasmCount => missingRowNippleDryOrgasmCount.add((uint)breastRows.Sum(x => x.nippleDryOrgasmCount));

		#endregion

		#region Nipple Aggregate Functions

		public float LargestNippleSize()
		{
			if (nipples.IsEmpty())
			{
				return 0;
			}
			return nipples.Max(x => x.length);
		}

		public Nipples LargestNipples()
		{
			if (nipples.IsEmpty())
			{
				return null;
			}
			return nipples.MaxItem(x => x.length);
		}

		public float SmallestNippleSize()
		{
			if (nipples.IsEmpty())
			{
				return 0;
			}
			return nipples.Min(x => x.length);
		}

		public Nipples SmallestNipples()
		{
			if (nipples.IsEmpty())
			{
				return null;
			}
			return nipples.MinItem(x => x.length);
		}

		public float AverageNippleSize()
		{
			if (nipples.IsEmpty())
			{
				return 0;
			}
			return nipples.Average(x => x.length);
		}

		public NippleData AverageNipple()
		{
			if (nipples.IsEmpty())
			{
				return null;
			}

			return new NippleData(creatureID, AverageNippleSize(), -1, lactationRate, quadNipples, blackNipples, nippleType, null, relativeLust);
		}

		#endregion

		#region Nipple Mutators
		public void SetQuadNipples(bool active)
		{
			quadNipples = active;
		}

		public void SetBlackNipples(bool active)
		{
			blackNipples = active;
		}

		#endregion

		#region Nipple Sex Related Functions

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

		#endregion

#warning Add a means of preventing lactation decrease due to pregnancy that doesn't require pregnancy set the perk value.
	}

	public partial class GenitalsData
	{
		#region Public Nipple Related Members
		public readonly bool blackNipples;
		public readonly bool quadNipples;

		public readonly NippleStatus nippleType;

		public readonly bool unlockedDickNipples;
		#endregion

		public int nippleCount => numBreasts * (quadNipples ? 4 : 1);

		public readonly uint nippleFuckCount;
		public readonly uint dickNippleSexCount;

		public readonly uint nippleOrgasmCount;
		public readonly uint nippleDryOrgasmCount;

		#region Nipple Aggregate Functions

		public float LargestNippleSize()
		{
			if (nipples.IsEmpty())
			{
				return 0;
			}
			return nipples.Max(x => x.length);
		}

		public NippleData LargestNipples()
		{
			if (nipples.IsEmpty())
			{
				return null;
			}
			return nipples.MaxItem(x => x.length);
		}

		public float SmallestNippleSize()
		{
			if (nipples.IsEmpty())
			{
				return 0;
			}
			return nipples.Min(x => x.length);
		}

		public NippleData SmallestNipples()
		{
			if (nipples.IsEmpty())
			{
				return null;
			}
			return nipples.MinItem(x => x.length);
		}

		public float AverageNippleSize()
		{
			if (nipples.IsEmpty())
			{
				return 0;
			}
			return nipples.Average(x => x.length);
		}

		public NippleData AverageNipple()
		{
			if (nipples.IsEmpty())
			{
				return null;
			}

			return new NippleData(creatureID, AverageNippleSize(), -1, lactationRate, quadNipples, blackNipples, nippleType, null, relativeLust);
		}

		#endregion
	}
}
