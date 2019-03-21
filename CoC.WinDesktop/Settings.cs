using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;

namespace CoCWinDesktop
{


	internal static class Settings
	{
		private static readonly string[] backgrounds = { Path.Combine("resources", "background1.jpg"), Path.Combine("resources", "background2.png"), Path.Combine("resources", "background3.png"), Path.Combine("resources", "background4.png"), null, Path.Combine("resources", "backgroundKaizo.png") };
		private static readonly SolidColorBrush[] textBgs = { newSolidColorWithTransparency(Colors.White, 0.4), new SolidColorBrush(Colors.White), new SolidColorBrush(Color.FromRgb(0xEB, 0xD5, 0xA6)), new SolidColorBrush(Colors.Transparent) };

		private static SolidColorBrush newSolidColorWithTransparency(Color color, double opacity)
		{
			return new SolidColorBrush(color)
			{
				Opacity = opacity
			};
		}
		public static int currBackground { get; private set; } = 0;
		public static int currentTextBackground { get; private set; } = 0;
		public static void SetBackgroundImage(int index)
		{
			if (index < 0)
			{
				index = 0;
			}
			else if (index >= backgrounds.Length)
			{
				index = backgrounds.Length - 1;
			}
			else if (index != currBackground)
			{
				bool wasNightMode = NightMode;
				currBackground = index;
				OnBackgroundChanged();
				if (wasNightMode != NightMode)
				{
					OnFontColorChanged();
				}
			}
		}
		public static string BackgroundImage => backgrounds[currBackground];
		public static Brush FontColor => NightMode ? (TanBg ? Brushes.White : nightModeBrush ): Brushes.Black;
		private static readonly Brush nightModeBrush = new SolidColorBrush(Color.FromRgb(0xC0, 0xC0, 0xC0));
		private static bool NightMode => backgrounds[currBackground] == null;
		private static bool TanBg => currentTextBackground == 2;
		public static SolidColorBrush TextBackground => textBgs[currentTextBackground]; 
		public static event EventHandler BackgroundImageChanged;
		public static event EventHandler FontColorChanged;
		public static event EventHandler TextBackgroundChanged;


		private static void OnFontColorChanged()
		{
			FontColorChanged?.Invoke(null, EventArgs.Empty);
		}
		private static void OnBackgroundChanged()
		{
			BackgroundImageChanged?.Invoke(null, EventArgs.Empty);
		}
		private static void OnTextBackgroundChanged()
		{
			TextBackgroundChanged?.Invoke(null, EventArgs.Empty);
		}

		static Settings()
		{
			BackgroundImageChanged += (x, y) => {return; };
			FontColorChanged += (x, y) => {return; };
			TextBackgroundChanged += (x, y) => {return; };
		}
	}
}
