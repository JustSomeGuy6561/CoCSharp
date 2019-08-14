//Deepwoods.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:03 AM
using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Frontend.Encounters.Deepwoods;
using CoC.Frontend.Encounters.Forest;
using CoC.Frontend.SaveData;
using System.Collections.Generic;

namespace CoC.Frontend.Areas.Locations
{
	internal sealed partial class Deepwoods : LocationBase
	{
		private const int UNLOCK_LEVEL = 1;

		private static readonly HashSet<RandomEncounter> randomEncounters = new HashSet<RandomEncounter>();
		private static readonly HashSet<SemiRandomEncounter> semiRandomEncounters = new HashSet<SemiRandomEncounter>();
		private static readonly HashSet<TriggeredEncounter> triggeredEncounters = new HashSet<TriggeredEncounter>();

		public static bool Unlocked => FrontendSessionSave.data.DeepwoodsUnlocked;

		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.DeepwoodsUnlocked;
			protected set => FrontendSessionSave.data.DeepwoodsUnlocked = value;
		}

		static Deepwoods()
		{
			//initialize encounters here. 
			//generic
			randomEncounters.Add(new WalkInDeepwoods());
			randomEncounters.Add(new GatherWood());
			//NPCs
			randomEncounters.Add(new FaerieEncounter());
			//monsters
			randomEncounters.Add(new CorruptedGrowthEncounter());
			randomEncounters.Add(new TamaniEncounter());
			randomEncounters.Add(new DryadEncounter());
			//items
			
			//semi-random (NPCs in this case)
			semiRandomEncounters.Add(new AkbalEncounter());
			semiRandomEncounters.Add(new AikoDeepwoodsEncounter());
			//triggers
			triggeredEncounters.Add(new FeraEncounter());
			triggeredEncounters.Add(new DiscoverDeepCaveEncounter());
		}

		public Deepwoods() : base(DeepwoodsName, UNLOCK_LEVEL, randomEncounters, semiRandomEncounters, triggeredEncounters)
		{
			
		}

		public override int timesVisited
		{
			get => FrontendSessionSave.data.DeepwoodsExplorationCount;
			protected set => FrontendSessionSave.data.DeepwoodsExplorationCount = value;
		}

		protected override SimpleDescriptor UnlockText => DeepwoodsUnlock;
	}
}

