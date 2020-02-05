//TripOnRoot.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 9:46 PM
using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.UI;
using CoC.Frontend.UI;

namespace CoC.Frontend.Encounters.Forest
{
	internal sealed partial class TripOnRoot : RandomEncounter
	{

		private const int CHANCES = 20;
		public TripOnRoot() : base() { }

		protected override int chances => CHANCES;

		protected override void RunEncounter()
		{
			//player.TakeDamage(10);
			GameEngine.currentlyControlledCharacter.TakeDamage(10);
			StandardDisplay currentDisplay = DisplayManager.GetCurrentDisplay();
			currentDisplay.OutputText(flavorText());
		}

		protected override bool EncounterDisabled()
		{
			return false;
		}

		protected override bool EncounterUnlocked()
		{
			return true;
		}
	}
}
