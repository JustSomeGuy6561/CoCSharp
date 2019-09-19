using CoC.Backend.Settings.Fetishes;
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
	public sealed partial class FetishOptionsModelView : OptionModelViewDataBase
	{
		private static string FetishOptionsTitleText()
		{
			return "Fetish Options";
		}

		private static string FetishOptionsHelperText()
		{
			return "Adjust settings regarding strange, exotic, and/or extreme fetishes appear in your game.";
		}

		private string FetishButtonText()
		{
			return "Fetishes";
		}
	}
}