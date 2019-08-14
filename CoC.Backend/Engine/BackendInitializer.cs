//BackendInitializer.cs
//Description:
//Author: JustSomeGuy
//3/21/2019, 5:56 AM
using CoC.Backend.Areas;
using CoC.Backend.Creatures;
using CoC.Backend.Perks;
using CoC.Backend.SaveData;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace CoC.Backend.Engine
{
	public static class BackendInitializer
	{
		//rundown: to allow you to do whatever you want in the frontend, some data needs to be passed back here. 

		public static void Init(FileInfo globalDataFile,
			ReadOnlyDictionary<Type, Func<PlaceBase>> gamePlaces, ReadOnlyDictionary<Type, Func<LocationBase>> gameLocations, 
			ReadOnlyDictionary<Type, Func<DungeonBase>> gameDungeons, ReadOnlyDictionary<Type, Func<HomeBaseBase>> homeBases, //AreaEngine
			Func<BasePerkModifiers> perkModifiers, /*Perks*/ ReadOnlyCollection<GameDifficulty> gameDifficulties, int defaultDifficultyIndex) //Game Difficulty Engine.
		{
			//initialize the saves. 
			SaveSystem.AddSessionSave(new BackendSessionData());
			SaveSystem.AddGlobalSave(new BackendGlobalData());


			#warning Add method to read file and load global backend game data. 

			//initialize game engine.
			GameEngine.InitializeEngine(gamePlaces, gameLocations, gameDungeons, homeBases, perkModifiers, gameDifficulties, defaultDifficultyIndex);
		}
	}
}
