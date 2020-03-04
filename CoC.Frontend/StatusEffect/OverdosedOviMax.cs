using CoC.Backend;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.StatusEffect
{
	public sealed partial class OverdosedOviMax : TimedPerk
	{
		private const ushort OVIMAX_DEFAULT_TIMEOUT = 336; //Two weeks.

		public byte overdoseCount { get; private set; }

		public OverdosedOviMax() : base(OVIMAX_DEFAULT_TIMEOUT)
		{
			overdoseCount = 1;
		}

		public override string ObtainText() => OverdoseText();



		public override string HasPerkText() => ODShort();

		protected override void OnActivation()
		{ }

		protected override void OnRemoval()
		{ }

		public void RepeatOverdose()
		{
			overdoseCount++;
			timeWearsOff = timeWearsOff.Delta(OVIMAX_DEFAULT_TIMEOUT);
		}

		protected override string OnStatusEffectTimePassing(byte hoursPassedSinceLastUpdate, out bool removeEffect)
		{
			removeEffect = false;
			return null;
		}

		protected override string OnStatusEffectWoreOff(byte hoursPassedSinceLastUpdate)
		{
			return null;
		}

		public override bool isAilment => true;
	}
}
