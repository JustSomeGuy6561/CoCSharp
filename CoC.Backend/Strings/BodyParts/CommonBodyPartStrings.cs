using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	internal static class CommonBodyPartStrings
	{
		public static string OneOfDescription(bool isPlural, string pronoun, string desc)
		{
			if (isPlural)
			{
				return "one of " + pronoun + " " + desc;
			}
			else
			{
				return pronoun + " " + desc;
			}
		}

		public static string EachOfDescription(bool isPlural, string pronoun, string desc)
		{
			if (isPlural)
			{
				return "each of " + pronoun + " " + desc;
			}
			else
			{
				return pronoun + " " + desc;
			}
		}

	}
}
