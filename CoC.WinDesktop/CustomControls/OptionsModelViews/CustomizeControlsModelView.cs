using CoC.Backend;
using CoC.Backend.Engine;
using CoC.WinDesktop.ContentWrappers;
using CoC.WinDesktop.Helpers;
using CoC.WinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CoC.WinDesktop.CustomControls.OptionsModelViews
{
	public sealed partial class CustomizeControlsModelView : OptionModelViewDataBase
	{

		private int lastLanguageIndex;
		public string CustomControlsText
		{
			get => _CustomControlsText;
			private set => CheckPropertyChanged(ref _CustomControlsText, value);
		}
		private string _CustomControlsText;
		protected override SimpleDescriptor TitleText => CustomControlsTitleText;

		public string CustomControlsHelper
		{
			get => _CustomControlsHelper;
			private set => CheckPropertyChanged(ref _CustomControlsHelper, value);
		}
		private string _CustomControlsHelper;
		protected override SimpleDescriptor TitleHelperText => CustomControlsHelperText;
	
		public override SimpleDescriptor ButtonText => CustomControlsButtonText;

		public string CustomControlsHowToText => HowToStr();

		public ReadOnlyCollection<HotKeyWrapper> hotKeys { get; }

		public CustomizeControlsModelView(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView) : base(modelViewRunner, optionsModelView)
		{
#warning Remember to run validation of hotkeys when loading from a save, so we know two keys dont have the same key combo. Assuming initially valid, it will remain that way.
			//This validates that whenever a new combo is created, but we cannot validate that elsewhere
			List<HotKeyWrapper> items = new List<HotKeyWrapper>()
			{
				runner.StatHotkey,
				runner.LevelHotkey,
				runner.MainMenuHotkey,
				runner.DataHotkey,
				runner.AppearanceHotkey,
				runner.PerksHotkey,
				runner.QuickSaveHotkey,
				runner.QuickLoadHotkey,
				runner.Button1Hotkey,
				runner.Button2Hotkey,
				runner.Button3Hotkey,
				runner.Button4Hotkey,
				runner.Button5Hotkey,
				runner.Button6Hotkey,
				runner.Button7Hotkey,
				runner.Button8Hotkey,
				runner.Button9Hotkey,
				runner.Button10Hotkey,
				runner.Button11Hotkey,
				runner.Button12Hotkey,
				runner.Button13Hotkey,
				runner.Button14Hotkey,
				runner.Button15Hotkey,
			};
			items.ForEach(x => x.PropertyChanged += OnHotkeyChanged);

			hotKeys = new ReadOnlyCollection<HotKeyWrapper>(items);

			lastLanguageIndex = LanguageEngine.currentLanguageIndex;

			_CustomControlsText = TitleText();
			_CustomControlsHelper = TitleHelperText();


		}

		private void OnHotkeyChanged(object sender, PropertyChangedEventArgs e)
		{
			var hotkeyWrapper = sender as HotKeyWrapper;

			if (e.PropertyName == nameof(hotkeyWrapper.primaryGesture) || e.PropertyName == nameof(hotkeyWrapper.secondaryGesture))
			{
				bool primary = e.PropertyName == nameof(hotkeyWrapper.primaryGesture);
				HotKey newValue = primary ? hotkeyWrapper.primaryGesture : hotkeyWrapper.secondaryGesture;

				if (newValue is null)
				{
					return;
				}

				foreach (var hotKey in hotKeys)
				{
					if ((!ReferenceEquals(sender, hotKey) || !primary) && hotKey.primaryGesture == newValue)
					{
						hotKey.primaryGesture = null;
					}

					if ((!ReferenceEquals(sender, hotKey) || primary) && hotKey.secondaryGesture == newValue)
					{
						hotKey.secondaryGesture = null;
					}
				}
			}
		}

		public override void ParseDataForDisplay()
		{
			if (lastLanguageIndex != LanguageEngine.currentLanguageIndex)
			{
				lastLanguageIndex = LanguageEngine.currentLanguageIndex;

				CustomControlsText = TitleText();
				CustomControlsHelper = TitleHelperText();
			}
		}

		public override Action OnConfirmation => ConfirmControls;



		private void ConfirmControls()
		{
			//Do Nothing, i guess. I suppose we should write the commands to the save here.
		}
	}
}
