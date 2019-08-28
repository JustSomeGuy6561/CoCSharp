using CoC.Backend.Settings.Fetishes;
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
	public sealed class FetishOptionsModelView : OptionModelViewDataBase
	{
		public string FetishOptionsText
		{
			get => _fetishOptionsText;
			private set => CheckPropertyChanged(ref _fetishOptionsText, value);
		}
		private string _fetishOptionsText;

		public string FetishOptionsHelper
		{
			get => _fetishOptionsHelper;
			private set => CheckPropertyChanged(ref _fetishOptionsHelper, value);
		}
		private string _fetishOptionsHelper;

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

		public ReadOnlyCollection<OptionsRowBase> fetishOptions
		{
			get => _fetishOptions;
			private set => CheckPropertyChanged(ref _fetishOptions, value);
		}
		private ReadOnlyCollection<OptionsRowBase> _fetishOptions;

		private readonly ReadOnlyCollection<OptionsRowBase> sessionOptions;
		private readonly ReadOnlyCollection<OptionsRowBase> globalOptions;


		public FetishOptionsModelView(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView) : base(modelViewRunner, optionsModelView)
		{
			ReadOnlyCollection<FetishSetting> fetishSettings = runner.controller.GetFetishSettings();

			List<OptionsRowBase> options = fetishSettings.Select(item => OptionsRowBase.BuildOptionRow(item.name, item.localSetting)).Where(x => x != null).ToList();
			sessionOptions = new ReadOnlyCollection<OptionsRowBase>(options);

			options = fetishSettings.Select(item => OptionsRowBase.BuildOptionRow(item.name, item.globalSetting)).Where(x => x != null).ToList();
			globalOptions = new ReadOnlyCollection<OptionsRowBase>(options);

			FetishOptionsText = "Fetish Options";
			FetishOptionsHelper = "You can change whether or not strange, exotic, and/or extreme fetishes appear in your game via by setting them here.";

			possibilities = new ObservableCollection<string>()
			{
				"Session",
				"Global"
			};

			setState(_selectedIndex != 0);//make sure the data is correctly initialized.
		}

		public override void ParseDataForDisplay()
		{
		}
	}
}
