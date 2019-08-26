using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Fetishes
{
	public sealed partial class PiercingFetish
	{
		private static string PiercingFetishName()
		{
			return "Exotic Piercings";
		}

		private static string EnabledHintFn()
		{
			return "Your character can have (multiple) exotic piercings not normally allowed.";
		}

		private static string DisabledHintFn()
		{
			return "Your character will only be able to have the standard piercings.";
		}
	}
}
