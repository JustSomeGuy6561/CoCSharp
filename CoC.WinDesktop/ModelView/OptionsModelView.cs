using CoCWinDesktop.CustomControls;
using CoCWinDesktop.CustomControls.OptionsModelViews;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace CoCWinDesktop.ModelView
{
	public sealed class OptionsModelView : ModelViewBase
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
		private readonly GameplayOptionsModelView gameplayOptions;
		private readonly InterfaceOptionsModelView interfaceOptions;
		private readonly DisplayOptionsModelView displayOptions;
		private readonly FetishOptionsModelView fetishOptions;
		private readonly SaveOptionsModelView saveOptions;
		private readonly CustomizeControlsModelView customizeControls;

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
			languageOptions = new LanguageOptionsModelView(runner, this);
			gameplayOptions = new GameplayOptionsModelView(runner, this);
			interfaceOptions = new InterfaceOptionsModelView(runner, this);
			fetishOptions = new FetishOptionsModelView(runner, this);
			saveOptions = new SaveOptionsModelView(runner, this);
			customizeControls = new CustomizeControlsModelView(runner, this);

			displayOptions = new DisplayOptionsModelView(runner, this);

			subModelView = gameplayOptions;

			OnLanguageHandle = GenerateCommand(languageOptions);
			OnGameplayHandle = GenerateCommand(gameplayOptions);
			OnInterfaceHandle = GenerateCommand(interfaceOptions);
			OnFetishHandle = GenerateCommand(fetishOptions);
			OnSaveOptionsHandle = GenerateCommand(saveOptions);
			OnControlsHandle = GenerateCommand(customizeControls);

			OnDisplayHandle = new RelayCommand(() => SwitchDisplay(displayOptions), () => true);

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

			OnConfirmHandle = new RelayCommand(HandleConfirmChanges, ConfirmEnabled);

			OnReturnHandle = new RelayCommand(HandleReturn, () => true); //note: it will be collapsed when not available, thus unclickable. 
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
