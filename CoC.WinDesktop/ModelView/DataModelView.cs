using CoC.Backend.Engine.Time;
using CoC.Frontend.UI;
using CoC.Frontend.UI.ControllerData;
using CoCWinDesktop.CustomControls;
using CoCWinDesktop.ModelView.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.ModelView
{

	//this is a helper - i could do it all in standard, but that gets a bit conviluted. 
	public sealed class DataModelView : ModelViewBase
	{
		private readonly StatDisplayParser statDisplayParser;

		public SideBarBase sideBar
		{
			get => _sideBar;
			private set
			{
				if (_sideBar != value)
				{
					_sideBar = value;
					NotifyPropertyChanged();
				}
			}
		}
		private SideBarBase _sideBar;

		public bool ShowSidebar
		{
			get => _ShowSidebar;
			private set => IHateYouBoat(ref _ShowSidebar, value);
		}
		private bool _ShowSidebar = true;

		public ReadOnlyCollection<SaveDisplayData> gameSaves { get; }
		private List<SaveDisplayData> gameSavesHolder;

		public DataModelView(ModelViewRunner modelViewRunner) : base(modelViewRunner)
		{
			statDisplayParser = new StatDisplayParser(new StatDataCollection(() => new GameDateTime(0,0)), true); //what's passed in here is irrelevant, we just need it for formatting.
			sideBar = statDisplayParser.GetSideBarBase(true, PlayerStatus.IDLE);
		}

		public override event PropertyChangedEventHandler PropertyChanged;

		protected override void ParseDataForDisplay()
		{
			throw new NotImplementedException();
		}

		protected override bool SwitchToThisModelView(Action lastAction)
		{
			throw new NotImplementedException();
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
