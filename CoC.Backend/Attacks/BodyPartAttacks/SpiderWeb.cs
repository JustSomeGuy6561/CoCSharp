using CoC.Backend.Creatures;
using System;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class SpiderWeb : ResourceAttackBase
	{
		const ushort MAX_CHARGE = 100;
		const ushort MAX_RECHARGE = 25;
		const ushort INITIAL_AMOUNT = 0;
		const ushort INITIAL_RECHARGE = 5;

		const ushort ATTACK_COST = 33;
		internal SpiderWeb(Func<ushort> getResourceCount, Action<ushort> setResourceCount) : base(MAX_CHARGE, MAX_RECHARGE, INITIAL_AMOUNT, INITIAL_RECHARGE, getResourceCount, setResourceCount, Attack) { }

		protected override bool CanUseAttack(CombatCreature attacker, CombatCreature defender)
		{
			return resourceCount > ATTACK_COST;
		}

		protected override DescriptorWithArg<CombatCreature> AttackDescription()
		{
			return (x) => Tip();
		}

		protected override SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender)
		{
			//if (!CanUseAttack())
			//{
			//	return NotEnoughResources();
			//}
			//attempt the webbing. 
			resourceCount -= ATTACK_COST;
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		protected override bool isPhysical => true;
	}
}
