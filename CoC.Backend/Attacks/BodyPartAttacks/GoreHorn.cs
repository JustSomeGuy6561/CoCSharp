//GoreHorn.cs
//Description:
//Author: JustSomeGuy
//4/29/2019, 12:05 AM
using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	//more successful with larger and/or multiple horns. I made it this way so unicorns could use it.
	public sealed partial class GoreHorn : PhysicalSpecial
	{
		private readonly Func<byte> len;
		private readonly Func<byte> count;

		private byte hornLength => len();
		private byte hornCount => count();

		private const byte ATTACK_COST = 15;

		protected override ushort attackCost => ATTACK_COST;

		public GoreHorn(Func<byte> hornCount, Func<byte> hornLength) : base(Attack)
		{
			len = hornLength ?? throw new ArgumentNullException();
			count = hornCount ?? throw new ArgumentNullException();
		}

		protected override DescriptorWithArg<CombatCreature> AttackDescription()
		{
			return Tip;
		}

		protected override SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender)
		{
			throw new NotImplementedException();
		}
	}
}
