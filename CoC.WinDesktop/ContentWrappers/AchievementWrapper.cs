using CoC.Backend.Achievements;
using CoCWinDesktop.Helpers;
using System;

namespace CoCWinDesktop.ContentWrappers
{
	public sealed class AchievementWrapper : NotifierBase
	{
		//View objects are created when needed, and destroyed when not needed. The achievements view objects will never be in scope when the language changes,
		//so we don't need to worry about language changing in these objects. 

		public readonly AchievementBase source;

		public string AchievementName => source.name();

		//it's technically possible (thout afaik it'll never happen) that viewing the achievements causes an achievement to unlock. Thus, this needs to be notified.
		//because it's technically possible for this to occur while the view objects are in scope.
		public string AchievementText => IsUnlocked ? source.AchievementUnlockedText() : source.AchievementLockedText();

		//To account for the above possibility, we notify the text changed on unlock changed.
		public bool IsUnlocked
		{
			get => _isUnlocked;
			private set
			{
				if (CheckPrimitivePropertyChanged(ref _isUnlocked, value))
				{
					RaisePropertyChanged(nameof(AchievementText));
				}
			}
		}
		private bool _isUnlocked;

		public AchievementWrapper(AchievementBase achievementBase, bool currentlyUnlocked)
		{
			source = achievementBase ?? throw new ArgumentNullException(nameof(achievementBase));

			_isUnlocked = currentlyUnlocked;
		}

		internal void NotifyUnlocked()
		{
			IsUnlocked = true;
		}
	}
}
