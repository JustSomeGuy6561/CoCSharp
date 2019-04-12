using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Encounters.Common
{
	internal sealed class ImpLordEncounter : RandomEncounter
	{
		private static Player player => GameEngine.currentPlayer;
		protected override int chances => (int)Math.Round(Utils.Lerp(10, 40, player.level.AsPercent(8,16)));

		protected override bool encounterDisabled()
		{
			return false;
		}

		protected override bool encounterUnlocked()
		{
			return player.level >= 8;
		}

		protected override void Run()
		{
			throw new NotImplementedException();
		}
	}
}
