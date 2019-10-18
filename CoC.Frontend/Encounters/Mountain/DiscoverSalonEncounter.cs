using CoC.Backend.Encounters;
using CoC.Backend.UI;

namespace CoC.Frontend.Encounters.Mountain
{
	internal class DiscoverSalonEncounter : SemiRandomEncounter
	{
		public DiscoverSalonEncounter() : base(9001)
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