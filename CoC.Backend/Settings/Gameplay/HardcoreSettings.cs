using CoC.Backend.Engine;
using CoC.Backend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings.Gameplay
{
	public sealed partial class HardcoreSettings : GameplaySetting
	{
		public HardcoreSettings() : base(HardcoreSettingsStr, new HardcoreSessionSetting(), new HardcoreGlobalSetting())
		{
		}

		public override void PostLocalSessionInit()
		{
			var glob = (SimpleSetting)globalSetting;
			var sess = (SimpleSetting)localSetting;
			sess.setting = glob.setting;
		}
	}

	public sealed partial class HardcoreSessionSetting : SimpleSetting
	{
		public HardcoreSessionSetting() : base()
		{
		}

		private BackendSessionSave session => BackendSessionSave.data;

		public override bool setting
		{
			get => session.hardcoreMode;
			set => session.hardcoreMode = value;
		}

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

	public sealed partial class HardcoreGlobalSetting : SimpleSetting
	{
		private BackendGlobalSave glob => BackendGlobalSave.data;

		public HardcoreGlobalSetting() : base()
		{
		}

		public override bool setting
		{
			get => glob.hardcoreModeGlobal;
			set => glob.hardcoreModeGlobal = value;
		}

		public override string EnabledHint() => EnabledHintStr();

		public override string DisabledHint() => DisabledHintStr();

		public override bool SettingEnabled(bool possibleSetting, out string whyNot)
		{
			if (possibleSetting == true && !GameEngine.difficulties[glob.difficultyGlobal].HardcoreModeCompatible)
			{
				whyNot = EasyModeNoAllowHardcoreYouPleb();
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}

		public override string WarnPlayersAboutChanging() => WarningStr();
	}
}
