//EasterEggImpEncounter.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 3:36 AM
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
		protected override int chances => Utils.LerpRound(1, 20, player.level, 2, 0);
			
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
