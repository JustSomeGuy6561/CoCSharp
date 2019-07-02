using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.SaveData
{
	internal sealed class BackendSessionData : SaveData
	{
		internal static BackendSessionData data => SaveSystem.getSessionSave<BackendSessionData>();

		public bool piercingFetish = false; //a perk may set this, but i think it's fine like this.

		//public bool hasBigTitPerk = false;

		//public bool hasBigCockPerk = false;

		//public ushort timesPCRecievedAnalSex = 0;
		//public bool isAnalVirgin = true;

		internal Player player; //read all player data from this. probably wont be stored as player, but w/e. 

		public bool SFW_Mode = false;
	}
}
