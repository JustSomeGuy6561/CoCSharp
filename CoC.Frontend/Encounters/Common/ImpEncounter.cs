using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Encounters.Common
{
	internal sealed class ImpEncounter : RandomEncounter
	{
		private static Player player => GameEngine.currentPlayer;

		internal static RandomEncounter[] AllImpEncounters()
		{
			return new RandomEncounter[] { new EasterEggImpEncounter(), new ImpEncounter(), new ImpLordEncounter(), new ImpWarlordEncounter(), new ImpOverlordEncounter() };
		}

		public ImpEncounter() : base() { }

		protected override int chances => (int)Math.Round(Utils.Lerp(20, 10, player.level.AsPercent(1, 20)));
		protected override void Run()
		{
			throw new NotImplementedException();
		}

		protected override bool encounterUnlocked()
		{
			return true;
		}

		protected override bool encounterDisabled()
		{
			return false;
		}
	}
}
