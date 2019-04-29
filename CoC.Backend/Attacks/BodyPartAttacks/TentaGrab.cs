using CoC.Backend.Creatures;
using System;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class TentaGrab : ResourceAttackBase
	{
		const ushort MAX_CHARGE = 10;
		const ushort MAX_RECHARGE = 2;
		const ushort INITIAL_AMOUNT = 0;
		const ushort INITIAL_RECHARGE = 1;

		const ushort MIN_ATTACK_AMOUNT = 3;

		internal TentaGrab(Func<ushort> getResourceCount, Action<ushort> setResourceCount) : base(MAX_CHARGE, MAX_RECHARGE, INITIAL_AMOUNT, INITIAL_RECHARGE, getResourceCount, setResourceCount, Attack) { }

		protected override DescriptorWithArg<CombatCreature> AttackDescription()
		{
			return x => Tip();
		}

		protected override bool CanUseAttack(CombatCreature attacker, CombatCreature defender)
		{
			return resourceCount > MIN_ATTACK_AMOUNT;
		}

		protected override SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender)
		{
			int attackStrength = resourceCount / 2; //num turns it'll last. 
			resourceCount = 0; //remove all tendrils.
			throw new NotImplementedException();
		}

		protected override bool isPhysical => true;
	}
}
