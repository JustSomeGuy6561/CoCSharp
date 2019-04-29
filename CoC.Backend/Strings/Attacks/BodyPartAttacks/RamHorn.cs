using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class RamHorn
	{
		private static string Attack()
		{
			return "Horn Stun";
		}

		private string Tip(CombatCreature attacker)
		{
			return "Use a ramming headbutt to try and stun your foe. " + Environment.NewLine +
				Environment.NewLine + "Fatigue Cost: " + attacker.physicalCost(attackCost);
		}
	}
}
