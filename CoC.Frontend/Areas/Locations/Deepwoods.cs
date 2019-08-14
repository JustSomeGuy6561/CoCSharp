//Deepwoods.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:03 AM
using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Frontend.SaveData;
using System.Collections.Generic;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Areas.Locations
{
	internal sealed partial class Deepwoods : LocationBase
	{
		private const int UNLOCK_LEVEL = 1;

		private static readonly HashSet<RandomEncounter> randomEncounters = new HashSet<RandomEncounter>();
		private static readonly HashSet<SemiRandomEncounter> semiRandomEncounters = new HashSet<SemiRandomEncounter>();
		private static readonly HashSet<TriggeredEncounter> triggeredEncounters = new HashSet<TriggeredEncounter>();

		public static bool Unlocked => FrontendSessionSave.data.DeepwoodsUnlocked;

		static Deepwoods()
		{
			//initialize encounters here. 
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

