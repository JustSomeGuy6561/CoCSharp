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
	internal partial class Cathedral : PlaceBase
	{
		private static HashSet<TriggeredEncounter> optionalusInterruptus = new HashSet<TriggeredEncounter>();
		public Cathedral() : base(CathedralName, GetTriggeredEncounters())
		{
		}

		private static HashSet<TriggeredEncounter> GetTriggeredEncounters()
		{
			return new HashSet<TriggeredEncounter>(optionalusInterruptus.Where(x => x.isActive && !x.isCompleted));
		}

		static Cathedral()
		{

		}

		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.CathedralUnlocked;
			protected set => FrontendSessionSave.data.CathedralUnlocked = value;
		}

		public override bool isDisabled
		{
			get => FrontendSessionSave.data.CathedralDisabled;
			protected set => FrontendSessionSave.data.CathedralDisabled = value;
		}

		public override int timesVisited
		{
			get => FrontendSessionSave.data.CathedralExplorationCount;
			protected set => FrontendSessionSave.data.CathedralExplorationCount = value;
		}

		protected override PageDataBase ExplorePlace()
		{
			throw new NotImplementedException();
		}

		protected override SimpleDescriptor UnlockText => CathedralUnlockText;
	}
}
