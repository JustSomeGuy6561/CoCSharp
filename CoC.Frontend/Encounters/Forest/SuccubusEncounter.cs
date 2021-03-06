﻿//SuccubusEncounter.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 9:46 PM
using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.UI;
using System;

namespace CoC.Frontend.Encounters.Forest
{
	internal sealed class SuccubusEncounter : RandomEncounter
	{
		private const int CHANCES = 10;
		public SuccubusEncounter() : base() { }

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
			return player.level >= 3;
		}
	}
}
