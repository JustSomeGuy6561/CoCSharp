//TailAttack.cs
//Description:
//Author: JustSomeGuy
//1/17/2019, 2:34 AM
using CoC.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Engine.Combat.Attacks
{
	internal static class TailAttack
	{
		private class SalamanderWhip : AttackBase
		{
			public override string attackHint()
			{
				throw new NotImplementedException();
			}

			public override string attackName()
			{
				throw new NotImplementedException();
			}

			public override void UseAttack(Creature attacker, Creature Defender)
			{
				throw new NotImplementedException();
			}
		}

		private class TailSlam : AttackBase
		{
			private readonly int level;
			public TailSlam(int attackStrength)
			{
				level = attackStrength;
			}
			public override string attackHint()
			{
				throw new NotImplementedException();
			}

			public override string attackName()
			{
				throw new NotImplementedException();
			}

			public override void UseAttack(Creature attacker, Creature Defender)
			{
				throw new NotImplementedException();
			}
		}
		private class TailSlap : AttackBase
		{
			private readonly int level;
			public TailSlap(int slapStrength)
			{
				level = slapStrength;
			}
			public override string attackHint()
			{
				throw new NotImplementedException();
			}

			public override string attackName()
			{
				throw new NotImplementedException();
			}

			public override void UseAttack(Creature attacker, Creature Defender)
			{
				throw new NotImplementedException();
			}
		}

		private class SpiderWeb : AttackBase
		{
			public override string attackHint()
			{
				throw new NotImplementedException();
			}

			public override string attackName()
			{
				throw new NotImplementedException();
			}

			public override void UseAttack(Creature attacker, Creature Defender)
			{
				throw new NotImplementedException();
			}
		}

		private class ScorpionSting : AttackBase
		{
			public override string attackHint()
			{
				throw new NotImplementedException();
			}

			public override string attackName()
			{
				throw new NotImplementedException();
			}

			public override void UseAttack(Creature attacker, Creature Defender)
			{
				throw new NotImplementedException();
			}
		}
		private class BeeSting : AttackBase
		{
			public override string attackHint()
			{
				throw new NotImplementedException();
			}

			public override string attackName()
			{
				throw new NotImplementedException();
			}

			public override void UseAttack(Creature attacker, Creature Defender)
			{
				throw new NotImplementedException();
			}
		}
		public static readonly AttackBase WEB = new SpiderWeb();
		public static readonly AttackBase BEE_STING = new BeeSting();
		public static readonly AttackBase SCORPION_STING = new ScorpionSting();
		public static readonly AttackBase SALAMANDER_WHIP = new SalamanderWhip();
		public static readonly AttackBase SLAP = new TailSlap(1);
		public static readonly AttackBase SLAM = new TailSlam(1);
		public static readonly AttackBase DRAGON_SLAM = new TailSlam(2);
		public static readonly AttackBase COON_SLAP = new TailSlap(2);
		

	}
}
