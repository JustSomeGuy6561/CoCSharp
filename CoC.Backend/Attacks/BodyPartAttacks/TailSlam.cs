//TailSlam.cs
//Description:
//Author: JustSomeGuy
//4/29/2019, 12:45 AM
using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class TailSlam : PhysicalSpecial
	{
		private readonly byte attackStrength;
		protected override ushort attackCost => attackStrength.mult(5).add(5);
		private readonly SimpleDescriptor name;
		internal TailSlam(ShortDescriptor tailName, byte strength = 1) : base(Attack)
		{
			attackStrength = Utils.Clamp2(strength, (byte)0, (byte)5);
			if (tailName is null) throw new ArgumentNullException(nameof(tailName));
			name = () => tailName(false);
		}

		internal TailSlam(SimpleDescriptor tailName, byte strength = 1) : base(Attack)
		{
			attackStrength = Utils.Clamp2(strength, (byte)0, (byte)5);
			name = tailName ?? throw new ArgumentNullException();
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
