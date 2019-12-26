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

		Gender gender { get; }

		float lactationRate { get; }

		LactationStatus lactationStatus { get; }

		bool isOverFull { get; }
	}

	public partial class Breasts : IBreast
	{
		public static string Name()
		{
			return "Breasts";
		}

		CupSize IBreast.cupSize => cupSize;

		NippleData IBreast.nipples => nipples.AsReadOnlyData();

		byte IBreast.numberOfBreasts => numBreasts;

		Gender IBreast.gender => currGender;

	}

	public partial class BreastData : IBreast
	{
		CupSize IBreast.cupSize => cupSize;

		NippleData IBreast.nipples => nipples;

		byte IBreast.numberOfBreasts => numberOfBreasts;

		float IBreast.lactationRate => lactationRate;

		LactationStatus IBreast.lactationStatus => lactationStatus;

		bool IBreast.isOverFull => isOverFull;

		Gender IBreast.gender => gender;
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

		private static readonly string[] articleCupText = new string[]
		{
			"a flat", "an A-cup", "a B-cup", "a C-cup", "a D-cup", "a DD-cup", "a big DD-cup", "an E-cup", "a big E-cup", "a EE-cup",// 1-9
			"a big EE-cup", "a F-cup", "a big F-cup", "a FF-cup", "a big FF-cup", "a G-cup", "a big G-cup", "a GG-cup", "a big GG-cup", "an H-cup",//10-19
			"a big H-cup", "a HH-cup", "a big HH-cup", "a HHH-cup", "an I-cup", "a big I-cup", "a II-cup", "a big II-cup", "a J-cup", "a big J-cup",//20-29
			"a JJ-cup", "a big JJ-cup", "a K-cup", "a big K-cup", "a KK-cup", "a big KK-cup", "a L-cup", "a big L-cup", "a LL-cup", "a big LL-cup",//30-39
			"a M-cup", "a big M-cup", "a MM-cup", "a big MM-cup", "a MMM-cup", "a large MMM-cup", "a N-cup", "a large N-cup", "a NN-cup", "a large NN-cup",//40-49
			"an O-cup", "a large O-cup", "a OO-cup", "a large OO-cup", "a P-cup", "a large P-cup", "a PP-cup", "a large PP-cup", "a Q-cup", "a large Q-cup",//50-59
			"a QQ-cup", "a large QQ-cup", "a R-cup", "a large R-cup", "a RR-cup", "a large RR-cup", "a S-cup", "a large S-cup", "a SS-cup", "a large SS-cup",//60-69
			"a T-cup", "a large T-cup", "a TT-cup", "a large TT-cup", "a U-cup", "a large U-cup", "a UU-cup", "a large UU-cup", "a V-cup", "a large V-cup",//70-79
			"a VV-cup", "a large VV-cup", "a W-cup", "a large W-cup", "a WW-cup", "a large WW-cup", "a X-cup", "a large X-cup", "a XX-cup", "a large XX-cup",//80-89
			"a Y-cup", "a large Y-cup", "a YY-cup", "a large YY-cup", "a Z-cup", "a large Z-cup", "a ZZ-cup", "a large ZZ-cup", "a ZZZ-cup", "a large ZZZ-cup",//90-99
			//HYPER ZONE
			"a hyper A-cup", "a hyper B-cup", "a hyper C-cup", "a hyper D-cup", "a hyper DD-cup", "a hyper big DD-cup", "a hyper E-cup", "a hyper big E-cup", "a hyper EE-cup", "a hyper big EE-cup", //100-109
			"a hyper F-cup", "a hyper big F-cup", "a hyper FF-cup", "a hyper big FF-cup", "a hyper G-cup", "a hyper big G-cup", "a hyper GG-cup", "a hyper big GG-cup", "a hyper H-cup", "a hyper big H-cup", //110-119
			"a hyper HH-cup", "a hyper big HH-cup", "a hyper HHH-cup", "a hyper I-cup", "a hyper big I-cup", "a hyper II-cup", "a hyper big II-cup", "a hyper J-cup", "a hyper big J-cup",  "a hyper JJ-cup", //120-129
			 "a hyper big JJ-cup", "a hyper K-cup", "a hyper big K-cup", "a hyper KK-cup", "a hyper big KK-cup", "a hyper L-cup", "a hyper big L-cup", "a hyper LL-cup", "a hyper big LL-cup", "a hyper M-cup", //130-139
			"a hyper big M-cup", "a hyper MM-cup", "a hyper big MM-cup", "a hyper MMM-cup", "a hyper large MMM-cup", "a hyper N-cup", "a hyper large N-cup", "a hyper NN-cup", "a hyper large NN-cup", "a hyper O-cup", //140-149
			"a hyper large O-cup", "a hyper OO-cup", "a hyper large OO-cup", "a hyper P-cup", "a hyper large P-cup", "a hyper PP-cup", "a hyper large PP-cup", "a hyper Q-cup", "a hyper large Q-cup", "a hyper QQ-cup", //150-159
			 "a hyper large QQ-cup", "a hyper R-cup", "a hyper large R-cup", "a hyper RR-cup", "a hyper large RR-cup", "a hyper S-cup", "a hyper large S-cup", "a hyper SS-cup", "a hyper large SS-cup", "a hyper T-cup", //160-169
			"a hyper large T-cup", "a hyper TT-cup", "a hyper large TT-cup", "a hyper U-cup", "a hyper large U-cup", "a hyper UU-cup", "a hyper large UU-cup", "a hyper V-cup", "a hyper large V-cup", "a hyper VV-cup", //170-179
			 "a hyper large VV-cup", "a hyper W-cup", "a hyper large W-cup", "a hyper WW-cup", "a hyper large WW-cup", "a hyper X-cup", "a hyper large X-cup", "a hyper XX-cup", "a hyper large XX-cup", "a hyper Y-cup", //180-189
			"a hyper large Y-cup", "a hyper YY-cup", "a hyper large YY-cup", "a hyper Z-cup", "a hyper large Z-cup", "a hyper ZZ-cup", "a hyper large ZZ-cup", "a hyper ZZZ-cup", "a hyper large ZZZ-cup", "a jacques00-cup" //190-199
		};

		public static string AsText(this CupSize cupSize, bool withArticle = false)
		{
			int index = Math.Min(cupText.Length - 1, (byte)cupSize);
			if (withArticle) return articleCupText[index];
			else return cupText[index];
		}

		public static string DescribeSize(this CupSize cupSize, Gender gender = Gender.MALE, bool withArticle = false)
		{
			//Catch all for dudes.
			if (cupSize == CupSize.FLAT) return (gender.HasFlag(Gender.FEMALE) ? "flat" : "manly");
			//Small - A->B
			if (cupSize <= CupSize.B)
			{
				return Utils.RandomChoice("palmable", "tight", "perky", "baseball-sized");
			}
			//C-D
			else if (cupSize <= CupSize.D)
			{
				return Utils.RandomChoice("nice", "hand-filling", "well-rounded", "supple", "softball-sized");
			}
			//DD->big EE
			else if (cupSize < CupSize.F)
			{
				return Utils.RandomChoice("big", "large", "pillowy", "jiggly", "volleyball-sized");
			}
			//F->big FF
			else if (cupSize < CupSize.G)
			{
				return Utils.RandomChoice("soccerball-sized", "hand-overflowing", "generous", "jiggling");
			}
			//G-> HHH
			else if (cupSize < CupSize.I)
			{
				return Utils.RandomChoice("basketball-sized", "whorish", "cushiony", "wobbling");
			}
			//I-> KK
			else if (cupSize < CupSize.L)
			{
				return Utils.RandomChoice("massive motherly", "luscious", "smothering", "prodigious");
			}
			//L-> ZZZ_BIG+
			else if (cupSize < CupSize.HYPER_A)
			{
				return Utils.RandomChoice("mountainous", "monumental", "back-breaking", "exercise-ball-sized", "immense");
			}
			//Hyper sizes
			else
			{
				return Utils.RandomChoice("ludicrously-sized", "hideously large", "absurdly large", "back-breaking", "colossal", "immense");
			}
		}


		//prints out single, plural, or single with article. plural takes priority over with article flag. Article only in here in case something requires "an"
		private static string Noun(IBreast breast, bool plural, bool withArticle)
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
				if (plural) return "tits";
				else if (withArticle) return "a tit";
				else return "tit";
			}
			else if (rand == 4 && breast.lactationRate > 1.5)
			{
				if (plural || !withArticle)
				{
					return "milky " + Utils.PluralizeIf(breast.cupSize > CupSize.D ? "tit" : "breast", plural);
				}
				else
				{
					return "a milky " + (breast.cupSize > CupSize.D ? "tit" : "breast");
				}
			}
			else if ((rand == 5 && breast.lactationRate > 2) || (rand == 6 && breast.lactationRate > 2.5))
			{
				if (plural || !withArticle)
				{
					return Utils.PluralizeIf("milk-udder", plural);
				}
				else
				{
					return "a milk-udder";
				}
			}
			else if (rand == 6)
			{
				if (withArticle && !plural)
				{
					return "a " + (breast.lactationRate > 1.5 ? "milk " : "") + "jug";
				}
				else
				{
					return (breast.lactationRate > 1.5 ? "milk " : "") + Utils.PluralizeIf("jug", plural);
				}
			}
			else if (rand == 7 && breast.cupSize > CupSize.DD_BIG)
			{
				if (withArticle && !plural)
					return "a love-pillow";
				else return Utils.Pluralize("love-pillow");
			}
			else if (rand == 7)
			{
				if (withArticle && !plural)
					return "a boob";
				else return Utils.Pluralize("boob");
			}
			//8 or 9 or fell through.

			if (withArticle && !plural)
			{
				return "a breast";
			}
			return Utils.PluralizeIf("breast", plural);
		}

		internal static string ShortDesc(IBreast breast, bool plural)
		{
			return Noun(breast, plural, false);
		}

		internal static string SingleItemDesc(IBreast breast)
		{
			return Noun(breast, false, true);
		}


		//standard: measurement as adjective + cupsize + breastNoun;
		internal static string Desc(IBreast breast, bool alternateFormat, bool plural, bool preciseMeasurements, bool includeNipples)
		{
			string intro = "";
			if (plural && alternateFormat)
			{
				intro = "a pair of ";
			}

			bool getArticle = alternateFormat && !plural;

			if (!includeNipples)
			{
				string milkText;

				if (preciseMeasurements)
				{
					if (breast.isOverFull)
					{
						milkText = (getArticle ? "a " : "") + "milk-swollen ";
					}
					else
					{
						milkText = " ";
					}
					return intro + milkText + breast.cupSize.AsText() + " " + ShortDesc(breast, plural);
				}
				else
				{
					milkText = breast.isOverFull ? ", milk-swollen " : " ";
					return intro + breast.cupSize.DescribeSize(breast.gender, getArticle) + milkText + ShortDesc(breast, plural);
				}
			}
			else
			{
				string milkText = breast.isOverFull ? ", milk-swollen " : " ";

				string withText = plural ? ", each with " : " with ";
				if (preciseMeasurements)
				{
					return intro + breast.cupSize.DescribeSize(breast.gender, getArticle) + milkText + breast.cupSize.AsText() + " " + ShortDesc(breast, plural) + withText + breast.nipples.FullDescription(true, true, true);
				}
				else
				{
					return intro + breast.cupSize.DescribeSize(breast.gender, getArticle) + milkText + ShortDesc(breast, plural) + withText + breast.nipples.FullDescription(true, true, false);
				}
			}
		}
	}
}
