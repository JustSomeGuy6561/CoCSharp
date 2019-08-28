using CoC.Backend.Strings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings.Fetishes
{
	public sealed partial class PiercingFetish
	{
		private static string PiercingFetishName()
		{
			return "Exotic Piercings";
		}
	}
	public sealed partial class PiercingFetishSetting
	{

		private static string EnabledHintFn()
		{
			return "Your character can have (multiple) exotic piercings not normally allowed.";
		}

		private static string DisabledHintFn()
		{
			return "Your character will only be able to have the standard piercings.";
		}

		private string PiercingUnsetText(bool isGlobal)
		{
			if (isGlobal) return EngineStrings.UnsetText();
			else return "Ask Me";
		}
		private string PiercingUnsetHint(bool isGlobal)
		{
			if (isGlobal) return "The game will default to using the piercing fetish setting for the current save, or through in-game interactions if this is set to \"Ask Me\".";
			else return "Your will get the option to choose whether or not to enable this through in-game interactions";
		}
	}
}
