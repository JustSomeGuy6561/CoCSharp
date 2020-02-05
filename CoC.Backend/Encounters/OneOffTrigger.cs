using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.UI;

namespace CoC.Backend.Encounters
{
	public class OneOffTrigger : TriggeredEncounter
	{
		private readonly Action doProc;
		public OneOffTrigger(Action onProc)
		{
			doProc = onProc ?? throw new ArgumentNullException(nameof(onProc));
		}

		public bool ran { get; private set; } = false;
		protected internal override bool EncounterDisabled()
		{
			return ran;
		}

		protected internal override bool EncounterUnlocked()
		{
			return !ran;
		}

		protected internal override bool isTriggered()
		{
			return !ran;
		}

		protected internal override void RunEncounter()
		{
			doProc();
			ran = true;
		}
	}
}
