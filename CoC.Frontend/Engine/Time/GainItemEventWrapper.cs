using CoC.Backend;
using CoC.Backend.Creatures;
using CoC.Backend.Engine.Time;
using CoC.Backend.Items;
using CoC.Frontend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Engine.Time
{
	public static class GainItemEventHelper
	{
		public static EventWrapper GainItemWrapper(Creature source, CapacityItem item, SimpleDescriptor gainItemContext = null)
		{
			if (source.TryAddItem(item) != -1)
			{
				return new EventWrapper(gainItemContext?.Invoke());
			}
			else
			{
				return new EventWrapper(new ItemFullSpecialEvent(source, item, gainItemContext));
			}
			
		}

		public static EventWrapper GainItemWrapper(Creature source, CapacityItem item, Action doSceneRelatedStuff)
		{
			return new EventWrapper(GainItemEvent(source, item, doSceneRelatedStuff));
		}

		public static SpecialEvent GainItemEvent(Creature source, CapacityItem item, Action doSceneRelatedStuff)
		{
			if (source.TryAddItem(item) != -1)
			{
				return new SimpleSpecialEvent(doSceneRelatedStuff);
			}
			else
			{
				return new ItemFullSpecialEvent(source, item, doSceneRelatedStuff);
			}
		}

		
	}

	class SimpleSpecialEvent : SpecialEvent
	{
		private readonly Action doMeForScene;

		public SimpleSpecialEvent(Action sceneCallback)
		{
			this.doMeForScene = sceneCallback;
		}

		protected override void BuildInitialScene(bool currentlyIdling, bool hasIdleHours, Action callMeWhenYourSceneIsDone)
		{
			doMeForScene();
			callMeWhenYourSceneIsDone();
		}
	}

	class ItemFullSpecialEvent : SpecialEvent
	{
		public ItemFullSpecialEvent(Creature source, CapacityItem item, SimpleDescriptor gainItemContext)
		{
			capacityItem = item ?? throw new ArgumentNullException(nameof(item));
			target = source ?? throw new ArgumentNullException(nameof(source));
			preItemCallback = () => OutputText(gainItemContext?.Invoke());
		}

		public ItemFullSpecialEvent(Creature source, CapacityItem item, Action gainItemContextEvent)
		{
			capacityItem = item ?? throw new ArgumentNullException(nameof(item));
			target = source ?? throw new ArgumentNullException(nameof(source));
			preItemCallback = gainItemContextEvent;
		}

		private readonly Creature target;
		private readonly CapacityItem capacityItem;
		private readonly Action preItemCallback;

		protected override void BuildInitialScene(bool currentlyIdling, bool hasIdleHours, Action callMeWhenYourSceneIsDone)
		{
			preItemCallback?.Invoke();
			target.AddItem(capacityItem, callMeWhenYourSceneIsDone);
		}
	}
}
