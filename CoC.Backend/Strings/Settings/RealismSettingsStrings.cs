using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings
{
	public partial class RealismSettings
	{
		private static string RealismSettingsStr()
		{
			return "Realistic Mode";
		}

		private string EnabledHintStr(bool isGlobal)
		{
			string globalStr = isGlobal ? " This affects the current game session and any new games." : "";

			return "Your body's production of various fluids is constrained by physical limitations, and overly large body parts adversely affect you." + globalStr;
			
		}

		private string DisabledHintStr(bool isGlobal)
		{
			string globalStr = isGlobal ? " This affects the current game session and any new games." : "";
			return "Your body's production of fluids is not capped in any way, and you can gain body parts of any size without consequence.";

		}
	}
}
