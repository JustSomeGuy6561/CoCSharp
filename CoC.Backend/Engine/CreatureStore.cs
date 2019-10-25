using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoC.Backend.Engine
{
	public static class CreatureStore
	{
		private static readonly Dictionary<Guid, WeakReference<Creature>> creatureLookup = new Dictionary<Guid, WeakReference<Creature>>()
		{
			[Guid.Empty] = new WeakReference<Creature>(null),
		};

		//artificially keeps creatures alive with a strong reference so they can participate in the time engine. 
		private static readonly Dictionary<Guid, Creature> activeCreatures = new Dictionary<Guid, Creature>();

		internal static IEnumerable<Creature> activeCreatureList => activeCreatures.Values;

		public static PlayerBase currentControlledCharacter { get; private set; } = null;
		public static PlayerBase activePlayer { get; private set; } = null;


		internal static Guid GenerateCreature(Creatures.Creature creature)
		{
			if (creature is null) return Guid.Empty;

			List<Guid> deleteMe = new List<Guid>();
			Guid retVal = Guid.Empty;

			foreach (var pair in creatureLookup)
			{
				if (pair.Value.TryGetTarget(out Creature value) == true)
				{
					if (value == creature)
					{
						retVal = pair.Key;
						break;
					}
				}
				else
				{
					deleteMe.Add(pair.Key);
				}
			}
			foreach (var deleted in deleteMe)
			{
				creatureLookup.Remove(deleted);
			}
			if (retVal != Guid.Empty)
			{
				return retVal;
			}


			do
			{
				retVal = Guid.NewGuid();

			}
			while (creatureLookup.ContainsKey(retVal));

			creatureLookup.Add(retVal, new WeakReference<Creature>(creature));
			return retVal;
		}

		public static bool TryGetCreature(Guid guid, out Creatures.Creature creature)
		{
			if (creatureLookup.TryGetValue(guid, out WeakReference<Creature> weak))
			{
				return weak.TryGetTarget(out creature);
			}
			else
			{
				creature = null;
				return false;
			}
		}

		public static Creature GetCreatureClean(Guid guid)
		{
			TryGetCreature(guid, out Creature creature);
			return creature;
		}

		internal static void SetActivePlayerCharacter(PlayerBase player)
		{
			if (player != null)
			{
				CheckPlayerInStore(player);
			}
			activePlayer = player;
			currentControlledCharacter = player;
		}

		public static void TakeControlOfQuestCharacter(PlayerBase questCharacter)
		{
			CheckPlayerInStore(questCharacter);

			currentControlledCharacter = questCharacter;
		}

		public static void ReturnControlToPlayerCharacter()
		{
			if (activePlayer != currentControlledCharacter)
			{
				currentControlledCharacter = activePlayer;
			}
		}

		private static void CheckPlayerInStore(PlayerBase pc)
		{
			if (!creatureLookup.ContainsKey(pc.id))
			{
				creatureLookup.Add(pc.id, new WeakReference<Creature>(pc));
			}
			else if (!creatureLookup[pc.id].TryGetTarget(out Creature creature) || creature != pc)
			{
#if DEBUG
				Debug.WriteLine("Creature provided does not match the creature in creature store with the same id. This may be caused by a bad reload.");
#endif
				//overwrite the weak reference to use the provided value.
				creatureLookup[pc.id].SetTarget(pc);
			}
		}

		internal static void markInactive(Guid id)
		{
			activeCreatures.Remove(id);
		}

		internal static void markActive(Guid id, Creature creature)
		{
			if (!creatureLookup.ContainsKey(id))
			{
				creatureLookup.Add(id, new WeakReference<Creature>(creature));
			}
			else
			{
				creatureLookup[id].TryGetTarget(out Creature stored);
				if (stored != creature)
				{
					creatureLookup[id].SetTarget(creature);
				}
			}
			if (!activeCreatures.ContainsKey(id))
			{
				activeCreatures.Add(id, creature);
			}
			else if (activeCreatures[id] != creature)
			{
				activeCreatures[id] = creature;
			}
		}
	}
}
