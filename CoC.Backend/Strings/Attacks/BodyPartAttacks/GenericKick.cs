using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class GenericKick
	{
		private static string Attack()
		{
			return "Kick";
		}

		private string Tip(CombatCreature attacker)
		{
			return "Attempt to kick an enemy using your powerful lower body." + Environment.NewLine +
				Environment.NewLine + "Fatigue Cost: " + attacker.physicalCost(attackCost);
		}
	}
}
