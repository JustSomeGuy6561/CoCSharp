//Breasts.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:27 AM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.BodyParts
{
	//Note: Breasts aren't generated until after perks have been created. Thus, their post perk init is never called, but initial constructor can use perk data without fail.

	public sealed class Breasts : SimpleSaveablePart<Breasts, BreastData>, IGrowable, IShrinkable
	{
		private Gender currGender => source.genitals.gender;

		public const CupSize DEFAULT_MALE_SIZE = CupSize.FLAT;
		public const CupSize DEFAULT_FEMALE_SIZE = CupSize.C;

		internal CupSize maleMinCup;
		internal CupSize femaleMinCup;
		internal CupSize maleDefaultCup;
		internal CupSize femaleDefaultCup;

		private CupSize resetSize => currGender.HasFlag(Gender.FEMALE) ? EnumHelper.Min(femaleMinCup, femaleDefaultCup) : EnumHelper.Min(maleMinCup, maleDefaultCup);
		private CupSize minimumCupSize => currGender.HasFlag(Gender.FEMALE) ? femaleMinCup : maleMinCup;

		internal float bigTiddyMultiplier = 1;
		internal float tinyTiddyMultiplier = 1;
		private bool makeBigTits => bigTiddyMultiplier > 1.1f;
		private bool makeSmallTits => tinyTiddyMultiplier > 1.1f;

		public float lactationMultiplier { get; private set; }

		internal float SetLactation(float lactationAmount)
		{
			lactationMultiplier = lactationAmount;
			return lactationMultiplier;
		}


		public const byte NUM_BREASTS = 2;
		public byte numBreasts => NUM_BREASTS;

		public override BreastData AsReadOnlyData()
		{
			return new BreastData(this);
		}

		public CupSize cupSize
		{
			get => _cupSize;
			private set
			{
				_cupSize = Utils.ClampEnum2(value, CupSize.FLAT, CupSize.JACQUES00); //enums: icomparable, but not really. woooo!
			}
		}
		private CupSize _cupSize;
		public readonly Nipples nipples;
		public bool isMale => cupSize == CupSize.FLAT && nipples.length <= .5f;

		internal Breasts(Creature source, BreastPerkHelper initialPerkData, Gender initialGender) : base(source)
		{

			if (initialGender.HasFlag(Gender.FEMALE))
			{
				cupSize = initialPerkData.FemaleNewLength();
			}
			else
			{
				cupSize = initialPerkData.MaleNewLength();
			}
			nipples = new Nipples(source, initialPerkData, initialGender);

			maleMinCup = initialPerkData.MaleMinCup;
			femaleMinCup = initialPerkData.FemaleMinCup;
			maleDefaultCup = initialPerkData.MaleNewDefaultCup;
			femaleDefaultCup = initialPerkData.FemaleNewDefaultCup;

			bigTiddyMultiplier = initialPerkData.TitsGrowthMultiplier;
			tinyTiddyMultiplier = initialPerkData.TitsShrinkMultiplier;

			source.genitals.onGenderChanged += OnGenderChanged;
		}
		internal Breasts(Creature source, BreastPerkHelper initialPerkData, CupSize cup, float nippleLength) : base(source)
		{
			nipples = new Nipples(source, initialPerkData, nippleLength);
			cupSize = cup;

			maleMinCup = initialPerkData.MaleMinCup;
			femaleMinCup = initialPerkData.FemaleMinCup;
			maleDefaultCup = initialPerkData.MaleNewDefaultCup;
			femaleDefaultCup = initialPerkData.FemaleNewDefaultCup;

			bigTiddyMultiplier = initialPerkData.TitsGrowthMultiplier;
			tinyTiddyMultiplier = initialPerkData.TitsShrinkMultiplier;

			source.genitals.onGenderChanged += OnGenderChanged;
		}

		private void OnGenderChanged(object sender, EventHelpers.GenderChangedEventArgs e)
		{
			CupSize min = e.newGender.HasFlag(Gender.FEMALE) ? femaleMinCup : maleMinCup;
			if (cupSize < min)
			{
				cupSize = min;
			}
		}

		protected internal override void LateInit()
		{
			nipples.LateInit();
		}

		internal byte GrowBreasts(byte byAmount, bool ignorePerks = false)
		{
			if (cupSize >= CupSize.JACQUES00)
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			if (!ignorePerks)
			{
				ushort val = (ushort)Math.Round(byAmount * bigTiddyMultiplier);
				byAmount = val > byte.MaxValue ? byte.MaxValue : (byte)val;
			}
			cupSize = cupSize.ByteEnumAdd(byAmount);
			return cupSize - oldSize;
		}

		internal byte ShrinkBreasts(byte byAmount, bool ignorePerks = false)
		{
			if (cupSize <= CupSize.FLAT)
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			if (!ignorePerks)
			{
				ushort val = (ushort)Math.Round(byAmount * tinyTiddyMultiplier);
				byAmount = val > byte.MaxValue ? byte.MaxValue : (byte)val;
			}
			cupSize = cupSize.ByteEnumSubtract(byAmount);
			return cupSize - oldSize;
		}

		internal void setCupSize(CupSize size)
		{
			cupSize = Utils.ClampEnum2(size, CupSize.FLAT, CupSize.JACQUES00);
		}
		public bool MakeMale(bool removeStatus = true)
		{
			if (isMale)
			{
				return false;
			}
			cupSize = maleMinCup;
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
			if (cupSize < femaleMinCup)
			{
				cupSize = femaleMinCup;
			}
			else if (cupSize == CupSize.FLAT)
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

		#region IGrowShrinkable
		bool IGrowable.CanGroPlus()
		{
			return cupSize < CupSize.JACQUES00;
		}

		bool IShrinkable.CanReducto()
		{
			return cupSize > minimumCupSize;
		}

		float IGrowable.UseGroPlus()
		{
			if (!((IGrowable)this).CanGroPlus())
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			this.cupSize += (byte)(Utils.Rand(2) + 1);
			//c# is a bitch in that all numbers are treated as ints or doubles unless explicitly cast - byte me
			this.cupSize += (byte)(makeBigTits && Utils.RandBool() ? 1 : 0); //add one for big tits perk 50% of the time
			return cupSize - oldSize;
		}

		float IShrinkable.UseReducto()
		{
			if (!((IShrinkable)this).CanReducto())
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			if (cupSize.ByteEnumSubtract(1) == minimumCupSize || !makeSmallTits || Utils.RandBool())
			{
				cupSize--;
			}
			else
			{
				cupSize -= 2;
			}
			return oldSize - cupSize;
		}
		#endregion
	}

	public sealed class BreastData
	{
		public readonly NippleData nipples;
		public readonly CupSize cupSize;
		public readonly float lactationAmount;

		internal BreastData(Breasts breasts)
		{
			if (breasts is null) throw new ArgumentNullException(nameof(breasts));
			cupSize = breasts.cupSize;
			nipples = breasts.nipples.AsReadOnlyData();
			lactationAmount = breasts.lactationMultiplier;
		}
	}
}
