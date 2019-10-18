﻿//TamaniEncounter.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 10:22 PM
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Frontend.Creatures.NPCs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Encounters.Forest
{
	class TamaniEncounter : RandomEncounter
	{
		private const int CHANCES = 3;
		public TamaniEncounter() : base() { }

		protected override int chances => CHANCES;

		protected override PageDataBase Run()
		{
			throw new NotImplementedException();
		}

		protected override bool encounterDisabled()
		{
			return Tamani.isDisabled;
		}

		protected override bool encounterUnlocked()
		{
			return true;
		}
	}
}
