using CoC.Backend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings
{
	public sealed partial class RealismSettings : SimpleGameplaySettingBase
	{
		private BackendSessionSave session => BackendSessionSave.data;
		private BackendGlobalSave glob => BackendGlobalSave.data;

		public RealismSettings() : base(RealismSettingsStr)
		{ }

		public override bool enabled
		{
			get => session.RealismEnabled;
			protected set => session.RealismEnabled = value;
		}
		public override bool? enabledGlobal
		{
			get => glob.RealismEnabledGlobal;
			protected set => glob.RealismEnabledGlobal = value;
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
