using CoC.Frontend.UI;
using CoC.Frontend.UI.ControllerData;
using CoCWinDesktop.CustomControls;
using CoCWinDesktop.ModelView.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

		public ReadOnlyCollection<SaveDisplayData> gameSaves
		{
			get => _gameSaves;
			private set
			{
				if (_gameSaves != value)
				{
					_gameSaves = value;
					NotifyPropertyChanged();
				}
			}
		}
		private ReadOnlyCollection<SaveDisplayData> _gameSaves;

		private List<SaveDisplayData> gameSavesHolder;

		public SaveDisplayData selectedItem
		{
			get => _selectedItem;
			set
			{
				if (_selectedItem != value)
				{
					_selectedItem = value;
					NotifyPropertyChanged();
					displaySource = value?.saveStatData;
				}
			}
		}
		private SaveDisplayData _selectedItem;

		private StatDataCollectionBase displaySource
		{
			get => _displaySource;
			set
			{
				if (value is null)
				{
					value = emptyDisplay;
				}

				if (_displaySource != value)
				{
					var oldSource = _displaySource;
					_displaySource = value;

					//if (oldSource != null)
					//{
					sideBar.UpdateSidebar(value);
					//}
				}
			}
		}
		private StatDataCollectionBase _displaySource;

		private bool optionsView
		{
			get => _optionsView;
			set
			{
				_optionsView = value;
				OptionsTextVisible = value;
			}
		}
		private bool _optionsView = false;
		public bool OptionsTextVisible
		{
			get => _optionsTextVisible;
			private set
			{
				if (_optionsTextVisible != value)
				{
					_optionsTextVisible = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(SaveItemsVisible));
				}
			}
		}
		private bool _optionsTextVisible = false;

		public bool SaveItemsVisible => !OptionsTextVisible;

		//if language change set this to true; set this to true when options Command is clicked or return is clicked;
		private bool forceUpdate = true; //initially true so it renders. set to true whenever we leave this view. 

		//on Options command, set this to true.

		private readonly StatDataCollectionBase emptyDisplay = new SaveDataCollection(null);

		private Action onCancel;

		public DataModelView(ModelViewRunner modelViewRunner) : base(modelViewRunner)
		{
			//get last known save location from runner.

			statDisplayParser = new StatDisplayParser(emptyDisplay, true);
			sideBar = statDisplayParser.GetSideBarBase(true, PlayerStatus.IDLE);

			//load the data into gameSavesHolder
			gameSavesHolder = new List<SaveDisplayData>();
			gameSaves = new ReadOnlyCollection<SaveDisplayData>(gameSavesHolder);

			displaySource = emptyDisplay;
		}

		public override event PropertyChangedEventHandler PropertyChanged;

		protected override void ParseDataForDisplay()
		{
			if (forceUpdate)
			{
				if (!optionsView)
				{
					UpdateSavesPage();
				}
				else
				{
					UpdateOptionsPage();
				}
			}
			forceUpdate = false;
		}

		protected override bool SwitchToThisModelView(Action lastAction)
		{
			onCancel = lastAction;
#warning TODO: Implement this when saves are actually in place. for now it'll just fail.
			//ParseData(); return true;
			return false;
		}

		private void OnOptionsCommand()
		{
			optionsView = true;
			selectedItem = null;

			forceUpdate = true;

			ParseData();
		}

		private void OnOptionsAction()
		{
			forceUpdate = true;

			ParseData();
		}

		private void UpdateSavesPage()
		{
			//update the buttons text and commands to correct values. 

			//if game save location changed:
			//update gameSavesHolder, gameSaves
			//else force it to focus on the selected item, if it's not null.

			sideBar.UpdateSidebar(displaySource);
		}

		private void UpdateOptionsPage()
		{
			//update the buttons text and commands to correct values. 

			//Display the text to notify the player of current settings. 

			throw new NotImplementedException();
		}

		private void OnReturnFromOptionsCommand()
		{
			optionsView = false;
			forceUpdate = true;

			ParseData();
		}

		private void OnCancelCommand()
		{
			forceUpdate = true;
			onCancel();
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
