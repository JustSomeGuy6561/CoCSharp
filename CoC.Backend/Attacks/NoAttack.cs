using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Attacks
{
	public sealed class NoAttack : AttackBase
	{
		internal static NoAttack instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new NoAttack();
				}
				return _instance;
			}
		}
		private static NoAttack _instance;

		private NoAttack() : base(GlobalStrings.None)
		{}

		protected override DescriptorWithArg<CombatCreature> AttackDescription()
		{
			return x => GlobalStrings.None();
		}

		protected override bool CanUseAttack(CombatCreature attacker, CombatCreature defender)
		{
			return false;
		}

		protected override SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender)
		{
			return GlobalStrings.None;
		}

		protected override bool isPhysical => false;
	}
}
