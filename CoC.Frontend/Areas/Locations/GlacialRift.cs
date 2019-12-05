using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Frontend.Encounters.Common;
using CoC.Frontend.Encounters.GlacialRift;
using CoC.Frontend.SaveData;
using System.Collections.Generic;
using System.Linq;

namespace CoC.Frontend.Areas.Locations
{
	internal partial class GlacialRift : LocationBase
	{
		private const byte UNLOCKED_AT = 1;
		public GlacialRift() : base(GlacialRiftName, UNLOCKED_AT, GetRandomEncounters(), GetSemiRandomEncounters(), GetTriggeredEncounters()) { }

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

		public static bool glacialRiftUnlocked => FrontendSessionSave.data.GlacialRiftUnlocked;

		public static int timesExploredGlacialRift => FrontendSessionSave.data.GlacialRiftExplorationCount;
		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.GlacialRiftUnlocked;
			protected set => FrontendSessionSave.data.GlacialRiftUnlocked = value;
		}
		public override int timesVisited
		{
			get => FrontendSessionSave.data.GlacialRiftExplorationCount;
			protected set => FrontendSessionSave.data.GlacialRiftExplorationCount = value;
		}

		static GlacialRift()
		{
			//generic
			randomEncounters.Add(new WalkThroughGlacialRiftEncounter());
			//NPCs
			randomEncounters.Add(new ValeriaAtGlacialRiftEncounter()); //technically you fight her but whatever.

			//monsters
			randomEncounters.Add(new ValkyrieEncounter());
			randomEncounters.Add(new FrostGiantEncounter());
			randomEncounters.Add(new YetiEncounter());
			randomEncounters.Add(new AmarokEncounter());
			//items

			//find godMead;
			//find Icicle;

			//semi-randoms
			semiRandomEncounters.Add(new DiscoverAnzuPalaceEncounter());
			semiRandomEncounters.Add(new FindOrnateChestEncounter());
			//triggers
			triggeredOccurances.Add(new AprilFoolsGlacialRiftEncounter());


		}

		protected override SimpleDescriptor UnlockText => GlacialRiftUnlock;
	}
}

