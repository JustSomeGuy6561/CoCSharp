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
			baseModifiers.incPregSpeedByOne();
		}

		protected override void OnRemoval()
		{
			baseModifiers.decPregSpeedByOne();
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
