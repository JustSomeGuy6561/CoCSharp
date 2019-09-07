using CoC.Frontend.UI.ControllerData;
using CoCWinDesktop.ContentWrappers;
using CoCWinDesktop.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace CoCWinDesktop.CustomControls.SideBarModelViews
{
	public sealed class StandardSideBarModelView : SideBarBase
	{

		public string dayStr
		{
			get => _dayStr;
			private set => CheckPropertyChanged(ref _dayStr, value);
		}
		private string _dayStr = "";

		public string hourStr
		{
			get => _hourStr;
			private set => CheckPropertyChanged(ref _hourStr, value);
		}
		private string _hourStr = "";

		public string nameText
		{
			get => _nameText;
			private set => CheckPropertyChanged(ref _nameText, value);
		}
		private string _nameText = "Name: ";

		public string coreStatText
		{
			get => _coreStatsText;
			private set => CheckPropertyChanged(ref _coreStatsText, value);
		}
		private string _coreStatsText = "Core Stats:";

		public ReadOnlyCollection<StatDisplayItemWrapper> coreStats { get; }

		public string combatStatText
		{
			get => _combatStatsText;
			private set => CheckPropertyChanged(ref _combatStatsText, value);
		}
		private string _combatStatsText = "Combat Stats:";

		public ReadOnlyCollection<StatDisplayItemWrapper> combatStats { get; }

		public string advancementStatText
		{
			get => _advancementStatsText;
			private set => CheckPropertyChanged(ref _advancementStatsText, value);
		}
		private string _advancementStatsText = "Advancement:";


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

			Strength = new StatDisplayItemWrapper(playerStats.Strength, nameof(playerStats.Strength), silent);
			Toughness = new StatDisplayItemWrapper(playerStats.Toughness, nameof(playerStats.Toughness), silent);
			Speed = new StatDisplayItemWrapper(playerStats.Speed, nameof(playerStats.Speed), silent);
			Intelligence = new StatDisplayItemWrapper(playerStats.Intelligence, nameof(playerStats.Intelligence), silent);
			Libido = new StatDisplayItemWrapper(playerStats.Libido, nameof(playerStats.Libido), silent);
			Sensitivity = new StatDisplayItemWrapper(playerStats.Sensitivity, nameof(playerStats.Sensitivity), silent);
			Corruption = new StatDisplayItemWrapper(playerStats.Corruption, nameof(playerStats.Corruption), silent);

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

			HP = new StatDisplayItemWrapper(playerStats.HP, nameof(playerStats.HP), silent) { regColorDefaultOrMax = Color.FromArgb(0xFF, 0xA0, 0xFF, 0x50), regColorMin = Color.FromArgb(0xFF, 0xFF, 0x66, 0x50) };
			Lust = new StatDisplayItemWrapper(playerStats.Lust, nameof(playerStats.Lust), silent) { regColorDefaultOrMax = Color.FromArgb(0xFF, 0xFF, 0x85, 0x69) };
			Fatigue = new StatDisplayItemWrapper(playerStats.Fatigue, nameof(playerStats.Fatigue), silent);
			Satiety = new StatDisplayItemWrapper(playerStats.Satiety, nameof(playerStats.Satiety), silent);

			var combatStatList = new List<StatDisplayItemWrapper>()
			{
				HP,
				Lust,
				Fatigue,
				Satiety,
			};

			Level = new StatDisplayItemWrapper(playerStats.Level, nameof(playerStats.Level), silent);
			XP = new StatDisplayItemWrapper(playerStats.XP, nameof(playerStats.XP), silent);
			Gems = new StatDisplayItemWrapper(playerStats.Gems, nameof(playerStats.Gems), silent);

			var advancementStatList = new List<StatDisplayItemWrapper>()
			{
				Level,
				XP,
				Gems,
			};

			coreStats = new ReadOnlyCollection<StatDisplayItemWrapper>(coreStatList);
			combatStats = new ReadOnlyCollection<StatDisplayItemWrapper>(combatStatList);
			advancementStats = new ReadOnlyCollection<StatDisplayItemWrapper>(advancementStatList);
		}

		protected override void GetData(StatDataCollectionBase statData)
		{
			PlayerStatData stats = statData.playerStats;

			//Handle stats bar data
			nameText = "Name: " + stats.nameString;
			coreStatText = "Core Stats:";
			combatStatText = "Combat Stats:";
			advancementStatText = "Advancement:";

			dayStr = "Date: " + statData.currentTime.day.ToString();
			hourStr = "Hour: " + statData.currentTime.GetFormattedHourString();

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
