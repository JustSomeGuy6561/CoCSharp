using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using CoC.Frontend.UI.ControllerData;
using CoC.UI;
using CoCWinDesktop.ModelView.Helpers;

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

		public StandardSideBarModelView(Controller controller) : base(controller)
		{
#warning: Convert this to manually selecting the right elements, this way it will always work correctly, and not be broken and ugly. 
			PlayerStatData playerStats = controller.playerStats;

			var coreStatList = playerStats.coreStats.Select(x => new StatDisplayItem(x)).ToList();
			var combatStatList = playerStats.combatStats.Select(x => new StatDisplayItem(x)).ToList();

			var HP = combatStatList.Find(x => x.Name == "HP");
			HP.regColorDefaultOrMax = Color.FromArgb(0xFF, 0xA0, 0xFF, 0x50);
			HP.regColorMin = Color.FromArgb(0xFF, 0xFF, 0x66, 0x50);

			var lust = combatStatList.Find(x => x.Name == "lust");
			lust.regColorDefaultOrMax = Color.FromArgb(0xFF, 0xFF, 0x85, 0x69);//0xFFFF8569

			coreStats = new ReadOnlyCollection<StatDisplayItem>(coreStatList);
			combatStats = new ReadOnlyCollection<StatDisplayItem>(combatStatList);

			advancementStats = new ReadOnlyCollection<StatDisplayItem>(playerStats.advancementStats.Select(x => new StatDisplayItem(x)).ToList());

			foreach (var stat in coreStats) stat.CheckText();
			foreach (var stat in combatStats) stat.CheckText();
			foreach (var stat in advancementStats) stat.CheckText();
		}

		protected override void GetData(Controller controller)
		{
			PlayerStatData stats = controller.playerStats;

			//Handle stats bar data
			nameText = "Name: " + stats.nameString;
			coreStatText = "Core Stats:";
			combatStatText = "Combat Stats:";
			advancementStatText = "Advancement:";

			dayStr = "Date: " + controller.currentTime.day.ToString();
			hourStr = "Hour: " + controller.currentTime.GetFormattedHourString();

			//iirc the rest is actually bound via the frontend. woo!
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
