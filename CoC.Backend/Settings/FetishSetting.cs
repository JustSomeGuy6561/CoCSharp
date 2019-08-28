using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings
{
	public abstract class FetishSetting : OptionBase
	{
		protected FetishSetting(SimpleDescriptor settingName, SettingBase globalSettingData) : base(settingName, globalSettingData)
		{
		}

		protected FetishSetting(SimpleDescriptor settingName, SettingBase localSettingData, SettingBase globalSettingData) : base(settingName, localSettingData, globalSettingData)
		{
		}
	}
}
