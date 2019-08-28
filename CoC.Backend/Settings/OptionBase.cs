using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings
{
	public abstract class OptionBase
	{
		public readonly SimpleDescriptor name;
		public readonly SettingBase localSetting;
		public readonly SettingBase globalSetting;


		protected OptionBase(SimpleDescriptor settingName, SettingBase localSettingData, SettingBase globalSettingData)
		{
			name = settingName ?? throw new ArgumentNullException(nameof(settingName));
			localSetting = localSettingData ?? throw new ArgumentNullException(nameof(localSettingData));
			globalSetting = globalSettingData ?? throw new ArgumentNullException(nameof(globalSettingData));
		}


		protected OptionBase(SimpleDescriptor settingName, SettingBase globalSettingData)
		{
			name = settingName ?? throw new ArgumentNullException(nameof(settingName));
			localSetting = null;
			globalSetting = globalSettingData ?? throw new ArgumentNullException(nameof(globalSettingData));
		}

		public abstract void PostLocalSessionInit();
	}
}
