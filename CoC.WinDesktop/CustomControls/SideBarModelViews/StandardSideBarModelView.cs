using CoC.Frontend.UI.ControllerData;
using CoCWinDesktop.ModelView.Helpers;
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
			private set => IHateYouBoat(ref _dayStr, value);
		}
		private string _dayStr = "";

		public string hourStr
		{
			get => _hourStr;
			private set => IHateYouBoat(ref _hourStr, value);
		}
		private string _hourStr = "";

		public string nameText
		{
			get => _nameText;
			private set => IHateYouBoat(ref _nameText, value);
		}
		private string _nameText = "Name: ";

		public string coreStatText
		{
			get => _coreStatsText;
			private set => IHateYouBoat(ref _coreStatsText, value);
		}
		private string _coreStatsText = "Core Stats:";

		public ReadOnlyCollection<StatDisplayItem> coreStats { get; }

		public string combatStatText
		{
			get => _combatStatsText;
			private set => IHateYouBoat(ref _combatStatsText, value);
		}
		private string _combatStatsText = "Combat Stats:";

		public ReadOnlyCollection<StatDisplayItem> combatStats { get; }

		public string advancementStatText
		{
			get => _advancementStatsText;
			private set => IHateYouBoat(ref _advancementStatsText, value);
		}
		private string _advancementStatsText = "Advancement:";

		public override event PropertyChangedEventHandler PropertyChanged;

		public ReadOnlyCollection<StatDisplayItem> advancementStats { get; }

		private readonly StatDisplayItem Strength;
		private readonly StatDisplayItem Toughness;
		private readonly StatDisplayItem Speed;
		private readonly StatDisplayItem Intelligence;
		private readonly StatDisplayItem Libido;
		private readonly StatDisplayItem Sensitivity;
		private readonly StatDisplayItem Corruption;

		private readonly StatDisplayItem HP;
		private readonly StatDisplayItem Lust;

		private readonly StatDisplayItem Fatigue;
		private readonly StatDisplayItem Satiety;

		private readonly StatDisplayItem Level;
		private readonly StatDisplayItem XP;
		private readonly StatDisplayItem Gems;

		public StandardSideBarModelView(StatDataCollection stats, bool silent = false) : base(stats)
		{
#warning: Convert this to manually selecting the right elements, this way it will always work correctly, and not be broken and ugly. 
			PlayerStatData playerStats = stats.playerStats;

			Strength = new StatDisplayItem(playerStats.Strength, nameof(playerStats.Strength), silent);
			Toughness = new StatDisplayItem(playerStats.Toughness, nameof(playerStats.Toughness), silent);
			Speed = new StatDisplayItem(playerStats.Speed, nameof(playerStats.Speed), silent);
			Intelligence = new StatDisplayItem(playerStats.Intelligence, nameof(playerStats.Intelligence), silent);
			Libido = new StatDisplayItem(playerStats.Libido, nameof(playerStats.Libido), silent);
			Sensitivity = new StatDisplayItem(playerStats.Sensitivity, nameof(playerStats.Sensitivity), silent);
			Corruption = new StatDisplayItem(playerStats.Corruption, nameof(playerStats.Corruption), silent);

			List<StatDisplayItem> coreStatList = new List<StatDisplayItem>()
			{
				Strength,
				Toughness,
				Speed,
				Intelligence,
				Libido,
				Sensitivity,
				Corruption,
			};

			HP = new StatDisplayItem(playerStats.HP, nameof(playerStats.HP), silent) { regColorDefaultOrMax = Color.FromArgb(0xFF, 0xA0, 0xFF, 0x50), regColorMin = Color.FromArgb(0xFF, 0xFF, 0x66, 0x50) };
			Lust = new StatDisplayItem(playerStats.Lust, nameof(playerStats.Lust), silent) { regColorDefaultOrMax = Color.FromArgb(0xFF, 0xFF, 0x85, 0x69) };
			Fatigue = new StatDisplayItem(playerStats.Fatigue, nameof(playerStats.Fatigue), silent);
			Satiety = new StatDisplayItem(playerStats.Satiety, nameof(playerStats.Satiety), silent);

			var combatStatList = new List<StatDisplayItem>()
			{
				HP,
				Lust,
				Fatigue,
				Satiety,
			};

			Level = new StatDisplayItem(playerStats.Level, nameof(playerStats.Level), silent);
			XP = new StatDisplayItem(playerStats.XP, nameof(playerStats.XP), silent);
			Gems = new StatDisplayItem(playerStats.Gems, nameof(playerStats.Gems), silent);

			var advancementStatList = new List<StatDisplayItem>()
			{
				Level,
				XP,
				Gems,
			};

			coreStats = new ReadOnlyCollection<StatDisplayItem>(coreStatList);
			combatStats = new ReadOnlyCollection<StatDisplayItem>(combatStatList);
			advancementStats = new ReadOnlyCollection<StatDisplayItem>(advancementStatList);
		}

		protected override void GetData(StatDataCollection statData)
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

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


		private void IHateYouBoat<T>(ref T data, T newValue, [CallerMemberName] string propertyName = "") where T : IEquatable<T>
		{
			if (data == null != (newValue == null) || (data != null && !data.Equals(newValue)))
			{
				data = newValue;
				NotifyPropertyChanged(propertyName);
			}
		}
	}
}
