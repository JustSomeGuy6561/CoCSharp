using System;

namespace CoCWinDesktop.Helpers
{
	public static class InterfaceStrings
	{
		internal static string AdjustDisplayOptionsText()
		{
			return "Adjust Display";
		}

		internal static string DisplayOptionsTitleText()
		{
			return "Display Options";
		}

		internal static string DisplayOptionsHelperText()
		{
			return "Adjust common display options such as Font Size, Background, and Text Background. Your changes will be reflected in real-time.";
		}

		internal static string OldText()
		{
			return "OLD";
		}

		internal static string NewText()
		{
			return "NEW";
		}

		internal static string OnText()
		{
			return "ON";
		}

		internal static string OffText()
		{
			return "OFF";
		}
	}
	public partial class SidebarFontOption
	{
		internal static string SidebarFont()
		{
			return "SideBar Font";
		}

		internal static string OldFontSelected()
		{
			return "Lucida Sans Typewriter will be used. This is the old font.";
		}

		internal static string NewFontSelected()
		{
			return "Palatino Linotype will be used. This is the current font.";
		}
	}

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
	public partial class ImagePackOption
	{
		internal static string ImagePackText()
		{
			return "Display Image Pack Images";
		}

		internal static string ImagePackEnabled()
		{
			return "Scenes may display images alongside content. If no image packs are installed, this will have no effect.";
		}

		internal static string ImagePackDisabled()
		{
			return "Scenes will not display images, regardless of whether or not any image packs are installed.";
		}
	}

	public partial class SpriteStatusOption
	{ 
		internal static string SpriteStatus()
		{
			return "Sprites";
		}

		internal static string NewSpritesSelected()
		{
			return "You like to look at pretty pictures. New, 16-bit sprites will be shown.";
		}

		internal static string OldSpritesSelected()
		{
			return "You like to look at pretty pictures. Old, 8-bit sprites will be shown.";
		}

		internal static string OffSpritesSelected()
		{
			return "You prefer the expanse of nothingness where sprite pictures should be.";
		}
	}

	public partial class SidebarAnimationOption
	{
		internal static string SidebarAnimationText()
		{
			return "Animate Side Bars";
		}

		internal static string AnimationsOn()
		{
			return "The side bars will animate their numbers and graphics when stats change.";
		}

		internal static string AnimationsOff()
		{
			return "The side bars will display stat changes without any animations.";
		}
	}
}
