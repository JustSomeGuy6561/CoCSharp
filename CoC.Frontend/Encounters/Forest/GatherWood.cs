using CoC.Backend.Encounters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Encounters.Forest
{
	internal sealed class GatherWood : RandomEncounter
	{
		private const int CHANCES = 10;
		protected override int chances => CHANCES;

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
			throw new NotImplementedException();
		}
	}
}
