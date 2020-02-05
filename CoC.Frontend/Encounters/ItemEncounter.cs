using CoC.Backend;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.Items;
using CoC.Backend.UI;
using CoC.Frontend.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Encounters
{
	internal class ItemEncounter : RandomEncounter
	{
		protected readonly CapacityItem item;
		protected readonly int chance;
		protected readonly SimpleDescriptor obtainText;
		public ItemEncounter(CapacityItem drop, int dropRate, SimpleDescriptor gainItemText)
		{
			item = drop;
			chance = dropRate;
			obtainText = gainItemText;
		}

		protected override int chances => chance;

		protected override bool EncounterDisabled()
		{
			return false;
		}

		protected override bool EncounterUnlocked()
		{
			return true;
		}

		protected override void RunEncounter()
		{
			StandardDisplay currentDisplay = DisplayManager.GetCurrentDisplay();
			currentDisplay.OutputText(obtainText());
			currentDisplay.DoNext(() => GameEngine.UseHoursGoToBase(1));
		}
	}
}
