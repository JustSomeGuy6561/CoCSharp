using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;

//most of these are simply bytes, though a few do have extra behavior. An common software engineering practice is to never use primitives directly - this can be
//confusing or arbitrary - 5 could mean 5 years, 5 decades, 5 score, 5 centuries, etc. While i don't agree with that assessment 100%, it sometimes has merit. 

//i'm not 100% familiar with C#'s optimizations, though it may align objects to word (4 byte) boundaries, which would mean these could all use ints instead, but w/e.
//though i suppose with even remotely modern hardware (read: it runs windows XP+) this game will never require memory sufficiently large enough to be an issue.
//Honestly, if this thing costs more than a few mbs (if that) i'll be very surprised. 
namespace CoC.Backend.BodyParts
{
	public sealed class Femininity
	{
		public const byte FEMININE_MAX = 100;
		public const byte MASCULINE_MAX = 0;

		public const byte MIN_ANDROGYNOUS = 35;
		public const byte ANDROGYNOUS = 50;
		public const byte MAX_ANDROGYNOUS = 65;

		public const byte SLIGHTLY_FEMININE = 60;
		public const byte FEMININE = 70;
		public const byte HYPER_FEMININE = 90;
		
		public const byte SLIGHTLY_MASCULINE = 40;
		public const byte MASCULINE = 30;
		public const byte HYPER_MASCULINE = 10;

		public byte value
		{
			get => _value;
			private set
			{
				if (value != _value)
				{
					_value = Utils.Clamp2(value, MASCULINE_MAX, FEMININE_MAX);
					femininityChanged(); //notify everyone.
				}
			}
		}
		private byte _value;

		public static implicit operator byte(Femininity femininity)
		{
			return femininity.value;
		}

		private Femininity(Gender gender)
		{
			if (gender == Gender.GENDERLESS)
			{
				_value = 50;
			}
			else if (gender == Gender.HERM)
			{
				_value = 60;
			}
			else if (gender == Gender.MALE)
			{
				_value = 25;
			}
			else
			{
				_value = 75;
			}
		}

		private Femininity(byte femininity)
		{
			_value = Utils.Clamp2(femininity, MASCULINE_MAX, FEMININE_MAX);
		}

		internal static Femininity GenerateFromGender(Gender gender)
		{
			return new Femininity(gender);
		}

		internal static Femininity GenerateDefault()
		{
			return new Femininity(50);
		}

		internal static Femininity Generate(byte femininity)
		{
			return new Femininity(femininity);
		}

		internal Femininity copy()
		{
			return new Femininity(this);
		}
		private readonly HashSet<IFemininityListener> femininityListeners = new HashSet<IFemininityListener>();
		internal bool RegisterListener(IFemininityListener listener)
		{
			if (femininityListeners.Add(listener))
			{
				listener.GetFemininityData(ToFemininityData);
				return true;
			}
			return false;

		}

		internal bool DeregisterListener(IFemininityListener listener)
		{
			return femininityListeners.Remove(listener);
		}

		internal void SetupFemininityAware(IFemininityAware femininityAware)
		{
			femininityAware.GetFemininityData(ToFemininityData);
		}

		private void femininityChanged()
		{
			foreach (IFemininityListener listener in femininityListeners)
			{
				listener.reactToChangeInFemininity();
			}
		}

		private FemininityData ToFemininityData()
		{
			return new FemininityData(this);
		}

		public bool isFemale => atLeastSlightlyFeminine;
		public bool isMale => atLeastSlightlyMasculine;

		public bool isAndrogynous => value >= MIN_ANDROGYNOUS && value <= MAX_ANDROGYNOUS;

