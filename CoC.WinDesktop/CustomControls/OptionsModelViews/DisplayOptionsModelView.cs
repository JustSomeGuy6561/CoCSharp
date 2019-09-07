using CoC.Backend;
using CoC.Backend.Engine;
using CoC.Backend.Settings;
using CoC.Backend.Tools;
using CoCWinDesktop.ContentWrappers;
using CoCWinDesktop.ContentWrappers.OptionsRow;
using CoCWinDesktop.CustomControls.SideBarModelViews;
using CoCWinDesktop.DisplaySettings;
using CoCWinDesktop.Engine;
using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;
using CoCWinDesktop.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CoCWinDesktop.CustomControls.OptionsModelViews
{
	public sealed partial class DisplayOptionsModelView : OptionModelViewDataBase
	{
		private int lastLanguageIndex;

		public StandardSideBarModelView sidebarView { get; }

		public OptionsRowSliderWrapper FontSizeSlider { get; }
		public OptionsRowSliderWrapper BackgroundSlider { get; }
		public OptionsRowSliderWrapper TextBackgroundSlider { get; }

		public string ContentString
		{
			get => _contentString;
			private set => CheckPropertyChanged(ref _contentString, value);
		}
		private string _contentString;

		private string rtfSource;
		private List<Color> colors;


		public string DisplayOptionsText
		{
			get => _DisplayOptionsText;
			private set => CheckPropertyChanged(ref _DisplayOptionsText, value);
		}
		private string _DisplayOptionsText;
		protected override SimpleDescriptor TitleText => DisplayOptionsTitleText;
		public string DisplayOptionsHelper
		{
			get => _DisplayOptionsHelper;
			private set => CheckPropertyChanged(ref _DisplayOptionsHelper, value);
		}
		private string _DisplayOptionsHelper;
		protected override SimpleDescriptor TitleHelperText => DisplayOptionsHelperText;

		public override SimpleDescriptor ButtonText => DisplayButtonText;

		public DisplayOptionsModelView(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView) : base(modelViewRunner, optionsModelView)
		{
			sidebarView = new StandardSideBarModelView(new SaveDataCollection(null), true);

			DisplayOptions option = DisplayOptionManager.GetOptionOfType<BackgroundOption>();
			option.AddGlobalSetListener(SetBackground);

			AdvancedSetting data = (AdvancedSetting)option.globalSetting;
			BackgroundSlider = new OptionsRowSliderWrapper(option.name, data.availableOptions, data.SelectedSettingText, data.SelectedSettingHint, data.Get,
				data.Set, data.WarnPlayersAboutChanging, data.SettingEnabled);

			option = DisplayOptionManager.GetOptionOfType<FontSizeOption>();
			option.AddGlobalSetListener(SetFontSize);

			data = (AdvancedSetting)option.globalSetting;
			FontSizeSlider = new OptionsRowSliderWrapper(option.name, data.availableOptions, data.SelectedSettingText, data.SelectedSettingHint, data.Get,
				data.Set, data.WarnPlayersAboutChanging, data.SettingEnabled);

			option = DisplayOptionManager.GetOptionOfType<TextBackgroundOption>();
			option.AddGlobalSetListener(SetTextBackground);

			data = (AdvancedSetting)option.globalSetting;
			TextBackgroundSlider = new OptionsRowSliderWrapper(option.name, data.availableOptions, data.SelectedSettingText, data.SelectedSettingHint, data.Get,
				data.Set, data.WarnPlayersAboutChanging, data.SettingEnabled);

			lastLanguageIndex = LanguageEngine.currentLanguageIndex;
			_DisplayOptionsText = TitleText();
			_DisplayOptionsHelper = TitleHelperText();
			
			//GetRTFText();
			rtfSource = null;

		}

		private void SetBackground()
		{
			CheckFontColor();
		}

		private void SetFontSize()
		{
			UpdateDisplay();
		}

		private void SetTextBackground()
		{
			CheckFontColor();
		}

		private void CheckFontColor()
		{
			//runner.CheckPrimaryFontColor();
			if (colors.Count > 0 && colors[0] != runner.FontColor.Color)
			{
				colors[0] = runner.FontColor.Color;
				UpdateDisplay();
			}
		}

		//Top: header, helper
		//ContentRTB in the middle. Full of lorem ipsum or language check text. 

		//bottom: 3 sliders - for font size, text bg color and bg image. 

		//confirm on bottom icons. 
		public override void ParseDataForDisplay()
		{
			if (lastLanguageIndex != LanguageEngine.currentLanguageIndex || string.IsNullOrEmpty(rtfSource))
			{
				lastLanguageIndex = LanguageEngine.currentLanguageIndex;
				OnLanguageChange();
			}
		}

		private void OnLanguageChange()
		{
			rtfSource = RTFParser.FromHTMLNoHeader(new StringBuilder(LanguageEngine.currentLanguage.GenericFlavorTextExample()), runner.FontColor.Color, out colors);

			DisplayOptionsText = TitleText();
			DisplayOptionsHelper = TitleHelperText();

			UpdateDisplay();
		}

		private void UpdateDisplay()
		{
			ContentString = RTFParser.FromRTFText(rtfSource, colors, runner);
		}

		public override Action OnConfirmation => ConfirmControls;


		private void ConfirmControls()
		{
			//do nothing. 
		}
	}
}
