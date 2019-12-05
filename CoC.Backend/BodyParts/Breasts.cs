//Breasts.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:27 AM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.BodyParts
{
	public enum CupSize : byte
	{
		FLAT, A, B, C, D, DD, DD_BIG, E, E_BIG, EE,
		EE_BIG, F, F_BIG, FF, FF_BIG, G, G_BIG, GG, GG_BIG, H,
		H_BIG, HH, HH_BIG, HHH, I, I_BIG, II, II_BIG, J, J_BIG,
		JJ, JJ_BIG, K, K_BIG, KK, KK_BIG, L, L_BIG, LL, LL_BIG,
		M, M_BIG, MM, MM_BIG, MMM, MMM_LARGE, N, N_LARGE, NN, NN_LARGE,
		O, O_LARGE, OO, OO_LARGE, P, P_LARGE, PP, PP_LARGE, Q, Q_LARGE,
		QQ, QQ_LARGE, R, R_LARGE, RR, RR_LARGE, S, S_LARGE, SS, SS_LARGE,
		T, T_LARGE, TT, TT_LARGE, U, U_LARGE, UU, UU_LARGE, V, V_LARGE,
		VV, VV_LARGE, W, W_LARGE, WW, WW_LARGE, X, X_LARGE, XX, XX_LARGE,
		Y, Y_LARGE, YY, YY_LARGE, Z, Z_LARGE, ZZ, ZZ_LARGE, ZZZ, ZZZ_LARGE,
		HYPER_A, HYPER_B, HYPER_C, HYPER_D, HYPER_DD, HYPER_DD_BIG, HYPER_E, HYPER_E_BIG, HYPER_EE, HYPER_EE_BIG, //it was supposed to be 109 here, it's was only 108. idk man. i've moved the one up so it really is 109.
		HYPER_F, HYPER_F_BIG, HYPER_FF, HYPER_FF_BIG, HYPER_G, HYPER_G_BIG, HYPER_GG, HYPER_GG_BIG, HYPER_H, HYPER_H_BIG,
		HYPER_HH, HYPER_HH_BIG, HYPER_HHH, HYPER_I, HYPER_I_BIG, HYPER_II, HYPER_II_BIG, HYPER_J, HYPER_J_BIG, HYPER_JJ,
		HYPER_JJ_BIG, HYPER_K, HYPER_K_BIG, HYPER_KK, HYPER_KK_BIG, HYPER_L, HYPER_L_BIG, HYPER_LL, HYPER_LL_BIG, HYPER_M,
		HYPER_M_BIG, HYPER_MM, HYPER_MM_BIG, HYPER_MMM, HYPER_MMM_LARGE, HYPER_N, HYPER_N_LARGE, HYPER_NN, HYPER_NN_LARGE, HYPER_O,
		HYPER_O_LARGE, HYPER_OO, HYPER_OO_LARGE, HYPER_P, HYPER_P_LARGE, HYPER_PP, HYPER_PP_LARGE, HYPER_Q, HYPER_Q_LARGE, HYPER_QQ,
		HYPER_QQ_LARGE, HYPER_R, HYPER_R_LARGE, HYPER_RR, HYPER_RR_LARGE, HYPER_S, HYPER_S_LARGE, HYPER_SS, HYPER_SS_LARGE, HYPER_T,
		HYPER_T_LARGE, HYPER_TT, HYPER_TT_LARGE, HYPER_U, HYPER_U_LARGE, HYPER_UU, HYPER_UU_LARGE, HYPER_V, HYPER_V_LARGE, HYPER_VV,
		HYPER_VV_LARGE, HYPER_W, HYPER_W_LARGE, HYPER_WW, HYPER_WW_LARGE, HYPER_X, HYPER_X_LARGE, HYPER_XX, HYPER_XX_LARGE, HYPER_Y,
		HYPER_Y_LARGE, HYPER_YY, HYPER_YY_LARGE, HYPER_Z, HYPER_Z_LARGE, HYPER_ZZ, HYPER_ZZ_LARGE, HYPER_ZZZ, HYPER_ZZZ_LARGE, JACQUES00
	}

	//Note: Breasts aren't generated until after perks have been created. Thus, their post perk init is never called, but initial constructor can use perk data without fail.
	public sealed partial class Breasts : SimpleSaveablePart<Breasts, BreastData>, IGrowable, IShrinkable
	{
		public override string BodyPartName() => Name();

		private Creature creature => CreatureStore.GetCreatureClean(creatureID);

		public float lactationRate => creature?.genitals.lactationRate ?? 0;
		public LactationStatus lactationStatus => creature?.genitals.lactationStatus ?? LactationStatus.NOT_LACTATING;

		private Gender currGender => creature?.genitals.gender ?? Gender.MALE;
		private int currentBreastRow => creature?.genitals.breastRows.IndexOf(this) ?? 0;

		public int index => currentBreastRow;

		public const CupSize DEFAULT_MALE_SIZE = CupSize.FLAT;
		public const CupSize DEFAULT_FEMALE_SIZE = CupSize.C;

		public CupSize maleMinCup
		{
			get => _maleMinCup;
			internal set
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
		public CupSize femaleMinCup
		{
			get => _femaleMinCup;
			internal set
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

		public float lactationAmount(bool perBreast)
		{
			if (creature is null)
			{
				return 0;
			}
			else if (!perBreast)
			{
				return creature.genitals.currentLactationAmount;
			}
			else
			{
				return creature.genitals.currentLactationAmount / (creature.genitals.breastRows.Count * NUM_BREASTS);
			}
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
				Utils.ClampEnum(ref value, minimumCupSize, CupSize.JACQUES00); //enums: icomparable, but not really. woooo!
				if (_cupSize != value)
				{
					var oldData = AsReadOnlyData();
					_cupSize = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private CupSize _cupSize;
		public uint orgasmCount { get; private set; } = 0;
		public uint dryOrgasmCount { get; private set; } = 0;

		public uint titFuckCount { get; private set; } = 0;
		public uint dickNippleFuckCount => nipples.dickNippleFuckCount;
		public uint nippleFuckCount => nipples.nippleFuckCount;
		public uint nippleOrgasmCount => nipples.orgasmCount;
		public uint nippleDryOrgasmCount => nipples.dryOrgasmCount;

		public readonly Nipples nipples;
		public bool isMale => cupSize == CupSize.FLAT && nipples.length <= .5f;

		internal Breasts(Guid creatureID, BreastPerkHelper initialPerkData, Gender initialGender) : base(creatureID)
		{

			if (initialGender.HasFlag(Gender.FEMALE))
			{
				_cupSize = initialPerkData.FemaleNewLength();
			}
			else
			{
				_cupSize = initialPerkData.MaleNewLength();
			}
			nipples = new Nipples(creatureID, this, initialPerkData, initialGender);

			_maleMinCup = initialPerkData.MaleMinCup;
			_femaleMinCup = initialPerkData.FemaleMinCup;
			maleDefaultCup = initialPerkData.MaleNewDefaultCup;
			femaleDefaultCup = initialPerkData.FemaleNewDefaultCup;

			bigTiddyMultiplier = initialPerkData.TitsGrowthMultiplier;
			tinyTiddyMultiplier = initialPerkData.TitsShrinkMultiplier;


			CupSize min = currGender.HasFlag(Gender.FEMALE) ? femaleMinCup : maleMinCup;
			if (cupSize < min)
			{
				cupSize = min;
			}
		}
		internal Breasts(Guid creatureID, BreastPerkHelper initialPerkData, CupSize cup, float nippleLength) : base(creatureID)
		{
			nipples = new Nipples(creatureID, this, initialPerkData, nippleLength);
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
			if (creature != null) creature.genitals.onGenderChanged += OnGenderChanged;
		}

		public byte GrowBreasts(byte byAmount, bool ignorePerks = false)
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

		public byte ShrinkBreasts(byte byAmount, bool ignorePerks = false)
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

		public void SetCupSize(CupSize size)
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
			NotifyDataChanged(oldData);
			return true;
		}

		public void Reset(bool resetPiercings = false)
		{
			cupSize = currGender.HasFlag(Gender.FEMALE) ? femaleMinCup : maleMinCup;

			nipples.Reset(resetPiercings);
			//nippleFuckCount = 0;
			titFuckCount = 0;
			//dickNippleFuckCount = 0;
		}

		public static BreastData GenerateAggregate(Guid creatureID, CupSize averageCup, float averageNippleLength, bool blackNipples, bool quadNipples, NippleStatus nippleType,
			float lactationRate, LactationStatus lactationStatus)
		{
			return new BreastData(creatureID, averageCup, new NippleData(creatureID, averageNippleLength, -1, quadNipples, blackNipples, nippleType), -1, 1, lactationRate, lactationStatus);
		}

		internal override bool Validate(bool correctInvalidData)
		{
			cupSize = cupSize;
			return nipples.Validate(correctInvalidData);
		}

		#region Nipple Alias

		public float GrowNipple(float growAmount, bool ignorePerk = false)
		{
			var oldData = AsReadOnlyData();
			var retVal = nipples.GrowNipple(growAmount, ignorePerk);
			if (retVal != 0)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}

		public float ShrinkNipple(float shrinkAmount, bool ignorePerk = false)
		{
			var oldData = AsReadOnlyData();
			var retVal = nipples.ShrinkNipple(shrinkAmount, ignorePerk);
			if (retVal != 0)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}
		#endregion
		//to be frank, idk what would actually orgasm when being titty fucked, but, uhhhh... i guess it can be stored in stats or some shit?
		internal void DoTittyFuck(float length, float girth, float knotWidth, bool reachOrgasm)
		{
			titFuckCount++;
			if (reachOrgasm)
			{
				orgasmCount++;
			}
		}

		internal void OrgasmTits(bool dryOrgasm)
		{
			orgasmCount++;
			if (dryOrgasm) dryOrgasmCount++;
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
		public bool TittyFuckable()
		{
			return cupSize > CupSize.A && numBreasts > 1;
		}
	}

	public sealed partial class BreastData : SimpleData
	{
		public readonly NippleData nipples;
		public readonly CupSize cupSize;
		public readonly int currBreastRowIndex;

		public readonly float lactationRate;
		public readonly LactationStatus lactationStatus;

		public readonly byte numberOfBreasts;



		internal BreastData(Breasts breasts, int currentBreastRow) : base(breasts?.creatureID ?? throw new ArgumentNullException(nameof(breasts)))
		{
			cupSize = breasts.cupSize;
			nipples = breasts.nipples.AsReadOnlyData();

			currBreastRowIndex = currentBreastRow;
			numberOfBreasts = breasts.numBreasts;

			lactationStatus = breasts.lactationStatus;
			lactationRate = breasts.lactationRate;
		}

		internal BreastData(Guid creatureID, CupSize cupSize, NippleData nippleData, int currentBreastRow, byte breastCount, float lactationRate, LactationStatus lactationStatus) : base(creatureID)
		{
			this.cupSize = cupSize;
			nipples = nippleData;
			currBreastRowIndex = currentBreastRow;
			numberOfBreasts = breastCount;

			this.lactationRate = lactationRate;
			this.lactationStatus = lactationStatus;
		}

		internal BreastData(Breasts breasts, NippleData overrideNippleData, int currentBreastRow) : base(breasts?.creatureID ?? throw new ArgumentNullException(nameof(breasts)))
		{
			cupSize = breasts.cupSize;
			nipples = overrideNippleData ?? throw new ArgumentNullException(nameof(overrideNippleData));

			numberOfBreasts = breasts.numBreasts;

			currBreastRowIndex = currentBreastRow;

			lactationStatus = breasts.lactationStatus;
			lactationRate = breasts.lactationRate;
		}
	}
}
