using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Engine
{
	public static class CreatureStore
	{
		private static readonly Dictionary<Guid, Creatures.Creature> creatureLookup = new Dictionary<Guid, Creatures.Creature>()
		{
			[Guid.Empty] = null,
		};

		internal static Guid GenerateCreature(Creatures.Creature creature)
		{
			if (creatureLookup.ContainsValue(creature))
			{
				foreach (var pair in creatureLookup)
				{
					if (pair.Value == creature)
					{
						return pair.Key;
					}
				}
			}
			Guid id;
			do
			{
				id = Guid.NewGuid();

			}
			while (creatureLookup.ContainsKey(id));

			creatureLookup.Add(id, creature);
			return id;
		}

		public static Creatures.Creature GetCreature(Guid guid)
		{
			return creatureLookup[guid];
		}

		public static bool TryGetCreature(Guid guid, out Creatures.Creature creature)
		{
			return creatureLookup.TryGetValue(guid, out creature);
		}
	}
}
