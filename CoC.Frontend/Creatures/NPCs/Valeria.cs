using System;
using System.Collections.Generic;
using System.Text;
using CoC.Frontend.SaveData;

namespace CoC.Frontend.Creatures.NPCs
{
	class Valeria
	{
		private static FrontendSessionSave data => FrontendSessionSave.data;

		public static bool isUnlocked => data.ValeriaUnlocked;
		public static bool isFollower => data.ValeriaInCamp;
		public static bool isDisabled => data.ValeriaIsDisabled;

		public static bool fluidsEnabled => data.ValeriaFluidsEnabled;

		public static byte sparIntensity => data.ValeriaSparIntensity;

		public static byte totalFluids => data.ValeriaTotalFluids;

		internal static void SetCampState(bool inCamp)
		{
			data.ValeriaInCamp = inCamp;
		}
	}
}
