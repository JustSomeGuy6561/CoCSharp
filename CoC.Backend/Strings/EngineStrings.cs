using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Strings
{
	public static class EngineStrings
	{
		public static string EnableText()
		{
			return "ON";
		}

		public static string DisableText()
		{
			return "OFF";
		}

		internal static string UnsetText()
		{
			return "DEFAULT";
		}

		internal static string UnsetHint()
		{
			return "No preference. The game will use default settings if possible, or prompt you when necessary.";
		}
	}
}
