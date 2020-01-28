using CoC.Backend;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Perks.SpeciesPerks
{
	class ElasticInnards : PerkBase
	{
		//slime perk. grants +10,000 to both anal and vaginal capacity. requires goo body, and will fire an event to remove itself whenever the creature loses their slime body.
		public ElasticInnards() : base()
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
