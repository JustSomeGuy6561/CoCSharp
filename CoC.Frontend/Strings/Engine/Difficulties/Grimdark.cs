using CoC.Backend;
using CoC.Backend.Engine;
using System;

namespace CoC.Frontend.Engine.Difficulties
{
	public partial class Grimdark : GameDifficulty
	{
		private static string GrimdarkStr()
		{
			return "Grimdark";
		}

		private static string GrimdarkHint()
		{
			return "Enemies have 50% less health and you can ignore bad-ends without penalty.";
		}

		private static string GrimdarkDesc()
		{
			return "Combat is easier and bad-ends can be ignored";
		}


	}
}
