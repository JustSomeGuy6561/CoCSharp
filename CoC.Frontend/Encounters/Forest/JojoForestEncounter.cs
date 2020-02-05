//JojoForestEncounter.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 10:07 PM
using CoC.Backend.Encounters;
using CoC.Backend.UI;
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

		protected override void RunEncounter()
		{
			throw new NotImplementedException();
		}

		protected override bool EncounterDisabled()
		{
			return Jojo.isDisabled;
		}

		protected override bool EncounterUnlocked()
		{
			return Jojo.isUnlocked && !Jojo.isFollower;
		}
	}
}
