using CoC.WinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.WinDesktop.CustomControls.OptionsModelViews
{

	public partial class SaveOptionsModelView
	{
		private static string SaveOptionText()
		{
			return "Save Options";
		}

		private static string SaveHelperText()
		{
			return "Adjust various save settings, like save location and autosave.";
		}

		private string SaveButtonText()
		{
			return "Save Options";
		}
	}
}
