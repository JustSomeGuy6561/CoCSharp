//AttackBase.cs
//Description:
//Author: JustSomeGuy
//1/17/2019, 2:12 AM
using CoC.Strings;
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Engine.Combat.Attacks
{
	public abstract class AttackBase
	{
		public abstract void UseAttack (Creature attacker, Creature Defender);

		public abstract string attackName();
		public abstract string attackHint();

		public static AttackBase NO_ATTACK = new NoAttack();

		private class NoAttack : AttackBase
		{
			public NoAttack() { }
			public override string attackHint()
			{
				return GlobalStrings.None();
			}

			public override string attackName()
			{
				return GlobalStrings.None();
			}

			public override void UseAttack(Creature attacker, Creature Defender)
			{ }
		}
	}

	
}
