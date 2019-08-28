using CoC.Backend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings.Gameplay
{

	public sealed partial class RealismSettings : GameplaySetting
	{
		public RealismSettings() : base(RealismSettingsStr, new RealismSessionSetting(), new RealismGlobalSetting())
		{
		}
		public override void PostLocalSessionInit()
		{
			var glob = (SimpleSetting)globalSetting;
			var sess = (SimpleSetting)localSetting;
			sess.setting = glob.setting;
		}
	}

	public sealed partial class RealismSessionSetting : SimpleSetting
	{
		public RealismSessionSetting() : base()
		{

		}

		private BackendSessionSave session => BackendSessionSave.data;

		public override bool setting
		{
			get => session.RealismEnabled;
			set => session.RealismEnabled = value;
		}
		//public override bool setting
		//{
		//	get => glob.RealismEnabledGlobal;
		//	set => glob.RealismEnabledGlobal = value;
		//}

		public override string EnabledHint() => EnabledHintStr();

		public override string DisabledHint() => DisabledHintStr();

		public override bool SettingEnabled(bool possibleSetting, out string whyNot)
		{
			if (SaveSystem.isSessionActive && setting && possibleSetting == false)
			{
				whyNot = ActiveSaveAndItsEnabledDummy();
				return false;
			}
			else
			{
				whyNot = "";
				return true;
			}
		}

		public override string WarnPlayersAboutChanging() => WarningStr();
	}

	public sealed partial class RealismGlobalSetting : SimpleSetting
	{
		public RealismGlobalSetting() : base()
		{

		}

		private BackendGlobalSave glob => BackendGlobalSave.data;

		public override bool setting
		{
			get => glob.RealismEnabledGlobal;
			set => glob.RealismEnabledGlobal = value;
		}

		public override string EnabledHint() => EnabledHintStr();

		public override string DisabledHint() => DisabledHintStr();

		public override bool SettingEnabled(bool possibleSetting, out string whyNot)
		{
				whyNot = "";
				return true;
		}

		public override string WarnPlayersAboutChanging() => WarningStr();

		
	}
}
