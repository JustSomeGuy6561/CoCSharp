using CoC.Backend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings.Gameplay
{
	public sealed partial class HungerSettings : GameplaySetting
	{
		public HungerSettings() : base(HungerSettingStr, new HungerSessionSetting(), new HungerGlobalSetting())
		{
		}

		public override void PostLocalSessionInit()
		{
			var glob = (SimpleSetting)globalSetting;
			var sess = (SimpleSetting)localSetting;
			sess.setting = glob.setting;
		}
	}
	public sealed partial class HungerSessionSetting : SimpleSetting
	{
		private BackendSessionSave session => BackendSessionSave.data;

		public override bool setting
		{
			get => session.HungerEnabled;
			set => session.HungerEnabled = value;
		}


		public override string DisabledHint() => DisabledHintStr();

		public override string EnabledHint() => EnabledHintStr();

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

		//public HungerSettings() : base(HungerSettingStr)
		//{ }

		
		//public override bool? enabledGlobal
		//{
		//	get => glob.HungerEnabledGlobal;
		//	protected set => glob.HungerEnabledGlobal = value;
		//}

		//public override string enabledHint(bool isGlobal)
		//{
		//	(isGlobal);
		//}

		//public override string disabledHint(bool isGlobal)
		//{
		//	return DisabledHintStr(isGlobal);
		//}
	}

	public sealed partial class HungerGlobalSetting : SimpleSetting
	{
		private BackendGlobalSave glob => BackendGlobalSave.data;

		public HungerGlobalSetting() : base()
		{
		}

		public override bool setting
		{
			get => glob.HungerEnabledGlobal;
			set => glob.HungerEnabledGlobal = value;
		}

		public override string DisabledHint() => DisabledHintStr();

		public override string EnabledHint() => EnabledHintStr();

		public override string WarnPlayersAboutChanging() => WarningStr();

		public override bool SettingEnabled(bool possibleSetting, out string whyNot)
		{
			whyNot = "";
			return true;
		}
	}
}