		public bool isSlightlyFeminine => value >= SLIGHTLY_FEMININE && value < FEMININE;
		public bool atLeastSlightlyFeminine => value >= SLIGHTLY_FEMININE && value < FEMININE;
		public bool isFeminine => value >= FEMININE && value < HYPER_FEMININE;
		public bool atLeastFeminine => value >= FEMININE;
		public bool isHyperFeminine => value >= HYPER_FEMININE;
		public bool isSlightlyMasculine => value <= SLIGHTLY_MASCULINE && value > MASCULINE;
		public bool atLeastSlightlyMasculine => value <= SLIGHTLY_MASCULINE;
		public bool isMasculine => value <= MASCULINE && value > HYPER_MASCULINE;
		public bool atLeastMasculine => value <= MASCULINE;
		public bool isHyperMasculine => value <= HYPER_MASCULINE;

		public byte feminize(byte amount)
		{
			if (value >= FEMININE_MAX)
			{
				return 0;
			}
			byte oldFemininity = value;
			value += amount;
			return value.subtract(oldFemininity);
		}

		public byte masculinize(byte amount)
		{
			if (value == 0)
			{
				return 0;
			}
			byte oldFemininity = value;
			value -= amount;
			return oldFemininity.subtract(value);
		}

		public void SetFemininity(byte newValue)
		{
			value = newValue;
		}

		public void Update(Femininity other)
		{
			value = other.value;
		}
	}

	//it wraps a byte. I dunno. 
	//now capping max base fertility to 75. Perks could boost this past the base 75 value. 
	public sealed class Fertility
	{
		public const byte MAX_BASE_FERTILITY = 75;
		//byte value => something, idk.

		public byte baseValue
		{
			get => _baseValue;
			private set
			{
				_baseValue = Utils.Clamp2<byte>(value, 0, MAX_BASE_FERTILITY);
			}
		}
		private byte _baseValue;

		public byte increaseFertility(byte amount = 1)
		{
			byte oldAmount = baseValue;
			baseValue += amount;
			return baseValue.subtract(oldAmount);
		}

		public byte decreaseFertility(byte amount = 1)
		{
			byte oldAmount = baseValue;
			baseValue -= amount;
			return oldAmount.subtract(baseValue);
		}

		public void setFertility(byte newValue)
		{
			baseValue = newValue;
		}

		private Fertility()
		{
			baseValue = 5;
		}

		internal static Fertility GenerateDefault()
		{
			return new Fertility();
		}

		internal static Fertility Generate(byte fertility)
		{
			return new Fertility()
			{
				baseValue = fertility
			};
		}

	}

	[Flags]
	public enum Gender : byte { GENDERLESS = 0, MALE = 1, FEMALE = 2, HERM = MALE | FEMALE }

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

	public static class GenitalHelpers
	{
		//Genderless can also be used if gender is unimportant.
		public static string AsPronoun(this Gender gender)
		{
			switch (gender)
			{
				case Gender.HERM:
				case Gender.FEMALE:
					return "her";
				case Gender.MALE:
					return "his";
				case Gender.GENDERLESS:
				default:
					return "its";
			}
		}

		public static string AsText(this Gender gender)
		{
			switch (gender)
			{
				case Gender.HERM:
					return "herm";
				case Gender.MALE:
					return "male";
				case Gender.FEMALE:
					return "female";
				case Gender.GENDERLESS:
				default:
					return "genderless";
			}
		}

