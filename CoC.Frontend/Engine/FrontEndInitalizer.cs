using CoC.Backend.Perks;
using CoC.Backend.SaveData;
using CoC.Frontend.Areas;
using CoC.Frontend.Areas.Locations;
using CoC.Frontend.Perks;
using CoC.Frontend.SaveData;
using CoC.Frontend.UI;
using System.IO;

namespace CoC.Frontend.Engine
{
	//this class is responsible for initializing all relevant data for the frontend. It then calls the backend's initalizer, passing it the relevant data it needs from the frontend to set up the 
	//overall engine. This the most technical part of the frontend, but it's necessary to allow you to create and alter content here in the frontend, but still have it work in the backend. 
	public static class FrontendInitalizer
	{
		public static void Init(FileInfo globalDataFile)
		{
			SaveSystem.AddGlobalSave(new FrontendGlobalSave());
			SaveSystem.AddSessionSave(new FrontendSessionSave());

			BasePerkModifiers getExtraData() => new ExtraPerkModifiers();

			Backend.Engine.BackendInitializer.Init(globalDataFile, MenuHelpers.DoNext, TextOutput.OutputText, 
				AreaManager.placeCollection, AreaManager.locationCollection, getExtraData, DifficultyManager.difficultyCollection);


			#warning Parse global File Data accordingly. note it may be null if file does not exist.
		}


	}
}
