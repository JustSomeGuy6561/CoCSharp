//MarbleForestEncounter.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 10:02 PM
using CoC.Backend.Encounters;
using CoC.Frontend.Creatures.NPCs;
using System;

namespace CoC.Frontend.Encounters.Forest
{
	class MarbleForestEncounter : SemiRandomEncounter
	{
		private const int CHANCES = 2;
		private const int PROC_COUNTER = 10;
		public MarbleForestEncounter() : base(PROC_COUNTER) { }

		protected override int chances => CHANCES;

		protected override void Run()
		{
			throw new NotImplementedException();
		}

		protected override bool encounterDisabled()
		{
			return Marble.isDisabled || Marble.isLover;
		}

		protected override bool encounterUnlocked()
		{
			return Marble.isUnlocked;
		}
	}
}