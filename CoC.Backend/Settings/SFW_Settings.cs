using CoC.Backend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings
{
	public sealed partial class SFW_Settings : SimpleGameplaySettingBase
	{
		public static bool SFW_Enabled => BackendSessionSave.data.SFW_Mode;

		private BackendSessionSave session => BackendSessionSave.data;
		private BackendGlobalSave glob => BackendGlobalSave.data;

		public SFW_Settings() : base(SfwSettingsStr)
		{
		}

		public override bool enabled
		{
			get => session.SFW_Mode;
			protected set => session.SFW_Mode = value;
		}
		public override bool? enabledGlobal
		{
			get => glob.SFW_ModeGlobal;
			protected set => glob.SFW_ModeGlobal = value;
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
