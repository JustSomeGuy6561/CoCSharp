using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Frontend.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Areas.Places
{
	//Called IngnamPlace to differentiate between this as a visitable place and a home base (grimdark mode)
	//At the moment, Ingnam is more or less impossible to visit outside of the prologue, though that could change in the future. 
	internal partial class IngnamPlace : PlaceBase
	{
		private static HashSet<TriggeredEncounter> optionalusInterruptus = new HashSet<TriggeredEncounter>();
		public IngnamPlace() : base(IngnamPlaceName, GetTriggeredEncounters())
		{
		}

		private static HashSet<TriggeredEncounter> GetTriggeredEncounters()
		{
			return new HashSet<TriggeredEncounter>(optionalusInterruptus.Where(x => x.isActive && !x.isCompleted));
		}

		static IngnamPlace()
		{

		}

		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.IngnamPlaceUnlocked;
			protected set => FrontendSessionSave.data.IngnamPlaceUnlocked = value;
		}

		public override bool isDisabled
		{
			get => FrontendSessionSave.data.IngnamPlaceDisabled;
			protected set => FrontendSessionSave.data.IngnamPlaceDisabled = value;
		}

		public override int timesVisited
		{
			get => FrontendSessionSave.data.IngnamPlaceExplorationCount;
			protected set => FrontendSessionSave.data.IngnamPlaceExplorationCount = value;
		}

		protected override PageDataBase ExplorePlace()
		{
			throw new NotImplementedException();
		}

		protected override SimpleDescriptor UnlockText => IngnamPlaceUnlockText;
	}
}
