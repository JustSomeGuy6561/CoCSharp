﻿using CoC.Backend.Encounters;
using CoC.Backend.UI;

namespace CoC.Frontend.Encounters.Lake
{
	class RathazulLakeEncounter : SemiRandomEncounter
	{
		static int maxRunsWithoutProcing = 5;
		public RathazulLakeEncounter() : base(maxRunsWithoutProcing)
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
