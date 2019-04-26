using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Strings;

namespace CoC.Backend.Engine.Combat.Attacks.BodyPartAttacks
{
	public sealed partial class SpiderWeb : ResourceAttackBase
	{
		const ushort MAX_CHARGE = 100;
		const ushort MAX_RECHARGE = 25;
		const ushort INITIAL_AMOUNT = 0;
		const ushort INITIAL_RECHARGE = 5;

		const ushort ATTACK_COST = 33;
		public SpiderWeb(Func<ushort> getResourceCount, Action<ushort> setResourceCount) : base(MAX_CHARGE, MAX_RECHARGE, INITIAL_AMOUNT, INITIAL_RECHARGE, getResourceCount, setResourceCount, Attack) {}

		public override bool CanUseAttack(CombatCreature source)
		{
			return resourceCount > ATTACK_COST;
		}

		public override SimpleDescriptor AttackDescription()
		{
			return Tip;
		}

		public override SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender)
		{
			//if (!CanUseAttack())
			//{
			//	return NotEnoughResources();
			//}
			//attempt the webbing. 
			resourceCount -= ATTACK_COST;
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
	}
}
