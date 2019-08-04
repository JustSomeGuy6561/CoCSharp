using CoC.Backend;
using CoC.Backend.Engine;
using System;

namespace CoC.Frontend.Engine.Difficulties
{
	public partial class Hard : GameDifficulty
	{
		private static string HardStr()
		{
			return "Hard";
		}

		private static string HardHint()
		{
			return "Enemies have 50% less health and you can ignore bad-ends without penalty.";
		}

		private static string HardDesc()
		{
			return "Combat is easier and bad-ends can be ignored";
		}


	}
}
