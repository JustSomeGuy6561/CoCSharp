using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.UI;
using CoC.Frontend.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Frontend.Areas.Places
{
	internal partial class Salon : PlaceBase
	{
		private static HashSet<TriggeredEncounter> optionalusInterruptus = new HashSet<TriggeredEncounter>();
		public Salon() : base(SalonName, GetTriggeredEncounters())
		{
		}

		private static HashSet<TriggeredEncounter> GetTriggeredEncounters()
		{
			return new HashSet<TriggeredEncounter>(optionalusInterruptus.Where(x => x.isActive && !x.isCompleted));
		}

		static Salon()
		{

		}

		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.SalonUnlocked;
			protected set => FrontendSessionSave.data.SalonUnlocked = value;
		}

		public override bool isDisabled
		{
			get => FrontendSessionSave.data.SalonDisabled;
			protected set => FrontendSessionSave.data.SalonDisabled = value;
		}

		public override int timesVisited
		{
			get => FrontendSessionSave.data.SalonExplorationCount;
			protected set => FrontendSessionSave.data.SalonExplorationCount = value;
		}

		protected override void ExplorePlace(DisplayBase currentDisplay)
		{
			throw new NotImplementedException();
		}

		protected override SimpleDescriptor UnlockText => SalonUnlockText;
	}
}
