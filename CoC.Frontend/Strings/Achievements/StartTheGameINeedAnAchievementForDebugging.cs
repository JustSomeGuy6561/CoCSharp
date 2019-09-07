using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;

namespace CoC.Frontend.Achievements
{
	public partial class StartTheGameINeedAnAchievementForDebugging
	{

		private static string StartTheGameStr()
		{
			return "Just Starting Out";
		}

		public override string AchievementLockedText()
		{
			return "This is a secret achievement... Just kidding! Start a New Game for the first time.";
		}

		public override string AchievementUnlockedText()
		{
			return "Start a New Game for the first time.";
		}
	}
}
