//GoblinEncounter.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 3:21 AM
using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.UI;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Encounters.Common
{
	class GoblinEncounter : RandomEncounter
	{


		internal static RandomEncounter[] AllGoblinEncounters()
		{
			return new RandomEncounter[] { new GoblinEncounter(), new GoblinAssassinEncounter(), new GoblinWarriorEncounter(), new GoblinShamanEncounter(), new GoblinElderEncounter() };
		}

		protected override int chances => Utils.LerpRound(1, 20, (int)player.level, 20, 10);

		protected override bool encounterDisabled()
		{
			return false;
		}

		protected override bool encounterUnlocked()
		{
			return player.level >= 1;
		}

		protected override void RunEncounter()
		{
			throw new NotImplementedException();
		}
	}
}
