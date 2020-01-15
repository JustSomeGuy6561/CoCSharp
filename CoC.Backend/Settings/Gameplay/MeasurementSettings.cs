using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings.Gameplay
{
	public sealed partial class MeasurementSettings : GameplaySetting
	{
		public MeasurementSettings() : base(MeasurementSettingsText, new MeasurementGlobalSetting())
		{
		}

		public override void PostLocalSessionInit()
		{
			//global only. do nothing.
		}
	}
	public sealed partial class MeasurementGlobalSetting : SimpleSetting
	{
		public MeasurementGlobalSetting() : base()
		{
		}

		public override string DisabledHint() => MetricHint();

		public override string EnabledHint() => ImperialHint();

		public override SimpleDescriptor enabledText => ImperialStr;
		public override SimpleDescriptor disabledText => MetricStr;

		public override bool setting
		{
			get => !BackendGlobalSave.data.UsesMetricMeasurements;
			set => BackendGlobalSave.data.UsesMetricMeasurements = !value;
		}

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
