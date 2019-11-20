//Forest.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 8:26 PM
using CoC.Backend.Encounters;
using CoC.Backend.UI;

namespace CoC.Frontend.Encounters.Forest
{
	internal class DryadEncounter : RandomEncounter
	{
		protected override int chances => throw new System.NotImplementedException();

		protected override bool encounterDisabled()
		{
			throw new System.NotImplementedException();
		}

		protected override bool encounterUnlocked()
		{
#warning NYI
			return false;
		}

		protected override void RunEncounter()
		{
			throw new System.NotImplementedException();
		}
	}
}
