//GenericBite.cs
//Description:
//Author: JustSomeGuy
//4/28/2019, 10:47 PM
using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class GenericBite : PhysicalSpecial
	{
		//note: strength is 0-5, cost increasing by 5 per level.
		private readonly byte biteStrength;
		private readonly SimpleDescriptor teeth;

		protected override ushort attackCost => biteStrength.mult(5).add(10);

		public GenericBite(SimpleDescriptor teethName, byte strength = 1) : base(Attack)
		{
			biteStrength = Utils.Clamp2(strength, (byte)0, (byte)5);
			teeth = teethName ?? throw new ArgumentNullException();
		}

		protected override DescriptorWithArg<CombatCreature> AttackDescription()
		{
			return (x) => Tip(teeth(), x);
		}

		protected override SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender)
		{
			throw new NotImplementedException();
		}
	}
}
