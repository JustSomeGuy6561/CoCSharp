//PhysicalSpecial.cs
//Description:
//Author: JustSomeGuy
//4/29/2019, 1:22 AM

using CoC.Backend.Creatures;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public abstract class PhysicalSpecial : AttackBase
	{

		protected abstract ushort attackCost { get; }
		public PhysicalSpecial(SimpleDescriptor name) : base(name) { }

		protected override bool CanUseAttack(CombatCreature attacker, CombatCreature defender)
		{
			return attacker.hasEnoughStamina(attackCost, isPhysical);
		}

		protected override bool isPhysical => true;
	}
}
