using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Pregnancies
{
	public sealed class BirthEvent : EventArgs 
	{
		public readonly Guid creatureID;
		public readonly ReadOnlyPregnancyStore birthSource;
		public readonly StandardSpawnData spawnData;
		public readonly uint totalBirthCount;

		public BirthEvent(Guid creatureID, ReadOnlyPregnancyStore source, StandardSpawnData spawn, uint totalBirthCount)
		{
			this.creatureID = creatureID;
			birthSource = source;
			spawnData = spawn;
			this.totalBirthCount = totalBirthCount;
		}
	}
}
