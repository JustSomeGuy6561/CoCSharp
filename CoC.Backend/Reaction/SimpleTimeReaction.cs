using CoC.Backend.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Reaction
{
	public abstract class SimpleTimeReaction : TimeReactionBase
	{
		private protected override DisplayBase SpecialEventAsFullPage(bool currentlyIdling, bool hasIdleHours)
		{
			throw new NotSupportedException("A simple special event cannot return as a full page. You should never see this.");
		}

		private protected override bool SpecialEventIsJustText(bool currentlyIdling, bool hasIdleHours)
		{
			return true;
		}

		private protected override string SpecialEventAsText(bool currentlyIdling, bool hasIdleHours)
		{
			return AsTextScene(currentlyIdling, hasIdleHours);
		}

		protected abstract string AsTextScene(bool currentlyIdling, bool hasIdleHours);

	}

	public sealed class GenericSimpleReaction : SimpleTimeReaction
	{
		private readonly SimpleReactionDelegate callback;

		public GenericSimpleReaction(SimpleReactionDelegate simpleEventFunctionCallback)
		{
			callback = simpleEventFunctionCallback ?? throw new ArgumentNullException(nameof(simpleEventFunctionCallback));
		}

		protected override string AsTextScene(bool currentlyIdling, bool hasIdleHours)
		{
			return callback(currentlyIdling, hasIdleHours);
		}
	}
}
