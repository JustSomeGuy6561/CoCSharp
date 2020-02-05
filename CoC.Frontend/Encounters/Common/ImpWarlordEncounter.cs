//ImpWarlordEncounter.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 3:50 AM
using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.UI;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Encounters.Common
{
	class ImpWarlordEncounter : RandomEncounter
	{
		protected override int chances => Utils.LerpRound(16, 20, (int)player.level, 10, 80);

		protected override bool EncounterDisabled()
		{
			return false;
		}

		protected override bool EncounterUnlocked()
		{
			return player.level >= 16;
		}

		protected override void RunEncounter()
		{
			throw new NotImplementedException();
		}
	}
}
