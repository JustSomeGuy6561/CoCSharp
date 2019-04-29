using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class SpiderBite
	{
		private static string Attack()
		{
			return "Bite";
		}

		private string Tip(CombatCreature attacker)
		{
			return "Attempt to bite your opponent and inject venom. \n\nFatigue Cost: " + attacker.physicalCost(attackCost);
		}
	}
}
