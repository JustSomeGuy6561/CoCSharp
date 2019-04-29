using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public partial class TailSlap
	{
		private static string Attack()
		{
			return "Tail Slap";
		}

		private string Tip(CombatCreature attacker)
		{
			return "Set your tail ablaze in red-hot flames to whip your foe with it to hurt and burn them!" +
				Environment.NewLine + Environment.NewLine + "Fatigue Cost: " + attacker.physicalCost(attackCost);
		}
	}
}
