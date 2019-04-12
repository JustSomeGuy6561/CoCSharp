using CoC.Frontend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Creatures.NPCs
{
	internal sealed class Essrayle
	{
		private static FrontendSessionSave data => FrontendSessionSave.data;
		public static bool isUnlocked => data.essrayleUnlocked;
		public static bool caughtBySandWitches => data.essrayleCapturedBySandWitches;

	}
}
