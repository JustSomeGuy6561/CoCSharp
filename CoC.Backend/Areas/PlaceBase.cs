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

		protected abstract void ExplorePlace();

		protected readonly HashSet<TriggeredEncounter> interrupts = new HashSet<TriggeredEncounter>();


		public PlaceBase(SimpleDescriptor placeName, HashSet<TriggeredEncounter> optionalInterrupts = null) : base(placeName)
		{
			if (optionalInterrupts != null)
			{
				interrupts.UnionWith(optionalInterrupts);
			}
		}

		internal override void RunArea()
		{
			LoadPlace();
		}

		protected virtual void LoadPlace()
		{
			foreach (var interrupt in interrupts)
			{
				if (interrupt.isActive && interrupt.isTriggered())
				{
					interrupt.RunEncounter();
					if (interrupt.isCompleted)
					{
						interrupts.Remove(interrupt);
					}
				}
			}
			//we didnt hit an interrupt, so we're fine to display normally. 
			ExplorePlace();
		}

	}
}
