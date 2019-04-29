using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class NagaConstrict
	{
		private static string Attack()
		{
			return "Constrict";
		}

		private string Tip(CombatCreature attacker)
		{
			return "Attempt to bind an enemy in your long snake-tail." + Environment.NewLine + Environment.NewLine + "Fatigue Cost: " + attacker.physicalCost(attackCost);
		}
	}
}
