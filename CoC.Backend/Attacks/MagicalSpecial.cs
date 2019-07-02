//MagicalSpecial.cs
//Description:
//Author: JustSomeGuy
//4/29/2019, 1:22 AM
using CoC.Backend.Creatures;

namespace CoC.Backend.Attacks
{
	public abstract class MagicalSpecial : AttackBase
	{
		protected abstract ushort attackCost { get; }
		public MagicalSpecial(SimpleDescriptor name) : base(name) { }

		protected override bool CanUseAttack(CombatCreature attacker, CombatCreature defender)
		{
			return attacker.hasEnoughStamina(attackCost, isPhysical);
		}

		protected override bool isPhysical => false;
	}
}
