using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class AnemoneSting
	{
		private static string Attack()
		{
			return "AnemoneSting"; //12 chars, or else we'd add a space.
		}

		private string Tip()
		{
			return " Attempt to strike an opponent with the stinging tentacles growing from your scalp. Reduces enemy speed and increases enemy lust. " +
				Environment.NewLine + Environment.NewLine + "No Fatigue Cost";
		}
	}
}
