using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Frontend.Encounters.HighMountain;
using CoC.Frontend.Encounters.Mountain;
using CoC.Frontend.SaveData;
using System.Collections.Generic;
using System.Linq;

namespace CoC.Frontend.Areas.Locations
{
	internal partial class HighMountain : LocationBase
	{
		private const byte UNLOCKED_AT = 1;
		public HighMountain() : base(HighMountainName, UNLOCKED_AT, GetRandomEncounters(), GetSemiRandomEncounters(), GetTriggeredEncounters()) { }

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

		public static bool highMountainUnlocked => FrontendSessionSave.data.HighMountainUnlocked;

		public static int timesExploredHighMountain => FrontendSessionSave.data.HighMountainExplorationCount;
		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.HighMountainUnlocked;
			protected set => FrontendSessionSave.data.HighMountainUnlocked = value;
		}
		public override int timesVisited
		{
			get => FrontendSessionSave.data.HighMountainExplorationCount;
			protected set => FrontendSessionSave.data.HighMountainExplorationCount = value;
		}

		static HighMountain()
		{
			//generic
			randomEncounters.Add(new HikeOnHighMountainEncounter());
			//NPCs
			randomEncounters.Add(new IzumiEncounter());
			randomEncounters.Add(new SophieMountainEncounter());
			randomEncounters.Add(new ChickenHarpyEncounter()); //i always thought this was a bunny ngl.
															   //monsters
			randomEncounters.Add(new MinotaurMobEncounter());
			randomEncounters.Add(new MinotaurEncounter());
			randomEncounters.Add(new PhoenixEncounter());
			randomEncounters.Add(new CockatriceEncounter());
			randomEncounters.Add(new BasiliskEncounter());
			randomEncounters.Add(new HarpyEncounter());
			//items
			//semi-randoms
			semiRandomEncounters.Add(new DiscoverLethiceKeep());
			semiRandomEncounters.Add(new DiscoverMinervaTower());

			//triggers
			triggeredOccurances.Add(new SnowAngelHighMountainEncounter());

		}

		protected override SimpleDescriptor UnlockText => HighMountainUnlock;
	}
}
