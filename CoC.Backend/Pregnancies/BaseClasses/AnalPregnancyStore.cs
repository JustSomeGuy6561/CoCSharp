﻿using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Engine;
using CoC.Backend.UI;
using CoC.Backend.Engine.Time;
using CoC.Backend.Reaction;

namespace CoC.Backend.Pregnancies
{
	public sealed class AnalPregnancyStore : PregnancyStore
	{
		public AnalPregnancyStore(Guid creatureID) : base(creatureID)
		{
		}

		protected override DynamicTimeReaction HandleBirthing()
		{
			DynamicTimeReaction retVal = ((SpawnTypeIncludeAnal)spawnType).HandleAnalBirth(creatureID);
			CreatureStore.GetCreatureClean(creatureID)?.genitals.ass.HandleBirth(spawnType.sizeOfCreatureAtBirth);
			return retVal;
		}

		protected override string NotifyTimePassed(double hoursTilBirth, double oldHoursToBirth)
		{
			return ((SpawnTypeIncludeAnal)spawnType).NotifyAnalBirthingProgressed(creatureID, hoursTilBirth, oldHoursToBirth);
		}

		protected override string DiapauseText()
		{
			return GenericDiapauseText();
		}

		internal override bool attemptKnockUp(double knockupChance, StandardSpawnType type)
		{
			if (type is SpawnTypeIncludeAnal)
			{
				return base.attemptKnockUp(knockupChance, type);
			}
			else
			{
				return false;
			}
		}
	}
}
