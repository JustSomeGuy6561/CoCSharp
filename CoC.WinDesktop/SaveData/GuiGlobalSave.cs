using CoC.Backend.SaveData;
using CoC.WinDesktop.Helpers;

namespace CoC.WinDesktop
{
	public sealed class GuiGlobalSave : GlobalSaveData
	{
		public static GuiGlobalSave data => SaveSystem.GetGlobalSave<GuiGlobalSave>();

		//20px, 15pt. 
		public int FontSizeInEms = 30;
		//public int FontSizeInEms = MeasurementHelpers.ConvertToEms(20, SizeUnit.PIXELS);


		public int backgroundIndex = 1;
		public int textBackgroundIndex = 0;

		public bool? usesOldSprites = false;

		public bool imagePackEnabled = true;

		public bool isAnimated = true;

		public bool showEnemyStatBars = false;

		public bool sidebarUsesModernFont = true;

	}
}
