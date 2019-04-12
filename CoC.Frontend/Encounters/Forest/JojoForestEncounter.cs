using CoC.Backend.Encounters;
using CoC.Frontend.Creatures.NPCs;
using System;

namespace CoC.Frontend.Encounters.Forest
{
	internal sealed class JojoForestEncounter : SemiRandomEncounter
	{
		private const int CHANCES = 20;
		private const int PROC_COUNTER = 8;
		public JojoForestEncounter() : base(PROC_COUNTER) { }

		protected override int chances => CHANCES;

		protected override void Run()
		{
			throw new NotImplementedException();
		}

		protected override bool encounterDisabled()
		{
			return Jojo.isDisabled;
		}

		protected override bool encounterUnlocked()
		{
			return Jojo.isUnlocked && !Jojo.isFollower;
		}
	}
}
