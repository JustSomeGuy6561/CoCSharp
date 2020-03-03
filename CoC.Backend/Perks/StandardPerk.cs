using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Perks
{
	//This is a 'standard' perk, or a perk that is obtained and removed based on the actions the creature chooses (or in the case of leveling, what the PC picks).
	//These perks are not lost over time, and generally are not lost automatically. these perks must be obtained manually (aka they must be granted via code, not automatically)
	public abstract class StandardPerk : PerkBase
	{
		private readonly List<IRemovableModifier> currentlyActiveModifiers = new List<IRemovableModifier>();

		//as long as a standard perk is attached to a creature, it's enabled
		private protected override bool enabled => !(sourceCreature is null);

		private protected override void OnCreate()
		{
			OnActivation();
		}

		private protected override void OnDestroy()
		{
			OnRemoval();
			foreach (var removeMe in currentlyActiveModifiers)
			{
				removeMe.RemoveModifier(this);
			}
		}

		private protected override bool AddActiveModifier<T>(PerkModifierBase<T> modifier, T value, bool overwriteExisting = false)
		{
			if (modifier.AddModifier(this, value, overwriteExisting))
			{
				currentlyActiveModifiers.Add(modifier);
				return true;
			}

			return false;
		}

		private protected override bool RemoveActiveModifier<T>(PerkModifierBase<T> modifier)
		{
			currentlyActiveModifiers.Remove(modifier);
			return modifier.RemoveModifier(this);
		}

		private protected override bool HasActiveModifier<T>(PerkModifierBase<T> modifier)
		{
			bool retVal = modifier.HasModifier(this);
			if (!retVal && currentlyActiveModifiers.Contains(modifier))
			{
				currentlyActiveModifiers.Remove(modifier);
			}
			else if (retVal && !currentlyActiveModifiers.Contains(modifier))
			{
				currentlyActiveModifiers.Add(modifier);
			}

			return retVal;
		}


		private protected override bool retainOnAscension => keepOnAscension;

		protected internal abstract bool keepOnAscension { get; }


		//called when the perk is added to the perk collection on the character. sourceCreature is guarenteed to be NOT NULL by this point.
		protected abstract void OnActivation();

		//called when the perk is removed from the perk collection. sourceCreature is guarenteed to be NOT NULL.
		//after this is called, sourceCreature WILL BE NULL. Additionally, any perks added via the AddActiveModifier function will automatically be removed.
		protected internal abstract void OnRemoval();
	}
}