		private static readonly string[] cupText = new string[]
		{
			"flat", "A-cup", "B-cup", "C-cup", "D-cup", "DD-cup", "big DD-cup", "E-cup", "big E-cup", "EE-cup",// 1-9
			"big EE-cup", "F-cup", "big F-cup", "FF-cup", "big FF-cup", "G-cup", "big G-cup", "GG-cup", "big GG-cup", "H-cup",//10-19
			"big H-cup", "HH-cup", "big HH-cup", "HHH-cup", "I-cup", "big I-cup", "II-cup", "big II-cup", "J-cup", "big J-cup",//20-29
			"JJ-cup", "big JJ-cup", "K-cup", "big K-cup", "KK-cup", "big KK-cup", "L-cup", "big L-cup", "LL-cup", "big LL-cup",//30-39
			"M-cup", "big M-cup", "MM-cup", "big MM-cup", "MMM-cup", "large MMM-cup", "N-cup", "large N-cup", "NN-cup", "large NN-cup",//40-49
			"O-cup", "large O-cup", "OO-cup", "large OO-cup", "P-cup", "large P-cup", "PP-cup", "large PP-cup", "Q-cup", "large Q-cup",//50-59
			"QQ-cup", "large QQ-cup", "R-cup", "large R-cup", "RR-cup", "large RR-cup", "S-cup", "large S-cup", "SS-cup", "large SS-cup",//60-69
			"T-cup", "large T-cup", "TT-cup", "large TT-cup", "U-cup", "large U-cup", "UU-cup", "large UU-cup", "V-cup", "large V-cup",//70-79
			"VV-cup", "large VV-cup", "W-cup", "large W-cup", "WW-cup", "large WW-cup", "X-cup", "large X-cup", "XX-cup", "large XX-cup",//80-89
			"Y-cup", "large Y-cup", "YY-cup", "large YY-cup", "Z-cup", "large Z-cup", "ZZ-cup", "large ZZ-cup", "ZZZ-cup", "large ZZZ-cup",//90-99
			//HYPER ZONE
			"hyper A-cup", "hyper B-cup", "hyper C-cup", "hyper D-cup", "hyper DD-cup", "hyper big DD-cup", "hyper E-cup", "hyper big E-cup", "hyper EE-cup", "hyper big EE-cup", //100-109
			"hyper F-cup", "hyper big F-cup", "hyper FF-cup", "hyper big FF-cup", "hyper G-cup", "hyper big G-cup", "hyper GG-cup", "hyper big GG-cup", "hyper H-cup", "hyper big H-cup", //110-119
			"hyper HH-cup", "hyper big HH-cup", "hyper HHH-cup", "hyper I-cup", "hyper big I-cup", "hyper II-cup", "hyper big II-cup", "hyper J-cup", "hyper big J-cup",  "hyper JJ-cup", //120-129
			 "hyper big JJ-cup", "hyper K-cup", "hyper big K-cup", "hyper KK-cup", "hyper big KK-cup", "hyper L-cup", "hyper big L-cup", "hyper LL-cup", "hyper big LL-cup", "hyper M-cup", //130-139
			"hyper big M-cup", "hyper MM-cup", "hyper big MM-cup", "hyper MMM-cup", "hyper large MMM-cup", "hyper N-cup", "hyper large N-cup", "hyper NN-cup", "hyper large NN-cup", "hyper O-cup", //140-149
			"hyper large O-cup", "hyper OO-cup", "hyper large OO-cup", "hyper P-cup", "hyper large P-cup", "hyper PP-cup", "hyper large PP-cup", "hyper Q-cup", "hyper large Q-cup", "hyper QQ-cup", //150-159
			 "hyper large QQ-cup", "hyper R-cup", "hyper large R-cup", "hyper RR-cup", "hyper large RR-cup", "hyper S-cup", "hyper large S-cup", "hyper SS-cup", "hyper large SS-cup", "hyper T-cup", //160-169
			"hyper large T-cup", "hyper TT-cup", "hyper large TT-cup", "hyper U-cup", "hyper large U-cup", "hyper UU-cup", "hyper large UU-cup", "hyper V-cup", "hyper large V-cup", "hyper VV-cup", //170-179
			 "hyper large VV-cup", "hyper W-cup", "hyper large W-cup", "hyper WW-cup", "hyper large WW-cup", "hyper X-cup", "hyper large X-cup", "hyper XX-cup", "hyper large XX-cup", "hyper Y-cup", //180-189
			"hyper large Y-cup", "hyper YY-cup", "hyper large YY-cup", "hyper Z-cup", "hyper large Z-cup", "hyper ZZ-cup", "hyper large ZZ-cup", "hyper ZZZ-cup", "hyper large ZZZ-cup", "jacques00-cup" //190-199
		};

		public static string AsText(this CupSize cupSize)
		{
			return cupText[(int)cupSize];
		}


	}
}