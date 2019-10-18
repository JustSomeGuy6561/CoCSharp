//TripOnRoot.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 9:46 PM
using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.UI;

namespace CoC.Frontend.Encounters.Forest
{
	internal sealed partial class TripOnRoot : RandomEncounter
	{

		private const int CHANCES = 20;
		public TripOnRoot() : base() { }

		protected override int chances => CHANCES;

		protected override void Run(DisplayBase currentDisplay)
		{
			//player.TakeDamage(10);
			GameEngine.currentlyControlledCharacter.TakeDamage(10);
			currentDisplay.OutputText(flavorText());
		}

		protected override bool encounterDisabled()
		{
			return false;
		}

		protected override bool encounterUnlocked()
		{
			return true;
		}
	}
}
