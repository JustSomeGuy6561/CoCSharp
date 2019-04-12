using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.SaveData
{
	internal sealed class BackendGlobalData : SaveData
	{
		internal static BackendGlobalData data => SaveSystem.getGlobalSave<BackendGlobalData>();

		public bool UsesMetricMeasurements = false;

	}
}
