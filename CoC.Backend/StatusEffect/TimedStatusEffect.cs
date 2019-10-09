using CoC.Backend.Engine.Time;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.StatusEffect
{
	public abstract class TimedStatusEffect : StatusEffectBase, ITimeLazyListener
	{
		protected readonly GameDateTime timeObtained;
		protected GameDateTime timeWearsOff;
		public int hoursRemaining => GameDateTime.Now.hoursTo(timeWearsOff);
		protected TimedStatusEffect(SimpleDescriptor name, ushort initialTimeout) : base(name)
		{
			timeWearsOff = GameDateTime.HoursFromNow(initialTimeout);
		}

		string ITimeLazyListener.reactToTimePassing(byte hoursPassed)
		{
			if (GameDateTime.Now.CompareTo(timeWearsOff) >= 0)
			{
				return OnStatusEffectWoreOff();
			}
			else
			{
				return OnStatusEffectTimePassing(hoursPassed);
			}
		}


		protected abstract string OnStatusEffectTimePassing(byte hoursPassedSinceLastUpdate);

		protected abstract string OnStatusEffectWoreOff();
	}
}
