using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Frontend.Areas.Locations;
using CoC.Frontend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Encounters.Forest
{
	internal sealed class FindDeepwoods : TriggeredEncounter
	{
		private static bool deepwoodsFound => Deepwoods.Unlocked;
		protected override bool isTriggered()
		{
			return !deepwoodsFound && Areas.Locations.Forest.timesExploredForest >= 20;
		}

		protected override void Run()
		{
			GameEngine.currentLocation = new Deepwoods();
			GameEngine.currentLocation.Unlock();
		}

		protected override bool encounterDisabled()
		{
			return deepwoodsFound;
		}

		protected override bool encounterUnlocked()
		{
			return true;
		}
	}
}
