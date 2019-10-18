//FindDeepwoods.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 10:40 PM
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.UI;
using CoC.Frontend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Encounters.Forest
{
	internal sealed class FindDeepwoods : TriggeredEncounter
	{
		private static bool deepwoodsFound => Areas.Locations.Deepwoods.Unlocked;
		protected override bool isTriggered()
		{
			return !deepwoodsFound && Areas.Locations.Forest.timesExploredForest >= 20;
		}

		protected override void Run(DisplayBase currentDisplay)
		{
			GameEngine.UnlockArea<Areas.Locations.Deepwoods>(out string unlockText);
			currentDisplay.OutputText(unlockText);
			currentDisplay.DoNext(() => GameEngine.UseHoursGoToBase(2));
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
