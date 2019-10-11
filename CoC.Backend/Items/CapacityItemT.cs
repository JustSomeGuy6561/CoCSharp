using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items
{
	public abstract class CapacityItem<T> : CapacityItem where T : CapacityItem
	{
		protected CapacityItem(SimpleDescriptor shortName, SimpleDescriptor fullName, SimpleDescriptor description) : base(shortName, fullName, description)
		{
		}

		public override void AttemptToUse(Creature target, UseItemCallback postItemUseCallback)
		{
			if (!CanUse(target))
			{
				postItemUseCallback(false, CantUseExplanation(target), null);
			}

			T retVal = UseItem(target, out string resultsOfUse);
			postItemUseCallback(true, resultsOfUse, retVal);
		}

		//safe variant that ensures T->T when item is used. this allows us to enforce inventories that require a certain Base Item Type. 
		//for example: the weapon rack will always have weapons; you can't pull a fast one and swap in a non-weapon when equipping a weapon off the rack.  
		public virtual void AttemptToUseSafe(Creature target, UseItemCallbackSafe<T> postItemUseCallbackSafe)
		{
			if (!CanUse(target))
			{
				postItemUseCallbackSafe(false, CantUseExplanation(target), null);
			}

			T retVal = UseItem(target, out string resultsText);
			postItemUseCallbackSafe(true, resultsText, retVal);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="target"></param>
		/// <param name="resultsOfUseText">Text explaining the results of the current use. </param>
		/// <returns></returns>
		protected abstract T UseItem(Creature target, out string resultsOfUseText);
	}
}
