using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings
{
	public partial class MeasurementSettings
	{
		private static string MeasurementSettingsText()
		{
			return "Measurement Units";
		}

		private static string MetricStr()
		{
			return "Metric";
		}

		private static string MetricHint()
		{
			return "In-game measurements will display in metric units (centimeters, meters, etc.)";
		}

		private static string ImperialStr()
		{
			return "Imperial";
		}

		private static string ImperialHint()
		{
			return "In-game measurements will display in imperial units (inches, feet, etc.)";
		}
	}
}
