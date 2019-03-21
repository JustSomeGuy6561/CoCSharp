using System;
using System.Collections.Generic;
using System.Text;
using CoC.Creatures;

namespace CoC.Engine.Combat.Attacks
{
	internal abstract class AttackWithResource : AttackBase
	{
		public abstract AttackResource GenerateAttackResource();

		public short attackCost;

		public virtual bool CanUseAttack(AttackResource resource)
		{
			return resource.numResource > attackCost;
		}

		public override void UseAttack(Creature attacker, Creature Defender)
		{
			attac
		}
	}
}
