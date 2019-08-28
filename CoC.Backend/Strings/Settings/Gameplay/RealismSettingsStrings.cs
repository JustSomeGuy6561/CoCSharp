using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings.Gameplay
{
	public partial class RealismSettings
	{
		private static string RealismSettingsStr()
		{
			return "Realistic Mode";
		}
	}
	public partial class RealismSessionSetting
	{ 
		private string EnabledHintStr()
		{
			return "Your body's production of various fluids is constrained by physical limitations, and overly large body parts adversely affect you.";
			
		}

		private string DisabledHintStr()
		{
			return "Your body's production of fluids is not capped in any way, and you can gain body parts of any size without consequence.";

		}

		private string ActiveSaveAndItsEnabledDummy()
		{
			return "This save has realism enabled. You cannot disable it.";
		}

		private string WarningStr()
		{
			return "Enabling this on any active save or game session is PERMANENT. You've been warned!";
		}
	}

	public partial class RealismGlobalSetting
	{
		private string EnabledHintStr()
		{
			return "Your body's production of various fluids is constrained by physical limitations, and overly large body parts adversely affect you.";

		}

		private string DisabledHintStr()
		{
			return "Your body's production of fluids is not capped in any way, and you can gain body parts of any size without consequence.";
		}

		private string WarningStr()
		{
			return "Be aware that changing this will affect the default value for all new games. You may of course override this when creating a new game, but it" +
				" is not part of the standard options, so you may miss it if rushing through creation.";
		}
	}
}
