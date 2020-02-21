using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;
using CoC.Backend.Creatures;
using CoC.Backend.Perks;
using CoC.Backend.Tools;

namespace CoC.Frontend.StatusEffect
{
	internal class NiamhDrunk : TimedPerk
	{
		private const byte TIMER = 8;
		//drops intelligence, speed by 5, +1 per repeat dose while still drunk.
		//you retrieve 90% of this drop when it wears off.
		private float deltaIntelligence, deltaSpeed, deltaLibido;

		//drops combat damage by 25%. reverts when wears off.
		private float deltaCombatModifier;

		public NiamhDrunk() : base(TIMER)
		{
		}

		public override string Name()
		{
			return "Drunk (Black Cat Beer)";
		}

		protected override void OnActivation()
		{
			if (sourceCreature is CombatCreature combatCreature)
			{
				float oldModifier = baseModifiers.combatDamageModifier;
				baseModifiers.combatDamageModifier *= .75f;
				deltaCombatModifier = baseModifiers.combatDamageModifier / oldModifier;
			}
			deltaIntelligence = sourceCreature.DecreaseSpeed(5, true);
			deltaSpeed = sourceCreature.DecreaseIntelligence(5, true);
			sourceCreature.IncreaseLust(20 + Utils.Rand(sourceCreature.libido / 4));

			deltaLibido = sourceCreature.IncreaseLibido(10, true);
		}

		protected override void OnRemoval()
		{
			if (sourceCreature is CombatCreature target)
			{
				if (deltaCombatModifier != 0)
				{
					baseModifiers.combatDamageModifier /= deltaCombatModifier;
					deltaCombatModifier = 0;
				}
			}

			sourceCreature.DecreaseLibido(deltaLibido, true);
			sourceCreature.IncreaseIntelligence(0.9f * deltaIntelligence, true);
			sourceCreature.IncreaseSpeed(0.9f * deltaSpeed, true);

			deltaSpeed = 0;
			deltaIntelligence = 0;

			deltaLibido = 0;
			//no lust change.
		}

		public void StackEffect(byte strength = 1)
		{
			deltaIntelligence += sourceCreature.DecreaseIntelligence(strength, true);
			deltaSpeed += sourceCreature.DecreaseSpeed(strength, true);

			//delay the wears off timer.
			timeWearsOff = base.timeWearsOff.Delta(2 * strength);

			sourceCreature.IncreaseLust(30 + Utils.Rand(sourceCreature.libido / 4));
			deltaLibido += sourceCreature.IncreaseLibido(10 * strength, true);
		}

		public override string ObtainText()
		{
			return "You are definitely drunk from Niamh's... beer-milk? milk-beer? breast-beer? Fuck it, you're too sloshed (and horny) to care.";
		}

		public override string HasPerkText()
		{
			return "You are somewhat drunk and definitely horny from consuming the beer Niamh produces. It'll wear off eventually, but do you really want it to?";
		}


		protected override string OnStatusEffectTimePassing(byte hoursPassedSinceLastUpdate, out bool removeEffect)
		{
			removeEffect = false;
			return null;
		}

		protected override string OnStatusEffectWoreOff()
		{
			return Environment.NewLine + SafelyFormattedString.FormattedText("The warm, fuzzy feeling finally dissipates, leaving you thinking clearer, focusing better, " +
				"and less horny. It was nice while it lasted, but it's also good to be back to normal. Still, a part of you kind of wants another beer.", StringFormats.BOLD)
				+ Environment.NewLine;
		}

		public override bool isAilment => true;
	}
}
