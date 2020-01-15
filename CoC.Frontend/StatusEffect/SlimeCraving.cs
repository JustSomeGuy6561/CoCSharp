using CoC.Backend;
using CoC.Backend.StatusEffect;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.StatusEffect
{
	internal class SlimeCraving : ConditionalStatusEffect
	{
		public SlimeCraving(SimpleDescriptor name) : base(name)
		{
		}

		public override string ObtainText() => throw new NotImplementedException();

		public override string ShortDescription() => throw new NotImplementedException();

		public override string LongDescription() => throw new NotImplementedException();

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
