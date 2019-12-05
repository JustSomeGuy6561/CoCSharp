using CoC.Backend.Engine;
using CoC.Backend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings.Gameplay
{
	public sealed partial class SillyModeSettings : GameplaySetting
	{
		private static SillyModeSettings currentSetting => (SillyModeSettings)GameplaySettingsManager.GetSetting(x => x.GetType() == typeof(SillyModeSettings));

		//provides a place we can store the silly mode setting lookup without going insane.
		public static bool promptUserForSillyness => currentSetting != null && ((SimpleNullableSetting)currentSetting.localSetting).setting == null;
		public static bool isEnabled => ((SimpleNullableSetting)currentSetting?.localSetting)?.setting == true;


		public SillyModeSettings() : base(SillySettingStr, new SillyModeLocal(), new SillyModeGlobal())
		{
		}

		public override void PostLocalSessionInit()
		{
			var glob = (SimpleSetting)globalSetting;
			var sess = (SimpleSetting)localSetting;
			sess.setting = glob.setting;
		}

		private partial class SillyModeLocal : SimpleNullableSetting
		{
			public override bool? setting { get => BackendSessionSave.data.SillyModeLocal; set => BackendSessionSave.data.SillyModeLocal = value; }

			public override string DisabledHint() => DisabledHintStr();

			public override string EnabledHint() => EnabledHintStr();

			public override bool SettingEnabled(bool? possibleSetting, out string whyNot)
			{
				whyNot = "";
				return true;
			}

			public override SimpleDescriptor unsetText => UnsetTextStr;

			public override string UnsetHint() => UnsetHintStr();

			public override string WarnPlayersAboutChanging()
			{
				return "";
			}
		}

		private partial class SillyModeGlobal : SimpleNullableSetting
		{
			public override bool? setting { get => BackendGlobalSave.data.SillyModeGlobal; set => BackendGlobalSave.data.SillyModeGlobal = value; }

			public override string DisabledHint() => DisabledHintStr();

			public override string EnabledHint() => EnabledHintStr();

			public override SimpleDescriptor unsetText => UnsetTextStr;

			public override string UnsetHint() => UnsetHintStr();

			public override bool SettingEnabled(bool? possibleSetting, out string whyNot)
			{
				whyNot = "";
				return true;
			}

			public override string WarnPlayersAboutChanging()
			{
				return "";
			}
		}
	}
}
