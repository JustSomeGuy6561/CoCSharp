using System;

namespace CoC.Backend.Attacks.BodyPartAttacks
{
	public sealed partial class SpiderWeb
	{
		private static string Attack()
		{
			return "Web";
		}

		private string Tip()
		{
			return "Attempt to use your abdomen to spray sticky webs at an enemy and greatly slow them down. Be aware it takes a while for your webbing to build up. "
				+ Environment.NewLine + Environment.NewLine + "Web Amount: " + resourceCount + " / " + MAX_CHARGE;
		}
	}
}
