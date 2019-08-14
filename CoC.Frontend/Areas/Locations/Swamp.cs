using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Frontend.Encounters.Common;
using CoC.Frontend.Encounters.Swamp;
using CoC.Frontend.SaveData;
using System.Collections.Generic;
using System.Linq;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Areas.Locations
{
	internal partial class Swamp : LocationBase
	{
		private const byte UNLOCKED_AT = 1;
		public Swamp() : base(SwampName, UNLOCKED_AT, GetRandomEncounters(), GetSemiRandomEncounters(), GetTriggeredEncounters()) { }

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

		public static bool swampUnlocked => FrontendSessionSave.data.SwampUnlocked;

		public static int timesExploredSwamp => FrontendSessionSave.data.SwampExplorationCount;
		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.SwampUnlocked;
			protected set => FrontendSessionSave.data.SwampUnlocked = value;
		}
		public override int timesVisited
		{
			get => FrontendSessionSave.data.SwampExplorationCount;
			protected set => FrontendSessionSave.data.SwampExplorationCount = value;
		}

		static Swamp()
		{
			//generic
			randomEncounters.Add(new WalkThroughSwampEncounter());
			//NPCs
			randomEncounters.Add(new RogarSwampEncounter());
			randomEncounters.Add(new KihaXHeliaSmashEncounter());
			//monsters
			randomEncounters.Add(new FemaleSpiderEncounter());
			randomEncounters.Add(new MaleSpiderEncounter());
			randomEncounters.Add(new DriderEncounter());
			//items
			//semi-randoms
			semiRandomEncounters.Add(new KihaSwampEncounter());
			semiRandomEncounters.Add(new FindEmberEgg());
			//triggers
			triggeredOccurances.Add(new DiscoverBog());
		}

		protected override SimpleDescriptor UnlockText => SwampUnlock;
	}
}
