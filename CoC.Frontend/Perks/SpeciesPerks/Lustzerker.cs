using CoC.Backend;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Perks.SpeciesPerks
{
	class Lustzerker : StandardPerk
	{
		//salamander perk. grants a berserker-like ability to combat, and deals additional damage when has high lust and/or low hp. lost when creature drops below a certain
		//salamander score.
		public Lustzerker() : base()
		{ }

		protected override bool keepOnAscension => throw new NotImplementedException();

		protected override void OnActivation()
		{
			throw new NotImplementedException();
		}

		protected override void OnRemoval()
		{
			throw new NotImplementedException();
		}

		public override string Name()
		{
			throw new NotImplementedException();
		}

		public override string HasPerkText()
		{
			throw new NotImplementedException();
		}
	}
}
