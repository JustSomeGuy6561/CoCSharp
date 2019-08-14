using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Frontend.Encounters.VolcanicCrag;
using CoC.Frontend.SaveData;
using System.Collections.Generic;
using System.Linq;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Areas.Locations
{
	internal partial class VolcanicCrag : LocationBase
	{
		private const byte UNLOCKED_AT = 1;
		public VolcanicCrag() : base(VolcanicCragName, UNLOCKED_AT, GetRandomEncounters(), GetSemiRandomEncounters(), GetTriggeredEncounters()) { }

		private static readonly HashSet<RandomEncounter> randomEncounters = new HashSet<RandomEncounter>();
		private static readonly HashSet<SemiRandomEncounter> semiRandomEncounters = new HashSet<SemiRandomEncounter>();
		private static readonly HashSet<TriggeredEncounter> triggeredOccurances = new HashSet<TriggeredEncounter>();

		private static HashSet<RandomEncounter> GetRandomEncounters()
		{
			return new HashSet<RandomEncounter>(randomEncounters.Where(x => x.isActive && !x.isCompleted));
		}

		private static HashSet<SemiRandomEncounter> GetSemiRandomEncounters()
		{
			return new HashSet<SemiRandomEncounter>(semiRandomEncounters.Where(x => x.isActive && !x.isCompleted));
		}

		private static HashSet<TriggeredEncounter> GetTriggeredEncounters()
		{
			return new HashSet<TriggeredEncounter>(triggeredOccurances.Where(x => x.isActive && !x.isCompleted));
		}

		public static bool volcanicCragUnlocked => FrontendSessionSave.data.VolcanicCragUnlocked;

		public static int timesExploredVolcanicCrag => FrontendSessionSave.data.VolcanicCragExplorationCount;
		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.VolcanicCragUnlocked;
			protected set => FrontendSessionSave.data.VolcanicCragUnlocked = value;
		}
		public override int timesVisited
		{
			get => FrontendSessionSave.data.VolcanicCragExplorationCount;
			protected set => FrontendSessionSave.data.VolcanicCragExplorationCount = value;
		}

		static VolcanicCrag()
		{
			//generic
			randomEncounters.Add(new WalkAcrossVolcanicCrag());
			//NPCs
			randomEncounters.Add(new BehemothEncounter());
			//monsters
			
			//really might want to add in some enemies here. 

			//items
			//semi-randoms
			//triggers
			triggeredOccurances.Add(new AprilFoolsCragEncounter());
		}

		protected override SimpleDescriptor UnlockText => VolcanicCragUnlock;
	}
}

