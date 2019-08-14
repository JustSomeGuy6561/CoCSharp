using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Frontend.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Areas.Places
{
	internal partial class Owca : PlaceBase
	{
		private static HashSet<TriggeredEncounter> optionalusInterruptus = new HashSet<TriggeredEncounter>();
		public Owca() : base(OwcaName, GetTriggeredEncounters())
		{
		}

		private static HashSet<TriggeredEncounter> GetTriggeredEncounters()
		{
			return new HashSet<TriggeredEncounter>(optionalusInterruptus.Where(x => x.isActive && !x.isCompleted));
		}

		static Owca()
		{

		}

		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.OwcaUnlocked;
			protected set => FrontendSessionSave.data.OwcaUnlocked = value;
		}

		public override bool isDisabled
		{
			get => FrontendSessionSave.data.OwcaDisabled;
			protected set => FrontendSessionSave.data.OwcaDisabled = value;
		}

		public override int timesVisited
		{
			get => FrontendSessionSave.data.OwcaExplorationCount;
			protected set => FrontendSessionSave.data.OwcaExplorationCount = value;
		}

		protected override void ExplorePlace()
		{
			throw new NotImplementedException();
		}

		protected override SimpleDescriptor UnlockText => OwcaUnlockText;
	}
}
