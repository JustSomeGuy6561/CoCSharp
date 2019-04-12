//Breasts.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:27 AM
using CoC.Tools;
using  CoC.BodyParts.SpecialInteraction;
using System.Runtime.Serialization;
using CoC.Serialization;

namespace  CoC.BodyParts
{

#warning Consider moving black nipples and quad nipples here. 
	//they need to persist when i reset the nipples.
	//black nipples were a "perk/status effect"
	//quads behavior varies.
	[DataContract]
	internal class Breasts : IGrowShrinkable
	{
		[Save]
		public CupSize cupSize
		{
			get => _cupSize;
			protected set
			{
				//weird workaround because enums aren't actually icomparabale. they do allow < or > though. idk.
				int val = value.asInt();
				//force the value to be valid. only really applies with enum arithmatic.
				Utils.Clamp(ref val, CupSize.FLAT.asInt(), CupSize.JACQUES00.asInt());
				_cupSize = (CupSize)val;
			}
		}
		private CupSize _cupSize;
		[Save]
		public Nipples nipples;
		private readonly bool hasBigTitsPerk;
		public bool isMale => cupSize == CupSize.FLAT && nipples.length <= .5f;

		protected Breasts(bool female)
		{
			if (female)
			{
				nipples = Nipples.GenerateWithLength(0.5f);
				cupSize = CupSize.C;
			}
			else
			{
				nipples = Nipples.Generate();
				cupSize = CupSize.FLAT;
			}
		}
		protected Breasts(CupSize cup, float nippleLength)
		{
			nipples = Nipples.GenerateWithLength(nippleLength);
			cupSize = cup;
		}
		public static Breasts GenerateFemale()
		{
			return new Breasts(true);
		}

		public static Breasts GenerateFemale(CupSize cup, float nippleLength)
		{
			return new Breasts(cup, nippleLength);
		}
		public static Breasts GenerateMale()
		{
			return new Breasts(false);
		}

		public int GrowBreasts(uint byAmount)
		{
			if (cupSize >= CupSize.JACQUES00)
			{
				return 0;
			}
			Utils.Clamp<uint>(ref byAmount, 0, int.MaxValue);
			int amount = (int)byAmount;
			CupSize oldSize = cupSize;
			cupSize += amount;
			return cupSize - oldSize;
		}

		public int ShrinkBreasts(uint byAmount)
		{
			if (cupSize <= CupSize.FLAT)
			{
				return 0;
			}
			Utils.Clamp<uint>(ref byAmount, 0, int.MaxValue);
			int amount = (int)byAmount;
			CupSize oldSize = cupSize;
			cupSize -= amount;
			return cupSize - oldSize;
		}

		public bool CanGrowPlus()
		{
			return cupSize < CupSize.JACQUES00;
		}

		public bool CanReducto()
		{
			return cupSize > CupSize.FLAT;
		}

		public float UseGroPlus()
		{
			if (!CanGrowPlus())
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			this.cupSize += (Utils.Rand(2) + 1);
			this.cupSize += (hasBigTitsPerk && Utils.RandBool()) ? 1 : 0; //add one for big tits perk 50% of the time
			return cupSize - oldSize;
		}

		public float UseReducto()
		{
			if (!CanReducto())
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			cupSize -= (!hasBigTitsPerk && Utils.RandBool()) ? 2 : 1; //if big tits perk: -1. otherwise -1 or -2, 50% split.
			return oldSize - cupSize;
		}

		public bool makeMale(bool removeStatus = true)
		{
			if (isMale)
			{
				return false;
			}
			cupSize = CupSize.FLAT;
			nipples.ShrinkNipple(nipples.length - Nipples.MIN_NIPPLE_LENGTH);
			if (removeStatus)
			{
				nipples.ForceNippleStatus(NippleStatus.NORMAL);
				nipples.DeactivateQuadNipples();
			}
			return true;
		}

		public bool makeFemale(bool removeStatus = true)
		{
			if (!isMale)
			{
				return false;
			}
			if (cupSize == CupSize.FLAT)
			{
				cupSize = CupSize.B;
			}
			if (nipples.length < 0.5)
			{
				nipples.GrowNipple(0.5f - nipples.length);
			}
			if (removeStatus)
			{
				nipples.ForceNippleStatus(NippleStatus.NORMAL);
			}
			return true;
		}
	}
}
