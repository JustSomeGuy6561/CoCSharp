//HairWhip.cs
//Description:
//Author: JustSomeGuy
//4/28/2019, 10:05 PM
using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class HairWhip : PhysicalSpecial
	{
		protected override ushort attackCost => 0;
		public HairWhip() : base(Attack) {}

		protected override DescriptorWithArg<CombatCreature> AttackDescription()
		{
			return Tip;
		}

		protected override bool CanUseAttack(CombatCreature attacker, CombatCreature defender)
		{
			return true;
		}

		protected override SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender)
		{
			throw new NotImplementedException();
		}
	}
}
