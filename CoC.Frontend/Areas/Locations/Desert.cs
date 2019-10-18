using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Encounters;
using CoC.Frontend.Encounters.Common;
using CoC.Frontend.Encounters.Desert;
using CoC.Frontend.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Frontend.Areas.Locations
{
	internal partial class Desert : LocationBase
	{
		private const byte UNLOCKED_AT = 1;
		public Desert() : base(DesertName, UNLOCKED_AT, GetRandomEncounters(), GetSemiRandomEncounters(), GetTriggeredEncounters()) { }

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

		public static bool desertUnlocked => FrontendSessionSave.data.DesertUnlocked;

		public static int timesExploredForest => FrontendSessionSave.data.DesertExplorationCount;
		public override bool isUnlocked
		{
			get => FrontendSessionSave.data.DesertUnlocked;
			protected set => FrontendSessionSave.data.DesertUnlocked = value;
		}
		public override int timesVisited
		{
			get => FrontendSessionSave.data.DesertExplorationCount;
			protected set => FrontendSessionSave.data.DesertExplorationCount = value;
		}

		static Desert()
		{
			randomEncounters.Add(new NagaEncounter());
			randomEncounters.Add(new SandTrapEncounter());
			randomEncounters.Add(new GhouldEncounter());
			randomEncounters.Add(new SandWitchEncounter());
			randomEncounters.Add(new CumWitchEncounter());
			randomEncounters.Add(new FindNailsEncounter());
			randomEncounters.Add(new MimicEncounter(DesertMimicText));
			randomEncounters.Add(new BigJunkEncounter(typeof(Desert)));
			randomEncounters.Add(new MirageEncounter());
			randomEncounters.Add(new SuccubusWandererEncounter());
			randomEncounters.Add(new DesertWalkEncounter());
			randomEncounters.Add(new DemonPackEncounter());

			semiRandomEncounters.Add(new DiscoverTelAdreEncounter());
			semiRandomEncounters.Add(new DoAntColonyEncounter()); //demon pack on first encounter - it's weird. 
			semiRandomEncounters.Add(new FindDesertDungeonEncounter());
			semiRandomEncounters.Add(new FindChestEncounter());
			semiRandomEncounters.Add(new FindFountainEncounter());

			triggeredOccurances.Add(new PregnantSandWitchEncounter());

		}

		protected override SimpleDescriptor UnlockText => DesertUnlock;


	}
}
/*
 		public function get desertEncounter():Encounter { //late init because it references getGame()
			const game:CoC = getGame();
			const fn:FnHelpers = Encounters.fn;
			if (_desertEncounter == null) _desertEncounter =
					{
						name: "wstaff",
						when: function ():Boolean {
							return flags[kFLAGS.FOUND_WIZARD_STAFF] == 0 && player.inte100 > 50;
						},
						call: wstaffEncounter
					}, {
						name: "walk",
						call: walkingDesertStatBoost
					});
			return _desertEncounter;
		}

 */
