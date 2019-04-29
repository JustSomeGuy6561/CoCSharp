using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class TailWhip : PhysicalSpecial
	{
		private const byte ATTACK_COST = 15;

		protected override ushort attackCost => ATTACK_COST;
		public TailWhip() : base(Attack) { }

		protected override DescriptorWithArg<CombatCreature> AttackDescription()
		{
			return Tip;
		}


		protected override SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender)
		{
			throw new NotImplementedException();
		}
	}
}
