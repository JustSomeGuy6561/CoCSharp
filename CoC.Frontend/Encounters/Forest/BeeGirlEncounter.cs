//BeeGirlEncounter.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 9:46 PM
using CoC.Backend.Encounters;
using CoC.Backend.UI;
using System;

namespace CoC.Frontend.Encounters.Forest
{
	internal sealed class BeeGirlEncounter : RandomEncounter
	{
		private const int CHANCES = 10;
		public BeeGirlEncounter() : base() { }

		protected override int chances => CHANCES;

		protected override void RunEncounter()
		{
			throw new NotImplementedException();
		}

		protected override bool EncounterDisabled()
		{
			return false;
		}

		protected override bool EncounterUnlocked()
		{
			return true;
		}
	}
}
