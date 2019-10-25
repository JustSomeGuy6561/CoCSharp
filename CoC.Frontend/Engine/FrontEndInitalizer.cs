using CoC.Backend.Achievements;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Perks;
using CoC.Backend.SaveData;
using CoC.Frontend.Areas;
using CoC.Frontend.Areas.Locations;
using CoC.Frontend.UI;
using CoC.Frontend.GameCredits;
using CoC.Frontend.Perks;
using CoC.Frontend.SaveData;
using System;
using System.IO;

namespace CoC.Frontend.Engine
{
	//this class is responsible for initializing all relevant data for the frontend. It then calls the backend's initalizer, passing it the relevant data it needs from the frontend to set up the 
	//overall engine. This the most technical part of the frontend, but it's necessary to allow you to create and alter content here in the frontend, but still have it work in the backend. 
	public static class FrontendInitalizer
	{

		/// <summary>
		/// Initialize any systems or engines in the frontend that the save data requires to exist before it loads. for example, any achievements declared in the frontend (so, all of them)
		/// must be added to the achievement manager, or else they won't point to anything when the data is loaded and therefore be lost or ignored. 
		/// </summary>
		public static void PreSaveInit()
		{
			BasePerkModifiers getExtraData(Creature source) => new ExtendedPerkModifiers(source);

			BackendInitializer.PreSaveInit(() => new StandardDisplay(), DisplayManager.GetCurrentDisplay, DisplayManager.LoadDisplay, AreaManager.placeCollection, AreaManager.locationCollection, 
				AreaManager.dungeonCollection, AreaManager.homeBaseCollection, getExtraData, DifficultyManager.difficultyCollection, DifficultyManager.defaultDifficultyIndex);

			CreditManager.AddCreditCategory(new FrontendCredits());
			CreditManager.AddCreditCategory(new FrontendModCredits());

			AchievementManager.RegisterAchievement(new Achievements.StartTheGameINeedAnAchievementForDebugging());

#warning Parse global File Data accordingly. note it may be null if file does not exist.
		}

		/// <summary>
		/// Initialize any additional systems that the save engine may require, but rely on other systems that were initialized in the preSaveInit function. This is mostly a convenience
		/// function, but there may arise cases where it is legitimately difficult, or impossible, to do this any other way.
		/// </summary>
		public static void LatePreSaveInit()
		{
			BackendInitializer.LatePreSaveInit();

			//handle any post init stuff here.

		}

		public static void InitializeSaveData(FileInfo globalDataFile)
		{
			BackendInitializer.InitializeSaveData(globalDataFile);

			if (globalDataFile is null)
			{
				SaveSystem.AddGlobalSave(new FrontendGlobalSave());
				SaveSystem.AddSessionSave<FrontendSessionSave>();
			}
			else throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		public static void PostSaveInit()
		{
			BackendInitializer.PostSaveInit();
		}

		public static void FinalizeInitialization()
		{
			BackendInitializer.FinalizeInitialization();
		}
	}
}
