//BackendSessionData.cs
//Description:
//Author: JustSomeGuy
//4/7/2019, 7:27 PM
using CoC.Backend.Creatures;
using System;

namespace CoC.Backend.SaveData
{
	public sealed class BackendSessionSave : SaveData
	{
		public static BackendSessionSave data => SaveSystem.getSessionSave<BackendSessionSave>();

		public bool piercingFetish = false; //a perk may set this, but i think it's fine like this.

		//could be stored in frontend, idk. 
		public bool HungerEnabled = false;
		public bool RealismEnabled = false;

		//placeholder. a form of player will be stored in here, but only for saving and loading. it will otherwise be ignored. 
		//internal Player player; 

		public bool SFW_Mode = false;

		public int difficulty = 0;
		public bool hardcoreMode = false;

		public byte NumTimeNewGamePlus = 0;
		public byte NewGamePlusLevel => Math.Min(NumTimeNewGamePlus, (byte)4);

		public bool UsesMetricMeasurements = false;
	}

}
