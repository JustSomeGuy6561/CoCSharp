using CoC.WinDesktop.ContentWrappers.ButtonWrappers;
using CoC.WinDesktop.CustomControls;
using CoC.WinDesktop.CustomControls.OptionsModelViews;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CoC.WinDesktop.ModelView
{
	public sealed partial class OptionsModelView : ModelViewBase
	{
		public LanguageAwareButtonWrapper DisplayButton { get; }

		public LanguageAwareButtonWrapper ConfirmButton { get; }
		public LanguageAwareButtonWrapper LanguageButton { get; }
		public LanguageAwareButtonWrapper GameplayButton { get; }
		public LanguageAwareButtonWrapper InterfaceButton { get; }
		public LanguageAwareButtonWrapper FetishButton { get; }
		public LanguageAwareButtonWrapper SaveButton { get; }
		public LanguageAwareButtonWrapper CustomControlsButton { get; }
		//public LanguageAwareButtonWrapper DebugButton { get; }
		public LanguageAwareButtonWrapper ReturnButton { get; }

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

		private readonly Dictionary<OptionModelViewDataBase, LanguageAwareButtonWrapper> contentToButtonLookup;

		private readonly List<LanguageAwareButtonWrapper> allButtons;
		private readonly List<LanguageAwareButtonWrapper> primaryButtons;

		public OptionsModelView(ModelViewRunner modelViewRunner) : base(modelViewRunner)
		{


			displayOptions = new DisplayOptionsModelView(runner, this);
			DisplayButton = new LanguageAwareButtonWrapper(displayOptions.ButtonText, () => SwitchDisplay(displayOptions), null, null);

			languageOptions = new LanguageOptionsModelView(runner, this);
			LanguageButton = new LanguageAwareButtonWrapper(languageOptions.ButtonText, () => SwitchDisplay(languageOptions), null, null);

			gameplayOptions = new GameplayOptionsModelView(runner, this);
			GameplayButton = new LanguageAwareButtonWrapper(gameplayOptions.ButtonText, () => SwitchDisplay(gameplayOptions), null, null);

			interfaceOptions = new InterfaceOptionsModelView(runner, this);
			InterfaceButton = new LanguageAwareButtonWrapper(interfaceOptions.ButtonText, () => SwitchDisplay(interfaceOptions), null, null);

			fetishOptions = new FetishOptionsModelView(runner, this);
			FetishButton = new LanguageAwareButtonWrapper(fetishOptions.ButtonText, () => SwitchDisplay(fetishOptions), null, null);

			saveOptions = new SaveOptionsModelView(runner, this);
			SaveButton = new LanguageAwareButtonWrapper(saveOptions.ButtonText, () => SwitchDisplay(saveOptions), null, null);

			customizeControls = new CustomizeControlsModelView(runner, this);
			CustomControlsButton = new LanguageAwareButtonWrapper(customizeControls.ButtonText, () => SwitchDisplay(customizeControls), null, null);

			ConfirmButton = new LanguageAwareButtonWrapper(ConfirmTextStr, HandleConfirmChanges, null, null);
			ReturnButton = new LanguageAwareButtonWrapper(ReturnTextStr, HandleReturn, null, null);

			allButtons = new List<LanguageAwareButtonWrapper>()
			{
				LanguageButton, GameplayButton, InterfaceButton, FetishButton, SaveButton, ConfirmButton, /*DebugButton,*/ ReturnButton, DisplayButton, CustomControlsButton
			};

			primaryButtons = new List<LanguageAwareButtonWrapper>()
			{
				LanguageButton, GameplayButton, InterfaceButton, FetishButton, SaveButton, ReturnButton, /*DebugButton,*/ DisplayButton, CustomControlsButton
			};


			//this requires both the option and the command to be initialized, so it's last.
			contentToButtonLookup = new Dictionary<OptionModelViewDataBase, LanguageAwareButtonWrapper>()
			{
				[languageOptions] = LanguageButton,
				[gameplayOptions] = GameplayButton,
				[interfaceOptions] = InterfaceButton,
				[fetishOptions] = FetishButton,
				[saveOptions] = SaveButton,
				[customizeControls] = CustomControlsButton,
				[displayOptions] = DisplayButton,
				//[debugOptions] = DebugButton,
			};

			_subModelView = gameplayOptions;
			contentToButtonLookup[subModelView].SetEnabled(false);

			UpdateButtonVisibility(_subModelView.requiresConfirmation);
		}

		public void OnLanguageChange()
		{
			allButtons.ForEach(x => x.OnLanguageChanged());

			subModelView.ParseDataForDisplay();
		}


		private void SwitchDisplay(OptionModelViewDataBase newDisplay)
		{
			if (newDisplay != subModelView)
			{
				var oldView = subModelView;
				UpdateButtonVisibility(newDisplay.requiresConfirmation);

				subModelView = newDisplay;
				//update the buttons' enabled properties.
				contentToButtonLookup[oldView].SetEnabled(true);
				contentToButtonLookup[subModelView].SetEnabled(false);
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

		private void HandleReturn()
		{
			runner.SwitchToMainMenu();
		}

		protected override void ParseDataForDisplay()
		{
			subModelView.ParseDataForDisplay();
			UpdateButtonVisibility(subModelView.requiresConfirmation);
		}

		private void UpdateButtonVisibility(bool requiresConfirmation)
		{
			Visibility visibility = requiresConfirmation ? Visibility.Collapsed : Visibility.Visible;
			foreach (var button in primaryButtons)
			{
				button.SetVisibility(visibility);
			}

			visibility = requiresConfirmation ? Visibility.Visible : Visibility.Collapsed;
			ConfirmButton.SetVisibility(visibility);
		}
	}
}
