using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.Helpers
{
	public partial class FontSizeOption
	{
		internal static string FontSizeText()
		{
			return "Font Size";
		}

		private static string FontText(int pointSize)
		{
			return $"{pointSize}PT";
		}
		private static string FontHint(int pointSize)
		{
			return $"This is equivalent to {MeasurementHelpers.Convert(pointSize, SizeUnit.POINTS, SizeUnit.PIXELS):0.##}px.";
		}
	}

	public partial class BackgroundOption
	{
		internal static string BackgroundText()
		{
			return "Background";
		}
		public static string MapBGText()
		{
			return "Map (Default)";
		}

		public static string ParchmentBGText()
		{
			return "Parchment";
		}

		public static string MarbleBGText()
		{
			return "Marble";
		}

		public static string ObsidianBGText()
		{
			return "Obsidian";
		}

		public static string NightModeBGText()
		{
			return "NightMode";
		}

		public static string GrimdarkBGText()
		{
			return "Grimdark";
		}
	}

	public partial class TextBackgroundOption
	{
		internal static string TextBackgroundText()
		{
			return "Text Background";
		}

		public static string NormalTextBgDesc()
		{
			return "Normal";
		}

		public static string WhiteTextBgDesc()
		{
			return "White";
		}

		public static string TanTextBgDesc()
		{
			return "Tan";
		}

		public static string ClearTextBgDesc()
		{
			return "Clear";
		}
	}
}
