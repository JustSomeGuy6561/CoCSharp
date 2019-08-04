using CoC.Backend;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Achievements
{
	public abstract class GenericAchievementBase : AchievementBase
	{
		public readonly SimpleDescriptor nameDesc;
		public readonly SimpleDescriptor achievementLockedTextDesc;
		public readonly SimpleDescriptor achievementUnlockedTextDesc;

		internal GenericAchievementBase(SimpleDescriptor achievementName, SimpleDescriptor lockedText, SimpleDescriptor unlockedText) : base()
		{
			nameDesc = achievementName ?? throw new ArgumentNullException(nameof(achievementName));
			achievementLockedTextDesc = lockedText ?? throw new ArgumentNullException(nameof(lockedText));
			achievementUnlockedTextDesc = unlockedText ?? throw new ArgumentNullException(nameof(unlockedText));
		}

		public override string AchievementLockedText()
		{
			return achievementLockedTextDesc();
		}

		public override string AchievementUnlockedText()
		{
			return achievementUnlockedTextDesc();
		}

		public override string Name()
		{
			return nameDesc();
		}


	}
}
