using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;
using CoC.Backend.Perks;

namespace CoC.Frontend.StatusEffect
{
	class Dysfunction : TimedPerk
	{
		const ushort INITIAL_TIMEOUT = 96;

		public Dysfunction(ushort timeout) : base(timeout)
		{ }

		public Dysfunction() : base(INITIAL_TIMEOUT)
		{ }

		public override string Name()
		{
			throw new NotImplementedException();
		}

		protected override string OnStatusEffectTimePassing(byte hoursPassedSinceLastUpdate, out bool removeEffect)
		{
			throw new NotImplementedException();
		}

		protected override string OnStatusEffectWoreOff(byte hoursPassedSinceLastUpdate)
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

		public override string HasPerkText()
		{
			throw new NotImplementedException();
		}

		public override bool isAilment => true;
	}
}
