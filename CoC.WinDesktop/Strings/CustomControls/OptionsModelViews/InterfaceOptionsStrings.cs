using CoC.Backend;
using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CoCWinDesktop.CustomControls.OptionsModelViews
{
	public sealed partial class InterfaceOptionsModelView : OptionModelViewDataBase
	{
		private static string InterfaceTitleText()
		{
			return "Interface Options";
		}

		private static string InterfaceHelperText()
		{
			return "Adjust settings affecting the game's look and feel.";
		}

		private string InterfaceButtonText()
		{
			return "Interface";
		}
	}
}