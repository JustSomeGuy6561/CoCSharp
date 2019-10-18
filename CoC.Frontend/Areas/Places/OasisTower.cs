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
	internal partial class OasisTower : PlaceBase
	{
		private static HashSet<TriggeredEncounter> optionalusInterruptus = new HashSet<TriggeredEncounter>();
		public OasisTower() : base(OasisTowerName, GetTriggeredEncounters())
		{
		}

		private static HashSet<TriggeredEncounter> GetTriggeredEncounters()
		{
			return new HashSet<TriggeredEncounter>(optionalusInterruptus.Where(x => x.isActive && !x.isCompleted));
		}

		static OasisTower()
		{

		}

		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.OasisTowerUnlocked;
			protected set => FrontendSessionSave.data.OasisTowerUnlocked = value;
		}

		public override bool isDisabled
		{
			get => FrontendSessionSave.data.OasisTowerDisabled;
			protected set => FrontendSessionSave.data.OasisTowerDisabled = value;
		}

		public override int timesVisited
		{
			get => FrontendSessionSave.data.OasisTowerExplorationCount;
			protected set => FrontendSessionSave.data.OasisTowerExplorationCount = value;
		}

		protected override PageDataBase ExplorePlace()
		{
			throw new NotImplementedException();
		}

		protected override SimpleDescriptor UnlockText => OasisTowerUnlockText;
	}
}
