using CoC.Backend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings.Gameplay
{
	public sealed partial class SFW_Settings : GameplaySetting
	{
		public SFW_Settings() : base(SfwSettingsStr, new SFW_SessionSetting(), new SFW_GlobalSetting())
		{
		}

		public override void PostLocalSessionInit()
		{
			var glob = (SimpleSetting)globalSetting;
			var sess = (SimpleSetting)localSetting;
			sess.setting = glob.setting;
		}
	}
	public sealed partial class SFW_SessionSetting : SimpleSetting
	{ 
		public static bool SFW_Enabled => BackendSessionSave.data.SFW_Mode;

		private BackendSessionSave session => BackendSessionSave.data;

		public SFW_SessionSetting() : base()
		{
		}

		public override bool setting
		{
			get => session.SFW_Mode;
			set => session.SFW_Mode = value;
		}
		public override string EnabledHint() => EnabledHintStr();

		public override string DisabledHint() => DisabledHintStr();

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

	public sealed partial class SFW_GlobalSetting : SimpleSetting
	{
		public static bool SFW_Enabled => BackendSessionSave.data.SFW_Mode;

		private BackendSessionSave session => BackendSessionSave.data;
		private BackendGlobalSave glob => BackendGlobalSave.data;

		public SFW_GlobalSetting() : base()
		{
		}

		public override bool setting
		{
			get => glob.SFW_ModeGlobal;
			set => glob.SFW_ModeGlobal = value;
		}
		public override string EnabledHint() => EnabledHintStr();

		public override string DisabledHint() => DisabledHintStr();

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
