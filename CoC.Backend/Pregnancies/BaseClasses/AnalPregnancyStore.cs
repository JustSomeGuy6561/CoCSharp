using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;

namespace CoC.Backend.Pregnancies
{
	public sealed class AnalPregnancyStore : PregnancyStore
	{
		public AnalPregnancyStore(Guid creatureID) : base(creatureID)
		{
		}

		protected override SpecialEvent HandleBirthing()
		{
			SpecialEvent retVal = ((SpawnTypeIncludeAnal)spawnType).HandleAnalBirth(creatureID);
			CreatureStore.GetCreatureClean(creatureID)?.genitals.ass.HandleBirth(spawnType.sizeOfCreatureAtBirth);
			return retVal;
		}

		protected override string NotifyTimePassed(float hoursTilBirth, float oldHoursToBirth)
		{
			return ((SpawnTypeIncludeAnal)spawnType).NotifyAnalBirthingProgressed(creatureID, hoursTilBirth, oldHoursToBirth);
		}
	}
}
