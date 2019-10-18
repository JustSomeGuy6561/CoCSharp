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
	internal partial class Farm : PlaceBase
	{
		private static HashSet<TriggeredEncounter> optionalusInterruptus = new HashSet<TriggeredEncounter>();
		public Farm() : base(FarmName, GetTriggeredEncounters())
		{
		}

		private static HashSet<TriggeredEncounter> GetTriggeredEncounters()
		{
			return new HashSet<TriggeredEncounter>(optionalusInterruptus.Where(x => x.isActive && !x.isCompleted));
		}

		static Farm()
		{

		}

		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.FarmUnlocked;
			protected set => FrontendSessionSave.data.FarmUnlocked = value;
		}

		public override bool isDisabled
		{
			get => FrontendSessionSave.data.FarmDisabled;
			protected set => FrontendSessionSave.data.FarmDisabled = value;
		}

		public override int timesVisited
		{
			get => FrontendSessionSave.data.FarmExplorationCount;
			protected set => FrontendSessionSave.data.FarmExplorationCount = value;
		}

		protected override PageDataBase ExplorePlace()
		{
			throw new NotImplementedException();
		}

		protected override SimpleDescriptor UnlockText => FarmUnlockText;
	}
}
