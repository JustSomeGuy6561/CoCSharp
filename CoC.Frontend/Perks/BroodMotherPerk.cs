using CoC.Backend;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Perks
{
	public sealed class BroodMotherPerk : StandardPerk
	{
		public BroodMotherPerk() : base()
		{
		}


		protected override bool keepOnAscension => false;

		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.pregnancySpeedModifier, new ValueModifierStore<sbyte>(ValueModifierType.FLAT_ADD, 1));
		}

		protected override void OnRemoval()
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
	}
}
