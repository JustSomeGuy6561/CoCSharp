using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public partial class TailSlam
	{
		private static string Attack()
		{
			return "Tail Slam";
		}

		private string Tip(CombatCreature attacker)
		{
			return "Slam your foe with your mighty "+ name() + "! This attack causes grievous harm and can stun your opponent or let it bleed. " +
				Environment.NewLine + Environment.NewLine + "Fatigue Cost: " + attacker.physicalCost(attackCost);
		}
	}
}
