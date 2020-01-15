using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings.Gameplay
{
	public sealed partial class TimeDisplaySettings : GameplaySetting
	{
		public TimeDisplaySettings() : base(TimeDisplaySettingsText, new TimeGlobalSetting())
		{
		}

		public override void PostLocalSessionInit()
		{
			//do nothing, as time is global only.
		}
	}
	public sealed partial class TimeGlobalSetting : SimpleSetting
	{
		private BackendGlobalSave glob => BackendGlobalSave.data;

		public TimeGlobalSetting() : base()
		{

		}

		public override string DisabledHint() => MilitaryHint();

		public override string EnabledHint() => AM_PM_Hint();

		public override SimpleDescriptor enabledText => TwelveHourStr;
		public override SimpleDescriptor disabledText => TwentyFourHourStr;

		public override bool setting
		{
			get => !glob.UsesMilitaryTime;
			set => glob.UsesMilitaryTime = !value;
		}
		public override bool SettingEnabled(bool possibleSetting, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public override string WarnPlayersAboutChanging()
		{
			return null;
		}
	}
}
