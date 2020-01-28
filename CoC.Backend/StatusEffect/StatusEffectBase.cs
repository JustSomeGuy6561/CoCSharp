using CoC.Backend.Creatures;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.StatusEffect
{
	public abstract class StatusEffectBase
	{
		public readonly SimpleDescriptor effectName;
		protected Creature sourceCreature { get; private set; } = null;

		protected StatusEffectCollection sourceCollection => sourceCreature?.statusEffects;
		protected BasePerkModifiers perkModifiers => sourceCollection?.baseModifiers;

		private protected StatusEffectBase(SimpleDescriptor name)
		{
			effectName = name ?? throw new ArgumentNullException(nameof(name));
			if (string.IsNullOrWhiteSpace(effectName())) throw new ArgumentException("the effect name must be valid");
		}

		internal void Activate(Creature source)
		{
			sourceCreature = source;
			OnActivation();
		}

		internal void Deactivate()
		{
			OnRemoval();
			sourceCreature = null;
		}

		//called when the status effect is added to the status effect collection on the character.
		protected abstract void OnActivation();

		//called when the status effect is removed from the status effect collection.
		protected abstract void OnRemoval();

		//optionally called when you obtain the status effect. this provides a generic text that things that cause this status effect can use to describe what happened.
		//of course, they may decide to roll out their own text, but this is necessary in case they don't want to and just want a nice default.
		public abstract string ObtainText();

		//called when the player wants to know what status effects they have and what it entails.
		public abstract string HaveStatusEffectText();
	}
}
