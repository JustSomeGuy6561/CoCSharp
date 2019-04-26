using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Backend.Engine.Combat.Attacks.BodyPartAttacks
{
	public sealed partial class TentaGrab : ResourceAttackBase
	{
		const ushort MAX_CHARGE = 10;
		const ushort MAX_RECHARGE = 2;
		const ushort INITIAL_AMOUNT = 0;
		const ushort INITIAL_RECHARGE = 1;

		const ushort MIN_ATTACK_AMOUNT = 3;

		public TentaGrab(Func<ushort> getResourceCount, Action<ushort> setResourceCount) : base(MAX_CHARGE, MAX_RECHARGE, INITIAL_AMOUNT, INITIAL_RECHARGE, getResourceCount, setResourceCount, Attack) { }

		public override SimpleDescriptor AttackDescription()
		{
			return Tip;
		}

		public override bool CanUseAttack(CombatCreature source)
		{
			return resourceCount > MIN_ATTACK_AMOUNT;
		}

		public override SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender)
		{
			int attackStrength = resourceCount / 2; //num turns it'll last. 
			resourceCount = 0; //remove all tendrils.
			throw new NotImplementedException();
		}
	}
}
