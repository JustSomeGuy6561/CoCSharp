//Forest.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 8:26 PM
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Frontend.Encounters.Common;
using CoC.Frontend.Encounters.Forest;
using CoC.Frontend.SaveData;
using System.Collections.Generic;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Areas.Locations
{
	internal partial class Forest : LocationBase
	{
		private const byte UNLOCKED_AT = 1;
		public Forest() : base(ForestName, UNLOCKED_AT, randomEncounters, semiRandomEncounters, triggeredOccurances) { }

		private static readonly HashSet<RandomEncounter> randomEncounters = new HashSet<RandomEncounter>();
		private static readonly HashSet<SemiRandomEncounter> semiRandomEncounters = new HashSet<SemiRandomEncounter>();
		private static readonly HashSet<TriggeredEncounter> triggeredOccurances = new HashSet<TriggeredEncounter>();


		public static bool forestUnlocked => FrontendSessionSave.data.ForestUnlocked;

		public static int timesExploredForest => FrontendSessionSave.data.ForestExplorationCount;
		public override int timesExplored
		{
			get => FrontendSessionSave.data.ForestExplorationCount;
			protected set => FrontendSessionSave.data.ForestExplorationCount = value;
		}

		static Forest()
		{
			//generic
			randomEncounters.Add(new TripOnRoot());
			randomEncounters.Add(new WalkInWoods());
			randomEncounters.Add(new GatherWood());
			randomEncounters.Add(new BigJunkEncounter(BigJunkForestText));
			//NPCs
			randomEncounters.Add(new EssrayleForestEncounter());
			//monsters
			randomEncounters.Add(new BeeGirlEncounter());
			randomEncounters.Add(new CorruptedGrowthEncounter());
			randomEncounters.Add(new MimicEncounter(ForestMimic));
			randomEncounters.Add(new SuccubusEncounter());
			randomEncounters.Add(new TamaniEncounter());
			//all the imps. 
			randomEncounters.UnionWith(ImpEncounter.AllImpEncounters());
			//all the goblins.
			randomEncounters.UnionWith(GoblinEncounter.AllGoblinEncounters());
			//items

			//semi-random (NPCs in this case)
			semiRandomEncounters.Add(new JojoForestEncounter());
			semiRandomEncounters.Add(new MarbleForestEncounter());
			//triggers
			triggeredOccurances.Add(new FindDeepwoods());
		}

		public override void Unlock()
		{
			if (!forestUnlocked)
			{
				FrontendSessionSave.data.ForestUnlocked = true;
				AddOutput(ForestUnlock);
			}
		}
	}
}
