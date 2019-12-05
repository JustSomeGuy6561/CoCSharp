using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Frontend.Encounters.Common;
using CoC.Frontend.Encounters.Mountain;
using CoC.Frontend.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Frontend.Areas.Locations
{
	internal partial class Mountain : LocationBase
	{
		private const byte UNLOCKED_AT = 1;
		public Mountain() : base(MountainName, UNLOCKED_AT, GetRandomEncounters(), GetSemiRandomEncounters(), GetTriggeredEncounters()) { }

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

		public static bool mountainUnlocked => FrontendSessionSave.data.MountainUnlocked;

		public static int timesExploredMountain => FrontendSessionSave.data.MountainExplorationCount;
		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.MountainUnlocked;
			protected set => FrontendSessionSave.data.MountainUnlocked = value;
		}
		public override int timesVisited
		{
			get => FrontendSessionSave.data.MountainExplorationCount;
			protected set => FrontendSessionSave.data.MountainExplorationCount = value;
		}

		static Mountain()
		{
			//generic
			randomEncounters.Add(new MountainHike());
			//NPCs
			randomEncounters.Add(new CeraphMountainEncounter());
			//monsters
			randomEncounters.Add(new HellhoundEncounter());
			randomEncounters.Add(new MinotaurEncounter());
			randomEncounters.Add(new MinotaurLordEncounter());
			randomEncounters.Add(new MimicEncounter(MountainMimic));
			//all the imps.
			randomEncounters.UnionWith(ImpEncounter.AllImpEncounters());
			//all the goblins.
			randomEncounters.UnionWith(GoblinEncounter.AllGoblinEncounters());
			//items

			//semi-random
			semiRandomEncounters.Add(new DiscoverSalonEncounter());
			semiRandomEncounters.Add(new HellhoundMasterEncounter());
			semiRandomEncounters.Add(new DiscoverFactoryEncounter());
			//triggers
			triggeredOccurances.Add(new FindHighMountains());

			triggeredOccurances.Add(new JackFrontMountainEncounter());
			triggeredOccurances.Add(new SnowAngelEncounter());
		}

		protected override SimpleDescriptor UnlockText => MountainUnlock;
	}
}
