using CoC.Backend.Achievements;
using CoC.Backend.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoC.Frontend.UI.ControllerData
{
	public sealed class AchievementData
	{
		static AchievementData()
		{
			instance = new AchievementData();
		}
		private static AchievementData instance;

		public static bool QueryData(out AchievementData achievementData)
		{
			achievementData = instance;
			if (instance is null)
			{
				throw new ArgumentException("Shit broke!");
			}
			return instance.QueryDataInternal();
		}

		private bool QueryDataInternal()
		{
			//clear all the unlocked since last query b/c this is a new query.
			unlockedSinceLastQueryStore.Clear();
			bool changed = GameEngine.QueryUnlockedAchievements(out ReadOnlyCollection<AchievementBase> currentUnlockedAchievements);
			if (changed)
			{
				//add the newly obtained achievements, via Linq. Except gets all the things in currentUnlocked not already in unlocked.
				unlockedSinceLastQueryStore.AddRange(currentUnlockedAchievements.Except(unlockedAchievements));
			}

			unlockedAchievements = currentUnlockedAchievements;
			return changed;
		}


		public readonly ReadOnlyCollection<AchievementBase> allAchievements;
		public ReadOnlyCollection<AchievementBase> unlockedAchievements { get; private set; }

		public ReadOnlyCollection<AchievementBase> achievementsUnlockedSinceLastQuery;
		private readonly List<AchievementBase> unlockedSinceLastQueryStore;

		internal AchievementData()
		{
			allAchievements = AchievementManager.availalbleAchievements;
			GameEngine.QueryUnlockedAchievements(out ReadOnlyCollection<AchievementBase> temp);
			unlockedAchievements = temp;

			unlockedSinceLastQueryStore = new List<AchievementBase>(unlockedAchievements);
			achievementsUnlockedSinceLastQuery = new ReadOnlyCollection<AchievementBase>(unlockedSinceLastQueryStore);
		}
	}
}
