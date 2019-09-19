using CoC.Backend;
using CoC.WinDesktop.Helpers;
using CoC.WinDesktop.ModelView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CoC.WinDesktop.CustomControls.OptionsModelViews
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