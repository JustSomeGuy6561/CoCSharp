using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Encounters.Common
{
	class EasterEggImpEncounter : RandomEncounter
	{
		private static Player player => GameEngine.currentPlayer;
		protected override int chances => (int)Math.Round(Utils.Lerp(2, 0, player.level.AsPercent(1,20)));

		protected override bool encounterDisabled()
		{
			return player.level >= 15;
		}

		protected override bool encounterUnlocked()
		{
			return true;
		}

		protected override void Run()
		{
			throw new NotImplementedException();
		}
	}
}
