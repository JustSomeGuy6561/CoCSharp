using CoC.Backend;
using CoC.Backend.Engine;
using CoC.Backend.Settings;
using CoC.Backend.Tools;
using CoC.WinDesktop.CustomControls.SideBarModelViews;
using CoC.WinDesktop.Helpers;
using CoC.WinDesktop.ModelView;
using CoC.WinDesktop.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CoC.WinDesktop.CustomControls.OptionsModelViews
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
