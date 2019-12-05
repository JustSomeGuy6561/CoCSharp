using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Backend.Achievements;
using CoC.Backend.Engine;
using CoC.Frontend.UI.ControllerData;
using CoC.WinDesktop.ContentWrappers;
using CoC.WinDesktop.Helpers;
using CoC.WinDesktop.ModelView;

namespace CoC.WinDesktop.CustomControls.ExtraItemModelViews
{
	public sealed partial class MenuAchievementsModelView : ExtraItemModelViewBase
	{
		private int numAchievementsUnlocked;
		private int totalAchievementCount;

		private int LastLanguageIndex;

		public ReadOnlyCollection<AchievementWrapper> achievementDisplays { get; }

		private AchievementData achievementData => runner.controller.achievementData;

		public MenuAchievementsModelView(ModelViewRunner modelViewRunner, ExtraMenuItemsModelView parentModelView) : base(modelViewRunner, parentModelView)
		{
			ContentTitle = AchievementStr();
			ContentHelper = AchievementHelperStr();

			HashSet<AchievementBase> unlockedAchievements = new HashSet<AchievementBase>(achievementData.unlockedAchievements);
			numAchievementsUnlocked = unlockedAchievements.Count;

			List<AchievementWrapper> achievementStore = achievementData.allAchievements.Select(x =>
			{
				bool isUnlocked = unlockedAchievements.Contains(x);
				return new AchievementWrapper(x, isUnlocked);
			}).ToList();

			achievementDisplays = new ReadOnlyCollection<AchievementWrapper>(achievementStore);
			totalAchievementCount = achievementDisplays.Count;

			Content = null; //unused.

			PostContent = PostAchievementStr();
			LastLanguageIndex = LanguageEngine.currentLanguageIndex;

		}

		internal override void ParseDataForDisplay()
		{
			if (LastLanguageIndex != LanguageEngine.currentLanguageIndex)
			{
				LastLanguageIndex = LanguageEngine.currentLanguageIndex;

				//achievement items handled automatically.

				ContentTitle = AchievementStr();
				ContentHelper = AchievementHelperStr();

				PostContent = PostAchievementStr();
			}

			if (runner.controller.AchievementsUnlockedChanged)
			{
				var data = runner.controller.achievementData;
				totalAchievementCount = data.allAchievements.Count;
				numAchievementsUnlocked = data.unlockedAchievements.Count;

				foreach (var unlocked in data.achievementsUnlockedSinceLastQuery)
				{
					var item = achievementDisplays.First(x => ReferenceEquals(x.source, unlocked));
					item.NotifyUnlocked();
				}

				PostContent = PostAchievementStr();

			}
		}
	}
}
