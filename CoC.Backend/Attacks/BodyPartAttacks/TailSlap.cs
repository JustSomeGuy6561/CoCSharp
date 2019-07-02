//TailSlap.cs
//Description:
//Author: JustSomeGuy
//4/29/2019, 12:45 AM
using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class TailSlap : PhysicalSpecial
	{
		private const byte ATTACK_COST = 30;

		protected override ushort attackCost => ATTACK_COST;

		internal TailSlap() : base(Attack) {}

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
