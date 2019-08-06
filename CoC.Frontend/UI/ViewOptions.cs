namespace CoC.Frontend.UI
{
	public enum PlayerStatus { IDLE, EXPLORING, TALKING, COMBAT, PRISON, INGRAM }

	internal static class ViewOptions
	{
		internal static bool showStandardMenu { get; private set; }
		internal static bool showStats { get; private set; }
		internal static PlayerStatus playerStatus { get; private set; }

		internal static void HideMenu()
		{
			showStandardMenu = false;
		}

		internal static void ShowMenu()
		{
			showStandardMenu = true;
		}

		internal static void HideStats()
		{
			showStats = false;
		}

		internal static void ShowStats()
		{
			showStats = true;
		}

		internal static void SetPlayerStatus(PlayerStatus status)
		{
			playerStatus = status;
		}

	}
}
