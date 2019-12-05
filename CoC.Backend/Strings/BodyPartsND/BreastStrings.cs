//BreastNippleStrings.cs
//Description:
//Author: JustSomeGuy
//1/10/2019, 9:07 PM

using CoC.Backend.Tools;
using System;

namespace CoC.Backend.BodyParts
{
	//feel free to use the body type for these descriptions for these -
	//because of the lack of types, 

	internal interface IBreast
	{
		CupSize cupSize { get; }

		NippleData nipples { get; }

		byte numberOfBreasts { get; }

		float lactationRate { get; }

		LactationStatus lactationStatus { get; }
	}

	public partial class Breasts : IBreast
	{
		public static string Name()
		{
			return "Breasts";
		}

		public string ShortDescription() => BreastData.ShortDesc(true);
		public string ShortDescription(bool plural) => BreastData.ShortDesc(plural);
		public string AsNoun(bool plural = true) => BreastData.BreastNoun(this, plural);

		public string LongDescription(bool preciseMeasurements, bool plural = true) => BreastData.Desc(this, preciseMeasurements, false, plural);

		public string FullDescription(bool preciseMeasurements, bool plural = true) => BreastData.Desc(this, preciseMeasurements, true, plural);

		CupSize IBreast.cupSize => cupSize;

		NippleData IBreast.nipples => nipples.AsReadOnlyData();

		byte IBreast.numberOfBreasts => numBreasts;
	}

	public partial class BreastData : IBreast
	{
		CupSize IBreast.cupSize => cupSize;

		NippleData IBreast.nipples => nipples;

		byte IBreast.numberOfBreasts => numberOfBreasts;

		float IBreast.lactationRate => lactationRate;

		LactationStatus IBreast.lactationStatus => lactationStatus;

		public string ShortDescription() => ShortDesc(true);
		public string ShortDescription(bool plural) => ShortDesc(plural);
		public string AsNoun(bool plural = true) => BreastNoun(this, plural);

		public string LongDescription(bool preciseMeasurements, bool plural = true) => Desc(this, preciseMeasurements, false, plural);

		public string FullDescription(bool preciseMeasurements, bool plural = true) => Desc(this, preciseMeasurements, true, plural);

		internal static string ShortDesc(bool plural)
		{
			return plural ? "breasts" : "breast";
		}

		internal static string BreastNoun(IBreast breast, bool plural)
		{
			//5 of 10: tits.*
			//2 of 10: breasts.
			//1 of 10: jugs.
			//1 of 10: boobs/love pillows
			//1 of 10: milk-udders*

			//2 of 10 on the tits require a size - 1st >4, 2nd >6, or fall through to breasts. 
			//udders requires lactation; falls through to breasts.
			int rand = Utils.Rand(10);
			if (rand < 3 || (rand == 3 && breast.cupSize > CupSize.DD_BIG))
			{
				return "tits";
			}
			else if (rand == 4 && breast.lactationRate > 1.5)
			{
				return "milky " + (breast.cupSize > CupSize.D ? "tits" : "breasts");
			}
			else if (rand == 5 && breast.lactationRate > 2)
			{
				return "milk-udders";
			}
			else if (rand == 6 && breast.lactationRate > 2.5)
			{
				return "milk-udder";
			}
			else if (rand == 6)
			{
				return (breast.lactationRate > 1.5 ? "milk " : "") + "jugs";
			}
			else if (rand == 7)
			{
				return breast.cupSize > CupSize.DD_BIG ? "love-pillows" : "boobs";
			}
			//8 or 9 or fell through.

			return "breasts";
		}

		//standard: measurement as adjective + cupsize + breastNoun;
		internal static string Desc(IBreast breast, bool preciseMeasurements, bool full, bool plural = true)
		{
			if (!full)
			{
				if (preciseMeasurements)
				{
					return breast.cupSize.AsText() + " " + BreastNoun(breast, plural);
				}
				else
				{
					return breast.cupSize.DescribeSize() + " " + BreastNoun(breast, plural);
				}
			}
			else
			{
				string withText = plural ? ", each with " : " with ";
				if (preciseMeasurements)
				{
					return breast.cupSize.DescribeSize() + breast.cupSize.AsText() + " " + BreastNoun(breast, plural) + withText + breast.nipples.FullDescription(false, true);
				}
				else
				{
					return breast.cupSize.DescribeSize() + " " + BreastNoun(breast, plural) + withText + breast.nipples.FullDescription(false, true);
				}
			}
		}


	}

	public static class BreastHelpers
	{
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
			int index = Math.Min(cupText.Length - 1, (byte)cupSize);
			return cupText[index];
		}

		public static string DescribeSize(this CupSize cupSize, Gender gender = Gender.FEMALE)
		{
			//Catch all for dudes.
			if (cupSize == CupSize.FLAT) return (gender.HasFlag(Gender.FEMALE) ? "flat " : "manly ");
			//Small - A->B
			if (cupSize <= CupSize.B)
			{
				return Utils.RandomChoice("palmable ", "tight ", "perky ", "baseball-sized ");
			}
			//C-D
			else if (cupSize <= CupSize.D)
			{
				return Utils.RandomChoice("nice ", "hand-filling ", "well-rounded ", "supple ", "softball-sized ");
			}
			//DD->big EE
			else if (cupSize < CupSize.F)
			{
				return Utils.RandomChoice("big ", "large ", "pillowy ", "jiggly ", "volleyball-sized ");
			}
			//F->big FF
			else if (cupSize < CupSize.G)
			{
				return Utils.RandomChoice("soccerball-sized ", "hand-overflowing ", "generous ", "jiggling ");
			}
			//G-> HHH
			else if (cupSize < CupSize.I)
			{
				return Utils.RandomChoice("basketball-sized ", "whorish ", "cushiony ", "wobbling ");
			}
			//I-> KK
			else if (cupSize < CupSize.L)
			{
				return Utils.RandomChoice("massive motherly ", "luscious ", "smothering ", "prodigious ");
			}
			//L-> ZZZ_BIG+
			else if (cupSize < CupSize.HYPER_A)
			{
				return Utils.RandomChoice("mountainous ", "monumental ", "back-breaking ", "exercise-ball-sized ", "immense ");
			}
			//Hyper sizes
			else
			{
				return Utils.RandomChoice("ludicrously-sized ", "hideously large ", "absurdly large ", "back-breaking ", "colossal ", "immense ");
			}
		}
	}
}
