using CoC.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.Helpers
{
	public sealed class BackgroundItem
	{
		public readonly string path;
		public readonly string sidebarPath;
		public readonly SimpleDescriptor title;
		public readonly EnabledOrDisabledWithToolTip disabledTooltip;
		public readonly bool isDarkMode;

		public BackgroundItem(string resourcePath, string sidebarResourcePath, SimpleDescriptor backgroundTitle, EnabledOrDisabledWithToolTip disabledWithToolTip, bool darkMode)
		{
			path = resourcePath;
			sidebarPath = sidebarResourcePath;
			title = backgroundTitle ?? throw new ArgumentNullException(nameof(backgroundTitle));
			disabledTooltip = disabledWithToolTip ?? throw new ArgumentNullException(nameof(disabledWithToolTip));
			isDarkMode = darkMode;
		}
	}
}
