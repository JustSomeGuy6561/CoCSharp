using CoC.Backend.Encounters;
using CoC.Backend.UI;

namespace CoC.Frontend.Encounters.Plains
{
	internal class SheilaEncounter : SemiRandomEncounter
	{
		public SheilaEncounter() : base(9001)
		{
		}

		protected override int chances => throw new System.NotImplementedException();

		protected override bool encounterDisabled()
		{
			throw new System.NotImplementedException();
		}

		protected override bool encounterUnlocked()
		{
			throw new System.NotImplementedException();
		}

		protected override void Run(DisplayBase currentDisplay)
		{
			throw new System.NotImplementedException();
		}
	}
}