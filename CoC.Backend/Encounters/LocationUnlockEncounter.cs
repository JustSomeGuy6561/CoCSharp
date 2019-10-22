using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Areas;
using CoC.Backend.Engine;
using CoC.Backend.UI;

namespace CoC.Backend.Encounters
{
	public sealed class LocationUnlockEncounter : TriggeredEncounter
	{
		private readonly byte unlockTrigger;
		private bool ran;
		private readonly Type locationType;
		private readonly Func<DisplayBase> GetCurrentDisplay;
		public LocationUnlockEncounter(Func<DisplayBase> targetDisplay, Func<LocationBase> constructor)
		{
			if (constructor is null) throw new ArgumentNullException(nameof(constructor));
			LocationBase result = constructor();
			if (result is null)
			{
				throw new ArgumentException("constructor cannot return a null value");
			}
			GetCurrentDisplay = targetDisplay ?? throw new ArgumentNullException(nameof(targetDisplay));

			locationType = result.GetType();
			unlockTrigger = result.unlockLevel;
			ran = false;
		}

		protected internal override bool encounterDisabled()
		{
			return ran;
		}

		protected internal override bool encounterUnlocked()
		{
			return true;
		}

		protected internal override bool isTriggered()
		{
			return GameEngine.currentlyControlledCharacter.level >= unlockTrigger;
		}

		protected internal override void RunEncounter()
		{
			DisplayBase currentDisplay = GetCurrentDisplay();
			if (GameEngine.UnlockArea(locationType, out string unlockText))
			{
				currentDisplay.OutputText(unlockText);
				currentDisplay.DoNext(() => GameEngine.UseHoursGoToBase(1));
			}
			else
			{
				currentDisplay.DoNext(() => GameEngine.ResumeExection());
			}
			ran = true;
		}
	}
}
