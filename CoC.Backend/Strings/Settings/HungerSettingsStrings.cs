using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings
{
	public partial class HungerSettings
	{
		private static string HungerSettingStr()
		{
			return "Survival Mode";
		}
		private string EnabledHintStr(bool isGlobal)
		{
			if (isGlobal)
			{
				return "Player characters will get hungry over time and need to eat to survive. This will apply to the current game session and all new games, unless overridden. " +
					"Any session with this enabled cannot disable it.";
			}
			else
			{
				return "The Player will get hungry and needs to eat to survive. This cannot be disabled for this game session and save.";
			}
		}

		private string DisabledHintStr(bool isGlobal)
		{
			if (isGlobal)
			{
				return "Player characters do not get hungry over time. This only applies to new saves; Existing saves will continue to use whatever setting they have.";
			}
			else
			{
				return "The Player will not get hungry over time. Enabling this cannot be undone and will apply to all future saves as well. ";
			}
		}
	}
}
