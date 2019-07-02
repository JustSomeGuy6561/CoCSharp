//LocationBase.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 4:58 PM
using CoC.Backend.Encounters;
using CoC.Backend.Tools;
using System.Collections.Generic;

namespace CoC.Backend.Areas
{
	public abstract class LocationBase : AreaBase
	{

		public readonly byte unlockLevel;

		protected readonly HashSet<RandomEncounter> randomScenes;
		protected readonly Dictionary<SemiRandomEncounter, int> semiRandomScenes;
		protected readonly HashSet<SemiRandomEncounter> semiRandomContainer;
		protected readonly HashSet<TriggeredEncounter> triggerScenes;

		protected readonly Queue<SemiRandomEncounter> badLuckPreventerScene = new Queue<SemiRandomEncounter>();
		protected readonly Queue<TriggeredEncounter> triggered = new Queue<TriggeredEncounter>();

		public LocationBase(SimpleDescriptor locationName, byte prereqLevel, HashSet<RandomEncounter> randoms, HashSet<SemiRandomEncounter> semiRandoms, HashSet<TriggeredEncounter> triggers) : base (locationName)
		{
			unlockLevel = prereqLevel;

			randomScenes = randoms;
			semiRandomContainer = semiRandoms;
			foreach (var elem in semiRandoms)
			{
				semiRandomScenes.Add(elem, 0);
			}
			triggerScenes = triggers;
		}

		public abstract int timesExplored { get; protected set; }


		internal void RollScene()
		{
			Encounter currentScene;
			//check trigger scenes, add them if not already in queue.
			foreach (var scene in triggerScenes)
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
				foreach (var entry in semiRandomScenes)
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
					foreach (var scene in randomScenes)
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
			currentScene.Run();

			//if the scene is now complete, remove it from the hashset/dictionary that contains it. 
			if (currentScene.isCompleted)
			{
				if (currentScene is TriggeredEncounter trigger)
				{
					triggerScenes.Remove(trigger);
				}
				else if (currentScene is SemiRandomEncounter semiRandom)
				{
					semiRandomScenes.Remove(semiRandom);
					semiRandomContainer.Remove(semiRandom);
				}
				else
				{
					RandomEncounter random = (RandomEncounter)currentScene;
					randomScenes.Remove(random);
				}
			}

			//if the current scene was semi-random, reset its bad luck preventer count. otherwise,
			//increment the bad luck preventer for each scene.
			foreach (var entry in semiRandomScenes.Keys)
			{
				if (entry == currentScene)
				{
					semiRandomScenes[entry] = 0;
				}
				else if (entry.isActive)
				{
					semiRandomScenes[entry]++;
				}
			}
		}

	}
}
