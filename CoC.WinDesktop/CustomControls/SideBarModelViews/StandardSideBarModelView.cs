using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Frontend.UI.ControllerData;
using CoCWinDesktop.ContentWrappers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace CoCWinDesktop.CustomControls.SideBarModelViews
{
	public sealed class StandardSideBarModelView : SideBarBase
	{
		private int LastLanguageIndex;

		public string dayStr => DateStr(gameTime);
		public string hourStr => HourStr(gameTime);
		private GameDateTime gameTime
		{
			get => _gameTime;
			set
			{
				if (_gameTime != value)
				{
					_gameTime = value;
					RaisePropertyChanged(nameof(dayStr));
					RaisePropertyChanged(nameof(hourStr));
				}
			}
		}
		private GameDateTime _gameTime;

		public string nameText => base.NameStr(playerName);
		private string playerName
		{
			get => _playerName;
			set
			{
				if (_playerName != value)
				{
					_playerName = value;
					RaisePropertyChanged(nameof(nameText));
				}
			} 
		}
		private string _playerName;

		public string coreStatText => CategoryString(CreatureStatCategory.CORE);
		public ReadOnlyCollection<StatDisplayItemWrapper> coreStats { get; }

		public string combatStatText => CategoryString(CreatureStatCategory.COMBAT);
		public ReadOnlyCollection<StatDisplayItemWrapper> combatStats { get; }

		public string advancementStatText => CategoryString(CreatureStatCategory.ADVANCEMENT);
		public ReadOnlyCollection<StatDisplayItemWrapper> advancementStats { get; }

		private readonly StatDisplayItemWrapper Strength;
		private readonly StatDisplayItemWrapper Toughness;
		private readonly StatDisplayItemWrapper Speed;
		private readonly StatDisplayItemWrapper Intelligence;
		private readonly StatDisplayItemWrapper Libido;
		private readonly StatDisplayItemWrapper Sensitivity;
		private readonly StatDisplayItemWrapper Corruption;

		private readonly StatDisplayItemWrapper HP;
		private readonly StatDisplayItemWrapper Lust;

		private readonly StatDisplayItemWrapper Fatigue;
		private readonly StatDisplayItemWrapper Satiety;

		private readonly StatDisplayItemWrapper Level;
		private readonly StatDisplayItemWrapper XP;
		private readonly StatDisplayItemWrapper Gems;

		public StandardSideBarModelView(StatDataCollectionBase stats, bool silent = false) : base(stats)
		{
			PlayerStatData playerStats = stats.playerStats;

			Strength = new StatDisplayItemWrapper(playerStats.Strength, playerStats.Strength.statName, silent);
			Toughness = new StatDisplayItemWrapper(playerStats.Toughness, playerStats.Toughness.statName, silent);
			Speed = new StatDisplayItemWrapper(playerStats.Speed, playerStats.Speed.statName, silent);
			Intelligence = new StatDisplayItemWrapper(playerStats.Intelligence, playerStats.Intelligence.statName, silent);
			Libido = new StatDisplayItemWrapper(playerStats.Libido, playerStats.Libido.statName, silent);
			Sensitivity = new StatDisplayItemWrapper(playerStats.Sensitivity, playerStats.Sensitivity.statName, silent);
			Corruption = new StatDisplayItemWrapper(playerStats.Corruption, playerStats.Corruption.statName, silent);

			List<StatDisplayItemWrapper> coreStatList = new List<StatDisplayItemWrapper>()
			{
				Strength,
				Toughness,
				Speed,
				Intelligence,
				Libido,
				Sensitivity,
				Corruption,
			};

			HP = new StatDisplayItemWrapper(playerStats.HP, playerStats.HP.statName, silent) { regColorDefaultOrMax = Color.FromArgb(0xFF, 0xA0, 0xFF, 0x50), regColorMin = Color.FromArgb(0xFF, 0xFF, 0x66, 0x50) };
			Lust = new StatDisplayItemWrapper(playerStats.Lust, playerStats.Lust.statName, silent) { regColorDefaultOrMax = Color.FromArgb(0xFF, 0xFF, 0x85, 0x69) };
			Fatigue = new StatDisplayItemWrapper(playerStats.Fatigue, playerStats.Fatigue.statName, silent);
			Satiety = new StatDisplayItemWrapper(playerStats.Satiety, playerStats.Satiety.statName, silent);

			var combatStatList = new List<StatDisplayItemWrapper>()
			{
				HP,
				Lust,
				Fatigue,
				Satiety,
			};

			Level = new StatDisplayItemWrapper(playerStats.Level, playerStats.Level.statName, silent);
			XP = new StatDisplayItemWrapper(playerStats.XP, playerStats.XP.statName, silent);
			Gems = new StatDisplayItemWrapper(playerStats.Gems, playerStats.Gems.statName, silent);

			var advancementStatList = new List<StatDisplayItemWrapper>()
			{
				Level,
				XP,
				Gems,
			};

			coreStats = new ReadOnlyCollection<StatDisplayItemWrapper>(coreStatList);
			combatStats = new ReadOnlyCollection<StatDisplayItemWrapper>(combatStatList);
			advancementStats = new ReadOnlyCollection<StatDisplayItemWrapper>(advancementStatList);

			LastLanguageIndex = LanguageEngine.currentLanguageIndex;
		}

		protected override void GetData(StatDataCollectionBase statData)
		{
			PlayerStatData stats = statData.playerStats;

			//Handle stats bar data - automatic, but only if language hasn't changed. if it has, handle it. 
			if (LastLanguageIndex != LanguageEngine.currentLanguageIndex)
			{
				LastLanguageIndex = LanguageEngine.currentLanguageIndex;
				RaisePropertyChanged(nameof(dayStr));
				RaisePropertyChanged(nameof(hourStr));
				RaisePropertyChanged(nameof(nameText));
				RaisePropertyChanged(nameof(coreStatText));
				RaisePropertyChanged(nameof(combatStatText));
				RaisePropertyChanged(nameof(advancementStatText));
			}
			//similarly, handle changes in gameTime or playerName. these properties fire off the corresponding property changed events automatically. 
			if (gameTime != statData.currentTime)
			{
				gameTime = statData.currentTime;
			}
			if (playerName != statData.playerStats.nameString)
			{
				playerName = statData.playerStats.nameString;
			}

			//manually update stats.
			Strength.UpdateStats(stats.Strength);
			Toughness.UpdateStats(stats.Toughness);
			Speed.UpdateStats(stats.Speed);
			Intelligence.UpdateStats(stats.Intelligence);
			Libido.UpdateStats(stats.Libido);
			Sensitivity.UpdateStats(stats.Sensitivity);
			Corruption.UpdateStats(stats.Corruption);

			HP.UpdateStats(stats.HP);
			Lust.UpdateStats(stats.Lust);

			Fatigue.UpdateStats(stats.Fatigue);
			Satiety.UpdateStats(stats.Satiety);

			Level.UpdateStats(stats.Level);
			XP.UpdateStats(stats.XP);
			Gems.UpdateStats(stats.Gems);
		}

		internal override void ClearArrows()
		{
			foreach (var s in coreStats)
			{
				s.ArrowVisibility = Visibility.Hidden;
			}

			foreach (var s in combatStats)
			{
				s.ArrowVisibility = Visibility.Hidden;
			}
			foreach (var s in advancementStats)
			{
				s.ArrowVisibility = Visibility.Hidden;
			}
		}
	}
}
