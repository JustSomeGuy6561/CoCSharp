using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;

namespace CoC.Frontend.Achievements
{
	public sealed partial class StartTheGameINeedAnAchievementForDebugging : Backend.Achievements.AchievementBase
	{
		public StartTheGameINeedAnAchievementForDebugging() : base(StartTheGameStr)
		{
		}
	}
}
