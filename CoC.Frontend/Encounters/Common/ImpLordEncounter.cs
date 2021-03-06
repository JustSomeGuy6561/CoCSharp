﻿//ImpLordEncounter.cs
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
	internal sealed class ImpLordEncounter : RandomEncounter
	{
		protected override int chances => Utils.LerpRound(8, 16, (int)player.level, 10, 40);

		protected override bool EncounterDisabled()
		{
			return false;
		}

		protected override bool EncounterUnlocked()
		{
			return player.level >= 8;
		}

		protected override void RunEncounter()
		{
			throw new NotImplementedException();
		}
	}
}
