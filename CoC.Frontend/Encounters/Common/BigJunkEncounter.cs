﻿//BigJunkEncounter.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 9:49 PM
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using CoC.Frontend.UI;
using System;
using System.Diagnostics;
using System.Linq;

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

		private bool bigJunkAvailable => player.genitals.LongestCockLength() > player.build.heightInInches && player.cocks.Sum(x => x.girth) >= 12;

		private int bigJunkChance()
		{
			if (bigJunkAvailable)
			{
				return (int)Math.Floor(20 * (1 + (player.genitals.LongestCockLength() - player.build.heightInInches) / 25.0));
			}
			return 0;
		}

		protected override void RunEncounter()
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

			StandardDisplay currentDisplay = DisplayManager.GetCurrentDisplay();
			OutputBigJunkText(currentDisplay, isForest, isLake);
			player.IncreaseLustBy(25 + Utils.Rand(player.corruption / 5));
			player.GainFatigue(5);
			currentDisplay.DoNext(() => GameEngine.UseHoursGoToBase(1));
		}

		protected override bool EncounterDisabled()
		{
			return false;
		}

		protected override bool EncounterUnlocked()
		{
			return bigJunkAvailable;
		}
	}
}
