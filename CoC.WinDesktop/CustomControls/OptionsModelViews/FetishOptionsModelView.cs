using CoC.Backend.Settings.Fetishes;
using CoC.Backend.Settings;
using CoC.WinDesktop.Helpers;
using CoC.WinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Backend;
using CoC.Backend.Engine;
using CoC.WinDesktop.ContentWrappers.OptionsRow;

namespace CoC.WinDesktop.CustomControls.OptionsModelViews
{
	public sealed partial class FetishOptionsModelView : OptionModelViewDataBase
	{
		private int lastLanguageIndex;

		public string FetishOptionsText
		{
			get => _fetishOptionsText;
			private set => CheckPropertyChanged(ref _fetishOptionsText, value);
		}
		private string _fetishOptionsText;
		protected override SimpleDescriptor TitleText => FetishOptionsTitleText;

		public string FetishOptionsHelper
		{
			get => _fetishOptionsHelper;
			private set => CheckPropertyChanged(ref _fetishOptionsHelper, value);
		}
		private string _fetishOptionsHelper;
		protected override SimpleDescriptor TitleHelperText => FetishOptionsHelperText;

		public override SimpleDescriptor ButtonText => FetishButtonText;

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
			fetishOptions = isGlobal ? globalOptions : sessionOptions;
		}

		private int _selectedIndex;

		public ReadOnlyCollection<OptionsRowWrapperBase> fetishOptions
		{
			get => _fetishOptions;
			private set => CheckPropertyChanged(ref _fetishOptions, value);
		}
		private ReadOnlyCollection<OptionsRowWrapperBase> _fetishOptions;

		private readonly ReadOnlyCollection<OptionsRowWrapperBase> sessionOptions;
		private readonly ReadOnlyCollection<OptionsRowWrapperBase> globalOptions;


		public FetishOptionsModelView(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView) : base(modelViewRunner, optionsModelView)
		{
			ReadOnlyCollection<FetishSetting> fetishSettings = runner.controller.GetFetishSettings();

			List<OptionsRowWrapperBase> options = fetishSettings.Select(item => OptionsRowWrapperBase.BuildOptionRow(item.name, item.localSetting)).Where(x => x != null).ToList();
			sessionOptions = new ReadOnlyCollection<OptionsRowWrapperBase>(options);

			options = fetishSettings.Select(item => OptionsRowWrapperBase.BuildOptionRow(item.name, item.globalSetting)).Where(x => x != null).ToList();
			globalOptions = new ReadOnlyCollection<OptionsRowWrapperBase>(options);

			lastLanguageIndex = LanguageEngine.currentLanguageIndex;

			_fetishOptionsText = TitleText() ;
			_fetishOptionsHelper = TitleHelperText();

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

				FetishOptionsText = TitleText();
				FetishOptionsHelper = TitleHelperText();
			}
		}
	}
}
