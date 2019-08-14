using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Frontend.Encounters.Common;
using CoC.Frontend.Encounters.Plains;
using CoC.Frontend.SaveData;
using System.Collections.Generic;
using System.Linq;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Areas.Locations
{
	internal partial class Plains : LocationBase
	{
		private const byte UNLOCKED_AT = 1;
		public Plains() : base(PlainsName, UNLOCKED_AT, GetRandomEncounters(), GetSemiRandomEncounters(), GetTriggeredEncounters()) { }

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

		public static bool plainsUnlocked => FrontendSessionSave.data.PlainsUnlocked;

		public static int timesExploredPlains => FrontendSessionSave.data.PlainsExplorationCount;
		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.PlainsUnlocked;
			protected set => FrontendSessionSave.data.PlainsUnlocked = value;
		}
		public override int timesVisited
		{
			get => FrontendSessionSave.data.PlainsExplorationCount;
			protected set => FrontendSessionSave.data.PlainsExplorationCount = value;
		}

		static Plains()
		{
			//generic
			//NPCs
			randomEncounters.Add(new BunnyEncounter());
			randomEncounters.Add(new HelXIzzySmashEncounter());
			//monsters
			randomEncounters.Add(new GnollEncounter());
			randomEncounters.Add(new GnollSpearThrowerEncounter());
			randomEncounters.Add(new SatyrEncounter());
			//items
			//semi-randoms
			semiRandomEncounters.Add(new IsabellaPlainsEncounter());
			semiRandomEncounters.Add(new HeliaEncounter());
			semiRandomEncounters.Add(new SheilaEncounter());
			semiRandomEncounters.Add(new TaintedNiamhPlainsEncounter());
			semiRandomEncounters.Add(new FindBazaarEncounter()); //weird, because you can discover it and not enter it b/c corruption. so find. idk.
			semiRandomEncounters.Add(new DiscoverOwca());
			semiRandomEncounters.Add(new PolarPeteEncounter());
			semiRandomEncounters.Add(new CandyCaneEncounter());
			//triggers
			triggeredOccurances.Add(new SheilaCaughtOffGuardEncounter());
		}

		protected override SimpleDescriptor UnlockText => PlainsUnlock;
	}
}
