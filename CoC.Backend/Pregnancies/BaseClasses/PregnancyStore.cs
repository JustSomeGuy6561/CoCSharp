using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Pregnancies
{
	//need way of checking for eggs - if egg pregnancy, they can be fertalized. 
	public sealed class PregnancyStore //itimeaware
	{
		public SpawnType spawnType { get; private set; }
		public int birthCountdown { get; private set; }

		public bool isPregnant => spawnType != null;

		internal bool attemptKnockUp(float knockupChance, SpawnType type)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		internal void Reset()
		{
			spawnType = null;
			birthCountdown = 0;
		}

		internal bool Validate(bool correctInvalidData = false)
		{
			if (spawnType != null || birthCountdown == 0)
			{
				return true;
			}
			else if (correctInvalidData)
			{
				birthCountdown = 0;
			}
			return false;
		}
	}
}
