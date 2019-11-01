//BackendInitializer.cs
//Description:
//Author: JustSomeGuy
//3/21/2019, 5:56 AM
using CoC.Backend.Areas;
using CoC.Backend.Creatures;
using CoC.Backend.GameCredits;
using CoC.Backend.Perks;
using CoC.Backend.SaveData;
using CoC.Backend.Settings.Fetishes;
using CoC.Backend.Settings.Gameplay;
using CoC.Backend.UI;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace CoC.Backend.Engine
{
	public static class BackendInitializer
	{
		//rundown: to allow you to do whatever you want in the frontend, some data needs to be passed back here. 

		public static void PreSaveInit(Func<DisplayBase> PageConstructor, Func<DisplayBase> GetCurrentPage, Action<DisplayBase> SetCurrentPage,
			ReadOnlyDictionary<Type, Func<PlaceBase>> gamePlaces, ReadOnlyDictionary<Type, Func<LocationBase>> gameLocations, 
			ReadOnlyDictionary<Type, Func<DungeonBase>> gameDungeons, ReadOnlyDictionary<Type, Func<HomeBaseBase>> homeBases, //AreaEngine
			ReadOnlyCollection<GameDifficulty> gameDifficulties, int defaultDifficultyIndex) //Game Difficulty Engine.
		{
			//add the fetish/game settings.

			FetishSettingsManager.IncludeFetish(new PiercingFetish());

			GameplaySettingsManager.IncludeGameplaySetting(new DifficultySetting(gameDifficulties));
			GameplaySettingsManager.IncludeGameplaySetting(new HungerSettings());
			GameplaySettingsManager.IncludeGameplaySetting(new HardcoreSettings());
			GameplaySettingsManager.IncludeGameplaySetting(new SFW_Settings());
			GameplaySettingsManager.IncludeGameplaySetting(new MeasurementSettings());
			GameplaySettingsManager.IncludeGameplaySetting(new TimeDisplaySettings());

			CreditManager.AddCreditCategory(new BackendCredits());

#warning Add method to read file and load global backend game data. 

			//initialize game engine.
			GameEngine.InitializeEngine(PageConstructor, GetCurrentPage, SetCurrentPage, gamePlaces, gameLocations, gameDungeons, homeBases, 
				gameDifficulties, defaultDifficultyIndex);
		}

		public static void LatePreSaveInit()
		{
			//CreditManager.AddCreditCategory(new LocalizationCredits());
			CreditManager.AddCreditCategory(new MiscellaneousCredits());

			
		}

		public static void InitializeSaveData(FileInfo globalDataFile)
		{
			if (globalDataFile is null)
			{
				//initialize the saves. 
				SaveSystem.AddGlobalSave(new BackendGlobalSave(GameEngine.defaultDifficultyIndex));
				SaveSystem.AddSessionSave<BackendSessionSave>();
			}
			else
			{
				//deserialize the global save data for the backend save. 
				//initialize the 
				throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
			}

			GameEngine.LoadFileBackend(globalDataFile);
		}

		public static void PostSaveInit()
		{
			foreach (var data in GameplaySettingsManager.gameSettings)
			{
				data.PostLocalSessionInit();
			}

			foreach (var data in FetishSettingsManager.fetishes)
			{
				data.PostLocalSessionInit();
			}

			GameEngine.PostSaveInit();
		}

		public static void FinalizeInitialization()
		{
			

		}
	}
}
