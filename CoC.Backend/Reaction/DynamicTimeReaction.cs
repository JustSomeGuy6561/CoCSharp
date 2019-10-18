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
	public abstract class DynamicTimeReaction : TimeReactionBase
	{
		private protected override DisplayBase SpecialEventAsFullPage(bool currentlyIdling, bool hasIdleHours)
		{
			return AsFullPageScene(currentlyIdling, hasIdleHours);
		}

		private protected override string SpecialEventAsText(bool currentlyIdling, bool hasIdleHours)
		{
			return AsTextScene(currentlyIdling, hasIdleHours);
		}

		private protected override bool SpecialEventIsJustText(bool currentlyIdling, bool hasIdleHours)
		{
			return IsJustTextScene(currentlyIdling, hasIdleHours);
		}

		protected abstract bool IsJustTextScene(bool currentlyIdling, bool hasIdleHours);

		protected abstract DisplayBase AsFullPageScene(bool currentlyIdling, bool hasIdleHours);
	
		protected abstract string AsTextScene(bool currentlyIdling, bool hasIdleHours);
	}
}
