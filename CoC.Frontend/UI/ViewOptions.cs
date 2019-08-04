namespace CoC.Frontend.UI
{
	internal static class ViewOptions
	{
		internal static bool showStandardMenu { get; private set; }
		internal static bool showStats { get; private set; }

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
	}
}
