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
	internal partial class TelAdre : PlaceBase
	{
		private static HashSet<TriggeredEncounter> optionalusInterruptus = new HashSet<TriggeredEncounter>();
		public TelAdre() : base(TelAdreName, GetTriggeredEncounters())
		{
		}

		private static HashSet<TriggeredEncounter> GetTriggeredEncounters()
		{
			return new HashSet<TriggeredEncounter>(optionalusInterruptus.Where(x => x.isActive && !x.isCompleted));
		}

		static TelAdre()
		{

		}

		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.TelAdreUnlocked;
			protected set => FrontendSessionSave.data.TelAdreUnlocked = value;
		}

		public override bool isDisabled
		{
			get => FrontendSessionSave.data.TelAdreDisabled;
			protected set => FrontendSessionSave.data.TelAdreDisabled = value;
		}

		public override int timesVisited
		{
			get => FrontendSessionSave.data.TelAdreExplorationCount;
			protected set => FrontendSessionSave.data.TelAdreExplorationCount = value;
		}

		protected override void ExplorePlace(DisplayBase currentDisplay)
		{
			throw new NotImplementedException();
		}

		protected override SimpleDescriptor UnlockText => TelAdreUnlockText;
	}
}
