using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class GoreHorn
	{
		private static string Attack()
		{
			return "Gore";
		}

		private string Tip(CombatCreature attacker)
		{
			return "Lower your head and charge your opponent, attempting to gore them on your horn" + (this.hornCount > 1 ? "s" : "") +
				". This attack is stronger and easier to land with large horns. " + Environment.NewLine + Environment.NewLine +
				"Fatigue Cost: " + attacker.physicalCost(attackCost);
		}
	}
}
