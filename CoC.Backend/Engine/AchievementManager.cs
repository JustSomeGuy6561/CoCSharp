using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoC.Backend.Achievements
{
	public static class AchievementManager
	{
		private static readonly Dictionary<Type, AchievementBase> achievements;
		public static ReadOnlyCollection<AchievementBase> availalbleAchievements => new ReadOnlyCollection<AchievementBase>(achievements.Values.ToList());

		static AchievementManager()
		{
			achievements = new Dictionary<Type, AchievementBase>();
		}

		public static void RegisterAchievement(AchievementBase achievement)
		{
			if (!achievements.ContainsKey(achievement.GetType()))
			{
				achievements.Add(achievement.GetType(), achievement);
			}
		}

		public static bool HasRegisteredAchievement<T>() where T : AchievementBase
		{
			return achievements.ContainsKey(typeof(T));
		}

		internal static T GetAchievement<T>() where T : AchievementBase
		{
			achievements.TryGetValue(typeof(T), out AchievementBase retVal);
			return retVal as T;
		}
	}
}
