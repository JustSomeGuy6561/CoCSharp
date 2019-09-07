using CoC.Backend;
using CoC.Backend.Settings;
using CoCWinDesktop.Helpers;
using System;
using System.Diagnostics;

namespace CoCWinDesktop.ContentWrappers.OptionsRow
{

	public abstract class OptionsRowWrapperBase : NotifierBase
	{
		//public string activeItemDescription
		//{
		//	get => _selectedItemDescription;
		//	protected set => CheckPropertyChanged(ref _selectedItemDescription, value);
		//}
		//private string _selectedItemDescription;

		public string OptionName => nameDesc();
		private readonly SimpleDescriptor nameDesc;


		protected OptionsRowWrapperBase(SimpleDescriptor optionName)
		{
			nameDesc = optionName ?? throw new ArgumentNullException(nameof(optionName));
			//_displayMode = defaultMode;
		}

		public static OptionsRowWrapperBase BuildOptionRow(SimpleDescriptor name, SettingBase setting)
		{
			if (setting is null) return null;

			if (setting is SimpleSetting simple)
			{
				return new OptionsRowButtonWrapper(name, simple.Set, simple.Get, simple.enabledText, simple.disabledText, simple.EnabledHint,
					simple.DisabledHint, simple.WarnPlayersAboutChanging, simple.SettingEnabled);
			}
			else if (setting is SimpleNullableSetting sn)
			{
				return new OptionsRowButtonWrapper(name, sn.Set, sn.Get, sn.enabledText, sn.disabledText, sn.unsetText, sn.EnabledHint,
					sn.DisabledHint, sn.UnsetHint, sn.WarnPlayersAboutChanging, sn.SettingEnabled);
			}
			else if (setting is AdvancedSetting advanced)
			{
				return new OptionsRowSliderWrapper(name, advanced.availableOptions, advanced.SelectedSettingText, advanced.SelectedSettingHint, advanced.Get, advanced.Set,
					advanced.WarnPlayersAboutChanging, advanced.SettingEnabled);
			}
			else if (setting is AdvancedNullableSetting an)
			{
				return new OptionsRowSliderWrapper(name, an.availableOptions, an.SelectedSettingText, an.SelectedSettingHint, an.Get, an.Set,
					an.WarnPlayersAboutChanging, an.SettingEnabled);
			}
			else
			{
				throw new NotSupportedException("This derived type is not (currently) supported. You should never see this.");
			}
		}
	}
}
