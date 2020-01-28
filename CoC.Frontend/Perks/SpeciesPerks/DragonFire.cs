using CoC.Backend;
using CoC.Backend.Perks;
using CoC.Backend.Tools;
using System;
using System.Text;

namespace CoC.Frontend.Perks.SpeciesPerks
{
	class DragonFire : PerkBase
	{
		//dragon perk. Grants a breath attack for use in combat. Unlike most other species perks, this perk is not lost when the creature loses their dragon-like traits.
		//it is entirely possible to remove this artificially, but as of the current implementation, no item, TF, or NPC interaction removes this. Also, why would you want to?
		public DragonFire() : base()
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
