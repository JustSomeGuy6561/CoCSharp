using CoC.Backend.Encounters;
using CoC.Backend.Engine;

namespace CoC.Frontend.Encounters.Lake
{
	class RathazulLakeEncounter : SemiRandomEncounter
	{
		static int maxRunsWithoutProcing = 5;
		public RathazulLakeEncounter() : base(maxRunsWithoutProcing)
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
