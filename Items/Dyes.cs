//Dyes.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 7:56 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using CoC.Internal.

namespace CoC.Items
{
	public class Dyes : EpidermalColors
	{
		public enum DYE_COLORS
		{
			NO_COLOR,
			AUBURN, BLACK, BLONDE, BLUE, BROWN, GRAY, GREEN, ORANGE,
			PINK, PURPLE, RAINBOW, RED, RUSSET, YELLOW, WHITE
		}

		public static readonly Dyes NO_FUR = new Dyes();
		public static readonly Dyes BLACK = new Dyes();
		public static readonly Dyes HUMAN_DEFAULT = BLACK;

		public string AsString()
		{
			throw new NotImplementedException();
		}
	}
}
