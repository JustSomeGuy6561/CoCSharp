//AchievementBase.cs
//Description:
//Author: JustSomeGuy
//9/19/2019, 1:11 AM
using CoC.Backend;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Achievements
{
	public abstract class AchievementBase
	{
		public readonly SimpleDescriptor name;
		protected AchievementBase(SimpleDescriptor achievementName)
		{
			name = achievementName ?? throw new ArgumentNullException(nameof(achievementName));
		}

		public abstract string AchievementLockedText();
		public abstract string AchievementUnlockedText();
	}
}
