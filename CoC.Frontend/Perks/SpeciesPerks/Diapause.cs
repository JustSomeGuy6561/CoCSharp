using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;
using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.SpeciesPerks
{
	//kangaroo perk. unlike most species perks, it is not lost when the creature loses their kangaroo-like traits; it is gender-locked to females - when the creature no longer
	//has a vagina, they lose the perk.
	class Diapause : PerkBase
	{
		public Diapause() : base()
		{
		}

		public override string Name()
		{
			throw new NotImplementedException();
		}

		public override string HasPerkText()
		{
			throw new NotImplementedException();
		}

		protected override void OnActivation()
		{
			baseModifiers.hasDiapause = true;
		}

		protected override void OnRemoval()
		{
			baseModifiers.hasDiapause = false;
		}

		protected override bool KeepOnAscension => false;
	}
}
