//PregnancyStore.cs
//Description:
//Author: JustSomeGuy
//4/7/2019, 7:57 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Tools;
using System;
using CoC.Backend.Reaction;

namespace CoC.Backend.Pregnancies
{
	//need way of checking for eggs - if egg pregnancy, they can be fertalized. 
	public abstract class PregnancyStore : SimpleSaveablePart<PregnancyStore, ReadOnlyPregnancyStore>, ITimeActiveListenerFull, ITimeLazyListener
	{
		public override string BodyPartName()
		{
			return Womb.Name(); //idk. really, this should implement saveable, not body part saveable. 
		}

		private Womb source => CreatureStore.GetCreatureClean(creatureID)?.womb;

		public float pregnancyMultiplier => source?.pregnancyMultiplier ?? 1.0f;

		public uint birthCount { get; private set; } = 0;
		public uint totalBirthCount => CreatureStore.GetCreatureClean(creatureID)?.womb.totalBirthCount ?? birthCount;

		//remember, we can have eggs even if we have normal womb due to ovi elixirs. 
		private bool? eggSize = null;
		public bool eggSizeKnown => eggSize != null;
		public bool eggsLarge => eggSize == true;

		public StandardSpawnType spawnType { get; private set; }

		public override ReadOnlyPregnancyStore AsReadOnlyData()
		{
			return new ReadOnlyPregnancyStore(creatureID, spawnType, birthCountdown);
		}


		private protected PregnancyStore(Guid creatureID) : base(creatureID)
		{
		}

		public ushort birthCountdown => hoursTilBirth <= 0 ? (ushort)0 : (ushort)Math.Ceiling(hoursTilBirth); //unless a pregnancy takes 7.50 years, a ushort is enough lol.

		private float hoursTilBirth; //note that this is passed in as a ushort, but we use float for more accurate pregnancy speed multiplier math, though i suppose this opens us up to floating point rounding errors.

		public bool isPregnant => spawnType != null;

		internal bool attemptKnockUp(float knockupChance, StandardSpawnType type)
		{
			if (knockupChance < 0 || type is null)
			{
				return false;
			}
			else if (this.spawnType != null)
			{
				if (spawnType.HandleNewKnockupAttempt(type, out StandardSpawnType newType)) 
				{
					spawnType = newType;
					return true;
				}
				return false;
			}
			else if (knockupChance > 1 || Utils.Rand(1000) <= Math.Round(knockupChance * 1000))
			{
				spawnType = type;
				hoursTilBirth = type.hoursToBirth;
				return true;
			}
			return false;
			//if pregnant: set spawnType and birthCountdown;
		}

		//sets the egg size for all future egg pregnancies in this womb.
		internal void SetEggSize(bool isLarge)
		{
			eggSize = isLarge;
		}

		//clears any set egg size that would otherwise affect all future egg pregnancies in this womb.
		internal void ClearEggSize()
		{
			eggSize = null;
		}

		internal void Reset(bool clearEggSize = false)
		{
			spawnType = null;
			hoursTilBirth = 0;
			if (clearEggSize)
			{
				eggSize = null;
			}
		}

		internal override bool Validate(bool correctInvalidData)
		{
			if (spawnType != null || birthCountdown == 0)
			{
				return true;
			}
			else if (correctInvalidData)
			{
				hoursTilBirth = 0;
			}
			return false;
		}

		#region ITimeListener
		TimeReactionBase ITimeActiveListenerFull.reactToHourPassing()
		{
			//set initial out values so we can return safely. 
			TimeReactionBase output = null;

			if (isPregnant)
			{
				hoursTilBirth -= pregnancyMultiplier;
				//override them if we are pregnant and giving birth.
				if (hoursTilBirth <= 0)
				{
					output = DoBirth();
				}
			}

			return output;
		}

		//in the rare event time passing causes premature birthing, you can do it here. 
		protected DynamicTimeReaction DoBirth()
		{
			var output = HandleBirthing();
			spawnType = null; //clear pregnancy.
			hoursTilBirth = 0;
			birthCount++;
			source?.RaiseBirthEvent(spawnType, this);
			return output;
		}

		protected abstract DynamicTimeReaction HandleBirthing();

		string ITimeLazyListener.reactToTimePassing(byte hoursPassed)
		{
			if (isPregnant)
			{
				float oldHours = hoursTilBirth + pregnancyMultiplier * hoursPassed;
				return NotifyTimePassed(hoursTilBirth, oldHours);
			}
			else
			{
				return null;
			}
		}

		protected abstract string NotifyTimePassed(float hoursTilBirth, float oldHoursToBirth);

		#endregion
	}

	public sealed class ReadOnlyPregnancyStore : SimpleData
	{
		public readonly StandardSpawnType spawnType;
		public readonly ushort hoursTilBirth;


		public ReadOnlyPregnancyStore(Guid creatureID, StandardSpawnType spawnType, ushort hoursToBirth) : base(creatureID)
		{
			this.spawnType = spawnType;
			hoursTilBirth = hoursToBirth;
		}
	}
}

