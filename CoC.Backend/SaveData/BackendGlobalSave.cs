//BackendGlobalData.cs
//Description:
//Author: JustSomeGuy
//4/7/2019, 7:43 PM

using CoC.Backend.Engine;

namespace CoC.Backend.SaveData
{
	public sealed class BackendGlobalSave : SaveData
	{
		internal static BackendGlobalSave data => SaveSystem.getGlobalSave<BackendGlobalSave>();

		public bool UsesMetricMeasurements = false;

		public bool? PiercingFetishGlobal = null;
		internal bool? HungerEnabledGlobal = null;

		public bool? RealismEnabledGlobal = null;
		public int difficultyGlobal;
		public bool? hardcoreModeGlobal = null;
		public bool? SFW_ModeGlobal = null;

		internal BackendGlobalSave(int defaultDifficulty)
		{
			difficultyGlobal = defaultDifficulty;
		}
	}
}
