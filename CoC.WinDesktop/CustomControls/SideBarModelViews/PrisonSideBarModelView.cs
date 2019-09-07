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
	public sealed class PrisonSideBarModelView : SideBarBase
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

		public string prisonStatText
		{
			get => _prisonStatsText;
			private set => CheckPropertyChanged(ref _prisonStatsText, value);
		}
		private string _prisonStatsText = "Prison Stats:";

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
			SelfEsteem = new StatDisplayItemWrapper(playerStats.SelfEsteem, nameof(playerStats.SelfEsteem), silent);
			Willpower = new StatDisplayItemWrapper(playerStats.Willpower, nameof(playerStats.Willpower), silent);
			Obedience = new StatDisplayItemWrapper(playerStats.Obedience, nameof(playerStats.Obedience), silent);

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

		}

		protected override void GetData(StatDataCollectionBase statData)
		{
			PlayerStatData stats = statData.playerStats;

			//Handle stats bar data
			nameText = "Name: " + stats.nameString;
			coreStatText = "Core Stats:";
			combatStatText = "Combat Stats:";

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
