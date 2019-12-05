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

		private bool achievementsChanged = false;

		internal AchievementCollection()
		{
			achievementStore = new List<AchievementBase>();
			unlockedAchievements = new ReadOnlyCollection<AchievementBase>(achievementStore);
			achievementsChanged = false;
		}

		internal bool AddAchievement(AchievementBase achievement)
		{
			if (!achievementStore.Contains(achievement))
			{
				achievementStore.Add(achievement);
				achievementsChanged = true;
				return true;
			}
			return false;
		}

		internal bool QueryNewAchievementsUnlocked()
		{
			bool retVal = achievementsChanged;
			achievementsChanged = false;
			return retVal;
		}
	}
}
