using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Frontend.UI.ControllerData;
using CoC.WinDesktop.ContentWrappers;
using CoC.WinDesktop.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace CoC.WinDesktop.CustomControls.SideBarModelViews
{
	public sealed class PrisonSideBarModelView : SideBarBase
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
		public string prisonStatText => CategoryString(CreatureStatCategory.PRISON);

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
		private readonly StatDisplayItemWrapper SelfEsteem;
		private readonly StatDisplayItemWrapper Willpower;
		private readonly StatDisplayItemWrapper Obedience;

		public PrisonSideBarModelView(StatDataCollectionBase stats, bool silent = false) : base(stats)
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
			SelfEsteem = new StatDisplayItemWrapper(playerStats.SelfEsteem, playerStats.SelfEsteem.statName, silent);
			Willpower = new StatDisplayItemWrapper(playerStats.Willpower, playerStats.Willpower.statName, silent);
			Obedience = new StatDisplayItemWrapper(playerStats.Obedience, playerStats.Obedience.statName, silent);

			var combatStatList = new List<StatDisplayItemWrapper>()
			{
				HP,
				Lust,
				Fatigue,
				Satiety,
				SelfEsteem,
				Willpower,
				Obedience,
			};

			coreStats = new ReadOnlyCollection<StatDisplayItemWrapper>(coreStatList);
			combatStats = new ReadOnlyCollection<StatDisplayItemWrapper>(combatStatList);

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
				RaisePropertyChanged(nameof(prisonStatText));
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

			SelfEsteem.UpdateStats(stats.SelfEsteem);
			Willpower.UpdateStats(stats.Willpower);
			Obedience.UpdateStats(stats.Obedience);
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
		}
	}
}
