using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings.Gameplay
{
	public partial class MeasurementSettings
	{
		private static string MeasurementSettingsText()
		{
			return "Measurement Units";
		}
	}
	public partial class MeasurementGlobalSetting
	{ 
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
