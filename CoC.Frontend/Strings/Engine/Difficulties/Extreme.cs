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
			return "Opponent has 100% more HP and does more 50% damage.";
		}

		private static string ExtremeDesc()
		{
			return "+50% HP, +30% damage.";
		}


	}
}
