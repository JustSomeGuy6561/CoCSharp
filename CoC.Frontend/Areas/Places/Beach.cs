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
	internal partial class Beach : PlaceBase
	{
		private static HashSet<TriggeredEncounter> optionalusInterruptus = new HashSet<TriggeredEncounter>();
		public Beach() : base(BeachName, GetTriggeredEncounters())
		{
		}

		private static HashSet<TriggeredEncounter> GetTriggeredEncounters()
		{
			return new HashSet<TriggeredEncounter>(optionalusInterruptus.Where(x => x.isActive && !x.isCompleted));
		}

		static Beach()
		{

		}

		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.BeachUnlocked;
			protected set => FrontendSessionSave.data.BeachUnlocked = value;
		}

		public override bool isDisabled
		{
			get => FrontendSessionSave.data.BeachDisabled;
			protected set => FrontendSessionSave.data.BeachDisabled = value;
		}

		public override int timesVisited
		{
			get => FrontendSessionSave.data.BeachExplorationCount;
			protected set => FrontendSessionSave.data.BeachExplorationCount = value;
		}

		protected override void ExplorePlace()
		{
			throw new NotImplementedException();
		}

		protected override SimpleDescriptor UnlockText => BeachUnlockText;
	}
}
