using CoC.Backend.BodyParts;
using CoC.Backend.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.BodyParts.SpecialInteraction;
namespace CoC.Backend.Pregnancies
{
	//need way of checking for eggs - if egg pregnancy, they can be fertalized. 
	public sealed class PregnancyStore : SimpleSaveablePart<PregnancyStore>, ITimeActiveListener  //, IPerkAware for perks 
	{
#warning TODO: fix me to use perks when available.
		private float pregnancySpeed => 1f;

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

		public ushort birthCountdown => hoursTilBirth <= 0 ? (ushort) 0 : (ushort)Math.Round(hoursTilBirth); //unless a pregnancy takes 7.50 years, a ushort is enough lol.

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
		void ITimeListener.ReactToTimePassing(byte hoursPassed)
		{
			needsOutput = false;
			outputBuilder.Clear();

			if (isPregnant)
			{
				ushort oldHours = birthCountdown;
				hoursTilBirth -= hoursPassed * pregnancySpeed;
				if (hoursTilBirth <= 0)
				{
					spawnType.HandleBirth(isVagina);
					if (spawnType.birthRequiresOutput)
					{
						needsOutput = true;
						outputBuilder.Append(spawnType.BirthText());
					}
				}
				else
				{
					spawnType.NotifyTimePassed(isVagina, birthCountdown, oldHours);
					if (spawnType.NeedsOutputDueToTimePassed)
					{
						needsOutput = true;
						outputBuilder.Append(spawnType.TimePassedText());
					}
				}
			}
		}

		bool ITimeListenerWithShortOutput.RequiresOutput => needsOutput;

		string ITimeListenerWithShortOutput.Output()
		{
			return outputBuilder.ToString();
		}

		#endregion
	}
}
