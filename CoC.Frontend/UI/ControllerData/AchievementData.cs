using CoC.Backend.Achievements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Frontend.UI.ControllerData
{
	public sealed class AchievementData
	{
		public static bool QueryData(out AchievementData achievementData)
		{
			achievementData = instance;
			return instance.dataChanged;
		}

		private static AchievementData instance = new AchievementData();

		private bool dataChanged;

		public ReadOnlyCollection<AchievementBase> allAchievements;

	}
}

public sealed class AchievementItem
{
	public readonly AchievementBase baseAchievement;
	public bool isUnlocked { get; private set; }
}
