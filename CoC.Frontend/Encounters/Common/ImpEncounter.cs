//ImpEncounter.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:43 AM
using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.UI;
using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Encounters.Common
{
	internal sealed class ImpEncounter : RandomEncounter
	{

		internal static RandomEncounter[] AllImpEncounters()
		{
			return new RandomEncounter[] { new EasterEggImpEncounter(), new ImpEncounter(), new ImpLordEncounter(), new ImpWarlordEncounter(), new ImpOverlordEncounter() };
		}

		public ImpEncounter() : base() { }

		protected override int chances => Utils.LerpRound(1,  20, (int)player.level, 20, 10);
		protected override void RunEncounter()
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
