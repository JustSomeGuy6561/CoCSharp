using CoC.Backend.Engine;
using CoC.Backend.SaveData;
using CoC.Backend.Settings;
using CoC.Frontend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Settings.Gameplay
{

	public sealed partial class HyperHappySettings : GameplaySetting
	{
		private static HyperHappySettings currentSetting => (HyperHappySettings)GameplaySettingsManager.GetSetting(x => x.GetType() == typeof(HyperHappySettings));

		//provides a place we can store the silly mode setting lookup without going insane.
		public static bool isEnabled => ((SimpleSetting)currentSetting?.localSetting)?.setting == true;

		public HyperHappySettings() : base(HyperHappySettingsStr, new HyperHappySessionSetting(), new HyperHappyGlobalSetting())
		{
		}
		public override void PostLocalSessionInit()
		{
			var glob = (SimpleSetting)globalSetting;
			var sess = (SimpleSetting)localSetting;
			sess.setting = glob.setting;
		}
	}

	public sealed partial class HyperHappySessionSetting : SimpleSetting
	{
		public HyperHappySessionSetting() : base()
		{

		}

		private FrontendSessionSave session => FrontendSessionSave.data;

		public override bool setting
		{
			get => session.HyperHappyLocal;
			set => session.HyperHappyLocal = value;
		}
		//public override bool setting
		//{
		//	get => glob.HyperHappyEnabledGlobal;
		//	set => glob.HyperHappyEnabledGlobal = value;
		//}

		public override string EnabledHint() => EnabledHintStr();

		public override string DisabledHint() => DisabledHintStr();

		public override bool SettingEnabled(bool possibleSetting, out string whyNot)
		{
			whyNot = "";
			return true;
		}

		public override string WarnPlayersAboutChanging() => "";
	}

	public sealed partial class HyperHappyGlobalSetting : SimpleSetting
	{
		public HyperHappyGlobalSetting() : base()
		{

		}

		private FrontendGlobalSave glob => FrontendGlobalSave.data;

		public override bool setting
		{
			get => glob.HyperHappyEnabledGlobal;
			set => glob.HyperHappyEnabledGlobal = value;
		}

		public override string EnabledHint() => EnabledHintStr();

		public override string DisabledHint() => DisabledHintStr();

		public override bool SettingEnabled(bool possibleSetting, out string whyNot)
		{
			whyNot = "";
			return true;
		}

		public override string WarnPlayersAboutChanging() => "";


	}
}
