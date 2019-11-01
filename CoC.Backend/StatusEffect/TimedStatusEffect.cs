using CoC.Backend.Engine.Time;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.StatusEffect
{
	public abstract class TimedStatusEffect : StatusEffectBase
	{
		protected readonly GameDateTime timeObtained;
		protected GameDateTime timeWearsOff;
		public int hoursRemaining => GameDateTime.Now.hoursTo(timeWearsOff);
		protected TimedStatusEffect(SimpleDescriptor name, ushort initialTimeout) : base(name)
		{
			timeWearsOff = GameDateTime.HoursFromNow(initialTimeout);
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


		protected abstract string OnStatusEffectTimePassing(byte hoursPassedSinceLastUpdate, out bool removeEffect);

		protected abstract string OnStatusEffectWoreOff();
	}
}
