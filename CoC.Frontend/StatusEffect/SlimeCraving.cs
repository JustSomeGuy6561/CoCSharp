using CoC.Backend;
using CoC.Backend.StatusEffect;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.StatusEffect
{

#warning  Consider moving this to species perks as a stackable effect on goo transformations.
	internal class SlimeCraving : ConditionalStatusEffect
	{
		public SlimeCraving() : base(Name)
		{
		}

		private static string Name()
		{
			throw new NotImplementedException();
		}

		public override string ObtainText() => throw new NotImplementedException();

		public override string HaveStatusEffectText() => throw new NotImplementedException();

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
