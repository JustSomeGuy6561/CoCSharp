using CoC.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CoCWinDesktop.Helpers
{
	public sealed class BackgroundWrapper
	{
		public readonly string path;
		public readonly string sidebarPath;
		public readonly SimpleDescriptor title;
		public readonly EnabledOrDisabledWithToolTip disabledTooltip;
		public readonly bool isDarkMode;

		public BackgroundWrapper(string resourcePath, string sidebarResourcePath, SimpleDescriptor backgroundTitle, EnabledOrDisabledWithToolTip disabledWithToolTip, bool darkMode)
		{
			path = resourcePath;
			sidebarPath = sidebarResourcePath;
			title = backgroundTitle ?? throw new ArgumentNullException(nameof(backgroundTitle));
			disabledTooltip = disabledWithToolTip ?? throw new ArgumentNullException(nameof(disabledWithToolTip));
			isDarkMode = darkMode;
		}
	}

	public sealed class TextBackgroundWrapper
	{
		public readonly SolidColorBrush color;
		public readonly SimpleDescriptor title;
		private readonly EnabledOrDisabledWithToolTip disabledTooltip;
		public readonly bool affectedByDarkMode;

		public TextBackgroundWrapper(SolidColorBrush backgroundColor, SimpleDescriptor backgroundTitle, EnabledOrDisabledWithToolTip disabledWithToolTip, bool isAffectedByDarkMode)
		{
			color = backgroundColor ?? throw new ArgumentNullException(nameof(backgroundColor));
			title = backgroundTitle ?? throw new ArgumentNullException(nameof(backgroundTitle));
			disabledTooltip = disabledWithToolTip ?? throw new ArgumentNullException(nameof(disabledWithToolTip));
			affectedByDarkMode = isAffectedByDarkMode;
		}
	}
}
