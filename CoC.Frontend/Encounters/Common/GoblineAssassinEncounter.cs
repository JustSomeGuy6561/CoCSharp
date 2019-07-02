﻿//GoblineAssassinEncounter.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 3:59 AM
using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Encounters.Common
{
	internal sealed class GoblinAssassinEncounter : RandomEncounter
	{
		protected override int chances => Utils.LerpRound(player.level, 10, player.level, 10, 40);

		protected override bool encounterDisabled()
		{
			return false;
		}

		protected override bool encounterUnlocked()
		{
			return player.level >= 10;
		}

		protected override void Run()
		{
			throw new NotImplementedException();
		}
	}
}
