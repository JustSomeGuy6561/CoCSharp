using System;

namespace CoC.Backend.Settings.Gameplay
{
	public partial class HardcoreSettings
	{
		private static string HardcoreSettingsStr()
		{
			return "Hardcore Mode";
		}
	}
	public partial class HardcoreSessionSetting
	{
		private string EnabledHintStr()
		{
			return "Your progress will automatically be saved, and you cannot save manually. Any Game Over will delete your save. This cannot be disabled";
		}


		private string DisabledHintStr()
		{
			return "You can save your game manually, and reload any such saves at any time. Enabling this will force autosave and prevent any further manual saves," +
				" and cannot be undone.";
		}

		private string WarningStr()
		{
			if (SaveData.SaveSystem.isSessionActive && !setting)
			{
				return "Enabling this will prevent any further manual saves for this campaign, and force autosave. A game over will remove this autosave. This is PERMANENT." +
					" You've been warned!";
			}
			return "";
		}

		private string ActiveSaveAndItsEnabledDummy()
		{
			return "This campaign has hardcore mode enabled. it cannot be disabled!";
		}
	}

	public partial class HardcoreGlobalSetting
	{
		private string EnabledHintStr()
		{
			return "By default, any new games will have their progress automatically saved, and cannot be saved manually. Any Game Over will delete the save."
				+ " This can be overridden when creating a new game";
		}


		private string DisabledHintStr()
		{
			return "By default, any new games will allow you to save and load them without restriction. This does not affect any existing saves."
				+ " This can be overridden when creating a new game";
		}

		private string EasyModeNoAllowHardcoreYouPleb()
		{
			return "You cannot enable hardcore mode while the difficulty is set to Easy.";
		}

		private string WarningStr()
		{
			return "Enabling this will prevent you from lowering the difficulty to Easy. All new games will default to this, though they can be overridden through a series of " +
				"extra actions. If you tend to rush through character creation, consider leaving this disabled.";
		}
	}
}
