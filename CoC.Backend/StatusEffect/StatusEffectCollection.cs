﻿using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.StatusEffect
{
	public sealed class StatusEffectCollection
	{
		private readonly Creature source;

		private readonly Dictionary<Type, StatusEffectBase> statusEffects = new Dictionary<Type, StatusEffectBase>();

		public StatusEffectCollection(Creature creature)
		{
			source = creature ?? throw new ArgumentNullException(nameof(creature));
		}

		internal void InitStatusEffects(params StatusEffectBase[] baseStatusEffects)
		{
			if (baseStatusEffects != null)
			{
				foreach (var statusEffect in baseStatusEffects)
				{
					AddStatusEffect(statusEffect);
				}
			}
		}

		//adds a new statusEffect to the collection. returns false if the character already has an instance of this statusEffect type.
		public bool AddStatusEffect(StatusEffectBase statusEffectBase)
		{
			Type type = statusEffectBase.GetType();
			if (statusEffects.ContainsKey(type))
			{
				return false;
			}
			else
			{
				statusEffects.Add(type, statusEffectBase);
				statusEffectBase.Activate(source);
				return true;
			}
		}

		public bool AddStatusEffect<T>() where T : StatusEffectBase, new()
		{
			Type type = typeof(T);
			if (statusEffects.ContainsKey(type))
			{
				return false;
			}
			else
			{
				T data = new T();
				statusEffects.Add(type, data);
				data.Activate(source);
				return true;
			}
		}

		//checks to see if an instance of this statusEffect type exists in the collection
		public bool HasStatusEffect<T>() where T : StatusEffectBase
		{
			if (typeof(T) == typeof(StatusEffectBase))
			{
				return false;
			}
			else return statusEffects.ContainsKey(typeof(T));
		}

		//checks to see if an instance of this statusEffect type exists in the collection
		public bool HasAnyStatusEffect()
		{
			return statusEffects.Count > 0;
		}

		//how many statusEffects currently available. 
		public int numStatusEffects => statusEffects.Count;

		//removes statusEffect of this type if the character has it. if they didn't have it to begin with, returns false. otherwise removes it and returns true.
		public bool RemoveStatusEffect<T>() where T : StatusEffectBase
		{
			Type type = typeof(T);
			if (type == typeof(StatusEffectBase))
			{
				return false;
			}
			if (statusEffects.ContainsKey(type))
			{
				T removed = (T)statusEffects[type];
				removed.Deactivate();
				return statusEffects.Remove(type);
			}
			return false;
		}

		//retrieves a statusEffect, so you can update it. 
		public T GetStatusEffect<T>() where T : StatusEffectBase
		{
			Type type = typeof(T);
			if (type == typeof(StatusEffectBase))
			{
				return null;
			}
			if (statusEffects.ContainsKey(type))
			{
				return (T)statusEffects[type];
			}
			return null;
		}
	}
}