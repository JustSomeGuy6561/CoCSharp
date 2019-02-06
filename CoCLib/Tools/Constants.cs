//Constants.cs
//Description:
//Author: JustSomeGuy
//1/29/2019, 7:35 PM
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Tools
{
	public static class Constants
	{
		public const int LEVEL_CAP = 120;

		//our leveling formula for a given level is 100 * level
		//thus our max is 100+200+300... or 100(n)(n+1)/2 = 50 * n * (n+1)
		public const int EXPERIENCE_CAP = 50 * LEVEL_CAP * (LEVEL_CAP + 1);
	}
}
