using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings
{
	public abstract class GameplaySetting : OptionBase
	{
		protected GameplaySetting(SimpleDescriptor settingName, SettingBase globalSettingData) : base(settingName, globalSettingData)
		{
		}

		protected GameplaySetting(SimpleDescriptor settingName, SettingBase localSettingData, SettingBase globalSettingData) : base(settingName, localSettingData, globalSettingData)
		{
		}
	}
}
