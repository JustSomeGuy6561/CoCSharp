using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class GenericBite
	{
		private static string Attack()
		{
			return "Bite";
		}

		private string Tip(string name, CombatCreature attacker)
		{
			return "Attempt to bite your opponent with your " + name + "." +
				Environment.NewLine + Environment.NewLine + "Fatigue Cost: " + attacker.physicalCost(attackCost); 
		}
	}
}
