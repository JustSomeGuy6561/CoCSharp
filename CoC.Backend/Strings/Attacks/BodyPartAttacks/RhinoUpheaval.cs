using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class RhinoUpheaval
	{
		private static string Attack()
		{
			return "Upheaval";
		}

		private string Tip(CombatCreature attacker)
		{
			return "Send your foe flying with your dual nose mounted horns." + Environment.NewLine +
				Environment.NewLine + "Fatigue Cost: " + attacker.physicalCost(attackCost);
		}
	}
}
