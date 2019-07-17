//FrontendSessionSave.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 11:12 PM
using CoC.Backend.SaveData;

namespace CoC.Frontend.SaveData
{
	internal sealed class FrontendSessionSave : Backend.SaveData.SaveData
	{

		public static FrontendSessionSave data => SaveSystem.getSessionSave<FrontendSessionSave>();

		internal bool ForestUnlocked = false;
		internal bool DeepwoodsUnlocked = false;

		internal int ForestExplorationCount = 0;
		internal int DeepwoodsExplorationCount = 0;

		internal byte corruptedGladesDestroyed = 0;

		internal bool essrayleUnlocked = false;
		internal bool essrayleCapturedBySandWitches = false;


		internal bool jojoUnlocked = false;
		internal bool jojoDisabled = false;
		internal bool jojoFollower = false;
		internal bool jojoWasFollower = false;
		internal bool jojoIsJoy = false;

		internal bool jojoCorrupted = false;
		internal bool tentacleJojo = false;

		internal bool MarbleUnlocked = false;
		internal bool MarbleIsLover = false;
		internal bool MarbleIsDisabled = false;

		internal bool TamaniIsDisabled = false;

		internal bool SillyMode = false;

	}
}
