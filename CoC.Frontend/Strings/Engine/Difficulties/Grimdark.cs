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

		private string YouStillNeedToUnlockGrimdark()
		{
			return "Grimdark is locked until you've beaten the game on EXTREME!, or through the powers of debugging. You're not missing much; it's not fully implemented.";
		}
	}
}
