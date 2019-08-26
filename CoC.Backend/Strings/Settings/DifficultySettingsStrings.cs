using CoC.Backend.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings
{
	public partial class DifficultySettings
	{
		private static string DifficultySettingsStr()
		{
			return "Difficulty";
		}

		private string HintStr(GameDifficulty gameDifficulty, bool isGlobal)
		{
			string globalStr = isGlobal
				? " This setting will affect the current session and all future new games, unless overridden."
				: " You may set this at any time, however, the game keeps track of the lowest difficulty used for this game save, and grants achievements/unlocks accordingly.";
			return gameDifficulty.difficultyHint() + globalStr;
		}
	}
}
