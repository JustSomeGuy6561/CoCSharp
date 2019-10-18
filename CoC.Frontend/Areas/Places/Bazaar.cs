using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Backend.UI;
using CoC.Frontend.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Frontend.Areas.Places
{
	internal partial class Bazaar : PlaceBase
	{
		private static HashSet<TriggeredEncounter> optionalusInterruptus = new HashSet<TriggeredEncounter>();
		public Bazaar() : base(BazaarName, GetTriggeredEncounters())
		{
		}

		private static HashSet<TriggeredEncounter> GetTriggeredEncounters()
		{
			return new HashSet<TriggeredEncounter>(optionalusInterruptus.Where(x => x.isActive && !x.isCompleted));
		}

		static Bazaar()
		{

		}

		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.BazaarUnlocked;
			protected set => FrontendSessionSave.data.BazaarUnlocked = value;
		}

		public override bool isDisabled
		{
			get => FrontendSessionSave.data.BazaarDisabled;
			protected set => FrontendSessionSave.data.BazaarDisabled = value;
		}

		public override int timesVisited
		{
			get => FrontendSessionSave.data.BazaarExplorationCount;
			protected set => FrontendSessionSave.data.BazaarExplorationCount = value;
		}

		protected override void ExplorePlace(DisplayBase currentDisplay)
		{
			throw new NotImplementedException();
		}

		protected override SimpleDescriptor UnlockText => BazaarUnlockText;
	}
}
