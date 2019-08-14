using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Frontend.Encounters.Bog;
using CoC.Frontend.SaveData;
using System.Collections.Generic;
using System.Linq;

namespace CoC.Frontend.Areas.Locations
{
	internal partial class Bog : LocationBase
	{
		private const byte UNLOCKED_AT = 1;
		public Bog() : base(BogName, UNLOCKED_AT, GetRandomEncounters(), GetSemiRandomEncounters(), GetTriggeredEncounters()) { }

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

		public static bool bogUnlocked => FrontendSessionSave.data.BogUnlocked;

		public static int timesExploredBog => FrontendSessionSave.data.BogExplorationCount;
		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.BogUnlocked;
			protected set => FrontendSessionSave.data.BogUnlocked = value;
		}
		public override int timesVisited
		{
			get => FrontendSessionSave.data.BogExplorationCount;
			protected set => FrontendSessionSave.data.BogExplorationCount = value;
		}

		static Bog()
		{
			//generic
			randomEncounters.Add(new WalkThroughBogEncounter());
			//NPCs
			randomEncounters.Add(new LizanRogueEncounter());
			//monsters
			randomEncounters.Add(new FrogGirlEncounter());
			randomEncounters.Add(new PhoukaEncounter());
			randomEncounters.Add(item: new ChameleonGirlEncounter());
			//items
			//semi-randoms
			semiRandomEncounters.Add(new MurkyChestEncounter());
			//triggers
			triggeredOccurances.Add(new HalloweenPhoukaEncounter());
		}

		protected override SimpleDescriptor UnlockText => BogUnlock;
	}
}
