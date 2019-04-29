using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class BasiliskStare
	{
		private static string Attack()
		{
			return "Stare";
		}
		private string Tip(CombatCreature attacker)
		{
			return " Focus your gaze at your opponent, lowerng their speed. The more you use this in a battle, the lesser effective it becomes." +
				Environment.NewLine + Environment.NewLine + "Fatigue Cost: " + attacker.spellCost(attackCost);
		}

	}
}
