//BigJunkEncounter.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 9:49 PM
using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using System;
using System.Diagnostics;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Encounters.Common
{
	internal sealed partial class BigJunkEncounter : RandomEncounter
	{
		private readonly Type sourceLocation;

		public BigJunkEncounter(Type location) : base()
		{
			sourceLocation = location ?? throw new ArgumentNullException(nameof(location));
		}
		protected override int chances => bigJunkChance();

		private int bigJunkChance()
		{
			//if (player.largestCock > player.height && player.totalCockGirth >= 12)
			//{
			//	return (int)Math.Floor(20 * (1 + (player.LargestCock() - player.heuight) / 25.0));
			//}
			return 0;
		}

		protected override void Run()
		{
			bool isForest = true, isLake = false;

			if (sourceLocation == typeof(Areas.Locations.Lake))
			{
				isForest = false;
				isLake = true;
			}
			else if (sourceLocation == typeof(Areas.Locations.Desert))
			{
				isForest = false;
			}
#if DEBUG
			else if (sourceLocation != typeof(Areas.Locations.Forest) && sourceLocation != typeof(Areas.Locations.Deepwoods))
			{
				Debug.WriteLine("An unexpected area has been given a big junk text location. it will default to using the forest. if this is not ideal, implement a new text for" +
					"this new location within the big junk encounter text function.");
			}
#endif
			OutputBigJunkText(isForest, isLake);
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
			//dynStats("lus", 25 + rand(player.cor / 5), "scale", false);
			//player.changeFatigue(5);
			//doNext(camp.returnToCampUseOneHour);
		}

		protected override bool encounterDisabled()
		{
			return false;
		}

		protected override bool encounterUnlocked()
		{
			//return player.largestCock > player.height && player.totalCockGirth >= 12;
			return false;
		}
	}
}
