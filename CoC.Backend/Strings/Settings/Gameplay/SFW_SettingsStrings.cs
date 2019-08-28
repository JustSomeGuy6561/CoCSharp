using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings.Gameplay
{
	public partial class SFW_Settings
	{
		private static string SfwSettingsStr()
		{
			return "Safe For 'Work' Mode";
		}
	}
	public partial class SFW_SessionSetting
	{ 
		private string EnabledHintStr()
		{
			return "SFW Mode enabled. Sex scenes are disabled, and you cannot get raped.";
		}

		private string DisabledHintStr()
		{
			return "SFW Mode disabled. You'll see sex scenes and can be raped.";

		}
	}

	public partial class SFW_GlobalSetting
	{
		private string EnabledHintStr()
		{

			return "SFW Mode enabled. Sex scenes are disabled, and you cannot get raped.";
		}

		private string DisabledHintStr()
		{
			return "SFW Mode disabled. You'll see sex scenes and can be raped.";

		}
	}
}
