using CoC.Frontend.UI;
using CoC.Frontend.UI.ControllerData;
using CoCWinDesktop.ContentWrappers;
using CoCWinDesktop.CustomControls;
using CoCWinDesktop.Helpers;
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
			private set => CheckPropertyChanged(ref _sideBar, value);
		}
		private SideBarBase _sideBar;

		public bool ShowSidebar
		{
			get => _ShowSidebar;
			private set => CheckPrimitivePropertyChanged(ref _ShowSidebar, value);
		}
		private bool _ShowSidebar = true;

		public ReadOnlyCollection<SaveDisplayDataWrapper> gameSaves
		{
			get => _gameSaves;
			private set => CheckPropertyChanged(ref _gameSaves, value);
		}
		private ReadOnlyCollection<SaveDisplayDataWrapper> _gameSaves;

		//for now. 
#pragma warning disable IDE0044 // Add readonly modifier
		private List<SaveDisplayDataWrapper> gameSavesHolder;
#pragma warning restore IDE0044 // Add readonly modifier

		public SaveDisplayDataWrapper selectedItem
		{
			get => _selectedItem;
			set
			{
				if (CheckPropertyChanged(ref _selectedItem, value))
				{
					displaySource = value?.saveStatData;
				}
			}
		}
		private SaveDisplayDataWrapper _selectedItem;

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
				if (CheckPrimitivePropertyChanged(ref _optionsTextVisible, value))
				{ 
					RaisePropertyChanged(nameof(SaveItemsVisible));
				}
			}
		}
		private bool _optionsTextVisible = false;

		public bool SaveItemsVisible => !OptionsTextVisible;

		//if language change set this to true; set this to true when options Command is clicked or return is clicked;
		private bool forceUpdate = true; //initially true so it renders. set to true whenever we leave this view. 

		//on Options command, set this to true.

		private readonly StatDataCollectionBase emptyDisplay = new SaveDataCollection(null);

		public DataModelView(ModelViewRunner modelViewRunner) : base(modelViewRunner)
		{
			//get last known save location from runner.

			statDisplayParser = new StatDisplayParser(emptyDisplay, true);
			sideBar = statDisplayParser.GetSideBarBase(true, PlayerStatus.IDLE);

			//load the data into gameSavesHolder
			gameSavesHolder = new List<SaveDisplayDataWrapper>();
			gameSaves = new ReadOnlyCollection<SaveDisplayDataWrapper>(gameSavesHolder);

			displaySource = emptyDisplay;
		}

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
			runner.SwitchToPreviousView();
		}
	}
}
