using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class ScorpionSting
	{
		private static string Attack()
		{
			return "Sting";
		}

		private string Tip()
		{
			return " Attempt to use your scorpion stinger, paralyzing and potentially poisoning your opponent. Be aware it takes quite a while for your venom to build up, " +
				"so be aware you may have to wait quite a while between stings." + Environment.NewLine + Environment.NewLine + "Venom: " + resourceCount + " / " + maxResource;
		}
	}
}
