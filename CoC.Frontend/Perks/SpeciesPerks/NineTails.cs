using CoC.Backend;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Perks.SpeciesPerks
{
	//combines the corrupted and enlightened perks into one, which makes it easier to maintain. grants the user a boost to magic damage, and frankly i have no idea what else.
	//i assume the corrupted form has some lust cost to it, idk. lost when the creature no longer has 9 Fox Tails.
	class NineTails : PerkBase
	{
		private readonly bool isEnlightened;
		public NineTails(bool enlightened) : base()
		{
			isEnlightened = enlightened;
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
