//PerkCollection.cs
//Description:
//Author: JustSomeGuy
//6/30/2019, 6:58 PM
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace CoC.Backend.Perks
{
	public sealed class PerkCollection
	{
		//the creature that has this perk collection. technically a circular reference, but it may be useful info, idk.
		private readonly Creature source;

		//public static ReadOnlyDictionary<Type, Func<PerkBase>> perkList => GameEngine.perkList;

		//the perks this creature has.
		private readonly Dictionary<Type, PerkBase> perks = new Dictionary<Type, PerkBase>();

		private readonly HashSet<ConditionalPerk> conditionalPerks;

		internal readonly BasePerkModifiers baseModifiers;

		internal PerkCollection(Creature sourceCreature, ConditionalPerk[] conditionalPerks)
		{
			source = sourceCreature ?? throw new ArgumentNullException(nameof(sourceCreature));
			if (conditionalPerks is null)
			{
				conditionalPerks = new ConditionalPerk[0];
			}

			this.conditionalPerks = new HashSet<ConditionalPerk>(conditionalPerks);

			baseModifiers = new BasePerkModifiers(sourceCreature);


		}

		internal void InitPerks(StandardPerk[] basePerks)
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
		public bool AddPerk(StandardPerk perkBase)
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

		public bool AddPerk<T>() where T: StandardPerk, new()
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
				return data.AttemptStackIncrease();
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
				return stackable.AttemptStackIncrease();
			}
			else
			{
				perks.Add(type, perk);
				perk.Activate(source);
				return true;
			}
		}

		public bool StackPerk<T>() where T : StackablePerk
		{
			Type type = typeof(T);
			if (perks.ContainsKey(type))
			{
				T data = (T)perks[type];
				return data.AttemptStackIncrease();
			}
			else
			{
				return false;
			}
		}

		//checks to see if an instance of this perk type exists in the collection
		public bool HasPerk<T>() where T : StandardPerk
		{
			if (typeof(T) == typeof(PerkBase))
			{
				return false;
			}
			else return perks.ContainsKey(typeof(T)) && perks[typeof(T)].isEnabled;
		}

		//checks to see if an instance of this perk type exists in the collection
		public bool HasAnyPerk()
		{
			return perks.Values.Any(x => x.isEnabled);
		}

		public bool AddTimedEffect(TimedPerk timedPerk)
		{
			Type type = timedPerk.GetType();
			if (perks.ContainsKey(type))
			{
				return false;
			}
			else
			{
				perks.Add(type, timedPerk);
				timedPerk.Activate(source);
				return true;
			}
		}

		public bool AddTimedEffect<T>() where T : TimedPerk, new()
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

		//how many perks currently available.
		public int numPerks => perks.Count;

		//checks to see if an instance of this perk type exists in the collection
		public bool HasTimedEffect<T>() where T : TimedPerk
		{
			if (typeof(T) == typeof(PerkBase))
			{
				return false;
			}
			else return perks.ContainsKey(typeof(T)) && perks[typeof(T)].isEnabled;
		}

		public bool ConditionalPerkActive<T>() where T : ConditionalPerk
		{
			if (typeof(T) == typeof(ConditionalPerk))
			{
				return false;
			}
			return perks.ContainsKey(typeof(T)) && perks[typeof(T)].isEnabled;
		}

		//removes perk of this type if the character has it. if they didn't have it to begin with, returns false. otherwise removes it and returns true.
		public bool RemovePerk<T>() where T : StandardPerk
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


		public bool RemovePerk(Type type)
		{
			if (type == typeof(PerkBase) || type == typeof(ConditionalPerk) || type.IsSubclassOf(typeof(ConditionalPerk)))
			{
				return false;
			}
			if (perks.ContainsKey(type))
			{
				PerkBase removed = perks[type];
				removed.Deactivate();
				return perks.Remove(type);
			}
			return false;
		}

		public bool RemovePerk(StandardPerk effect)
		{

			if (perks.ContainsKey(effect.GetType()) && ReferenceEquals(perks[effect.GetType()], effect))
			{
				effect.Deactivate();
				return perks.Remove(effect.GetType());
			}
			return false;
		}

		public bool RemoveTimedEffect(TimedPerk effect)
		{

			if (perks.ContainsKey(effect.GetType()) && ReferenceEquals(perks[effect.GetType()], effect))
			{
				effect.Deactivate();
				return perks.Remove(effect.GetType());
			}
			return false;
		}

		//retrieves a timed perk, so you can update it.
		public T GetPerkData<T>() where T : StandardPerk
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

		public bool RemoveTimedEffect<T>() where T : TimedPerk
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

		//retrieves a timed perk, so you can update it.
		public T GetTimedEffectData<T>() where T : TimedPerk
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

		//retrives a conditional perk, so you can update it. note that by default, this does not return conditional perks that aren't currently active.
		public T GetConditionalPerkData<T>(bool ignoreIfInactive = true) where T : ConditionalPerk
		{
			Type type = typeof(T);
			if (type == typeof(PerkBase))
			{
				return null;
			}
			if (perks.ContainsKey(type) && (!ignoreIfInactive || perks[type].isEnabled))
			{
				return (T)perks[type];
			}
			return null;
		}
	}
}
