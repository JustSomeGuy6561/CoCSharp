﻿using CoC.Backend.Encounters;
using CoC.Backend.Engine;

namespace CoC.Frontend.Encounters.Plains
{
	internal class SheilaEncounter : SemiRandomEncounter
	{
		public SheilaEncounter() : base(9001)
		{
		}

		protected override int chances => throw new System.NotImplementedException();

		protected override bool encounterDisabled()
		{
			throw new System.NotImplementedException();
		}

		protected override bool encounterUnlocked()
		{
			throw new System.NotImplementedException();
		}

		protected override PageDataBase Run()
		{
			throw new System.NotImplementedException();
		}
	}
}