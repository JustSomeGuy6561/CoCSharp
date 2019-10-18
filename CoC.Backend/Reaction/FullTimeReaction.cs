using CoC.Backend.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Reaction
{
	/// <summary>
	/// Full variant of the generic special event, guarenteed to be full
	/// </summary>
	public abstract class FullTimeReaction : TimeReactionBase
	{
		private protected override string SpecialEventAsText(bool currentlyIdling, bool hasIdleHours)
		{
			return null;
		}

		private protected override bool SpecialEventIsJustText(bool currentlyIdling, bool hasIdleHours)
		{
			return false;
		}

		private protected override DisplayBase SpecialEventAsFullPage(bool currentlyIdling, bool hasIdleHours)
		{
			return AsFullPageScene(currentlyIdling, hasIdleHours);
		}

		protected abstract DisplayBase AsFullPageScene(bool currentlyIdling, bool hasIdleHours);
	}
}
