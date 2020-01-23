using CoC.Backend;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Perks.SpeciesPerks
{
	class Lustzerker : PerkBase
	{
		//salamander perk. grants a berserker-like ability to combat, and deals additional damage when has high lust and/or low hp. lost when creature drops below a certain
		//salamander score.
		public Lustzerker(SimpleDescriptor perkName, SimpleDescriptor havePerkText) : base(perkName, havePerkText)
		{
		}

		protected override bool KeepOnAscension => throw new NotImplementedException();

		protected override void OnActivation()
		{
			throw new NotImplementedException();
		}

		protected override void OnRemoval()
		{
			throw new NotImplementedException();
		}
	}
}
