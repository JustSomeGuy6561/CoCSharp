using CoC.Backend;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Perks
{
	public sealed class BroodMotherPerk : PerkBase
	{
		public BroodMotherPerk() : base(PerkName(), PerkText())
		{
		}

		private static SimpleDescriptor PerkName()
		{
			throw new NotImplementedException();
		}

		private static SimpleDescriptor PerkText()
		{
			throw new NotImplementedException();
		}

		protected override bool KeepOnAscension => false;

		protected override void OnActivation()
		{
			baseModifiers.incPregSpeedByOne();
		}

		protected override void OnRemoval()
		{
			baseModifiers.decPregSpeedByOne();
		}
	}
}
