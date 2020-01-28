using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;
using CoC.Backend.StatusEffect;

namespace CoC.Frontend.StatusEffect
{
	class Dysfunction : TimedStatusEffect
	{
		const ushort INITIAL_TIMEOUT = 96;

		public Dysfunction(ushort timeout) : base(Name, timeout)
		{ }

		public Dysfunction() : base(Name, INITIAL_TIMEOUT)
		{ }

		private static string Name()
		{
			throw new NotImplementedException();
		}

		protected override string OnStatusEffectTimePassing(byte hoursPassedSinceLastUpdate, out bool removeEffect)
		{
			throw new NotImplementedException();
		}

		protected override string OnStatusEffectWoreOff()
		{
			throw new NotImplementedException();
		}

		protected override void OnActivation()
		{
			throw new NotImplementedException();
		}

		protected override void OnRemoval()
		{
			throw new NotImplementedException();
		}

		public override string ObtainText()
		{
			throw new NotImplementedException();
		}

		public override string HaveStatusEffectText()
		{
			throw new NotImplementedException();
		}
	}
}
