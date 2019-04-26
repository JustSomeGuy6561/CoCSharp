using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.SaveData
{
	internal sealed class BackendSessionData : SaveData
	{
		internal static BackendSessionData data => SaveSystem.getSessionSave<BackendSessionData>();

		public bool piercingFetish = false;

		public bool hasBigTitPerk = false;

		public bool hasBigCockPerk = false;

		internal Player player;
	}
}
