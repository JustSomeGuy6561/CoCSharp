using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Tools
{
	public static partial class Measurement
	{
		private static readonly string[] numbers = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };

		private static string AsText(int number)
		{
			if (number >= numbers.Length || number < 0)
			{
				return number.ToString();
			}
			return numbers[number];
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

	}
}
