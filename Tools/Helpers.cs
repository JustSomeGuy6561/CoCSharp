//Helpers.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 2:12 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Tools
{
	static class Helpers
	{
		public static string asText(this Gender gender)
		{
			switch (gender)
			{
				case Gender.HERM:
					return "herm";
				case Gender.MALE:
					return "male";
				case Gender.FEMALE:
					return "female";
				case Gender.GENDERLESS:
				default:
					return "genderless";
			}
		}

		//Genderless can also be used if gender is unimportant.

		public static string asPronoun(this Gender gender)
		{
			switch (gender)
			{
				case Gender.HERM:
				case Gender.FEMALE:
					return "her";
				case Gender.MALE:
					return "his";
				case Gender.GENDERLESS:
				default:
					return "its";
			}
		}
	}
}
