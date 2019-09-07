using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Strings.Engine
{
	public static class Instructions
	{
		public static string HowToPlayTitle()
		{
			return "How To Play:";
		}

		public static string HowToPlayDesc()
		{
			return "Choose between the various options given to you to progress the story and/or explore and interact to your heart's content. The overall 'goal' is to put an end" +
				"to the demonic corruption spreading across the land and ultimately save your hometown, but you're given complete freedom to completely ignore that if you like.";
		}

		public static string ExplorationTitle()
		{
			return "Exploration:";
		}


		public static string ExplorationDesc()
		{
			return "Exploration is the key to finding new areas and ultimately progressing in the game. As you explore, you'll find new areas and places, and can return to them by " +
				"visiting the \"Area\" and \"Place\" menus, respectively. Note that you may occasionally run into enemies when exploring, which will require you to engage in combat.";
		}

		public static string CombatTitle()
		{
			return "Combat";
		}

		public static string CombatDesc()
		{
			return "There are two ways to win in combat: Either by bringing an opponent's hp to 0, or maxing out their lust. The same conditions apply to you, and you'll lose if " +
				"either happen. Combat is turn-based, with a new round occuring if neither side has won yet. Winning will grant you gems and experience, and additional options " +
				"may be available depending on how you won. Losing will allow the opponent to take gems from you, and they may further humiliate you, depending on the opponent." +
				" For the most part, losing in combat will not cause a Game Over, though Bosses or other enemies can cause that to happen if you aren't careful. There is no penalty" +
				" for running, so feel free to spam that option if you're over your head.";
		}

		public static string TipsTitle()
		{
			return "Early-Game Tips";
		}

		public static string TipsDesc()
		{
			return "The game will get easier as you go along, and more options are available to you. If you're having trouble early, the Lake is initially a safe area, and you can " +
				"find places there that will allow you to boost your stats. Interacting with people and places can also yield surprising gains. You'll want to find a few safe places " +
				"early in the game where you can buy or sell items or exercise and/or train, so try exploring the Desert and the Lake. If you find a new area, make sure you're fully " +
				"prepared before exploring it again - enemies there may be a great deal tougher than what you've dealt with so far. Running is always an option if you find yourself " +
				"outclassed.";
		}
	}
}
