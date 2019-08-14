using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Frontend.Encounters.Common;
using CoC.Frontend.Encounters.Bog;
using CoC.Frontend.SaveData;
using System.Collections.Generic;
using System.Linq;
using static CoC.Frontend.UI.TextOutput;
using CoC.Backend;

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
			randomEncounters.Add(new TripOnRoot());
			randomEncounters.Add(new WalkInWoods());
			randomEncounters.Add(new GatherWood());
			randomEncounters.Add(new BigJunkEncounter(typeof(Bog)));
			//NPCs
			randomEncounters.Add(new EssrayleBogEncounter());
			//monsters
			randomEncounters.Add(new BeeGirlEncounter());
			randomEncounters.Add(new CorruptedGrowthEncounter());
			randomEncounters.Add(new MimicEncounter(BogMimic));
			randomEncounters.Add(new SuccubusEncounter());
			randomEncounters.Add(new TamaniEncounter());
			//all the imps. 
			randomEncounters.UnionWith(ImpEncounter.AllImpEncounters());
			//all the goblins.
			randomEncounters.UnionWith(GoblinEncounter.AllGoblinEncounters());
			//items

			//semi-random (NPCs in this case)
			semiRandomEncounters.Add(new JojoBogEncounter());
			semiRandomEncounters.Add(new MarbleBogEncounter());
			//triggers
			triggeredOccurances.Add(new FindDeepwoods());
		}

		protected override SimpleDescriptor UnlockText => BogUnlock;
	}
}
