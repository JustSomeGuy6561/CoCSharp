using System;

namespace CoCWinDesktop.ModelView
{
	public partial class MainMenuModelView
	{
		private static string ContinueBtnText()
		{
			return "Continue";
		}

		private static string ContinueBtnLockedText()
		{
			return "Please start a new game or load an existing save file.";
		}

		private string ContinueButtonTip(bool isUnlocked)
		{
			return isUnlocked ? ContinueBtnUnlockedText() : ContinueBtnLockedText();
		}
		private string ContinueBtnUnlockedText()
		{
			return "Get back to gameplay?";
		}

		private string newGameBtnText()
		{
			return "New Game";
		}
		private string NewGameBtnTip()
		{
			return "Start a new game.";
		}
		private string DataButtonText()
		{
			return "Data";
		}
		private string DataButtonTip()
		{
			return "Load or manage saved games.";
		}
		private string OptionsButtonText()
		{
			return "Options";
		}
		private string OptionsButtonTip()
		{
			return "Configure game settings and enable cheats.";
		}
		private string AchievementsButtonText()
		{
			return "Achievements";
		}
		private string AchievementsButtonTip()
		{
			return "View all achievements you have earned so far.";
		}
		private string InstructionsButtonText()
		{
			return "Instructions";
		}
		private string InstructionsButtonTip()
		{
			return "How to play, and tips.";
		}
		private string CreditsButtonText()
		{
			return "Credits";
		}
		private string CreditsButtonTip()
		{
			return "See a list of all the cool people who have contributed to content for this game!";
		}

		private string ModThreadButtonText()
		{
			return "Mod Thread";
		}
		private string ModThreadButtonTip()
		{
			return "Check the official mod thread on Fenoxo's forum.";
		}
	}
}
