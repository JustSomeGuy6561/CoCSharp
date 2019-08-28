//BackendGlobalData.cs
//Description:
//Author: JustSomeGuy
//4/7/2019, 7:43 PM

using CoC.Backend.Engine;

namespace CoC.Backend.SaveData
{
	public sealed class BackendGlobalSave : GlobalSaveData
	{
		public static BackendGlobalSave data => SaveSystem.GetGlobalSave<BackendGlobalSave>();

		public bool UsesMetricMeasurements = false;
		public bool UsesMilitaryTime = false;

		public bool? PiercingFetishGlobal = null;
		public bool HungerEnabledGlobal = false;

		public bool RealismEnabledGlobal = false;
		public int difficultyGlobal;
		public bool hardcoreModeGlobal = false;
		public bool SFW_ModeGlobal = false;
		public int languageIndex = 0;


		public int? highestDifficultyBeaten = null;

		internal BackendGlobalSave(int defaultDifficulty)
		{
			difficultyGlobal = defaultDifficulty;
		}
	}
}
