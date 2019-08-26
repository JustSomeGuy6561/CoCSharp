using CoC.Backend.Engine;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoC.Backend.Settings
{
	public sealed partial class DifficultySettings : AdvancedGameplaySettingBase
	{
		private readonly ReadOnlyCollection<GameDifficulty> gameDifficulties;
		public DifficultySettings(ReadOnlyCollection<GameDifficulty> difficulties) : base(DifficultySettingsStr)
		{
			gameDifficulties = difficulties ?? throw new ArgumentNullException(nameof(difficulties));
			availableSettings = new OrderedHashSet<int>(Enumerable.Range(0, difficulties.Count));
		}

		public override int status
		{
			get => BackendSessionSave.data.difficulty;
			protected set => BackendSessionSave.data.difficulty = value;
		}
		public override int? statusGlobal
		{
			get => BackendGlobalSave.data.difficultyGlobal;
			protected set => BackendGlobalSave.data.difficultyGlobal = value ?? GameEngine.defaultDifficultyIndex;
		}

		public override bool globalCannotBeNull => true;

		protected internal override OrderedHashSet<int> availableSettings { get; }

		private BackendSessionSave session => BackendSessionSave.data;
		private BackendGlobalSave glob => BackendGlobalSave.data;

		public override string settingHint(int status, bool isGlobal)
		{
			return HintStr(gameDifficulties[status], isGlobal);
		}

		public override string settingText(int status, bool isGlobal)
		{
			return gameDifficulties[status].difficultyStr();
		}
	}
}
