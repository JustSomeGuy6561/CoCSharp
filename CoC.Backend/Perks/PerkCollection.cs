using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Perks
{
	public sealed class PerkCollection
	{
		//the creature that has this perk collection. technically a circular reference, but it may be useful info, idk. 
		private readonly CombatCreature source;
		//the perks this creature has.
		private readonly Dictionary<Type, PerkBase> perks = new Dictionary<Type, PerkBase>();

		internal readonly PassiveStatModifiers baseModifiers;

		//adds a new perk to the collection. returns false if the character already has an instance of this perk type.
		public bool AddPerk(PerkBase perkBase)
		{
			Type type = perkBase.GetType();
			if (perks.ContainsKey(type))
			{
				return false;
			}
			else
			{
				perkBase.Activate(source);
				perks.Add(type, perkBase);
				return true;
			}
		}

		//checks to see if an instance of this perk type exists in the collection
		public bool HasPerk<T>() where T : PerkBase
		{
			if (typeof(T) == typeof(PerkBase))
			{
				return false;
			}
			else return perks.ContainsKey(typeof(T));
		}

		//checks to see if an instance of this perk type exists in the collection
		public bool HasAnyPerk()
		{
			return perks.Count > 0;
		}

		//how many perks currently available. 
		public int numPerks => perks.Count;

		//removes perk of this type if the character has it. if they didn't have it to begin with, returns false. otherwise removes it and returns true.
		public bool RemovePerk<T>() where T : PerkBase
		{
			Type type = typeof(T);
			if (type == typeof(PerkBase))
			{
				return false;
			}
			if (perks.ContainsKey(type))
			{
				T removed = (T)perks[type];
				removed.Deactivate();
				return perks.Remove(type);
			}
			return false;
		}

		//retrieves a perk, so you can update it. 
		public T GetPerk<T>() where T : PerkBase
		{
			Type type = typeof(T);
			if (type == typeof(PerkBase))
			{
				return null;
			}
			if (perks.ContainsKey(type))
			{
				return (T)perks[type];
			}
			return null;
		}
	}
}
