using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Encounters
{
	public abstract class Encounter
	{
		//encounters occur between player and others. so here's a helper!
		protected Player player => GameEngine.currentPlayer;

		internal protected bool isActive => encounterUnlocked();
		internal protected bool isCompleted => encounterDisabled();

		internal protected abstract void Run();

		internal protected abstract bool encounterUnlocked();
		internal protected abstract bool encounterDisabled();



		private protected Encounter() { }
	}

	public abstract class RandomEncounter : Encounter
	{
		internal protected abstract int chances { get; }
		protected RandomEncounter() : base()
		{}
	}

	public abstract class SemiRandomEncounter : Encounter
	{
		internal readonly int numEncountersBeforeRequiringThis;
		internal protected abstract int chances { get; }

		protected SemiRandomEncounter(int maxRunsWithoutProcing) : base()
		{
			numEncountersBeforeRequiringThis = maxRunsWithoutProcing;
		}
	}

	public abstract class TriggeredEncounter : Encounter
	{
		internal protected abstract bool isTriggered();


	}

}
