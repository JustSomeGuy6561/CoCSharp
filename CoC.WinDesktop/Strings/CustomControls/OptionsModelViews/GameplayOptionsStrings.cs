using CoC.Backend.Settings;
using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.CustomControls.OptionsModelViews
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