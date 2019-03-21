using System;
using System.Collections.Generic;
using System.Text;
using CoC.Creatures;

namespace CoC.Engine.Combat.Attacks
{
	internal abstract class AttackWithCooldown : AttackBase
	{
		public abstract byte cooldownTurns { get; }
		public override bool canUseAttack()
		{
			return base.canUseAttack();
		}

		public override void UseAttack(Creature attacker, Creature Defender)
		{
			throw new NotImplementedException();
		}
	}
}
