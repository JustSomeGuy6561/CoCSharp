//FrontendSessionSave.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 11:12 PM
using CoC.Backend.SaveData;

namespace CoC.Frontend.SaveData
{
	internal sealed class FrontendSessionSave : Backend.SaveData.SessionSaveData
	{
		public static FrontendSessionSave data => SaveSystem.GetSessionSave<FrontendSessionSave>();

		internal bool ForestUnlocked = false;
		internal bool DeepwoodsUnlocked = false;
		internal bool LakeUnlocked = false;
		internal bool DesertUnlocked = false;
		internal bool MountainUnlocked = false;
		internal bool PlainsUnlocked = false;
		internal bool BogUnlocked = false;
		internal bool SwampUnlocked = false;
		internal bool HighMountainUnlocked = false;
		internal bool GlacialRiftUnlocked = false;
		internal bool VolcanicCragUnlocked = false;


		internal int ForestExplorationCount = 0;
		internal int DeepwoodsExplorationCount = 0;
		internal int LakeExplorationCount = 0;
		internal int DesertExplorationCount = 0;
		internal int MountainExplorationCount = 0;
		internal int PlainsExplorationCount = 0;
		internal int BogExplorationCount = 0;
		internal int SwampExplorationCount = 0;
		internal int HighMountainExplorationCount = 0;
		internal int GlacialRiftExplorationCount = 0;
		internal int VolcanicCragExplorationCount = 0;

		internal bool BazaarUnlocked = false;
		internal bool BazaarDisabled = false;
		internal int BazaarExplorationCount = 0;

		internal bool BeachUnlocked = false;
		internal bool BeachDisabled = false;
		internal int BeachExplorationCount = 0;

		internal bool CathedralUnlocked = false;
		internal bool CathedralDisabled = false;
		internal int CathedralExplorationCount = 0;

		internal bool FarmUnlocked = false;
		internal bool FarmDisabled = false;
		internal int FarmExplorationCount = 0;

		internal bool GreatTreeUnlocked = false;
		internal bool GreatTreeDisabled = false;
		internal int GreatTreeExplorationCount = 0;

		internal bool IngnamPlaceUnlocked = false;
		internal bool IngnamPlaceDisabled = false;
		internal int IngnamPlaceExplorationCount = 0;

		internal bool OasisTowerUnlocked = false;
		internal bool OasisTowerDisabled = false;
		internal int OasisTowerExplorationCount = 0;

		internal bool OwcaUnlocked = false;
		internal bool OwcaDisabled = false;
		internal int OwcaExplorationCount = 0;

		internal bool SalonUnlocked = false;
		internal bool SalonDisabled = false;
		internal int SalonExplorationCount = 0;

		internal bool TelAdreUnlocked = false;
		internal bool TelAdreDisabled = false;
		internal int TelAdreExplorationCount = 0;

		internal bool TownRuinsUnlocked = false;
		internal bool TownRuinsDisabled = false;
		internal int TownRuinsExplorationCount = 0;


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

		//will generally not be null. Null means it hasn't been set yet.

		//if null, checks for global setting, and uses that if it's not null.
		//if global is null, prompts user on first
		//possible time it's needed. for silly mode, this
		//is during game start, i guess.

		internal bool? WaterSportsLocal = null;
		internal byte cabinProgress = 0;
		internal byte wallProgress = 0;
		internal ushort wallSkullCount = 0;
		internal ushort wallStatueCount = 0;
		internal bool HyperHappyLocal = false;
	}
}
