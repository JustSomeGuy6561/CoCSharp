using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public partial class TailWhip
	{
		private static string Attack()
		{
			return "Tail Whip";
		}

		private string Tip(CombatCreature attacker)
		{
			return "Whip your foe with your tail to enrage them and lower their defense!" +
				Environment.NewLine + Environment.NewLine + "Fatigue Cost: " + attacker.physicalCost(attackCost);
		}
	}
}
