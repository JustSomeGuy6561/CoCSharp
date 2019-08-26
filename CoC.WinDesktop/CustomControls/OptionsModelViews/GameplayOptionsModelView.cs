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
	public sealed class GameplayOptionsModelView : OptionModelViewDataBase
	{

		public string GameplayOptionsText
		{
			get => _gameplayOptionsText;
			private set => CheckPropertyChanged(ref _gameplayOptionsText, value);
		}
		private string _gameplayOptionsText;

		public string GameplayOptionsHelper
		{
			get => _gameplayOptionsHelper;
			private set => CheckPropertyChanged(ref _gameplayOptionsHelper, value);
		}
		private string _gameplayOptionsHelper;

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
			foreach (var val in gameplayOptions)
			{
				val.ChangeState(isGlobal);
			}
		}

		private int _selectedIndex;



		public ObservableCollection<OptionsRowBase> gameplayOptions { get; }

		public GameplayOptionsModelView(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView) : base(modelViewRunner, optionsModelView)
		{
			ReadOnlyCollection<GameplaySettingBase> gameplaySettings = runner.controller.GetGameplaySettings();

			IEnumerable<OptionsRowBase> options = gameplaySettings.Select<GameplaySettingBase, OptionsRowBase>(item =>
			{
				if (item is SimpleGameplaySettingBase simple)
				{
					return new OptionsRowButtonWrapper(item.name, simple.SetEnabled, (x) => x ? simple.enabledGlobal : simple.enabled, simple.enabledText, simple.disabledText,
						simple.enabledHint, simple.disabledHint, !simple.globalCannotBeNull);
				}
				else if (item is AdvancedGameplaySettingBase advanced)
				{
					return new OptionsRowSliderWrapper(item.name, advanced.availableStatuses, advanced.settingText, advanced.settingHint, advanced.GetStatus,
						advanced.SetStatus, !item.globalCannotBeNull);
				}
				else if (item is LimitedGameplaySettingBase limited)
				{
					return new OptionsRowButtonWrapper(item.name, limited.SetEnabled, (x) => x ? limited.enabledGlobal : limited.enabled, limited.enabledText,
						limited.disabledText, limited.enabledHint, limited.disabledHint, !item.globalCannotBeNull);
				}
				else
				{
					return null;
				}
			}).Where(x => x != null);

			gameplayOptions = new ObservableCollection<OptionsRowBase>(options);

			GameplayOptionsText = "Gameplay";
			GameplayOptionsHelper = "You can change the gameplay options here. Fetishes are given their own category.";

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
