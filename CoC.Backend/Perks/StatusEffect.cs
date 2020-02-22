using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Engine.Time;

namespace CoC.Backend.Perks
{
	using StatusEffect = TimedPerk;

	//A Timed Perk (or, for a legacy perspective, a StatusEffect) is like a standard perk, but it automatically deactivates after a period of time has passed.
	//Beyond this, a timed perk has no further limitations - it may be possible to stack a timed perk indefinitely if you feel the need to do so.

	//Note that there is no fundamental difference between a Status Effect and a Perk now, aside from how they are removed.
	public abstract class TimedPerk : PerkBase
	{
		protected GameDateTime timeWearsOff;

		public int hoursRemaining => GameDateTime.Now.hoursTo(timeWearsOff);

		protected TimedPerk(ushort initialTimeout) : base()
		{
			timeWearsOff = GameDateTime.HoursFromNow(initialTimeout);
		}

		private protected override void OnCreate()
		{
			OnActivation();
		}

		private protected override void OnDestroy()
		{
			OnRemoval();
		}

		internal string ReactToTimePassing(byte hoursPassed, out bool removeEffect)
		{
			string text = null;
			if (GameDateTime.Now.CompareTo(timeWearsOff) >= 0)
			{
				text = OnStatusEffectWoreOff();
				removeEffect = true;
			}
			else
			{
				text = OnStatusEffectTimePassing(hoursPassed, out removeEffect);
			}
			return text;
		}


		//called when the status effect is added to the status effect collection on the character.
		protected abstract void OnActivation();

		//called when the status effect is removed from the status effect collection.
		protected abstract void OnRemoval();

		//optionally called when you obtain the status effect. this provides a generic text that things that cause this status effect can use to describe what happened.
		//of course, they may decide to roll out their own text, but this is necessary in case they don't want to and just want a nice default.
		public abstract string ObtainText();

		//a timed status effect can optionally output text as time passes, and optionally tell us we should remove it early, if conditions are met that you decide merits early
		//removal. be sure to set removeEffect to true or false before the function returns - i generally recommend setting it to false as the first line, then updating it to
		//true later if you need to.
		protected abstract string OnStatusEffectTimePassing(byte hoursPassedSinceLastUpdate, out bool removeEffect);

		//a timed status effect can explain to its source creature that it has worn off via this function. if you have nothing to say, simply return null or the empty string.
		protected abstract string OnStatusEffectWoreOff();

		private protected override bool retainOnAscension => false;

		private protected override bool enabled => !(sourceCreature is null);
	}
}
