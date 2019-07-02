using CoC.Backend.BodyParts;
using CoC.Backend.Engine.Time;
using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Tools;

namespace CoC.Backend.Pregnancies
{
	//need way of checking for eggs - if egg pregnancy, they can be fertalized. 
	public sealed class PregnancyStore : SimpleSaveablePart<PregnancyStore>, ITimeActiveListener, ITimeLazyListener, IBaseStatPerkAware
	{
		private float pregnancySpeed => basePerkStats?.Invoke().pregnancyMultiplier ?? 1f;

		private BasePerkDataGetter basePerkStats;

		//remember, we can have eggs even if we have normal womb due to ovi elixirs. 
		private bool? eggSize = null;
		public bool eggSizeKnown => eggSize != null;
		public bool eggsLarge => eggSize == true;

		public SpawnType spawnType { get; private set; }

		private readonly bool isVagina;

		public PregnancyStore(bool isThisVagina)
		{
			isVagina = isThisVagina;
		}

		public ushort birthCountdown => hoursTilBirth <= 0 ? (ushort) 0 : (ushort)Math.Ceiling(hoursTilBirth); //unless a pregnancy takes 7.50 years, a ushort is enough lol.

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
				hoursTilBirth -= pregnancySpeed;
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
				float oldHours = hoursTilBirth + pregnancySpeed * hoursPassed;
				return spawnType.NotifyTimePassed(isVagina, hoursTilBirth, oldHours);
			}
			else
			{
				return EventWrapper.Empty;
			}
		}

		void IBaseStatPerkAware.GetBasePerkStats(BasePerkDataGetter getter)
		{
			basePerkStats = getter;
		}

		private ITimeActiveListener active => this;
		private ITimeLazyListener lazy => this;

		#endregion
	}
}

