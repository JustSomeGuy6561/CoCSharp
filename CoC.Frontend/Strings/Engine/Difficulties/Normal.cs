using CoC.Backend;
using CoC.Backend.Engine;
using System;

namespace CoC.Frontend.Engine.Difficulties
{
	public partial class Normal : GameDifficulty
	{
		private static string NormalStr()
		{
			return "Normal";
		}

		private static string NormalHint()
		{
			return "Enemies have 50% less health and you can ignore bad-ends without penalty.";
		}

		private static string NormalDesc()
		{
			return "Combat is easier and bad-ends can be ignored";
		}


	}
}
