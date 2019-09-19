using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.WinDesktop.ModelView;

namespace CoC.WinDesktop.CustomControls.ExtraItemModelViews
{
	public partial class MenuInstructionsModelView
	{
		private string InstructionsHeader()
		{
			return "Instructions and Tips";
		}

		private string SaveWarning()
		{
			return "Don't forget to save often via the Data Menu - you never know when your journey may come to an end!";
		}

		private static string ControlsTitle()
		{
			return "Controls";
		}

		private static string ControlsDesc()
		{
			return "For the most part, the game will be controlled exclusively through button presses, though on rare occasions, you may have to select an option from a drop-" +
				"down menu, or input text into an input field. Efforts have been made to make this as intuitive as possible, but we're only human. " +
				"This version of this game has 15 possible buttons at the bottom of the screen for various options. Each button may also have a tooltip " +
				"to help further explain what choosing that option entails. Buttons can be clicked via mouse, or selected via TAB and 'clicked' via ENTER. " +
				"Additionally, it is possible to use various keyboard hotkeys, which can be customized via the Options button on the Main Menu. Note that hotkeys are " +
				"disabled when entering text in a textfield. When necessary, a text input field will appear, and its will have the initial focus, so you can type and " +
				"continue without needing to select it manually. A drop down menu can also be controlled only using TAB and Enter if you prefer a keyboard-only approach." +
				"Finally, some input fields may restrict your input, not accepting characters that aren't valid. For example, a field requesting your height won't allow " +
				" text characters.";
		}
	}
}
