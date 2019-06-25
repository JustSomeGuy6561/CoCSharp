//Breasts.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:27 AM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;


namespace CoC.Backend.BodyParts
{

	public sealed class Breasts : SimpleSaveablePart<Breasts>, IGrowShrinkable //IPerkAware
	{

		#warning replace this later when a perk system is in place
		private bool hasBigTitPerk => BackendSessionData.data.hasBigTitPerk;

		public CupSize cupSize
		{
			get => _cupSize;
			private set
			{
				//weird workaround because enums aren't actually icomparabale. they do allow < or > though. idk.
				//force the value to be valid. only really applies with enum arithmatic.
				byte val = Utils.Clamp2((byte)_cupSize, (byte)CupSize.FLAT, (byte)CupSize.JACQUES00);
				_cupSize = (CupSize)val;
			}
		}
		private CupSize _cupSize;

		public readonly Nipples nipples;
		public bool isMale => cupSize == CupSize.FLAT && nipples.length <= .5f;

		private Breasts(bool female)
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
		private Breasts(CupSize cup, float nippleLength)
		{
			nipples = Nipples.GenerateWithLength(nippleLength);
			cupSize = cup;
		}

		internal static Breasts GenerateFromGender(Gender gender)
		{
			return new Breasts(gender.HasFlag(Gender.FEMALE));
		}

		internal static Breasts Generate(CupSize cup, float nippleLength)
		{
			return new Breasts(cup, nippleLength);
		}

		internal byte GrowBreasts(byte byAmount)
		{
			if (cupSize >= CupSize.JACQUES00)
			{
				return 0;
			}
			Utils.Clamp<byte>(ref byAmount, 0, byte.MaxValue);
			CupSize oldSize = cupSize;
			cupSize += byAmount;
			return cupSize - oldSize;
		}

		internal byte ShrinkBreasts(byte byAmount)
		{
			if (cupSize <= CupSize.FLAT)
			{
				return 0;
			}
			Utils.Clamp<byte>(ref byAmount, 0, byte.MaxValue);
			CupSize oldSize = cupSize;
			cupSize -= byAmount;
			return cupSize - oldSize;
		}

		internal void setCupSize(CupSize size)
		{
			byte val = Utils.Clamp2((byte)size, (byte)CupSize.FLAT, (byte)CupSize.JACQUES00);
			cupSize = size;
		}

		bool IGrowShrinkable.CanGrowPlus()
		{
			return cupSize < CupSize.JACQUES00;
		}

		bool IGrowShrinkable.CanReducto()
		{
			return cupSize > CupSize.FLAT;
		}

		float IGrowShrinkable.UseGroPlus()
		{
			if (!((IGrowShrinkable)this).CanGrowPlus())
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			this.cupSize += (byte)(Utils.Rand(2) + 1);
			//c# is a bitch in that all numbers are treated as ints or doubles unless explicitly cast - byte me
			this.cupSize += (byte)(hasBigTitPerk && Utils.RandBool() ? 1 : 0); //add one for big tits perk 50% of the time
			return cupSize - oldSize;
		}

		float IGrowShrinkable.UseReducto()
		{
			if (!((IGrowShrinkable)this).CanReducto())
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			cupSize -= (byte)(!hasBigTitPerk && Utils.RandBool() ? 2 : 1); //if big tits perk: -1. otherwise -1 or -2, 50% split.
			return oldSize - cupSize;
		}

		public bool MakeMale(bool removeStatus = true)
		{
			if (isMale)
			{
				return false;
			}
			cupSize = CupSize.FLAT;
			nipples.ShrinkNipple(nipples.length - Nipples.MIN_NIPPLE_LENGTH);
			if (removeStatus)
			{
				nipples.setNippleStatus(NippleStatus.NORMAL);
			}
			return true;
		}

		public bool MakeFemale(bool removeStatus = true)
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
				nipples.setNippleStatus(NippleStatus.NORMAL);
			}
			return true;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			cupSize = cupSize;
			return nipples.Validate(correctInvalidData);
		}
	}
}
