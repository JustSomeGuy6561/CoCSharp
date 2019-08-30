using CoCWinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.CustomControls.OptionsModelViews
{
	public sealed partial class CustomizeControlsModelView : OptionModelViewDataBase
	{
		private static string CustomControlsTitleText()
		{
			return "Customize Controls";
		}

		private static string CustomControlsHelperText()
		{
			return "Alter or disable various hotkeys for common gameplay actions";
		}

		private string CustomControlsButtonText()
		{
			return "Controls";
		}
	}
}