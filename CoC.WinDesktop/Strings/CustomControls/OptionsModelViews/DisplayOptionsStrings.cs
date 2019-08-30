using CoC.Backend;
using CoC.Backend.Engine;
using CoC.Backend.Settings;
using CoC.Backend.Tools;
using CoCWinDesktop.CustomControls.SideBarModelViews;
using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;
using CoCWinDesktop.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CoCWinDesktop.CustomControls.OptionsModelViews
{
	public sealed partial class DisplayOptionsModelView : OptionModelViewDataBase
	{
		private static string DisplayOptionsTitleText()
		{
			return "Display Options";
		}

		private static string DisplayOptionsHelperText()
		{
			return "Adjust font size, background, and text background settings.";
		}

		private static string DisplayButtonText()
		{
			return "Adjust Display";
		}

	}
}