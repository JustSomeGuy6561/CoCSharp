using CoC.Backend;
using System;
using System.Windows.Media;

namespace CoCWinDesktop.Helpers
{
	public sealed class TextBackgroundItem
	{
		public readonly SolidColorBrush color;
		public readonly SimpleDescriptor title;
		private readonly EnabledOrDisabledWithToolTip disabledTooltip;
		public readonly bool affectedByDarkMode;

		public TextBackgroundItem(SolidColorBrush backgroundColor, SimpleDescriptor backgroundTitle, EnabledOrDisabledWithToolTip disabledWithToolTip, bool isAffectedByDarkMode)
		{
			color = backgroundColor ?? throw new ArgumentNullException(nameof(backgroundColor));
			title = backgroundTitle ?? throw new ArgumentNullException(nameof(backgroundTitle));
			disabledTooltip = disabledWithToolTip ?? throw new ArgumentNullException(nameof(disabledWithToolTip));
			affectedByDarkMode = isAffectedByDarkMode;
		}
	}
}
