using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Perks
{
	//A conditional perk is different than a standard perk in that it is always 'active', but the creature may not have it, anyway.
	//A creature has a conditional perk when the requirements for that perk are met, and loses it when those conditions are no longer met.
	//To do this, it always remains active, and listens to see if whatever trigger condition has procced. Its OnCreate and OnDestroy are different than
	//the standard perk variant as it's assumed that these will only be created or destroyed when the creature they are attached to (via perk collection)
	//is also being created or destroyed. This fundamental difference in design is why they are coded differently.

	//Additionally, it's more or less assumed this will not set anything, but simply report to the player that certain conditions are met, so they have a 'perk'
	//however, if you need it to set things, that's ok, however, you will need to add and remove it from the perk modifier values whenever it is enabled or disabled,
	//respectively - we can't handle that for you.
	public abstract class ConditionalPerk : PerkBase
	{
		private readonly Dictionary<object, PerkHelper> handleActiveModifiers = new Dictionary<object, PerkHelper>();

		protected bool currentlyEnabled
		{
			get => _currentlyEnabled;
			set
			{
				if (_currentlyEnabled != value)
				{
					_currentlyEnabled = value;
					if (_currentlyEnabled)
					{
						foreach (var helper in handleActiveModifiers.Values)
						{
							helper.onRemove();
						}
					}
					else
					{
						foreach (var helper in handleActiveModifiers.Values)
						{
							helper.onAdd();
						}
					}
				}
			}
		}
		private bool _currentlyEnabled = false;

		protected ConditionalPerk()
		{
		}

		private protected override bool enabled => currentlyEnabled;

		private protected override void OnCreate()
		{
			SetupActivationConditions();
		}

		private protected override void OnDestroy()
		{
			RemoveActivationConditions();
		}

		private protected override bool retainOnAscension => false;

		private protected override bool AddActiveModifier<T>(PerkModifierBase<T> modifier, T value, bool overwriteExisting = false)
		{
			if (handleActiveModifiers.ContainsKey(modifier) && !overwriteExisting)
			{
				return false;
			}
			else
			{
				handleActiveModifiers[modifier] = new PerkHelper(() => modifier.AddModifier(this, value, overwriteExisting), () => modifier.RemoveModifier(this));
				return true;
			}
		}

		private protected override bool RemoveActiveModifier<T>(PerkModifierBase<T> modifier)
		{
			return handleActiveModifiers.Remove(modifier);
		}

		private protected override bool HasActiveModifier<T>(PerkModifierBase<T> modifier)
		{
			return handleActiveModifiers.ContainsKey(modifier);
		}


		//this realistically could be done in the constructor, but this more or less forces a user to do it.
		protected abstract void SetupActivationConditions();
		protected abstract void RemoveActivationConditions();

		private class PerkHelper
		{
			public readonly Func<bool> onAdd;
			public readonly Func<bool> onRemove;

			public PerkHelper(Func<bool> onAdd, Func<bool> onRemove)
			{
				this.onAdd = onAdd ?? throw new ArgumentNullException(nameof(onAdd));
				this.onRemove = onRemove ?? throw new ArgumentNullException(nameof(onRemove));
			}
		}
	}
}
