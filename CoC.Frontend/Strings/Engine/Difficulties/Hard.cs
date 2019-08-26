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
			return "Opponent has 25% more HP and does 15% more damage. Bad-ends can ruin your game.";
		}

		private static string HardDesc()
		{
			return "+25% HP, +15% damage.";
		}


	}
}
