using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public abstract class PhysicalSpecial : AttackBase
	{

		protected abstract ushort attackCost { get; }
		public PhysicalSpecial(SimpleDescriptor name) : base(name) {}

		protected override bool CanUseAttack(CombatCreature attacker, CombatCreature defender)
		{
			return attacker.hasEnoughStamina(attackCost, isPhysical);
		}

		protected override bool isPhysical => true;
	}
}
