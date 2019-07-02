//BeeSting.cs
//Description:
//Author: JustSomeGuy
//4/29/2019, 1:01 AM
using CoC.Backend.Creatures;
using System;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class BeeSting : ResourceAttackBase
	{
		const ushort MAX_CHARGE = 100;
		const ushort MAX_RECHARGE = 25;
		const ushort INITIAL_AMOUNT = 0;
		const ushort INITIAL_RECHARGE = 5;

		const ushort ATTACK_COST = 33;
		internal BeeSting(Func<ushort> getResourceCount, Action<ushort> setResourceCount) : base(MAX_CHARGE, MAX_RECHARGE, INITIAL_AMOUNT, INITIAL_RECHARGE, getResourceCount, setResourceCount, Attack) { }

		protected override DescriptorWithArg<CombatCreature> AttackDescription()
		{
			return (x) => Tip();
		}

		protected override bool CanUseAttack(CombatCreature attacker, CombatCreature defender)
		{
			return resourceCount >= ATTACK_COST;
		}

		protected override SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender)
		{

			resourceCount -= ATTACK_COST;
			throw new NotImplementedException();
		}

		protected override bool isPhysical => true;
	}
}
