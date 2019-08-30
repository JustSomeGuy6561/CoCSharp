using CoC.Backend;
using CoC.Backend.Engine;
using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CoCWinDesktop.CustomControls.OptionsModelViews
{
	public sealed partial class InterfaceOptionsModelView : OptionModelViewDataBase
	{
		private int lastLanguageIndex;

		public string InterfaceOptionsText
		{
			get => _InterfaceOptionsText;
			private set => CheckPropertyChanged(ref _InterfaceOptionsText, value);
		}
		private string _InterfaceOptionsText;
		protected override SimpleDescriptor TitleText => InterfaceTitleText;

		public string InterfaceOptionsHelper
		{
			get => _InterfaceOptionsHelper;
			private set => CheckPropertyChanged(ref _InterfaceOptionsHelper, value);
		}
		private string _InterfaceOptionsHelper;
		protected override SimpleDescriptor TitleHelperText => InterfaceHelperText;

		public override SimpleDescriptor ButtonText => InterfaceButtonText;

		//display options was so conviluted it's getting its own view, but the only way to see it is via a button in interface options. 

		public string DisplayOptionsTitleText
		{
			get => _displayOptionsTitleText;
			private set => CheckPropertyChanged(ref _displayOptionsTitleText, value);
		}
		private string _displayOptionsTitleText;
		private SimpleDescriptor displayOptionTitleSource => InterfaceStrings.DisplayOptionsTitleText;

		public string DisplayOptionsHelperText
		{
			get => _displayOptionsHelperText;
			private set => CheckPropertyChanged(ref _displayOptionsHelperText, value);
		}
		private string _displayOptionsHelperText;
		private SimpleDescriptor displayOptionHelperSource => InterfaceStrings.DisplayOptionsHelperText;

		public string DisplayOptionsBtnText
		{
			get => _displayOptionsBtnText;
			private set => CheckPropertyChanged(ref _displayOptionsBtnText, value);
		}
		private string _displayOptionsBtnText;
		private SimpleDescriptor displayOptionTextSource => InterfaceStrings.AdjustDisplayOptionsText;


		public ICommand OnDisplayButton { get; }

		public ReadOnlyCollection<OptionsRowBase> interfaceOptions { get; }


		public InterfaceOptionsModelView(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView) : base(modelViewRunner, optionsModelView)
		{
			lastLanguageIndex = LanguageEngine.currentLanguageIndex;
			_displayOptionsBtnText = displayOptionTextSource();
			_displayOptionsTitleText = displayOptionTitleSource();
			_displayOptionsHelperText = displayOptionHelperSource();

			_InterfaceOptionsText = TitleText();
			_InterfaceOptionsHelper = TitleHelperText();


			OnDisplayButton = parent.OnDisplayHandle;

			List<OptionsRowBase> data = new List<OptionsRowBase>()
			{
				OptionsRowBase.BuildOptionRow(runner.sidebarFontOption.name, runner.sidebarFontOption.globalSetting),
				OptionsRowBase.BuildOptionRow(runner.spriteStatusOption.name, runner.spriteStatusOption.globalSetting),
				OptionsRowBase.BuildOptionRow(runner.imagePackOption.name, runner.imagePackOption.globalSetting),
				OptionsRowBase.BuildOptionRow(runner.sidebarAnimationOption.name, runner.sidebarAnimationOption.globalSetting),
				OptionsRowBase.BuildOptionRow(runner.enemySidebarOption.name, runner.enemySidebarOption.globalSetting),
			};

			interfaceOptions = new ReadOnlyCollection<OptionsRowBase>(data);

		}

		public override void ParseDataForDisplay()
		{
			if (lastLanguageIndex != LanguageEngine.currentLanguageIndex)
			{
				lastLanguageIndex = LanguageEngine.currentLanguageIndex;

				DisplayOptionsBtnText = displayOptionTextSource();
				DisplayOptionsTitleText = displayOptionTitleSource();
				DisplayOptionsHelperText = displayOptionHelperSource();

				InterfaceOptionsText = TitleText();
				InterfaceOptionsHelper = TitleHelperText();
			}
		}
	}
}
