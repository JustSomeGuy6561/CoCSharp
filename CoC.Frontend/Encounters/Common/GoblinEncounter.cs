﻿using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Encounters.Common
{
	class GoblinEncounter : RandomEncounter
	{
		private static Player player => GameEngine.currentPlayer;


		internal static RandomEncounter[] AllGoblinEncounters()
		{
			return new RandomEncounter[] { new GoblinEncounter(), new GoblinAssassinEncounter(), new GoblinWarriorEncounter(), new GoblinShamanEncounter(), new GoblinElderEncounter() };
		}

		protected override int chances => Utils.LerpRound(1, 20, player.level, 20, 10);

		protected override bool encounterDisabled()
		{
			return false;
		}

		protected override bool encounterUnlocked()
		{
			return player.level >= 1;
		}

		protected override void Run()
		{
			throw new NotImplementedException();
		}
	}
}