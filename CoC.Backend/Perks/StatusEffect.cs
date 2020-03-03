using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Engine.Time;

namespace CoC.Backend.Perks
{
	using StatusEffect = TimedPerk;

	//A Timed Perk (or, for a legacy perspective, a StatusEffect) is like a standard perk, but it automatically deactivates after a period of time has passed.
	//Beyond this, a timed perk has no further limitations - it may be possible to stack a timed perk indefinitely if you feel the need to do so.

	//Note that there is no fundamental difference between a Status Effect and a Perk now, aside from how they are removed.
	public abstract class TimedPerk : PerkBase
	{
		private readonly List<IRemovableModifier> currentlyActiveModifiers = new List<IRemovableModifier>();

		protected GameDateTime timeWearsOff;

		public int hoursRemaining => GameDateTime.Now.HoursTo(timeWearsOff);

		protected TimedPerk(ushort initialTimeout) : base()
		{
			timeWearsOff = GameDateTime.HoursFromNow(initialTimeout);
		}

		private protected override void OnCreate()
		{
			OnActivation();
		}

		private protected override void OnDestroy()
		{
			OnRemoval();
		}

		private protected override bool AddActiveModifier<T>(PerkModifierBase<T> modifier, T value, bool overwriteExisting = false)
		{
			if (modifier.AddModifier(this, value, overwriteExisting))
			{
				currentlyActiveModifiers.Add(modifier);
				return true;
			}

			return false;
		}

		private protected override bool RemoveActiveModifier<T>(PerkModifierBase<T> modifier)
		{
			currentlyActiveModifiers.Remove(modifier);
			return modifier.RemoveModifier(this);
		}

		private protected override bool HasActiveModifier<T>(PerkModifierBase<T> modifier)
		{
			bool retVal = modifier.HasModifier(this);
			if (!retVal && currentlyActiveModifiers.Contains(modifier))
			{
				currentlyActiveModifiers.Remove(modifier);
			}
			else if (retVal && !currentlyActiveModifiers.Contains(modifier))
			{
				currentlyActiveModifiers.Add(modifier);
			}

			return retVal;
		}

		internal string ReactToTimePassing(byte hoursPassed, out bool removeEffect)
		{
			string text = null;
			if (GameDateTime.Now.CompareTo(timeWearsOff) >= 0)
			{
				text = OnStatusEffectWoreOff(hoursPassed);
				removeEffect = true;
			}
			else
			{
				text = OnStatusEffectTimePassing(hoursPassed, out removeEffect);
			}
			return text;
		}


		//called when the status effect is added to the status effect collection on the character. This is responsible for setting any internal values necessary to
		//make the status effect work properly.
		protected abstract void OnActivation();

		//The default text to display when this timed perk/status effect is obtained. This is expected to be called immediately after adding this to the creature,
		//and any other use is not defined and may produce incorrect text (or even untranslated text if that's a thing ever) or even error.
		//Note that it's possible that this will never be called, as it may simply be ignored, or the content creator may roll out their own, custom text.
		//If you have a default text to display, override this. if not, leave it alone.
		public virtual string ObtainText() => "";

		//The following deal with time passing on the timed effect - if the amount of time passing exceeds the remaining time left on the effect,
		//OnStatusEffectWoreOff is called. otherwise, OnStatusEffectTimePassing is called.

		//update the status effect values as time passes, then return any text that is generated as a result of doing so. Because it's possible some time passing
		//and other factors may cause the perk to be removed, you are also required to set a flag noting if the effect should be removed.
		//It's recommended to set this removeEffect flag to false on the first line of the function, then change it to true if needed later.

		//By default, we assume nothing happens when time passes, and you have no text to display. if either are incorrect, override this.

		//Note: This function is for both updating and displaying text - if you need to adjust creature and/or internal values before displaying text, do so.
		protected virtual string OnStatusEffectTimePassing(byte hoursPassedSinceLastUpdate, out bool removeEffect)
		{
			removeEffect = false;
			return null;
		}

		//Update the status effect values after enough time has passed that the effect should be removed. note that this works in tandem with the time passing value.
		//Also note that OnRemoval will be called immediately after this is removed, HOWEVER, this function is for both updating and displaying text, so feel free to
		//change any values you need to make this happen.
		protected abstract string OnStatusEffectWoreOff(byte hoursPassedSinceLastUpdate);


		//Update the status effect and creature values necessary before removing the perk completely, and otherwise prepare for cleanup. Note that it may be possible
		//for a content creator to manually remove a status effect, so do not assume OnStatusEffectWoreOff or OnStatusEffectTimePassing has been called before this is.
		//It may be useful to have a 'cleaned up' flag which WoreOff or TimePassing can set so on remove doesn't do double work.
		protected abstract void OnRemoval();



		private protected override bool retainOnAscension => false;

		private protected override bool enabled => !(sourceCreature is null);
	}
}
