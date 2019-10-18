//SpecialEvent.cs
//Description:
//Author: JustSomeGuy
//6/29/2019, 11:55 PM
using CoC.Backend.UI;
using System;

namespace CoC.Backend.Reaction
{
	/// <summary>
	/// Special Time reaction that may require a full page, or simply be some text, depending on the desired results and the current context. Unfortunately, it's impossible
	/// to have an either-or return type, so this class requires both be at least in theory supported. 
	/// </summary>
	public abstract class TimeReactionBase
	{
		//if the scene built by this function 

		/// <summary>
		/// Runs the special event. queries the type of event it is, and returns accordingly. resume callback is assumed not to be called unless needed.
		/// </summary>
		/// <param name="currentlyIdling"></param>
		/// <param name="hasIdleHours"></param>
		/// <returns></returns>
		internal DisplayWrapper RunEvent(bool currentlyIdling, bool hasIdleHours)
		{
			if (SpecialEventIsJustText(currentlyIdling, hasIdleHours))
			{
				return new DisplayWrapper(SpecialEventAsText(currentlyIdling, hasIdleHours));
			}
			else
			{
				return new DisplayWrapper(SpecialEventAsFullPage(currentlyIdling, hasIdleHours));
			}
		}

		/// <summary>
		/// Determines if the special event will return a full scene or merely some text. 
		/// </summary>
		/// <param name="currentlyIdling"></param>
		/// <param name="hasIdleHours"></param>
		/// <returns></returns>
		private protected abstract bool SpecialEventIsJustText(bool currentlyIdling, bool hasIdleHours);
		private protected abstract string SpecialEventAsText(bool currentlyIdling, bool hasIdleHours);
		private protected abstract DisplayBase SpecialEventAsFullPage(bool currentlyIdling, bool hasIdleHours);
	}
}
