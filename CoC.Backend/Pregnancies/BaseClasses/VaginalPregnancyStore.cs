using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Engine;
using CoC.Backend.UI;
using CoC.Backend.Engine.Time;
using CoC.Backend.Reaction;

namespace CoC.Backend.Pregnancies
{
	public sealed class VaginalPregnancyStore : PregnancyStore
	{
		public readonly byte vaginaIndex;
		public VaginalPregnancyStore(Guid creatureID, byte vaginaIndex) : base(creatureID)
		{
			this.vaginaIndex = vaginaIndex;
		}

		protected override DynamicTimeReaction HandleBirthing()
		{
			DynamicTimeReaction retVal = spawnType.HandleVaginalBirth(vaginaIndex);
			CreatureStore.GetCreatureClean(creatureID)?.genitals.vaginas[vaginaIndex].HandleBirth(spawnType.sizeOfCreatureAtBirth);
			return retVal;
		}

		protected override string NotifyTimePassed(float hoursTilBirth, float oldHoursToBirth)
		{
			return spawnType.NotifyVaginalBirthingProgressed(vaginaIndex, hoursTilBirth, oldHoursToBirth);
		}
	}
}
