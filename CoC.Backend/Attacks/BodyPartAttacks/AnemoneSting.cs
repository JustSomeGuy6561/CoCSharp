﻿using CoC.Backend.Creatures;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class AnemoneSting : PhysicalSpecial
	{
		protected override ushort attackCost => 0;

		internal AnemoneSting() : base(Attack) { }

		protected override DescriptorWithArg<CombatCreature> AttackDescription()
		{
			return (x) => Tip();
		}

		protected override SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender)
		{
			throw new System.NotImplementedException();
		}
	}
}
