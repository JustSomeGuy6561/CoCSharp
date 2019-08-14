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
			randomEncounters.Add(new TripOnRoot());
			randomEncounters.Add(new WalkInWoods());
			randomEncounters.Add(new GatherWood());
			randomEncounters.Add(new BigJunkEncounter(typeof(Plains)));
			//NPCs
			randomEncounters.Add(new EssraylePlainsEncounter());
			//monsters
			randomEncounters.Add(new BeeGirlEncounter());
			randomEncounters.Add(new CorruptedGrowthEncounter());
			randomEncounters.Add(new MimicEncounter(PlainsMimic));
			randomEncounters.Add(new SuccubusEncounter());
			randomEncounters.Add(new TamaniEncounter());
			//all the imps. 
			randomEncounters.UnionWith(ImpEncounter.AllImpEncounters());
			//all the goblins.
			randomEncounters.UnionWith(GoblinEncounter.AllGoblinEncounters());
			//items

			//semi-random (NPCs in this case)
			semiRandomEncounters.Add(new JojoPlainsEncounter());
			semiRandomEncounters.Add(new MarblePlainsEncounter());
			//triggers
			triggeredOccurances.Add(new FindDeepwoods());
		}

		protected override SimpleDescriptor UnlockText => PlainsUnlock;
	}
}
