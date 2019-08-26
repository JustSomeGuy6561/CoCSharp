using CoC.Backend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings
{
	public sealed partial class HungerSettings : LimitedGameplaySettingBase
	{
		private BackendSessionSave session => BackendSessionSave.data;
		private BackendGlobalSave glob => BackendGlobalSave.data;

		public HungerSettings() : base(HungerSettingStr)
		{ }

		public override bool enabled
		{
			get => session.HungerEnabled;
			protected set => session.HungerEnabled = value;
		} 
		public override bool? enabledGlobal
		{
			get => glob.HungerEnabledGlobal;
			protected set => glob.HungerEnabledGlobal = value;
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
