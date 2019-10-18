using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Frontend.Encounters.Common;
//using CoC.Frontend.Encounters.Lake;
using CoC.Frontend.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Frontend.Areas.Locations
{
	internal partial class Lake : LocationBase
	{
		private const byte UNLOCKED_AT = 1;
		public Lake() : base(LakeName, UNLOCKED_AT, GetRandomEncounters(), GetSemiRandomEncounters(), GetTriggeredEncounters()) { }

		private static readonly HashSet<RandomEncounter> randomEncounters = new HashSet<RandomEncounter>();
		private static readonly HashSet<SemiRandomEncounter> semiRandomEncounters = new HashSet<SemiRandomEncounter>();
		private static readonly HashSet<TriggeredEncounter> triggeredOccurances = new HashSet<TriggeredEncounter>();

		static Lake()
		{
			//randomEncounters.Add(new FetishCultistEncounter());
			//randomEncounters.Add(new AprilFoolsEncounter());

			//randomEncounters.Add(new GooCreatureEncounter());
			//all the imps. 
			randomEncounters.UnionWith(ImpEncounter.AllImpEncounters());
			//all the goblins.
			randomEncounters.UnionWith(GoblinEncounter.AllGoblinEncounters());
			randomEncounters.Add(new BigJunkEncounter(typeof(Lake)));
			//randomEncounters.Add(new WalkAlongLakeEncounter());
			//randomEncounters.Add(new KaijuEncounter());
			//randomEncounters.Add(new IzmaLakeEncounter());

			////find the foods.

			//semiRandomEncounters.Add(new RathazulLakeEncounter());
			//semiRandomEncounters.Add(new DisoverFarmEncounter());
			//semiRandomEncounters.Add(new DiscoverTownRuinsEncounter());

			//only available if pregnant.
			//semiRandomEncounters.Add(new EggPregnancyLakeEncounter());

			//find the NU-DEST BEEEEEEEEEEACH! 
			//https://www.youtube.com/watch?v=c8WZ8DDejd4
			//i can't find a clip of the dub, but i suppose both work.
			//beach replaces "Boat" as a place, as "Boat" was more like a location with all the RNG. i've moved everything in "Boat" to various places within Beach. 
			//triggeredOccurances.Add(new DiscoverBeachEncounter());
		}

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

		public static bool lakeUnlocked => FrontendSessionSave.data.LakeUnlocked;

		public override int timesVisited
		{
			get => FrontendSessionSave.data.LakeExplorationCount;
			protected set => FrontendSessionSave.data.LakeExplorationCount = value;
		}
		public override bool isUnlocked {
			get => FrontendSessionSave.data.LakeUnlocked;
			protected set => FrontendSessionSave.data.LakeUnlocked = value;
		}

		protected override SimpleDescriptor UnlockText => LakeUnlock;
	}
}
