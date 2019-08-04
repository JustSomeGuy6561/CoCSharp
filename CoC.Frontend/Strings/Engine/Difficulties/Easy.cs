using CoC.Backend;
using CoC.Backend.Engine;
using System;

namespace CoC.Frontend.Engine.Difficulties
{
	public partial class Easy : GameDifficulty
	{
		private static string EasyStr()
		{
			return "Easy";
		}

		private static string EasyHint()
		{
			return "Enemies have 50% less health and you can ignore bad-ends without penalty.";
		}

		private static string EasyDesc()
		{
			return "Combat is easier and bad-ends can be ignored";
		}


	}
}
