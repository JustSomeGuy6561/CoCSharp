namespace CoC.Frontend.UI
{
	public enum PlayerStatus { IDLE, EXPLORING, TALKING, COMBAT, PRISON, INGRAM }

	internal static class ViewOptions
	{
		//clears everything. if you want to do less than this, for example, keep the buttons but display some text saying they can't do whatever they chose due to circumstances,
		//you can just call TextOutput.ClearText(); 
		public static void ClearOutput()
		{
			TextOutput.ClearText();
			ButtonManager.ClearButtons();
			InputField.DeactivateInputField();
			DropDownMenu.DeactivateDropDownMenu();
		}

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
