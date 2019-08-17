//BackendGlobalData.cs
//Description:
//Author: JustSomeGuy
//4/7/2019, 7:43 PM

namespace CoC.Backend.SaveData
{
	internal sealed class BackendGlobalSave : SaveData
	{
		internal static BackendGlobalSave data => SaveSystem.getGlobalSave<BackendGlobalSave>();

		public bool UsesMetricMeasurements = false;

	}
}
