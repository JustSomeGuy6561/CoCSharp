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
		private int currentBreastRow => source.genitals.breasts.IndexOf(this);

		public const CupSize DEFAULT_MALE_SIZE = CupSize.FLAT;
		public const CupSize DEFAULT_FEMALE_SIZE = CupSize.C;

		internal CupSize maleMinCup
		{
			get => _maleMinCup;
			set
			{
				_maleMinCup = value;
				if (cupSize < minimumCupSize)
				{
					var oldValue = AsReadOnlyData();
					cupSize = minimumCupSize;
					NotifyDataChanged(oldValue);
				}
			}
		}
		private CupSize _maleMinCup = CupSize.FLAT;
		internal CupSize femaleMinCup
		{
			get => _femaleMinCup;
			set
			{
				_femaleMinCup = value;
				if (cupSize < minimumCupSize)
				{
					var oldValue = AsReadOnlyData();
					cupSize = minimumCupSize;
					NotifyDataChanged(oldValue);
				}
			}
		}
		private CupSize _femaleMinCup = CupSize.C;

		internal CupSize maleDefaultCup;
		internal CupSize femaleDefaultCup;

		private CupSize resetSize => currGender.HasFlag(Gender.FEMALE) ? EnumHelper.Min(femaleMinCup, femaleDefaultCup) : EnumHelper.Min(maleMinCup, maleDefaultCup);
		private CupSize minimumCupSize => currGender.HasFlag(Gender.FEMALE) ? femaleMinCup : maleMinCup;

		internal float bigTiddyMultiplier = 1;
		internal float tinyTiddyMultiplier = 1;
		private bool makeBigTits => bigTiddyMultiplier > 1.1f;
		private bool makeSmallTits => tinyTiddyMultiplier > 1.1f;

		public float lactationMultiplier
		{
			get => _lactationMultiplier;
			private set
			{
				if (_lactationMultiplier != value)
				{
					_lactationMultiplier = value;
				}
			}
		}
		private float _lactationMultiplier;

		internal float SetLactation(float lactationAmount)
		{
			if (lactationAmount != lactationMultiplier)
			{
				var oldData = AsReadOnlyData();
				lactationMultiplier = lactationAmount;
				NotifyDataChanged(oldData);
			}
			return lactationMultiplier;
		}


		public const byte NUM_BREASTS = 2;
		public byte numBreasts => NUM_BREASTS;

		public override BreastData AsReadOnlyData()
		{
			return new BreastData(this, currentBreastRow);
		}

		public CupSize cupSize
		{
			get => _cupSize;
			private set
			{
				Utils.ClampEnum(ref value, CupSize.FLAT, CupSize.JACQUES00); //enums: icomparable, but not really. woooo!
				if (_cupSize != value)
				{
					var oldData = AsReadOnlyData();
					_cupSize = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private CupSize _cupSize;
		public readonly Nipples nipples;
		public bool isMale => cupSize == CupSize.FLAT && nipples.length <= .5f;

		internal Breasts(Creature source, BreastPerkHelper initialPerkData, Gender initialGender) : base(source)
		{

			if (initialGender.HasFlag(Gender.FEMALE))
			{
				_cupSize = initialPerkData.FemaleNewLength();
			}
			else
			{
				_cupSize = initialPerkData.MaleNewLength();
			}
			nipples = new Nipples(source, this, initialPerkData, initialGender);

			_maleMinCup = initialPerkData.MaleMinCup;
			_femaleMinCup = initialPerkData.FemaleMinCup;
			maleDefaultCup = initialPerkData.MaleNewDefaultCup;
			femaleDefaultCup = initialPerkData.FemaleNewDefaultCup;

			bigTiddyMultiplier = initialPerkData.TitsGrowthMultiplier;
			tinyTiddyMultiplier = initialPerkData.TitsShrinkMultiplier;

			source.genitals.onGenderChanged += OnGenderChanged;

			CupSize min = currGender.HasFlag(Gender.FEMALE) ? femaleMinCup : maleMinCup;
			if (cupSize < min)
			{
				cupSize = min;
			}
		}
		internal Breasts(Creature source, BreastPerkHelper initialPerkData, CupSize cup, float nippleLength) : base(source)
		{
			nipples = new Nipples(source, this, initialPerkData, nippleLength);
			_cupSize = Utils.ClampEnum2(cup, CupSize.FLAT, CupSize.JACQUES00);

			_maleMinCup = initialPerkData.MaleMinCup;
			_femaleMinCup = initialPerkData.FemaleMinCup;
			maleDefaultCup = initialPerkData.MaleNewDefaultCup;
			femaleDefaultCup = initialPerkData.FemaleNewDefaultCup;

			bigTiddyMultiplier = initialPerkData.TitsGrowthMultiplier;
			tinyTiddyMultiplier = initialPerkData.TitsShrinkMultiplier;

		}

		private void OnGenderChanged(object sender, EventHelpers.GenderChangedEventArgs e)
		{
			CupSize min = e.newGender.HasFlag(Gender.FEMALE) ? femaleMinCup : maleMinCup;
			if (cupSize < min)
			{
				cupSize = min;
			}
		}

		protected internal override void PostPerkInit()
		{
			nipples.PostPerkInit();
		}

		protected internal override void LateInit()
		{
			nipples.LateInit();
			source.genitals.onGenderChanged += OnGenderChanged;
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

			var oldData = AsReadOnlyData();
			cupSize = cupSize.ByteEnumAdd(byAmount);
			if (cupSize != oldSize)
			{
				NotifyDataChanged(oldData);
			}
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
			var oldData = AsReadOnlyData();

			cupSize = cupSize.ByteEnumSubtract(byAmount);
			if (cupSize != oldSize)
			{
				NotifyDataChanged(oldData);
			}
			return cupSize - oldSize;
		}

		internal void setCupSize(CupSize size)
		{

			Utils.ClampEnum(ref size, CupSize.FLAT, CupSize.JACQUES00);
			if (cupSize != size)
			{
				var oldData = AsReadOnlyData();
				cupSize = size;
				NotifyDataChanged(oldData);
			}
		}
		public bool MakeMale(bool removeStatus = true)
		{
			if (isMale)
			{
				return false;
			}
			var oldData = AsReadOnlyData();

			cupSize = maleMinCup;
			nipples.ShrinkNipple(nipples.length - Nipples.MIN_NIPPLE_LENGTH);
			if (removeStatus)
			{
				nipples.setNippleStatus(NippleStatus.NORMAL);
				lactationMultiplier = 0f;
			}
			NotifyDataChanged(oldData);
			return true;
		}

		public bool MakeFemale(bool removeStatus = true)
		{
			if (!isMale)
			{
				return false;
			}
			var oldData = AsReadOnlyData();
			if (cupSize < femaleMinCup)
			{
				cupSize = femaleMinCup;
			}
			else if (cupSize == CupSize.FLAT)
			{
				cupSize = CupSize.B;
			}

			//nipple data change 
			if (nipples.length < 0.5)
			{
				nipples.GrowNipple(0.5f - nipples.length);
			}
			if (removeStatus)
			{
				nipples.setNippleStatus(NippleStatus.NORMAL);
			}
			NotifyDataChanged(oldData);
			return true;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			cupSize = cupSize;
			return nipples.Validate(correctInvalidData);
		}

		#region Nipple Alias

		internal float GrowNipple(float growAmount, bool ignorePerk = false)
		{
			var oldData = AsReadOnlyData();
			var retVal = nipples.GrowNipple(growAmount, ignorePerk);
			if (retVal != 0)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}

		internal float ShrinkNipple(float shrinkAmount, bool ignorePerk = false)
		{
			var oldData = AsReadOnlyData();
			var retVal = nipples.ShrinkNipple(shrinkAmount, ignorePerk);
			if (retVal != 0)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}

		internal bool setQuadNipple(bool active)
		{
			var oldData = AsReadOnlyData();
			var retVal = nipples.setQuadNipple(active);
			if (retVal)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}

		internal bool setBlackNipple(bool active)
		{
			var oldData = AsReadOnlyData();
			var retVal = nipples.setBlackNipple(active);
			if (retVal)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}

		internal bool setNippleStatus(NippleStatus status)
		{
			var oldData = AsReadOnlyData();
			var retVal = nipples.setNippleStatus(status);
			if (retVal)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}

		#endregion
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
		public readonly int currBreastRowIndex;
		internal BreastData(Breasts breasts, int currentBreastRow)
		{
			if (breasts is null) throw new ArgumentNullException(nameof(breasts));
			cupSize = breasts.cupSize;
			nipples = breasts.nipples.AsReadOnlyData();
			lactationAmount = breasts.lactationMultiplier;

			currBreastRowIndex = currentBreastRow;
		}

		internal BreastData(Breasts breasts, NippleData overrideNippleData, int currentBreastRow)
		{
			if (breasts is null) throw new ArgumentNullException(nameof(breasts));
			cupSize = breasts.cupSize;
			nipples = overrideNippleData ?? throw new ArgumentNullException(nameof(overrideNippleData));
			lactationAmount = breasts.lactationMultiplier;

			currBreastRowIndex = currentBreastRow;
		}
	}
}
