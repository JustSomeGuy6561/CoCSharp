using CoC.Backend.SaveData;
using System;

namespace CoC.Backend.Tools
{
	//Note: the game uses Imperial measurements - I can only assume the creators were American (despite the fact save files were stored using the extended character set.)
	//an option should be provided somewhere in UI land that allows you to set it to metric. Ideally, meansurements are not hard-coded, so they will convert. 
	//while the correct conversion rate is 1 in:2.54 cm, these may be rounded to a more presentable number.
	public static partial class Measurement
	{
		public static string ToText(int number)
		{
			return AsText(number);
		}

		//in the event you really want to go HAM with this, you could use Humanizer. it also has multi-lingual support, so there's that. https://github.com/Humanizr/Humanizer

		public static bool UsesMetric => BackendGlobalData.data.UsesMetricMeasurements;

		//are these remotely necessary? nope. but they're more readible imo than magic numbers.
		public const double TO_CENTIMETERS = 2.54;
		public const double TO_INCHES = 1 / TO_CENTIMETERS;
		public const byte IN_PER_FT = 12;
		public const byte CM_PER_M = 100;



		public static string ToNearestSmallUnit(double measure, bool abbreviate, bool useTextIfSmall, bool? plural = null)
		{
			string unit;
			if (UsesMetric)
			{
				measure *= TO_CENTIMETERS;
			}
			if (useTextIfSmall && measure <= 10 && measure >= -10)
			{
				unit = getSmallUnit(false, measure > 1 || measure < -1);
				return smallText(measure) + " " + unit;
			}
			if (plural == null)
			{
				plural = measure > 1 || measure < -1;
			}

			unit = getSmallUnit(abbreviate, (bool)plural);
			return string.Format("{0:F0} {1}", measure, unit);
		}

		public static string ToNearestHalfSmallUnit(double measure, bool abbreviate, bool useTextIfSmall, bool? plural = null)
		{
			if (UsesMetric)
			{
				measure *= TO_CENTIMETERS;
			}
			string unit;
			measure *= 2;
			measure = Math.Round(measure);
			measure /= 2;
			if (useTextIfSmall && measure <= 10 && measure >= -10)
			{
				unit = getSmallUnit(false, measure > 1.25 || measure < -1.25);
				return smallHalfText(measure) + " " + unit;
			}

			if (plural == null)
			{
				plural = measure > 1.25 || measure < -1.25;
			}

			unit = getSmallUnit(abbreviate, (bool)plural);
			return string.Format("{0:0.#} {1}", measure, unit);
		}

		public static string ToNearestQuarterInchOrMillimeter(double measure, bool abbreviate, bool? plural = null)
		{
			if (UsesMetric)
			{
				measure *= TO_CENTIMETERS;
				measure *= 10;
				measure = Math.Round(measure);
				measure /= 10;
			}
			else
			{
				measure *= 4;
				measure = Math.Round(measure);
				measure /= 4;
			}

			if (plural == null)
			{
				plural = measure != 1 && measure != -1;
			}
			string unit = getSmallUnit(abbreviate, (bool)plural);
			if (UsesMetric)
			{
				return string.Format("{0:0.#} {1}", measure, unit);
			}
			else
			{
				return string.Format("{0:0.##} {1}", measure, unit);
			}
		}

		public static string ToNearestQuarterInchOrHalfCentimeter(double measure, bool abbreviate, bool? plural = null)
		{
			if (UsesMetric)
			{
				measure *= TO_CENTIMETERS;
				measure *= 5;
				measure = Math.Round(measure);
				measure /= 10;
			}
			else
			{
				measure *= 4;
				measure = Math.Round(measure);
				measure /= 4;
			}
			if (plural == null)
			{
				plural = measure != 1 && measure != -1;
			}
			string unit = getSmallUnit(abbreviate, (bool)plural);
			if (UsesMetric)
			{
				return string.Format("{0:0.#} {1}", measure, unit);
			}
			else
			{
				return string.Format("{0:0.##} {1}", measure, unit);
			}
		}

		public static string ToNearestLargeAndSmallUnit(double measure, bool abbreviate, bool? plural = null)
		{
			bool useLarge = UsesMetric ? measure > CM_PER_M : measure > IN_PER_FT;

			if (UsesMetric && useLarge)
			{
				measure /= 10;
				if (plural == null)
				{
					plural = measure != 1 && measure != -1;
				}
				string unit = getLargeUnit(abbreviate, (bool)plural);
				return string.Format("{0:0.##} {1}", measure, unit);
			}
			else if (UsesMetric)
			{
				if (plural == null)
				{
					plural = measure != 1 && measure != -1;
				}
				string unit = getSmallUnit(abbreviate, (bool)plural);
				return string.Format("{0:F0} {1}", measure, unit);
			}
			else if (useLarge)
			{
				bool smallPl, largePl;
				int large = (int)Math.Floor(measure / IN_PER_FT);
				measure /= IN_PER_FT;
				if (plural == null)
				{
					largePl = large != 1 && large != -1;
					smallPl = measure != 1 && measure != -1;
				}
				else
				{
					smallPl = (bool)plural;
					largePl = (bool)plural;
				}
				string smallUnit = getSmallUnit(abbreviate, smallPl);
				string largeUnit = getLargeUnit(abbreviate, largePl);

				return string.Format("{0} {1} {2:F0} {3}", large, largeUnit, measure, smallUnit);
			}
			else
			{
				if (plural == null)
				{
					plural = measure != 1 && measure != -1;
				}
				string unit = getSmallUnit(abbreviate, (bool)plural);
				return string.Format("{0:F0} {1}", measure, unit);
			}
		}

		private static string getSmallUnit(bool abbr, bool plural)
		{
			if (UsesMetric)
			{
				if (abbr)
				{
					return centimeterAbbr();
				}
				else
				{
					return centimeterDesc(plural);
				}
			}
			else
			{
				if (abbr)
				{
					return inchAbbr();
				}
				else
				{
					return inchDesc(plural);
				}
			}
		}

		private static string getLargeUnit(bool abbr, bool plural)
		{
			if (UsesMetric)
			{
				if (abbr)
				{
					return meterAbbr();
				}
				else
				{
					return meterDesc(plural);
				}
			}
			else
			{
				if (abbr)
				{
					return footAbbr();
				}
				else
				{
					return footDesc(plural);
				}
			}
		}

		public static string ToNearestHalfLargeUnit(double measure, bool abbreviate, bool useTextIfSmall, bool? plural = null)
		{
			if (UsesMetric)
			{
				measure *= TO_CENTIMETERS;
				measure /= 100;
			}
			else
			{
				measure /= 12;
			}
			if (measure % 1 <= 0.25)
			{
				measure = Math.Floor(measure);
			}
			else if (measure % 1 >= 0.75)
			{
				measure = Math.Ceiling(measure);
			}
			else
			{
				measure = (Math.Floor(measure) * 2 + 1) / 2;
			}

			string unit;
			if (useTextIfSmall && measure <= 10 && measure >= -10)
			{
				unit = getLargeUnit(false, measure > 1.25 || measure < -1.25);
				return smallHalfText(measure) + " " + unit;
			}

			if (plural == null)
			{
				plural = measure > 1.25 || measure < -1.25 ;
			}
			unit = getLargeUnit(abbreviate, (bool)plural);
			return string.Format("{0:0.#} {1}", measure, unit);
		}
	}
}
