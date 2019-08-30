using CoC.Backend;
using CoC.Backend.Engine;
using CoC.Backend.Settings;
using CoC.Backend.Tools;
using CoCWinDesktop.CustomControls.SideBarModelViews;
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

			AdvancedSetting data = (AdvancedSetting)runner.backgroundOption.globalSetting;
			BackgroundSlider = new OptionsRowSliderWrapper(runner.backgroundOption.name, data.availableOptions, data.SelectedSettingText, data.SelectedSettingHint, data.Get,
				SetBackground, data.WarnPlayersAboutChanging, data.SettingEnabled);

			data = (AdvancedSetting)runner.fontSizeOption.globalSetting;
			FontSizeSlider = new OptionsRowSliderWrapper(runner.fontSizeOption.name, data.availableOptions, data.SelectedSettingText, data.SelectedSettingHint, data.Get,
				SetFontSize, data.WarnPlayersAboutChanging, data.SettingEnabled);

			data = (AdvancedSetting)runner.textBackgroundOption.globalSetting;
			TextBackgroundSlider = new OptionsRowSliderWrapper(runner.textBackgroundOption.name, data.availableOptions, data.SelectedSettingText, data.SelectedSettingHint, data.Get,
				SetTextBackground, data.WarnPlayersAboutChanging, data.SettingEnabled);



			lastLanguageIndex = LanguageEngine.currentLanguageIndex;
			_DisplayOptionsText = TitleText();
			_DisplayOptionsHelper = TitleHelperText();
			
			//GetRTFText();
			rtfSource = null;

		}

		private void SetBackground(int index)
		{
			((AdvancedSetting)runner.backgroundOption.globalSetting).Set(index);
			CheckFontColor();
		}

		private void SetFontSize(int index)
		{
			((AdvancedSetting)runner.fontSizeOption.globalSetting).Set(index);
			UpdateDisplay();
		}

		private void SetTextBackground(int index)
		{
			((AdvancedSetting)runner.textBackgroundOption.globalSetting).Set(index);
			CheckFontColor();
		}

		private void CheckFontColor()
		{
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
