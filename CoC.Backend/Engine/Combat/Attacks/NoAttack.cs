using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Engine.Combat.Attacks
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

		private NoAttack() : base(GlobalStrings.None, GlobalStrings.None, GlobalStrings.None)
		{}

		public override bool CanUseAttack(CombatCreature source)
		{
			return false;
		}

		public override SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender)
		{
			return GlobalStrings.None;
		}
	}
}
