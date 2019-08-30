﻿using CoC.Backend;
using CoC.Backend.Engine;
using CoC.Backend.Settings;
using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.CustomControls.OptionsModelViews
{
	public sealed partial class GameplayOptionsModelView : OptionModelViewDataBase
	{
		private int lastLanguageIndex;

		public string GameplayOptionsText
		{
			get => _gameplayOptionsText;
			private set => CheckPropertyChanged(ref _gameplayOptionsText, value);
		}
		private string _gameplayOptionsText;
		protected override SimpleDescriptor TitleText => GameplayTitleText;

		public string GameplayOptionsHelper
		{
			get => _gameplayOptionsHelper;
			private set => CheckPropertyChanged(ref _gameplayOptionsHelper, value);
		}
		private string _gameplayOptionsHelper;
		protected override SimpleDescriptor TitleHelperText => GameplayHelperText;

		public override SimpleDescriptor ButtonText => GameplayButtonText;

		public ObservableCollection<string> possibilities { get; }

		public int selectedIndex
		{
			get => _selectedIndex;
			set
			{
				if (CheckPrimitivePropertyChanged(ref _selectedIndex, value))
				{
					setState(_selectedIndex != 0);
				}
			}
		}

		private void setState(bool isGlobal)
		{
			gameplayOptions = isGlobal ? globalOptions : sessionOptions;
		}

		private int _selectedIndex;



		public ReadOnlyCollection<OptionsRowBase> gameplayOptions
		{
			get => _gameplayOptions;
			private set => CheckPropertyChanged(ref _gameplayOptions, value);
		}
		private ReadOnlyCollection<OptionsRowBase> _gameplayOptions;

		private readonly ReadOnlyCollection<OptionsRowBase> sessionOptions;
		private readonly ReadOnlyCollection<OptionsRowBase> globalOptions;

		public GameplayOptionsModelView(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView) : base(modelViewRunner, optionsModelView)
		{
			ReadOnlyCollection<GameplaySetting> gameplaySettings = runner.controller.GetGameplaySettings();

			List<OptionsRowBase> options = gameplaySettings.Select(item => OptionsRowBase.BuildOptionRow(item.name, item.localSetting)).Where(x => x != null).ToList();
			sessionOptions = new ReadOnlyCollection<OptionsRowBase>(options);

			options = gameplaySettings.Select(item => OptionsRowBase.BuildOptionRow(item.name, item.globalSetting)).Where(x => x != null).ToList();
			globalOptions = new ReadOnlyCollection<OptionsRowBase>(options);

			lastLanguageIndex = LanguageEngine.currentLanguageIndex;

			_gameplayOptionsText = TitleText();
			_gameplayOptionsHelper = TitleHelperText();

			possibilities = new ObservableCollection<string>()
			{
				"Session",
				"Global"
			};

			setState(_selectedIndex != 0);//make sure the data is correctly initialized.
		}

		public override void ParseDataForDisplay()
		{
			if (lastLanguageIndex != LanguageEngine.currentLanguageIndex)
			{
				lastLanguageIndex = LanguageEngine.currentLanguageIndex;

				GameplayOptionsText = TitleText();
				GameplayOptionsHelper = TitleHelperText();
			}
		}
	}
}
