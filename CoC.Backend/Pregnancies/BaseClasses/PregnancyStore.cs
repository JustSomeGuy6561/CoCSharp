//PregnancyStore.cs
//Description:
//Author: JustSomeGuy
//4/7/2019, 7:57 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Engine.Time;
using System;

namespace CoC.Backend.Pregnancies
{
	//need way of checking for eggs - if egg pregnancy, they can be fertalized. 
	public sealed class PregnancyStore : SimpleSaveablePart<PregnancyStore, ReadOnlyPregnancyStore>, ITimeActiveListener, ITimeLazyListener
	{
		public float pregnancyMultiplier
		{
			get
			{
				if (pregnancyMultiplierCounter == 0)
				{
					return 1;
				}
				else if (pregnancyMultiplierCounter > 0)
				{
					return 1 + pregnancyMultiplierCounter / 2.0f;
				}
				else
				{
					return 1 / (1 - pregnancyMultiplierCounter / 2.0f);
				}
			}
		}
		internal int pregnancyMultiplierCounter = 0;

		//remember, we can have eggs even if we have normal womb due to ovi elixirs. 
		private bool? eggSize = null;
		public bool eggSizeKnown => eggSize != null;
		public bool eggsLarge => eggSize == true;

		public SpawnType spawnType { get; private set; }

		public override ReadOnlyPregnancyStore AsReadOnlyData()
		{
			return new ReadOnlyPregnancyStore(spawnType, birthCountdown);
		}

		private readonly bool isVagina;

		public PregnancyStore(Creature source, bool isThisVagina) : base(source)
		{
			isVagina = isThisVagina;
		}

		public ushort birthCountdown => hoursTilBirth <= 0 ? (ushort)0 : (ushort)Math.Ceiling(hoursTilBirth); //unless a pregnancy takes 7.50 years, a ushort is enough lol.

		private float hoursTilBirth; //note that this is passed in as a ushort, but we use float for more accurate pregnancy speed multiplier math, though i suppose this opens us up to floating point rounding errors.

		public bool isPregnant => spawnType != null;

		internal bool attemptKnockUp(float knockupChance, SpawnType type)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
			//if pregnant: set spawnType and birthCountdown;
		}

		internal void SetEggSize(bool isLarge)
		{
			eggSize = isLarge;
		}

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
		EventWrapper ITimeActiveListener.reactToHourPassing()
		{
			//set initial out values so we can return safely. 
			EventWrapper output = EventWrapper.Empty;

			if (isPregnant)
			{
				hoursTilBirth -= pregnancyMultiplier;
				//override them if we are pregnant and giving birth.
				if (hoursTilBirth <= 0)
				{
					output = spawnType.HandleBirth(isVagina);
					spawnType = null; //clear pregnancy.
				}
			}

			return output;
		}

		//active takes care of our hoursTilBirth. we only care about Hours Passed because it lets us figure out what our old timer was. 
		internal EventWrapper reactToTimePassing(byte hoursPassed)
		{
			return lazy.reactToTimePassing(hoursPassed);
		}

		internal EventWrapper reactToHourPassing()
		{
			return active.reactToHourPassing();
		}

		EventWrapper ITimeLazyListener.reactToTimePassing(byte hoursPassed)
		{
			if (isPregnant)
			{
				float oldHours = hoursTilBirth + pregnancyMultiplier * hoursPassed;
				return spawnType.NotifyTimePassed(isVagina, hoursTilBirth, oldHours);
			}
			else
			{
				return EventWrapper.Empty;
			}
		}



		private ITimeActiveListener active => this;
		private ITimeLazyListener lazy => this;

		#endregion
	}

	public sealed class ReadOnlyPregnancyStore
	{
		public readonly SpawnType spawnType;
		public readonly ushort hoursTilBirth;


		public ReadOnlyPregnancyStore(SpawnType spawnType, ushort hoursToBirth)
		{
			this.spawnType = spawnType;
			hoursTilBirth = hoursToBirth;
		}
	}
}

