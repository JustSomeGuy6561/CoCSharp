//Helpers.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 2:12 PM
using  CoC.BodyParts;
using System;

namespace CoC.Tools
{
	static class Helpers
	{
		public static string asText(this Gender gender)
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

		//Genderless can also be used if gender is unimportant.

		public static string asPronoun(this Gender gender)
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

		public static int asInt(this CupSize cupSize)
		{
			return (int)cupSize;
		}

		public static float ToNearestQuarter(float value)
		{
			double decimalPoint = value % 1;
			if (decimalPoint > .875)
			{
				decimalPoint = 1;
			}
			else if (decimalPoint > .625)
			{
				decimalPoint = 0.75;
			}
			else if (decimalPoint > .375)
			{
				decimalPoint = 0.5;
			}
			else if (decimalPoint > .125)
			{
				decimalPoint = 0.25;
			}
			else
			{
				decimalPoint = 0f;
			}
			value = (float)(Math.Floor(value) + decimalPoint);
			return value;
		}

		public static bool IsInverted(this NippleStatus nippleStatus)
		{
			return nippleStatus == NippleStatus.FULLY_INVERTED || nippleStatus == NippleStatus.SLIGHTLY_INVERTED;
		}
	}
}
