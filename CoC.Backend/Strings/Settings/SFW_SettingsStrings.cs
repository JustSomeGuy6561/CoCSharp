using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings
{
	public partial class SFW_Settings
	{
		private static string SfwSettingsStr()
		{
			return "Safe For 'Work' Mode";
		}

		private string EnabledHintStr(bool isGlobal)
		{
			string globalStr = isGlobal ? " This applies to the current game session and all new games going forward." : "";

			return "SFW Mode enabled. Sex scenes are disabled, and you cannot get raped." + globalStr;
		}

		private string DisabledHintStr(bool isGlobal)
		{
			string globalStr = isGlobal ? " This applies to the current game session and all new games going forward." : "";
			return "SFW Mode disabled. You'll see sex scenes and can be raped.";

		}
	}
}
