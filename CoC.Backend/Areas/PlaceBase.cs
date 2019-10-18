//PlaceBase.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 8:11 PM
using CoC.Backend.Encounters;
using CoC.Backend.Engine;
using CoC.Backend.UI;
using System.Collections.Generic;

namespace CoC.Backend.Areas
{
	public abstract class PlaceBase : VisitableAreaBase
	{
		public abstract bool isDisabled { get; protected set; }

		protected abstract void ExplorePlace(DisplayBase display);

		protected readonly HashSet<TriggeredEncounter> interrupts = new HashSet<TriggeredEncounter>();


		public PlaceBase(SimpleDescriptor placeName, HashSet<TriggeredEncounter> optionalInterrupts = null) : base(placeName)
		{
			if (optionalInterrupts != null)
			{
				interrupts.UnionWith(optionalInterrupts);
			}
		}

		internal override DisplayBase RunArea()
		{
			return LoadPlace();
		}

		protected virtual DisplayBase LoadPlace()
		{
			DisplayBase display = pageMaker(); //if overriding this, use the implementation available (i.e. the frontend can just say new StandardDisplay() or whatever the implementer is called)
			foreach (var interrupt in interrupts)
			{
				if (interrupt.isActive && interrupt.isTriggered())
				{
					interrupt.Run(display);
					if (interrupt.isCompleted)
					{
						interrupts.Remove(interrupt);
					}
					return display;
				}
			}
			//we didnt hit an interrupt, so we're fine to display normally. 
			ExplorePlace(display);
			return display;
		}

	}
}
