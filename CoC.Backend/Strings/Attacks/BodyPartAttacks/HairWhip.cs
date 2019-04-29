using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class HairWhip
	{
		private static string Attack()
		{
			return "Hair Whip";
		}

		private static string Tip(CombatCreature attacker)
		{
			return "Whip your opponent with your long purple hair, though you'll probably hurt your neck in the process." +
				Environment.NewLine + Environment.NewLine + "No Fatigue Cost.";
		}
	}
}
