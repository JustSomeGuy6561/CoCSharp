using CoC.Backend;
using CoC.Backend.Creatures;
using CoC.Backend.Engine.Time;
using CoC.Backend.Items;
using System;
using System.Collections.Generic;
using System.Text;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Engine.Time
{
	class GainItemEventWrapper : SpecialEvent
	{
		private readonly Creature target;
		private readonly CapacityItem capacityItem;
		private readonly SimpleDescriptor preText;
		public GainItemEventWrapper(Creature source, CapacityItem item, SimpleDescriptor gainItemContext = null)
		{
			capacityItem = item ?? throw new ArgumentNullException(nameof(item));
			target = source ?? throw new ArgumentNullException(nameof(source));
			preText = gainItemContext;
		}

		protected override void BuildInitialScene(bool currentlyIdling, bool hasIdleHours, Action callMeWhenYourSceneIsDone)
		{
			if (preText != null)
			{
				OutputText(preText());
			}
			target.AddStandardItem(capacityItem, callMeWhenYourSceneIsDone);
		}
	}
}
