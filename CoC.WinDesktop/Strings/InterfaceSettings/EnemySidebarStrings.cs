namespace CoC.WinDesktop.InterfaceSettings
{
	public partial class EnemySidebarOption
	{
		private static string EnemyStatBar()
		{
			return "Show Enemy Stats Sidebar";
		}

		private static string EnemyStatBarsOff()
		{
			return "During combat, enemy stats will be shown as part of the regular content, after all the standard combat text. This is the classic style.";
		}

		private static string EnemyStatBarsOn()
		{
			return "During combat, the content section will be shrunk to accomodate a sidebar displaying enemy stats on the opposite side of the players' stats." +
				"This is the modern style.";
		}
	}
}
