using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class NagaConstrict : PhysicalSpecial
	{
		internal NagaConstrict() : base(Attack) {}

		protected override ushort attackCost => throw new NotImplementedException();

		protected override DescriptorWithArg<CombatCreature> AttackDescription()
		{
			return Tip;
		}

		protected override bool CanUseAttack(CombatCreature attacker, CombatCreature defender)
		{
			return base.CanUseAttack(attacker, defender);// && !defender.isConstricted; //should be a combat status effect. so !defender.hasCombatStatusEffect(CombatStatusEffect.Constricted);
		}

		protected override SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender)
		{
			throw new NotImplementedException();
		}
	}
}
