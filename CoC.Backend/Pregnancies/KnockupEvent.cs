using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Pregnancies
{
	public sealed class KnockupEvent : EventArgs
	{
		public readonly Guid creatureID;
		public readonly ReadOnlyPregnancyStore birthSource;
		//will be null if the creature was not already pregnant. 
		//only null if the given spawn type can be updated when another knockup attempt is called (i.e. eggs).
		public readonly StandardSpawnData originalSpawnSource;
		public readonly StandardSpawnData currentSpawnSource;

		public KnockupEvent(Guid creatureID, ReadOnlyPregnancyStore birthSource)
		{
			this.creatureID = creatureID;
			this.birthSource = birthSource ?? throw new ArgumentNullException(nameof(birthSource));
			originalSpawnSource = null;
			this.currentSpawnSource = birthSource.spawnType;
		}

		public KnockupEvent(Guid creatureID, ReadOnlyPregnancyStore birthSource, StandardSpawnData originalSpawnData)
		{
			this.creatureID = creatureID;
			this.birthSource = birthSource ?? throw new ArgumentNullException(nameof(birthSource));
			originalSpawnSource = originalSpawnData ?? throw new ArgumentNullException(nameof(originalSpawnData));
			this.currentSpawnSource = birthSource.spawnType;
		}
	}
}
