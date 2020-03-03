//PerkBase.cs
//Description:
//Author: JustSomeGuy
//6/30/2019, 6:57 PM
using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Perks
{
	//each perk is to be declared as its own class. we use class type to determine if you have a perk or not, not instances. we do this because Perks are NOT static, and we need an easy
	//way to ask if the character has a perk without needing to allocate a new instance. This allows perks to do anything, and have any variables and helper functions they need.
	//get perk will return the Type you give it, so you can then use these helper functions or update these variables without needing to cast or do typechecking.
	//one downside is we don't need a "PerkLib" anymore, which means perks can come from anywhere. This makes it far more flexible, but also means they can be anywhere (more difficult to debug)
	//Also, while the game is open source and anyone can build their own version with a god mode or whatever they like, it tends to be complicated and thus dissuade this type of behavior.
	//A downside to this approach is it's really easy to add perks now, because there's no validation layer that PerkLib provided.

	public abstract class PerkBase
	{

		//the creature that has this perk. Note that this is NULL until the perk is activated by the creature.
		protected Creature sourceCreature { get; private set; } = null;
		//Giving you these means you have access to all the perks the creature currently has, and the base stat multipliers. They should only be used in this context; don't misuse this.
		protected PerkCollection basicData => sourceCreature.perks; //gives you access to hasPerk. allows you to hard-code other checks, to prevent mutual exclusives.
		protected BasePerkModifiers baseModifiers => basicData.baseModifiers; //allows you to update base stats.

		public bool isEnabled => enabled;

		public PerkBase() {}

		#region Non-Public abstracts.

		private protected abstract bool enabled { get; }


		private protected abstract bool AddActiveModifier<T>(PerkModifierBase<T> modifier, T value, bool overwriteExisting = false);

		private protected abstract bool RemoveActiveModifier<T>(PerkModifierBase<T> modifier);

		private protected abstract bool HasActiveModifier<T>(PerkModifierBase<T> modifier);

		//called when the perk is initialized. When this occurs varies by perk type.
		private protected abstract void OnCreate();

		//called when the perk is to be removed. called just before all references to this perk are lost and the perk is to be collected by garbage collection.
		//When this occurs varies by perk type.
		private protected abstract void OnDestroy();

		private protected abstract bool retainOnAscension { get; }
		#endregion

		#region Public Abstracts

		public abstract string Name();

		public abstract string HasPerkText();

		//To help make various perks feel like "Status Effects," we've added an ailment boolean flag. This flag allows you to designate a perk as 'harmful' or 'inconvenient' or
		//whatever - there is no exact definition of what makes it an ailment, so use your best judgement. Note that this will have little use beyond sorting how we display perks -
		//this will either affect display order, or cause additional text (probably "- Ailment") alongside the perk name.
		public virtual bool isAilment => false;

		#endregion

		#region Backend-only Functions
		internal void Activate(Creature source)
		{
			sourceCreature = source;
			OnCreate();
		}

		internal void Deactivate()
		{
			OnDestroy();
			sourceCreature = null;
		}
		#endregion

		#region PerkBase-Derived only functions
		//for most perks that adust a modifier, you can simply say: when this is active, apply this modifier. when it's not, remove it.
		//To make this more convenient, we've provided a means to do this automatically. You can also manually remove one of these if a perk dynamically
		//changes, or update them to new values if your perk does that, too, or check if it exists if needed.
		//To use these: put them in your code responsible for activating the perk.



		//Like all variables, be aware that any changes to these will only exist for the lifetime of the perk, and that different types of perks have different lifespans
		//for example, conditional perks have a lifespan as long as their parent creature - they are simply inactive when the creature doesn't mean the conditions for that
		//perk. Standard perks and timed perks (aka status effects), meanwhile, only live as long as the creature has them - they are destroyed when the creature loses that
		//perk.
		//Thus, for any standard/timed perks, if you want an adjusted value on all subsequent times that perk is obtained, you'll need to store that data somewhere it will
		//persist, (likely the extended creature data section) then reload it during construction. Meanwhile, if you only want a temporary effect on a conditional perk,
		//handle this whenever you handle the enable and disable related checks.

		protected bool AddModifierToPerk<T>(PerkModifierBase<T> modifier, T value, bool overwriteExisting = false)
		{
			return AddActiveModifier(modifier, value, overwriteExisting);
		}

		protected bool UpdatePerkModifier<T>(PerkModifierBase<T> modifier, T value)
		{
			return AddActiveModifier(modifier, value, true);
		}

		protected bool RemoveModifierFromPerk<T>(PerkModifierBase<T> modifier)
		{
			return RemoveActiveModifier<T>(modifier);
		}

		protected bool HasModifier<T>(PerkModifierBase<T> modifier)
		{
			return HasActiveModifier<T>(modifier);
		}
		#endregion
	}
}
