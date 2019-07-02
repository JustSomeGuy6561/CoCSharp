//BackendGlobalData.cs
//Description:
//Author: JustSomeGuy
//4/7/2019, 7:43 PM

namespace CoC.Backend.SaveData
{
	internal sealed class BackendGlobalData : SaveData
	{
		internal static BackendGlobalData data => SaveSystem.getGlobalSave<BackendGlobalData>();

		public bool UsesMetricMeasurements = false;

	}
}
