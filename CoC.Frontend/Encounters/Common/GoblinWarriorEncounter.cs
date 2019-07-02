//GoblinWarriorEncounter.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 4:00 AM
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
		protected override int chances => Utils.LerpRound(12, 18, player.level, 10, 60);

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
