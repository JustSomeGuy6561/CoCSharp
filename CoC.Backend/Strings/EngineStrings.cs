using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Strings
{
	public static class EngineStrings
	{
		public static string EnableText()
		{
			return "On";
		}

		public static string DisableText()
		{
			return "Off";
		}

		internal static string UnsetText()
		{
			return "Unset";
		}

		internal static string UnsetHint()
		{
			return "No preference. The game will use default settings if possible, or prompt you when necessary.";
		}
	}
}
