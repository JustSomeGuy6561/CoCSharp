using CoC.Backend.Engine;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CoC.Backend.Settings.Gameplay
{
	public sealed partial class DifficultySetting : GameplaySetting
	{
		public DifficultySetting(ReadOnlyCollection<GameDifficulty> difficulties) : base(DifficultySettingsStr,
			new DifficultyGameplaySetting(difficulties, false), new DifficultyGameplaySetting(difficulties, true))
		{
		}

		public override void PostLocalSessionInit()
		{
			var glob = (AdvancedSetting)globalSetting;
			var sess = (AdvancedSetting)localSetting;
			sess.setting = glob.setting;
		}
	}
	public sealed partial class DifficultyGameplaySetting : AdvancedSetting
	{
		private BackendSessionSave session => BackendSessionSave.data;
		private BackendGlobalSave glob => BackendGlobalSave.data;
		private readonly bool isGlobal;

		public override int setting
		{
			get => isGlobal ? glob.difficultyGlobal : session.difficulty;
			set
			{
				if (isGlobal) glob.difficultyGlobal = value;
				else session.difficulty = value;
			}
		}

		public override OrderedHashSet<int> availableOptions { get; }

		private readonly ReadOnlyCollection<GameDifficulty> gameDifficulties;

		public DifficultyGameplaySetting(ReadOnlyCollection<GameDifficulty> difficulties, bool usesGlobal) : base()
		{
			gameDifficulties = difficulties ?? throw new ArgumentNullException(nameof(difficulties));
			isGlobal = usesGlobal;


			availableOptions = new OrderedHashSet<int>(Enumerable.Range(0, gameDifficulties.Count).Where(x => gameDifficulties[x] != null));
		}

		public override string SelectedSettingText(int selectedSetting)
		{
			return gameDifficulties[selectedSetting].difficultyStr();
		}

		public override string SelectedSettingHint(int selectedSetting)
		{
			return gameDifficulties[selectedSetting].difficultyHint();
		}

		public override bool SettingEnabled(int possibleSetting, out string whyNot)
		{
			return gameDifficulties[possibleSetting].IsEnabled(isGlobal, out whyNot);
		}

		public override string WarnPlayersAboutChanging()
		{
			return LoweringDifficultyWarning();
		}
	}
}
