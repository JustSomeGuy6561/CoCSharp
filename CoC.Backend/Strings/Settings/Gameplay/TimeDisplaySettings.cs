using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings.Gameplay
{
	public partial class TimeDisplaySettings
	{
		private static string TimeDisplaySettingsText()
		{
			return "Time Format";
		}
	}
	public partial class TimeGlobalSetting
	{
		private static string TwelveHourStr()
		{
			return "12-Hour";
		}

		private static string AM_PM_Hint()
		{
			return "Time will be shown in a 12-hour format (AM/PM)";
		}

		private static string TwentyFourHourStr()
		{
			return "24-Hour";
		}

		private static string MilitaryHint()
		{
			return "Time will be shown in a 24-hour format";
		}
	}
}
