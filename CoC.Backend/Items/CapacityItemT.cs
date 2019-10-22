using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.UI;

namespace CoC.Backend.Items
{
	public abstract class CapacityItem<T> : CapacityItem where T : CapacityItem<T>
	{
		protected CapacityItem(SimpleDescriptor shortName, SimpleDescriptor fullName, SimpleDescriptor description) : base(shortName, fullName, description)
		{
		}

		public override void AttemptToUse(Creature target, UseItemCallback postItemUseCallback)
		{
			AttemptToUseSafe(target, (x, y, z) => postItemUseCallback(x, y, z));
		}

		//safe variant that ensures T->T when item is used. this allows us to enforce inventories that require a certain Base Item Type. 
		//for example: the weapon rack will always have weapons; you can't pull a fast one and swap in a non-weapon when equipping a weapon off the rack.  
		public virtual void AttemptToUseSafe(Creature target, UseItemCallbackSafe<T> postItemUseCallbackSafe)
		{
			if (!CanUse(target, out string whyNot))
			{
				postItemUseCallbackSafe(false, whyNot, (T)this);
			}

			T retVal = UseItem(target, out string resultsOfUse);

			postItemUseCallbackSafe(true, resultsOfUse, retVal);
		}

		/// <summary>
		/// Do any internal logic for using the item. if this item overrides AttemptToUseSafe, this can be ignored.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="resultsOfUseText">Text explaining the results of the current use. </param>
		/// <returns></returns>
		protected abstract T UseItem(Creature target, out string resultsOfUseText);
	}
}
