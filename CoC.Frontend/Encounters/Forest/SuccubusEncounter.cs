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

		protected override void Run(DisplayBase currentDisplay)
		{
			throw new NotImplementedException();
		}

		protected override bool encounterDisabled()
		{
			return false;
		}

		protected override bool encounterUnlocked()
		{
			return player.level >= 3;
		}
	}
}
