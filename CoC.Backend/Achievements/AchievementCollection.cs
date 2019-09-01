using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.Achievements
{
	//all achievements unlocked. attached to the backend global save. stores 

	public sealed class AchievementCollection
	{
		public readonly ReadOnlyCollection<AchievementBase> unlockedAchievements;
		private readonly List<AchievementBase> achievementStore;

		internal AchievementCollection()
		{
			achievementStore = new List<AchievementBase>();
			unlockedAchievements = new ReadOnlyCollection<AchievementBase>(achievementStore);
		}

		internal bool AddAchievement(AchievementBase achievement)
		{
			if (!achievementStore.Contains(achievement))
			{
				achievementStore.Add(achievement);
				return true;
			}
			return false;
		}
	}
}
