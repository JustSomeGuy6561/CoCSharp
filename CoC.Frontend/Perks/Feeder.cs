using CoC.Backend;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Perks
{
	public sealed class Feeder : StandardPerk
	{
		public Feeder() : base()
		{
		}

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
