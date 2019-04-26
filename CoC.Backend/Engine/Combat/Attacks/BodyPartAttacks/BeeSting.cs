using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Backend.Engine.Combat.Attacks.BodyPartAttacks
{
	public sealed partial class BeeSting : ResourceAttackBase
	{
		const ushort MAX_CHARGE = 100;
		const ushort MAX_RECHARGE = 25;
		const ushort INITIAL_AMOUNT = 0;
		const ushort INITIAL_RECHARGE = 5;

		const ushort ATTACK_COST = 33;
		public BeeSting(Func<ushort> getResourceCount, Action<ushort> setResourceCount) : base(MAX_CHARGE, MAX_RECHARGE, INITIAL_AMOUNT, INITIAL_RECHARGE, getResourceCount, setResourceCount, Attack) { }

		public override SimpleDescriptor AttackDescription()
		{
			return Tip;
		}

		public override bool CanUseAttack(CombatCreature source)
		{
			return resourceCount > ATTACK_COST;
		}

		public override SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender)
		{

			resourceCount -= ATTACK_COST;
			throw new NotImplementedException();
		}
	}
}
