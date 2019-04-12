﻿using CoC.Backend.Encounters;
using System;

namespace CoC.Frontend.Encounters.Forest
{
	internal sealed class BeeGirlEncounter : RandomEncounter
	{
		private const int CHANCES = 10;
		public BeeGirlEncounter() : base() { }

		protected override int chances => CHANCES;

		protected override void Run()
		{
			throw new NotImplementedException();
		}

		protected override bool encounterDisabled()
		{
			return false;
		}

		protected override bool encounterUnlocked()
		{
			return true;
		}
	}
}
