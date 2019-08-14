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
	internal partial class TownRuins : PlaceBase
	{
		private static HashSet<TriggeredEncounter> optionalusInterruptus = new HashSet<TriggeredEncounter>();
		public TownRuins() : base(TownRuinsName, GetTriggeredEncounters())
		{
		}

		private static HashSet<TriggeredEncounter> GetTriggeredEncounters()
		{
			return new HashSet<TriggeredEncounter>(optionalusInterruptus.Where(x => x.isActive && !x.isCompleted));
		}

		static TownRuins()
		{

		}

		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.TownRuinsUnlocked;
			protected set => FrontendSessionSave.data.TownRuinsUnlocked = value;
		}

		public override bool isDisabled
		{
			get => FrontendSessionSave.data.TownRuinsDisabled;
			protected set => FrontendSessionSave.data.TownRuinsDisabled = value;
		}

		public override int timesVisited
		{
			get => FrontendSessionSave.data.TownRuinsExplorationCount;
			protected set => FrontendSessionSave.data.TownRuinsExplorationCount = value;
		}

		protected override void ExplorePlace()
		{
			throw new NotImplementedException();
		}

		protected override SimpleDescriptor UnlockText => TownRuinsUnlockText;
	}
}
