using CoC.Backend.SaveData;

namespace CoCWinDesktop
{
	public sealed class GuiGlobalSave : SaveData
	{
		public static GuiGlobalSave data => SaveSystem.getGlobalSave<GuiGlobalSave>();

		public double fontSize = 15;
		public int backgroundIndex = 0;
		public int textBackgroundIndex = 0;

		public bool? usesOldSprites = false;

		public bool imagePackEnabled = true;

		public bool isAnimated = true;

		public bool showEnemyStatBars = false;

		public bool sidebarUsesModernFont = true;

	}
}
