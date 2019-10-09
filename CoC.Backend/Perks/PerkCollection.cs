//PerkCollection.cs
//Description:
//Author: JustSomeGuy
//6/30/2019, 6:58 PM
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.Perks
{
	public sealed class PerkCollection
	{
		//the creature that has this perk collection. technically a circular reference, but it may be useful info, idk. 
		private readonly Creature source;

		//public static ReadOnlyDictionary<Type, Func<PerkBase>> perkList => GameEngine.perkList;

		//the perks this creature has.
		private readonly Dictionary<Type, PerkBase> perks = new Dictionary<Type, PerkBase>();

		internal readonly BasePerkModifiers baseModifiers;

		internal PerkCollection(Creature sourceCreature)
		{
			source = sourceCreature ?? throw new ArgumentNullException(nameof(sourceCreature));
			baseModifiers = GameEngine.constructPerkModifier(source);
			if (baseModifiers is null)
			{
				throw new ArgumentException("BaseModifiers, set during the game Engine initialization, returned null. Ensure this is not null.");
			}
		}

		internal void InitPerks(params PerkBase[] basePerks)
		{
			if (basePerks != null)
			{
				foreach (var perk in basePerks)
				{
					AddPerk(perk);
				}
			}
		}

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
				perks.Add(type, perkBase);
				perkBase.Activate(source);
				return true;
			}
		}

		public bool AddPerk<T>() where T: PerkBase, new()
		{
			Type type = typeof(T);
			if (perks.ContainsKey(type))
			{
				return false;
			}
			else
			{
				T data = new T();
				perks.Add(type, data);
				data.Activate(source);
				return true;
			}
		}

		public bool AddOrStackPerk<T>() where T : StackablePerk, new()
		{
			Type type = typeof(T);
			if (perks.ContainsKey(type))
			{
				T data = (T)perks[type];
				return data.attemptStackIncrease();
			}
			else
			{
				T data = new T();
				perks.Add(type, data);
				data.Activate(source);
				return true;
			}
		}

		public bool AddOrStackPerk(StackablePerk perk)
		{
			Type type = perk.GetType();
			if (perks.ContainsKey(type))
			{
				StackablePerk stackable = (StackablePerk)perks[type];
				return stackable.attemptStackIncrease();
			}
			else
			{
				perks.Add(type, perk);
				perk.Activate(source);
				return true;
			}
		}

		public bool StackPerk<T>() where T : StackablePerk, new()
		{
			Type type = typeof(T);
			if (perks.ContainsKey(type))
			{
				T data = (T)perks[type];
				return data.attemptStackIncrease();
			}
			else
			{
				return false;
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
