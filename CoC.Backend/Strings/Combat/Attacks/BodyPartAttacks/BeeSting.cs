using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Backend.Engine.Combat.Attacks.BodyPartAttacks
{
	public sealed partial class BeeSting
	{
		private static string Attack()
		{
			return "Sting";
		}

		private string Tip()
		{
			return " Attempt to use your venomous bee stinger on an enemy. Be aware it takes quite a while for your venom to build up, so depending on your abdomen's refractory period, " +
				"you may have to wait quite a while between stings." + Environment.NewLine + Environment.NewLine + "Venom: " + resourceCount + " / " + maxResource;
		}
	}
}
