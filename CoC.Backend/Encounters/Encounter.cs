﻿//Encounter.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 10:22 PM
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.UI;

namespace CoC.Backend.Encounters
{
	public abstract class Encounter
	{
		//encounters occur between player and others. so here's a helper!
		protected PlayerBase player => GameEngine.currentlyControlledCharacter;

		public bool isActive => EncounterUnlocked();
		public bool isCompleted => EncounterDisabled();

		internal protected abstract void RunEncounter();

		internal protected abstract bool EncounterUnlocked();
		internal protected abstract bool EncounterDisabled();



		private protected Encounter() { }
	}

	public abstract class RandomEncounter : Encounter
	{
		internal protected abstract int chances { get; }
		protected RandomEncounter() : base()
		{ }
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
