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


		public abstract SimpleDescriptor obtainText { get; }

		public abstract SimpleDescriptor ShortDescription { get; }
		public abstract SimpleDescriptor FullDescription { get; }
	}
}
