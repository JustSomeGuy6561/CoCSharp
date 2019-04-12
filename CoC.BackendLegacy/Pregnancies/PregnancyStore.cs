using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Pregnancies
{
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
	}
}
