using CoC.Backend.Settings;
using CoC.WinDesktop.Helpers;
using CoC.WinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.WinDesktop.CustomControls.OptionsModelViews
{
	public partial class GameplayOptionsModelView
	{
		private static string GameplayTitleText()
		{
			return "Gameplay";
		}

		private static string GameplayHelperText()
		{
			return "Adjust various gameplay settings. Fetishes are given their own category.";
		}

		private string GameplayButtonText()
		{
			return "Gameplay";
		}
	}
}