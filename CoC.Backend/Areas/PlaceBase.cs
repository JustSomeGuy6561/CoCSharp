//PlaceBase.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 8:11 PM
using CoC.Backend.Encounters;
using System;
using System.Collections.Generic;
using System.Text;

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
			bool interrupted = false;
			foreach (var interrupt in interrupts)
			{
				if (interrupt.isActive && interrupt.isTriggered())
				{
					interrupted = true;
					interrupt.Run();
					if (interrupt.isCompleted)
					{
						interrupts.Remove(interrupt);
					}
					break;
				}
			}
			if (!interrupted)
			{
				ExplorePlace();
			}
		}

	}
}
