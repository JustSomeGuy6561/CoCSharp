using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Encounters.Common
{
	class GoblinWarriorEncounter : RandomEncounter
	{
		private static Player player => GameEngine.currentPlayer;
		protected override int chances => (int)Math.Round(Utils.Lerp(10, 60, player.level.AsPercent(12,18)));

		protected override bool encounterDisabled()
		{
			return false;
		}

		protected override bool encounterUnlocked()
		{
			return player.level >= 12;
		}

		protected override void Run()
		{
			throw new NotImplementedException();
		}
	}
}
