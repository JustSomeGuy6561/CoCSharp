using CoC.Backend;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Perks.SpeciesPerks
{
	//another goo perk - i considered adding it to elastic innards, and probably still could. it's here mostly for legacy reasons, though i suppose it could simply be a conditional
	//perk as well.
	internal class SlimeCraving : StandardPerk
	{
		public SlimeCraving() : base()
		{
		}

		public override string Name()
		{
			throw new NotImplementedException();
		}

		public override string HasPerkText() => throw new NotImplementedException();

		protected override void OnActivation()
		{
			throw new NotImplementedException();
		}

		protected override void OnRemoval()
		{
			throw new NotImplementedException();
		}

		protected override bool keepOnAscension => false;

		public override bool isAilment => true;
	}
}
