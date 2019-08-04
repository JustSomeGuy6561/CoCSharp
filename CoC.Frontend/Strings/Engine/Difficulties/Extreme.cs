using CoC.Backend;
using CoC.Backend.Engine;
using System;

namespace CoC.Frontend.Engine.Difficulties
{
	public partial class Extreme : GameDifficulty
	{
		private static string ExtremeStr()
		{
			return "Extreme";
		}

		private static string ExtremeHint()
		{
			return "Enemies have 50% less health and you can ignore bad-ends without penalty.";
		}

		private static string ExtremeDesc()
		{
			return "Combat is easier and bad-ends can be ignored";
		}


	}
}
