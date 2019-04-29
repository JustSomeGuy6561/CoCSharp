using System;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public partial class TentaGrab
	{
		private static string Attack()
		{
			return "Bind";
		}

		private string Tip()
		{
			return " Attempt to all of your tendrils to hold your enemy in place. This will use all your tendrils, and they will be ripped apart if the enemy escapes. " +
				"Fortunately, they regrow, but you'll have to wait a while before they do." + Environment.NewLine + Environment.NewLine +
				"Tendril Count: " + resourceCount + " / " + maxResource;
		}
	}
}
