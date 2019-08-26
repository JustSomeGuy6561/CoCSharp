using CoC.Backend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings
{
	public sealed partial class HardcoreSettings : LimitedGameplaySettingBase
	{
		private BackendSessionSave session => BackendSessionSave.data;
		private BackendGlobalSave glob => BackendGlobalSave.data;

		public HardcoreSettings() : base(HardcoreSettingsStr)
		{
		}

		public override bool enabled
		{
			get => session.hardcoreMode;
			protected set => session.hardcoreMode = value;
		}
		public override bool? enabledGlobal
		{
			get => glob.hardcoreModeGlobal;
			protected set => glob.hardcoreModeGlobal = value;
		}

		public override string enabledHint(bool isGlobal)
		{
			return EnabledHintStr(isGlobal);
		}

		public override string disabledHint(bool isGlobal)
		{
			return DisabledHintStr(isGlobal);
		}
	}
}
