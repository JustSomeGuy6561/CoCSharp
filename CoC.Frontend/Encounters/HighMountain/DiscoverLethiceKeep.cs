using CoC.Backend.Encounters;
using CoC.Backend.Engine;

namespace CoC.Frontend.Encounters.HighMountain
{
	internal class DiscoverLethiceKeep : SemiRandomEncounter
	{
		private const byte BAD_LUCK_VALUE = 5;
		public DiscoverLethiceKeep() : base(BAD_LUCK_VALUE)
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

		protected override PageDataBase Run()
		{
			throw new System.NotImplementedException();
		}
	}
}