﻿using CoC.Backend;
using CoC.Backend.StatusEffect;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.StatusEffect
{
	public sealed partial class OverdosedOviMax : TimedStatusEffect
	{
		private const ushort OVIMAX_DEFAULT_TIMEOUT = 336; //Two weeks.

		public byte overdoseCount { get; private set; } 

		public OverdosedOviMax() : base(OviMaxODText, OVIMAX_DEFAULT_TIMEOUT)
		{
			overdoseCount = 1;
		}

		public override SimpleDescriptor obtainText => OverdoseText;

		

		public override SimpleDescriptor ShortDescription => ODShort;

		public override SimpleDescriptor FullDescription => ODFull;

		

		protected override void OnActivation()
		{ }

		protected override void OnRemoval()
		{ }

		public void RepeatOverdose()
		{
			overdoseCount++;
			timeWearsOff = timeWearsOff.delta(OVIMAX_DEFAULT_TIMEOUT);
		}

		protected override string OnStatusEffectTimePassing(byte hoursPassedSinceLastUpdate)
		{
			return null;
		}

		protected override string OnStatusEffectWoreOff()
		{
			return null;
		}
	}
}