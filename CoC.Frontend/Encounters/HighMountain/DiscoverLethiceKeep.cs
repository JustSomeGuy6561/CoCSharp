using CoC.Backend.Encounters;
using CoC.Backend.UI;

namespace CoC.Frontend.Encounters.HighMountain
{
	internal class DiscoverLethiceKeep : SemiRandomEncounter
	{
		private const byte BAD_LUCK_VALUE = 5;
		public DiscoverLethiceKeep() : base(BAD_LUCK_VALUE)
		{
		}

		protected override int chances => throw new System.NotImplementedException();

		protected override bool EncounterDisabled()
		{
			throw new System.NotImplementedException();
		}

		protected override bool EncounterUnlocked()
		{
			throw new System.NotImplementedException();
		}

		protected override void RunEncounter()
		{
			throw new System.NotImplementedException();
		}
	}
}
