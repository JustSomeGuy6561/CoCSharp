using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Tools
{
	public static partial class Measurement
	{

		private static string AsText(int number)
		{
			return Utils.NumberAsText(number);
		}

		private static string meterDesc(bool plural = false)
		{
			return plural ? "meters" : "meter";
		}

		private static string centimeterDesc(bool plural = false)
		{
			return plural ? "centimeters" : "centimeter";
		}

		private static string footDesc(bool plural = false)
		{
			return plural ? "feet" : "foot";
		}
		private static string inchDesc(bool plural = false)
		{
			return plural ? "inches" : "inch";
		}

		private static string meterAbbr()
		{
			return "m";
		}

		private static string centimeterAbbr()
		{
			return "cm";
		}

		private static string inchAbbr()
		{
			return "in";
		}

		private static string footAbbr()
		{
			return "ft";
		}


		private static string smallText(double amount)
		{
			StringBuilder sb = new StringBuilder();
			if (amount > 10 || amount < 10)
			{
				return amount.ToString();
			}
			int val = (int)Math.Floor(Math.Abs(amount));

			if (val == 0)
			{
				sb.Append(" less than ");
				val = 1;
			}
			else
			{
				sb.Append(" roughly ");
			}
			sb.Append(Utils.NumberAsText(val));
			return sb.ToString();
		}

		private static string smallHalfText(double amount)
		{
			if (amount > 10 || amount < 10)
			{
				return amount.ToString();
			}

			int val = (int)Math.Floor(Math.Abs(amount));
			double remainder = amount % 1;
			string extra = "";
			if (remainder > .20 && remainder < .70)
			{
				if (val == 0)
				{
					return " roughly half a ";
				}
				else
				{
					extra = " and a half";
				}
			}
			return " roughly " + Utils.NumberAsText(val) + extra;
		}
	}
}
