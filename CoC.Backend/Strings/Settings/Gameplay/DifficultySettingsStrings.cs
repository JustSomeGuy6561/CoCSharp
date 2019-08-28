using CoC.Backend.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings.Gameplay
{
	public partial class DifficultySetting
	{
		private static string DifficultySettingsStr()
		{
			return "Difficulty";
		}

	}
	public partial class DifficultyGameplaySetting
	{ 
		private string LoweringDifficultyWarning()
		{
			return "Lowering the difficulty may prevent you from getting certain achievements, as achievements will only respect the lowest difficulty used in game.";
		}
	}
}
