using CoC.Backend;
using CoC.Backend.Encounters;
using System;
using System.Collections.Generic;
using System.Text;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Encounters
{
	internal class ItemEncounter : RandomEncounter
	{
		protected readonly Item item;
		protected readonly int chance;
		protected readonly SimpleDescriptor obtainText;
		public ItemEncounter(Item drop, int dropRate, SimpleDescriptor gainItemText)
		{
			item = drop;
			chance = dropRate;
			obtainText = gainItemText;
		}

		protected override int chances => chance;

		protected override bool encounterDisabled()
		{
			return false;
		}

		protected override bool encounterUnlocked()
		{
			return true;
		}

		protected override void Run()
		{
			OutputText(obtainText());
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
	}
}
