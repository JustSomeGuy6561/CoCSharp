using CoCWinDesktop.Helpers;

namespace CoCWinDesktop.DisplaySettings
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
}
