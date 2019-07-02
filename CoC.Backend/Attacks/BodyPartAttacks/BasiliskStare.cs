//BasiliskStare.cs
//Description:
//Author: JustSomeGuy
//4/28/2019, 10:05 PM
using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class BasiliskStare : MagicalSpecial
	{
		private const byte FATIGUE_COST = 20;

		protected override ushort attackCost => FATIGUE_COST;

		public BasiliskStare() : base(Attack) {}

		protected override DescriptorWithArg<CombatCreature> AttackDescription()
		{
			return Tip;
		}

		protected override bool CanUseAttack(CombatCreature attacker, CombatCreature defender)
		{
			return base.CanUseAttack(attacker, defender);// && !defender.hasStatusEffect(this);
		}

		protected override SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender)
		{
			throw new NotImplementedException();
		}
	}
}
