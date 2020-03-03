using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;
using CoC.Backend.Achievements;

namespace CoC.Frontend.Achievements
{
	class Smashed : AchievementBase
	{
		public Smashed() : base(SmashedText)
		{
		}

		private static string SmashedText()
		{
			return "Smashed!";
		}

		public override string AchievementLockedText()
		{
			throw new NotImplementedException();
		}

		public override string AchievementUnlockedText()
		{
			throw new NotImplementedException();
		}
	}
}
