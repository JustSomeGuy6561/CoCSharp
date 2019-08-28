//BackendInitializer.cs
//Description:
//Author: JustSomeGuy
//3/21/2019, 5:56 AM
using CoC.Backend.Areas;
using CoC.Backend.Creatures;
using CoC.Backend.Perks;
using CoC.Backend.SaveData;
using CoC.Backend.Settings.Fetishes;
using CoC.Backend.Settings.Gameplay;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace CoC.Backend.Engine
{
	public static class BackendInitializer
	{
		//rundown: to allow you to do whatever you want in the frontend, some data needs to be passed back here. 

		public static void Init(FileInfo globalDataFile, Action<string> output,
			ReadOnlyDictionary<Type, Func<PlaceBase>> gamePlaces, ReadOnlyDictionary<Type, Func<LocationBase>> gameLocations, 
			ReadOnlyDictionary<Type, Func<DungeonBase>> gameDungeons, ReadOnlyDictionary<Type, Func<HomeBaseBase>> homeBases, //AreaEngine
			Func<BasePerkModifiers> perkModifiers, /*Perks*/ ReadOnlyCollection<GameDifficulty> gameDifficulties, int defaultDifficultyIndex) //Game Difficulty Engine.
		{
			//initialize the saves. 
			SaveSystem.AddGlobalSave(new BackendGlobalSave(defaultDifficultyIndex));
			SaveSystem.AddSessionSave<BackendSessionSave>();

			//add the fetish/game settings.

			FetishSettingsManager.IncludeFetish(new PiercingFetish());

			GameplaySettingsManager.IncludeGameplaySetting(new DifficultySetting(gameDifficulties));
			GameplaySettingsManager.IncludeGameplaySetting(new HungerSettings());
			GameplaySettingsManager.IncludeGameplaySetting(new HardcoreSettings());
			GameplaySettingsManager.IncludeGameplaySetting(new RealismSettings());
			GameplaySettingsManager.IncludeGameplaySetting(new SFW_Settings());
			GameplaySettingsManager.IncludeGameplaySetting(new MeasurementSettings());
			GameplaySettingsManager.IncludeGameplaySetting(new TimeDisplaySettings());

#warning Add method to read file and load global backend game data. 

			//initialize game engine.
			GameEngine.InitializeEngine(output, gamePlaces, gameLocations, gameDungeons, homeBases, perkModifiers, gameDifficulties, defaultDifficultyIndex);
		}
	}
}
