using CoC.Backend.Encounters;
using CoC.Frontend.Creatures.NPCs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Encounters.Forest
{
	internal sealed class EssrayleForestEncounter : RandomEncounter
	{
		private const int CHANCES = 4;
		public EssrayleForestEncounter() : base() {}

		protected override int chances => CHANCES;

		protected override bool encounterDisabled()
		{
			return false;
		}

		protected override bool encounterUnlocked()
		{
			return Essrayle.isUnlocked && !Essrayle.caughtBySandWitches;
		}

		protected override void Run()
		{
			throw new NotImplementedException();
		}
	}
}
