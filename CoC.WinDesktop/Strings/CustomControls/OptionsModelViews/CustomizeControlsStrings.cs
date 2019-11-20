using CoC.WinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.WinDesktop.CustomControls.OptionsModelViews
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

		private string HowToStr()
		{
			return "Controls can be set by clicking or tabbing on an individual item. " +
				"Controls can either be a single key, or a combination of a modifier key (ALT/CTRL/Win/Shift) and a regular key. " +
				"The Keys 'Escape', 'Backspace', 'Enter' and 'Tab' Are reserved, and cannot be assigned anywhere. " + Environment.NewLine + Environment.NewLine +
				"Enter will trigger the default button (the only button available for an option, if applicable). Escape and backspace will clear the current keybind. " +
				"Tab, and it's inversion, Shift+Tab, are used to nagivate between items in the display. ";
		}
	}
}
