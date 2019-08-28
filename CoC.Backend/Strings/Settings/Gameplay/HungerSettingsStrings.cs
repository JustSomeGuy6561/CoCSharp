namespace CoC.Backend.Settings.Gameplay
{
	public partial class HungerSettings
	{
		private static string HungerSettingStr()
		{
			return "Survival Mode";
		}
	}
	public partial class HungerSessionSetting
	{

		private string EnabledHintStr()
		{
			return "The Player will get hungry and needs to eat to survive. If a game is loaded, this cannot be disabled.";
		}

		private string ActiveSaveAndItsEnabledDummy()
		{
			return "This save has hunger enabled. You cannot disable it.";
		}

		private string DisabledHintStr()
		{
			return "The Player will not get hungry over time.";
		}

		private string WarningStr()
		{
			return "Enabling this on any active save or game session is PERMANENT. You've been warned!";
		}
	}

	public partial class HungerGlobalSetting
	{

		private string EnabledHintStr()
		{
			return "Player characters will get hungry over time and need to eat to survive. This will apply to all new games, unless overridden.";
		}

		private string DisabledHintStr()
		{
			return "Player characters do not get hungry over time. This only applies to new saves; Existing saves will continue to use whatever setting they have.";
		}

		private string WarningStr()
		{
			return "Be aware that changing this will affect the default value for all new games. You may of course override this when creating a new game, but it" +
				" is not part of the standard options, so you may miss it if rushing through creation.";
		}
	}
}
