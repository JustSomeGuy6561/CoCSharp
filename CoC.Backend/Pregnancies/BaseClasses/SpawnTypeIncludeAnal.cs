using CoC.Backend.Engine.Time;
using CoC.Backend.Reaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Pregnancies
{
	public abstract class SpawnTypeIncludeAnal : StandardSpawnType
	{
		protected SpawnTypeIncludeAnal(Guid creatureID, Guid spawnTypeID, SimpleDescriptor desc, SimpleDescriptor nameOfFather, ushort birthTime)
			: base(creatureID, spawnTypeID, desc, nameOfFather, birthTime)
		{
		}

		protected internal abstract DynamicTimeReaction HandleAnalBirth(Guid creatureID);

		protected internal abstract string NotifyAnalBirthingProgressed(Guid creatureID, double hoursToBirth, double previousHoursToBirth);

		//does this source ignore the target's anal pregnancy preferences? Note this value is ignored if allowAnalPregnancy is false.

		//by default, anything that may anally impregnate first checks to see what the target's preferences for anal pregnancies are, and fails if they prefer no anal pregnancies.
		//this flag allows the knockup type to try to "force" an anal pregnancy. Note that some creatures may override this default behavior, and allow or deny anal pregnancies,
		//regardless of this setting.
		public virtual bool ignoreAnalPregnancyPreferences => false;
	}
}
