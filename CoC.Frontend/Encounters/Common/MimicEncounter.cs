using CoC.Backend;
using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using System;
using System.Collections.Generic;
using System.Text;
namespace CoC.Frontend.Encounters.Common
{
	class MimicEncounter : RandomEncounter
	{
		private const int CHANCES = 10;
		private static Player player => GameEngine.currentPlayer;

		private readonly SimpleDescriptor introText;

		public MimicEncounter(SimpleDescriptor locationIntroText) : base()
		{
			introText = locationIntroText;
		}

		protected override int chances => CHANCES;

		protected override bool encounterDisabled()
		{
			return false;
		}

		protected override bool encounterUnlocked()
		{
			return player.level >= 3;
		}

		protected override void Run()
		{
			throw new NotImplementedException();
		}
	}
}
