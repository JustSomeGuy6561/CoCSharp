using CoC.Backend.Fetishes;
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
			foreach (var val in fetishOptions)
			{
				val.ChangeState(isGlobal);
			}
		}

		private int _selectedIndex;

		public ObservableCollection<OptionsRowBase> fetishOptions { get; }


		public FetishOptionsModelView(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView) : base(modelViewRunner, optionsModelView)
		{
			ReadOnlyCollection<FetishBase> fetishSettings = runner.controller.GetFetishSettings();

			IEnumerable<OptionsRowBase> options = fetishSettings.Select<FetishBase, OptionsRowBase>(item =>
			{
				if (item is SimpleFetish simple)
				{
					return new OptionsRowButtonWrapper(item.name, simple.SetEnabled, (x) => x ? simple.enabledGlobal : simple.enabled, simple.enabledText, simple.disabledText,
						x=>simple.enabledHint(), x=>simple.disabledHint(), !simple.globalCannotBeNull);
				}
				else if (item is AdvancedFetish advanced)
				{
					return new OptionsRowSliderWrapper(item.name, advanced.availableStatuses, advanced.settingText, advanced.settingHint, advanced.GetStatus,
						advanced.SetStatus, !item.globalCannotBeNull);
				}
				else
				{
					return null;
				}
			}).Where(x => x != null);

			fetishOptions = new ObservableCollection<OptionsRowBase>(options);

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
