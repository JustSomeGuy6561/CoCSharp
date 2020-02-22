using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Strings;

namespace CoC.Backend.BodyParts
{
	internal static class CommonBodyPartStrings
	{
		public static string OneOfDescription(bool isPlural, Conjugate conjugate, string desc)
		{
			string pronoun = conjugate.PossessiveAdjective();

			if (isPlural)
			{
				return "one of " + pronoun + " " + desc;
			}
			else
			{
				return pronoun + " " + desc;
			}
		}

		public static string EachOfDescription(bool isPlural, Conjugate conjugate, string desc)
		{
			string pronoun = conjugate.PossessiveAdjective();
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
