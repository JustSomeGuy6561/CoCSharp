using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CoC.Backend.Encounters;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Backend.UI;

namespace CoC.Backend.Areas
{
	public static class Exploration
	{
		private static readonly HashSet<RandomEncounter> randomEncounters = new HashSet<RandomEncounter>();
		private static readonly Dictionary<SemiRandomEncounter,int> semiRandomEncounters = new Dictionary<SemiRandomEncounter, int>();
		private static readonly OrderedHashSet<TriggeredEncounter> allTriggers = new OrderedHashSet<TriggeredEncounter>();
		private static readonly OrderedHashSet<TriggeredEncounter> areaUnlocks = new OrderedHashSet<TriggeredEncounter>();

		private static readonly Queue<SemiRandomEncounter> badLuckPreventerScene = new Queue<SemiRandomEncounter>();
		private static readonly Queue<TriggeredEncounter> triggered = new Queue<TriggeredEncounter>();

		public static bool AddRandomEncounter(RandomEncounter encounter)
		{
			return randomEncounters.Add(encounter);
		}

#warning need to handle items that persist across playthroughs - idk how just yet.
		public static bool AddSemiRandomEncounter(SemiRandomEncounter encounter)
		{
			if (semiRandomEncounters.ContainsKey(encounter))
			{
				return false;
			}
			semiRandomEncounters.Add(encounter, 0);
			return true;
		}

		public static bool AddTriggeredEncounter(TriggeredEncounter encounter)
		{
			return allTriggers.Add(encounter);
		}

		internal static void InitializeAreas(Func<DisplayBase> GetCurrentDisplay, IEnumerable<Func<LocationBase>> locations)
		{
			foreach (var item in locations)
			{
				TriggeredEncounter trigger = new LocationUnlockEncounter(GetCurrentDisplay, item);
				areaUnlocks.Add(trigger);
			}
		}

		internal static void SetupForNewGame(IEnumerable<Action> initialExplorations)
		{
			allTriggers.Clear();
			allTriggers.UnionWith(initialExplorations.Select(x => new OneOffTrigger(x)));
			allTriggers.UnionWith(areaUnlocks);
		}

		internal static void RunArea(DisplayBase currentDisplay)
		{
			Encounter currentScene;

			foreach (var scene in allTriggers)
			{
				if (scene.isActive && scene.isTriggered())
				{
					triggered.Enqueue(scene);
				}
			}

			if (triggered.Count > 0)
			{
				currentScene = triggered.Dequeue();
			}
			//if we have no triggers, check for bad luck prevention.
			else
			{
				//set up the picker as a fallback.
				Lottery<Encounter> picker = new Lottery<Encounter>();

				//loop through the semi-randoms.
				foreach (var entry in semiRandomEncounters)
				{
					//ignore inactives.
					if (!entry.Key.isActive)
					{
						continue;
					}
					//add any new items that are still active and proc our bad luck preventer. 
					else if (entry.Value >= entry.Key.numEncountersBeforeRequiringThis && !badLuckPreventerScene.Contains(entry.Key))
					{
						badLuckPreventerScene.Enqueue(entry.Key);
					}
					//only add them to the RNG lottery if we're actually going to use the RNG lottery - that is, we don't have any bad luck preventers taking priority.
					else if (badLuckPreventerScene.Count == 0)
					{
						picker.addItem(entry.Key, entry.Key.chances);
					}
				}
				//do the first bad luck preventer if any exist.
				if (badLuckPreventerScene.Count > 0)
				{
					currentScene = badLuckPreventerScene.Dequeue();
				}
				//if none exist
				else
				{
					//finish adding the entries to the RNG lottery.
					foreach (var scene in randomEncounters)
					{
						if (scene.isActive)
						{
							picker.addItem(scene, scene.chances);
						}
					}
					//select an entry from the lottery.
					currentScene = picker.Select();
				}
			}

			//run the scene.
			currentScene.RunEncounter();

			//if the scene is now complete, remove it from the hashset/dictionary that contains it. 
			if (currentScene.isCompleted)
			{
				if (currentScene is TriggeredEncounter trigger)
				{
					allTriggers.Remove(trigger);
				}
				else if (currentScene is SemiRandomEncounter semiRandom)
				{
					semiRandomEncounters.Remove(semiRandom);
				}
				else
				{
					RandomEncounter random = (RandomEncounter)currentScene;
					randomEncounters.Remove(random);
				}
			}

			//if the current scene was semi-random, reset its bad luck preventer count. otherwise,
			//increment the bad luck preventer for each scene.
			foreach (var entry in semiRandomEncounters.Keys)
			{
				if (entry == currentScene)
				{
					semiRandomEncounters[entry] = 0;
				}
				else if (entry.isActive)
				{
					semiRandomEncounters[entry]++;
				}
			}

		}
	}
}
