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
			return "No opponent stats modifiers. You can resume from bad-ends with penalties";
		}

		private static string NormalDesc()
		{
			return "No stats changes.";
		}


	}
}
