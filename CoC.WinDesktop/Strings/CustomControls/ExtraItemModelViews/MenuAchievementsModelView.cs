using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.WinDesktop.ModelView;

namespace CoC.WinDesktop.CustomControls.ExtraItemModelViews
{
	public partial class MenuAchievementsModelView
	{
		private static string AchievementStr()
		{
			return "Achievements";
		}

		private static string AchievementHelperStr()
		{
			return "Locked achievements will appear less visible than their unlocked counterparts.";
		}

		private string PostAchievementStr()
		{
			double percent = numAchievementsUnlocked * 1.0 / totalAchievementCount;
			return $"Achievement progress: {numAchievementsUnlocked}/{totalAchievementCount} ({percent:P0})";
		}
	}
}
