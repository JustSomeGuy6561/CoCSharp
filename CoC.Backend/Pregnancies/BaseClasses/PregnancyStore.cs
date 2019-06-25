using CoC.Backend.BodyParts;
using CoC.Backend.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Pregnancies
{
	//need way of checking for eggs - if egg pregnancy, they can be fertalized. 
	public sealed class PregnancyStore : SimpleSaveablePart<PregnancyStore>, ITimeListenerWithOutput //, IPerkAware for perks 
	{
#warning TODO: fix me to use perks when available.
		private float pregnancySpeed => 1f;
		public SpawnType spawnType { get; private set; }
		public ushort birthCountdown => (ushort)Math.Round(hoursTilBirth); //unless a pregnancy takes 7.50 years, a ushort is enough lol.

		private float hoursTilBirth; //note that this is passed in as a ushort, but we use float for more accurate pregnancy speed multiplier math. 

		public bool isPregnant => spawnType != null;

		internal bool attemptKnockUp(float knockupChance, SpawnType type)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
			//if pregnant: set spawnType and birthCountdown;
		}

		internal void Reset()
		{
			spawnType = null;
			hoursTilBirth = 0;
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

		#region ITimeAware
		void ITimeListener.ReactToTimePassing(byte hoursPassed)
		{
			needsOutput = false;
			outputText = "";

			if (isPregnant)
			{
				hoursTilBirth -= hoursPassed * pregnancySpeed;
				if (hoursTilBirth <= 0)
				{
					needsOutput = true;
					outputText = spawnType.HandleBirth();
				}
			}
		}

		bool ITimeListenerWithOutput.RequiresOutput => needsOutput;

		string ITimeListenerWithOutput.Output()
		{
			return outputText;
		}

		private bool needsOutput = false;
		private string outputText = "";
		#endregion
	}
}
