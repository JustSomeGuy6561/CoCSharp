using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings
{
	public partial class HardcoreSettings
	{
		private static string HardcoreSettingsStr()
		{
			return "Hardcore Mode";
		}

		private string EnabledHintStr(bool isGlobal)
		{
			if (isGlobal)
			{
				return "Your progress will automatically be saved, and you cannot save manually. Any Game Over will delete your save. Applies to the current game session and " +
					"any new games going forward.";
			}
			else
			{
				return "Your progress will automatically be saved, and you cannot save manually. Any Game Over will delete your save. This cannot be disabled";
			}
		}


		private string DisabledHintStr(bool isGlobal)
		{
			if (isGlobal)
			{
				return "You can save your game manually, and reload any such saves at any time. This does not affect any existing saves that have this enabled.";
			}
			else
			{
				return "You can save your game manually, and reload any such saves at any time. Enabling this will force autosave and prevent any further manual saves," +
					" and cannot be undone.";
			}
		}
	}
}
