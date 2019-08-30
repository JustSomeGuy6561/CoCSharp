using CoCWinDesktop.CustomControls;
using CoCWinDesktop.CustomControls.OptionsModelViews;
using System.Collections.Generic;

namespace CoCWinDesktop.ModelView
{
	public sealed partial class OptionsModelView : ModelViewBase
	{
		public RelayCommand OnGameplayHandle { get; }

		public RelayCommand OnInterfaceHandle { get; }

		public RelayCommand OnDisplayHandle { get; }

		public RelayCommand OnFetishHandle { get; }
		public RelayCommand OnControlsHandle { get; }
		public RelayCommand OnSaveOptionsHandle { get; }
		public RelayCommand OnLanguageHandle { get; }


		//special case that does weird shit.
		public RelayCommand OnConfirmHandle { get; }
		//public RelayCommand OnCancelHandle { get; } //we _should_ have a cancel button, but supporting that seems like a lot of work lol. 

		public RelayCommand OnReturnHandle { get; }

		public OptionModelViewDataBase subModelView
		{
			get => _subModelView;
			private set
			{
				OptionModelViewDataBase currModel = _subModelView;
				if (CheckPropertyChanged(ref _subModelView, value))
				{
					_subModelView.OnSwitchTo();
					previousSubModel = currModel;
				}
			}
		}
		private OptionModelViewDataBase _subModelView;

		private OptionModelViewDataBase previousSubModel;

		private readonly LanguageOptionsModelView languageOptions;
		public string LanguageText => languageOptions.ButtonText();

		private readonly GameplayOptionsModelView gameplayOptions;
		public string GameplayText => gameplayOptions.ButtonText();

		private readonly InterfaceOptionsModelView interfaceOptions;
		public string InterfaceText => interfaceOptions.ButtonText();

		private readonly DisplayOptionsModelView displayOptions;
		public string DisplayText => displayOptions.ButtonText();

		private readonly FetishOptionsModelView fetishOptions;
		public string FetishText => fetishOptions.ButtonText();

		private readonly SaveOptionsModelView saveOptions;
		public string SaveText => saveOptions.ButtonText();

		private readonly CustomizeControlsModelView customizeControls;
		public string CustomizeText => customizeControls.ButtonText();

		public string ConfirmText => ConfirmTextStr();

		public string ReturnText => ReturnTextStr();

		public bool primaryButtonsVisible
		{
			get => _primaryButtonsVisible;
			private set
			{
				if (CheckPrimitivePropertyChanged(ref _primaryButtonsVisible, value))
				{
					OnConfirmHandle.RaiseExecuteChanged();
				}
			}
		}
		private bool _primaryButtonsVisible = true;

		private readonly Dictionary<OptionModelViewDataBase, RelayCommand> contentToCommandLookup;

		public OptionsModelView(ModelViewRunner modelViewRunner) : base(modelViewRunner)
		{
			//order of operations matters. we'll do the confirm and return first because they don't require anything to be initialized to work.
			OnConfirmHandle = new RelayCommand(HandleConfirmChanges, ConfirmEnabled);

			OnReturnHandle = new RelayCommand(HandleReturn, () => true); //note: it will be collapsed when not available, thus unclickable. 

			//commands require their respective modelview be initialized, so we do them after the modelview initializer. 
			//display is a part of interface, so we need to create it before interface. 
			displayOptions = new DisplayOptionsModelView(runner, this);
			OnDisplayHandle = new RelayCommand(() => SwitchDisplay(displayOptions), () => true);

			languageOptions = new LanguageOptionsModelView(runner, this);
			OnLanguageHandle = GenerateCommand(languageOptions);

			gameplayOptions = new GameplayOptionsModelView(runner, this);
			OnGameplayHandle = GenerateCommand(gameplayOptions);

			interfaceOptions = new InterfaceOptionsModelView(runner, this);
			OnInterfaceHandle = GenerateCommand(interfaceOptions);

			fetishOptions = new FetishOptionsModelView(runner, this);
			OnFetishHandle = GenerateCommand(fetishOptions);

			saveOptions = new SaveOptionsModelView(runner, this);
			OnSaveOptionsHandle = GenerateCommand(saveOptions);

			customizeControls = new CustomizeControlsModelView(runner, this);
			OnControlsHandle = GenerateCommand(customizeControls);

			//this requires both the option and the command to be initialized, so it's last. 
			contentToCommandLookup = new Dictionary<OptionModelViewDataBase, RelayCommand>()
			{
				[languageOptions] = OnLanguageHandle,
				[gameplayOptions] = OnGameplayHandle,
				[interfaceOptions] = OnInterfaceHandle,
				[fetishOptions] = OnFetishHandle,
				[saveOptions] = OnSaveOptionsHandle,
				[customizeControls] = OnControlsHandle,
				[displayOptions] = OnDisplayHandle,
			};

			_subModelView = gameplayOptions;
		}

		public void OnLanguageChange()
		{
			RaisePropertyChanged(nameof(LanguageText));
			RaisePropertyChanged(nameof(GameplayText));
			RaisePropertyChanged(nameof(InterfaceText));
			RaisePropertyChanged(nameof(DisplayText));
			RaisePropertyChanged(nameof(FetishText));
			RaisePropertyChanged(nameof(SaveText));
			RaisePropertyChanged(nameof(CustomizeText));
			RaisePropertyChanged(nameof(ConfirmText));
			RaisePropertyChanged(nameof(ReturnText));

			subModelView.ParseDataForDisplay();
		}

		private RelayCommand GenerateCommand(OptionModelViewDataBase target)
		{
			return new RelayCommand(() => SwitchDisplay(target), () => CheckButtonEnabled(target));
		}


		private void SwitchDisplay(OptionModelViewDataBase newDisplay)
		{
			if (newDisplay != subModelView)
			{
				var oldView = subModelView;
				//update the buttons.
				if (newDisplay.requiresConfirmation)
				{
					primaryButtonsVisible = false;
					//update the confirm button target.
				}
				else
				{
					primaryButtonsVisible = true;
				}

				subModelView = newDisplay;
				//update the buttons' enabled properties. 
				contentToCommandLookup[oldView].RaiseExecuteChanged();
				contentToCommandLookup[subModelView].RaiseExecuteChanged();
				ParseData();
			}
		}

		private bool CheckButtonEnabled(OptionModelViewDataBase target)
		{
			return target != subModelView;
		}

		private void HandleConfirmChanges()
		{
			subModelView.OnConfirmation();
			SwitchDisplay(previousSubModel);
		}

		private bool ConfirmEnabled()
		{
			return !primaryButtonsVisible;
		}

		private void HandleReturn()
		{
			runner.SwitchToMainMenu();
		}

		protected override void ParseDataForDisplay()
		{
			subModelView.ParseDataForDisplay();
			if (subModelView.requiresConfirmation)
			{
				primaryButtonsVisible = false;
				//update the confirm button target.
			}
			else
			{
				primaryButtonsVisible = true;
			}
		}
	}
}
