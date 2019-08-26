using CoC.Backend;
using CoC.Backend.Engine;
using System;

namespace CoC.Frontend.Engine.Difficulties
{
	public partial class Nightmare : GameDifficulty
	{
		private static string NightmareStr()
		{
			return "Nightmare";
		}

		private static string NightmareHint()
		{
			return "Opponent has 50% more HP and does 30% more damage.";
		}

		private static string NightmareDesc()
		{
			return "+50% HP, +30% damage.";
		}


	}
}
