//TripOnRoot.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 9:46 PM
using CoC.Backend.Creatures;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Encounters.Forest
{
	internal sealed partial class TripOnRoot : RandomEncounter
	{

		private const int CHANCES = 20;
		public TripOnRoot() : base() { }

		protected override int chances => CHANCES;

		protected override void Run()
		{
			//player.TakeDamage(10);
			OutputText(flavorText());
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
