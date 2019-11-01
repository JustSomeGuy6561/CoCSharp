using CoC.Backend;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Perks.SpeciesPerks
{
	class VagOfHolding : PerkBase
	{
		public VagOfHolding(SimpleDescriptor perkName, SimpleDescriptor havePerkText) : base(perkName, havePerkText)
		{
		}

		protected override bool KeepOnAscension => false;

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
