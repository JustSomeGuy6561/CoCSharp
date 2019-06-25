﻿using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using System;

namespace CoC.Frontend.Encounters.Forest
{
	internal sealed class SuccubusEncounter : RandomEncounter
	{
		private static Player player = GameEngine.currentPlayer;
		private const int CHANCES = 10;
		public SuccubusEncounter() : base() { }

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
			return player.level >= 3;
		}
	}
}