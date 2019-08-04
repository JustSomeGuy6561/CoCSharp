using CoC.Backend;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Achievements
{
	public abstract class AchievementBase
	{
		internal AchievementBase()
		{

		}

		public abstract string Name();
		public abstract string AchievementLockedText();
		public abstract string AchievementUnlockedText();

		public bool isUnlocked()
		{
			return UnlockConditions();
		}

		protected abstract bool UnlockConditions();
	}
}
