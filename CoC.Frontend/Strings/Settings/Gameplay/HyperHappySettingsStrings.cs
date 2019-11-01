using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Settings.Gameplay
{
	public partial class HyperHappySettings
	{
		private static string HyperHappySettingsStr()
		{
			return "Hyper Happy";
		}
	}
	public partial class HyperHappySessionSetting
	{ 
		private string EnabledHintStr()
		{
			return "Items and transformations that would normally have unique effects based on your gender will no longer do so. Transformations that occur regardless of your gender" +
				" will be unaffected";
			
		}

		private string DisabledHintStr()
		{
			return "Transformations that would alter male or female endowments based on your current gender will behave normally - Male enhancing effects will shrink female endowments," +
				" and vice versa";
		}

		//private string ActiveSaveAndItsEnabledDummy()
		//{
		//	return "This save has realism enabled. You cannot disable it.";
		//}

		//private string WarningStr()
		//{
		//	return "Enabling this on any active save or game session is PERMANENT. You've been warned!";
		//}
	}

	public partial class HyperHappyGlobalSetting
	{
		private string EnabledHintStr()
		{
			return "Items and transformations that would normally have unique effects based on your gender will no longer do so. Transformations that occur regardless of your gender" +
				" will be unaffected";
		}

		private string DisabledHintStr()
		{
			return "Transformations that would alter male or female endowments based on your current gender will behave normally - Male enhancing effects will shrink female endowments," +
				" and vice versa";
		}

		private string WarningStr()
		{
			return "Be aware that changing this will affect the default value for all new games. It can be changed at any time.";
		}
	}
}
