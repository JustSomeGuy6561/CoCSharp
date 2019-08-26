using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Settings
{
	public sealed partial class MeasurementSettings : SimpleGameplaySettingBase
	{
		private BackendSessionSave session => BackendSessionSave.data;
		private BackendGlobalSave glob => BackendGlobalSave.data;

		public MeasurementSettings() : base(MeasurementSettingsText)
		{
		}

		public override string disabledHint(bool isGlobal)
		{
			return MetricHint();
		}

		public override string enabledHint(bool isGlobal)
		{
			return ImperialHint();
		}

		public override SimpleDescriptor enabledText => ImperialStr;
		public override SimpleDescriptor disabledText => MetricStr;

		public override bool enabled
		{
			get => !BackendSessionSave.data.UsesMetricMeasurements;
			protected set => BackendSessionSave.data.UsesMetricMeasurements = !value;
		}

		public override bool? enabledGlobal
		{
			get => !BackendGlobalSave.data.UsesMetricMeasurements;
			protected set => BackendGlobalSave.data.UsesMetricMeasurements = !value ?? false;
		}

		public override bool globalCannotBeNull => true;
	}
}
