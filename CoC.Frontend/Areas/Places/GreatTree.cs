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
	internal partial class GreatTree : PlaceBase
	{
		private static HashSet<TriggeredEncounter> optionalusInterruptus = new HashSet<TriggeredEncounter>();
		public GreatTree() : base(GreatTreeName, GetTriggeredEncounters())
		{
		}

		private static HashSet<TriggeredEncounter> GetTriggeredEncounters()
		{
			return new HashSet<TriggeredEncounter>(optionalusInterruptus.Where(x => x.isActive && !x.isCompleted));
		}

		static GreatTree()
		{

		}

		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.GreatTreeUnlocked;
			protected set => FrontendSessionSave.data.GreatTreeUnlocked = value;
		}

		public override bool isDisabled
		{
			get => FrontendSessionSave.data.GreatTreeDisabled;
			protected set => FrontendSessionSave.data.GreatTreeDisabled = value;
		}

		public override int timesVisited
		{
			get => FrontendSessionSave.data.GreatTreeExplorationCount;
			protected set => FrontendSessionSave.data.GreatTreeExplorationCount = value;
		}

		protected override void ExplorePlace()
		{
			throw new NotImplementedException();
		}

		protected override SimpleDescriptor UnlockText => GreatTreeUnlockText;
	}
}
