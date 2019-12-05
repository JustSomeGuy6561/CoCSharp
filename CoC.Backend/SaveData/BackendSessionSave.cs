//BackendSessionData.cs
//Description:
//Author: JustSomeGuy
//4/7/2019, 7:27 PM
using CoC.Backend.Creatures;
using System;

namespace CoC.Backend.SaveData
{
	public sealed class BackendSessionSave : SessionSaveData
	{
		public static BackendSessionSave data => SaveSystem.GetSessionSave<BackendSessionSave>();

		public bool piercingFetishEnabled => piercingFetishStore ?? false;

		public bool? piercingFetishStore = null; //a perk may set this, but i think it's fine like this.
		public bool playerAskedAboutPiercingFetish = false; //using the classic formula, which i assume was before the fetish settings page existed, the players have the 
															//chance to set the fetish during regular gameplay. We'll allow this behavior, but only if the piercingFetishStore is null, and the global setting is also null.

		//could be stored in frontend, idk. 
		public bool HungerEnabled = false;
		public bool RealismEnabled = false;

		//placeholder. a form of player will be stored in here, but only for saving and loading. it will otherwise be ignored. 
		//internal Player player; 

		public bool SFW_Mode = false;

		public int difficulty
		{
			get => _difficulty;
			set
			{
				_difficulty = value;
				if (difficulty < lowestDifficultyForThisCampaign || !SaveSystem.isSessionActive)
				{
					lowestDifficultyForThisCampaign = value;
				}
			}
		}
		private int _difficulty = 1;
		public int lowestDifficultyForThisCampaign = 1;
		public bool hardcoreMode = false;

		internal bool? SillyModeLocal = false;

		public byte NumTimeNewGamePlus = 0;

		public byte NewGamePlusLevel => Math.Min(NumTimeNewGamePlus, (byte)4);
	}

}
