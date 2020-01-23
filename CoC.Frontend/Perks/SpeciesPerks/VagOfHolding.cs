using CoC.Backend;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Perks.SpeciesPerks
{
	class VagOfHolding : PerkBase
	{
		public VagOfHolding() : base(PerkName, HavePerkText)
		{
		}

		private static string HavePerkText()
		{
			return "In addition to the various kitsune-like traits you've obtained over your travels, you've inherited the ability to handle otherwise impossible penetrations " +
				"without any adverse effect on your body. It's safe to assume you would lose this ability if you ever lost all of your kitsune-like traits.";
		}

		private static string PerkName()
		{
			return "Vag of Holding";
		}

		protected override bool KeepOnAscension => false;

		protected override void OnActivation()
		{
			this.baseModifiers.PerkBasedBonusVaginalCapacity += 8000;
			//subscribe to some non-existent score check. when it procs, check the kitsune score. if 0, fire a reaction that removes this.
		}

		protected override void OnRemoval()
		{
			this.baseModifiers.PerkBasedBonusVaginalCapacity -= 8000;
			//unsubscribe from score check.
		}
	}
}
