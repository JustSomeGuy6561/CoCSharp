﻿//MimicEncounter.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 11:33 PM
using CoC.Backend;
using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.UI;
using System;
using System.Collections.Generic;
using System.Text;
namespace CoC.Frontend.Encounters.Common
{
	class MimicEncounter : RandomEncounter
	{
		private const int CHANCES = 10;

		private readonly SimpleDescriptor introText;

		public MimicEncounter(SimpleDescriptor locationIntroText) : base()
		{
			introText = locationIntroText;
		}

		protected override int chances => CHANCES;

		protected override bool EncounterDisabled()
		{
			return false;
		}

		protected override bool EncounterUnlocked()
		{
			return player.level >= 3;
		}

		protected override void RunEncounter()
		{
			throw new NotImplementedException();
		}
	}
}
