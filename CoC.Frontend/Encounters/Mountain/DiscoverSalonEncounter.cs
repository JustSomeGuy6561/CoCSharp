﻿using CoC.Backend.Encounters;
using CoC.Backend.UI;

namespace CoC.Frontend.Encounters.Mountain
{
	internal class DiscoverSalonEncounter : SemiRandomEncounter
	{
		public DiscoverSalonEncounter() : base(9001)
		{
		}

		protected override int chances => throw new System.NotImplementedException();

		protected override bool EncounterDisabled()
		{
			throw new System.NotImplementedException();
		}

		protected override bool EncounterUnlocked()
		{
			throw new System.NotImplementedException();
		}

		protected override void RunEncounter()
		{
			throw new System.NotImplementedException();
		}
	}
}
